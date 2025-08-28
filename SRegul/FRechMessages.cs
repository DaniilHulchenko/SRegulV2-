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
    public partial class FRechMessages : Form
    {
        public FRechMessages()
        {
            InitializeComponent();

            //Liste des messages
            listView1.Columns.Add("N° Mess", 1);     //Colonne invisible
            listView1.Columns.Add("Auteur", 160);
            listView1.Columns.Add("Message", 450);
            listView1.Columns.Add("Destinataire", 160);          
            listView1.Columns.Add("PieceJointe", 160);
            listView1.Columns.Add("DateMessage", 160);
            listView1.View = View.Details;    //Pour afficher les subItems    
        }

        private void bRecherche_Click(object sender, EventArgs e)
        {
            DataTable dtMessages = new DataTable();
            dtMessages = FonctionsAppels.RechercheMessages(dTimePDeb.Value, dTimePFin.Value);

            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtMessages.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtMessages.Rows[i]["IdMessage"].ToString());
                item.SubItems.Add(dtMessages.Rows[i]["NomAuteur"].ToString());
                item.SubItems.Add(dtMessages.Rows[i]["Message"].ToString());
                item.SubItems.Add(dtMessages.Rows[i]["NomDestinataire"].ToString());               
                item.SubItems.Add(dtMessages.Rows[i]["PieceJointe"].ToString());
                item.SubItems.Add(dtMessages.Rows[i]["DateM"].ToString());
                listView1.Items.Add(item);                                               
            }

            listView1.EndUpdate();  //Rafraichi le controle    
        }


        //On regarde s'il y a une pièce jointe
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {            
        }


        private void bExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {                                    
            //On récupère la position du pointe de la souris dans le contrôle
            Point mousePos = listView1.PointToClient(Control.MousePosition);
            ListViewHitTestInfo hitTest = listView1.HitTest(mousePos);
            
            //On récupère l'index de la colonne ou est le pointeur de la souris
            int columnIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);

            //Si on a cliqué sur la pièce jointe et qu'il y a qqchose
            if (columnIndex == 4 && hitTest.SubItem.Text != "")
            {
                //Récup du Temp de l'utilisateur
                string DossierTempUtilisateur = Path.GetTempPath(); 
                string pathSource = ConfigurationManager.AppSettings["Path_PiecesJointes"].ToString();    //Serveur où sont les pieces jointes                                               
                string Source = pathSource + "\\" + hitTest.SubItem.Text;
                string Destination = DossierTempUtilisateur + "\\" + hitTest.SubItem.Text;

                //Le fichier existe il encore sur le serveur?
                 if (File.Exists(Source))
                 {
                    //OK donc on copie la pièce jointe sur le Temp de l'utilisateur
                    try
                    {
                        File.Copy(Source, Destination, true); 
                        
                        //Puis on l'ouvre
                        System.Diagnostics.Process.Start(Destination);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Erreur lors de l'ouverture de la pièce jointe...Le message est : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;   //On s'arrête là
                    }                     
                 }
                 else
                     MessageBox.Show("La pièce jointe est introuvable.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                        
            }            
        }
       

       
    }
}
