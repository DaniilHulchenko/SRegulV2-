using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FGardeMedecin : Form
    {
        public static DataTable dtGardeMedecin = new DataTable();
        public static DataTable dtMedecin = new DataTable();
        public static DataTable dtGarde = new DataTable();
        public static DataTable dtSmartphone = new DataTable();
        public static string Etat = "Lecture";       
        public static int IdGarde = -1;
        public static int Smartphone = -1;
        public static int CodeMedecin = -1;
        public static int IdStatusGarde = -1;
        public static DateTime DateDebGarde = DateTime.Now;
        public static int Nb_Visite = 0;
        public static double LatCentrale = 0;
        public static double LngCentrale = 0;
        
        public FGardeMedecin()
        {
            InitializeComponent();

            //Def des colonnes des Listeview
            //Pour la liste des médecins "Actifs"
            listView1.Columns.Add("Code Médecin", 1);         //Colonne invisible
            listView1.Columns.Add("Médecin", 170);           
            listView1.View = View.Details;    //Pour afficher les subItems  

            //Pour la liste des médecins en garde
            listView2.Columns.Add("Code Médecin", 1);         //Colonne invisible
            listView2.Columns.Add("Médecin", 150);
            listView2.Columns.Add("Type Garde", 30);
            listView2.Columns.Add("Créneau", 100);
            listView2.Columns.Add("Smartphone", 100);
            listView2.View = View.Details;    //Pour afficher les subItems  

            //Pour la liste des gardes
            listView3.Columns.Add("Idgarde", 1);         //Colonne invisible
            listView3.Columns.Add("Type de garde", 120);
            listView3.Columns.Add("Créneau", 120);            
            listView3.View = View.Details;    //Pour afficher les subItems  

            //Pour la liste des smartphones
            listView4.Columns.Add("Idsmart", 1);         //Colonne invisible
            listView4.Columns.Add("Smartphone", 150);     //Nom du smartphone
            listView4.Columns.Add("Medecin", 150);       //Medecin attribué   
            listView4.Columns.Add("CodeMed", 1);         //Colonne invisible
            listView4.View = View.Details;    //Pour afficher les subItems              
        }

        private void FGardeMedecin_Load(object sender, EventArgs e)
        {
            //On charge la liste des médecins qui ne sont pas en garde
            dtMedecin.Clear();
            dtMedecin = FonctionsAppels.ChargeListeMedecins("non garde");

            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtMedecin.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtMedecin.Rows[i]["CodeMedecin"].ToString());
                item.SubItems.Add(dtMedecin.Rows[i]["Nom"].ToString() + " " + dtMedecin.Rows[i]["Prenom"].ToString());
              
                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle            
            
            
            //Au tour des médecins en garde
            dtGardeMedecin.Clear();
            dtGardeMedecin = FonctionsAppels.ChargeListeMedecins("en garde");

            //On vide la liste pour la rafraichir                
            listView2.BeginUpdate();
            listView2.Items.Clear();

            for (int i = 0; i < dtGardeMedecin.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtGardeMedecin.Rows[i]["CodeMedecin"].ToString());
                item.SubItems.Add(dtGardeMedecin.Rows[i]["Nom"].ToString() + " " + dtGardeMedecin.Rows[i]["Prenom"].ToString());
                item.SubItems.Add(dtGardeMedecin.Rows[i]["TypeGarde"].ToString());
                item.SubItems.Add(dtGardeMedecin.Rows[i]["HeureDeb"].ToString() + "->" + dtGardeMedecin.Rows[i]["HeureFin"].ToString());
                item.SubItems.Add(dtGardeMedecin.Rows[i]["NomSmartphone"].ToString());

                listView2.Items.Add(item);
            }

            listView2.EndUpdate();  //Rafraichi le controle            


            //Pour la liste des gardes
            dtGarde.Clear();
            dtGarde = FonctionsAppels.ChargeListeGardes();

            //On vide la liste pour la rafraichir                
            listView3.BeginUpdate();
            listView3.Items.Clear();

            for (int i = 0; i < dtGarde.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtGarde.Rows[i]["IdGarde"].ToString());
                item.SubItems.Add(dtGarde.Rows[i]["TypeGarde"].ToString());
                item.SubItems.Add(dtGarde.Rows[i]["HeureDeb"].ToString() + "->" + dtGarde.Rows[i]["HeureFin"].ToString());
              
                listView3.Items.Add(item);
            }

            listView3.EndUpdate();  //Rafraichi le controle       

            //Et enfin pour les Smartphones
            dtSmartphone.Clear();
            dtSmartphone = FonctionsAppels.ChargeListeSmarphone();

            //On vide la liste pour la rafraichir                
            listView4.BeginUpdate();
            listView4.Items.Clear();

            for (int i = 0; i < dtSmartphone.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtSmartphone.Rows[i]["IdSmartphone"].ToString());     //IdSmartphone
                item.SubItems.Add(dtSmartphone.Rows[i]["NomSmartphone"].ToString());                       //NomSmartphone                    

                if (SRegulV2.FonctionsAppels.NomMedecin(dtSmartphone.Rows[i]["CodeMedecin"].ToString()).ToString() != "Anomyme")
                    item.SubItems.Add(SRegulV2.FonctionsAppels.NomMedecin(dtSmartphone.Rows[i]["CodeMedecin"].ToString()).ToString());   //Nom du médecin
                else
                    item.SubItems.Add("Non attribué");

                item.SubItems.Add(dtSmartphone.Rows[i]["CodeMedecin"].ToString());                         //CodeMedecin         
                listView4.Items.Add(item);
            }

            listView4.EndUpdate();  //Rafraichi le controle       
   
            //*************SI ON VIENT D'AILLEUR*******************//
            //Si on a selectionné un médecin déjà en garde (Code Médecin != -1) depuis une autre forme, on affiche les infos
            if (CodeMedecin != -1)
            {
                //On affiche les infos du médecin et de sa garde
                //string SqlSelect = "CodeMedecin =  '" + CodeMedecin + "' AND IdStatusGarde = '" + IdStatusGarde + "'";            
                string SqlSelect = "CodeMedecin =  '" + CodeMedecin + "'";            

                if (dtGardeMedecin.Select(SqlSelect).Any())    //Si on a quelque chose
                {
                    DataTable dtGardeMedecinTrie = dtGardeMedecin.Select(SqlSelect).CopyToDataTable();
                
                    //Puis on renseigne les champs
                    tBCodeMedecin.Text = dtGardeMedecinTrie.Rows[0]["CodeMedecin"].ToString();
                    tBoxNom.Text = dtGardeMedecinTrie.Rows[0]["Nom"].ToString();
                    tBoxPrenom.Text = dtGardeMedecinTrie.Rows[0]["Prenom"].ToString();
                    tBTypeGarde.Text = dtGardeMedecinTrie.Rows[0]["TypeGarde"].ToString();
                    tBSmartphone.Text = dtGardeMedecinTrie.Rows[0]["NomSmartphone"].ToString();

                    //Pour les variables globales et les boutons
                    Etat = "Modif";

                    CodeMedecin = int.Parse(dtGardeMedecinTrie.Rows[0]["CodeMedecin"].ToString());
                    IdGarde = int.Parse(dtGardeMedecinTrie.Rows[0]["IdGarde"].ToString());
                    Smartphone = int.Parse(dtGardeMedecinTrie.Rows[0]["IdSmartphone"].ToString());
                    IdStatusGarde = int.Parse(dtGardeMedecinTrie.Rows[0]["IdStatusGarde"].ToString());
                    DateDebGarde = DateTime.Parse(FonctionsAppels.convertDateMaria(dtGardeMedecinTrie.Rows[0]["DateDebGarde"].ToString(), "Texte"));
                    Nb_Visite = int.Parse(dtGardeMedecinTrie.Rows[0]["Nb_Visite"].ToString());

                    //S'il n'a pas de visite (càd Disponible ou En pause), on peut activer la sortie de garde
                    if (dtGardeMedecinTrie.Rows[0]["StatusGarde"].ToString() == "Disponible" || dtGardeMedecinTrie.Rows[0]["StatusGarde"].ToString() == "En pause")                    
                        bFin.Enabled = true;         
                    else                 
                        bFin.Enabled = false;         
                                                            
                    bDebut.Enabled = false;
                    bModif.Enabled = true;                    
                }
                else
                {
                    //On vide les champs et initialise les boutons
                    VideChamps();
                }                                   
            }
            else
            {
                //On vide les champs et initialise les boutons
                VideChamps();
            }
        }


        private void VideChamps()
        {
            tBCodeMedecin.Text = "";
            tBoxNom.Text = "";
            tBoxPrenom.Text = "";
            tBTypeGarde.Text = "";
            tBSmartphone.Text = "";               

            //Pour les variables globales
            Etat = "Lecture";
            IdGarde = -1;
            Smartphone = -1;
            CodeMedecin = -1;
            IdStatusGarde = -1;
            DateDebGarde = DateTime.Now;
            Nb_Visite = 0;

            //Pour les boutons
            bDebut.Enabled = false;
            bModif.Enabled = false;
            bFin.Enabled = false;              
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            //Médecins pas en garde
            int index = listView1.SelectedItems[0].Index;

            //On peuple les champs
            tBCodeMedecin.Text = dtMedecin.Rows[index]["CodeMedecin"].ToString();
            tBoxNom.Text = dtMedecin.Rows[index]["Nom"].ToString();
            tBoxPrenom.Text = dtMedecin.Rows[index]["Prenom"].ToString();
            tBTypeGarde.Text = "";
            tBSmartphone.Text = "";
           
            //Pour les variables globales et les boutons
            Etat = "Ajout";
            CodeMedecin = int.Parse(dtMedecin.Rows[index]["CodeMedecin"].ToString());
            IdGarde = -1;
            Smartphone = -1;           
            IdStatusGarde = -1;
            DateDebGarde = DateTime.Now;
            Nb_Visite = 0;

            bDebut.Enabled = true;
            bModif.Enabled = false;
            bFin.Enabled = false;       
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            //Médecins en garde
            int index = listView2.SelectedItems[0].Index;
            
            //On peuple les champs
            tBCodeMedecin.Text = dtGardeMedecin.Rows[index]["CodeMedecin"].ToString();
            tBoxNom.Text = dtGardeMedecin.Rows[index]["Nom"].ToString();
            tBoxPrenom.Text = dtGardeMedecin.Rows[index]["Prenom"].ToString();
            tBTypeGarde.Text =  dtGardeMedecin.Rows[index]["TypeGarde"].ToString();
            tBSmartphone.Text = dtGardeMedecin.Rows[index]["NomSmartphone"].ToString();

            //Pour les variables globales et les boutons
            Etat = "Modif";
            CodeMedecin = int.Parse(dtGardeMedecin.Rows[index]["CodeMedecin"].ToString());
            IdGarde = int.Parse(dtGardeMedecin.Rows[index]["IdGarde"].ToString());
            Smartphone = int.Parse(dtGardeMedecin.Rows[index]["IdSmartphone"].ToString());
            IdStatusGarde = int.Parse(dtGardeMedecin.Rows[index]["IdStatusGarde"].ToString());
            DateDebGarde = DateTime.Parse(FonctionsAppels.convertDateMaria(dtGardeMedecin.Rows[index]["DateDebGarde"].ToString(), "Texte"));
            Nb_Visite = int.Parse(dtGardeMedecin.Rows[index]["Nb_Visite"].ToString());

            //S'il n'a pas de visite (càd Disponible ou En pause), on peut activer la sortie de garde
            if (dtGardeMedecin.Rows[index]["StatusGarde"].ToString() == "Disponible" || dtGardeMedecin.Rows[index]["StatusGarde"].ToString() == "En pause")
                bFin.Enabled = true;
            else
                bFin.Enabled = false;

            bDebut.Enabled = false;
            bModif.Enabled = true;
        }

        private void listView3_DoubleClick(object sender, EventArgs e)
        {
            if (Etat == "Lecture")
                MessageBox.Show("Vous devez d'abord choisir un médecin : Pas en Garde (à gauche) ou déjà en garde (à droite). ", "Aide", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                               
            int index = listView3.SelectedItems[0].Index;

            //On peuple les champs
            tBTypeGarde.Text = dtGarde.Rows[index]["TypeGarde"].ToString();
            
           
            //Pour les variables globales et les boutons                      
            IdGarde = int.Parse(dtGarde.Rows[index]["IdGarde"].ToString());
            }                                 
        }

        private void listView4_DoubleClick(object sender, EventArgs e)
        {
            //Smartphone
            if (Etat == "Lecture")
                MessageBox.Show("Vous devez d'abord choisir un médecin : Pas en Garde (à gauche) ou déjà en garde (à droite). ", "Aide", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {                
                int index = listView4.SelectedItems[0].Index;

                //On regarde que ce Smartphone n'est pas déjà attribué à un autre médecin en garde
                if (FonctionsAppels.VerifSmartphone(int.Parse(dtSmartphone.Rows[index]["IdSmartphone"].ToString())) == "OK")
                {
                    //On peuple les champs
                    tBSmartphone.Text = dtSmartphone.Rows[index]["NomSmartphone"].ToString();

                    //Pour les variables globales et les boutons                      
                    Smartphone = int.Parse(dtSmartphone.Rows[index]["IdSmartphone"].ToString());
                }
                else
                {
                    MessageBox.Show("Ce Smartphone est déjà attribué à un autre médecin qui travail, veuillez en choisir un autre. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }                                 
        }

        private void bDebut_Click(object sender, EventArgs e)
        {
            //On valide un début de garde (nvlle garde)
            if (VerifSaisie() == "OK" && Etat == "Ajout")
            {
                //On charge les paramètres (pour recup des coordonnées de la centrale)
                DataTable Param = new DataTable();
                Param = FonctionsAppels.ChargeParam();

                //Conversion de la virgule decimal en point
                NumberFormatInfo n = new NumberFormatInfo();
                n.NumberDecimalSeparator = ".";

                if (Param.Rows.Count > 0)
                {
                    LatCentrale = Double.Parse(Param.Rows[0]["CentraleLat"].ToString(), n);
                    LngCentrale = Double.Parse(Param.Rows[0]["CentraleLng"].ToString(), n);
                }

                //On créer un Enreg dans Engarde
                if (AjoutEnreg() == "OK")
                {
                    //On incrémente le compteur de rafraichissement (base)
                    FonctionsAppels.IncrementeRafraichissement();
                    this.Close();
                }
            }
            else
                MessageBox.Show("Les conditions ne sont pas satisfaites pour mettre en garde ce médecin. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void bModif_Click(object sender, EventArgs e)
        {
            //On valide les modificatons (garde en cours)
            if (VerifSaisie() == "OK" && Etat == "Modif")
            {
                //On met à jour l'enreg dans Engarde
                if (UpdateEnreg() == "OK")
                {
                    //On incrémente le compteur de rafraichissement (base)
                    FonctionsAppels.IncrementeRafraichissement();
                    this.Close();
                }
            }
            else
                MessageBox.Show("Les conditions ne sont pas satisfaites pour mettre à jour la garde de ce médecin. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void bFin_Click(object sender, EventArgs e)
        {
            //On termine la garde (garde en cours)
            if (VerifSaisie() == "OK" && Etat == "Modif")
            {
                //On met à jour l'enreg dans Engarde
                if (TermineGarde() == "OK")
                {
                    //On incrémente le compteur de rafraichissement (base)
                    FonctionsAppels.IncrementeRafraichissement();
                    this.Close();
                }
            }
            else
                MessageBox.Show("Les conditions ne sont pas satisfaites pour clore la garde de ce médecin. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        //vérification des saisies
        private string VerifSaisie()
        {
            string retour = "KO";
            int V1 = 0;
         
            if (CodeMedecin != -1)
                V1 += 1;
            else
                MessageBox.Show("Vous devez choisir un médecin. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (IdGarde != -1)
                V1 += 1;
            else
                MessageBox.Show("Vous devez choisir une garde. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);


            if (Smartphone != -1)
                V1 += 1;
            else
                MessageBox.Show("Vous devez attribuer un smartphone à ce médecin. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                

            //En fonction des résultats
            switch (V1)
            {
                case 3: retour = "OK"; break;
                default: retour = "KO"; break;
            }

            return retour;
        }


        //On ajoute un nvl enregistrement 
        public string AjoutEnreg()
        {
            string Retour = "KO";

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            //On déclare ici la requete (à cause de la transaction)
            string SqlStr0 = "";
            string SqlStr1 = "";
            string SqlStr2 = "";
            string SqlStr3 = "";

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Def de la requete
                SqlStr0 = "INSERT INTO engarde ";
                SqlStr0 += " (IdGarde, CodeMedecin, IdSmartphone, StatusGarde, Nb_Visite, DateDebGarde)";
                SqlStr0 += " VALUES('" + IdGarde.ToString() + "','" + CodeMedecin.ToString() + "','" + Smartphone.ToString();
                SqlStr0 += "','Disponible', 0,'" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(),"MariaDb") + "')";

                //désatribuer eventuellement l'ancien Smartphone
                SqlStr1 = "UPDATE smartphone SET CodeMedecin = 0";
                SqlStr1 += " WHERE CodeMedecin = '" + CodeMedecin.ToString() + "'";

                //Attribuer eventuellement le nvx smartphone
                SqlStr2 = "UPDATE smartphone SET CodeMedecin = '" + CodeMedecin.ToString() + "'";
                SqlStr2 += " WHERE IdSmartphone = '" + Smartphone.ToString() + "'";
               
                //Ajouter un enregistrement pour la géolocalisation(Position centrale SOS/defaut) et Alarme silentieuse
                SqlStr3 = "INSERT INTO geoloc ";
                SqlStr3 += " (CodeMedecin, Lat, Lng, Vitesse, Alarme) ";
                SqlStr3 += " VALUES(" + CodeMedecin + "," + LatCentrale + "," + LngCentrale + ", 0, 0)";                

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr3; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    //on valide la transaction
                    trans.Commit();

                    Retour = "OK";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la mise en garde (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la mise en garde. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return Retour;
        }


        //On met à jour un enregistrement
        private string UpdateEnreg()
        {
            string Retour = "KO";

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            //On déclare ici la requete (à cause de la transaction)
            string SqlStr0 = "";
            string SqlStr1 = "";
            string SqlStr2 = "";

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Def de la requete
                SqlStr0 = "UPDATE engarde ";
                SqlStr0 += " SET IdGarde = '" + IdGarde.ToString() + "', IdSmartphone = '" + Smartphone.ToString() + "',";
                SqlStr0 += " DateDebGarde = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "'";
                SqlStr0 += " WHERE IdStatusGarde = '" + IdStatusGarde.ToString() + "'";

                //désatribuer eventuellement l'ancien Smartphone
                SqlStr1 = "UPDATE smartphone SET CodeMedecin = 0";
                SqlStr1 += " WHERE CodeMedecin = '" + CodeMedecin.ToString() + "'";                
                
                //Attribuer eventuellement le nvx smartphone
                SqlStr2 = "UPDATE smartphone SET CodeMedecin = '" + CodeMedecin.ToString() + "'";
                SqlStr2 += " WHERE IdSmartphone = '" + Smartphone.ToString() + "'";
                
                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();
                    Retour = "OK";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la modification de la garde du médecin (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la modification de la garde du médecin. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return Retour;
        }

        //On termine la garde en cours
        private string TermineGarde()
        {
            string Retour = "KO";

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            //On déclare ici la requete (à cause de la transaction)
            string SqlStr0 = "";
            string SqlStr1 = "";
            string SqlStr2 = "";

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Ajout d'un enregistrement dans HistoGarde
                SqlStr0 = "INSERT INTO histogarde ";
                SqlStr0 += " (CodeMedecin, IdSmartphone, IdGarde, DebGarde, FinGarde, TypeGarde, Nb_Visite)";
                SqlStr0 += " VALUES('" + CodeMedecin.ToString() + "','" + Smartphone.ToString() + "','" + IdGarde.ToString() + "'";
                SqlStr0 += ",'" + FonctionsAppels.convertDateMaria(DateDebGarde.ToString(), "MariaDb") + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "'";
                SqlStr0 += ",'" + tBTypeGarde.Text + "'," + Nb_Visite  + ")";
           
                //Puis suppression de l'enregistrement engarde
                SqlStr1 = "DELETE FROM engarde ";                
                SqlStr1 += " WHERE IdStatusGarde = '" + IdStatusGarde.ToString() + "'";

                //et Geoloc
                SqlStr2 = "DELETE FROM geoloc ";
                SqlStr2 += " WHERE CodeMedecin = '" + CodeMedecin.ToString() + "'";

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();
                    Retour = "OK";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la fin de garde du médecin (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la fin de garde du médecin. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return Retour;
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            if (Etat != "Lecture")
            {
                var result = MessageBox.Show("Il y a des modification en cours...Voulez vous les abandonner? ", "Abandon", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    this.Close();
            }
            else
                this.Close();
        }
     

      
    }
}



//A faire:
