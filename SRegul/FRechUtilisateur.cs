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
    public partial class FRechUtilisateur : Form
    {
        //Pour retourner le résultat de la recherche
        private string IdUtilisateur;
        public string idUtilisateur
        {
            get { return IdUtilisateur; }
            set { IdUtilisateur = value; }
        }

        private string NomUtilisateur;
        public string nomUtilisateur
        {
            get { return NomUtilisateur; }
            set { NomUtilisateur = value; }
        }

        private DataTable dtUtilisateur = new DataTable();

        public FRechUtilisateur()
        {
            InitializeComponent();

            //Pour la liste des utilisateurs
            listView1.Columns.Add("Id Utilisateur", 1);         //Colonne invisible
            listView1.Columns.Add("Utilisateur", 150);
            listView1.View = View.Details;    //Pour afficher les subItems  

            IdUtilisateur = "-1";
            NomUtilisateur = "";
        }

        private void FRechUtilisateur_Load(object sender, EventArgs e)
        {
            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            dtUtilisateur = FonctionsAppels.ChargeListeUtilisateurs("actifs");

            for (int i = 0; i < dtUtilisateur.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtUtilisateur.Rows[i]["IdUtilisateur"].ToString());
                item.SubItems.Add(dtUtilisateur.Rows[i]["Nom"].ToString() + " " + dtUtilisateur.Rows[i]["Prenom"].ToString());
                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle        
        }

        private void tBoxUtilisateur_TextChanged(object sender, EventArgs e)
        {
            //on lance une recherche avec ce qui a été entré
            string SqlSelect = "Nom like '" + tBoxUtilisateur.Text + "%'";
            string Trie = "Nom";

            if (dtUtilisateur.Select(SqlSelect, Trie).Any())    //Si on a quelque chose
            {
                DataTable dtUtilTrie = dtUtilisateur.Select(SqlSelect, Trie).CopyToDataTable();

                //On vide la liste pour la rafraichir                
                listView1.BeginUpdate();
                listView1.Items.Clear();

                for (int i = 0; i < dtUtilTrie.Rows.Count; i++)
                {
                    ListViewItem item = new ListViewItem(dtUtilTrie.Rows[i]["IdUtilisateur"].ToString());
                    item.SubItems.Add(dtUtilTrie.Rows[i]["Nom"].ToString() + " " + dtUtilTrie.Rows[i]["Prenom"].ToString());
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
            IdUtilisateur = listView1.SelectedItems[0].Text;
            NomUtilisateur = listView1.SelectedItems[0].SubItems[1].Text;

            //On détermine la réponse de retour
            DialogResult = System.Windows.Forms.DialogResult.OK;

            //Puis on ferme la form
            this.Close();            
        }

        private void tBoxUtilisateur_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //On selectionne la première ligne de la liste (s'il y a qqchose), puis on ferme
                if (listView1.Items.Count > 0)
                {
                    //On peuple les variables       
                    IdUtilisateur = listView1.Items[0].Text;
                    NomUtilisateur = listView1.Items[0].SubItems[1].Text;

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
