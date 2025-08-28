using MySqlConnector;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace SRegulV2
{

    public partial class Form1 : Form
    {

        enum ListView2Columns
        {
            CodeMedecin = 0,
            NombreDeVisite,
            TitreEmployee,
            NomMed,
            Garde,
            HAttribut,
            HEntrée,
            Commune,
            Adresse,
            NumAppel
        }

        //Chargement des paramètres du programme      
        public static DataTable dtProv = new DataTable();       //La provenance
        public static DataTable dtCommune = new DataTable();    //Les communes
        public static DataTable dtRue = new DataTable();    //Les communes
        public static DataTable dtMotif = new DataTable();      //Les Motifs
        public static DataTable dtMotifAnnul = new DataTable();   //Les motifs Annulation
        public static DataTable dtCpt = new DataTable();      //Le compteur
        public static DataTable dtAssurance = new DataTable();      //Les assurances

        public static DataTable dtEtatMed = new DataTable();

        public static bool ActivationCTI = true;            //Activation CTI activé / defaut
        public static bool ActivationBandeAudio = true;     //Activation de la récupération des bandes audios activé / defaut
        public static int RafraichissementTimer = 60000;    //Timer 1 minute/defaut
        public static string NotifFinVisite = "";           //Son de fin de visite
        public static string NotifMessage = "";             //Son de Notification de visite
        public static int DureeAffichageMessages = 24;   //Durée affichages des messages sur le dispatch en heure        
        public static int TypeCarte = 1;                 //Type de carte utilisée   
        public int CompteurFormVisite = 0;
        public int NBFormVisiteMax = 1;                  //Nombre de form Max de form visite ouverte en même temps
        public int CompteurFormRessaisieVisite = 0;                  //Nombre de form Max de form Re-saisie visite ouverte en même temps (On en veut qu'une)        
        public static String[] Utilisateur = new String[8];  //idutilisateur, Droit, Nom ...Attention tableau de longueur 7 (1 à 8) mais index de 0 à 7               
        public static string EmailTAFrom = "";        //Email pour envoi des email TA
        public static string PassMailTA = "";         //Mot de passe de l'email TA
        public static string EmailTADest = "";      //Mail du destinataire TA
        public static string EmailInfFrom = "";        //Email pour envoi des email aux infirmières
        public static string PassMailInf = "";         //Mot de passe de l'email aux infirmières
        public static string EmailInfDest = "";      //Mail du destinataire aux infirmières
        public static string EmailMutuaideFrom = "";        //Email pour envoi des email a Mutuaide
        public static string PassMailMutuaide = "";         //Mot de passe de l'email Mutuaide
        public static bool ActivationSurveillanceMailSMA = true;   //On active la surveillance des emails SMA        

        //private EmailMonitor _emailMonitor;
        private RecupMailImap _RecupMailImap;

        //Pour Affecter le comteur depuis FonctionsAppels
        private static int Compteur;
        public static int compteur
        {
            get { return Compteur; }
            set { Compteur = value; }
        }

        //Pour Affecter les variables depuis les autres classes
        private static string token;
        public static string Token
        {
            get { return token; }
            set { token = value; }
        }

        private static string ligne;
        public static string Ligne
        {
            get { return ligne; }
            set { ligne = value; }
        }


        public Form1()
        {
            InitializeComponent();

            //Visites en attentes
            listView1.Columns.Add("Visite", 1);  //Colonne invisible
            listView1.Columns.Add("Urg.", 40);
            listView1.Columns.Add("H Appel", 50);
            listView1.Columns.Add("Attente", 1);    //Attente...Colonne invisible
            listView1.Columns.Add("Adresse", 140);
            listView1.Columns.Add("Commune", 140);
            listView1.Columns.Add("Nom Prenom", 120);
            listView1.Columns.Add("Age", 40);
            listView1.Columns.Add("Motif1", 110);
            listView1.Columns.Add("Motif2", 110);
            listView1.Columns.Add("Provenance", 80);
            listView1.Columns.Add("Assurance", 80);
            listView1.Columns.Add("Médecin", 120);
            listView1.Columns.Add("Cond. Part.", 120);
            listView1.View = View.Details;    //Pour afficher les subItems

            //Listes des médecins
            listView2.Columns.Add("N° Med", 50);
            listView2.Columns.Add("Nb V", 40);      //Nombre de visite du médecin depuis le début de sa garde
            listView2.Columns.Add("Titre d'emploi", 80); // Medsister or Doctor
            listView2.Columns.Add("Nom Med", 130);
            listView2.Columns.Add("Garde", 43);
            listView2.Columns.Add("H. Attribut", 65);
            listView2.Columns.Add("H. Entrée V", 70);
            listView2.Columns.Add("Commune", 110);
            listView2.Columns.Add("Adresse", 220);
            listView2.Columns.Add("NumAppel", 1);      //Colonne invisible
            listView2.View = View.Details;    //Pour afficher les subItems    

            //Liste des messages
            listView3.Columns.Add("N° Mess", 1);     //Colonne invisible
            listView3.Columns.Add("Auteur", 130);
            listView3.Columns.Add("Message", 650);
            listView3.Columns.Add("Destinataire", 110);
            listView3.Columns.Add("CodeAuteur", 1);
            listView3.Columns.Add("CodeDest", 1);
            listView3.Columns.Add("DateMessage", 120);
            listView3.View = View.Details;    //Pour afficher les subItems    

            splitContainer1.FixedPanel = FixedPanel.Panel1;    //On fixe la taille du panel1 (ou il y a les boutons)   

            //Par défaut Compteur à 1
            Compteur = 1;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //on stop le timer
            timer1.Stop();
            
            //On affiche le login
            FLogin flogin = new FLogin();

            if (flogin.ShowDialog() == DialogResult.OK)
            {
                //on charge le tableau de valeurs (paramètres de l'utilisateur)
                for (int x = 0; x < Utilisateur.Length; ++x)
                {
                    Utilisateur[x] = flogin.user[x];
                }

                //On affiche l'utilisateur dans la barre des titre
                this.Text = "REGULATION" + "                                                                                                          "
                          + "                                                 Bonjour " + Utilisateur[3];

                //En fonction des droits .... 0 : Désactivé, 1 Invité, 2 regul, 3 Chef Regul, 10 Admin, 11 Developpeur
                switch (int.Parse(Utilisateur[1]))
                {
                    case 0: Application.Exit(); return; //il a pas le droit, on le sort
                    case 1:
                        MenuMotif.Enabled = false; MenuProvenances.Enabled = false; MenuRues.Enabled = false; MenuCommune.Enabled = false; MenuGarde.Enabled = false;
                        MenuSmartphone.Enabled = false; MenuUtilisateur.Enabled = false; MenuMedecin.Enabled = false; MenuCTI.Enabled = false;
                        MenuParametresDivers.Enabled = false; Menuressaisie.Enabled = false; MenuParamUtil.Enabled = true; MenutelUtil.Enabled = true;
                        MenuRechEvent.Enabled = true; MenuHistoAttribSmart.Enabled = true; MenudebloquerMed.Enabled = false; menuGestDoc.Enabled = false; break;
                    case 2:
                        MenuMotif.Enabled = false; MenuProvenances.Enabled = false; MenuRues.Enabled = false; MenuCommune.Enabled = false; MenuGarde.Enabled = false;
                        MenuSmartphone.Enabled = false; MenuUtilisateur.Enabled = false; MenuMedecin.Enabled = false; MenuCTI.Enabled = true;
                        MenuParametresDivers.Enabled = false; Menuressaisie.Enabled = true; MenuParamUtil.Enabled = true; MenutelUtil.Enabled = true;
                        MenuRechEvent.Enabled = true; MenuHistoAttribSmart.Enabled = true; MenudebloquerMed.Enabled = false; menuGestDoc.Enabled = false; break;
                    case 3:
                        MenuMotif.Enabled = true; MenuProvenances.Enabled = true; MenuRues.Enabled = true; MenuCommune.Enabled = true; MenuGarde.Enabled = false;
                        MenuSmartphone.Enabled = false; MenuUtilisateur.Enabled = false; MenuMedecin.Enabled = false; MenuCTI.Enabled = true;
                        MenuParametresDivers.Enabled = false; Menuressaisie.Enabled = true; MenuParamUtil.Enabled = true; MenutelUtil.Enabled = true;
                        MenuRechEvent.Enabled = true; MenuHistoAttribSmart.Enabled = true; MenudebloquerMed.Enabled = false; menuGestDoc.Enabled = false; break;
                    //pas util pour le momment case 4: MenuMotifs.Enabled = false; MenuProvenance.Enabled = false; MenuRue.Enabled = false; MenuCommunes.Enabled = false;
                    //MenuGardes.Enabled = false; MenuSmartphones.Enabled = false; MenuUtilisateurs.Enabled = false; MenuMedecins.Enabled = false; MenuCTI.Enabled = true;
                    //MenuParametresDivers.Enabled = false; MenudebloquerMed.Enabled = false; break;
                    case 10:
                        MenuMotif.Enabled = true; MenuProvenances.Enabled = true; MenuRues.Enabled = true; MenuCommune.Enabled = true; MenuGarde.Enabled = true;
                        MenuSmartphone.Enabled = true; MenuUtilisateur.Enabled = true; MenuMedecin.Enabled = true; MenuCTI.Enabled = true; MenuParametresDivers.Enabled = true;
                        Menuressaisie.Enabled = true; MenuParamUtil.Enabled = true; MenutelUtil.Enabled = true; MenuRechEvent.Enabled = true; MenuHistoAttribSmart.Enabled = true;
                        MenudebloquerMed.Enabled = true; menuGestDoc.Enabled = true; break;
                    case 11:
                        MenuMotif.Enabled = true; MenuProvenances.Enabled = true; MenuRues.Enabled = true; MenuCommune.Enabled = true; MenuGarde.Enabled = true;
                        MenuSmartphone.Enabled = true; MenuUtilisateur.Enabled = true; MenuMedecin.Enabled = true; MenuCTI.Enabled = true; MenuParametresDivers.Enabled = true;
                        Menuressaisie.Enabled = true; MenuParamUtil.Enabled = true; MenutelUtil.Enabled = true; MenuRechEvent.Enabled = true; MenuHistoAttribSmart.Enabled = true;
                        MenudebloquerMed.Enabled = true; menuGestDoc.Enabled = true; break;
                }

                MenuUtilisateur.Visible = true;

                //On charge les preferences de l'utilisateur
                ChargePreferences(Utilisateur[0]);

                //On charge les paramètres                        
                ChargeParametres();

                //On charge les listes
                ChargeAppelsNonAttribues();   //Liste des appels en attente
                ChargeAppelsAttribues();      //Liste des médecins en garde avec leurs appels (s'il y en a)
                ChargeMessages();             //Liste des messages
                ChargeAideRegulation();        //Aide à la régulation

                lnbFicheOuverte.Text = "0";
              
                string retour = FonctionsAppels.SupprimeRDVPerime();        //Suppression des rendez vous de la veille

                if (FonctionsAppels.NombreRDV() > 0)            //on retourne le nombre de rdv en attente pour la couleur du bouton
                    bRDV.ImageIndex = 14;     //b rouge
                else 
                    bRDV.ImageIndex = 13;      //b vert
                
                //****Pour relever les emails SMA*** désactivé à la demande de Xavier le 07.03.2024
                /*if (ActivationSurveillanceMailSMA == true)
                {
                    //Ecoute l'arrivée de nouveau Mail
                    string host = ConfigurationManager.AppSettings["ServeurImapOVH"].ToString();
                    int port = int.Parse(ConfigurationManager.AppSettings["PortImap"].ToString());
                    string username = EmailMutuaideFrom;
                    string password = PassMailMutuaide;
                    bool useSsl = true;

                    SecureSocketOptions SslOptions = SecureSocketOptions.Auto;

                    try
                    {
                        _RecupMailImap = new RecupMailImap(host, port, SslOptions, username, password);
                        _RecupMailImap.StartSurveilleImap();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    // Fin nvx messages
                }*/

                //on demmarre le timer
                timer1.Start();
            }
            else
            {
                try
                {
                    Application.Exit();   //sinon on ferme l'appli
                }
                catch
                {

                }
                return;
            }
        }


        //Chargement des preferences utilisateur au lancement de la form
        private void ChargePreferences(string IdUtilisateur)
        {
            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT * FROM pref_utilisateur WHERE IdUtilisateur = '" + IdUtilisateur + "'";
                cmd.CommandText = sqlstr0;

                DataTable dtParam = new DataTable();
                dtParam.Load(cmd.ExecuteReader());    //on execute

                if (dtParam.Rows.Count > 0)
                {
                    NotifFinVisite = dtParam.Rows[0]["Notif_Medecin"].ToString() + ".wav";           //Son de fin de visite
                    NotifMessage = dtParam.Rows[0]["Notif_Message"].ToString() + ".wav";             //Son de nvx message
                    TypeCarte = int.Parse(dtParam.Rows[0]["Typecarte"].ToString());
                }
                else    //Valeurs par defaut
                {
                    NotifFinVisite = "Clavecin.wav";           //Son de fin de visite
                    NotifMessage = "sms-airport.wav";             //Son de nvx message
                    TypeCarte = 1;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la récupération des paramètres." + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


        //Chargement des paramètres au lancement de la form
        private void ChargeParametres()
        {
            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Le compteur de rafraichissement
                string sqlstr0 = "SELECT Compteur FROM rafraichissement";
                cmd.CommandText = sqlstr0;

                dtCpt.Load(cmd.ExecuteReader());    //on execute

                //Les Provenances
                sqlstr0 = "SELECT IdProvenance, LibelleProvenance FROM provenance ORDER BY LibelleProvenance";
                cmd.CommandText = sqlstr0;

                dtProv.Load(cmd.ExecuteReader());    //on execute

                //Les communes
                sqlstr0 = "SELECT IdCommune, NomCommune, CodePostal FROM commune ORDER BY NomCommune";

                cmd.CommandText = sqlstr0;

                dtCommune.Load(cmd.ExecuteReader());    //on execute

                //Les rues
                sqlstr0 = " SELECT r.IdRue, r.NomRue, r.Commune, r.CodePostal, ";
                sqlstr0 += "            CASE WHEN c.Pays is null THEN 'Suisse' ELSE c.Pays END AS Pays ";
                sqlstr0 += " FROM rue r LEFT JOIN commune c ON c.CodePostal = r.CodePostal ";
                sqlstr0 += " GROUP BY IdRue ";
                sqlstr0 += " ORDER BY r.NomRue, r.Commune ";

                cmd.CommandText = sqlstr0;

                dtRue.Load(cmd.ExecuteReader());    //on execute

                //Les motifs
                sqlstr0 = "SELECT IdMotif, LibelleMotif, Urgence, TypeMotif FROM motif ORDER BY LibelleMotif";

                cmd.CommandText = sqlstr0;

                dtMotif.Load(cmd.ExecuteReader());    //on execute

                //Les motifs annulation
                sqlstr0 = "SELECT IdMotif, LibelleMotif, Urgence, TypeMotif FROM motif WHERE TypeMotif = 'A' ORDER BY LibelleMotif";
                cmd.CommandText = sqlstr0;
                dtMotifAnnul.Load(cmd.ExecuteReader());    //on execute

                //Les assurances
                sqlstr0 = "SELECT IdAssurance, NomAssurance FROM assurance ORDER BY NomAssurance";
                cmd.CommandText = sqlstr0;
                dtAssurance.Load(cmd.ExecuteReader());    //on execute

                //Les paramètres divers
                sqlstr0 = "SELECT ActivationCTI, Timer, ActivationBandeAudio, DureeMessages, NbFicheVisite,";
                sqlstr0 += " EmailTA, PassMailTA, EmailTADest, EmailInfirmiere, PassMailInf, EmailInfDest,";
                sqlstr0 += " EmailMutuaideFrom, PassMailMutuaide, ActivationMailSMA FROM param_divers";

                cmd.CommandText = sqlstr0;
                DataTable dtParam = new DataTable();
                dtParam.Load(cmd.ExecuteReader());    //on execute

                Compteur = int.Parse(dtCpt.Rows[0]["Compteur"].ToString());
                ActivationCTI = Boolean.Parse(dtParam.Rows[0]["ActivationCTI"].ToString());    //CTI
                RafraichissementTimer = int.Parse(dtParam.Rows[0]["Timer"].ToString()) * 1000;  //Rafraichissement du dispatch
                ActivationBandeAudio = Boolean.Parse(dtParam.Rows[0]["ActivationBandeAudio"].ToString());    //On va recupérer les bandes audio              
                DureeAffichageMessages = int.Parse(dtParam.Rows[0]["DureeMessages"].ToString());     //Durée d'affichage des messages (24h/defaut)
                NBFormVisiteMax = int.Parse(dtParam.Rows[0]["NbFicheVisite"].ToString());   //Nb de Fiche Visite ouverte en meme temps
                EmailTAFrom = dtParam.Rows[0]["EmailTA"].ToString();    //compte Email pour l'envoi des donnees TA
                PassMailTA = dtParam.Rows[0]["PassMailTA"].ToString();    //Mot de passe de l'email d'envoi TA
                EmailTADest = dtParam.Rows[0]["EmailTADest"].ToString();    //mail du destinataire TA
                PassMailMutuaide = dtParam.Rows[0]["PassMailMutuaide"].ToString();    //Mot de passe de l'email d'envoi Mutuaide

                EmailInfFrom = dtParam.Rows[0]["EmailInfirmiere"].ToString();    //compte Email pour l'envoi des donnees aux infirmières
                PassMailInf = dtParam.Rows[0]["PassMailInf"].ToString();    //Mot de passe de l'email d'envoi aux infirmières
                EmailInfDest = dtParam.Rows[0]["EmailInfDest"].ToString();    //mail du destinataire (infirmières)
                EmailMutuaideFrom = dtParam.Rows[0]["EmailMutuaideFrom"].ToString();    //compte Email pour l'envoi des donnees a Mutuaide
                ActivationSurveillanceMailSMA = bool.Parse(dtParam.Rows[0]["ActivationMailSMA"].ToString());    //Activation de la surveillance de réception des mail SMA

                //Affectation du temps de rafraichissement
                timer1.Interval = RafraichissementTimer;
                timer1.Enabled = true;

                //On desactive certains boutons
                bEtatMed.ImageIndex = 2;
                bEtatMed.Enabled = false;
                bVisiteJournaliere.Enabled = false;
                bVisiteMed.Enabled = false;
                bFindegarde.Enabled = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la récupération des paramètres." + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //Récup des appels en cours (Non attribués) ou des pré-attribués
        private void ChargeAppelsNonAttribues()
        {
            DataTable dtAppelsNonAttribues = new DataTable();
            dtAppelsNonAttribues = FonctionsAppels.ChargeListeAppelsNonAttribues();

            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dtAppelsNonAttribues.Rows.Count; i++)
            {
                //pour l'age
                int age = 0;
                if (dtAppelsNonAttribues.Rows[i]["DateNaissance"].ToString() != "" || dtAppelsNonAttribues.Rows[i]["DateNaissance"] != DBNull.Value)
                    age = FonctionsAppels.CalculeAge(DateTime.Parse(dtAppelsNonAttribues.Rows[i]["DateNaissance"].ToString()));
                else
                    age = 0;

                ListViewItem item = new ListViewItem(dtAppelsNonAttribues.Rows[i]["Num_Appel"].ToString());
                item.SubItems.Add(dtAppelsNonAttribues.Rows[i]["Urgence"].ToString());
                item.SubItems.Add(dtAppelsNonAttribues.Rows[i]["heure"].ToString());
                item.SubItems.Add(dtAppelsNonAttribues.Rows[i]["Attente"].ToString());
                item.SubItems.Add(dtAppelsNonAttribues.Rows[i]["Adresse"].ToString());
                item.SubItems.Add(dtAppelsNonAttribues.Rows[i]["Commune"].ToString() + ", " + dtAppelsNonAttribues.Rows[i]["CodePostal"].ToString());
                item.SubItems.Add(dtAppelsNonAttribues.Rows[i]["NomPrenom"].ToString());
                item.SubItems.Add(age.ToString());
                item.SubItems.Add(dtAppelsNonAttribues.Rows[i]["Motif1"].ToString());
                item.SubItems.Add(dtAppelsNonAttribues.Rows[i]["Motif2"].ToString());
                item.SubItems.Add(dtAppelsNonAttribues.Rows[i]["Provenance"].ToString());
                item.SubItems.Add(dtAppelsNonAttribues.Rows[i]["Assurance"].ToString());
                item.SubItems.Add(dtAppelsNonAttribues.Rows[i]["Med"].ToString());
                item.SubItems.Add(dtAppelsNonAttribues.Rows[i]["CondParticuliere"].ToString());

                //On colorise
                switch (dtAppelsNonAttribues.Rows[i]["Provenance"].ToString())
                {
                    case "144": item = ColoriseListview1("144", item, 6); item = ColoriseListview1Complete("144", item); break;
                    case "POLICE": item = ColoriseListview1("Police", item, 6); item = ColoriseListview1Complete("Police", item); break;
                    case "Télé Alarme": item = ColoriseListview1("TA", item, 6); item = ColoriseListview1Complete("TA", item); break;
                    case "HOTELS": item = ColoriseListview1("Hotels", item, 6); item = ColoriseListview1Complete("Hotels", item); break;
                    case "PRIVE-PARTICULIER": item = ColoriseListview1("Prive-Particulier", item, 6); break;
                    default: item = ColoriseListview1("Disponible", item, 6); break;
                }

                //On gère la couleur de l'urgence
                if (dtAppelsNonAttribues.Rows[i]["Urgence"].ToString() == "1")
                    item = ColoriseListview1Complete("Urgence1", item);

                //Si le patient a rappelé, on souligne le tout
                if (dtAppelsNonAttribues.Rows[i]["PatientRappel"].ToString() == "1")
                    item = SouligneListview1Complete(item);

                listView1.Items.Add(item);
            }

            listView1.EndUpdate();  //Rafraichi le controle 

            LNbVisiteEnAttente.Text = dtAppelsNonAttribues.Rows.Count.ToString();
        }


        //Récup des médecins en garde, et EVENTUELLEMENT la liste des appels Attribués en cours (Table Status_visite dont Status in = AT, AQ, V)        
        private void ChargeAppelsAttribues()
        {
            DataTable dtAppelsAttribues = new DataTable();
            dtAppelsAttribues = FonctionsAppels.ChargeListeAppelsAttribues();

            //On vide la liste pour la rafraichir                
            listView2.BeginUpdate();
            listView2.Items.Clear();

            for (int i = 0; i < dtAppelsAttribues.Rows.Count; i++)
            {
                //On determine les différentes dates
                DateTime? Hattrib = FonctionsAppels.HeureOP(Int32.Parse(dtAppelsAttribues.Rows[i]["Num_Appel"].ToString()), "Attribution");   //Heure d'attribution
                DateTime? DebVisite = FonctionsAppels.HeureOP(Int32.Parse(dtAppelsAttribues.Rows[i]["Num_Appel"].ToString()), "Début de visite");  //Heure début de visite

                ListViewItem item = new ListViewItem(dtAppelsAttribues.Rows[i]["CodeMedecin"].ToString());  //Code Médecin
                item.SubItems.Add(dtAppelsAttribues.Rows[i]["Nb_Visite"].ToString());                       //Nb visite depuis debut de garde
                item.SubItems.Add(dtAppelsAttribues.Rows[i]["TitreEmployee"].ToString());                   //Titre Employee
                item.SubItems.Add(dtAppelsAttribues.Rows[i]["NomMedecin"].ToString());                      //NomMedecin
                item.SubItems.Add(dtAppelsAttribues.Rows[i]["TypeGarde"].ToString());                       //Type de garde

                //On ne récupère que les heures
                //Heure de la dernière attribution
                if (Hattrib.HasValue)
                    item.SubItems.Add(Hattrib.Value.ToString("HH:mm"));
                else
                    item.SubItems.Add(Hattrib.ToString());

                //Heure début de visite
                if (DebVisite.HasValue)
                    item.SubItems.Add(DebVisite.Value.ToString("HH:mm"));
                else
                    item.SubItems.Add(DebVisite.ToString());


                item.SubItems.Add(dtAppelsAttribues.Rows[i]["Commune"].ToString());                         //Commune
                item.SubItems.Add(dtAppelsAttribues.Rows[i]["Adresse"].ToString());                         //Adresse                  
                item.SubItems.Add(dtAppelsAttribues.Rows[i]["Num_Appel"].ToString());                       //Colonne invisible                 

                //On colorise
                switch (dtAppelsAttribues.Rows[i]["StatusGarde"].ToString())
                {
                    case "Disponible":
                        item = ColoriseListview2("Disponible", item, (int)ListView2Columns.TitreEmployee);
                        item = ColoriseListview2("Disponible", item, (int)ListView2Columns.NomMed);
                        break;
                    case "Attribuée":
                        item = ColoriseListview2("Attribuée", item, (int)ListView2Columns.TitreEmployee);
                        item = ColoriseListview2("Attribuée", item, (int)ListView2Columns.NomMed);
                        break;
                    case "Acquitée":
                        item = ColoriseListview2("Acquitée", item, (int)ListView2Columns.TitreEmployee);
                        item = ColoriseListview2("Acquitée", item, (int)ListView2Columns.NomMed);
                        break;
                    case "Visite":
                        item = ColoriseListview2("Visite", item, (int)ListView2Columns.TitreEmployee);
                        item = ColoriseListview2("Visite", item, (int)ListView2Columns.NomMed);
                        break;
                    case "En pause":
                        item = ColoriseListview2("En pause", item, (int)ListView2Columns.TitreEmployee);
                        item = ColoriseListview2("En pause", item, (int)ListView2Columns.NomMed);
                        break;
                    default:
                        item = ColoriseListview2("Disponible", item, (int)ListView2Columns.TitreEmployee);
                        item = ColoriseListview2("Disponible", item, (int)ListView2Columns.NomMed);
                        break;
                }

                listView2.Items.Add(item);
               
            }

            listView2.EndUpdate();  //Rafraichi le controle
           
            //on regarde si l'Etat des médecins ont changé par rapport au précédent rafraîchissement
            RafraichiEtat(dtAppelsAttribues);
        }


        //Chargement des messages
        private void ChargeMessages()
        {
            DataTable dtMessages = new DataTable();
            dtMessages = FonctionsAppels.ChargeListeMessages(DureeAffichageMessages);

            //On vide la liste pour la rafraichir                
            listView3.BeginUpdate();
            listView3.Items.Clear();

            for (int i = 0; i < dtMessages.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtMessages.Rows[i]["IdMessage"].ToString());
                item.SubItems.Add(dtMessages.Rows[i]["NomAuteur"].ToString());
                item.SubItems.Add(dtMessages.Rows[i]["Message"].ToString());
                item.SubItems.Add(dtMessages.Rows[i]["NomDestinataire"].ToString());
                item.SubItems.Add(dtMessages.Rows[i]["Auteur"].ToString());
                item.SubItems.Add(dtMessages.Rows[i]["Destinataire"].ToString());
                item.SubItems.Add(dtMessages.Rows[i]["DateM"].ToString());
                listView3.Items.Add(item);

                //Si ça fait - de RafraichissementTimer en " que le message a été écrit, alors notif.
                var diffInSeconds = (DateTime.Now - DateTime.Parse(dtMessages.Rows[i]["DateM"].ToString())).TotalSeconds;

                Console.WriteLine(diffInSeconds);

                if (diffInSeconds < (RafraichissementTimer / 1000))   //car le RafraichissementTimer est en milli seconde
                {
                    Beeper(NotifMessage);
                }
            }

            listView3.EndUpdate();  //Rafraichi le controle           
        }

        //Chargement des aides à la régulation
        private void ChargeAideRegulation()
        {
            //On nettoye rTBoxAideRegul
            rTBoxAideRegul.Clear();

            //On met le titre
            string Titre = "Aide à la régulation :";
            int longueur = rTBoxAideRegul.Text.Length;   //Longueur d'un éventuel texte existant

            //Ajout du titre
            rTBoxAideRegul.AppendText(Titre + "\r\n\r\n");

            //On selectionne le titre (qu'on vient d'ajouter a la box)
            rTBoxAideRegul.Select(longueur, Titre.Length);

            //On détermine les paramètres de la Font
            rTBoxAideRegul.SelectionFont = new Font(rTBoxAideRegul.Font, FontStyle.Bold);
            // rTBoxRapport.SelectionColor = Color.Red;                                        

            //Pour déterminer le prochain médecin qui doit recevoir une visite 
            string Medecin = FonctionsAppels.ProchainMedecin();
            if (Medecin != "")
            {
                string Message = " est le prochain médecin auquel il faudra attribuer une visite.\r\n";
                //On met le nom prenom du médecin en gras              
                rTBoxAideRegul.AppendText(Medecin + Message);
                rTBoxAideRegul.Select(Titre.Length + 2, Medecin.Length);    //+2 car \r\n
                rTBoxAideRegul.SelectionFont = new Font(rTBoxAideRegul.Font, FontStyle.Bold);
            }
            else
            {
                rTBoxAideRegul.AppendText("Aucun médecin de disponible, pour le moment.");
            }


            //Alarme Silencieuse?
            DataTable dtmedecinEnDetresse = new DataTable();
            dtmedecinEnDetresse = FonctionsAppels.Alarme();

            if (dtmedecinEnDetresse.Rows.Count > 0)
            {
                //On a qque chose...
                //On met le titre
                string TitreAlerte = "ALERTE MEDECIN EN DETRESSE !!!";
                int longueurTitre = rTBoxAideRegul.Text.Length;   //Longueur d'un éventuel texte existant

                //On determine la longueur du texte déjà existant
                longueur = rTBoxAideRegul.Text.Length + 2;   //les \r\n

                //Ajout du titre
                rTBoxAideRegul.AppendText("\r\n\r\n" + TitreAlerte + "\r\n");

                //On selectionne le titre (qu'on vient d'ajouter a la box)
                rTBoxAideRegul.Select(longueur, TitreAlerte.Length);

                //On détermine les paramètres de la Font
                rTBoxAideRegul.SelectionFont = new Font(rTBoxAideRegul.Font, FontStyle.Bold);
                rTBoxAideRegul.SelectionColor = Color.Orange;

                for (int i = 0; i < dtmedecinEnDetresse.Rows.Count; i++)
                {
                    //Affichage d'un message                                                   
                    //On determine a nouveaux la longueur du texte déjà existant
                    longueur = rTBoxAideRegul.Text.Length;

                    string MedecinAlerte = dtmedecinEnDetresse.Rows[i]["NomMed"].ToString();
                    string Message = " a déclanché un appel à l'aide.\r\n";

                    //On met le nom prenom du médecin en gras et orange              
                    rTBoxAideRegul.AppendText(MedecinAlerte + Message);
                    rTBoxAideRegul.Select(longueur, MedecinAlerte.Length);    //+2 car \r\n
                    rTBoxAideRegul.SelectionFont = new Font(rTBoxAideRegul.Font, FontStyle.Bold);
                    rTBoxAideRegul.SelectionColor = Color.Orange;

                    //puis le reste du texte en orange normal
                    rTBoxAideRegul.Select(longueur + MedecinAlerte.Length, Message.Length);
                    rTBoxAideRegul.SelectionFont = new Font(rTBoxAideRegul.Font, FontStyle.Regular);
                    rTBoxAideRegul.SelectionColor = Color.Orange;
                }

                //on déclanche le beep
                Beeper("Beep.wav");

            }          //Fin d'il y a des alarmes silencieuses              
        }


        //Rafraichissement des etats des médecins
        private void RafraichiEtat(DataTable dtNvlEtat)
        {
            DataTable dtMaJEtat = new DataTable();

            dtMaJEtat.Clear();
            //Ajout des colonnes
            dtMaJEtat.Columns.Add("CodeMedecin");
            dtMaJEtat.Columns.Add("Nb_Visite");
            dtMaJEtat.Columns.Add("TitreEmployee");
            dtMaJEtat.Columns.Add("NomMedecin");
            dtMaJEtat.Columns.Add("TypeGarde");
            dtMaJEtat.Columns.Add("NomPrenom");
            dtMaJEtat.Columns.Add("Adresse");
            dtMaJEtat.Columns.Add("CodePostal");
            dtMaJEtat.Columns.Add("Commune");
            dtMaJEtat.Columns.Add("Pays");
            dtMaJEtat.Columns.Add("Num_Appel");
            dtMaJEtat.Columns.Add("Latitude");
            dtMaJEtat.Columns.Add("Longitude");
            dtMaJEtat.Columns.Add("StatusGarde");

            try
            {
                //Si c'est le premier demarrage (Table dtEtatMed vide), on la rempli, sinon on compare
                if (dtEtatMed.Rows.Count > 0)
                {
                    for (int i = 0; i < dtNvlEtat.Rows.Count; i++)
                    {
                        DataRow[] Ancien = dtEtatMed.Select("CodeMedecin = '" + dtNvlEtat.Rows[i]["CodeMedecin"].ToString() + "'");

                        if (Ancien.Count() > 0)    //Si on a quelque chose
                        {
                            //On compare l'état précédent avec l'état actuel Et on agit en concéquence               
                            if (dtNvlEtat.Rows[i]["StatusGarde"].ToString() != Ancien[0]["StatusGarde"].ToString())   //ça a changé
                            {
                                switch (dtNvlEtat.Rows[i]["StatusGarde"].ToString())
                                {
                                    case "Disponible": Beeper(NotifFinVisite); break;
                                    case "Attribuée": break;
                                    case "Acquitée": break;
                                    case "Visite": break;
                                    case "En pause": break;
                                    default: break;
                                }
                            }
                        }

                        //on copie l'enregistrement de dtNvlEtat...
                        DataRow row = dtNvlEtat.Rows[i];
                        //dtMaJEtat.ImportRow(row);
                        dtMaJEtat.Rows.Add(row.ItemArray);
                    }

                    //...Pour pouvoir mettre à jour dtEtatMed (NvlEtat deviendra l'AncienEtat au prochain rafraichissement)
                    dtEtatMed.Rows.Clear();
                    dtEtatMed = dtMaJEtat.Copy();
                }
                else     //1er demarrage table vide
                    dtEtatMed = dtNvlEtat.Copy();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        //Beeper
        private void Beeper(string SonAJouer)
        {
            SoundPlayer FinVisiteSound = new SoundPlayer(Application.StartupPath + @"\Sons\" + SonAJouer);
            FinVisiteSound.Play();
        }


        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listView1.DoDragDrop(listView1.SelectedItems, DragDropEffects.Move);
        }


        //Affectation d'une visite
        private void listView2_DragEnter(object sender, DragEventArgs e)
        {
            //Lorsqu'on glisse une visite sur le contrôle          
            e.Effect = DragDropEffects.Move;

            //On affecte les infos au bon médecin
            affecte(listView1.SelectedItems[0].Text, listView2.SelectedItems[0].Text);
        }


        public void affecte(string visite, string Medecin)
        {
            //On affecte la visite au médecin Sauf si c'est une pré-attribué
            //Maj des tables dans la base

            string Etat = FonctionsAppels.AttributionVisite(visite, Medecin);

            if (Etat == "AT")    //Il était libre donc on lui attribue
            {
                //Et si c'est ok, Maj de la ligne medecin..
                //ici on incrémente le compteur de visite (sans tout rafraichir)
                int cpt = int.Parse(listView2.SelectedItems[0].SubItems[1].Text) + 1;
                listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NombreDeVisite] = new ListViewItem.ListViewSubItem() { Text = cpt.ToString() };       //Compteur                                
                listView2.SelectedItems[0].SubItems[(int)ListView2Columns.HAttribut] = new ListViewItem.ListViewSubItem() { Text = DateTime.Now.ToString() };   //Date Attrib
                listView2.SelectedItems[0].SubItems[(int)ListView2Columns.Commune] = new ListViewItem.ListViewSubItem() { Text = listView1.SelectedItems[0].SubItems[5].Text };  //Commune
                listView2.SelectedItems[0].SubItems[(int)ListView2Columns.Adresse] = new ListViewItem.ListViewSubItem() { Text = listView1.SelectedItems[0].SubItems[4].Text };  //Adr
                listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NumAppel] = new ListViewItem.ListViewSubItem() { Text = listView1.SelectedItems[0].SubItems[0].Text };  //NumAppel

                //Coloration du nom du médecin (En jaune: Attribuée)               
                ColoriseSelectionListview2("AT");

                //Puis on l'efface de la liste des visites disponibles
                listView1.SelectedItems[0].Remove();

                ChargeAideRegulation();     //On rafraichi l'aide à la régulation                

                //On incrémente le compteur de rafraichissement (base)
                FonctionsAppels.IncrementeRafraichissement();
            }
            else if (Etat == "PR")
            {
                //On incrémente le compteur de visite
                int cpt = int.Parse(listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NombreDeVisite].Text) + 1;
                listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NombreDeVisite] = new ListViewItem.ListViewSubItem() { Text = cpt.ToString() };       //Compteur                  

                //ET on met le nom du médecin dans la ligne selectionnée, sans effacer de la ligne car c'est une pre attribuée
                listView1.SelectedItems[0].SubItems[12] = new ListViewItem.ListViewSubItem() { Text = listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NomMed].Text };

                //On incrémente le compteur de rafraichissement (base)
                FonctionsAppels.IncrementeRafraichissement();
            }

            //Sinon on ne fait rien                            

            //Si c'est un médecin de garde pour la Télémédecine, on ouvre la boite pour l'affectation d'une infirmière
            string TypeDeGarde = FonctionsAppels.RecupTypeGarde(Medecin);
            //Si c'est une garde de télémédecine
            if(TypeDeGarde.Substring(0,1) == "T")
            {
                //On regarde le status de la visite au niveau des organisations
                string Status = FonctionsAppels.StatusVisiteOrg(int.Parse(visite));

                //Si elle non attribuée (NA), on ouvre FListeInfirmière pour affecter éventuellement la visite
                if (Status !=  "KO" && Status == "NA")   //Pas déjà attribuée
                {
                    AfficheFormAttribInf(visite);
                }
            }
        }


        //Désafectation d'une visite 
        private void listView2_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //Si le médecin a une visite
            Int32 NumVisite = -1;
            bool Conversion = Int32.TryParse(listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NumAppel].Text, out NumVisite);

            //Si on a une visite, on peut la rendre
            if (NumVisite != -1)
            {
                if (Conversion)   //Si on a une visite, on peut la rendre
                {
                    //Remise de la visite dans le pool Sauf si le médecin l'a déjà acquité (dans ce cas il faut passer par le panneau)          
                    if (FonctionsAppels.StatusVisite(NumVisite) == "AT")
                    {
                        //On désaffecte les infos au bon médecin
                        Desaffecte(listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NumAppel].Text, listView2.SelectedItems[(int)ListView2Columns.CodeMedecin].Text);
                    }
                }
            }
        }


        public void Desaffecte(string visite, string Medecin)
        {
            //On désaffecte la visite du médecin 
            //Maj des tables dans la base
            if (FonctionsAppels.DesattribuerVisite(visite, Medecin, "AT") == "OK")
            {
                //On regarde enfin, s'il y a des visites pré-attribuées pour ce médecin....
                DataTable dtAppelsPre = FonctionsAppels.ListeVisitesPreAttribuee(Medecin);

                if (dtAppelsPre.Rows.Count > 0)
                {
                    //on affecte la 1er de la liste                        
                    if (FonctionsAppels.AttributionVisitePreAttr(dtAppelsPre.Rows[0]["Num_Appel"].ToString(), Medecin) == "KO")
                    {
                        System.Windows.Forms.MessageBox.Show("Echec de l'attribution de la visite pré-attrbuée. ", "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }

                ChargeAppelsNonAttribues();
                ChargeAppelsAttribues();    //On recharge la liste des médecins de garde avec les appels attribués s'il y en a
                ChargeAideRegulation();

                //On incrémente le compteur de rafraichissement (base)
                FonctionsAppels.IncrementeRafraichissement();
            }

        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {

        }


        //On selectionne la visite, 1 click pour l'afficher sur la carte
        private void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                try
                {
                    LocaliseVisiteCarte(listView1.SelectedItems[0]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            //On affiche les infos de la visite que l'on peut modifier si on en a une...
            if (listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NumAppel].Text != "-1")
            {
                //On stop le timer
                timer1.Stop();

                FVisite fVisite = new FVisite();
                fVisite.NumVisite = int.Parse(listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NumAppel].Text);
                fVisite.ShowDialog(this);
                fVisite.Dispose();

                //On le redémarre
                timer1.Start();
            }

            //On rafraichi la liste en attente dans le dispatch
            ChargeAppelsNonAttribues();
        }

        private void bvisite_Click(object sender, EventArgs e)
        {
            //On regarde combien il y a de Form Visite ouverte
            if (CompteurFormVisite < NBFormVisiteMax)
            {
                //On stop le timer
                timer1.Stop();

                //On incrémente +1 au compteur CompteurFormVisite
                CompteurFormVisite += 1;

                //et on l'affiche
                lnbFicheOuverte.Text = CompteurFormVisite.ToString();

                //Appel de la fiche visite en Ajout
                FVisite fVisite = new FVisite();
                fVisite.NumVisite = -1;
                fVisite.Show(this);
            }
        }


        //A la fermeture d'une fiche visite...
        public void QuitteFicheVisite(string reponse)
        {
            if (reponse == "OK")
            {
                //On décrémente -1 au compteur CompteurFormVisite
                CompteurFormVisite -= 1;

                //et on l'affiche
                lnbFicheOuverte.Text = CompteurFormVisite.ToString();

                //On rafraichi la liste en attente dans le dispatch
                ChargeAppelsNonAttribues();

                //On le redémarre
                timer1.Start();
            }
        }


        //A la fermeture d'une fiche visite...
        public void QuitteFicheRessaisie(string reponse)
        {
            if (reponse == "OK")
            {
                //On décrémente -1 au compteur CompteurFormVisite
                CompteurFormRessaisieVisite -= 1;

                //On le redémarre
                timer1.Start();
            }
        }

        #region RDV
        //on affiche la liste des rdv
        private void bRDV_Click(object sender, EventArgs e)
        {
            //On stop le timer
            timer1.Stop();

            FListeRDV fListeRDV = new FListeRDV();
            fListeRDV.ShowDialog();
            fListeRDV.Dispose();

            //Au retour on compte le nb de rdv... pour la couoleur du bouton
            string retour = FonctionsAppels.SupprimeRDVPerime();        //Suppression des rendez vous de la veille

            if (FonctionsAppels.NombreRDV() > 0)            //on retourne le nombre de rdv en attente pour la couleur du bouton
                bRDV.ImageIndex = 14;     //b rouge
            else
                bRDV.ImageIndex = 13;      //b vert


            //On le redémarre
            timer1.Start();

            //on raffraichi les liste
            RafraichiListes(sender, e);

        }
              
        #endregion

        #region les menus
        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //On quitte l'appli         
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //On se déconecte du CTI (si on s'en sert) et qu'on est toujours connecté
            if (ActivationCTI == true && FonctionsCTI.toujoursConnecte(Form1.Token, Form1.Ligne) == "OK")
                FonctionsCTI.deconnecte(Token, Ligne);

            //En fonction des droits...
            //on désactive le proxy si on est dev
            if (Utilisateur[1] != null)
            {
                switch (int.Parse(Utilisateur[1]))
                {
                    case 11: FonctionsCTI.DesactiveProxyHIN(); break;
                }
            }

            try
            {
                //On ferme toute les formes
                FormCollection fc = Application.OpenForms;

                //on parcours la liste des forms ouvertes et on les fermes Sauf la principale: REGULATION
                for (int i = 0; i < fc.Count; i++)
                {
                    if (fc[i].Name.ToString() != "REGULATION")
                        fc[i].Dispose();
                }

                //Puis on détruit la form principale: REGULATION
                this.Dispose();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void MenuGardeMedecins_Click(object sender, EventArgs e)
        {
            //On affiche la forme de mise en garde d'un médecin        
            //on stop le timer
            timer1.Stop();

            FGardeMedecin fGardeMedecin = new FGardeMedecin();

            //Si on a selectionné un médecin
            if (listView2.SelectedItems.Count > 0)
            {
                FGardeMedecin.CodeMedecin = int.Parse(listView2.SelectedItems[0].SubItems[(int)ListView2Columns.CodeMedecin].Text);
                FGardeMedecin.Etat = "Modif";
            }
            else
                FGardeMedecin.CodeMedecin = -1;

            fGardeMedecin.ShowDialog(this);
            fGardeMedecin.Dispose();

            //On rafraichi la liste des médecins dans le dispatch                        
            ChargeAppelsNonAttribues();
            ChargeAppelsAttribues();
            ChargeAideRegulation();

            //On le redémarre
            timer1.Start();
        }


        private void MenuCTI_Click(object sender, EventArgs e)
        {
            //On active ou désactive le CTI
            //on stop le timer
            timer1.Stop();

            FCTI fCTI = new FCTI();
            fCTI.ShowDialog(this);
            fCTI.Dispose();

            //On le redémarre
            timer1.Start();
        }

        private void MenuParametresDivers_Click(object sender, EventArgs e)
        {
            //Parametrages divers

            //on stop le timer
            timer1.Stop();

            FParametresDivers fParametresDivers = new FParametresDivers();
            fParametresDivers.ShowDialog(this);
            fParametresDivers.Dispose();

            //On le redémarre
            timer1.Start();
        }

        private void MenuParamUtil_Click(object sender, EventArgs e)
        {
            //Préférences de l'utilisateur

            //on stop le timer
            timer1.Stop();

            FParamUtilisateur fParamUtilisateur = new FParamUtilisateur();

            fParamUtilisateur.Text = "Paramètres de " + Utilisateur[3];
            fParamUtilisateur.ShowDialog(this);
            fParamUtilisateur.Dispose();

            //On le redémarre
            timer1.Start();
        }


        private void Menuressaisie_Click(object sender, EventArgs e)
        {
            //On regarde combien il y a de Form Re-saisie Visite ouverte
            if (CompteurFormRessaisieVisite < 1)
            {
                //On stop le timer
                timer1.Stop();

                //On incrémente +1 au compteur CompteurFormVisite
                CompteurFormRessaisieVisite += 1;


                //On previent
                DialogResult dialogResult = MessageBox.Show("Attention vous allez saisir avec la re-saisie. est-ce bien ce que vous voulez faire?", "Re-saisie", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    //Appel de la fiche visite en Ajout
                    FRessaisie fRessaisie = new FRessaisie();
                    fRessaisie.NumVisite = -1;
                    fRessaisie.Show(this);
                }
            }
        }

        private void MenudebloquerMed_Click(object sender, EventArgs e)
        {
            //On débloque un médecin "Bloqué"
            //on stop le timer
            timer1.Stop();

            FDebloqueMed fDebloqueMed = new FDebloqueMed();
            fDebloqueMed.ShowDialog(this);
            fDebloqueMed.Dispose();

            //On le redémarre
            timer1.Start();
        }

        //Réinitialisation des alarmes silencieuses
        private void MenureinitialiserAlarmes_Click(object sender, EventArgs e)
        {
            //On stop le timer
            timer1.Stop();

            FAlarmeS fAlarmeS = new FAlarmeS();
            fAlarmeS.ShowDialog(this);
            fAlarmeS.Dispose();

            //On le redémarre
            timer1.Start();
        }


        //Gestion des documents dispos
        private void menuGestDoc_Click(object sender, EventArgs e)
        {
            //On affiche la forme de recherche des évènements
            //on stop le timer
            timer1.Stop();

            FGestionDocuments fGestionDocuments = new FGestionDocuments();
            fGestionDocuments.ShowDialog(this);
            fGestionDocuments.Dispose();

            //On le redémarre
            timer1.Start();
        }

        private void MenuOrganisation_Click(object sender, EventArgs e)
        {
            //On affiche la forme de gestion des organisations
            //on stop le timer
            timer1.Stop();

            FOrganisation fOrganisation = new FOrganisation();
            fOrganisation.ShowDialog(this);
            fOrganisation.Dispose();

            //On le redémarre
            timer1.Start();
        }

        private void MenuProvenances_Click(object sender, EventArgs e)
        {
            //On affiche la forme de gestion des provenances
            //on stop le timer
            timer1.Stop();

            FProvenance fProvenance = new FProvenance();
            fProvenance.ShowDialog(this);
            fProvenance.Dispose();

            //On le redémarre
            timer1.Start();
        }

        private void MenuAssurance_Click(object sender, EventArgs e)
        {
            //On affiche la forme de gestion des assurances
            //on stop le timer
            timer1.Stop();

            FAssurance fAssurance = new FAssurance();
            fAssurance.ShowDialog(this);
            fAssurance.Dispose();

            //On le redémarre
            timer1.Start();
        }

        private void MenuRues_Click(object sender, EventArgs e)
        {
            //On affiche la forme de gestion des rues
            //on stop le timer
            timer1.Stop();

            FRue fRue = new FRue();
            fRue.ShowDialog(this);
            fRue.Dispose();

            //On le redémarre
            timer1.Start();
        }

        private void MenuCommune_Click(object sender, EventArgs e)
        {
            //On affiche la forme de gestion des communes
            //on stop le timer
            timer1.Stop();

            FCommune fCommune = new FCommune();
            fCommune.ShowDialog(this);
            fCommune.Dispose();

            //On le redémarre
            timer1.Start();
        }

        private void MenuGarde_Click(object sender, EventArgs e)
        {
            //On affiche la forme de gestion des gardes
            //on stop le timer
            timer1.Stop();

            FGarde fgarde = new FGarde();
            fgarde.ShowDialog(this);
            fgarde.Dispose();

            //On le redémarre
            timer1.Start();
        }

        private void MenuSmartphone_Click(object sender, EventArgs e)
        {
            //On affiche la forme de gestion des smartphones    
            //on stop le timer
            timer1.Stop();

            FSmartphone fSmartphone = new FSmartphone();
            fSmartphone.ShowDialog(this);
            fSmartphone.Dispose();

            //On le redémarre
            timer1.Start();
        }

        private void MenuMedecin_Click(object sender, EventArgs e)
        {
            //On affiche la forme de gestion des médecins
            //on stop le timer
            timer1.Stop();

            FMedecin fMedecin = new FMedecin();
            fMedecin.ShowDialog(this);
            fMedecin.Dispose();

            //On le redémarre
            timer1.Start();
        }

        private void MenuUtilisateur_Click(object sender, EventArgs e)
        {
            //On affiche la forme de gestion des provenances
            //on stop le timer
            timer1.Stop();

            FUtilisateur fUtilisateur = new FUtilisateur();
            fUtilisateur.ShowDialog(this);
            fUtilisateur.Dispose();

            //On le redémarre
            timer1.Start();
        }

        private void MenutelUtil_Click(object sender, EventArgs e)
        {
            //Liste des téléphones utils
            //on stop le timer
            timer1.Stop();

            FTelUtils fTelUtils = new FTelUtils();
            fTelUtils.ShowDialog(this);
            fTelUtils.Dispose();

            //On le redémarre
            timer1.Start();
        }

        private void MenuMotif_Click(object sender, EventArgs e)
        {
            //On affiche la forme de gestion des motifs
            //on stop le timer
            timer1.Stop();

            FMotif fMotif = new FMotif();
            fMotif.ShowDialog(this);
            fMotif.Dispose();

            //On le redémarre
            timer1.Start();
        }

        //Recherche d'évènements
        private void MenuRechEvent_Click(object sender, EventArgs e)
        {
            //On affiche la forme de recherche des évènements
            //on stop le timer
            timer1.Stop();

            FRechEvenements fRechEvenements = new FRechEvenements();
            fRechEvenements.ShowDialog(this);
            fRechEvenements.Dispose();

            //On le redémarre
            timer1.Start();
        }

        //Recherche de l'historique d'attribution des Smartphones
        private void MenuHistoAttribSmart_Click(object sender, EventArgs e)
        {
            //On affiche la forme de recherche de l'historique d'attribution des Smartphones
            //on stop le timer
            timer1.Stop();

            FHistoPda fHistoPda = new FHistoPda();
            fHistoPda.ShowDialog(this);
            fHistoPda.Dispose();

            //On le redémarre
            timer1.Start();
        }


        //Affichage de la boite A propos
        private void MenuAPropos_Click(object sender, EventArgs e)
        {
            //On stop le timer
            timer1.Stop();

            FApropos fApropos = new FApropos();
            fApropos.ShowDialog(this);
            fApropos.Dispose();

            //On le redémarre
            timer1.Start();
        }

        #endregion


        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            //On affiche la visite non attribuée selectionnée            
            //on stop le timer
            timer1.Stop();

            FVisite fVisite = new FVisite();
            fVisite.NumVisite = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            fVisite.ShowDialog(this);
            fVisite.Dispose();

            //On rafraichi la liste en attente dans le dispatch
            ChargeAppelsNonAttribues();

            //On le redémarre
            timer1.Start();
        }


        private void bGardes_Click(object sender, EventArgs e)
        {
            MenuGardeMedecins_Click(sender, e);
        }


        private void bVisiteMed_Click(object sender, EventArgs e)
        {
            //Si on a sélectionné une ligne...
            if (listView2.SelectedItems.Count > 0)
            {
                //On regarde s'il a des visites attribuées                                   
                //On affiche une boite avec la liste des visites pour un médecin
                //on stop le timer
                timer1.Stop();

                FVisiteMed fVisiteMed = new FVisiteMed();
                fVisiteMed.CodeMedecin = listView2.SelectedItems[0].SubItems[(int)ListView2Columns.CodeMedecin].Text;
                fVisiteMed.ShowDialog(this);
                fVisiteMed.Dispose();

                //On rafraichi les listes
                //ChargeAppelsNonAttribues();   //Liste des appels en attente
                //ChargeAppelsAttribues();      //Liste des médecins en garde avec leurs appels (s'il y en a)
                //ChargeAideRegulation();

                RafraichiListes(sender, e);

                //On le redémarre
                timer1.Start();
            }
        }

        //Recherche d'un appel
        private void bRechercheAppel_Click(object sender, EventArgs e)
        {
            //on stop le timer
            timer1.Stop();

            FRechercheAppels fRechercheAppels = new FRechercheAppels();
            fRechercheAppels.ShowDialog(this);
            fRechercheAppels.Dispose();

            //on redemarre le timer
            timer1.Start();
        }


        private void rechercheVisitesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bRechercheAppel_Click(sender, e);
        }

        //affiche les visites terminées ou attribuée d'un médecin pour la journée en cours (FRechercheAppels)
        private void bVisiteJournaliere_Click(object sender, EventArgs e)
        {
            //Si on a sélectionné une ligne...
            if (listView2.SelectedItems.Count > 0)
            {
                //on stop le timer
                timer1.Stop();

                FRechercheAppels fRechercheAppels = new FRechercheAppels();
                fRechercheAppels.Text = "Visite depuis le début de la garde du médecin " + listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NomMed].Text;
                fRechercheAppels.codeMedecin = listView2.SelectedItems[0].SubItems[(int)ListView2Columns.CodeMedecin].Text;
                fRechercheAppels.ShowDialog(this);
                fRechercheAppels.Dispose();

                //on redemarre le timer
                timer1.Start();
            }
        }


        //On envoi un message au médecin sélectionné ou à tout le monde si pas de médecin sélectionnés
        private void bMessage_Click(object sender, EventArgs e)
        {
            //On affiche la forme d'envoi des messages       
            //on stop le timer
            timer1.Stop();

            FMessage fMessage = new FMessage();

            if (listView2.SelectedItems.Count > 0)
            {
                fMessage.CodeMedecin = listView2.SelectedItems[0].Text;
                fMessage.Destinataire = listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NomMed].Text;
            }
            else
            {
                fMessage.CodeMedecin = "-1";
                fMessage.Destinataire = "Tous les médecins";
            }

            fMessage.ShowDialog(this);
            fMessage.Dispose();

            //On rafraichi la liste des médecins dans le dispatch                        
            RafraichiListes(sender, e);
            //ChargeAppelsNonAttribues();
            //ChargeAppelsAttribues();
            //ChargeMessages();
            //ChargeAideRegulation();

            //On le redémarre
            timer1.Start();
        }



        //*************************** COLORATION **********************************************************
        #region Coloration
        //Colorise le listview1 (Visites en attentes en fonction de la provenance)
        private ListViewItem ColoriseListview1(string typeColoration, ListViewItem item, int numSubItem)
        {
            item.UseItemStyleForSubItems = false;

            //Coloration du nom du patient (Fond et ecriture)                                               
            if (typeColoration == "144")
            {
                item.SubItems[numSubItem].BackColor = Color.Salmon;
                item.SubItems[numSubItem].ForeColor = Color.Black;
            }
            else if (typeColoration == "Police")
            {
                item.SubItems[numSubItem].BackColor = Color.PaleGreen;
                item.SubItems[numSubItem].ForeColor = Color.Black;
            }
            else if (typeColoration == "TA")
            {
                item.SubItems[numSubItem].BackColor = Color.Cyan;
                item.SubItems[numSubItem].ForeColor = Color.Black;
            }
            else if (typeColoration == "Hotels")
            {
                item.SubItems[numSubItem].BackColor = Color.Pink;
                item.SubItems[numSubItem].ForeColor = Color.Black;
            }
            else if (typeColoration == "Prive-Particulier")
            {
                // item.SubItems[numSubItem].BackColor = SystemColors.ControlDarkDark;
                item.SubItems[numSubItem].ForeColor = Color.Black;
            }

            return item;
        }


        //Colorise le listview1 (Visites en attentes en fonction de l'urgence), le texte seulement SAUF Le nom Prenom
        private ListViewItem ColoriseListview1Complete(string typeColoration, ListViewItem item)
        {
            item.UseItemStyleForSubItems = false;

            if (typeColoration == "Urgence1")
            {
                for (int i = 0; i < 12; i++)
                {
                    if (i != 6)
                        item.SubItems[i].ForeColor = Color.Orange;
                }
            }
            else if (typeColoration == "Police")
            {
                for (int i = 0; i < 12; i++)
                {
                    if (i != 6)
                        item.SubItems[i].ForeColor = Color.PaleGreen;
                }
            }
            else if (typeColoration == "Hotels")
            {
                for (int i = 0; i < 12; i++)
                {
                    if (i != 6)
                        item.SubItems[i].ForeColor = Color.Pink;
                }
            }
            else if (typeColoration == "TA")
            {
                for (int i = 0; i < 12; i++)
                {
                    if (i != 6)
                        item.SubItems[i].ForeColor = Color.Cyan;
                }
            }
            else if (typeColoration == "144")
            {
                for (int i = 0; i < 12; i++)
                {
                    if (i != 6)
                        item.SubItems[i].ForeColor = Color.Salmon;
                }
            }

            return item;
        }

        //Souligne une ligne complète (Notament lorsque le patient à rappelé...Il s'impatiente)
        private ListViewItem SouligneListview1Complete(ListViewItem item)
        {
            item.UseItemStyleForSubItems = false;

            for (int i = 0; i < 12; i++)
            {
                if (i != 6)
                    item.SubItems[i].Font = new Font(item.Font, FontStyle.Underline);
            }

            return item;
        }


        //Colorise le listview1 (visites en attentes) lorsqu'elle est sélectionnée et que l'on fait des opérations la concernant
        private void ColoriseSelectionListview1(string typeColoration)
        {
            //Pour activer la coloration différente de celle par défaut
            /*  listView1.SelectedItems[0].UseItemStyleForSubItems = false;

              if (typeColoration == "AT")
              {
                  //Coloration du nom du médecin (Fond en jaune: Attribuée)                                               
                  for (int i = 0; i < 9; i++)
                  {
                      if (i != 2)
                      {
                          listView2.SelectedItems[0].SubItems[i].BackColor = SystemColors.ControlDarkDark;
                          listView2.SelectedItems[0].SubItems[i].ForeColor = SystemColors.Window;
                      }
                      else
                      {
                          listView2.SelectedItems[0].SubItems[i].BackColor = Color.Yellow;
                          listView2.SelectedItems[0].SubItems[i].ForeColor = Color.Black;
                      }
                  }
              }*/
        }


        //Colorise le listview2 (médecin en garde avec ou sans visites) lors d'un rafraichissement par exemple
        private ListViewItem ColoriseListview2(string typeColoration, ListViewItem item, int numSubItem)
        {
            item.UseItemStyleForSubItems = false;

            //Coloration du nom du médecin (Fond et ecriture)                                               
            if (typeColoration == "Disponible")
            {
                item.SubItems[numSubItem].BackColor = SystemColors.ControlDarkDark;
                item.SubItems[numSubItem].ForeColor = SystemColors.Window;
            }
            else if (typeColoration == "Attribuée")
            {
                item.SubItems[numSubItem].BackColor = Color.Yellow;
                item.SubItems[numSubItem].ForeColor = Color.Black;
            }
            else if (typeColoration == "Acquitée")
            {
                item.SubItems[numSubItem].BackColor = Color.Orange;
                item.SubItems[numSubItem].ForeColor = Color.Black;
            }
            else if (typeColoration == "Visite")
            {
                item.SubItems[numSubItem].BackColor = Color.Green;
                item.SubItems[numSubItem].ForeColor = Color.Black;
            }
            else if (typeColoration == "En pause")
            {
                item.SubItems[numSubItem].BackColor = Color.Sienna;
                item.SubItems[numSubItem].ForeColor = Color.White;
            }

            return item;
        }


        //Colorise le listview2 (médecin en garde avec ou sans visites) lorsqu'il est sélectionné et que l'on fait des opérations le concernant
        private void ColoriseSelectionListview2(string typeColoration)
        {
            //Pour activer la coloration différente de celle par défaut
            listView2.SelectedItems[0].UseItemStyleForSubItems = false;

            if (typeColoration == "AT")
            {
                //Coloration du nom du médecin (Fond en jaune: Attribuée)                                               
                for (int i = 0; i < 9; i++)
                {
                    if (i != (int)ListView2Columns.NomMed || i != (int)ListView2Columns.TitreEmployee)
                    {
                        listView2.SelectedItems[0].SubItems[i].BackColor = SystemColors.ControlDarkDark;
                        listView2.SelectedItems[0].SubItems[i].ForeColor = SystemColors.Window;
                    }
                    else
                    {
                        listView2.SelectedItems[0].SubItems[i].BackColor = Color.Yellow;
                        listView2.SelectedItems[0].SubItems[i].ForeColor = Color.Black;
                    }
                }
            }
        }

        #endregion

        //****************************FIN COLORATION********************************************

        #region timer
        //Geston de l'évènement du timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("On regarde s'il faut rafraichir l'écran");

            if (FonctionsAppels.Rafraichir(Compteur) == "Oui")
            {
                //On vérifie que la carto est ouverte
                //On rafraichi d'abord la carte (car c'est plus long)
                var fCarto = Application.OpenForms["FCarto"];

                if (fCarto != null)
                {
                    FCarto.rafraichiCarte = "OK";
                }

                //On memorise les selections précédentes pour les réatribuer par la suite
                int indexL1 = -1;
                int indexL2 = -1;

                if (listView1.SelectedItems.Count > 0)
                    indexL1 = listView1.Items.IndexOf(listView1.SelectedItems[0]);

                if (listView2.SelectedItems.Count > 0)
                    indexL2 = listView2.Items.IndexOf(listView2.SelectedItems[0]);

                //Puis on rafraichi les listes et évènements
                ChargeAppelsNonAttribues();   //Liste des appels en attente
                ChargeAppelsAttribues();      //Liste des médecins en garde avec leurs appels (s'il y en a)
                ChargeMessages();             //Les messages
                ChargeAideRegulation();       //L'aide à la régulation  

                //Enfin on met à jour les boutons
                //On desactive certains boutons
                bEtatMed.ImageIndex = 2;
                bEtatMed.Enabled = false;
                bVisiteJournaliere.Enabled = false;
                bVisiteMed.Enabled = false;
                bFindegarde.Enabled = false;

                try
                {
                    //Puis on reafecte les selections
                    if (indexL1 != -1)
                    {
                        //On verifie que la liste n'a pas diminuée et que l'index est toujours valable 
                        if (indexL1 + 1 <= listView1.Items.Count)
                        {
                            listView1.Items[indexL1].Selected = true;
                            listView1.Select();
                        }
                    }

                    if (indexL2 != -1)
                    {
                        //On verifie que la liste n'a pas diminuée et que l'index est toujours valable 
                        if (indexL2 + 1 <= listView2.Items.Count)
                        {
                            listView2.Items[indexL2].Selected = true;
                            listView2.Select();

                            //Puis on gère l'état des boutons
                            listView2_Click(sender, e);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur rafraichissement" + ex.Message);
                }

                Console.WriteLine("Fin rafraichissement");
            }
        }

        #endregion


        //Gère l'état des boutons en fct de l'état de la visite du médecin (En pause, en visite, Dispo)
        private void listView2_Click(object sender, EventArgs e)
        {            
            //On récupère l'Etat de dispo du médecin            
            string Etat = FonctionsAppels.RechEtatDispoMedecin(listView2.SelectedItems[0].Text);
            switch (Etat)
            {
                case "Disponible":
                    bEtatMed.ImageIndex = 2;    //Il peut se mettre en pause
                    bEtatMed.Enabled = true;
                    bVisiteJournaliere.Enabled = true;
                    bVisiteMed.Enabled = true;
                    bFindegarde.Enabled = true;
                    LocaliseMedecinCarte(listView2.SelectedItems[0].Text);     //On localise le médecin sur la carte
                                                                               // PositionneEnPause(listView2.SelectedItems[0].SubItems[2].Text);     //Il attends, on le met à la centrale...Pour le moment
                    toolTip1.SetToolTip(bEtatMed, "Etat du médecin (Disponible)");      //la légende
                    break;

                case "Attribuée":
                case "Acquitée":
                    bEtatMed.ImageIndex = 2;    //Il ne peut plus se mettre en pause
                    bEtatMed.Enabled = false;
                    bVisiteJournaliere.Enabled = true;
                    bVisiteMed.Enabled = true;
                    bFindegarde.Enabled = false;
                    LocaliseMedecinCarte(listView2.SelectedItems[0].Text);     //On localise le médecin sur la carte
                    toolTip1.SetToolTip(bEtatMed, "Etat du médecin (V. Attribuée/acquitée)");   //la légende
                    break;

                case "Visite":
                    bEtatMed.ImageIndex = 4;    //Il ne peut plus se mettre en pause et il est en visite
                    bEtatMed.Enabled = true;
                    bVisiteJournaliere.Enabled = true;
                    bVisiteMed.Enabled = true;
                    bFindegarde.Enabled = false;
                    LocaliseMedecinCarte(listView2.SelectedItems[0].Text);     //On localise le médecin sur la carte
                    toolTip1.SetToolTip(bEtatMed, "Etat du médecin (En visite)");   //la légende
                    break;

                case "En pause":
                    bEtatMed.ImageIndex = 3;    //On peut le Dé-pauser
                    bEtatMed.Enabled = true;
                    bVisiteJournaliere.Enabled = true;
                    bVisiteMed.Enabled = true;
                    bFindegarde.Enabled = true;
                    LocaliseMedecinCarte(listView2.SelectedItems[0].Text);     //On localise le médecin sur la carte
                                                                               //PositionneEnPause(listView2.SelectedItems[0].SubItems[2].Text);     //On met le médecin à la centrale (recup nom du medecin)
                    toolTip1.SetToolTip(bEtatMed, "Etat du médecin (En pause)");   //la légende
                    break;
                default: break;
            }
            
        }


        #region gestion des Messages
        //click droit sur le medecin pour lui envoyer un message
        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {                
                //Si c'est un médecin de garde pour la Télémédecine, on ouvre la boite pour l'affectation d'une infirmière
                 string TypeDeGarde = FonctionsAppels.RecupTypeGarde(listView2.SelectedItems[0].Text);
               
                if (TypeDeGarde.Substring(0, 1) == "T")
                {                                                                                                   
                    //On regarde le status de la visite au niveau des organisations
                    string Status = FonctionsAppels.StatusVisiteOrg(int.Parse(listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NumAppel].Text));

                    //Si elle non attribuée (NA), on ouvre FListeInfirmière pour affecter éventuellement la visite
                    if (Status != "KO" && Status == "NA")   //Pas déjà attribuée
                    {
                        //On affiche un menu pour le choix Attribution Org ou Boite de messagerie                       
                        AfficheCustomMenu(sender, e, "Affectation organisation", "Messagerie");                        
                    }
                    else
                    {
                        //Elle est déjà attribuée (ou pré.) donc on ouvre juste la forme d'envoi des messages                       
                        //On affiche la forme d'envoi des messages       
                        //on stop le timer
                        timer1.Stop();

                        FMessage fMessage = new FMessage();
                        fMessage.CodeMedecin = listView2.SelectedItems[0].Text;
                        fMessage.Destinataire = listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NomMed].Text;

                        fMessage.ShowDialog(this);
                        fMessage.Dispose();

                        //On rafraichi la liste des médecins dans le dispatch                        
                        RafraichiListes(sender, e);
                        
                        //On le redémarre
                        timer1.Start();
                    }
                }
                else
                {                                      
                    //On affiche la forme d'envoi des messages       
                    //on stop le timer
                    timer1.Stop();

                    FMessage fMessage = new FMessage();
                    fMessage.CodeMedecin = listView2.SelectedItems[0].Text;
                    fMessage.Destinataire = listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NomMed].Text;

                    fMessage.ShowDialog(this);
                    fMessage.Dispose();

                    //On rafraichi la liste des médecins dans le dispatch                        
                    RafraichiListes(sender, e);
                  
                    //On le redémarre
                    timer1.Start();
                }               
            }
        }

        //click droit sur le listview3 pour envoyer un message à tous les médecins
        private void listView3_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //On affiche la forme d'envoi des messages       
                //on stop le timer
                timer1.Stop();

                FMessage fMessage = new FMessage();
                fMessage.CodeMedecin = "-1";
                fMessage.Destinataire = "Tous les médecins";

                fMessage.ShowDialog(this);
                fMessage.Dispose();

                //On rafraichi la liste des médecins dans le dispatch                        
                RafraichiListes(sender, e);

                //On le redémarre
                timer1.Start();
            }
        }


        //Alterne la couleur du listview
        private void listView3_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
            if ((e.ItemIndex % 2) == 1)
            {
                e.Item.BackColor = Color.DarkGray;
                e.Item.UseItemStyleForSubItems = true;
            }
        }


        //Affiche l'entête de colonne
        private void listView3_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView3_MouseHover(object sender, EventArgs e)
        {
            //Affiche le message
            //  listView3.SelectedItems[0].SubItems[2].Text;
        }

        //Recherche de messages
        private void rechercheDeMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //On affiche la forme de recherche de messages
            //on stop le timer
            timer1.Stop();

            FRechMessages fRechMessages = new FRechMessages();
            fRechMessages.ShowDialog(this);
            fRechMessages.Dispose();

            //On rafraichi la liste des médecins dans le dispatch                        
            RafraichiListes(sender, e);
            //ChargeAppelsNonAttribues();
            //ChargeAppelsAttribues();
            //ChargeMessages();
            //ChargeAideRegulation();

            //On le redémarre
            timer1.Start();
        }

        //Envoi d'un message
        private void MenuNvxMessage_Click(object sender, EventArgs e)
        {
            bMessage_Click(sender, e);
        }

        #endregion

        //Mise en pause/dépause du médecin
        private void bEtatMed_Click(object sender, EventArgs e)
        {
            //if (listView2.SelectedItems[0].Index != -1)
            if (listView2.SelectedItems.Count > 0)
            {
                if (bEtatMed.ImageIndex != 4)       //Si le médecin n'est pas en visite
                {
                    if (bEtatMed.ImageIndex == 2)      //Tasse café
                    {
                        //Mise en pause du médecin sélectionné
                        FonctionsAppels.MiseEnPause(listView2.SelectedItems[0].Text);

                        //Puis maj du bouton
                        bEtatMed.ImageIndex = 3;    //On peut le Dé-pauser
                        bEtatMed.Enabled = true;
                        toolTip1.SetToolTip(bEtatMed, "Etat du médecin (En pause)");   //la légende

                        ChargeAideRegulation();     //On rafraichi l'aide à la régulation     
                    }
                    else if (bEtatMed.ImageIndex == 3)      //Tasse café barrée rouge 
                    {
                        //On dé-pause du médecin sélectionné
                        FonctionsAppels.SortiePause(listView2.SelectedItems[0].Text);

                        //Puis maj du boutons
                        bEtatMed.ImageIndex = 2;    //On peut le Dé-pauser
                        bEtatMed.Enabled = true;
                        toolTip1.SetToolTip(bEtatMed, "Etat du médecin (Disponible)");   //la légende

                        ChargeAideRegulation();     //On rafraichi l'aide à la régulation     
                    }
                }
                else   //Il est en visite
                {
                    toolTip1.SetToolTip(bEtatMed, "Etat du médecin (En visite)");   //la légende
                }
            }
        }

        //Sortie de garde d'un médecin
        private void bFindegarde_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                //On stop le rafraichissement               
                timer1.Stop();

                DialogResult result = MessageBox.Show("Voulez vous sortir " + listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NomMed].Text + " de garde ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    //On termine la garde du médecin sélectionné
                    FonctionsAppels.TermineGarde(listView2.SelectedItems[0].Text);

                    //On rafraichi la liste des médecins dans le dispatch                        
                    RafraichiListes(sender, e);
                    //ChargeAppelsNonAttribues();
                    //ChargeAppelsAttribues();
                    //ChargeAideRegulation();                    
                }

                //on le redémare
                timer1.Start();
            }
        }

        //On rafraichi tout
        private void bRafraichi_Click(object sender, EventArgs e)
        {
            var fCarto = Application.OpenForms["FCarto"];

            if (fCarto != null)
            {
                FCarto.rafraichiCarte = "OK";
            }

            //On memorise les selections précédentes pour les réatribuer par la suite
            int indexL1 = -1;
            int indexL2 = -1;

            if (listView1.SelectedItems.Count > 0)
                indexL1 = listView1.Items.IndexOf(listView1.SelectedItems[0]);

            if (listView2.SelectedItems.Count > 0)
                indexL2 = listView2.Items.IndexOf(listView2.SelectedItems[0]);


            //Puis on rafraichi les listes et évènements
            ChargeAppelsNonAttribues();   //Liste des appels en attente
            ChargeAppelsAttribues();      //Liste des médecins en garde avec leurs appels (s'il y en a)
            ChargeMessages();             //Les messages
            ChargeAideRegulation();       //L'aide à la régulation  


            //Enfin on met à jour les boutons
            //On desactive certains boutons
            bEtatMed.ImageIndex = 2;
            bEtatMed.Enabled = false;
            bVisiteJournaliere.Enabled = false;
            bVisiteMed.Enabled = false;
            bFindegarde.Enabled = false;

            try
            {
                //Puis on reafecte les selections
                if (indexL1 != -1)
                {
                    //On verifie que la liste n'a pas diminuée et que l'index est toujours valable 
                    if (indexL1 + 1 <= listView1.Items.Count)
                    {
                        listView1.Items[indexL1].Selected = true;
                        listView1.Select();
                    }
                }

                if (indexL2 != -1)
                {
                    //On verifie que la liste n'a pas diminuée et que l'index est toujours valable 
                    if (indexL2 + 1 <= listView2.Items.Count)
                    {
                        listView2.Items[indexL2].Selected = true;
                        listView2.Select();

                        //Puis on gère l'état des boutons
                        listView2_Click(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur rafraichissement" + ex.Message);
            }

            Console.WriteLine("Fin rafraichissement");
        }


        private void RafraichiListes(object sender, EventArgs e)
        {
            var fCarto = Application.OpenForms["FCarto"];

            if (fCarto != null)
            {
                FCarto.rafraichiCarte = "OK";
            }

            //On memorise les selections précédentes pour les réatribuer par la suite
            int indexL1 = -1;
            int indexL2 = -1;

            if (listView1.SelectedItems.Count > 0)
                indexL1 = listView1.Items.IndexOf(listView1.SelectedItems[0]);

            if (listView2.SelectedItems.Count > 0)
                indexL2 = listView2.Items.IndexOf(listView2.SelectedItems[0]);


            //Puis on rafraichi les listes et évènements
            ChargeAppelsNonAttribues();   //Liste des appels en attente
            ChargeAppelsAttribues();      //Liste des médecins en garde avec leurs appels (s'il y en a)
            ChargeMessages();             //Les messages
            ChargeAideRegulation();       //L'aide à la régulation  


            //Enfin on met à jour les boutons
            //On desactive certains boutons
            bEtatMed.ImageIndex = 2;
            bEtatMed.Enabled = false;
            bVisiteJournaliere.Enabled = false;
            bVisiteMed.Enabled = false;
            bFindegarde.Enabled = false;
           

            try
            {
                //Puis on reafecte les selections
                if (indexL1 != -1)
                {
                    //On verifie que la liste n'a pas diminuée et que l'index est toujours valable 
                    if (indexL1 + 1 <= listView1.Items.Count)
                    {
                        listView1.Items[indexL1].Selected = true;
                        listView1.Select();
                    }
                }

                if (indexL2 != -1)
                {
                    //On verifie que la liste n'a pas diminuée et que l'index est toujours valable 
                    if (indexL2 + 1 <= listView2.Items.Count)
                    {
                        listView2.Items[indexL2].Selected = true;
                        listView2.Select();

                        //Puis on gère l'état des boutons
                        listView2_Click(sender, e);
                    }
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur rafraichissement" + ex.Message);
            }

            Console.WriteLine("Fin rafraichissement");
        }


        #region Carte
        //***************************CONCERNE LA CARTE**********************************                                              
        //Affichage de la carto
        private void bCarto_Click(object sender, EventArgs e)
        {
            var fCarto = Application.OpenForms["FCarto"];

            if (fCarto == null)
            {
                //Elle n'est pas ouverte, on la créer et on l'ouvre
                fCarto = new FCarto();
                fCarto.Show();
            }
            else   //Sinon on lui donne le focus
                fCarto.Activate();
        }

        //Est que la carte est ouverte?
        /*  private string CarteOuverte()
          {
              //On vérifie que la carto est ouverte
              string Retour = "KO";

              var fCarto = Application.OpenForms["FCarto"];

              if (fCarto != null)
              {
                  Console.WriteLine("Carto ouverte");
                  Retour = "OK";
              }

              return Retour;
          }*/


        //On localise le médecin sur la carte (Qd il est en visite) et on trace ses visites pré attribués
        private void LocaliseMedecinCarte(string CodeMedecin)
        {
            //On vérifie que la carto est ouverte

            var fCarto = Application.OpenForms["FCarto"];

            if (fCarto != null)
            {
                FCarto.FocusMedecinGPS(CodeMedecin);
                
                //var FormCarto = new FCarto();                
                //FormCarto.FocusMedecinGPS(CodeMedecin);  

                //recup des infos de sa visite en cours (AT, AQ, V)
                //  DataTable dtvisite = FonctionsAppels.RechAppelsEnCoursMed(CodeMedecin, "ATAQV");
                /*  if (dtvisite.Rows.Count > 0)
                  {                  
                      string NomMed = dtvisite.Rows[0]["Medecin"].ToString();

                      DataRow rowVEncours = dtvisite.Rows[0];

                      //Puis récup de ses visites pré atribués
                      DataTable dtvisitePr = FonctionsAppels.RechAppelsEnCoursMed(CodeMedecin, "PR");

                      //On affiche sur la carte....Si elle est active
                      FCarto.FocusMedecin(rowVEncours, dtvisitePr, NomMed);                                                                
                  }*/
            }
        }


        //On localise la visite sur la carte en affichant le popup
        private void LocaliseVisiteCarte(ListViewItem item)
        {
            Double Lat = 0;
            Double Lng = 0;

            //On vérifie que la carto est ouverte
            var fCarto = Application.OpenForms["FCarto"];

            if (fCarto != null)
            {
                //On recupère les coordonnées de la visite
                DataTable ptGeo = FonctionsAppels.RecupCoordonnees(item.SubItems[0].Text.ToString());
                if (ptGeo.Rows[0] != null)
                {
                    Lat = double.Parse(ptGeo.Rows[0]["Latitude"].ToString());
                    Lng = double.Parse(ptGeo.Rows[0]["Longitude"].ToString());
                }

                //On affiche sur la carte....Si elle est active
                //var FormCarto = new FCarto();
                //FormCarto.FocusVisite(item, Lat, Lng);

                FCarto.FocusVisite(item, Lat, Lng);
            }
        }


        //On efface le popup de localisation de la visite sur la carte
        private void EffacePopupVisiteCarte()
        {
            //On vérifie que la carto est ouverte
            var fCarto = Application.OpenForms["FCarto"];

            if (fCarto != null)
            {
                //On efface le popup sur la carte....Si elle est active
                FCarto.EffacePopupVisite();
            }
        }

        //On met le médecin à la centrale         
        /*  private void PositionneEnPause(string NomMed)     
          {
              //On vérifie que la carto est ouverte
              if (CarteOuverte() == "OK")
              {                
                  //On affiche sur la carte....Si elle est active
                  FCarto.MedecinEnPause(NomMed);                        
              }                     
          }*/




        //***************************FIN CONCERNE LA CARTE**********************************
        #endregion

        #region Infirmiere
        //******************A replacer dans le code ------Plus haut
        //Gestion des visites des infirmières
        private void bInfirmiereEtat_Click(object sender, EventArgs e)
        {
            //Affichage de la forme des infirmières

            //on stop le timer
            timer1.Stop();

            FListeVisiteOrg fListeVisiteOrg = new FListeVisiteOrg();            
            fListeVisiteOrg.ShowDialog(this);
            fListeVisiteOrg.Dispose();

            //on redemarre le timer
            timer1.Start();
        }

        private void AfficheFormAttribInf(string Visite)
        {
            //on stop le timer
            timer1.Stop();

            FListeInfirmiere fListeInfirmiere = new FListeInfirmiere();            
            fListeInfirmiere.numVisite = Visite;
            fListeInfirmiere.ShowDialog(this);
            fListeInfirmiere.Dispose();

            //on redemarre le timer
            timer1.Start();
        }

        private void AfficheCustomMenu(object sender, EventArgs e, string Bouton1, string Bouton2)
        {
            CustomMenuBox1 customMenuBox1 = new CustomMenuBox1(Bouton1, Bouton2, Cursor.Position.X, Cursor.Position.Y);
            DialogResult Reponse = customMenuBox1.ShowDialog(this);
            customMenuBox1.Dispose();

            if(Reponse == DialogResult.OK)  //Si ok => affichage de la Form FListeInfirmiere pour attribution de la visite
            {
                AfficheFormAttribInf(listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NumAppel].Text);
            }
            else if (Reponse == DialogResult.No)     //Si c'est No, alors on affiche la boite Messagerie
            {
                timer1.Stop();

                FMessage fMessage = new FMessage();
                fMessage.CodeMedecin = listView2.SelectedItems[0].Text;
                fMessage.Destinataire = listView2.SelectedItems[0].SubItems[(int)ListView2Columns.NomMed].Text;

                fMessage.ShowDialog(this);
                fMessage.Dispose();

                //On rafraichi la liste des médecins dans le dispatch                        
                RafraichiListes(sender, e);

                //On le redémarre
                timer1.Start();
            }
        }
        #endregion

       
    }
}


//A faire
//afficher boite des rdv au lancement?





