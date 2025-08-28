using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FDialogRecherche : Form
    {
        //Pour le retour de la sélection à la fermeture de la form
        int selection = -1;
        public int Selection
        {
            get { return selection; }
            set { selection = value; }
        }


        public FDialogRecherche(DataTable Recherche)
        {
            InitializeComponent();

            listView1.Columns.Add("NomPrenom", 250);           
            listView1.Columns.Add("DateNaissance", 80);
            listView1.Columns.Add("Age", 70);
            listView1.Columns.Add("Sexe", 45);
            listView1.Columns.Add("Adresse", 200);   //Rue + N° Rue
            listView1.Columns.Add("Commune", 150);    //CP + Ville
            listView1.Columns.Add("TA", 1);    //TA
            listView1.Columns.Add("IdPersonne", 1);           
            listView1.View = View.Details;    //Pour afficher les subItems

            ChargeListe(Recherche);

        }


        private void ChargeListe(DataTable Rech)
        {
            //Si on a rien trouvé, on le signale
            if (Rech.Rows.Count == 0)
            {
                ListViewItem item = new ListViewItem("Aucun enregistrement trouvé");
                listView1.Items.Add(item);

                timer1.Enabled = true;   //On laisse 2 secondes avant de fermer la forme
            }
            else
            {
                //On charge la ListView
                for (int i = 0; i < Rech.Rows.Count; i++)
                {
                    ListViewItem item = new ListViewItem(Rech.Rows[i]["Nom"].ToString() + " " + Rech.Rows[i]["Prenom"].ToString());
                  
                    //formatage de la date de naissance => On enleve les heures
                    if (Rech.Rows[i]["DateNaissance"].ToString() != null && Rech.Rows[i]["DateNaissance"] != DBNull.Value)
                    {
                        try
                        {
                            //Date de naissance
                            DateTime DateNaiss = DateTime.ParseExact(Rech.Rows[i]["DateNaissance"].ToString(), "dd.MM.yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                            item.SubItems.Add(DateNaiss.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));

                            //Age
                            int age =FonctionsAppels.CalculeAge(DateNaiss);
                            item.SubItems.Add(age.ToString() + " ans");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            item.SubItems.Add("");   //DateNaiss
                            item.SubItems.Add("");   //Age
                        }
                    }
                    else
                    {
                        item.SubItems.Add("");   //DateNaiss
                        item.SubItems.Add("");   //Age
                    }

                    item.SubItems.Add(Rech.Rows[i]["Sexe"].ToString());

                    item.SubItems.Add(Rech.Rows[i]["Rue"].ToString() + " " + Rech.Rows[i]["NumeroDansRue"].ToString());
                    item.SubItems.Add(Rech.Rows[i]["CodePostal"].ToString() + " " + Rech.Rows[i]["Commune"].ToString());
                    item.SubItems.Add(Rech.Rows[i]["TeleAlarme"].ToString());
                    item.SubItems.Add(Rech.Rows[i]["IdPersonne"].ToString());
                    listView1.Items.Add(item);
                }
            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Passe 2 fois lors d'un changement de selection: 1ere Fois index = 0, 2eme fois index Ok
            try
            {
                if (listView1.SelectedItems.Count == 1)
                {                   
                    Console.WriteLine(listView1.SelectedItems[0].SubItems[1].Text);
                    Console.WriteLine(listView1.SelectedIndices[0].ToString());
                    //On retourne la selection et on ferme la form
                    selection = int.Parse(listView1.SelectedIndices[0].ToString());
                }                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }           

            //Si on a selectionné qqe chose, on ferme la form si non, non
            if (selection != -1)
            {
                this.Close();    //On ferme la form
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();    //On ferme la form
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Close();    //On ferme la form
        }

      
    }
}

