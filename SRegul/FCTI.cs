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
    public partial class FCTI : Form
    {
        public FCTI()
        {
            InitializeComponent();
        }

        private void FCTI_Load(object sender, EventArgs e)
        {
            //On charge l'etat du CTI (Actif ou pas)                               
            if (FonctionsAppels.EtatCTI() == true)
                rBActive.Checked = true;
            else rBdesactive.Checked = true;
        }

        private void bValider_Click(object sender, EventArgs e)
        {
            //On regarde si on a désactivé le CTI                                   
            if (rBdesactive.Checked)
            {
                DialogResult dialogResult = MessageBox.Show("Vous allez désactiver le CTI. Est ce bien ce que vous voulez faire?", "Déactivation du CTI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    MAJCTI(0);

                    mouchard.evenement("Désactivation du CTI", Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log                                                
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("OK, alors on active le CTI", "Activation du CTI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    MAJCTI(1);
                    mouchard.evenement("Activation du CTI", Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log
                }
            }
            else
            {                
                MessageBox.Show("Activation du CTI", "Activation du CTI", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                MAJCTI(1);
                mouchard.evenement("Activation du CTI", Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log
            }

            this.Close();
        }

        private void MAJCTI(int val)
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "UPDATE param_divers SET ActivationCTI = '" + val + "'";
                cmd.CommandText = sqlstr0;
               
                cmd.ExecuteNonQuery();    //on execute    

                //Puis MAJ de la valeur globale
                if (val == 0)
                    Form1.ActivationCTI = false;
                else Form1.ActivationCTI = true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du changement d'état du CTI: " + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
       
    }
}
