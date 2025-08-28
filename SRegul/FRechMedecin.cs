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
    public partial class FRechMedecin : Form
    {
        //Pour retourner le résultat de la recherche
        private string CodeMedecin;
        public string codeMedecin
        {
            get { return CodeMedecin; }
            set { CodeMedecin = value; }
        }

        private string NomMedecin;
        public string nomMedecin
        {
            get { return NomMedecin; }
            set { NomMedecin = value; }
        }

        private DataTable dtMedecin = new DataTable();

        public FRechMedecin()
        {
            InitializeComponent();

            //Pour la liste des médecins "Actifs"
            listView1.Columns.Add("Code Médecin", 1);         //Colonne invisible
            listView1.Columns.Add("Médecin", 150);
            listView1.View = View.Details;    //Pour afficher les subItems  

            CodeMedecin = "-1";
            NomMedecin = "";
        }

        private void FRechMedecin_Load(object sender, EventArgs e)
        {                 
            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            dtMedecin = FonctionsAppels.ChargeListeMedecins("actifs");

            for (int i = 0; i < dtMedecin.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtMedecin.Rows[i]["CodeMedecin"].ToString());
                item.SubItems.Add(dtMedecin.Rows[i]["Nom"].ToString() + " " + dtMedecin.Rows[i]["Prenom"].ToString());
                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle        
        }

        private void tBMedecin_TextChanged(object sender, EventArgs e)
        {
            //on lance une recherche avec ce qui a été entré
            string SqlSelect = "Nom like '" + tBMedecin.Text.Replace("'", "''") + "%'";
            string Trie = "Nom";

            if (dtMedecin.Select(SqlSelect, Trie).Any())    //Si on a quelque chose
            {
                DataTable dtMedTrie = dtMedecin.Select(SqlSelect, Trie).CopyToDataTable();

                //On vide la liste pour la rafraichir                
                listView1.BeginUpdate();
                listView1.Items.Clear();

                for (int i = 0; i < dtMedTrie.Rows.Count; i++)
                {
                    ListViewItem item = new ListViewItem(dtMedTrie.Rows[i]["CodeMedecin"].ToString());
                    item.SubItems.Add(dtMedTrie.Rows[i]["Nom"].ToString() + " " + dtMedTrie.Rows[i]["Prenom"].ToString());
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
            CodeMedecin = listView1.SelectedItems[0].Text;
            NomMedecin = listView1.SelectedItems[0].SubItems[1].Text;

            //On détermine la réponse de retour
            DialogResult = System.Windows.Forms.DialogResult.OK;

            //Puis on ferme la form
            this.Close();            
        }

        private void tBMedecin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //On selectionne la première ligne de la liste (s'il y a qqchose), puis on ferme
                if (listView1.Items.Count > 0)
                {
                    //On peuple les variables       
                    CodeMedecin = listView1.Items[0].Text;
                    NomMedecin = listView1.Items[0].SubItems[1].Text;

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
