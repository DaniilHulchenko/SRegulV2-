using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CartoGeolocMedecins
{
    public partial class FDialogue : Form
    {
        public string NvlleAdr = "";

        public FDialogue()
        {
            InitializeComponent();
        }

        private void bGeoloc_Click(object sender, EventArgs e)
        {
            //On 
            NvlleAdr = tbRue.Text + "," + tbCp.Text + "," + tbVille.Text + "," + tbPays.Text;

            if (NvlleAdr != "")
                this.DialogResult = DialogResult.OK;
            else this.DialogResult = DialogResult.Cancel;
        }

        //pour valider quand on appuie sur la touche entrer
        private void TbVille_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;// j'ai mis cette ligne sinon sa fait "Ding" quand on appuie sur entrer
                bGeoloc_Click(sender, e);
            }
        }
    }
}
