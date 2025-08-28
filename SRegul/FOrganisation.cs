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
    public partial class FOrganisation : Form
    {
        public static DataTable dtOrganisation = new DataTable();
        private static string Etat = "Lecture";        

        public FOrganisation()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("IdOrganisation", 1);         //Colonne invisible
            listView1.Columns.Add("Organisation", 200);           
            listView1.View = View.Details;    //Pour afficher les subItems  

            //On vide les champs et iniitalise les boutons
            VideChamps();
        }

        private void FOrganisation_Load(object sender, EventArgs e)
        {
            dtOrganisation.Clear();
            dtOrganisation = FonctionsAppels.ChargeListeOrganisation();

            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtOrganisation.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtOrganisation.Rows[i]["IdOrganisation"].ToString());
                item.SubItems.Add(dtOrganisation.Rows[i]["LibelleOrg"].ToString());                

                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle        
        }

        private void VideChamps()
        {
            lIdOrg.Text = "";
            tBLibelleOrg.Text = "";
            cBoxAvecAppli.Checked = false;

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

        //Selection d'une orgination dans la liste
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            //On peuple les champs
            lIdOrg.Text = dtOrganisation.Rows[index]["IdOrganisation"].ToString();
            tBLibelleOrg.Text = dtOrganisation.Rows[index]["LibelleOrg"].ToString();
            
            if((bool)dtOrganisation.Rows[index]["UtiliseAppli"] == true)
                cBoxAvecAppli.Checked = true;
            else
                cBoxAvecAppli.Checked = false;


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

                        //On rafraichi la liste des Organisations
                        FOrganisation_Load(sender, e);
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

                        //On rafraichi la liste des Organisations
                        FOrganisation_Load(sender, e);
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

            if (tBLibelleOrg.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner le nom de l'organisation. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
           
            //En fonction des résultats
            switch (V1)
            {
                case 1: retour = "OK"; break;
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
                SqlStr0 = "INSERT INTO organisation ";
                SqlStr0 += " (LibelleOrg, UtiliseAppli)";

                if(cBoxAvecAppli.Checked)
                    SqlStr0 += " VALUES('" + tBLibelleOrg.Text + "',1)";
                else
                    SqlStr0 += " VALUES('" + tBLibelleOrg.Text + "',0)";

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
                    MessageBox.Show("Erreur lors de la création d'une organisation (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    //fermeture des connexions
                    if (dbConnection.State == ConnectionState.Open)
                    {
                        dbConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la création d'une organisation. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
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
                SqlStr0 = "UPDATE organisation ";
                SqlStr0 += " SET LibelleOrg = @LibelleOrg, UtiliseAppli = @UtiliseAppli";
                SqlStr0 += " WHERE IdOrganisation = '" + lIdOrg.Text + "'";

                cmd.CommandText = SqlStr0;
                cmd.Parameters.AddWithValue("LibelleOrg", tBLibelleOrg.Text);
                
                if(cBoxAvecAppli.Checked)
                    cmd.Parameters.AddWithValue("UtiliseAppli", 1);
                else
                    cmd.Parameters.AddWithValue("UtiliseAppli", 0);

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
                    MessageBox.Show("Erreur lors de la modification de l'organisation (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    //fermeture des connexions
                    if (dbConnection.State == ConnectionState.Open)
                    {
                        dbConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la modification de l'organisation. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return Retour;
        }


        private void bSupprimer_Click(object sender, EventArgs e)
        {
            //On essaie de supprimer l'organisation...S'il elle n'est pas dans médecins/infirmières ET histogarde
            DataTable dtExiste = FonctionsAppels.RechercheHisto("Organisation", tBLibelleOrg.Text);
            if (dtExiste.Rows.Count > 0)
                MessageBox.Show("Impossible de supprimer cette organisaton car elle a déjà été utilisée. ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //Suppression de l'enregistrement
                if (FonctionsAppels.SupprOrganisation(lIdOrg.Text) == "OK")
                {
                    MessageBox.Show("Suppression de l'organisation => OK ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    VideChamps();

                    //Gestion des boutons
                    bAjouter.Visible = true;
                    bValider.Enabled = false;
                    bCancel.Visible = false;
                    bSupprimer.Enabled = false;

                    Etat = "Lecture";

                    //On rafraichi la liste des Organisations
                    FOrganisation_Load(sender, e);
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

            //On rafraichi la liste des Organisations
            FOrganisation_Load(sender, e);
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

        private void tBLibelleOrg_Click(object sender, EventArgs e)
        {
            //si on est en lecture, on passe en ajout
            if (Etat == "Lecture")
            {
                bAjouter_Click(sender, e);
            }
        }

      
    }
}
