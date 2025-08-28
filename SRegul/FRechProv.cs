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
    public partial class FRechProv : Form
    {
       // public static DataTable dtProvenance = new DataTable();

        //Pour retourner le résultat de la recherche
        private string IdProvenance;
        public string idProvenance
        {
            get { return IdProvenance; }
            set { IdProvenance = value; }
        }

        private string Libelle;
        public string libelle
        {
            get { return Libelle; }
            set { Libelle = value; }
        }

        public FRechProv()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("IdProvenance", 1);         //Colonne invisible
            listView1.Columns.Add("Libelle", 300);            
            listView1.View = View.Details;    //Pour afficher les subItems  

            IdProvenance = "0";
            Libelle = "";
        }

        private void FRecherche_Load(object sender, EventArgs e)
        {
            //On charge toute les provenances           
            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < Form1.dtProv.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(Form1.dtProv.Rows[i]["IdProvenance"].ToString());
                item.SubItems.Add(Form1.dtProv.Rows[i]["LibelleProvenance"].ToString());                
                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle        
        }

        private void tBProvenance_TextChanged(object sender, EventArgs e)
        {
            //on lance une recherche avec ce qui a été entré
            string SqlSelect = "LibelleProvenance like '" +  tBProvenance.Text + "%'";
            string Trie = "LibelleProvenance";

            if (Form1.dtProv.Select(SqlSelect, Trie).Any())    //Si on a quelque chose
            {
                DataTable dtProvTrie = Form1.dtProv.Select(SqlSelect, Trie).CopyToDataTable();

                listView1.BeginUpdate();
                listView1.Items.Clear();
             
                //On charge la liste
                for (int i = 0; i < dtProvTrie.Rows.Count; i++)
                {
                    ListViewItem item1 = new ListViewItem(dtProvTrie.Rows[i]["IdProvenance"].ToString());
                    item1.SubItems.Add(dtProvTrie.Rows[i]["LibelleProvenance"].ToString());                   

                    listView1.Items.Add(item1);
                }

                listView1.EndUpdate();  //Rafraichi le controle 
            }                       

        }

        //On sélectionne la ligne puis on ferme la form
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;
            
            //On peuple les variables       
            IdProvenance = listView1.SelectedItems[0].Text;
            Libelle = listView1.SelectedItems[0].SubItems[1].Text;

            //On détermine la réponse de retour
            DialogResult = System.Windows.Forms.DialogResult.OK;               
            
            //Puis on ferme la form
            this.Close();            
        }

        private void tBProvenance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {                
                //On selectionne la première ligne de la liste (s'il y a qqchose), puis on ferme
                if (listView1.Items.Count > 0)
                {
                    //On peuple les variables       
                    IdProvenance = listView1.Items[0].Text;
                    Libelle = listView1.Items[0].SubItems[1].Text;

                    //On détermine la réponse de retour
                    DialogResult = System.Windows.Forms.DialogResult.OK;         
                }
                else
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;

                //Puis on ferme la form
                this.Close();                                                    
            }
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            //On ferme en envoyant cancel
            DialogResult = System.Windows.Forms.DialogResult.Cancel;

            //Puis on ferme la form
            this.Close();        
        }

      
    }
}


//A faire: