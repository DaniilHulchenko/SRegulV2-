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
    public partial class FRechercheAppels : Form
    {
        private string CodeMedecin;
        public string codeMedecin
        {
            get { return CodeMedecin; }
            set { CodeMedecin = value; }
        }

        private DataTable dtAppels = new DataTable();

        public FRechercheAppels()
        {
            InitializeComponent();

            //On fixe la taille du panel1 (ou il y a les champs de recherche et les boutons) 
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;

            //Def des colonnes du Listeview          
            listView1.Columns.Add("CodeMedecin", 1);         //Colonne invisible  
            listView1.Columns.Add("N° de Visite", 85);
            listView1.Columns.Add("Date Ap", 80);
            listView1.Columns.Add("Heure Ap", 80);
            listView1.Columns.Add("Date Fin V", 80);
            listView1.Columns.Add("Heure Fin V", 80);
            listView1.Columns.Add("Nom/Prénom", 200);
            listView1.Columns.Add("Adresse", 300);
            listView1.Columns.Add("Téléphone", 100);
            listView1.Columns.Add("Régul", 160);
            listView1.Columns.Add("Medecin", 160);           

            listView1.View = View.Details;    //Pour afficher les subItems  

            //On initialise la variable
            CodeMedecin = "-1";
        }

        private void FRechercheAppels_Load(object sender, EventArgs e)
        {
            if (CodeMedecin != "-1")   //Si on a renvoyé un code médecin, on affiche ses visites du jour
            {
                //On désactive les champs et le bouton rechercher
                tBNumAppel.Enabled = false;
                tBAppellant.Enabled = false;
                tBNom.Enabled = false;
                tBoxCommune.Enabled = false;
                tBRue.Enabled = false;
                tBoxMedecin.Enabled = false;
                tBoxProvenance.Enabled = false;
                tBRegulateur.Enabled = false;
                mTBoxDateNaiss.Enabled = false;
                dTimePDeb.Enabled = false;
                dTimePFin.Enabled = false;
                bRecherche.Enabled = false;
                cBoxAnnules.Enabled = false;

                //Puis on recherche les visites du jour
                dtAppels = FonctionsAppels.RechVisiteJour(CodeMedecin);

                //Puis on affecte les champs                    
                //On vide la liste pour la rafraichir                
                listView1.BeginUpdate();
                listView1.Items.Clear();

                for (int i = 0; i < dtAppels.Rows.Count; i++)
                {
                    ListViewItem item = new ListViewItem(CodeMedecin);
                    item.SubItems.Add(dtAppels.Rows[i]["Num_Appel"].ToString());
                    item.SubItems.Add(FonctionsAppels.convertDateMaria(dtAppels.Rows[i]["DateAppel"].ToString(), "Texte").Remove(10, 9));
                    item.SubItems.Add(dtAppels.Rows[i]["HeureAppel"].ToString());
                    item.SubItems.Add(dtAppels.Rows[i]["DateTerm"].ToString());  //Attention peut renvoyer valeur null!!!
                    // item.SubItems.Add(FonctionsAppels.convertDateMaria(dtAppels.Rows[i]["DateTerm"].ToString(), "Texte").Remove(10, 9));
                    item.SubItems.Add(dtAppels.Rows[i]["HeureTerm"].ToString());
                    item.SubItems.Add(dtAppels.Rows[i]["NomPat"].ToString() + " " + dtAppels.Rows[i]["PrenomPat"].ToString());
                    item.SubItems.Add(dtAppels.Rows[i]["Adr1"].ToString() + ", " + dtAppels.Rows[i]["Num_Rue"].ToString() + " "
                                      + dtAppels.Rows[i]["CodePostal"].ToString() + " " + dtAppels.Rows[i]["Commune"].ToString());
                    item.SubItems.Add(dtAppels.Rows[i]["Tel_Appel"].ToString());
                    item.SubItems.Add(dtAppels.Rows[i]["Util"].ToString());
                    item.SubItems.Add(FonctionsAppels.NomMedecin(codeMedecin));                   
                    listView1.Items.Add(item);
                }

                listView1.EndUpdate();  //Rafraichi le controle  
            }
            else  //Sinon c'est la boite de recherche
            {
                //On active les champs et le bouton rechercher
                tBNumAppel.Enabled = true;
                tBAppellant.Enabled = true;
                tBNom.Enabled = true;
                tBoxCommune.Enabled = true;
                tBRue.Enabled = true;
                tBoxMedecin.Enabled = true;
                tBoxProvenance.Enabled = true;
                tBRegulateur.Enabled = true;
                mTBoxDateNaiss.Enabled = true;
                dTimePDeb.Enabled = true;
                dTimePFin.Enabled = true;
                bRecherche.Enabled = true;
                cBoxAnnules.Enabled = true;
            }
        }

        private void tBoxCommune_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //On affiche la boite de recherche des communes
                FRechCommune fRechCommune = new FRechCommune();

                //On récupères les valeurs
                if (fRechCommune.ShowDialog() == DialogResult.OK)
                {
                    tBoxCommune.Text = fRechCommune.nomCommune;                    
                }
                else
                {
                    tBoxProvenance.Text = "";                  
                }

                fRechCommune.Dispose();
            }
        }

        private void tBRue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //On affiche la boite de recherche des rues
                FRechRue fRechRue = new FRechRue();

                //On récupères les valeurs
                if (fRechRue.ShowDialog() == DialogResult.OK)
                {
                    tBRue.Text = fRechRue.nomRue;
                }
                else
                {
                    tBRue.Text = "";
                }

                fRechRue.Dispose();
            }
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

        private void tBoxProvenance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //On affiche la boite des de recherche des provenances
                FRechProv fRechProv = new FRechProv();

                //On récupères les valeurs
                if (fRechProv.ShowDialog() == DialogResult.OK)
                {
                    tBoxProvenance.Text = fRechProv.libelle;                   
                }
                else
                {                   
                    tBoxProvenance.Text = "";
                }

                fRechProv.Dispose();
            }
        }

        private void tBRegulateur_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //On affiche la boite des régulateurs
                FRechUtilisateur fRechUtilisateur = new FRechUtilisateur();

                fRechUtilisateur.Text = "Recherche d'un régulateur";
                //On récupères les valeurs
                if (fRechUtilisateur.ShowDialog() == DialogResult.OK)
                {
                    lCodeUtilisateur.Text = fRechUtilisateur.idUtilisateur;
                    tBRegulateur.Text = fRechUtilisateur.nomUtilisateur;
                }
                else
                {
                    lCodeUtilisateur.Text = "-1";
                    tBRegulateur.Text = "";
                }

                fRechUtilisateur.Dispose();
            }
        }

        private void bRecherche_Click(object sender, EventArgs e)
        {
            //En fonction de ce que l'on a rempli, on lance une recherche
            RechercheAppelsSelonCriteres();

            //Puis on affecte les champs                    
            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtAppels.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtAppels.Rows[i]["CodeMedecin"].ToString());
                item.SubItems.Add(dtAppels.Rows[i]["Num_Appel"].ToString());
                item.SubItems.Add(FonctionsAppels.convertDateMaria(dtAppels.Rows[i]["DateAppel"].ToString(), "Texte").Remove(10,9));
                item.SubItems.Add(dtAppels.Rows[i]["HeureAppel"].ToString());
                item.SubItems.Add(dtAppels.Rows[i]["DateTerm"].ToString());
                //item.SubItems.Add(FonctionsAppels.convertDateMaria(dtAppels.Rows[i]["DateTerm"].ToString(), "Texte").Remove(10, 9));
                item.SubItems.Add(dtAppels.Rows[i]["HeureTerm"].ToString());
                item.SubItems.Add(dtAppels.Rows[i]["Nom"].ToString() + " " + dtAppels.Rows[i]["Prenom"].ToString());
                item.SubItems.Add(dtAppels.Rows[i]["Adr1"].ToString() + ", " + dtAppels.Rows[i]["Num_Rue"].ToString() + " "
                                  + dtAppels.Rows[i]["CodePostal"].ToString() + " " + dtAppels.Rows[i]["Commune"].ToString());
                item.SubItems.Add(dtAppels.Rows[i]["Tel_Appel"].ToString());
                item.SubItems.Add(dtAppels.Rows[i]["Util"].ToString());
                item.SubItems.Add(dtAppels.Rows[i]["Med"].ToString());

                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle  
        }

       

        //####################################################REQUETE EN COUR DE TEST########################################
        private void RechercheAppelsSelonCriteres()
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT a.*, Date(p.DateOp) as DateAppel, Time(p.DateOp) as HeureAppel, ";
            sqlstr0 += "             Date(p.DateOp) as DateTerm, Time(p.DateOp) as HeureTerm, ";
            sqlstr0 += "             CONCAT(m.Nom, ' ', m.Prenom) as Med,";
            sqlstr0 += "             CONCAT(u.Nom, ' ', u.Prenom) as Util";
            sqlstr0 += " FROM appels a INNER JOIN suiviappel p ON p.Num_Appel = a.Num_Appel";            
            sqlstr0 += " 	           LEFT JOIN utilisateur u ON u.IdUtilisateur = p.IdUtilisateur ";
            sqlstr0 += " 	           LEFT JOIN medecins m ON a.CodeMedecin = m.CodeMedecin";
            sqlstr0 += " WHERE Date(p.DateOp) <= Date('" + FonctionsAppels.convertDateMaria(dTimePFin.Value.ToString(), "MariaDb") + "')";  //Date du jour
            sqlstr0 += " AND p.Type_Operation = 'Création'";

            if (tBNumAppel.Text != "")
                sqlstr0 += " AND a.Num_Appel = '" + tBNumAppel.Text + "'";
            else
                sqlstr0 += " AND Date(p.DateOp) >= Date('" + FonctionsAppels.convertDateMaria(dTimePDeb.Value.ToString("d"), "MariaDb") + "')";

            if (tBAppellant.Text != "")
                sqlstr0 += " AND a.Tel_Appel = '" + FormateNumTel(tBAppellant.Text) + "'";

            if (tBtelPatient.Text != "")
                sqlstr0 += " AND a.Tel_Patient = '" + FormateNumTel(tBtelPatient.Text) + "'";

            if (tBNom.Text != "")
                sqlstr0 += " AND a.Nom like '" + tBNom.Text.Replace("'", "''") + "%'";

            if (tBoxCommune.Text != "")
                sqlstr0 += " AND a.Commune = '" + tBoxCommune.Text.Replace("'", "''") + "'";

            if (tBRue.Text != "")
                sqlstr0 += " AND a.Adr1 = '" + tBRue.Text.Replace("'", "''") + "'";

            if (lCodeMedecin.Text != "-1")
                sqlstr0 += " AND a.CodeMedecin = '" + lCodeMedecin.Text + "'";

            if (tBoxProvenance.Text != "")
                sqlstr0 += " AND a.Provenance = '" + tBoxProvenance.Text + "'";

            DateTime DateNaiss;
            if (DateTime.TryParse(mTBoxDateNaiss.Text, out DateNaiss)) //Si on a une date de naissance valide
            {
                sqlstr0 += " AND a.DateNaissance = '" + FonctionsAppels.convertDateMaria(mTBoxDateNaiss.Text, "MariaDb") + "'";
            }

            if (lCodeUtilisateur.Text != "-1")
                sqlstr0 += " AND (p.IdUtilisateur = '" + lCodeUtilisateur.Text + "')";

            if (cBoxAnnules.Checked)
                sqlstr0 += " AND a.IdMotifAnnulation <> ''";
           
            sqlstr0 += " ORDER BY Date(p.DateOp) DESC, Time(p.DateOp) DESC";

            try
            {
                cmd.CommandText = sqlstr0;

                dtAppels.Rows.Clear();
                dtAppels.Load(cmd.ExecuteReader());    //on execute              
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la recherche :" + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void bFermer_Click(object sender, EventArgs e)
        {
            //On ferme la form
            this.Close();
        }

        private void tBoxMedecin_TextChanged(object sender, EventArgs e)
        {
            if (tBoxMedecin.Text == "")
                lCodeMedecin.Text = "-1";
        }

        private void tBRegulateur_TextChanged(object sender, EventArgs e)
        {
            if (tBRegulateur.Text == "")
                lCodeUtilisateur.Text = "-1";
        }

        private string FormateNumTel(string NumTel)
        {
            if (NumTel != "")
            {
                //Formatage du n° de Tel: Si c'est au format International
                if (NumTel.IndexOf("+") == -1)
                {
                    //Si on a le début d'un indicatif internationnal
                    if (NumTel.Substring(0, 2) == "00")
                    {
                        //On enlève les 2 Zéro et on remplace par un +
                        NumTel = "+" + NumTel.Remove(0, 2);
                    }
                    else    //N° nationnal, on l'internationnalise             
                    {
                        NumTel = "+41" + NumTel.Remove(0, 1);
                    }
                }
                else
                {   //On le reformate
                    NumTel = "+" + NumTel.Replace("+", "");
                }
            }

            return NumTel;
        }

        //On trie quand on clique sur la colonne
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.listView1.ListViewItemSorter = new ListViewTriRechAppels(e.Column);
        }

        //click gauche => on affiche la fiche, click droit => on affiche la liste des 20 dernières visites
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //On affiche la visite
                if (listView1.SelectedItems[0].SubItems[1].Text != "-1")
                {
                    FVisite fVisite = new FVisite();
                    fVisite.NumVisite = int.Parse(listView1.SelectedItems[0].SubItems[1].Text);
                    fVisite.provenance = "RechercheAppels";
                    fVisite.ShowDialog(this);
                    fVisite.Dispose();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                //affiche les 20 derniers appels
                DataTable dt20DerAppels = new DataTable();

                //Recup des infos du patient à partir du n° de consult
                DataTable dtInfosPatient = new DataTable();
                dtInfosPatient = FonctionsAppels.RecupInfosPatient(listView1.SelectedItems[0].SubItems[1].Text);

                if (dtInfosPatient.Rows.Count > 0)
                {
                    //On recherche les 20 derniers appels
                    dt20DerAppels = FonctionsAppels.Derniers20Appels(dtInfosPatient.Rows[0]["Nom"].ToString(), dtInfosPatient.Rows[0]["Prenom"].ToString(),
                                                                                                                dtInfosPatient.Rows[0]["DateNaissance"].ToString());

                    if (dt20DerAppels.Rows.Count > 0)
                    {
                        FRechDerniersAppels fRechDerniersAppels = new FRechDerniersAppels(dt20DerAppels);
                        fRechDerniersAppels.ShowDialog(this);
                        fRechDerniersAppels.Dispose();
                    }
                }
            }
        }      
    }

    //Pour implémenter le tri en cliquant sur le titre des colonnes
    class ListViewTriRechAppels : IComparer
    {
        private int col;
        public ListViewTriRechAppels()
        {
            col = 0;
        }
        public ListViewTriRechAppels(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
        }
    }
}



//A faire: