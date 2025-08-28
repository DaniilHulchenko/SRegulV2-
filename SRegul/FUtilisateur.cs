using MySqlConnector;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FUtilisateur : Form
    {

        public static DataTable dtUtilisateur = new DataTable();
        private static string Etat = "Lecture";
        public static string PassEnClair = "";
        private static Encryption Crypto = new Encryption();

        public FUtilisateur()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("Id Utilisateur", 1);    //Colonne invisible
            listView1.Columns.Add("Nom", 240);                      
            listView1.View = View.Details;    //Pour afficher les subItems  

            //On vide les champs et iniitalise les boutons
            VideChamps();
        }

        private void FUtilisateur_Load(object sender, EventArgs e)
        {
            dtUtilisateur.Clear();
            dtUtilisateur = FonctionsAppels.ChargeListeUtilisateurs("Tous");

            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtUtilisateur.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtUtilisateur.Rows[i]["IdUtilisateur"].ToString());
                item.SubItems.Add(dtUtilisateur.Rows[i]["Nom"].ToString() + " " + dtUtilisateur.Rows[i]["Prenom"].ToString());               
                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle            
        }


        private void VideChamps()
        {
            tBIdUtilisateur.Text = "";
            tBoxNom.Text = "";
            tBoxPrenom.Text = "";
            tBIdSmartRapport.Text = "";
            tBIdCTI.Text = "";
            tBPosteTel.Text = "";
            tBPassCTI.Text = "";

            PassEnClair = "";

            rBRegul.Checked = true;

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


        //Selection d'un utilisateur dans la liste
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            //On peuple les champs
            tBIdUtilisateur.Text = dtUtilisateur.Rows[index]["IdUtilisateur"].ToString();
            tBoxNom.Text = dtUtilisateur.Rows[index]["Nom"].ToString();
            tBoxPrenom.Text = dtUtilisateur.Rows[index]["Prenom"].ToString();
            tBIdSmartRapport.Text = dtUtilisateur.Rows[index]["IdSmartRapport"].ToString();
            tBIdCTI.Text = dtUtilisateur.Rows[index]["IdCTI"].ToString();
            tBPosteTel.Text = dtUtilisateur.Rows[index]["NumPoste"].ToString();
            tBPassCTI.Text = dtUtilisateur.Rows[index]["PassCTI"].ToString();

            //En fonction des droits
            switch(dtUtilisateur.Rows[index]["Droit"].ToString())
            {
                case "0": rBdesactif.Checked = true; break;    //Désactivé
                case "1": rBInvite.Checked = true; break;      //Invité (On désactive la gestion du proxy...HIN)
                case "2": rBRegul.Checked = true; break;      //Régulateur
                case "3": rBChefRegul.Checked = true; break;  //Chef Régulateur
                case "10": rBAdmin.Checked = true; break;     //Admin
                default: rBInvite.Checked = true; break;      //Par défaut c'est un invité
                    
                //A completer en fct des droits
            }

            //Recup du mot de passe que l'on décrypte 
            if (dtUtilisateur.Rows[index]["Password"].ToString() != "")              
                PassEnClair = Crypto.Decrypt(dtUtilisateur.Rows[index]["Password"].ToString());               
            else
                PassEnClair = "";
       
            //Gestion des boutons: On passe en modif
            Etat = "Modif";

            //On verrouille L'IDUtilisateur (On ne peut plus le modifier)
            tBIdUtilisateur.ReadOnly = true;

            bPassword.Enabled = true;
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

            tBIdUtilisateur.ReadOnly = false;

            bPassword.Enabled = true;
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

                        tBIdUtilisateur.ReadOnly = false;

                        //Gestion des boutons
                        bPassword.Enabled = false;
                        bAjouter.Visible = true;
                        bValider.Enabled = false;
                        bCancel.Visible = false;
                        bSupprimer.Enabled = false;

                        Etat = "Lecture";

                        //On rafraichi la liste des SmartPhones
                        FUtilisateur_Load(sender, e);
                    }
                    else
                    {
                        //Gestion des boutons
                        bPassword.Enabled = true;
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

                        tBIdUtilisateur.ReadOnly = false;

                        //Gestion des boutons
                        bPassword.Enabled = false;
                        bAjouter.Visible = true;
                        bValider.Enabled = false;
                        bCancel.Visible = false;
                        bSupprimer.Enabled = false;

                        Etat = "Lecture";

                        //On rafraichi la liste des SmartPhones
                        FUtilisateur_Load(sender, e);
                    }
                    else
                    {
                        tBIdUtilisateur.ReadOnly = true;

                        //Gestion des boutons
                        bPassword.Enabled = false;
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

            if (Etat == "Ajout")   //On verifie IdUtilisateur seulement si on est en Ajout
            {
                if (tBIdUtilisateur.Text != "")
                {
                    if (VerifNumIdUtilisateur(tBIdUtilisateur.Text) == "OK")
                        V1 += 1;
                    else MessageBox.Show("L'Id utilisateur est déjà utilisé pour quelqu'un d'autre. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Vous devez saisir l'Id Utilisateur. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                V1 += 1;

            if (tBoxNom.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner le nom de l'utilisateur ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (tBoxPrenom.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner le Prénom de l'utilisateur. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (tBIdSmartRapport.Text != "")            
                V1 += 1;
            else
                 MessageBox.Show("Vous devez renseigner le Code SmartRapport de cet utilisateur. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            //Si on a renseigné un code CTI, alors on doit avoir un N° de poste
            if (tBIdCTI.Text != "")
            {
                if (tBPosteTel.Text != "")
                    V1 += 1;
                else
                {
                    MessageBox.Show("Comme vous avez renseigné un code CTI, vous devez également fournir son n° de poste. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    V1 += -1;
                }
            }
            else if (tBPosteTel.Text != "")
                      V1 += 1;

                
            if(PassEnClair != "")
                V1 += 1;
            else
                MessageBox.Show("Vous donner un mot de passe à cet utilisateur. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
          
            //En fonction des résultats
            switch (V1)
            {                
                case 6: retour = "OK"; break;
                default: retour = "KO"; break;
            }

            return retour;
        }

        private string VerifNumIdUtilisateur(string IdUtilisateur)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT count(*) FROM utilisateur WHERE IdUtilisateur = @IdUtilisateur";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdUtilisateur", IdUtilisateur);

                DataTable dtRetour = new DataTable();

                dtRetour.Load(cmd.ExecuteReader());    //on execute    

                if (int.Parse(dtRetour.Rows[0][0].ToString()) == 0)
                    Retour = "OK";
                else
                    Retour = "KO";
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la vérification d'un IdUtilisateur " + IdUtilisateur + " :" + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string SqlStr1 = "";

            //Gestion du Password On crypte le Pass          
            string PassCrypte = Crypto.Encrypt(PassEnClair);

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Def de la requete
                SqlStr0 = "INSERT INTO utilisateur ";
                SqlStr0 += " (IdUtilisateur, Password, Droit, Nom, Prenom, IdSmartRapport, IdCTI, NumPoste, PassCTI)";
                SqlStr0 += " VALUES('" + tBIdUtilisateur.Text + "','" + PassCrypte + "',";

                if (rBdesactif.Checked)
                    SqlStr0 += "'0',";
                else if (rBInvite.Checked)
                    SqlStr0 += "'1',";
                else if (rBRegul.Checked)
                    SqlStr0 += "'2',";
                else if (rBChefRegul.Checked)
                    SqlStr0 += "'3',";
                else if (rBAdmin.Checked)
                    SqlStr0 += "'10',";

                SqlStr0 += "'" + tBoxNom.Text.Replace("'", "''") + "','" + tBoxPrenom.Text.Replace("'", "''") + "','";
                SqlStr0 += tBIdSmartRapport.Text + "','" + tBIdCTI.Text + "','" + tBPosteTel.Text +  "','" + tBPassCTI.Text + "')";                

                //Puis pour les parametres utilisateur
                //Def de la requete
                SqlStr1 = "INSERT INTO pref_utilisateur ";
                SqlStr1 += " (IdUtilisateur, Notif_Medecin, Notif_Message, Typecarte)";
                SqlStr1 += " VALUES('" + tBIdUtilisateur.Text + "','Clavecin', 'come_and_get_your_love', 1)";

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();

                    Retour = "OK";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de l'ajout de l'utilisateur (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Erreur lors de l'ajout de l'utilisateur. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

            //On cypte le Pass
            string PassCrypte = Crypto.Encrypt(PassEnClair);

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Def de la requete
                SqlStr0 = "UPDATE utilisateur SET Password = '" + PassCrypte + "'";

                if (rBdesactif.Checked)
                    SqlStr0 += ", Droit = '0',";
                else if (rBInvite.Checked)
                    SqlStr0 += ", Droit = '1',";
                else if (rBRegul.Checked)
                    SqlStr0 += ", Droit = '2',";
                else if (rBChefRegul.Checked)
                    SqlStr0 += ", Droit = '3',";
                else if (rBAdmin.Checked)
                    SqlStr0 += ", Droit = '10',";

                SqlStr0 += " Nom = '" + tBoxNom.Text + "', Prenom = '" + tBoxPrenom.Text;
                SqlStr0 += "', IdSmartRapport = '" + tBIdSmartRapport.Text + "', IdCTI = '" + tBIdCTI.Text + "', NumPoste = '" + tBPosteTel.Text + "', PassCTI = '" + tBPassCTI.Text + "'";
                SqlStr0 += " WHERE IdUtilisateur = '" + tBIdUtilisateur.Text + "'";

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
                    MessageBox.Show("Erreur lors de la modification de l'utilisateur (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Erreur lors de la modification de l'utilisateur. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
            //On essaie de supprimer l'utilisateur...S'il n'a jamais travaillé (SuiviAppel)
            DataTable dtExiste = FonctionsAppels.RechercheSuiviAppels("NumRegul", tBIdUtilisateur.Text);
            if (dtExiste.Rows.Count > 0)
                MessageBox.Show("Impossible de supprimer cet utilisateur car il a déjà travaillé. ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //Suppression de l'enregistrement
                if (FonctionsAppels.SupprUtilisateur(tBIdUtilisateur.Text) == "Ok")
                {
                    MessageBox.Show("Suppression de l'utilisateur => OK ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    VideChamps();
                    //Gestion des boutons
                    bAjouter.Visible = true;
                    bValider.Enabled = false;
                    bCancel.Visible = false;
                    bSupprimer.Enabled = false;

                    Etat = "Lecture";

                    //On rafraichi la liste des utilisateurs
                    FUtilisateur_Load(sender, e);
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

            //On rafraichi la liste des utilisateurs
            FUtilisateur_Load(sender, e);
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

        private void tBIdUtilisateur_Click(object sender, EventArgs e)
        {
            //si on est en lecture, on passe en ajout
            if (Etat == "Lecture")
            {
                bAjouter_Click(sender, e);
            }
        }

        private void tBPosteTel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bValider_Click(sender, e);
            }
        }

        private void bPassword_Click(object sender, EventArgs e)
        {
            //On ouvre la forme de changement de mot de passe           
            FPassword fPassword = new FPassword();
            fPassword.MotDePassEnClair = PassEnClair;

            fPassword.ShowDialog(this);
            fPassword.Dispose();

        }

    }
}
