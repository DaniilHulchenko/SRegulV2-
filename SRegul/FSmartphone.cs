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
    public partial class FSmartphone : Form
    {

        public static DataTable dtSmartphone = new DataTable();
        public static DataTable dtMedecin = new DataTable();
        private bool nonNumerique = false;    //Pour s'assurer qu'on rentre bien un caractere numerique, ou le +
        private static string Etat = "Lecture";

        public FSmartphone()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("Idsmart", 1);         //Colonne invisible
            listView1.Columns.Add("Smartphone", 120);     //Nom du smartphone
            listView1.Columns.Add("Medecin", 150);       //Medecin attribué   
            listView1.Columns.Add("CodeMed", 1);         //Colonne invisible
            listView1.View = View.Details;    //Pour afficher les subItems  
          
            //On vide les champs et iniitalise les boutons
            VideChamps();
        }

        private void FSmartphone_Load(object sender, EventArgs e)
        {
            dtSmartphone.Clear();
            dtSmartphone = FonctionsAppels.ChargeListeSmarphone();

            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtSmartphone.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtSmartphone.Rows[i]["IdSmartphone"].ToString());     //IdSmartphone
                item.SubItems.Add(dtSmartphone.Rows[i]["NomSmartphone"].ToString());                       //NomSmartphone                    

                if (SRegulV2.FonctionsAppels.NomMedecin(dtSmartphone.Rows[i]["CodeMedecin"].ToString()).ToString() != "Anomyme")
                    item.SubItems.Add(SRegulV2.FonctionsAppels.NomMedecin(dtSmartphone.Rows[i]["CodeMedecin"].ToString()).ToString());   //Nom du médecin
                else
                    item.SubItems.Add("Non attribué");

                item.SubItems.Add(dtSmartphone.Rows[i]["CodeMedecin"].ToString());                         //CodeMedecin         
                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle

            //Puis la liste des medecins
            dtMedecin = FonctionsAppels.ChargeListeMedecins("actifs toutes les org");
        }


        private void VideChamps()
        {           
            labelId.Text = "";
            tBoxNomSmart.Text = "";
            tBoxSim.Text = "";
            tBoxTel.Text = "";
            tBoxCodeMedecin.Text = "";
            comboMedecin.Text = "";
            cBoxActif.Checked = false;

            //Pour les boutons
            bAjouter.ImageIndex = 0 ;
            bAjouter.Visible = true;             

            bValider.ImageIndex = 4 ;
            bValider.Enabled = false;                    
            
            bSupprimer.ImageIndex = 6;
            bSupprimer.Enabled = false;

            bCancel.Visible = false;

            bExit.ImageIndex = 7;   
        }

        //Selection d'un Smartphone dans la liste
        private void listView1_DoubleClick(object sender, EventArgs e)
        {                        
            int index = listView1.SelectedItems[0].Index;
           
            //On peuple les champs
            labelId.Text = dtSmartphone.Rows[index]["IdSmartphone"].ToString();
            tBoxNomSmart.Text = dtSmartphone.Rows[index]["NomSmartphone"].ToString();
            tBoxSim.Text = dtSmartphone.Rows[index]["CarteSim"].ToString();
            tBoxTel.Text = dtSmartphone.Rows[index]["NumTel"].ToString();
            tBoxCodeMedecin.Text = dtSmartphone.Rows[index]["CodeMedecin"].ToString();
           
            //Affichage du nom du medecin dans la combo
            comboMedecin.Text = FonctionsAppels.NomMedecin(dtSmartphone.Rows[index]["CodeMedecin"].ToString());

            if (dtSmartphone.Rows[index]["Actif"].ToString() == "True")
                cBoxActif.Checked = true;
            else
                cBoxActif.Checked = false;

            //Gestion des boutons: On passe en modif
            Etat = "Modif";

            bAjouter.Visible = false;        
            bValider.Enabled = true;
            bCancel.Visible = true;
            bSupprimer.Enabled = true;
        }

        private void comboMedecin_DropDown(object sender, EventArgs e)
        {
            //On recherche le code si trouvé on l'affiche ainsi que le libellé dans le cBox
            //On vide la liste...Avant de la recharger
            comboMedecin.Items.Clear();
            
            if (comboMedecin.Text != "")
            {
                string SqlSelect = "Nom LIKE '" + comboMedecin.Text + "%'";
                string Trie = "Nom";

                //Si on a des résultats
                if (dtMedecin.Select(SqlSelect).Any())
                {
                    DataTable dtMedecinTrie = dtMedecin.Select(SqlSelect, Trie).CopyToDataTable();

                    //On charge la liste des medecins
                    comboMedecin.Items.Clear();

                    for (int i = 0; i < dtMedecinTrie.Rows.Count; i++)
                    {
                        comboMedecin.Items.Add(dtMedecinTrie.Rows[i]["Nom"].ToString() + ' ' + dtMedecinTrie.Rows[i]["Prenom"].ToString());
                    }

                }
            }
            else
            {
                //On affiche tout le monde
                for (int i = 0; i < dtMedecin.Rows.Count; i++)
                {
                     comboMedecin.Items.Add(dtMedecin.Rows[i]["Nom"].ToString() + ' ' + dtMedecin.Rows[i]["Prenom"].ToString());
                }               
            }
        }

        private void comboMedecin_SelectedIndexChanged(object sender, EventArgs e)
        {
            //On recherche le code si trouvé on l'affiche ainsi que le nom dans le comboMedecin
            string SqlSelect = "Nom + ' ' + Prenom ='" + comboMedecin.Text + "'";

            //Si on a des résultats
            try
            {
                dtMedecin.Select(SqlSelect).Any();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
                
            if (dtMedecin.Select(SqlSelect).Any()) //Si on a des résultats
            {
                DataTable dtMedecinTrie = dtMedecin.Select(SqlSelect).CopyToDataTable();
                tBoxCodeMedecin.Text = dtMedecinTrie.Rows[0]["CodeMedecin"].ToString();
            }
        }


        private void tBoxCodeMedecin_Leave(object sender, EventArgs e)
        {
            //On recherche le code si trouvé on l'affiche ainsi que le libellé dans le cBox
            if (tBoxCodeMedecin.Text != "")
            {
                string SqlSelect = "CodeMedecin ='" + tBoxCodeMedecin.Text + "'";
                string Trie = "Nom";

                comboMedecin.Items.Clear();

                //Si on a des résultats
                if (dtMedecin.Select(SqlSelect).Any())
                {
                    DataTable dtMedecinTrie = dtMedecin.Select(SqlSelect, Trie).CopyToDataTable();

                    for (int i = 0; i < dtMedecinTrie.Rows.Count; i++)
                    {
                        comboMedecin.Items.Add(dtMedecinTrie.Rows[i]["Nom"].ToString() + ' ' + dtMedecinTrie.Rows[i]["Prenom"].ToString());
                    }

                    //S'il n'y a qu'un seul résultat, on l'affiche directement (pas besoin de choisir)
                    if (dtMedecinTrie.Rows.Count == 1)
                    {
                        comboMedecin.Text = dtMedecinTrie.Rows[0]["Nom"].ToString() + ' ' + dtMedecinTrie.Rows[0]["Prenom"].ToString();
                    }
                }
            }
        }

        private void comboMedecin_TextChanged(object sender, EventArgs e)
        {
            if (comboMedecin.Text == "")
            {
                if (tBoxCodeMedecin.Text.Length > 1)
                    tBoxCodeMedecin.Text = "";
            }
        }

        //********Gestion des contrôles de saisie********
        //On regarde si on rentre bien un n° ou un + Quand on appuie sur une touche
        private void tBoxTel_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumerique = false;

            //Pour les nombres du haut du clavier ET la touche + ET les nombres du pavé numérique
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) && e.KeyCode != Keys.Add &&
                 (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
            {
                nonNumerique = true;
                Console.WriteLine("KO");
            }
        }

        //Quand on relache la touche....
        private void tBoxTel_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (nonNumerique == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
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
            if (VerifSaisie() =="OK")
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
                        FSmartphone_Load(sender, e);
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
                        FSmartphone_Load(sender, e);
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

            if (tBoxTel.Text != "")     //Formatage du Telephone
            {
                //Formatage du n° de Tel: Si c'est au format International
                if (tBoxTel.Text.IndexOf("+") == -1)
                {
                    if (tBoxTel.Text.Substring(0, 1) == "0")
                        tBoxTel.Text = "+41" + tBoxTel.Text.Remove(0, 1);
                    else tBoxTel.Text = "+" + tBoxTel.Text;
                }
                else
                {   //On le reformate
                    tBoxTel.Text = "+" + tBoxTel.Text.Replace("+", "");
                }

                V1 += 1;
            }
            else
                MessageBox.Show("Vous devez saisir un n° de téléphone. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (tBoxNomSmart.Text != "")
                V1 +=1;
            else
                MessageBox.Show("Vous devez renssigner un nom de smartphone. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            if (tBoxSim.Text != "")
                V1 +=1;
            else MessageBox.Show("Vous devez saisir le N° de la carte SIM du Smartphone. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            if (tBoxCodeMedecin.Text != "")
                V1 +=1;
            else MessageBox.Show("Vous devez saisir un code médecin (0 si aucun). ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                           
            //En fonction des résultats
            switch (V1)
            {                
                case 4: retour = "OK"; break;
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
                //*****SmartPhones***
                SqlStr0 = "INSERT INTO smartphone ";
                SqlStr0 += " (NomSmartphone, CarteSim, NumTel, CodeMedecin, Actif)";                
                SqlStr0 += " VALUES('" + tBoxNomSmart.Text + "','" + tBoxSim.Text + "','" + tBoxTel.Text + "','" + tBoxCodeMedecin.Text + "'";

                if(cBoxActif.Checked)
                    SqlStr0 += ",'1')";
                else SqlStr0 += ",'0')";

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
                catch(Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la création du Smartphone (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);                                  
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
            catch(Exception e)
            {                    
                 MessageBox.Show("Erreur lors de la création du Smartphone. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 
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
                //*****SmartPhones***
                SqlStr0 = "UPDATE smartphone ";
                SqlStr0 += " SET NomSmartphone = '" + tBoxNomSmart.Text + "', CarteSim = '" + tBoxSim.Text + "', NumTel = '" + tBoxTel.Text;
                SqlStr0 += "', CodeMedecin= '" + tBoxCodeMedecin.Text +"', Actif = ";                
               
                if(cBoxActif.Checked)
                    SqlStr0 += "'1' WHERE IdSmartphone = '" + labelId.Text + "'";
                else SqlStr0 += "'0' WHERE IdSmartphone = '" + labelId.Text + "'";
              
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
                catch(Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la modification du Smartphone (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);                  
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
            catch(Exception e)
            {                    
                 MessageBox.Show("Erreur lors de la modification du Smartphone. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 
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

        private void bSupprimer_Click(object sender, EventArgs e)
        {
            //On essaie de supprimer le smartphone...S'il n'a jamais été utilisé (HistoGarde)
            DataTable dtExiste = FonctionsAppels.RechercheHisto("IdSmartPhone", labelId.Text);
            if (dtExiste.Rows.Count > 0)
                MessageBox.Show("Impossible de supprimer ce Smartphone car il est dans l'historique. ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //Suppression de l'enregistrement
                if (FonctionsAppels.SupprSmartphone(labelId.Text) == "OK")
                {
                    MessageBox.Show("suppression. ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    VideChamps();
                    //Gestion des boutons
                    bAjouter.Visible = true;
                    bValider.Enabled = false;
                    bCancel.Visible = false;
                    bSupprimer.Enabled = false;

                    Etat = "Lecture";

                    //On rafraichi la liste des SmartPhones
                    FSmartphone_Load(sender, e);
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

            //On rafraichi la liste des SmartPhones
            FSmartphone_Load(sender, e);
        }

        private void tBoxNomSmart_Click(object sender, EventArgs e)
        {
            //si on est en lecture, on passe en ajout
            if (Etat == "Lecture")
            {
                bAjouter_Click(sender, e);
            }
        }

        private void tBoxCodeMedecin_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumerique = false;

            //Pour les nombres du haut du clavier ET les nombres du pavé numérique
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) &&
                 (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
            {
                nonNumerique = true;
                Console.WriteLine("KO");
            }          
        }

        private void tBoxCodeMedecin_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (nonNumerique == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }
                   
      
    }
}


//A faire

