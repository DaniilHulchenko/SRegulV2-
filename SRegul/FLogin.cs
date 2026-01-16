using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FLogin : Form
    {
        public string[] user = new string[8];   //7 taille du tableau <> index!
        private int chance = 0;
        private static Encryption Crypto = new Encryption();

        public FLogin()
        {
            InitializeComponent();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            chance += 1;

            //On vérifie le login
            if ((TButilisateur.Text.ToString() != "") && (TBpass.Text.ToString() != ""))          
            {
                //on recherche le nom de l'utilisateur
                string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
                MySqlConnection dbConnection = new MySqlConnection(connex);      //Chaine de connection récupérée dans le app.config                

                try
                {
                    //on ouvre la connexion
                    dbConnection.Open();

                    MySqlCommand cmd = dbConnection.CreateCommand();
                    cmd.Connection = dbConnection;       //On passe les parametres query et connection

                    //on charge tous les utilisateurs correspondant au login                 
                    string sqlstr0 = "select * from utilisateur where IdUtilisateur = @utilisateur";

                    cmd.CommandText = sqlstr0;

                    // Ajout des paramètres
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("utilisateur", TButilisateur.Text);

                    DataTable Dtlogin = new DataTable();

                    Dtlogin.Load(cmd.ExecuteReader());       //on execute

                    string Ouverture = "KO";

                    //On les parcours pour décoder le pass et le comparer à celui rentré
                    for (int i = 0; i < Dtlogin.Rows.Count; i++)
                    {
                        if (Crypto.Decrypt(Dtlogin.Rows[i]["Password"].ToString()) == TBpass.Text)                                                                                               
                            Ouverture = "OK";
                    }

                    //Si on a quelque chose
                    if (Ouverture == "OK")
                    {
                        //on affecte la variable user                        
                        user[0] = Dtlogin.Rows[0][0].ToString();    //IdUtilisateur
                        user[1] = Dtlogin.Rows[0][2].ToString();    //Droit
                        user[2] = Dtlogin.Rows[0][3].ToString();    //Nom
                        user[3] = Dtlogin.Rows[0][4].ToString();    //Prénom
                        user[4] = Dtlogin.Rows[0][5].ToString();    //IdSmartRapport
                        user[5] = Dtlogin.Rows[0][6].ToString();    //IdCTI
                        user[6] = Dtlogin.Rows[0][7].ToString();    //NumPoste
                        user[7] = Dtlogin.Rows[0][8].ToString();    //PassCTI

                        mouchard.evenement("Connexion utilisateur", user[2].ToString() + " " + user[3].ToString());   //log

                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        //Pas trouvé on donne encore 2 chance
                        if (chance < 3)
                        {
                            MessageBox.Show("Erreur d'identification " + "\nIl vous reste " + Convert.ToString(3 - chance) + " essais", "Identification ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            TButilisateur.Text = "";
                            TBpass.Text = "";

                            TButilisateur.Focus();
                        }
                        else
                        {
                            this.DialogResult = DialogResult.Abort;
                            MessageBox.Show("Loupé ! ", "Identification ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception a)
                {
                    //Console.WriteLine("Erreur : " + a.Message);
                    MessageBox.Show("Erreur0 : " + a.Message);
                }
                finally
                {
                    // Fermeture de la connexion
                    if (dbConnection.State == System.Data.ConnectionState.Open)
                        dbConnection.Close();
                }

                if (dbConnection.State == System.Data.ConnectionState.Open)
                    dbConnection.Close();
            }
        }

        private void FLogin_Load(object sender, EventArgs e)
        {
            //Récup du chemin de l'exécutable
            string exepath = Environment.GetCommandLineArgs()[0];
           
            label3.Text = "Version du " + File.GetCreationTime(exepath).ToShortDateString();   //Recup de la date de création du fichier pour la mettre dans le label

            Console.WriteLine(File.GetLastWriteTime(exepath).ToShortDateString());         
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
        }

        private void TBpass_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si on appuye sur Enter => on fait ok
            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                bOk_Click(sender, e);
            }
        }

    }
}
