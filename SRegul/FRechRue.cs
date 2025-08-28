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
    public partial class FRechRue : Form
    {
        //Pour retourner le résultat de la recherche             
        private string NomRue;
        public string nomRue
        {
            get { return NomRue; }
            set { NomRue = value; }
        }

        private string CodePostal;
        public string codepostal
        {
            get { return CodePostal; }
            set { CodePostal = value; }
        }   

        private string Commune;
        public string commune
        {
            get { return Commune; }
            set { Commune = value; }
        }

        private string Pays;
        public string pays
        {
            get { return Pays; }
            set { Pays = value; }
        }

        //Pour ne rechercher les rues appartenant à cette commune
        private string CommuneSaisie;
        public string communesaisie
        {
            get { return CommuneSaisie; }
            set { CommuneSaisie = value; }
        }   

        public FRechRue()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("IdRue", 1);         //Colonne invisible
            listView1.Columns.Add("Nom de la rue", 200);
            listView1.Columns.Add("CodePostal", 50);             //Colonne invisible
            listView1.Columns.Add("Nom de la commune", 150);
            listView1.Columns.Add("Pays", 1);               //Colonne invisible
            listView1.View = View.Details;    //Pour afficher les subItems  

            //On initialise Commune Saisie dans Visite ou Re-saisie
            CommuneSaisie = "";

            NomRue = "";           
        }

        private void FRechRue_Load(object sender, EventArgs e)
        {                        
            //On donne le focus a la rue
            this.ActiveControl = tBRue;           
        }

        private void tBRue_TextChanged(object sender, EventArgs e)
        {
            //Critères de selection
            string SqlSelect = "";
            string Trie = "";

            if (CommuneSaisie != "")    //On a déjà saisie la commune, on filtre uniquement les rue de la commune
            {
                SqlSelect = "NomRue like '%" + tBRue.Text.Replace("'", "''") + "%' AND Commune = '" + CommuneSaisie + "'";
                Trie = "Commune";
            }
            else   //On affiche toute les rues correspondantes quelque soit la commune
            {
                SqlSelect = "NomRue like '%" + tBRue.Text.Replace("'", "''") + "%'";   // OR nomRue like '%-"+ tBRue.Text.Replace("'", "''") + "%'";
                Trie = "Commune";
            }


            //on lance une recherche avec ce qui a été entré      
            if (Form1.dtRue.Select(SqlSelect, Trie).Any())    //Si on a quelque chose
            {
                DataTable dtRueTrie = Form1.dtRue.Select(SqlSelect, Trie).CopyToDataTable();

                listView1.BeginUpdate();
                listView1.Items.Clear();

                //On charge la liste
                for (int i = 0; i < dtRueTrie.Rows.Count; i++)
                {
                    ListViewItem item1 = new ListViewItem(dtRueTrie.Rows[i]["IdRue"].ToString());
                    item1.SubItems.Add(dtRueTrie.Rows[i]["NomRue"].ToString());
                    item1.SubItems.Add(dtRueTrie.Rows[i]["CodePostal"].ToString());
                    item1.SubItems.Add(dtRueTrie.Rows[i]["Commune"].ToString());
                    item1.SubItems.Add(dtRueTrie.Rows[i]["Pays"].ToString());

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
            NomRue = listView1.SelectedItems[0].SubItems[1].Text;
            CodePostal = listView1.SelectedItems[0].SubItems[2].Text;
            Commune = listView1.SelectedItems[0].SubItems[3].Text;
            Pays = listView1.SelectedItems[0].SubItems[4].Text;

            //Puis on ferme la form
            DialogResult = System.Windows.Forms.DialogResult.OK;

            //Puis on ferme la form
            this.Close();            
        }

        private void tBRue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //On selectionne la première ligne de la liste (s'il y a qqchose), puis on ferme
                if (listView1.Items.Count > 0)
                {
                    //On peuple les variables
                    NomRue = listView1.Items[0].SubItems[1].Text;
                    CodePostal = listView1.Items[0].SubItems[2].Text;
                    Commune = listView1.Items[0].SubItems[3].Text;
                    Pays = listView1.Items[0].SubItems[4].Text;

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
            DialogResult = System.Windows.Forms.DialogResult.Cancel;

            //Puis on ferme la form
            this.Close();
        }
       
    }
}
