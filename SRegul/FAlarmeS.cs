using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FAlarmeS : Form
    {
        public FAlarmeS()
        {
            InitializeComponent();
        }

        private void bClearAlarme_Click(object sender, EventArgs e)
        {
            //On ré-initialise l'alarme silencieuse  
            if (FonctionsAppels.ClearAlarme() == "OK")
            {
                //On ecrit dans le mouchard
                mouchard.evenement("Ré-initialisation des alarmes silencieuses.", Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log                        
                
                //On affiche l'icone que tout est ok puis on ferme la Form
                pictureBox1.BackgroundImage = SRegulV2.Properties.Resources.smiley_1ok;
                pictureBox1.Visible = true;
                timer1.Enabled = true;
            }
            else
            {
                pictureBox1.BackgroundImage = SRegulV2.Properties.Resources.Smiley_Embete1;
                pictureBox1.Visible = true;
            }                          
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
