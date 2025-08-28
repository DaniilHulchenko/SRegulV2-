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
    public partial class FListeVisiteOrg : Form
    {
        public string CodeMedOrg = "-1";
        private static DataTable dtListeVisitesOrg = new DataTable();
        private static DataTable dtMedOrg = new DataTable();

        public FListeVisiteOrg()
        {
            InitializeComponent();

            //Def des colonnes du Listeview1
            listView1.Columns.Add("Code med Org.", 1);
            listView1.Columns.Add("Organisation", 100);
            listView1.Columns.Add("Nom Prenom", 250);           
            listView1.View = View.Details;    //Pour afficher les subItems    


            //Def des colonnes du Listeview2
            listView2.Columns.Add("CodeMedOrg", 1);         //Colonne invisible            
            listView2.Columns.Add("Nom/Prénom", 200);
            listView2.Columns.Add("Adresse", 350);
            listView2.Columns.Add("Status", 60);
            listView2.Columns.Add("Date/Heure du status", 160);
            listView2.Columns.Add("N° de Visite", 90);
            listView2.View = View.Details;    //Pour afficher les subItems  
        }

        private void FListeVisiteOrg_Load(object sender, EventArgs e)
        {
            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            dtMedOrg = FonctionsAppels.ChargeListeInfirmiere("En garde avec visite");

            for (int i = 0; i < dtMedOrg.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtMedOrg.Rows[i]["CodeMedecin"].ToString());
                item.SubItems.Add(dtMedOrg.Rows[i]["LibelleOrg"].ToString());
                item.SubItems.Add(dtMedOrg.Rows[i]["NomMedecin"].ToString());                
                listView1.Items.Add(item);
            }
           
            listView1.EndUpdate();  //Rafraichi le controle        

            //On désactive tout les boutons
            bReprise.Enabled = false; bEntreeVisite.Enabled = false; bAnnulEV.Enabled = false; bFinVisite.Enabled = false;
        }

        //Sélection d'une infirmière dans la selection
        private void listView1_Click(object sender, EventArgs e)
        {
            listView2Refresh(listView1.SelectedItems[0].Text);
        }

        
        private void listView2Refresh(string CodeMedOrg)
        {
            //on charge les viste en cours et pré-attribuées à ce médecin
            dtListeVisitesOrg.Clear();
            dtListeVisitesOrg = FonctionsAppels.ListVisitAttribPreAtOrg(CodeMedOrg);

            //On vide la liste pour la rafraichir                
            listView2.BeginUpdate();
            listView2.Items.Clear();

            for (int i = 0; i < dtListeVisitesOrg.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtListeVisitesOrg.Rows[i]["CodeMedecin"].ToString());
                item.SubItems.Add(dtListeVisitesOrg.Rows[i]["Nom"].ToString() + " " + dtListeVisitesOrg.Rows[i]["Prenom"].ToString());
                item.SubItems.Add(dtListeVisitesOrg.Rows[i]["Adr1"].ToString() + " " + dtListeVisitesOrg.Rows[i]["Num_Rue"].ToString() + " "
                                  + dtListeVisitesOrg.Rows[i]["CodePostal"].ToString() + " " + dtListeVisitesOrg.Rows[i]["Commune"].ToString());
                item.SubItems.Add(dtListeVisitesOrg.Rows[i]["Status"].ToString());
                item.SubItems.Add(dtListeVisitesOrg.Rows[i]["DateStatus"].ToString());
                item.SubItems.Add(dtListeVisitesOrg.Rows[i]["Num_Appel"].ToString());

                listView2.Items.Add(item);
            }

            listView2.EndUpdate();  //Rafraichi le controle    

            //On désactive tout les boutons
            bReprise.Enabled = false; bEntreeVisite.Enabled = false; bAnnulEV.Enabled = false; bFinVisite.Enabled = false;
        }

        private void listView2_Click(object sender, EventArgs e)
        {
            //int index = listView2.SelectedItems[0].Index;

            switch (listView2.SelectedItems[0].SubItems[3].Text)
            {
                case "PR": bReprise.Enabled = true; bEntreeVisite.Enabled = false; bAnnulEV.Enabled = false; bFinVisite.Enabled = false; break;
                case "AT": bReprise.Enabled = true; bEntreeVisite.Enabled = true; bAnnulEV.Enabled = false; bFinVisite.Enabled = false; break;                
                case "V": bReprise.Enabled = true;  bEntreeVisite.Enabled = false; bAnnulEV.Enabled = true; bFinVisite.Enabled = true; break;
            }
        }

        #region Gestion des boutons

        //On Entre en visite par le regulateur
        private void bEntreeVisite_Click(object sender, EventArgs e)
        {
            //int index = listView1.SelectedItems[0].Index;

            string CodeMedOrg = listView2.SelectedItems[0].Text;
            string Num_Appel = listView2.SelectedItems[0].SubItems[5].Text;

            if (FonctionsAppels.DebutVisiteOrg(Num_Appel, CodeMedOrg) == "OK")
            {
                //On rafraichi la liste
                listView2Refresh(CodeMedOrg);
            }
            else
                MessageBox.Show("Erreur lors de la mise en visite. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion


        //On annule l'entrée en visite par le régulateur
        private void bAnnulEV_Click(object sender, EventArgs e)
        {            
            string CodeMedOrg = listView2.SelectedItems[0].Text;
            string Num_Appel = listView2.SelectedItems[0].SubItems[5].Text;

            if (FonctionsAppels.AnnuleDebutVisiteOrg(Num_Appel, CodeMedOrg) == "OK")
            {                
                //On rafraichi la liste
                listView2Refresh(CodeMedOrg);
            }
            else
                MessageBox.Show("Erreur lors de l'annulation de l'entrée en visite. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }

        //On fait la fin de visite
        private void bFinVisite_Click(object sender, EventArgs e)
        {
            string CodeMedOrg = listView2.SelectedItems[0].Text;
            string Num_Appel = listView2.SelectedItems[0].SubItems[5].Text;

            if (FonctionsAppels.FinVisiteOrg(Num_Appel, CodeMedOrg) == "OK")
            {
                //On rafraichi la liste
                listView2Refresh(CodeMedOrg);
            }
            else
                MessageBox.Show("Erreur lors de la fin de visite. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //On reprend la visite
        private void bReprise_Click(object sender, EventArgs e)
        {
            string CodeMedOrg = listView2.SelectedItems[0].Text;
            string Num_Appel = listView2.SelectedItems[0].SubItems[5].Text;            
            string Status = listView2.SelectedItems[0].SubItems[3].Text;


            if (FonctionsAppels.DesattribuerVisiteOrg(Num_Appel, CodeMedOrg, Status) == "OK")
            {
                //On rafraichi la liste
                listView2Refresh(CodeMedOrg);
            }
            else
                MessageBox.Show("Erreur lors de la reprise de la visite. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);           

        }

        private void bFermer_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
