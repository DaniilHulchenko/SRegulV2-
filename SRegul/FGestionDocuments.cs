using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FGestionDocuments : Form
    {

        private string FichierAUploader = "";

        public FGestionDocuments()
        {
            InitializeComponent();

            listView1.Columns.Add("Nom du document", 250);
            listView1.View = View.Details;    //Pour afficher les subItems
        }

        private void FGestionDocuments_Load(object sender, EventArgs e)
        {
            //On charge la liste des documents
            chargeListeFichiers();
        }

        //chargement de la liste des fichiers
        private void chargeListeFichiers()
        {
            label1.Text = "Nom de fichier : ";

            string pathSource = ConfigurationManager.AppSettings["Path_DocumentsDivers"].ToString();    //Chemin où sont stockés les docs            
            DirectoryInfo repertoire = new DirectoryInfo(pathSource);

            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            listView1.EndUpdate();  //Rafraichi le controle        

            foreach (var file in repertoire.EnumerateFiles())
            {
                ListViewItem item = new ListViewItem(file.Name);
                listView1.Items.Add(item);
            }
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            //On affiche les infos du fichier
            label1.Text = "Nom de fichier : " + listView1.SelectedItems[0].Text;

            //Pour les boutons
            bAjouter.Enabled = true;
            bSupprimer.Enabled = true;
            bUploader.Visible = false;
        }
        

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Si on a selectionné quelque chose, on active ou pas les boutons
            if(listView1.SelectedItems.Count > 0)
            {
                bAjouter.Enabled = true;
                bSupprimer.Enabled = true;
                bUploader.Visible = false;
            }
            else
            {
                bAjouter.Enabled = true;
                bSupprimer.Enabled = false;
                bUploader.Visible = false;
            }
        }

        private void bSupprimer_Click(object sender, EventArgs e)
        {
            //On supprime le fichier selectionné
            if (listView1.SelectedItems.Count > 0 && listView1.SelectedItems[0].Text != "")
            {
                string pathSource = ConfigurationManager.AppSettings["Path_DocumentsDivers"].ToString();    //Chemin où sont stockés les docs        
                string fichier = Path.Combine(pathSource, listView1.SelectedItems[0].Text);

                //Si le fichier existe bien, on l'efface
                if (File.Exists(fichier))
                {
                    File.Delete(fichier);

                    //Puis on rafraichi la liste
                    chargeListeFichiers();
                }
            }
        }

        private void bAjouter_Click(object sender, EventArgs e)
        {
            //On ajoute un nouveau fichier                                           
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Fichier pdf (*.pdf) | *.pdf";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FichierAUploader = openFileDialog1.FileName;
                label1.Text = "Fichier à uploader :" + Path.GetFileName(FichierAUploader);

                //Pour les boutons
                bAjouter.Enabled = false;
                bSupprimer.Enabled = false;
                bUploader.Visible = true;
            }
            else
            {
                label1.Text = "Pas de fichier à uploader.";
            }
        }

        private void bUploader_Click(object sender, EventArgs e)
        {
            //On upload le fichier            
            if (FichierAUploader != "")
            {
                string pathSource = ConfigurationManager.AppSettings["Path_DocumentsDivers"].ToString();    //Chemin où sont stockés les docs                                                     

                string Extention = Path.GetExtension(FichierAUploader);
                string NomF = Path.GetFileNameWithoutExtension(FichierAUploader);               

                //On copie le document sur le serveur
                try
                {
                    File.Copy(Path.Combine(FichierAUploader), Path.Combine(pathSource, NomF + Extention), true);
                   
                    //Pour les boutons
                    bAjouter.Enabled = true;
                    bSupprimer.Enabled = false;
                    bUploader.Visible = false;

                    MessageBox.Show("Copie vers le serveur OK");                
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la copie du document...Le message est : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;   //On s'arrête là
                }
               
                //Puis on rafraichi la liste
                chargeListeFichiers();
            }
        }
    }
}
