using MySqlConnector;
using System;
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
    public partial class FListeRDV : Form
    {
        //Pour retourner le résultat de la recherche       
        private string Num_Appel_RDV;
        public string num_Appel_RDV
        {
            get { return Num_Appel_RDV; }
            set { Num_Appel_RDV = value; }
        }

        private string NomPrenom;
        public string nomPrenom
        {
            get { return NomPrenom; }
            set { NomPrenom = value; }
        }

        private string DateRDV;
        public string dateRDV
        {
            get { return DateRDV; }
            set { DateRDV = value; }
        }

        private string AttribMedecin;
        public string attribMedecin
        {
            get { return attribMedecin; }
            set { attribMedecin = value; }
        }


        public FListeRDV()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("Num Appel Rdv", 1);         //Colonne invisible
            listView1.Columns.Add("Nom Patient", 150);
            listView1.Columns.Add("Date du RDV", 150);
            listView1.Columns.Add("Attribué au médecin", 150);
            listView1.View = View.Details;    //Pour afficher les subItems  

            Num_Appel_RDV = "-1";
            NomPrenom = "";
            DateRDV = "";
            AttribMedecin = "";
        }


        private void FListeRDV_Load(object sender, EventArgs e)
        {
            //On affiche tous les rendez-vous à venir          
            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //AppelsRDV
                string sqlstr0 = "SELECT a.Num_Appel_Rdv, CONCAT(a.Nom, ' ', a.Prenom) as Patient, CONCAT(m.Nom,' ', m.Prenom) AS Medecin, a.DateRDV" +
                                 " FROM appelsRDV a LEFT JOIN medecins m ON m.CodeMedecin = a.CodeMedecin " +
                                 " WHERE DateRDV >= CURDATE() ORDER BY DateRDV DESC";

                cmd.CommandText = sqlstr0;
               
                DataTable dtRdv = new DataTable();

                dtRdv.Load(cmd.ExecuteReader());    //on execute

                //Chargement de la liste
                listView1.BeginUpdate();
                listView1.Items.Clear();

                //On charge la liste
                for (int i = 0; i < dtRdv.Rows.Count; i++)
                {
                    ListViewItem item1 = new ListViewItem(dtRdv.Rows[i]["Num_Appel_Rdv"].ToString());
                    item1.SubItems.Add(dtRdv.Rows[i]["Patient"].ToString());                   
                    item1.SubItems.Add(dtRdv.Rows[i]["DateRDV"].ToString());
                    item1.SubItems.Add(dtRdv.Rows[i]["Medecin"].ToString());

                    listView1.Items.Add(item1);
                }

                listView1.EndUpdate();  //Rafraichi le controle 

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération de la liste des rendez-vous. " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);               
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }          
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            //On affiche la visite
            if (listView1.SelectedItems[0].SubItems[0].Text != "-1")
            {                
                FAppelsRDV fAppelsRDV = new FAppelsRDV();
                fAppelsRDV.NumAppelRDV = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
                fAppelsRDV.provenance = "ListeAppels";
                fAppelsRDV.ShowDialog(this);
                fAppelsRDV.Dispose();

                //Puis on rafraichi la liste
                FListeRDV_Load(sender, e);
            }
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            //Puis on ferme la form
            Close();
        }      

        private void bNvxRdv_Click(object sender, EventArgs e)
        {
            FAppelsRDV fAppelsRDV = new FAppelsRDV();
            fAppelsRDV.NumAppelRDV = -1;           
            fAppelsRDV.ShowDialog(this);
            fAppelsRDV.Dispose();

            //Puis on rafraichi la liste
            FListeRDV_Load(sender, e);
        }
    }
}



//à faire
//vérifier le fonctionnement de la liste
