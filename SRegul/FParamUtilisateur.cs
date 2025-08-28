using MySqlConnector;
using System;
using System.Configuration;
using System.Data;
using System.Media;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FParamUtilisateur : Form
    {
        public FParamUtilisateur()
        {
            InitializeComponent();

            timer1.Enabled = false;
        }

        private void FParamUtilisateur_Load(object sender, EventArgs e)
        {            
            DataTable dtPref = FonctionsAppels.ChargePreferencesUtil(Form1.Utilisateur[0]);
            if (dtPref.Rows.Count > 0)
            {                
                //Sons de notification
                cBoxFinVisite.Text = dtPref.Rows[0]["Notif_Medecin"].ToString();
                cBoxMessage.Text = dtPref.Rows[0]["Notif_Message"].ToString();

                //Le type de carte
                switch (int.Parse(dtPref.Rows[0]["Typecarte"].ToString()))
                {
                    case 1: rB1.Checked = true; break;
                    case 2: rB2.Checked = true;  break;
                    case 3: rB3.Checked = true; break;
                    
                    default: rB1.Checked = true; break;
                }

                //Puis en fonction des droits de l'utilisateur, on bloque le choix ou non de la notif de fn de visite
              /*  if (int.Parse(Form1.Utilisateur[1]) == 3 || int.Parse(Form1.Utilisateur[1]) == 10)
                    cBoxFinVisite.Enabled = true;
                else cBoxFinVisite.Enabled = false;*/
            }                                  
        }

        private void bFermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bNotifs_Click(object sender, EventArgs e)
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "";
            string sqlstr1 = "";

            try
            {   
                //En fonction des droits des utilisateurs
               //if (int.Parse(Form1.Utilisateur[1]) == 3 || int.Parse(Form1.Utilisateur[1]) == 10)
               // {
                    //On met à jour la notif de fin de visite pour tout le monde
                    if (cBoxFinVisite.Text != "")
                    {
                        sqlstr0 = @"UPDATE pref_utilisateur 
                              SET Notif_Medecin = @Notif_Medecin";

                        cmd.CommandText = sqlstr0;

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("Notif_Medecin", cBoxFinVisite.Text);

                        cmd.ExecuteNonQuery();    //on execute      
                    }
                //}

                //Dans tout les cas les prefs de l'utilisateur            
                if (cBoxMessage.Text != "")
                {
                    sqlstr1 = @"UPDATE pref_utilisateur 
                            SET Notif_Message = @Notif_Message, Typecarte = @Typecarte
                            WHERE IdUtilisateur = @IdUtil";                                                               
                    cmd.CommandText = sqlstr1;

                    cmd.Parameters.Clear();               
                    cmd.Parameters.AddWithValue("Notif_Message", cBoxMessage.Text);               

                    //Le type de carte
                    int carte = 1;
                    if (rB1.Checked == true)
                        carte = 1;
                    else if (rB2.Checked == true)
                        carte = 2;
                    else if (rB3.Checked == true)
                        carte = 3;
                                        
                    cmd.Parameters.AddWithValue("Typecarte",carte);
                    cmd.Parameters.AddWithValue("IdUtil", Form1.Utilisateur[0]);

                    cmd.ExecuteNonQuery();    //on execute       
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la mise à jour des notifications et de la carte, table pref_utilisateur: " + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                timer1.Enabled = true;
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                     dbConnection.Close();
                }
            }

            MessageBox.Show("Mise à jour des Notifications et de la carte OK, \r\nRedemarrez pour appliquer les motifications.", "notifications et carte", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            timer1.Enabled = true;            
        }

        private void Beeper(string SonAJouer)
        {
            SoundPlayer FinVisiteSound = new SoundPlayer(Application.StartupPath + @"\Sons\" + SonAJouer);
            FinVisiteSound.Play();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bTestSon1_Click(object sender, EventArgs e)
        {
            if (cBoxFinVisite.Text != "")
                Beeper(cBoxFinVisite.Text + ".wav");
        }

        private void bTestSon2_Click(object sender, EventArgs e)
        {
            if (cBoxMessage.Text != "")
                Beeper(cBoxMessage.Text + ".wav");
        }
    }
}
