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
    public partial class FRechAssurance : Form
    {
        //Pour retourner le résultat de la recherche             
        private string NomAssurance;
        public string nomAssurance
        {
            get { return NomAssurance; }
            set { NomAssurance = value; }
        }

        public FRechAssurance()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("IdAssurance", 1);         //Colonne invisible
            listView1.Columns.Add("Assurance", 250);           
            listView1.View = View.Details;    //Pour afficher les subItems  

            //On initialise l'assurance Saisie dans Visite ou Re-saisie            
            NomAssurance = "";
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;

            //Puis on ferme la form
            this.Close();
        }

        private void FRechAssurance_Load(object sender, EventArgs e)
        {
            //On donne le focus a la rue
            this.ActiveControl = tBAssurance;
        }

        private void tBAssurance_TextChanged(object sender, EventArgs e)
        {
            //Critères de selection
            string SqlSelect = "";
            string Trie = "";

            //On affiche toute les assurances correspondantes
            SqlSelect = "NomAssurance like '%" + tBAssurance.Text.Replace("'", "''") + "%'";
            Trie = "NomAssurance";
            
            //on lance une recherche avec ce qui a été entré      
            if (Form1.dtAssurance.Select(SqlSelect, Trie).Any())    //Si on a quelque chose
            {
                DataTable dtAssuranceTrie = Form1.dtAssurance.Select(SqlSelect, Trie).CopyToDataTable();

                listView1.BeginUpdate();
                listView1.Items.Clear();

                //On charge la liste
                for (int i = 0; i < dtAssuranceTrie.Rows.Count; i++)
                {
                    ListViewItem item1 = new ListViewItem(dtAssuranceTrie.Rows[i]["IdAssurance"].ToString());
                    item1.SubItems.Add(dtAssuranceTrie.Rows[i]["NomAssurance"].ToString());
                   
                    listView1.Items.Add(item1);
                }

                listView1.EndUpdate();  //Rafraichi le controle 
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            //On peuple les variables
            NomAssurance = listView1.SelectedItems[0].SubItems[1].Text;
            
            //Puis on ferme la form
            DialogResult = System.Windows.Forms.DialogResult.OK;

            //Puis on ferme la form
            this.Close();
        }

        private void tBAssurance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //On selectionne la première ligne de la liste (s'il y a qqchose), puis on ferme
                if (listView1.Items.Count > 0)
                {
                    //On peuple les variables
                    NomAssurance = listView1.Items[0].SubItems[1].Text;

                    //Puis on ferme la form
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;

                //Puis on ferme la form
                this.Close();
            }
        }
    }
}
