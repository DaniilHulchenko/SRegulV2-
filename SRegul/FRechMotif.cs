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
    public partial class FRechMotif : Form
    {
        //Pour retourner le résultat de la recherche
        private string Motif;
        public string motif
        {
            get { return Motif; }
            set { Motif = value; }
        }
                               
        private string IdMotif;
        public string idMotif
        {
            get { return IdMotif; }
            set { IdMotif = value; }
        }

        private string LibelleMotif;
        public string libelleMotif
        {
            get { return LibelleMotif; }
            set { LibelleMotif = value; }
        }

        private string TypeMotif;
        public string typeMotif
        {
            get { return TypeMotif; }
            set { TypeMotif = value; }
        }

        public FRechMotif()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("Id Motif", 1);    //Colonne invisible           
            listView1.Columns.Add("Libelle", 300);
            listView1.Columns.Add("Type", 1);
            listView1.View = View.Details;    //Pour afficher les subItems  

            Motif = "";
            IdMotif = "0";
            LibelleMotif = "";
            TypeMotif = "";           
        }

        private void FRechMotif_Load(object sender, EventArgs e)
        {
            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            //En fonction du type de motif (Appel ou annulation)
            if (Motif != "MotifAnnulation")
            {
                for (int i = 0; i < Form1.dtMotif.Rows.Count; i++)
                {
                    ListViewItem item = new ListViewItem(Form1.dtMotif.Rows[i]["IdMotif"].ToString());
                    item.SubItems.Add(Form1.dtMotif.Rows[i]["LibelleMotif"].ToString());
                    item.SubItems.Add(Form1.dtMotif.Rows[i]["TypeMotif"].ToString());

                    listView1.Items.Add(item);
                }
            }
            else
            {
                for (int i = 0; i < Form1.dtMotifAnnul.Rows.Count; i++)
                {
                    ListViewItem item = new ListViewItem(Form1.dtMotifAnnul.Rows[i]["IdMotif"].ToString());
                    item.SubItems.Add(Form1.dtMotifAnnul.Rows[i]["LibelleMotif"].ToString());
                    item.SubItems.Add(Form1.dtMotifAnnul.Rows[i]["TypeMotif"].ToString());

                    listView1.Items.Add(item);
                }
            }

            listView1.EndUpdate();  //Rafraichi le controle      

            tBMotif.Focus();
        }

        private void tBMotif_TextChanged(object sender, EventArgs e)
        {
            //on lance une recherche avec ce qui a été entré
            string SqlSelect = "";
            string Trie = "";
            
            switch(Motif)
            {
                case "Motif1":
                case "Motif2": SqlSelect = "LibelleMotif LIKE '" + tBMotif.Text.Replace("'","''") + "%' AND TypeMotif = 'V'";
                               Trie = "LibelleMotif";
                               break;

                case "MotifAnnulation": SqlSelect = "LibelleMotif LIKE '" + tBMotif.Text.Replace("'", "''") + "%' AND TypeMotif = 'A'";
                                        Trie = "LibelleMotif";
                                        break;
            }
          
            if (Motif != "MotifAnnulation")
            {
                if (Form1.dtMotif.Select(SqlSelect, Trie).Any())    //Si on a quelque chose
                {
                    DataTable dtMotifTrie = Form1.dtMotif.Select(SqlSelect, Trie).CopyToDataTable();

                    listView1.BeginUpdate();
                    listView1.Items.Clear();

                    //On charge les Motifs
                    for (int i = 0; i < dtMotifTrie.Rows.Count; i++)
                    {
                        ListViewItem item1 = new ListViewItem(dtMotifTrie.Rows[i]["IdMotif"].ToString());
                        item1.SubItems.Add(dtMotifTrie.Rows[i]["LibelleMotif"].ToString());
                        item1.SubItems.Add(dtMotifTrie.Rows[i]["TypeMotif"].ToString());

                        listView1.Items.Add(item1);
                    }

                    listView1.EndUpdate();  //Rafraichi le controle 
                }
            }
            else
            {
                if (Form1.dtMotifAnnul.Select(SqlSelect, Trie).Any())    //Si on a quelque chose
                {
                    DataTable dtMotifTrie = Form1.dtMotifAnnul.Select(SqlSelect, Trie).CopyToDataTable();

                    listView1.BeginUpdate();
                    listView1.Items.Clear();

                    //On charge les Motifs
                    for (int i = 0; i < dtMotifTrie.Rows.Count; i++)
                    {
                        ListViewItem item1 = new ListViewItem(dtMotifTrie.Rows[i]["IdMotif"].ToString());
                        item1.SubItems.Add(dtMotifTrie.Rows[i]["LibelleMotif"].ToString());
                        item1.SubItems.Add(dtMotifTrie.Rows[i]["TypeMotif"].ToString());

                        listView1.Items.Add(item1);
                    }

                    listView1.EndUpdate();  //Rafraichi le controle 
                }
            }
            
        }


        //On sélectionne la ligne puis on ferme la form
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;
          
            //On peuple les variables                      
            IdMotif = listView1.SelectedItems[0].Text;
            LibelleMotif = listView1.SelectedItems[0].SubItems[1].Text;
            TypeMotif = listView1.SelectedItems[0].SubItems[2].Text;

            //Puis on ferme la form
            DialogResult = System.Windows.Forms.DialogResult.OK;
           
            //Puis on ferme la form
            this.Close();            
        }


        private void tBMotif_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //On selectionne la première ligne de la liste (s'il y a qqchose), puis on ferme
                if (listView1.Items.Count > 0)
                {
                    //On peuple les variables       
                    IdMotif = listView1.Items[0].Text;
                    LibelleMotif = listView1.Items[0].SubItems[1].Text;
                    TypeMotif = listView1.Items[0].SubItems[2].Text;

                    //Puis on ferme la form
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
