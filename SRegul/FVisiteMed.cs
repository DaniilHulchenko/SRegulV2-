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
    public partial class FVisiteMed : Form
    {
        public string CodeMedecin = "-1";
        private static DataTable dtListeVisitesMed = new DataTable();

        public FVisiteMed()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("CodeMedecin", 1);         //Colonne invisible            
            listView1.Columns.Add("Nom/Prénom", 200);
            listView1.Columns.Add("Adresse", 250);
            listView1.Columns.Add("Status", 100);
            listView1.Columns.Add("Date/Heure du status", 140);
            listView1.Columns.Add("N° de Visite", 60);
            listView1.View = View.Details;    //Pour afficher les subItems  
        }

        private void FVisiteMed_Load(object sender, EventArgs e)
        {
            //on charge les viste en cours et pré-attribuées à ce médecin
            dtListeVisitesMed.Clear();
            dtListeVisitesMed = FonctionsAppels.ListeVisitesAttribPreAttrib(CodeMedecin);

            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtListeVisitesMed.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtListeVisitesMed.Rows[i]["CodeMedecin"].ToString());
                item.SubItems.Add(dtListeVisitesMed.Rows[i]["Nom"].ToString() + " " + dtListeVisitesMed.Rows[i]["Prenom"].ToString());
                item.SubItems.Add(dtListeVisitesMed.Rows[i]["Adr1"].ToString() + " " + dtListeVisitesMed.Rows[i]["Num_Rue"].ToString() + " "
                                  + dtListeVisitesMed.Rows[i]["CodePostal"].ToString() + " " + dtListeVisitesMed.Rows[i]["Commune"].ToString());
                item.SubItems.Add(dtListeVisitesMed.Rows[i]["Status"].ToString());
                item.SubItems.Add(dtListeVisitesMed.Rows[i]["DateStatus"].ToString());
                item.SubItems.Add(dtListeVisitesMed.Rows[i]["Num_Appel"].ToString());

                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle    
           
            //On désactive tout les boutons
            bReprise.Enabled = false; bAcquitement.Enabled = false; bEntreeVisite.Enabled = false; bAnnulEV.Enabled = false; bFinVisite.Enabled = false;

        }

        //Sélection d'une visite dans la liste
        private void listView1_Click(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            switch(listView1.SelectedItems[0].SubItems[3].Text)
            {
                case "PR": bReprise.Enabled = true; bAcquitement.Enabled = false; bEntreeVisite.Enabled = false; bAnnulEV.Enabled = false; bFinVisite.Enabled = false; break;
                case "AT": bReprise.Enabled = true; bAcquitement.Enabled = true; bEntreeVisite.Enabled = false; bAnnulEV.Enabled = false; bFinVisite.Enabled = false; break;
                case "AQ": bReprise.Enabled = true; bAcquitement.Enabled = false; bEntreeVisite.Enabled = true; bAnnulEV.Enabled = false; bFinVisite.Enabled = false; break;
                case "V": bReprise.Enabled = true; bAcquitement.Enabled = false; bEntreeVisite.Enabled = false; bAnnulEV.Enabled = true; bFinVisite.Enabled = true; break;               
            }
        }

        #region Gestion des boutons
        //On acquite la visite par le regulateur
        private void bAcquitement_Click(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            string CodeMedecin = listView1.SelectedItems[0].Text;
            string Num_Appel = listView1.SelectedItems[0].SubItems[5].Text;

            if (FonctionsAppels.AcquitementVisite(Num_Appel, CodeMedecin) == "OK")
            {
                //On affecte la fiche de consultation à ce médecin
                FonctionsAppels.AttribueFicheAQ(int.Parse(Num_Appel), int.Parse(CodeMedecin));
                
                //On rafraichi la liste
                FVisiteMed_Load(sender, e);
            }
            else
                MessageBox.Show("Erreur lors de l'acquitement de la visite. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //On Entre en visite par le regulateur
        private void bEntreeVisite_Click(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            string CodeMedecin = listView1.SelectedItems[0].Text;
            string Num_Appel = listView1.SelectedItems[0].SubItems[5].Text;

            if (FonctionsAppels.DebutVisite(Num_Appel, CodeMedecin) == "OK")
            {
                //On rafraichi la liste
                FVisiteMed_Load(sender, e);
            }
            else
                MessageBox.Show("Erreur lors de la mise En début de visite. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //On annule l'entrée en visite par le régulateur
        private void bAnnulEV_Click(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            string CodeMedecin = listView1.SelectedItems[0].Text;
            string Num_Appel = listView1.SelectedItems[0].SubItems[5].Text;

            if (FonctionsAppels.AnnuleDebutVisite(Num_Appel, CodeMedecin) == "OK")
            {
                //On rafraichi la liste
                FVisiteMed_Load(sender, e);
            }
            else
                MessageBox.Show("Erreur lors de l'annulation de l'entrée en visite. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //On fait la fin de visite
        private void bFinVisite_Click(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            string CodeMedecin = listView1.SelectedItems[0].Text;
            string Num_Appel = listView1.SelectedItems[0].SubItems[5].Text;

            if (FonctionsAppels.FinVisite(Num_Appel, CodeMedecin) == "OK")
            {
                //On rafraichi la liste
                FVisiteMed_Load(sender, e);
            }
            else
                MessageBox.Show("Erreur lors de la fin de visite. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //On reprend la visite
        private void bReprise_Click(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            string CodeMedecin = listView1.SelectedItems[0].Text;
            string Status = listView1.SelectedItems[0].SubItems[3].Text;
            string Num_Appel = listView1.SelectedItems[0].SubItems[5].Text;                      

            if (FonctionsAppels.DesattribuerVisite(Num_Appel, CodeMedecin, Status) == "OK")
            {
                //On rafraichi la liste
                FVisiteMed_Load(sender, e);
            }
            else
                MessageBox.Show("Erreur lors de la reprise de la visite. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }       
        #endregion

        private void bFermer_Click(object sender, EventArgs e)
        {            
            this.Close();
        }
    }
}


//A faire: