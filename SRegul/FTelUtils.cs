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
    public partial class FTelUtils : Form
    {
        public static DataTable dtTelUtils = new DataTable();
        private static string Etat = "Lecture";     

        public FTelUtils()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("Id", 1);   //invisible
            listView1.Columns.Add("Libellé", 280);
            listView1.Columns.Add("Numero", 100);
            listView1.Columns.Add("Catégorie", 180);
            listView1.Columns.Add("Sous Catégorie", 160);
            listView1.View = View.Details;    //Pour afficher les subItems  

            //On vide les champs et initialise les boutons
            VideChamps();
        }

        private void FTelUtils_Load(object sender, EventArgs e)
        {
            dtTelUtils.Clear();
            dtTelUtils = FonctionsAppels.ChargeListeTelsUtils();

            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtTelUtils.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtTelUtils.Rows[i]["IdTelUtil"].ToString());
                item.SubItems.Add(dtTelUtils.Rows[i]["Libelle"].ToString());
                item.SubItems.Add(dtTelUtils.Rows[i]["Telephone"].ToString());
                item.SubItems.Add(dtTelUtils.Rows[i]["Categorie"].ToString());
                item.SubItems.Add(dtTelUtils.Rows[i]["Sous_Categorie"].ToString());

                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle       
        }

        private void VideChamps()
        {
            Num.Text = "";
            tBoxLibelle.Text = "";
            tBoxTel.Text = "";
            cBoxCategorie.Text = "";
            cBoxSScategorie.Text = "";
           
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

        //Selection d'un tel dans la liste
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            //On peuple les champs
            Num.Text = dtTelUtils.Rows[index]["IdTelUtil"].ToString();
            tBoxLibelle.Text = dtTelUtils.Rows[index]["Libelle"].ToString();
            tBoxTel.Text = dtTelUtils.Rows[index]["Telephone"].ToString();
            cBoxCategorie.Text = dtTelUtils.Rows[index]["Categorie"].ToString();
            cBoxSScategorie.Text = dtTelUtils.Rows[index]["Sous_Categorie"].ToString();

            //Gestion des boutons: On passe en modif
            Etat = "Modif";

            bAjouter.Visible = false;
            bValider.Enabled = true;
            bCancel.Visible = true;
            bSupprimer.Enabled = true;
        }

        //Gestion des boutons
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

                        //On rafraichi la liste des SmartPhones
                        FTelUtils_Load(sender, e);
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

                        //On rafraichi la liste des SmartPhones
                        FTelUtils_Load(sender, e);
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

            if (tBoxLibelle.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez saisir un libellé. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (tBoxTel.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner le n° de téléphone. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (cBoxCategorie.Text != "")
            {
                V1 += 1;               
            }
            else MessageBox.Show("Vous devez renseigner la catégorie. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
           

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

            try
            {               
                //Def de la requete         
                string SqlStr0 = "INSERT INTO telephones_utils ";
                SqlStr0 += " (Libelle, Telephone, Categorie, Sous_Categorie)";
                SqlStr0 += " VALUES('" + tBoxLibelle.Text.Replace("'", "''") + "','" + tBoxTel.Text + "','" + cBoxCategorie.Text.Replace("'", "''");
                SqlStr0 += "','" + cBoxSScategorie.Text.Replace("'", "''") + "')";
               
                //on execute les requettes                                       
                cmd.CommandText = SqlStr0; cmd.ExecuteNonQuery();
                    
                Retour = "OK";              
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la création du téléphone. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            try
            {
                //Def de la requete  
                string SqlStr0 = "UPDATE telephones_utils ";
                SqlStr0 += " SET Libelle = '" + tBoxLibelle.Text.Replace("'", "''") + "', Telephone = '" + tBoxTel.Text;
                SqlStr0 += "', Categorie = '" + cBoxCategorie.Text.Replace("'", "''") + "', Sous_Categorie = '" + cBoxSScategorie.Text.Replace("'", "''") + "'";               
                SqlStr0 += " WHERE IdTelUtil = '" + Num.Text + "'";

                //on execute les requettes                                       
                cmd.CommandText = SqlStr0; cmd.ExecuteNonQuery();
                
                Retour = "OK";               
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la modification du téléphone. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
            //On supprime le Tel.                     
            if (FonctionsAppels.SupprTelUtil(Num.Text) == "Ok")
            {
                 MessageBox.Show("Suppression du médecin => OK ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    VideChamps();
                    //Gestion des boutons
                    bAjouter.Visible = true;
                    bValider.Enabled = false;
                    bCancel.Visible = false;
                    bSupprimer.Enabled = false;

                    Etat = "Lecture";

                    //On rafraichi la liste des SmartPhones
                    FTelUtils_Load(sender, e);
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

            //On rafraichi la liste des médecins
            FTelUtils_Load(sender, e);
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

        private void tBoxLibelle_Click(object sender, EventArgs e)
        {
            //si on est en lecture, on passe en ajout
            if (Etat == "Lecture")
            {
                bAjouter_Click(sender, e);
            }
        }
       
    }
}
