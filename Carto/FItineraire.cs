using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CartoGeolocMedecins
{
    public partial class FItineraire : Form
    {
        public string adrDepart = "";
        public string adrArriver = "";
        public FItineraire()
        {
            InitializeComponent();
        }

        private void BGeoloc_Click(object sender, EventArgs e)
        {
            adrDepart = tbRueDepart.Text + "," + tbVilleDepart.Text + "," + tbCpDepart.Text + "," + tbPaysDepart.Text;
            adrArriver = tbRueArrive.Text + "," + tbVilleArrive.Text + "," + tbCpArrive.Text + "," + tbPaysArrive.Text;

            if (adrDepart != "")
                this.DialogResult = DialogResult.OK;
            else this.DialogResult = DialogResult.Cancel;
        }

        private void BGeoloc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;// j'ai mis cette ligne sinon sa fait "Ding" quand on appuie sur entrer
                BGeoloc_Click(sender, e);
            }
        }
    }
}
