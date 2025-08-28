using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FRechEvenements : Form
    {

        private DataTable dtEvenements = new DataTable();

        public FRechEvenements()
        {
            InitializeComponent();

            //On fixe la taille du panel1 (ou il y a les champs de recherche et les boutons) 
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;

            //Def des colonnes du Listeview          
            listView1.Columns.Add("Id", 60);         //Colonne invisible            
            listView1.Columns.Add("Date Ev.", 140);
            listView1.Columns.Add("Evènement", 900);
            listView1.Columns.Add("Utilisateur", 160);
           
            listView1.View = View.Details;    //Pour afficher les subItems  
        }

       
        private void bRecherche_Click(object sender, EventArgs e)
        {
            //En fonction de ce que l'on a rempli, on lance une recherche
            RechercheEvenementsSelonCriteres();

            //Puis on affecte les champs                    
            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtEvenements.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtEvenements.Rows[i]["Id"].ToString());
                item.SubItems.Add(FonctionsAppels.convertDateMaria(dtEvenements.Rows[i]["DateEvenement"].ToString(), "Texte"));
                item.SubItems.Add(dtEvenements.Rows[i]["Evenement"].ToString());
                item.SubItems.Add(dtEvenements.Rows[i]["Utilisateur"].ToString());               

                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle  

        }


        private void RechercheEvenementsSelonCriteres()
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT * FROM evenements ";           
            sqlstr0 += " WHERE Date(DateEvenement) >= Date('" + FonctionsAppels.convertDateMaria(dTimePDeb.Value.ToString(), "MariaDb") + "')";
            sqlstr0 += " AND Date(DateEvenement) <= Date('" + FonctionsAppels.convertDateMaria(dTimePFin.Value.ToString(), "MariaDb") + "')";

            if (tBCritere.Text != "")
                sqlstr0 += " AND Evenement like '%" + tBCritere.Text + "%'";

           
            try
            {
                cmd.CommandText = sqlstr0;

                dtEvenements.Rows.Clear();
                dtEvenements.Load(cmd.ExecuteReader());    //on execute              
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }
        }

        private void bFermer_Click(object sender, EventArgs e)
        {
            //On ferme la form
            this.Close();
        }

        //On trie quand on clique sur la colonne
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.listView1.ListViewItemSorter = new ListViewTri(e.Column);
        }
    }


    //Pour implémenter le tri en cliquant sur le titre des colonnes
    class ListViewTri : IComparer
    {
        private int col;
        public ListViewTri()
        {
            col = 0;
        }
        public ListViewTri(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
        }
    }

}
