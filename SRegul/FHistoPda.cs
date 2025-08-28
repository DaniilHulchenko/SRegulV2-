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
    public partial class FHistoPda : Form
    {

        public static int IdPDA;
        private DataTable dtPDA = new DataTable();

        public FHistoPda()
        {
            InitializeComponent();

            //On fixe la taille du panel1 (ou il y a les champs de recherche et les boutons) 
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;

            //Def des colonnes du Listeview                           
            listView1.Columns.Add("Date Début de garde", 160);
            listView1.Columns.Add("Date Fin de garde", 160);
            listView1.Columns.Add("Médecin", 200);
            listView1.Columns.Add("Smartphone", 200);
            listView1.Columns.Add("Type de garde", 120);
            listView1.Columns.Add("Nb visite eff.", 100);

            listView1.View = View.Details;    //Pour afficher les subItems  
        }

        private void tBoxMedecin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //On affiche la boite de recherche des médecins
                FRechMedecin fRechMedecin = new FRechMedecin();

                //On récupères les valeurs
                if (fRechMedecin.ShowDialog() == DialogResult.OK)
                {
                    lCodeMedecin.Text = fRechMedecin.codeMedecin;
                    tBoxMedecin.Text = fRechMedecin.nomMedecin;
                }
                else
                {
                    lCodeMedecin.Text = "-1";
                    tBoxMedecin.Text = "";
                }

                fRechMedecin.Dispose();
            }
        }

        private void tBoxMedecin_TextChanged(object sender, EventArgs e)
        {
            if (tBoxMedecin.Text == "")
                lCodeMedecin.Text = "-1";
        }

        private void tBsmartphone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                IdPDA = -1;

                //On affiche la boite de recherche des smartphones
                FRechSmartphone fRechSmartphone = new FRechSmartphone();

                //On récupères les valeurs
                if (fRechSmartphone.ShowDialog() == DialogResult.OK)
                {
                    IdPDA = fRechSmartphone.idSmartphone;
                    tBsmartphone.Text = fRechSmartphone.nomSmartphone;
                }
                else
                {
                    IdPDA = -1;
                    tBsmartphone.Text = "";
                }

                fRechSmartphone.Dispose();
            }
        }

        private void tBsmartphone_TextChanged(object sender, EventArgs e)
        {
            if (tBsmartphone.Text == "")
                IdPDA = -1;
        }

        private void bRecherche_Click(object sender, EventArgs e)
        {
            //En fonction de ce que l'on a rempli, on lance une recherche
            RechercheHistoPDASelonCriteres();

            //Puis on affecte les champs                    
            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtPDA.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(FonctionsAppels.convertDateMaria(dtPDA.Rows[i]["DebGarde"].ToString(), "Texte"));
                item.SubItems.Add(FonctionsAppels.convertDateMaria(dtPDA.Rows[i]["FinGarde"].ToString(), "Texte"));
                item.SubItems.Add(dtPDA.Rows[i]["Med"].ToString());
                item.SubItems.Add(dtPDA.Rows[i]["NomSmartphone"].ToString());
                item.SubItems.Add(dtPDA.Rows[i]["TypeGarde"].ToString());
                item.SubItems.Add(dtPDA.Rows[i]["Nb_Visite"].ToString());

                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle  

        }


        private void RechercheHistoPDASelonCriteres()
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT h.*, CONCAT(m.Nom, ' ', m.Prenom) as Med, s.NomSmartphone FROM histogarde h";
            sqlstr0 += "                 INNER JOIN medecins m ON m.CodeMedecin = h.CodeMedecin";
            sqlstr0 += "                 INNER JOIN smartphone s ON s.IdSmartphone = h.IdSmartphone";         
            sqlstr0 += " WHERE Date(h.DebGarde) >= Date('" + FonctionsAppels.convertDateMaria(dTimePDeb.Value.ToString(), "MariaDb") + "')";
            sqlstr0 += " AND Date(h.DebGarde) <= Date('" + FonctionsAppels.convertDateMaria(dTimePFin.Value.ToString(), "MariaDb") + "')";

            if (lCodeMedecin.Text != "-1")
            {
                sqlstr0 += " AND m.CodeMedecin = " + lCodeMedecin.Text;
            }

            if (tBsmartphone.Text != "")
                sqlstr0 += " AND s.IdSmartphone = " + IdPDA;
           
            try
            {
                cmd.CommandText = sqlstr0;

                dtPDA.Rows.Clear();
                dtPDA.Load(cmd.ExecuteReader());    //on execute              
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

        //On trie quand on clique sur la colonne
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.listView1.ListViewItemSorter = new ListViewTriRechHistoPDA(e.Column);
        }

        private void bFermer_Click(object sender, EventArgs e)
        {
            //On ferme la form
            this.Close();
        }
    }


    //Pour implémenter le tri en cliquant sur le titre des colonnes
    class ListViewTriRechHistoPDA : IComparer
    {
        private int col;
        public ListViewTriRechHistoPDA()
        {
            col = 0;
        }
        public ListViewTriRechHistoPDA(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
        }
    }

}
