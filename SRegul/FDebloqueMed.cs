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
    public partial class FDebloqueMed : Form
    {
        public string IdStatusGarde = "-1";
        public string NomMedecin = "";
        public DataTable dtMedecin;

        public FDebloqueMed()
        {
            InitializeComponent();

            //Pour la liste des médecins "Actifs"
            listViewMed.Columns.Add("IdStatusGarde", 1);         //Colonne invisible
            listViewMed.Columns.Add("Médecin", 150);
            listViewMed.View = View.Details;    //Pour afficher les subItems  

            IdStatusGarde = "-1";
            NomMedecin = "";
        }

        private void FDebloqueMed_Load(object sender, EventArgs e)
        {
            //Au chargement, on charge la liste qui auraient un état non cohérent (Attribuée, Acquitée, Visite) sans visite derrière
            //On vide la liste pour la rafraichir                
            listViewMed.BeginUpdate();
            listViewMed.Items.Clear();

            dtMedecin = FonctionsAppels.RechercheIncoherence();

            for (int i = 0; i < dtMedecin.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtMedecin.Rows[i]["IdStatusGarde"].ToString());
                item.SubItems.Add(dtMedecin.Rows[i]["Nom"].ToString() + " " + dtMedecin.Rows[i]["Prenom"].ToString());
                listViewMed.Items.Add(item);
            }

            listViewMed.EndUpdate();  //Rafraichi le controle
                                      //
            bDebloquer.Enabled = false;
        }

        private void listViewMed_DoubleClick(object sender, EventArgs e)
        {
            int index = listViewMed.SelectedItems[0].Index;

            //On peuple les variables       
            IdStatusGarde = listViewMed.SelectedItems[0].Text;
            NomMedecin = listViewMed.SelectedItems[0].SubItems[1].Text;

            tBoxMedecin.Text = NomMedecin;

            bDebloquer.Enabled = true;
        }

        private void bDebloquer_Click(object sender, EventArgs e)
        {
            //On débloque le médecin
            FonctionsAppels.RemetDispoIncoherence(IdStatusGarde);

            //on rafraichi la liste des incohérences
            FDebloqueMed_Load(sender, e);
        }

        private void bFermer_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
