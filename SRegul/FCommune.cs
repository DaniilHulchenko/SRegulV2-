using MySqlConnector;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FCommune : Form
    {
        public static DataTable dtCommune = new DataTable();
        private static string Etat = "Lecture";
        private static bool nonNumeriqueCP1 = false;
        
        public FCommune()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("IdCommune", 1);         //Colonne invisible
            listView1.Columns.Add("Nom Commune", 200);
            listView1.Columns.Add("Code Postal", 100);
            listView1.Columns.Add("Pays", 100);
            listView1.View = View.Details;    //Pour afficher les subItems  

            //On vide les champs et iniitalise les boutons
            VideChamps();
        }

        private void FCommune_Load(object sender, EventArgs e)
        {
            dtCommune.Clear();
            dtCommune = FonctionsAppels.ChargeListeCommune();

            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtCommune.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtCommune.Rows[i]["IdCommune"].ToString());
                item.SubItems.Add(dtCommune.Rows[i]["NomCommune"].ToString());
                item.SubItems.Add(dtCommune.Rows[i]["CodePostal"].ToString());
                item.SubItems.Add(dtCommune.Rows[i]["Pays"].ToString());
               
                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle        
        }


        private void VideChamps()
        {
            lIdCommune.Text = "";
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


        //Selection d'une commune dans la liste
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            //On peuple les champs
            lIdCommune.Text = dtCommune.Rows[index]["IdCommune"].ToString();
            tBNomCommune.Text = dtCommune.Rows[index]["NomCommune"].ToString();
            tBCodePostal.Text = dtCommune.Rows[index]["CodePostal"].ToString();
            cBoxPays.Text = dtCommune.Rows[index]["Pays"].ToString();
           
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

                        //On rafraichi la liste des Communes
                        FCommune_Load(sender, e);
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

                        //On rafraichi la liste des communes
                        FCommune_Load(sender, e);
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

            if (tBNomCommune.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner le nom de la commune. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (tBCodePostal.Text != "")                           
                V1 += 1;
            else                
                MessageBox.Show("Vous devez renseigner le code postal. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (cBoxPays.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner le pays. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);     
           
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
                SqlStr0 = "INSERT INTO commune ";
                SqlStr0 += " (NomCommune, CodePostal, Pays)";
                SqlStr0 += " VALUES('" + tBNomCommune.Text + "','" + tBCodePostal.Text + "','" + cBoxPays.Text + "')";
               
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
                    MessageBox.Show("Erreur lors de la création de la commune (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Erreur lors de la création de la commune. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                SqlStr0 = "UPDATE commune ";
                SqlStr0 += " SET NomCommune = @Commune, CodePostal = @CodePostal, Pays = @Pays";
                SqlStr0 += " WHERE IdCommune = '" + lIdCommune.Text + "'";

                cmd.CommandText = SqlStr0;
                cmd.Parameters.AddWithValue("Commune", tBNomCommune.Text);
                cmd.Parameters.AddWithValue("CodePostal", tBCodePostal.Text);
                cmd.Parameters.AddWithValue("Pays", cBoxPays.Text);
                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.Transaction = trans;
                    cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();
                    Retour = "OK";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la modification de la commune (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Erreur lors de la modification de la commune. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
            //On essaie de supprimer la Commune...S'il elle n'a jamais été utilisée (Appels)
            DataTable dtExiste = FonctionsAppels.RechercheAppels("Commune", tBNomCommune.Text);
            if (dtExiste.Rows.Count > 0)
                MessageBox.Show("Impossible de supprimer cette commune car elle a déjà été utilisée. ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //Suppression de l'enregistrement
                if (FonctionsAppels.SupprCommune(lIdCommune.Text) == "OK")
                {
                    MessageBox.Show("Suppression de la commune => OK ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    VideChamps();

                    //Gestion des boutons
                    bAjouter.Visible = true;
                    bValider.Enabled = false;
                    bCancel.Visible = false;
                    bSupprimer.Enabled = false;

                    Etat = "Lecture";

                    //On rafraichi la liste des Communes
                    FCommune_Load(sender, e);
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

            //On rafraichi la liste des Communes
            FCommune_Load(sender, e);
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
    }
}
