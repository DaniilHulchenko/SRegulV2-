using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FMotif : Form
    {
        public static DataTable dtMotif = new DataTable();
        private static string Etat = "Lecture";      
        
        public FMotif()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("Id Motif", 60);       
            listView1.Columns.Add("Urgence", 80);
            listView1.Columns.Add("Libelle", 200);
            listView1.Columns.Add("Type", 20);
            listView1.View = View.Details;    //Pour afficher les subItems  

            //On vide les champs et initalise les boutons
            VideChamps();
        }

        private void FMotif_Load(object sender, EventArgs e)
        {
            dtMotif.Clear();
            dtMotif = FonctionsAppels.ChargeListeMotif();

            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtMotif.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtMotif.Rows[i]["IdMotif"].ToString());
                item.SubItems.Add(dtMotif.Rows[i]["Urgence"].ToString());
                item.SubItems.Add(dtMotif.Rows[i]["LibelleMotif"].ToString());
                item.SubItems.Add(dtMotif.Rows[i]["TypeMotif"].ToString());

                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle        
        }

        private void VideChamps()
        {
            tBIdMotif.Text = "";
            tBUrgence.Text = "";
            tBLibelle.Text = "";
            rBVisite.Checked = true;

            //Pour les boutons
            bAjouter.ImageIndex = 0;
            bAjouter.Visible = true;

            bValider.ImageIndex = 4;
            bValider.Enabled = false;

            bSupprimer.ImageIndex = 6;
            bSupprimer.Enabled = false;

            bCancel.Visible = false;

            bExit.ImageIndex = 7;
        }


        //Selection d'un motif dans la liste
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            //On peuple les champs            
            tBIdMotif.Text = dtMotif.Rows[index]["IdMotif"].ToString();
            tBUrgence.Text = dtMotif.Rows[index]["Urgence"].ToString();
            tBLibelle.Text = dtMotif.Rows[index]["LibelleMotif"].ToString();

            if (dtMotif.Rows[index]["TypeMotif"].ToString() == "V")
                rBVisite.Checked = true;
            else
                rBAnnulation.Checked = true;

            //Gestion des boutons: On passe en modif
            Etat = "Modif";

            tBIdMotif.ReadOnly = true;  //On met en lecture seule tBIdMotif
            
            bAjouter.Visible = false;
            bValider.Enabled = true;
            bCancel.Visible = true;
            bSupprimer.Enabled = true;
        }


        //***Gestion des boutons
        private void bAjouter_Click(object sender, EventArgs e)
        {
            //Gestion des boutons: On passe en Ajout
            Etat = "Ajout";

            VideChamps();

            //On propose un IdMotif
            tBIdMotif.Text = FonctionsAppels.NvxIdMotif();

            tBIdMotif.ReadOnly = true;

            bAjouter.Visible = false;
            bValider.Enabled = true;
            bCancel.Visible = true;
            bSupprimer.Enabled = false;
        }


        //On valide en fonction de l'état
        private void bValider_Click(object sender, EventArgs e)
        {
            if (VerifSaisie() == "OK")
            {
                if (Etat == "Ajout" && VerifID(tBIdMotif.Text) == "OK")
                {                                        
                    if (AjoutEnreg() == "OK")
                    {
                        VideChamps();

                        //Gestion des boutons
                        bAjouter.Visible = true;
                        bValider.Enabled = false;
                        bCancel.Visible = false;
                        bSupprimer.Enabled = false;

                        Etat = "Lecture";

                        //On rafraichi la liste des Motifs
                        FMotif_Load(sender, e);
                    }
                    else
                    {
                        //Gestion des boutons
                        bAjouter.Visible = false;
                        bValider.Enabled = true;
                        bCancel.Visible = true;
                        bSupprimer.Enabled = false;
                    }
                }
                else if (Etat == "Modif")
                {
                    if (UpdateEnreg() == "OK")
                    {
                        VideChamps();
                        //Gestion des boutons
                        bAjouter.Visible = true;
                        bValider.Enabled = false;
                        bCancel.Visible = false;
                        bSupprimer.Enabled = false;

                        Etat = "Lecture";

                        //On rafraichi la liste des motifs
                        FMotif_Load(sender, e);
                    }
                    else
                    {
                        //Gestion des boutons
                        bAjouter.Visible = false;
                        bValider.Enabled = true;
                        bCancel.Visible = true;
                        bSupprimer.Enabled = false;
                    }
                }
            }
        }


        //vérification des saisies
        private string VerifSaisie()
        {
            string retour = "KO";
            int V1 = 0;

            if (tBIdMotif.Text != "")
            {
                if (Etat == "Ajout")
                {
                    if (VerifID(tBIdMotif.Text) == "OK")
                        V1 += 1;
                    else MessageBox.Show("Cet'Id est déjà utilisé, pière d'en mettre un autre. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    V1 += 1;
            }
            else
                MessageBox.Show("Vous devez renseigner l'Id de ce motif. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);


            if (tBUrgence.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner le niveau d'urgence de ce motif. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (tBLibelle.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner le libellé de ce motif. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
         

            //En fonction des résultats
            switch (V1)
            {
                case 3: retour = "OK"; break;
                default: retour = "KO"; break;
            }

            return retour;
        }


        //On ajoute un nvl enregistrement 
        public string AjoutEnreg()
        {
            string Retour = "KO";

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            //On déclare ici la requete (à cause de la transaction)
            string SqlStr0 = "";

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Def de la requete
                //*****Commune***
                SqlStr0 = "INSERT INTO motif ";
                SqlStr0 += " (IdMotif, LibelleMotif, Urgence, TypeMotif)";
                SqlStr0 += " VALUES('" + tBIdMotif.Text + "','" + tBLibelle.Text + "','" + tBUrgence.Text;

                if (rBVisite.Checked)
                            SqlStr0 += "', 'V')";
                else SqlStr0 += "', 'A')";

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();

                    Retour = "OK";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la création du Motif (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    //fermeture des connexions
                    if (dbConnection.State == System.Data.ConnectionState.Open)
                    {
                        dbConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la création du Motif. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return Retour;
        }


        private string VerifID(string IdMotif)
        {
            string retour = "KO";

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string SqlStr0 = "SELECT COUNT(*) FROM motif WHERE IdMotif = @IdMotif";
                cmd.CommandText = SqlStr0;

                cmd.Parameters.AddWithValue("IdMotif", IdMotif);

                DataTable dtMotif = new DataTable();
                dtMotif.Load(cmd.ExecuteReader());    //on execute

                if (int.Parse(dtMotif.Rows[0][0].ToString()) > 0)   //Si on a un enregistrement                
                    retour = "KO";                
                else retour = "OK";
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche de l'IdMotif dans la table Motif :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }
                              
            return retour;
        }

       
        //On met à jour un enregistrement
        private string UpdateEnreg()
        {
            string Retour = "KO";

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            //On déclare ici la requete (à cause de la transaction)
            string SqlStr0 = "";

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Def de la requete
                SqlStr0 = "UPDATE motif ";
                SqlStr0 += " SET LibelleMotif = '" + tBLibelle.Text + "', Urgence = '" + tBUrgence.Text + "'";
               
                if (rBVisite.Checked)
                     SqlStr0 += ", TypeMotif = 'V'";
                else SqlStr0 += ", TypeMotif = 'A'";
                
                SqlStr0 += " WHERE IdMotif = '" + tBIdMotif.Text + "'";

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();
                    Retour = "OK";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la modification du motif (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    //fermeture des connexions
                    if (dbConnection.State == System.Data.ConnectionState.Open)
                    {
                        dbConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la modification du Motif. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return Retour;
        }

        //Suppression du motif
        private void bSupprimer_Click(object sender, EventArgs e)
        {
            //On essaie de supprimer le motif...S'il n'a jamais été utilisé (Appels)
            DataTable dtExiste = FonctionsAppels.RechercheAppels("IdMotif", tBIdMotif.Text);
            if (dtExiste.Rows.Count > 0)
                MessageBox.Show("Impossible de supprimer ce motif car il a déjà été utilisée. ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //Suppression de l'enregistrement
                if (FonctionsAppels.SupprMotif(tBIdMotif.Text) == "OK")
                {
                    MessageBox.Show("Suppression du motif => OK ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    VideChamps();
                    //Gestion des boutons
                    bAjouter.Visible = true;
                    bValider.Enabled = false;
                    bCancel.Visible = false;
                    bSupprimer.Enabled = false;

                    Etat = "Lecture";

                    //On rafraichi la liste des motifs
                    FMotif_Load(sender, e);
                }                    
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            //on annule
            VideChamps();
            //Gestion des boutons
            bAjouter.Visible = true;
            bValider.Enabled = false;
            bCancel.Visible = false;
            bSupprimer.Enabled = false;

            Etat = "Lecture";

            //On rafraichi la liste des motifs
            FMotif_Load(sender, e);
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            if (Etat != "Lecture")
            {
                var result = MessageBox.Show("Il y a des modification en cours...Voulez vous les abandonner? ", "Abandon", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    this.Close();
            }
            else
                this.Close();
        }

        private void tBLibelle_Click(object sender, EventArgs e)
        {
            //si on est en lecture, on passe en ajout
            if (Etat == "Lecture")
            {
                bAjouter_Click(sender, e);
            }
        }

        private void tBLibelle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bValider_Click(sender, e);
            }
        }
 

    }
}
