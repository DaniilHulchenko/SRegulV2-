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
    public partial class FFacturesImpayees : Form
    {

        public int NumPersonne;

        public FFacturesImpayees(int NumPers)
        {
            InitializeComponent();

            //Pour la liste des factures impayées
            listView1.Columns.Add("Num Facture", 120, HorizontalAlignment.Center);         //Colonne invisible
            listView1.Columns.Add("Date Facture", 150, HorizontalAlignment.Center);
            listView1.Columns.Add("Montant Facture", 150, HorizontalAlignment.Right);
            listView1.View = View.Details;    //Pour afficher les subItems  

            NumPersonne = NumPers;
        }

        private void FFacturesImpayees_Load(object sender, EventArgs e)
        {
            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            DataTable dtFacture = FonctionsAppels.GetFacturesImpayees(NumPersonne);

            for (int i = 0; i < dtFacture.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtFacture.Rows[i]["NFacture"].ToString());
                item.SubItems.Add(dtFacture.Rows[i]["DateFacture"].ToString());
                item.SubItems.Add(dtFacture.Rows[i]["MONTANT_DU"].ToString() + " CHF");
                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle        
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            //On ferme en envoyant cancel
            DialogResult = DialogResult.Cancel;

            //Puis on ferme la form
            this.Close();
        }
    }
}
