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
    public partial class FRue : Form
    {
        public static DataTable dtRue = new DataTable();
        private static string Etat = "Lecture";
        private static bool nonNumeriqueCP1 = false;

        public FRue()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("IdRue", 1);         //Colonne invisible
            listView1.Columns.Add("Nom rue", 240);
            listView1.Columns.Add("Nom commune", 200);
            listView1.Columns.Add("Code postal", 100);
            listView1.View = View.Details;    //Pour afficher les subItems  

            //On vide les champs et iniitalise les boutons
            VideChamps();
        }

        private void FRue_Load(object sender, EventArgs e)
        {
            dtRue.Clear();
            dtRue = FonctionsAppels.ChargeListeRue();

            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtRue.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtRue.Rows[i]["IdRue"].ToString());
                item.SubItems.Add(dtRue.Rows[i]["NomRue"].ToString());
                item.SubItems.Add(dtRue.Rows[i]["Commune"].ToString());
                item.SubItems.Add(dtRue.Rows[i]["CodePostal"].ToString());

                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle       
        }


        private void VideChamps()
        {
            lIdRue.Text = "";
            tBNomRue.Text = "";
            tBNomCommune.Text = "";
            tBCodePostal.Text = "";

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


        //Selection d'une rue dans la liste
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            //On peuple les champs
            lIdRue.Text = listView1.SelectedItems[0].Text;
            tBNomRue.Text = listView1.SelectedItems[0].SubItems[1].Text;
            tBNomCommune.Text = listView1.SelectedItems[0].SubItems[2].Text;
            tBCodePostal.Text = listView1.SelectedItems[0].SubItems[3].Text;

            //Gestion des boutons: On passe en modif
            Etat = "Modif";

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
                if (Etat == "Ajout")
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

                        //On rafraichi la liste des rues
                        FRue_Load(sender, e);
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

                        //On rafraichi la liste des rues
                        FRue_Load(sender, e);
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

            if (tBNomRue.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner le nom de la rue. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (tBNomCommune.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner le nom de la commune. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (tBCodePostal.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner le code postal. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                SqlStr0 = "INSERT INTO rue ";
                SqlStr0 += " (NomRue, Commune, CodePostal)";
                SqlStr0 += " VALUES( @NomRue, @Commune, @CodePostal)";

                cmd.CommandText = SqlStr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("NomRue", tBNomRue.Text.Replace("'", "''"));
                cmd.Parameters.AddWithValue("Commune", tBNomCommune.Text.Replace("'", "''"));
                cmd.Parameters.AddWithValue("CodePostal", tBCodePostal.Text);

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.Transaction = trans; cmd.ExecuteNonQuery(); trans.Commit();

                    Retour = "OK";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la création de la rue (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Erreur lors de la création de la rue. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                //*****Commune***
                SqlStr0 = "UPDATE rue ";
                SqlStr0 += " SET NomRue = '" + tBNomRue.Text.Replace("'", "''") + "', Commune = '" + tBNomCommune.Text.Replace("'", "''") + "', CodePostal = '" + tBCodePostal.Text + "'";
                SqlStr0 += " WHERE IdRue = '" + lIdRue.Text + "'";

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
                    MessageBox.Show("Erreur lors de la modification de la rue (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Erreur lors de la modification de la rue. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

        private void bSupprimer_Click(object sender, EventArgs e)
        {
            //On essaie de supprimer la rue...Pas de restriction puisqu'on ne récupère que le libellé           
            //Suppression de l'enregistrement
            if (FonctionsAppels.SupprRue(lIdRue.Text) == "OK")
            {
                MessageBox.Show("Suppression de la rue => OK ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                VideChamps();
                //Gestion des boutons
                bAjouter.Visible = true;
                bValider.Enabled = false;
                bCancel.Visible = false;
                bSupprimer.Enabled = false;

                Etat = "Lecture";

                //On rafraichi la liste des rues
                FRue_Load(sender, e);
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

            //On rafraichi la liste des Communes
            FRue_Load(sender, e);
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

        private void tBNomRue_Click(object sender, EventArgs e)
        {
            //si on est en lecture, on passe en ajout
            if (Etat == "Lecture")
            {
                bAjouter_Click(sender, e);
            }
        }

        private void tBNomCommune_Click(object sender, EventArgs e)
        {
            //si on est en lecture, on passe en ajout
            if (Etat == "Lecture")
            {
                bAjouter_Click(sender, e);
            }
        }

        private void tBCodePostal_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumeriqueCP1 = false;

            //Pour les nombres du haut du clavier ET les nombres du pavé numérique        
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) &&
                     (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
            {
                nonNumeriqueCP1 = true;
                Console.WriteLine("KO");
            }

            if (e.KeyCode == Keys.Enter)
            {
                bValider_Click(sender, e);
            }

            Console.WriteLine(e.KeyValue + " " + e.KeyData);
        }

        private void tBCodePostal_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (nonNumeriqueCP1 == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }           
        }

        private void tBRue_TextChanged(object sender, EventArgs e)
        {
            //on lance une recherche avec ce qui a été entré           
            string SqlSelect = "NomRue like '%" + tBRue.Text + "%'";
            string Trie = "Commune";

            if (dtRue.Select(SqlSelect, Trie).Any())    //Si on a quelque chose
            {
                DataTable dtRueTrie = dtRue.Select(SqlSelect, Trie).CopyToDataTable();

                listView1.BeginUpdate();
                listView1.Items.Clear();

                //On charge la liste
                for (int i = 0; i < dtRueTrie.Rows.Count; i++)
                {
                    ListViewItem item1 = new ListViewItem(dtRueTrie.Rows[i]["IdRue"].ToString());
                    item1.SubItems.Add(dtRueTrie.Rows[i]["NomRue"].ToString());
                    item1.SubItems.Add(dtRueTrie.Rows[i]["Commune"].ToString());
                    item1.SubItems.Add(dtRueTrie.Rows[i]["CodePostal"].ToString());

                    listView1.Items.Add(item1);
                }

                listView1.EndUpdate();  //Rafraichi le controle 
            }                       
        }


    }
}
