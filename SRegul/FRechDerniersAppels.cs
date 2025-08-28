using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FRechDerniersAppels : Form
    {

        private DataTable DerniersAppels = new DataTable();
        
        public FRechDerniersAppels(DataTable dt20DerAppels)
        {
            InitializeComponent();

            listView1.Columns.Add("Date Appel", 150);
            listView1.Columns.Add("Médecin", 160);
            listView1.Columns.Add("Motif appel", 100);         
            listView1.Columns.Add("NConsultation", 1);   //Colonne cachée 
            listView1.View = View.Details;    //Pour afficher les subItems

            DerniersAppels = dt20DerAppels;
        }

        private void FRechDerniersAppels_Load(object sender, EventArgs e)
        {
            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < DerniersAppels.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(FonctionsAppels.convertDateMaria(DerniersAppels.Rows[i]["DateOp"].ToString(), "Texte"));
                item.SubItems.Add(DerniersAppels.Rows[i]["NomP"].ToString());
                item.SubItems.Add(DerniersAppels.Rows[i]["LibelleMotif"].ToString());         
                item.SubItems.Add(DerniersAppels.Rows[i]["NConsultation"].ToString());
                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle        
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            //Puis on ferme la form
            this.Close();      
        }


        private void RechercheRapport()
        {
            rTBoxRapport.Text = "";     //On efface le précédent rapport

            //On recherche les infos du rapport
            DataTable dtRapport = new DataTable();
            Int32 NConsultation = Int32.Parse(listView1.SelectedItems[0].SubItems[3].Text);

            dtRapport = FonctionsAppels.RetourneRapport(NConsultation);

            if (dtRapport.Rows.Count > 0)
            {
                //On affiche les différentes rubriques du rapport                                           
                rTBoxRapport.AppendText("Concerne " + dtRapport.Rows[0]["RapConcerne"].ToString() + "\r\n \r\n");
                rTBoxRapport.AppendText("pour la visite du " + dtRapport.Rows[0]["DateRapport"].ToString() + "\r\n \r\n");
                rTBoxRapport.AppendText("Vu par le Dr " + dtRapport.Rows[0]["Nom"].ToString() + "\r\n \r\n");

                //On charge les paragraphes du rapport
                for (int i = 0; i < dtRapport.Rows.Count; i++)
                {
                    //On met en gras le titre des libellés
                    string Titre = dtRapport.Rows[i]["LibelleCategorie"].ToString() + ":";
                    int longueur = rTBoxRapport.Text.Length;

                    rTBoxRapport.AppendText(Titre + "\r\n");

                    //On selectionne le titre (qu'on vient d'ajouter a la box)
                    rTBoxRapport.Select(longueur, Titre.Length);

                    //On détermine les paramètres de la Font
                    rTBoxRapport.SelectionFont = new Font(rTBoxRapport.Font, FontStyle.Bold);
                    // rTBoxRapport.SelectionColor = Color.Red;                                                                      

                    //Ajour du contenu de la rubrique
                    rTBoxRapport.AppendText(dtRapport.Rows[i]["ValeurCategorie"].ToString() + "\r\n \r\n");
                }
            }
            else
            {
                //On affiche "PAS DE RAPPORT" pour cet appel et le motif d'annulation (s'il y en a un)

                string MotifAnnulation = FonctionsAppels.RetourneMotifAnnulationV(NConsultation);

                rTBoxRapport.Text = "\n\n\n\n\n\n\n\rPAS DE RAPPORT POUR CET APPEL\n\r" + MotifAnnulation;
                //On détermine les paramètres de la Font et l'alignement 
                rTBoxRapport.SelectAll();
                rTBoxRapport.SelectionFont = new Font(rTBoxRapport.Font, FontStyle.Bold);
                rTBoxRapport.SelectionAlignment = HorizontalAlignment.Center;
                rTBoxRapport.DeselectAll();
            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            RechercheRapport();
        }

        //click droit de la souris, on affiche la fiche de visite
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {            
            if (e.Button == MouseButtons.Right)
            {                
                //On affiche la fiche de la visite
                if (listView1.SelectedItems[0].SubItems[1].Text != "-1")
                {
                    FVisite fVisite = new FVisite();
                    fVisite.NumVisite = Int32.Parse(listView1.SelectedItems[0].SubItems[3].Text);
                    fVisite.provenance = "RechDerniersAppels";
                    fVisite.ShowDialog(this);
                    fVisite.Dispose();
                }                            
            }
        }
       
        //Flèches haut / Bas
        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                RechercheRapport();
            }
        }
    }
}
