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
    public partial class FRechSmartphone : Form
    {

        //Pour retourner le résultat de la recherche
        private int IdSmartphone;
        public int idSmartphone
        {
            get { return IdSmartphone; }
            set { IdSmartphone = value; }
        }

        private string NomSmartphone;
        public string nomSmartphone
        {
            get { return NomSmartphone; }
            set { NomSmartphone = value; }
        }

        private DataTable dtSmartphone = new DataTable();

        public FRechSmartphone()
        {
            InitializeComponent();

            //Pour la liste des PDA
            listView1.Columns.Add("Code PDA", 1);         //Colonne invisible
            listView1.Columns.Add("Nom Smartphone", 150);
            listView1.View = View.Details;    //Pour afficher les subItems  

            IdSmartphone = -1;
            NomSmartphone = "";
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            //On ferme en envoyant cancel
            DialogResult = System.Windows.Forms.DialogResult.Cancel;

            //Puis on ferme la form
            this.Close();
        }

        private void FRechSmartphone_Load(object sender, EventArgs e)
        {
            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            dtSmartphone = FonctionsAppels.ChargeListeSmarphone();

            for (int i = 0; i < dtSmartphone.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtSmartphone.Rows[i]["IdSmartphone"].ToString());
                item.SubItems.Add(dtSmartphone.Rows[i]["NomSmartphone"].ToString());
                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle        
        }

        private void tBSmartphone_TextChanged(object sender, EventArgs e)
        {
            //on lance une recherche avec ce qui a été entré
            string SqlSelect = "NomSmartphone like '" + tBSmartphone.Text.Replace("'", "''") + "%'";
            string Trie = "NomSmartphone";

            if (dtSmartphone.Select(SqlSelect, Trie).Any())    //Si on a quelque chose
            {
                DataTable dtMedTrie = dtSmartphone.Select(SqlSelect, Trie).CopyToDataTable();

                //On vide la liste pour la rafraichir                
                listView1.BeginUpdate();
                listView1.Items.Clear();

                for (int i = 0; i < dtMedTrie.Rows.Count; i++)
                {
                    ListViewItem item = new ListViewItem(dtMedTrie.Rows[i]["IdSmartphone"].ToString());
                    item.SubItems.Add(dtMedTrie.Rows[i]["NomSmartphone"].ToString());
                    listView1.Items.Add(item);
                }

                listView1.EndUpdate();  //Rafraichi le controle  
            }
        }

        //On sélectionne la ligne puis on ferme la form
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int index = listView1.SelectedItems[0].Index;

            //On peuple les variables       
            IdSmartphone = int.Parse(listView1.SelectedItems[0].Text);
            NomSmartphone = listView1.SelectedItems[0].SubItems[1].Text;

            //On détermine la réponse de retour
            DialogResult = System.Windows.Forms.DialogResult.OK;

            //Puis on ferme la form
            this.Close();
        }

        private void tBSmartphone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //On selectionne la première ligne de la liste (s'il y a qqchose), puis on ferme
                if (listView1.Items.Count > 0)
                {
                    //On peuple les variables       
                    IdSmartphone = int.Parse(listView1.Items[0].Text);
                    NomSmartphone = listView1.Items[0].SubItems[1].Text;

                    //On détermine la réponse de retour
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
