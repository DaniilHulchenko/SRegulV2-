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
    public partial class FRechCommune : Form
    {       
        //Pour retourner le résultat de la recherche       
        private string IdCommune;
        public string idCommune
        {
            get { return IdCommune; }
            set { IdCommune = value; }
        }

        private string NomCommune;
        public string nomCommune
        {
            get { return NomCommune; }
            set { NomCommune = value; }
        }

        private string CodePostal;
        public string codePostal
        {
            get { return CodePostal; }
            set { CodePostal = value; }
        }

        private string Pays;
        public string pays
        {
            get { return Pays; }
            set { Pays = value; }
        }        

        public FRechCommune()
        {
            InitializeComponent();
          
            //Def des colonnes du Listeview
            listView1.Columns.Add("IdCommune", 1);         //Colonne invisible
            listView1.Columns.Add("Nom de la Commune", 200);
            listView1.Columns.Add("Code Postal", 60);  
            listView1.View = View.Details;    //Pour afficher les subItems  
           
            IdCommune = "0";
            NomCommune = "";
            CodePostal = "";
        }

        private void FRechCommune_Load(object sender, EventArgs e)
        {
            //On charge toute les communes           
            //On vide la liste pour la rafraichir                
          /*  listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < Form1.dtCommune.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(Form1.dtCommune.Rows[i]["IdCommune"].ToString());
                item.SubItems.Add(Form1.dtCommune.Rows[i]["NomCommune"].ToString());
                item.SubItems.Add(Form1.dtCommune.Rows[i]["CodePostal"].ToString());
                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle     */
            //On donne le focus a la commune
            this.ActiveControl = tBCommune;                 
        }

        private void tBCommune_TextChanged(object sender, EventArgs e)
        {
            //on lance une recherche avec ce qui a été entré           
            string SqlSelect = "NomCommune like '%" + tBCommune.Text.Replace("'", "''") + "%'";
            string Trie = "NomCommune";

            if (Form1.dtCommune.Select(SqlSelect, Trie).Any())    //Si on a quelque chose
            {
                DataTable dtCommuneTrie = Form1.dtCommune.Select(SqlSelect, Trie).CopyToDataTable();

                listView1.BeginUpdate();
                listView1.Items.Clear();
                
                //On charge la liste
                for (int i = 0; i < dtCommuneTrie.Rows.Count; i++)
                {
                    ListViewItem item1 = new ListViewItem(dtCommuneTrie.Rows[i]["IdCommune"].ToString());
                    item1.SubItems.Add(dtCommuneTrie.Rows[i]["NomCommune"].ToString());
                    item1.SubItems.Add(dtCommuneTrie.Rows[i]["CodePostal"].ToString());

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
            IdCommune = listView1.SelectedItems[0].Text;
            NomCommune = listView1.SelectedItems[0].SubItems[1].Text;
            CodePostal = listView1.SelectedItems[0].SubItems[2].Text;

            //Puis on ferme la form
            DialogResult = DialogResult.OK;
         
            //Puis on ferme la form
            this.Close();            
        }

        private void tBCommune_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //On selectionne la première ligne de la liste (s'il y a qqchose), puis on ferme
                if (listView1.Items.Count > 0)
                {
                    //On peuple les variables       
                    IdCommune = listView1.Items[0].Text;
                    NomCommune = listView1.Items[0].SubItems[1].Text;
                    CodePostal = listView1.Items[0].SubItems[2].Text;

                    //Puis on ferme la form
                    DialogResult = DialogResult.OK;
                }
                else
                    DialogResult = DialogResult.Cancel;

                //Puis on ferme la form
                this.Close();
            }
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            //Puis on ferme la form
            this.Close();
        }
    }
}
