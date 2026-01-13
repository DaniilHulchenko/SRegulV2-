using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using MySqlConnector;
using System.Text.RegularExpressions;
using GMap.NET;
using GMap.NET.MapProviders;
using System.Net;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
//using SRegulV2.FonctionsAppels;

namespace SRegulV2
{
              
    public partial class FVisite : Form
    {
        public int NumVisite = -1;
        public int NumPersonne = -1;
        public string provenance = "MainForm";
        private bool nonNumerique = false;    //Pour s'assurer qu'on rentre bien un caractere numerique, ou le +
        private bool nonNumeriqueUrg = false;  //Pour l'urgence
        private bool nonNumeriqueCP1 = false;  //Pour le cp adresse
        private bool nonNumeriqueCP2 = false;  //Pour le cp adresse facturation
        private bool toucheF5 = false;  //Pour la touche F5 seulement autorisée
        private bool TropLong = false;
        private string IdMotif1 = "";          //Pour mémoriser les motifs
        private string IdMotif2 = "";
        private string IdMotifAnnul = "";
        private bool TA = false;                     //Est ce un TA?
        private bool EmailTaEnvoye = false;          //Est ce qu'on a déjà envoyé un EmailTA?
        private bool EmailInfEnvoye = false;          //Est ce qu'on a déjà envoyé un Email Infirmière
        private bool Remarquable = false;            //Est ce un patient remarquable?     
        private bool FactureImpayee = false;            //Y a t'il des factures impayées?    
        private string IdUnilab = "";
        private DateTime? DateAccord = null;        //A t'il déjà donné son accord pour la LPD
        private int IdMedecinTraitant = -1;
        private string NomMedecinTraitant = "";
        private string PrenomMedecinTraitant = "";
        private bool MajMedecinTraitantTexte = false;
        //private DataTable dt10DerAppels = new DataTable();
        private DataTable dt20DerAppels = new DataTable();


        public static PointLatLng ptNonGeocode = new PointLatLng();         //Point par défaut pour les adresses non géocodées                

        public static double LatN;     //On détermine le bornage autour de genève
        public static double LatS;
        public static double LngW;
        public static double LngE;

        //Pour le retour de la recherche d'assurance
        public static string NumAssure = "", NomAssure = "", PrenomAssure = "", DateNaissanceAssure = "", GenreAssure = "";
        public static string[] AdresseDecompose = new string[5];

        public static DataTable dtParam;   //Pour stocker les paramètres 


        /*  public DataTable dtProv = new DataTable();
          public DataTable dtCommune = new DataTable();
          public DataTable dtMotif = new DataTable();*/

        public FVisite()
        {
            InitializeComponent();

            listView1.Columns.Add("Etat", 260);

            //Pour la securité
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //On défini le point comme séparateur décimal
            CultureInfo CultureFr = new CultureInfo("fr-CH");
            CultureFr.NumberFormat.NumberDecimalSeparator = ".";           
        }

        private void bAbandonner_Click(object sender, EventArgs e)
        {
            //On abandonne la visite
            DialogResult dialogResult = MessageBox.Show("Voulez vous abandonner cette visite?", "Abandon de visite", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                FonctionsCTI.ReactiveProxyHIN();
                //On ferme la form
                this.Close();
            }
        }

        //On efface les champs adresse et adresse de facturation
        private void beffaceAdresse_Click(object sender, EventArgs e)
        {
            tBoxCP.Text = "";
            tBoxCommune.Text = "";
            cBoxPays.Text = "Suisse";            
            tBoxNumRue.Text ="";
            tBRue.Text = "";
            tBRueCompl.Text = "";
            tBDigicode.Text = "";

            //Interphone
            tBoxNomPorte.Text = "";
            cBoxInterphone.Checked = false;            
            tBoxEtage.Text = "";

            //On tronque la chaine
            tBoxPorte.Text = "";
            
            //Pour l'adresse de facturation
            if (cBoxAutreAdrFact.Checked == true)
            {
                tBoxNomFact.Text = "";
                tBoxPrenomFact.Text = "";
                tBoxCPFact.Text = "";
                tBoxCommuneFact.Text = "";
                cBoxPaysFact.Text = "";
                tBoxNumRueFact.Text = "";
                tBoxRueFact.Text = "";
                tBoxRue2Fact.Text = "";
                cBoxAutreAdrFact.Checked = false;
            }

            //Puis on désactive Bvalidé (s'il était actif) et l'envoi de mail aux infirmières
            bValider.Enabled = false;
          //  bMailInf.Enabled = false;
        }

        private void bValider_Click(object sender, EventArgs e)
        {
            //On valide la visite                                                                    
            //1 - Si on est en création
            //Ajout un enreg dans Appels, AppelsEnCours, SuiviAppel, AdrFacturation (s'il y a ) et Status_Visite
            if (VerifSaisie() == "OK")
            {
                //On incrémente le compteur de rafraichissement (base)
                FonctionsAppels.IncrementeRafraichissement();

                //Si c'est une nouvelle visite.
                if (NumVisite == -1)                    
                {
                    //ET un nouveau patient..
                    if (NumPersonne == -1)
                    {
                        //On vérifie que ce n'est pas un doublon
                        DataTable dtVerif = FonctionsAppels.VerifSiDoublon(tBoxNom.Text, tBoxPrenom.Text, mTBoxDateNaiss.Text);

                        if (dtVerif.Rows.Count > 0)
                        {
                            //On averti de la création possible d'un doublon Mais avant on regarde si c'est un TA
                            string TA = "";
                            if (dtVerif.Rows[0]["TeleAlarme"].ToString() == "O")
                                TA = "En plus c'est un Télé-Alarme!\r\n";

                            DialogResult dialogResult = MessageBox.Show("Attention probabilité forte de création de doublon avec \r\n" +
                                         dtVerif.Rows[0]["Nom"].ToString() + " " + dtVerif.Rows[0]["Prenom"].ToString() + " né le " + dtVerif.Rows[0]["DateNaissance"].ToString() + ".\r\n" +
                                         dtVerif.Rows[0]["NumeroDansRue"].ToString() + " " + dtVerif.Rows[0]["Rue"].ToString() + " " + dtVerif.Rows[0]["CodePostal"].ToString() + " " +
                                         dtVerif.Rows[0]["Commune"].ToString() + ".\r\n" + TA + "\r\n" +
                                         "Je vous conseille d'approfondir la recherche. Voulez-vous vraiment continuer ?", "Attention Doublon!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); ;

                            //Création d'un doublon!
                            if (dialogResult == DialogResult.Yes)
                            {
                                //On donne un nouvel IdUnilab                                
                                IdUnilab = FonctionsAppels.ToUnixTime(DateTime.Now).ToString();

                                int NumAppel = CreationAppel(NumPersonne);

                                if (NumAppel != -1)  //Tout c'est bien passé
                                {
                                    //On créer maintenant la fiche de consultation
                                if (CreerFicheConsult(NumAppel, NumPersonne) == "KO")                                    
                                        MessageBox.Show("Un problème est survenu lors de la création de la Fiche de consultation pour cette visite.", "Création de la fiche de consultation", MessageBoxButtons.OK, MessageBoxIcon.Warning);                                    

                                    EnregistrerMedecinTraitant();
                                                                                                          
                                    mouchard.evenement("Création d''un doublon!, appel n°: " + NumAppel +
                                        " de " + tBoxNom.Text + " " + tBoxPrenom.Text + " n° pers. :" + NumPersonne + " né le " + mTBoxDateNaiss.Text + " Adr: " + tBRue.Text.Replace("'", "''").Replace(",", ",,") + " " + tBoxNumRue.Text + " " +
                                        tBoxCP.Text + " " + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,"), Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log                        

                                    //On réactive le proxy HIN
                                    FonctionsCTI.ReactiveProxyHIN();

                                    //On ferme la fiche                                        
                                    this.Close();
                                }
                            }
                        }
                        else
                        {
                            //Pas de doublon
                            //On donne un nouvel IdUnilab                                
                            IdUnilab = FonctionsAppels.ToUnixTime(DateTime.Now).ToString();

                            int NumAppel = CreationAppel(NumPersonne);

                            if (NumAppel != -1)  //Tout c'est bien passé
                            {
                                //On créer maintenant la fiche de consultation
                                if (CreerFicheConsult(NumAppel, NumPersonne) == "KO")                                
                                    MessageBox.Show("Un problème est survenu lors de la création de la Fiche de consultation pour cette visite.", "Création de la fiche de consultation", MessageBoxButtons.OK, MessageBoxIcon.Warning);                                

                                EnregistrerMedecinTraitant();

                                mouchard.evenement("Création appel n°: " + NumAppel +
                                    " de " + tBoxNom.Text + " " + tBoxPrenom.Text + " n° pers. :" + NumPersonne + " né le " + mTBoxDateNaiss.Text + " Adr: " + tBRue.Text.Replace("'", "''").Replace(",", ",,") + " " + tBoxNumRue.Text + " " +
                                    tBoxCP.Text + " " + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,"), Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log                        
                                                                                                                                                                                            //On réactive le proxy HIN
                                FonctionsCTI.ReactiveProxyHIN();

                                //On ferme la fiche                                        
                                this.Close();
                            }
                        }
                    }
                    else  //C'est une personne déjà connue, on continu
                    {
                        int NumAppel = CreationAppel(NumPersonne);

                        if (NumAppel != -1)  //Tout c'est bien passé
                        {
                            //On créer maintenant la fiche de consultation
                            if (CreerFicheConsult(NumAppel, NumPersonne) == "KO")                            
                                MessageBox.Show("Un problème est survenu lors de la création de la Fiche de consultation pour cette visite.", "Création de la fiche de consultation", MessageBoxButtons.OK, MessageBoxIcon.Warning);                            

                            EnregistrerMedecinTraitant();

                            mouchard.evenement("Création appel n°: " + NumAppel +
                                " de " + tBoxNom.Text + " " + tBoxPrenom.Text + " n° pers. :" + NumPersonne + " né le " + mTBoxDateNaiss.Text +" Adr: " + tBRue.Text.Replace("'", "''").Replace(",", ",,") + " " + tBoxNumRue.Text + " " +
                                tBoxCP.Text + " " + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,"), Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log                        
                                                                                                                                                                                        //On réactive le proxy HIN
                            FonctionsCTI.ReactiveProxyHIN();

                            //On ferme la fiche                                        
                            this.Close();
                        }
                    }                              
                }
                else   //C'est une modification
                {
                    //2 - Si on est en modif                 
                    //On vérifie qu'on ne créer pas de Doublon la encore après la MAJ
                    if (NumPersonne == -1)
                    {
                        //On vérifie que ce n'est pas un doublon
                        DataTable dtVerif = FonctionsAppels.VerifSiDoublon(tBoxNom.Text, tBoxPrenom.Text, mTBoxDateNaiss.Text);

                        if (dtVerif.Rows.Count > 0)
                        {
                            //On averti de la création possible d'un doublon Mais avant on regarde si c'est un TA
                            string TA = "";
                            if (dtVerif.Rows[0]["TeleAlarme"].ToString() == "O")
                                TA = "En plus c'est un Télé-Alarme!\r\n";

                            DialogResult dialogResult = MessageBox.Show("Attention probabilité forte de création de doublon avec \r\n" +
                                         dtVerif.Rows[0]["Nom"].ToString() + " " + dtVerif.Rows[0]["Prenom"].ToString() + " né le " + dtVerif.Rows[0]["DateNaissance"].ToString() + ".\r\n" +
                                         dtVerif.Rows[0]["NumeroDansRue"].ToString() + " " + dtVerif.Rows[0]["Rue"].ToString() + " " + dtVerif.Rows[0]["CodePostal"].ToString() + " " +
                                         dtVerif.Rows[0]["Commune"].ToString() + ".\r\n" + TA + "\r\n" +
                                         "Je vous conseille d'approfondir la recherche. Voulez-vous vraiment continuer ?", "Attention Doublon!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); ;

                            //Création d'un doublon!
                            if (dialogResult == DialogResult.Yes)
                            {
                                //On donne un nouvel IdUnilab                                
                                if (IdUnilab == "")
                                    IdUnilab = FonctionsAppels.ToUnixTime(DateTime.Now).ToString();

                                MajAppel();   //Maj de l'appel

                                EnregistrerMedecinTraitant();
                                
                                mouchard.evenement("Création d''un doublon pendant la maj de l''appel!, appel n°: " + NumVisite +
                                        " de " + tBoxNom.Text + " " + tBoxPrenom.Text + " n° pers. :" + NumPersonne + " né le " + mTBoxDateNaiss.Text + " Adr: " + tBRue.Text.Replace("'", "''").Replace(",", ",,") + " " + tBoxNumRue.Text + " " +
                                        tBoxCP.Text + " " + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,"), Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log                        

                                //On réactive le proxy HIN
                                FonctionsCTI.ReactiveProxyHIN();

                                //On ferme la fiche                                        
                                this.Close();                                
                            }
                        }
                        else
                        {
                            //On donne un nouvel IdUnilab                                
                            if (IdUnilab == "")
                                IdUnilab = FonctionsAppels.ToUnixTime(DateTime.Now).ToString();

                            MajAppel();   //Maj de l'appel                                                      

                            EnregistrerMedecinTraitant();

                            mouchard.evenement("Maj de l'appel n° " + NumVisite + " concernant " + tBoxNom.Text + " " + tBoxPrenom.Text + " n° pers. :" + NumPersonne + " né le " + mTBoxDateNaiss.Text + " Adr: " + tBRue.Text.Replace("'", "''").Replace(",", ",,") + " " + tBoxNumRue.Text + " " +
                                    tBoxCP.Text + " " + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,"), Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log                        

                            //On réactive le proxy HIN
                            FonctionsCTI.ReactiveProxyHIN();

                            //Puis on ferme la form
                            this.Close();
                        }
                    }
                    else   //personne déjà connue
                    {
                        MajAppel();  //Maj de l'appel

                        EnregistrerMedecinTraitant();
                       
                        mouchard.evenement("Maj de l'appel n° " + NumVisite + " concernant " + tBoxNom.Text + " " + tBoxPrenom.Text + " n° pers. :" + NumPersonne +" né le " + mTBoxDateNaiss.Text + " Adr: " + tBRue.Text.Replace("'", "''").Replace(",", ",,") + " " + tBoxNumRue.Text + " " +
                                tBoxCP.Text + " " + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,"), Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log                        

                        //On réactive le proxy HIN
                        FonctionsCTI.ReactiveProxyHIN();

                        //Puis on ferme la form
                        this.Close();
                    }
                }
                //Puis on met à jour/vérifie la présence des fichiers audios sur le serveur Nginx
               // if (Form1.ActivationBandeAudio == true)
                   // FonctionsAppels.VerifSiAudioPresent();                             
            }
        }        

        //On envoi un email aux ambulanciers avec les informations de la fiche TA
        private void bMailTA_Click(object sender, EventArgs e)
        {
            //On regarde si on a déjà envoyé un email pour cet appel
            if (EmailTaEnvoye == true)
            {
                DialogResult dialogResult = MessageBox.Show("Vous avez déjà envoyé un email pour cette visite\r\n" +
                                         "Voulez vous l'envoyer de nouveau ?", "Email déjà envoyé", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    EnvoyeEmailTA();
                }
            }
            else
            {
                EnvoyeEmailTA();
            }
        }


        //On envoi un email TA
        private void EnvoyeEmailTA()
        { 
            //Formatage du message avec les données TA du patient
            string message;           
            string Sexe = "";

            if (rBMal.Checked == true)
                Sexe = "Monsieur";
            else
                Sexe = "Madame";

            message = $@"<html><head><style type = 'text/css'>body, p, div {{font-family: Helvetica, Arial, sans-serif;font-size: 14px;}}a{{text-decoration: none;}}</style><title></title></head>";
            message += $@"<body><h3>Intervention TéléAlarme, le {DateTime.Now}</h3><p></p><p>Concerne : <strong style=""color: blue; "">{Sexe} {tBoxNom.Text} {tBoxPrenom.Text}</strong></p>";
            message += $@"<p>Date de naissance : <strong style=""color: blue; "">{mTBoxDateNaiss.Text} => {tBoxAge.Text} ans</strong></p>";
            message += $@"<p>Adresse : <strong style=""color: blue; "">{tBRue.Text}, {tBoxNumRue.Text}</strong></p><p><strong style=""color: blue; "">{tBRueCompl.Text}</strong></p>";
            message += $@"<p><strong style=""color: blue; "">{tBoxCP.Text}, </strong></p><p><strong style=""color: blue; "">{tBoxCommune.Text}</strong></p>";
            message += $@"<p>Digicode :<strong style=""color: blue; "">{tBDigicode.Text}</strong></br>Interphone : <strong style=""color: blue; "">{tBoxNomPorte.Text}</strong></p>";
            message += $@"<div>Etage :<strong style=""color: blue; "">{tBoxEtage.Text}</strong></br>Porte: <strong style=""color: blue; "">{tBoxPorte.Text}</strong></div>";           
            message += $@"<p></p><h4>Motif de l'intervention :</h4><div><strong style=""color: red; "">{tBoxMotif1.Text}</strong></div><div><strong style=""color: red; "">{tBoxMotif2.Text}</strong></div>";
            message += $@"<p><strong style=""color: red; "">{tBCommentaireDiag.Text}</strong></p><p><strong style=""color: red; "">{tBoxCondPart.Text}</strong></p>";
            message += $@"<p></p><h4>Clé</h4><div><strong style=""color: blue; "">{label7.Text.Replace("\r\n", "</br>")}</strong>,</br><strong style=""color: blue; "">{label9.Text.Replace("\r\n","</br>")}</strong></div></br>";
            message += $@"<div><strong style=""color: blue; "">{label8.Text.Replace("\r\n","</br>")}</strong></div>";
            message += $@"<h4>Autres infos</h4><div>{label10.Text.Replace("\r\n","</br>")}</br>{label11.Text.Replace("\r\n","</br>")}</div></br>";
            message += $@"</body></html>";

            string ReponseEnvoiMail = "KO";

            if (Form1.EmailTADest != "" && Form1.EmailTAFrom != "")
            {
                //sos@sk-transports.ch;info-regulation@sos-medecins.ch
                 ReponseEnvoiMail = EmailService.SendMail(
                                                    to: Form1.EmailTADest,
                                                    subject: "Intervention TéléAlarme",
                                                    html: $@"{message}",
                                                    from: Form1.EmailTAFrom,
                                                    typeDest: "TA"
                                                    );
            }

            picBoxEnvoiMail.Visible = true;

            //Si l'envoi c'est bien passé
            if (ReponseEnvoiMail == "OK")
            {
                picBoxEnvoiMail.Image = imageList2.Images[0];
                mouchard.evenement("Envoi d'un email TA, appel n°: " + NumVisite +
                                       " de " + tBoxNom.Text + " " + tBoxPrenom.Text + " n° pers. :" + NumPersonne + " né le " + mTBoxDateNaiss.Text, Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log                        

                //On met à jour la fiche d'appel pour signaler l'envoi d'un Email
                MajMailEnvoye("TA");                               
            }
            else   //Sinon on affiche le message d'erreur
            {
                picBoxEnvoiMail.Image = imageList2.Images[1];
                MessageBox.Show(ReponseEnvoiMail, "Erreur lors de l'envoi du mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //On envoi un email aux infirmières avec les informations de la fiche
        private void bMailInf_Click(object sender, EventArgs e)
        {
            //On regarde si on a déjà envoyé un email pour cet appel
            if (EmailInfEnvoye == true)
            {
                DialogResult dialogResult = MessageBox.Show("Vous avez déjà envoyé un email pour cette visite\r\n" +
                                         "Voulez vous l'envoyer de nouveau ?", "Email déjà envoyé", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    EnvoyeEmailInfirmiere();
                }
            }
            else
            {
                EnvoyeEmailInfirmiere();
            }          
        }


        //On envoi un email aux infirmières               
        private void EnvoyeEmailInfirmiere()
        {            
            //Formatage du message avec les données du patient
              string message;
              string Sexe = "";

              if (rBMal.Checked == true)
                  Sexe = "Monsieur";
              else
                  Sexe = "Madame";

              message = $@"<html><head><style type = 'text/css'>body, p, div {{font-family: Helvetica, Arial, sans-serif;font-size: 14px;}}a{{text-decoration: none;}}</style><title></title></head>";
              message += $@"<body><h3>Demande de visite, le {DateTime.Now}</h3><p></p><p>Concerne : <strong style=""color: blue; "">{Sexe} {tBoxNom.Text} {tBoxPrenom.Text}</strong></p>";
              message += $@"<p>Date de naissance : <strong style=""color: blue; "">{mTBoxDateNaiss.Text} => {tBoxAge.Text} ans</strong></p>";

              message += $@"<p>Téléphone appellant : <strong style=""color: blue; "">{tBAppellant.Text}</strong></p>";
              message += $@"<p>Téléphone du patient : <strong style=""color: blue; "">{tBtelPatient.Text}</strong></p>";

              message += $@"<p>Adresse : <strong style=""color: blue; "">{tBRue.Text}, {tBoxNumRue.Text}</strong></p><p><strong style=""color: blue; "">{tBRueCompl.Text}</strong></p>";
              message += $@"<p><strong style=""color: blue; "">{tBoxCP.Text}, </strong></p><p><strong style=""color: blue; "">{tBoxCommune.Text}</strong></p>";
              message += $@"<p>Digicode :<strong style=""color: blue; "">{tBDigicode.Text}</strong></br>Interphone : <strong style=""color: blue; "">{tBoxNomPorte.Text}</strong></p>";
              message += $@"<div>Etage :<strong style=""color: blue; "">{tBoxEtage.Text}</strong></br>Porte: <strong style=""color: blue; "">{tBoxPorte.Text}</strong></div>";
              message += $@"<p></p><h4>Motif de l'intervention :</h4><div><strong style=""color: red; "">{tBoxMotif1.Text}</strong></div><div><strong style=""color: red; "">{tBoxMotif2.Text}</strong></div>";

              message += $@"<p><strong style=""color: red; "">{tBCommentaireDiag.Text}</strong></p><p><strong style=""color: red; "">{tBoxCondPart.Text}</strong></p>";

              message += $@"<p></p><h4>Assurance:</h4><div><strong style=""color: blue; "">{tBoxAssuranceNom.Text}</strong>,</br>";
              message += $@"<p></p><h4>Numéro assure:</h4><div><strong style=""color: blue; "">{tBoxNumCarte.Text}</strong>,</br>";

              message += $@"</body></html>";

              string ReponseEnvoiMail = "KO";

              //report@sitexsa.ch;info-regulation@sos-medecins.ch
              if (Form1.EmailInfDest != "" && Form1.EmailInfFrom != "")
              {
                  ReponseEnvoiMail = EmailService.SendMail(
                                                      to: Form1.EmailInfDest,
                                                      subject: "Consultation SOS Chez un patient",
                                                      html: $@"{message}",
                                                      from: Form1.EmailInfFrom,
                                                      typeDest: "Infirmiere"
                                                      );
              }

              picBoxEnvoiMail.Visible = true;

              //Si l'envoi c'est bien passé
              if (ReponseEnvoiMail == "OK")
              {
                  picBoxEnvoiMail.Image = imageList2.Images[0];
                  mouchard.evenement("Envoi d'un email infirmière, appel n°: " + NumVisite +
                                         " de " + tBoxNom.Text + " " + tBoxPrenom.Text + " n° pers. :" + NumPersonne + " né le " + mTBoxDateNaiss.Text, Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log                        

                  //On met à jour la fiche d'appel pour signaler l'envoi d'un Email
                  MajMailEnvoye("Infirmiere");
              }
              else   //Sinon on affiche le message d'erreur
              {
                  picBoxEnvoiMail.Image = imageList2.Images[1];
                  MessageBox.Show(ReponseEnvoiMail, "Erreur lors de l'envoi du mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
        }
        

        private void FVisite_Load(object sender, EventArgs e)
        {            
            //Chargement des coordonnées géographiques pour le géocodage de la visite
            dtParam = FonctionsAppels.ChargeParam();

            if (dtParam.Rows.Count > 0)
            {
                //Zone géographique de géocodage
                LatN = double.Parse(dtParam.Rows[0]["ZGeoLatN"].ToString());
                LatS = double.Parse(dtParam.Rows[0]["ZGeoLatS"].ToString());
                LngW = double.Parse(dtParam.Rows[0]["ZGeoLngW"].ToString());
                LngE = double.Parse(dtParam.Rows[0]["ZGeoLngE"].ToString());
               
                //Position de la Zone pour adresse non Géocodées
                ptNonGeocode.Lat = double.Parse(dtParam.Rows[0]["ZStockLat"].ToString());
                ptNonGeocode.Lng = double.Parse(dtParam.Rows[0]["ZStockLng"].ToString());
            }            
            
            
            //On prépare la fiche en fonction Visite...Si numero != -1 => Modification, ou consultation (Si Suivi Appel est Terminé), sinon => Création 
            if (NumVisite == -1)     //Nouvelle fiche
            {
                //MessageBox.Show("Nouvelle Fiche");
                this.Text = "Nouvelle visite";
                
                //On enlève les onglets TA et Remarque par défaut             
                tabControl1.TabPages["tabPageTA"].Text = "";
                tabControl1.TabPages["tabPageRemarque"].Text = "";

                TA = false;                    
                Remarquable = false;            

                labelTA.Text = "";
                labelRemarque.Text = "";
                lRemMedicales.Text = "";
                labelFactureImp.Text = "";

                //initialiser quelques champs
                cBoxPays.Text = "Suisse";
                MajMedecinTraitantTexte = true;
                tBoxMedTraitant.Text = "";
                MajMedecinTraitantTexte = false;
                IdMedecinTraitant = -1;
                NomMedecinTraitant = "";
                PrenomMedecinTraitant = "";

                //Désactivation de Annulation
                cBoxAnnulation.Checked = false;
                tBoxMotifAnnul.Enabled = false;


                //Si le CTI est actif on active le bouton
                if (Form1.ActivationCTI == true)
                    bRappeler.Enabled = true;
                else bRappeler.Enabled = false;

                //On désactive les contrôles Autre adresse pour facturation
                cBoxAutreAdrFact.Checked = false;
                tBoxNomFact.Enabled = false;
                tBoxPrenomFact.Enabled = false;
                tBoxCPFact.Enabled = false;
                tBoxCommuneFact.Enabled = false;
                cBoxPaysFact.Enabled = false;
                tBoxNumRueFact.Enabled = false;
                tBoxRueFact.Enabled = false;
                tBoxRue2Fact.Enabled = false;
           
                bAbandonner.Visible = true;
                bAbandonner.Enabled = true;
                bValider.Enabled = false;
                bMailInf.Enabled = false;
                bFermer.Visible = false;
                beffaceAdresse.Visible = false;  //Effacement adresse existante
                
                bMailTA.Visible = false;            //Envoi de l'eamil TA
                picBoxEnvoiMail.Visible = false;    //reponse de l'envoi de l'email
                bConsentement.Enabled = false;     

                //On donne le focus au telephone
                this.ActiveControl = tBAppellant;
            }
            else if (NumVisite != -1)   //Fiche existante
            {
                //On enlève les onglets TA et Remarque par défaut (quitte à les remettre après)                          
                tabControl1.TabPages["tabPageTA"].Text = "";
                tabControl1.TabPages["tabPageRemarque"].Text = "";

                labelTA.Text = "";
                labelRemarque.Text = "";
                lRemMedicales.Text = "";
                labelFactureImp.Text = "";
                IdUnilab = "";

                picBoxEnvoiMail.Visible = false;    //dans tout les cas

                //On charge ici l'enregistrement Appels correspondant, Ainsi que l'historique de ce dernier (table SuiviAppel)
                DataSet DsAppel = new DataSet();
                DsAppel = FonctionsAppels.RetourneVisite(NumVisite);   //Retourne les 4 tables qui concerne l'appel

                beffaceAdresse.Visible = true;    //on peut effacer l'adresse de la fiche par défaut.

                PeuplerChampsAppel(DsAppel);               

                //Si l'appel est terminé ET transféré
                if (bool.Parse(DsAppel.Tables["Appels"].Rows[0]["Termine"].ToString()) == true && 
                     (bool.Parse(DsAppel.Tables["Appels"].Rows[0]["Export"].ToString()) == true || DsAppel.Tables["Appels"].Rows[0]["Export"].ToString() != ""))
                {
                    this.Text = "Consultation de la visite " + NumVisite.ToString();

                    beffaceAdresse.Visible = false;   //Dans tout les cas

                    bAbandonner.Visible = false;  //On ne peut plus abandonner une fiche déjà créée Mais on peut la fermer
                    bValider.Visible = false;
                    bMailInf.Visible = false;

                    cBoxPatientRappel.Visible = false;   //Le rappel du patient ne sert plus à rien, on le cache

                    bMailTA.Visible = false;        //Cette visite est terminée on ne peut plus envoyer un Email TA
                   
                    bFermer.Visible = true;
                    bFermer.Enabled = true;
                }
                else
                {
                    //Si elle n'est pas transférée on peut la modifier
                    this.Text = "Modification de la visite " + NumVisite.ToString();                    

                    bAbandonner.Visible = false;  //On ne peut plus abandonner une fiche déjà créée Mais on peut la fermer
                    bAbandonner.Enabled = false;
                    bValider.Visible = true;
                    bValider.Enabled = true;
                    
                   // bMailInf.Visible = true;
                   // bMailInf.Enabled = true;

                    //Le rappel du patient est actif
                    cBoxPatientRappel.Visible = true;      

                    //et on va vérifier son état (table AppelsEnCours)
                    if (FonctionsAppels.PatientRappel(NumVisite) == "0")
                        cBoxPatientRappel.Checked = false;
                    else cBoxPatientRappel.Checked = true;

                    //Si c'est une fiche TA, on peut envoyer un email
                    if (TA == true)
                        bMailTA.Visible = true;
                    else
                        bMailTA.Visible = false;

                    bFermer.Visible = true;
                }
            }   //Fin de visite Existe

            //En fonction de la provenance
            if (provenance == "MainForm")
                b10visites.Visible = true;
            else
                b10visites.Visible = false;

            //On désactive le proxy HIN
            FonctionsCTI.DesactiveProxyHIN();
        }


        //vérification des saisies
        private string VerifSaisie()
        {
            string retour = "KO";
            int V1 = 0;

            if (tBAppellant.Text != "")     //Formatage du Telephone appelant
            {
                tBAppellant.BackColor = SystemColors.ControlDark;

                //Formatage du n° de Tel: Si c'est au format International
                tBAppellant.Text = FormateNumTel(tBAppellant.Text);               

                V1 += 1;
            }
            else
            {
                tBAppellant.BackColor = Color.Red;
            }


            if (tBtelPatient.Text != "")     //Formatage du Telephone Patient
            {
                tBtelPatient.BackColor = SystemColors.ControlDark;

                //Formatage du n° de Tel: Si c'est au format International
                tBtelPatient.Text = FormateNumTel(tBtelPatient.Text);               

                V1 += 1;
            }
            else if (tBAppellant.Text != "")     //sinon si le n° Appelant n'est pas vide, on le copie dans TelPatient
            {
                tBtelPatient.Text = tBAppellant.Text;
                tBtelPatient.BackColor = SystemColors.ControlDark;
                V1 += 1;
            }
            else
            {
                tBtelPatient.BackColor = Color.Red;
            }


            if (tBoxProvenance.Text != "")
            {
                V1 += 1;
                tBoxProvenance.BackColor = SystemColors.ControlDark;
            }
            else
                tBoxProvenance.BackColor = Color.Red;

            if (tBoxNom.Text != "" && tBoxNom.Text != "<NOUVEAU>")
            {
                V1 += 1;
                tBoxNom.BackColor = SystemColors.ControlDark;
            }
            else
                tBoxNom.BackColor = Color.Red;

            if (tBoxCP.Text != "")
            {
                V1 += 1;
                tBoxCP.BackColor = SystemColors.ControlDark;
            }
            else
                tBoxCP.BackColor = Color.Red;

            if (tBoxCommune.Text != "")
            {
                V1 += 1;
                tBoxCommune.BackColor = SystemColors.ControlDark;
            }
            else
                tBoxCommune.BackColor = Color.Red;

            if (cBoxPays.Text != "")
            {
                V1 += 1;
                cBoxPays.BackColor = SystemColors.ControlDark;
            }
            else
                cBoxPays.BackColor = Color.Red;

            if (tBoxNumRue.Text != "")
            {
                V1 += 1;
                tBoxNumRue.BackColor = SystemColors.ControlDark;
            }
            else
                tBoxNumRue.BackColor = Color.Red;

            if (tBRue.Text != "")
            {
                V1 += 1;
                tBRue.BackColor = SystemColors.ControlDark;
            }
            else
                tBRue.BackColor = Color.Red;

            //Pour le sexe
            if (rBMal.Checked == true || rBFemelle.Checked == true)
            {
                V1 += 1;
                rBMal.BackColor = SystemColors.ControlDarkDark;
                rBFemelle.BackColor = SystemColors.ControlDarkDark;                
            }
            else
            {
                rBMal.BackColor = Color.Red;
                rBFemelle.BackColor = Color.Red;
            }

            if (tBoxMotif1.Text != "")
            {
                V1 += 1;
                tBoxMotif1.BackColor = SystemColors.ControlDark;
            }
            else
                tBoxMotif1.BackColor = Color.Red;

            if (tBoxUrgence.Text != "")
            {
                V1 += 1;
                tBoxUrgence.BackColor = SystemColors.ControlDark;
            }
            else
                tBoxUrgence.BackColor = Color.Red;

            //Si autre adresse est cochée, on vérifie les chammps
            if (cBoxAutreAdrFact.Checked)
            {
                V1 += 10;
                if (tBoxNomFact.Text != "")
                {
                    V1 += 1;
                    tBoxNomFact.BackColor = SystemColors.ControlDark;
                }
                else                
                    tBoxNomFact.BackColor = Color.Red;                

                if (tBoxPrenomFact.Text != "")
                {
                    V1 += 1;
                    tBoxPrenomFact.BackColor = SystemColors.ControlDark;
                }
                else
                    tBoxPrenomFact.BackColor = Color.Red;

                if (tBoxCPFact.Text != "")
                {
                    V1 += 1;
                    tBoxCPFact.BackColor = SystemColors.ControlDark;
                }
                else
                    tBoxCPFact.BackColor = Color.Red;

                if (tBoxCommuneFact.Text != "")
                {
                    V1 += 1;
                    tBoxCommuneFact.BackColor = SystemColors.ControlDark;
                }
                else
                    tBoxCommuneFact.BackColor = Color.Red;
              
                if (cBoxPaysFact.Text != "")
                {
                    V1 += 1;
                    cBoxPaysFact.BackColor = SystemColors.ControlDark;
                }
                else
                    cBoxPaysFact.BackColor = Color.Red;

                if (tBoxNumRueFact.Text != "")
                {
                    V1 += 1;
                    tBoxNumRueFact.BackColor = SystemColors.ControlDark;
                }
                else
                    tBoxNumRueFact.BackColor = Color.Red;

                if (tBoxRueFact.Text != "")
                {
                    V1 += 1;
                    tBoxRueFact.BackColor = SystemColors.ControlDark;
                }
                else
                    tBoxRueFact.BackColor = Color.Red;
            }


            if (cBoxAnnulation.Checked)
            {
                V1 += 20;
                if (tBoxMotifAnnul.Text != "")
                    V1 += 1;
            }

            //Signaler en orange mais ce n'est pas bloquant pour la fiche
            //Email, on vérifie le format
            if (tBoxEmail.Text != "")
            {
                //Expression régulière pour valider une adresse e-mail
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regex = new Regex(pattern);

                //Vérifier si l'adresse e-mail correspond au modèle
                if (regex.IsMatch(tBoxEmail.Text))
                    tBoxEmail.BackColor = SystemColors.ControlDark;
                else
                    tBoxEmail.BackColor = Color.Orange;               
            }
            else
                tBoxEmail.BackColor = Color.Orange;


            //En fonction des résultats
            switch (V1)
            {
                case 12:
                case 23:
                case 27:
                case 29:
                case 33:
                case 38:
                case 48:
                case 50: retour = "OK"; break;
                default: retour = "KO"; break;
            }

            //Puis pour certains contrôles non obligatoires, on met la couleur SystemColors.ControlDark
            tBoxPrenom.BackColor = SystemColors.ControlDark;
            mTBoxDateNaiss.BackColor = SystemColors.ControlDark;
            tBoxAssuranceNom.BackColor = SystemColors.ControlDark;
            tBRueCompl.BackColor = SystemColors.ControlDark;                       
            tBoxMotif2.BackColor = SystemColors.ControlDark;                   
            tBoxMotifAnnul.BackColor = SystemColors.ControlDark; 
            tBoxRue2Fact.BackColor = SystemColors.ControlDark; 
 
            return retour;
        }

        private void EnregistrerMedecinTraitant()
        {
            if (NumPersonne == -1)
            {
                return;
            }

            FonctionsAppels.MajMedecinTraitant(NumPersonne, IdMedecinTraitant);
        }

        private void ChargerMedecinTraitant(int numPersonne)
        {
            MajMedecinTraitantTexte = true;

            int idMedecin;
            string nom;
            string prenom;

            if (FonctionsAppels.GetMedecinTraitant(numPersonne, out idMedecin, out nom, out prenom))
            {
                IdMedecinTraitant = idMedecin;
                NomMedecinTraitant = nom;
                PrenomMedecinTraitant = prenom;
                tBoxMedTraitant.Text = (nom + " " + prenom).Trim();
            }
            else
            {
                IdMedecinTraitant = -1;
                NomMedecinTraitant = "";
                PrenomMedecinTraitant = "";
                tBoxMedTraitant.Text = "";
            }

            MajMedecinTraitantTexte = false;
        }


        //On peuple les champs de l'appel à partir des DataSet (Base regulation) Appel existant
        private void PeuplerChampsAppel(DataSet DsAppel)
        {
            //L'appel                             
            //On affect le numpersonne à la variable globale
            NumPersonne = int.Parse(DsAppel.Tables["Appels"].Rows[0]["Num_Personne"].ToString());
            EmailTaEnvoye = bool.Parse(DsAppel.Tables["Appels"].Rows[0]["EmailTaEnvoye"].ToString());
            EmailInfEnvoye = bool.Parse(DsAppel.Tables["Appels"].Rows[0]["EmailInfEnvoye"].ToString());

            //Pour Le labo Unilab
            IdUnilab = DsAppel.Tables["Appels"].Rows[0]["IdUnilab"].ToString();

            if (DsAppel.Tables["Appels"].Rows.Count > 0)
            {
                tBAppellant.Text = DsAppel.Tables["Appels"].Rows[0]["Tel_Appel"].ToString();
                tBtelPatient.Text = DsAppel.Tables["Appels"].Rows[0]["Tel_Patient"].ToString();
                tBoxNom.Text = DsAppel.Tables["Appels"].Rows[0]["Nom"].ToString();
                tBoxPrenom.Text = DsAppel.Tables["Appels"].Rows[0]["Prenom"].ToString();
                tBoxProvenance.Text = DsAppel.Tables["Appels"].Rows[0]["Provenance"].ToString();

                try
                {
                    mTBoxDateNaiss.Text = DsAppel.Tables["Appels"].Rows[0]["DateNaissance"].ToString();
                }
                catch (Exception)
                {

                }

                //Calcul de l'âge
                DateTime DateNaiss;
                try
                {
                    DateNaiss = DateTime.Parse(DsAppel.Tables["Appels"].Rows[0]["DateNaissance"].ToString());
                    int Age = FonctionsAppels.CalculeAge(DateNaiss);
                    tBoxAge.Text = Age.ToString();
                }
                catch (Exception)
                {
                    tBoxAge.Text = "0";
                }

                tBoxCP.Text = DsAppel.Tables["Appels"].Rows[0]["CodePostal"].ToString();
                tBoxCommune.Text = DsAppel.Tables["Appels"].Rows[0]["Commune"].ToString();
                if (DsAppel.Tables["Appels"].Rows[0]["Pays"].ToString() == "")
                    cBoxPays.Text = "Suisse";
                else
                    cBoxPays.Text = DsAppel.Tables["Appels"].Rows[0]["Pays"].ToString();

                tBoxNumRue.Text = DsAppel.Tables["Appels"].Rows[0]["Num_Rue"].ToString();
                tBRue.Text = DsAppel.Tables["Appels"].Rows[0]["Adr1"].ToString();
                tBRueCompl.Text = DsAppel.Tables["Appels"].Rows[0]["Adr2"].ToString();
                tBDigicode.Text = DsAppel.Tables["Appels"].Rows[0]["Digicode"].ToString();

                //Interphone
                if (DsAppel.Tables["Appels"].Rows[0]["InterphoneNom"].ToString() != "" && DsAppel.Tables["Appels"].Rows[0]["InterphoneNom"] != DBNull.Value)
                {
                    tBoxNomPorte.Text = DsAppel.Tables["Appels"].Rows[0]["InterphoneNom"].ToString();
                    cBoxInterphone.Checked = true;
                }
                else
                {
                    tBoxNomPorte.Text = "";
                    cBoxInterphone.Checked = false;
                }

                tBoxEtage.Text = DsAppel.Tables["Appels"].Rows[0]["Etage"].ToString();

                //On tronque la chaine
                if (DsAppel.Tables["Appels"].Rows[0]["Porte"].ToString().Length > 19)
                    tBoxPorte.Text = DsAppel.Tables["Appels"].Rows[0]["Porte"].ToString().Substring(0, 19);
                else tBoxPorte.Text = DsAppel.Tables["Appels"].Rows[0]["Porte"].ToString();
                              
                //Pour les assurances       
                tBoxAssuranceNom.Text = DsAppel.Tables["Appels"].Rows[0]["Assurance"].ToString();
                tBoxNumCarte.Text = DsAppel.Tables["Appels"].Rows[0]["NumCarte"].ToString();

                tBoxEmail.Text = DsAppel.Tables["Appels"].Rows[0]["Email"].ToString();                

                //Pour le sexe
                switch (DsAppel.Tables["Appels"].Rows[0]["Sexe"].ToString())
                {
                    case "H": rBMal.Checked = true; rBFemelle.Checked = false; break;
                    case "F": rBMal.Checked = false; rBFemelle.Checked = true; break;
                    default: rBMal.Checked = false; rBFemelle.Checked = true; break;
                }

                //Pour les motifs 
                if (DsAppel.Tables["Appels"].Rows[0]["IdMotif1"].ToString() != "")
                {
                    DataTable DtMotif = FonctionsAppels.InfosMotif(DsAppel.Tables["Appels"].Rows[0]["IdMotif1"].ToString());
                    if (DtMotif.Rows.Count > 0 && DtMotif.Rows[0][0] != DBNull.Value)
                    {
                        tBoxMotif1.Text = DtMotif.Rows[0][1].ToString();
                        IdMotif1 = DtMotif.Rows[0][0].ToString();
                    }
                }

                if (DsAppel.Tables["Appels"].Rows[0]["IdMotif2"].ToString() != "")
                {
                    DataTable DtMotif = FonctionsAppels.InfosMotif(DsAppel.Tables["Appels"].Rows[0]["IdMotif2"].ToString());
                    if (DtMotif.Rows.Count > 0 && DtMotif.Rows[0][0] != DBNull.Value)
                    {
                        tBoxMotif2.Text = DtMotif.Rows[0][1].ToString();
                        IdMotif2 = DtMotif.Rows[0][0].ToString();
                    }
                }

                if (DsAppel.Tables["Appels"].Rows[0]["IdMotifAnnulation"].ToString() != "")
                {
                    DataTable DtMotif = FonctionsAppels.InfosMotif(DsAppel.Tables["Appels"].Rows[0]["IdMotifAnnulation"].ToString());
                    if (DtMotif.Rows.Count > 0 && DtMotif.Rows[0][0] != DBNull.Value)
                    {
                        cBoxAnnulation.Checked = true;
                        tBoxMotifAnnul.Text = DtMotif.Rows[0][1].ToString();
                        IdMotifAnnul = DtMotif.Rows[0][0].ToString();
                    }
                }

                //Le commentaire
                tBCommentaireDiag.Text = DsAppel.Tables["Appels"].Rows[0]["Commentaire"].ToString();

                tBoxCondPart.Text = DsAppel.Tables["Appels"].Rows[0]["CondParticuliere"].ToString();


                if (DsAppel.Tables["Appels"].Rows[0]["Urgence"].ToString() != "")
                {
                    tBoxUrgence.Text = DsAppel.Tables["Appels"].Rows[0]["Urgence"].ToString();
                }

                //****Précédentes visites****
                string Nom = DsAppel.Tables["Appels"].Rows[0]["Nom"].ToString();
                string Prenom = DsAppel.Tables["Appels"].Rows[0]["Prenom"].ToString();
                string DateNaissance = DsAppel.Tables["Appels"].Rows[0]["DateNaissance"].ToString();

                dt20DerAppels = FonctionsAppels.Derniers20Appels(Nom, Prenom, DateNaissance);

                if (dt20DerAppels.Rows.Count > 0)
                {
                    lDerAppel.Text = "Dernier appel le " + FonctionsAppels.convertDateMaria(dt20DerAppels.Rows[0]["DateOp"].ToString(), "Texte") +
                                                                                   "\r\n=> motif: " + dt20DerAppels.Rows[0]["LibelleMotif"].ToString();
                }
                else    //Pas d'appels précédents pour cette personne
                {
                    lDerAppel.Text = "Pas de d'appels précédents pour cette personne.";
                }


                //Pour le bouton accord Désactivé sur demande du patron
                /*DateTime dt;
                var ok = DateTime.TryParse(DsAppel.Tables["Appels"].Rows[0]["DateAccord"].ToString(), out dt);

                if (ok)
                {
                    bConsentement.Enabled = false;    //On a une date donc un consentement
                    DateAccord = DateTime.Parse(DsAppel.Tables["Appels"].Rows[0]["DateAccord"].ToString());
                }
                else
                {
                    bConsentement.Enabled = true;
                    DateAccord = null;
                }*/


                /*int Npers = int.Parse(DsAppel.Tables["Appels"].Rows[0]["Num_Personne"].ToString());

                if (Npers != -1)
                {
                    //La personne est connue
                    string NumTel = DsAppel.Tables["Appels"].Rows[0]["Tel_Appel"].ToString();
                    
                    //Recup de la Date de cette consult (c'est toujours la 1ere de SuiviAppel)                   
                    string DateJ = FonctionsAppels.convertDateMaria(DsAppel.Tables["SuiviAppel"].Rows[0]["DateOp"].ToString(), "Texte");

                    dt10DerAppels.Rows.Clear();
                    dt10DerAppels = FonctionsAppels.Derniers10Appels(NumTel, Npers, DateJ);

                    if (dt10DerAppels.Rows.Count > 0)
                    {                    
                        lDerAppel.Text = "Dernier appel le " + FonctionsAppels.convertDateMaria(dt10DerAppels.Rows[0]["DateOp"].ToString(), "Texte") +
                                                                    "\r\n=> motif: " + dt10DerAppels.Rows[0]["LibelleMotif"].ToString();
                    }
                    else    //Pas d'appels précédents pour cette personne
                    {
                        lDerAppel.Text = "Pas de d'appels précédents pour cette personne.";
                    }
                }
                else  //Elle n'est pas connu donc pas d'historique
                {
                    lDerAppel.Text = "Pas de d'appels précédents pour cette personne.";
                }*/
                    //*******Fin de recherche des précédentes visites                                                    

                    //Pour l'adresse de facturation...On regarde si on a un enregistrement
                if (DsAppel.Tables["adrfacturation"].Rows.Count > 0)
                {
                    //On active l'adresse de Facturation
                    cBoxAutreAdrFact.Checked = true;
                    tBoxNomFact.Text = DsAppel.Tables["adrfacturation"].Rows[0]["Nom"].ToString();
                    tBoxPrenomFact.Text = DsAppel.Tables["adrfacturation"].Rows[0]["Prenom"].ToString();

                    tBoxCPFact.Text = DsAppel.Tables["adrfacturation"].Rows[0]["CodePostal"].ToString();
                    tBoxCommuneFact.Text = DsAppel.Tables["adrfacturation"].Rows[0]["Commune"].ToString();
                    if (DsAppel.Tables["adrfacturation"].Rows[0]["Pays"].ToString() == "")
                        cBoxPaysFact.Text = "Suisse";
                    else
                        cBoxPaysFact.Text = DsAppel.Tables["adrfacturation"].Rows[0]["Pays"].ToString();

                    tBoxNumRueFact.Text = DsAppel.Tables["adrfacturation"].Rows[0]["Num_Rue"].ToString();
                    tBoxRueFact.Text = DsAppel.Tables["adrfacturation"].Rows[0]["Adr1"].ToString();
                    tBoxRue2Fact.Text = DsAppel.Tables["adrfacturation"].Rows[0]["Adr2"].ToString();
                }    //Fin d'on a une adresse de facturation

                //Est un TA? si oui va chercher les infos dans la base SmartRapport
                if (DsAppel.Tables["Appels"].Rows[0]["TA"].ToString() == "True")
                {
                    PeuplerChampsTA(int.Parse(DsAppel.Tables["Appels"].Rows[0]["Num_Personne"].ToString()));

                    //On ne peut pas modifier l'adresse d'un TA
                    beffaceAdresse.Visible = false;
                }

                //A t'il des remarques? si oui va chercher les infos dans la base SmartRapport
                if (DsAppel.Tables["Appels"].Rows[0]["Remarquable"].ToString() == "True")
                {
                    PeuplerChampsRemarque(int.Parse(DsAppel.Tables["Appels"].Rows[0]["Num_Personne"].ToString()));
                }

                //Pour les factures impayées
                facturesImpayes(NumPersonne);

                ChargerMedecinTraitant(NumPersonne);

                try
                {
                    //Pour le Suivi de l'appel
                    for (int i = 0; i < DsAppel.Tables["SuiviAppel"].Rows.Count; i++)
                    {
                        switch (DsAppel.Tables["SuiviAppel"].Rows[i]["Type_Operation"].ToString())
                        {
                            case "Création": ListViewItem item0 = new ListViewItem("Appel créé le " + DsAppel.Tables["SuiviAppel"].Rows[i]["DateOp"].ToString());
                                ListViewItem item1 = new ListViewItem("Par " + FonctionsAppels.NomUtilisateur(DsAppel.Tables["SuiviAppel"].Rows[i]["IdUtilisateur"].ToString()));
                                listView1.Items.AddRange(new ListViewItem[] { item0, item1 }); break;

                            case "Attribution": ListViewItem item3 = new ListViewItem("Attribué le " + DsAppel.Tables["SuiviAppel"].Rows[i]["DateOp"].ToString());
                                ListViewItem item4 = new ListViewItem("Par " + FonctionsAppels.NomUtilisateur(DsAppel.Tables["SuiviAppel"].Rows[i]["IdUtilisateur"].ToString()));
                                ListViewItem item5 = new ListViewItem("Au médecin " + FonctionsAppels.NomMedecin(DsAppel.Tables["SuiviAppel"].Rows[i]["CodeMedecin"].ToString()));
                                listView1.Items.AddRange(new ListViewItem[] { item3, item4, item5 }); break;

                            case "Pré-attribution":
                                ListViewItem item6 = new ListViewItem("Pré-Attribué le " + DsAppel.Tables["SuiviAppel"].Rows[i]["DateOp"].ToString());
                                ListViewItem item7 = new ListViewItem("Par " + FonctionsAppels.NomUtilisateur(DsAppel.Tables["SuiviAppel"].Rows[i]["IdUtilisateur"].ToString()));
                                ListViewItem item8 = new ListViewItem("Au médecin " + FonctionsAppels.NomMedecin(DsAppel.Tables["SuiviAppel"].Rows[i]["CodeMedecin"].ToString()));
                                listView1.Items.AddRange(new ListViewItem[] { item6, item7, item8 }); break;

                            case "Reprise": ListViewItem item9 = new ListViewItem("Repris le " + DsAppel.Tables["SuiviAppel"].Rows[i]["DateOp"].ToString());
                                ListViewItem item10 = new ListViewItem("Par " + FonctionsAppels.NomUtilisateur(DsAppel.Tables["SuiviAppel"].Rows[i]["IdUtilisateur"].ToString()));
                                listView1.Items.AddRange(new ListViewItem[] { item9, item10 }); break;

                            case "Annulation": ListViewItem item11 = new ListViewItem("Annulé le " + DsAppel.Tables["SuiviAppel"].Rows[i]["DateOp"].ToString());
                                ListViewItem item12 = new ListViewItem("Par " + FonctionsAppels.NomUtilisateur(DsAppel.Tables["SuiviAppel"].Rows[i]["IdUtilisateur"].ToString()));
                                ListViewItem item13 = new ListViewItem("Au motif de " + FonctionsAppels.LibelleMotif(DsAppel.Tables["Appels"].Rows[i]["IdMotifAnnulation"].ToString()));
                                listView1.Items.AddRange(new ListViewItem[] { item11, item12, item13 }); break;

                            case "Acquitement": ListViewItem item14 = new ListViewItem("Acquité le " + DsAppel.Tables["SuiviAppel"].Rows[i]["DateOp"].ToString());
                                ListViewItem item15 = new ListViewItem("Par " + FonctionsAppels.NomMedecin(DsAppel.Tables["SuiviAppel"].Rows[i]["CodeMedecin"].ToString()));
                                listView1.Items.AddRange(new ListViewItem[] { item14, item15 }); break;

                            case "Début de visite": ListViewItem item16 = new ListViewItem("Entrée en visite le " + DsAppel.Tables["SuiviAppel"].Rows[i]["DateOp"].ToString());
                                ListViewItem item17 = new ListViewItem("Par " + FonctionsAppels.NomMedecin(DsAppel.Tables["SuiviAppel"].Rows[i]["CodeMedecin"].ToString()));
                                listView1.Items.AddRange(new ListViewItem[] { item16, item17 }); break;

                            case "Terminée": ListViewItem item18 = new ListViewItem("Terminée le " + DsAppel.Tables["SuiviAppel"].Rows[i]["DateOp"].ToString());
                                listView1.Items.AddRange(new ListViewItem[] { item18 }); break;
                        }
                    }   //Fin chargement SuiviAppel
                }
                catch (Exception)
                {

                }

            }   //Fin chargement de Table Appels            
        }

       
        //Après changement
        private void cBoxAnnulation_CheckStateChanged(object sender, EventArgs e)
        {                        
            if (cBoxAnnulation.Checked)
            {
                tBoxMotifAnnul.Enabled = true;
                bValider.Enabled = false;
               // bMailInf.Enabled = false;
            }
            else        
            {
                tBoxMotifAnnul.Text = "";
                tBoxMotifAnnul.Enabled = false;
               // bMailInf.Enabled = true;
                bValider.Enabled = true;
            }
        }

        //Affichage d'une boite de dialogue pour retourner le résultat de la recherche (personne)
        private int DialAfficheRecherche(DataTable dtRecherche)
        {
            //On affiche la liste des personnes retournées   
            FDialogRecherche fDialogRecherche = new FDialogRecherche(dtRecherche);

            // fDialogRecherche.NumVisite = int.Parse(listView2.SelectedItems[0].SubItems[8].Text);

            fDialogRecherche.ShowDialog(this);

            //string recup = fDialogRecherche.Selection;

            fDialogRecherche.Dispose();

            int recup = fDialogRecherche.Selection;

            return recup;
        }


        //On peuple les champs à partir de la recherche dans SmartRapport (Nouvel Appel)
        private void PeuplerChamps(int index, DataTable dtPersonne)
        {
            NumPersonne = int.Parse(dtPersonne.Rows[index]["IdPersonne"].ToString());        

            //On rempli les champs
            if (tBAppellant.Text == "")
            {
                tBAppellant.Text = dtPersonne.Rows[index]["Telephone"].ToString();
            }

            tBoxNom.Text = dtPersonne.Rows[index]["Nom"].ToString();
            tBoxPrenom.Text = dtPersonne.Rows[index]["Prenom"].ToString();
            mTBoxDateNaiss.Text = dtPersonne.Rows[index]["DateNaissance"].ToString();

            //Si c'est un patient connu, on verrouille les contrôles
            if (NumPersonne != -1)
            {
                tBoxNom.ReadOnly = true;
                tBoxPrenom.ReadOnly = true;
                mTBoxDateNaiss.ReadOnly = true;
            }
            else
            {
                tBoxNom.ReadOnly = false;
                tBoxPrenom.ReadOnly = false;
                mTBoxDateNaiss.ReadOnly = false;
            }

            //Calcul de l'âge
            DateTime DateNaiss;
            try
            {
                if (dtPersonne.Rows[index]["DateNaissance"] != DBNull.Value)
                {
                    DateNaiss = DateTime.Parse(dtPersonne.Rows[index]["DateNaissance"].ToString());
                    int Age = FonctionsAppels.CalculeAge(DateNaiss);
                    tBoxAge.Text = Age.ToString();
                }
                else
                {
                    tBoxAge.Text = "0";
                }
            }
            catch (Exception)
            {
                tBoxAge.Text = "";
            }


            beffaceAdresse.Visible = true;    //on peut effacer l'adresse de la fiche par défaut.

            tBoxCP.Text = dtPersonne.Rows[index]["CodePostal"].ToString();
            tBoxCommune.Text = dtPersonne.Rows[index]["Commune"].ToString();

            if (dtPersonne.Rows[index]["Pays"].ToString() == "")
                cBoxPays.Text = "Suisse";
            else
                cBoxPays.Text = dtPersonne.Rows[index]["Pays"].ToString();

            tBoxNumRue.Text = dtPersonne.Rows[index]["NumeroDansRue"].ToString();
            tBRue.Text = dtPersonne.Rows[index]["Rue"].ToString();
            tBRueCompl.Text = dtPersonne.Rows[index]["rue2"].ToString();
            tBDigicode.Text = dtPersonne.Rows[index]["Digicode"].ToString();

            //Interphone
            if (dtPersonne.Rows[index]["Internom"].ToString() != "" && dtPersonne.Rows[index]["Internom"] != DBNull.Value)
            {
                tBoxNomPorte.Text = dtPersonne.Rows[index]["Internom"].ToString();
                cBoxInterphone.Checked = true;
            }
            else
            {
                tBoxNomPorte.Text = "";
                cBoxInterphone.Checked = false;
            }

            tBoxEtage.Text = dtPersonne.Rows[index]["Etage"].ToString();

            if (dtPersonne.Rows[index]["Porte"].ToString().Length < 15)
                tBoxPorte.Text = dtPersonne.Rows[index]["Porte"].ToString();
            else 
                tBoxPorte.Text = dtPersonne.Rows[index]["Porte"].ToString().Substring(0,14);

            tBoxEmail.Text = dtPersonne.Rows[index]["Email"].ToString();

            //Pour le sexe
            switch (dtPersonne.Rows[index]["Sexe"].ToString())
            {
                case "H": rBMal.Checked = true; rBFemelle.Checked = false; break;
                case "F": rBMal.Checked = false; rBFemelle.Checked = true; break;
                default: rBMal.Checked = false; rBFemelle.Checked = true; break;
            }

            //****Précédentes visites par rapport à MAINTENANT****                   
            string Nom = dtPersonne.Rows[index]["Nom"].ToString();
            string Prenom = dtPersonne.Rows[index]["Prenom"].ToString();
            string DateNaissance = dtPersonne.Rows[index]["DateNaissance"].ToString();

            dt20DerAppels = FonctionsAppels.Derniers20Appels(Nom, Prenom, DateNaissance);

            if (dt20DerAppels.Rows.Count > 0)
            {
                lDerAppel.Text = "Dernier appel le " + FonctionsAppels.convertDateMaria(dt20DerAppels.Rows[0]["DateOp"].ToString(), "Texte") +
                                                                               "\r\n=> motif: " + dt20DerAppels.Rows[0]["LibelleMotif"].ToString();
            }
            else    //Pas d'appels précédents pour cette personne
            {
                lDerAppel.Text = "Pas d'appels précédents pour cette personne.";
            }

            /*int Npers = NumPersonne;

            if (Npers != -1)
            {
                //La personne est connue
                string NumTel = tBAppellant.Text;

                //Recup de la Date de Maintenant              
                string DateJ = DateTime.Now.ToString();

                dt10DerAppels.Rows.Clear();
                dt10DerAppels = FonctionsAppels.Derniers10Appels(NumTel, Npers, DateJ);

                if (dt10DerAppels.Rows.Count > 0)
                {
                    lDerAppel.Text = "Dernier appel le " + FonctionsAppels.convertDateMaria(dt10DerAppels.Rows[0]["DateOp"].ToString(), "Texte") +
                                                                " => motif: " + dt10DerAppels.Rows[0]["LibelleMotif"].ToString();
                }
                else    //Pas d'appels précédents pour cette personne
                {
                    lDerAppel.Text = "Pas de d'appels précédents pour cette personne.";
                }
            }
            else  //Elle n'est pas connu donc pas d'historique
            {
                lDerAppel.Text = "Pas de d'appels précédents pour cette personne.";
            }
            */
            //*******Fin de recherche des précédentes visites                                                                 

            //Pour l'adresse de facturation...Si la rue est différente, on coche Autre et on renseigne les champs
            if (dtPersonne.Rows[index]["Rue"].ToString() != dtPersonne.Rows[index]["RueFact"].ToString()
                && dtPersonne.Rows[index]["RueFact"].ToString().Replace(" ", "") != "")
            {
                //On active l'adresse de Facturation
                cBoxAutreAdrFact.Checked = true;
                tBoxNomFact.Text = dtPersonne.Rows[index]["NomFact"].ToString();
                tBoxPrenomFact.Text = dtPersonne.Rows[index]["PrenomFact"].ToString();

                tBoxCPFact.Text = dtPersonne.Rows[index]["CodePostalFact"].ToString();
                tBoxCommuneFact.Text = dtPersonne.Rows[index]["CommuneFact"].ToString();
                if (dtPersonne.Rows[index]["PaysFact"].ToString() == "")
                    cBoxPaysFact.Text = "Suisse";
                else
                    cBoxPaysFact.Text = dtPersonne.Rows[index]["PaysFact"].ToString();

                tBoxNumRueFact.Text = dtPersonne.Rows[index]["NumeroDansRueFact"].ToString();
                tBoxRueFact.Text = dtPersonne.Rows[index]["RueFact"].ToString();
                tBoxRue2Fact.Text = dtPersonne.Rows[index]["Rue2Fact"].ToString();
            }
           
            //Si c'est un TA, on rempli également les infos TA
            if (dtPersonne.Rows[index]["TeleAlarme"].ToString() == "O")
            {
               PeuplerChampsTA(NumPersonne);

               //On ne peut pas modifier l'adresse d'un TA
               beffaceAdresse.Visible = false;
            }

            //Si c'est un patient remarquable, on rempli les remarques
            if (dtPersonne.Rows[index]["PatientRemarquable"].ToString() == "O")
            {
                PeuplerChampsRemarque(NumPersonne);
            }

            //Pour le n° de carte d'assurance, on bloque la saisie si un n° existe déjà
            if (dtPersonne.Rows[index]["Num_Carte"].ToString().Length > 0 && dtPersonne.Rows[index]["Num_Carte"].ToString().Substring(0, 1) == "8")
            {
                tBoxNumCarte.Text = dtPersonne.Rows[index]["Num_Carte"].ToString();
               // tBoxNumCarte.ReadOnly = true;
            }
            else
            {
                tBoxNumCarte.Text = "Inconnu";
                //tBoxNumCarte.ReadOnly = false;
            }

            //Pour l'IdUnilab s'il y a rien on en donne un sinon on le récupère
            if(dtPersonne.Rows[index]["IdUnilab"] == DBNull.Value || dtPersonne.Rows[index]["IdUnilab"].ToString() == "")                                           
                IdUnilab = FonctionsAppels.ToUnixTime(DateTime.Now).ToString();  //On donne un nouvel IdUnilab     
            else    
                IdUnilab = dtPersonne.Rows[index]["IdUnilab"].ToString();

            //Pour les factures impayées
            facturesImpayes(NumPersonne);

            ChargerMedecinTraitant(NumPersonne);

            //Pour l'accord LPD on le recherche dans les précédents appels  => Désactivé sur demande du patron           
            //Vérif de la date de naissance
           /* DateTime dt;
            var ok = DateTime.TryParse(DateNaissance, out dt);

            if (ok)
            {
                DateAccord = FonctionsAppels.RechercheAccord(dtPersonne.Rows[index]["Nom"].ToString(),
                                                             dtPersonne.Rows[index]["Prenom"].ToString(),
                                                             dtPersonne.Rows[index]["DateNaissance"].ToString());

                //si on a trouvé une date
                if (DateAccord != null)
                {
                    bConsentement.Enabled = false;  //C'est ok, plus besoin du bouton                   
                }
                else
                {
                    bConsentement.Enabled= true;
                }
                
            }
            else
            {
                //DateNaissance non correcte on active le bouton
                bConsentement.Enabled = true;
            }   */        
        }


        //on peuple les champs TA
        private void PeuplerChampsTA(int NumPersonne)
        {           
            DataSet dsTA = FonctionsAppels.GetInfoTA(NumPersonne);

            if (dsTA.Tables["Abonnement"].Rows.Count > 0)
            {
                //On active l'onglet TA et on redéfini les colonnes des listview               
                TA = true;

                tabControl1.TabPages["tabPageTA"].Text = "TA/Médicalerte";
               
                //ainsi que le Label TA
                labelTA.Text = "TA";                

                listViewTa1.Columns.Clear();
                listViewTa1.Columns.Add("Nom", 200);
                listViewTa1.Columns.Add("Téléphone", 100);
                listViewTa1.View = View.Details;    //Pour afficher les subItems  

                listViewTa2.Columns.Clear();
                listViewTa2.Columns.Add("N° Fact.", 70);
                listViewTa2.Columns.Add("Date", 80);
                listViewTa2.Columns.Add("Montant", 70);
                listViewTa2.Columns.Add("Début période", 80);
                listViewTa2.Columns.Add("Fin période", 80);
                listViewTa2.Columns.Add("Payé", 80);
                listViewTa2.View = View.Details;    //Pour afficher les subItems  

                listViewTa3.Columns.Clear();
                listViewTa3.Columns.Add("Opération", 80);
                listViewTa3.Columns.Add("Envoie de", 130);
                listViewTa3.Columns.Add("Commentaire1", 300);
                listViewTa3.Columns.Add("Date Opé", 80);
                listViewTa3.Columns.Add("Type Clé", 50);
                listViewTa3.Columns.Add("Commentaire2", 300);
                listViewTa3.View = View.Details;    //Pour afficher les subItems                    

                //Puis on rempli les différentes pages
                //Abonnement
                label5.Text = "Id Abonnement : " + dsTA.Tables["Abonnement"].Rows[0]["IDAbonnement"].ToString();
                label6.Text = "Id Contrat : " + dsTA.Tables["Abonnement"].Rows[0]["IDContrat"].ToString();
                label7.Text = "Numero de cle: " + dsTA.Tables["Abonnement"].Rows[0]["NumeroCle"].ToString();
                //clé
                label9.Text = "Clé à la centrale: " + dsTA.Tables["Cle"].Rows[0]["Cle_presente"].ToString();
                label8.Text = "Commentaire: " + dsTA.Tables["Cle"].Rows[0]["Commentaire"].ToString();
                //Pour les infos de l'onglet Dossiers Médicaux
                label10.Text = "Problèmes médicaux : \r\nRisque de Chute: " + dsTA.Tables["DosMed"].Rows[0]["RisqueChute"].ToString() + "\r\n" +
                                dsTA.Tables["DosMed"].Rows[0]["TD_PbMedicaux"].ToString();
                label11.Text = "Traitement : " + dsTA.Tables["DosMed"].Rows[0]["TD_Traitements"].ToString();


                //liste Contacts                 
                listViewTa1.BeginUpdate();
                listViewTa1.Items.Clear();

                for (int i = 0; i < dsTA.Tables["Contacts"].Rows.Count; i++)
                {
                    ListViewItem item = new ListViewItem(dsTA.Tables["Contacts"].Rows[i]["Nom"].ToString() + " " + dsTA.Tables["Contacts"].Rows[i]["Prenom"].ToString());
                    item.SubItems.Add(dsTA.Tables["Contacts"].Rows[i]["Telephone"].ToString());
                    listViewTa1.Items.Add(item);
                }

                listViewTa1.EndUpdate();  //Rafraichi le controle     

                //liste Factures              
                listViewTa2.BeginUpdate();
                listViewTa2.Items.Clear();

                for (int i = 0; i < dsTA.Tables["Facture"].Rows.Count; i++)
                {
                    ListViewItem item = new ListViewItem(dsTA.Tables["Facture"].Rows[i]["NFacture"].ToString());
                    item.SubItems.Add(dsTA.Tables["Facture"].Rows[i]["Date_facture"].ToString());
                    item.SubItems.Add(dsTA.Tables["Facture"].Rows[i]["Montant"].ToString());
                    item.SubItems.Add(dsTA.Tables["Facture"].Rows[i]["Début_periode"].ToString());
                    item.SubItems.Add(dsTA.Tables["Facture"].Rows[i]["Fin_période"].ToString());
                    item.SubItems.Add(dsTA.Tables["Facture"].Rows[i]["Payé"].ToString());
                    listViewTa2.Items.Add(item);
                }

                listViewTa2.EndUpdate();  //Rafraichi le controle    

                //liste Journal              
                listViewTa3.BeginUpdate();
                listViewTa3.Items.Clear();

                for (int i = 0; i < dsTA.Tables["Journal"].Rows.Count; i++)
                {
                    ListViewItem item = new ListViewItem(dsTA.Tables["Journal"].Rows[i]["TypeOp"].ToString());
                    item.SubItems.Add(dsTA.Tables["Journal"].Rows[i]["EnvoiDe"].ToString());
                    item.SubItems.Add(dsTA.Tables["Journal"].Rows[i]["TexteA"].ToString());
                    item.SubItems.Add(dsTA.Tables["Journal"].Rows[i]["DateOp"].ToString());
                    item.SubItems.Add(dsTA.Tables["Journal"].Rows[i]["NbCle"].ToString());
                    item.SubItems.Add(dsTA.Tables["Journal"].Rows[i]["Commentaire"].ToString());
                    listViewTa3.Items.Add(item);
                }

                listViewTa3.EndUpdate();  //Rafraichi le controle    
            }
            else
                TA = false;
        }

        //On peuple les champs Remarque
        private void PeuplerChampsRemarque(int NumPersonne)
        {           
            DataTable dtRemarquable = FonctionsAppels.GetPatientRemarquable(NumPersonne);

            if (dtRemarquable.Rows.Count > 0)
            {
                //On active l'onglet Remarques et on redéfini les colonnes des listview                               
                tabControl1.TabPages["tabPageRemarque"].Text = "Remarques";

                Remarquable = true;

                //ainsi que le label Remarque
                labelRemarque.Text = "R";

                //Puis on rempli les champs
                if (dtRemarquable.Rows[0]["Encaisse1"].ToString() == "Oui")        //Si c'est un mauvais payeur
                {
                    label51.Text = "Patient litigieux";
                    label51.ForeColor = Color.Tomato;
                    labelRemarque.ForeColor = Color.Tomato;
                }
                else
                {
                    label51.Text = "Patient Ok";
                    label51.ForeColor = Color.LightGreen;
                    labelRemarque.ForeColor = Color.Yellow;
                }
               
                label53.Text = "Faire signer cession de créance : " + dtRemarquable.Rows[0]["Cession1"].ToString();
                label54.Text = "Remarques économiques : \r\n" + dtRemarquable.Rows[0]["Economique"].ToString();

                //S'il y a des remarques médicales, le signaler sur la fiche
                label55.Text = "Remarques médicales : \r\n" + dtRemarquable.Rows[0]["Medical"].ToString();

                if (dtRemarquable.Rows[0]["Medical"].ToString().Trim(' ').Length > 0)
                    lRemMedicales.Text = "M";
                else
                    lRemMedicales.Text = "";
            }
            else
                Remarquable = false;
        }

        //On verifie s'il y a des factures impayés
        private void facturesImpayes(int NumPersonne)
        {
            DataTable dtFacture = FonctionsAppels.GetFacturesImpayees(NumPersonne);

            if (dtFacture.Rows.Count > 0)
            {
                FactureImpayee = true;

                //ainsi que le label Factures Impayées
                labelFactureImp.Text = "$";
            }
            else
            {
                FactureImpayee = false;
                labelFactureImp.Text = "";
            }
        }

        //Quand on click sur le label factureImpayé
        private void labelFactureImp_Click(object sender, EventArgs e)
        {
            if (FactureImpayee == true)
            {
                //On affiche une boite de dialogue avec la liste des factures impayées
                FFacturesImpayees fFacturesImpayees = new FFacturesImpayees(NumPersonne);
                fFacturesImpayees.ShowDialog(this);
                fFacturesImpayees.Dispose();
            }
        }

        private void bRechNomPre_Click(object sender, EventArgs e)
        {
            //On recherche dans SmartRapport le Nom, prénom et on retourne la liste des personnes correspondantes
            if (tBoxNom.Text != "" && tBoxPrenom.Text != "")
            {
                DataTable dtPersonne = new DataTable();
                dtPersonne = FonctionsAppels.RecherchePersonne("NomPrenom", null, tBoxNom.Text.Trim(), tBoxPrenom.Text.Trim(), null);

                int index = DialAfficheRecherche(dtPersonne);

                //On peuple les champs en fonction de l'index retourné
                if (index != -1)
                {
                    PeuplerChamps(index, dtPersonne);
                }
            }
            else
            {
                MessageBox.Show("Veuillez renseigner au moins un début de nom ET Prénom.", "Recherche par Nom-Prenom", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void bRechAge_Click(object sender, EventArgs e)
        {
            //On recherche dans SmartRapport la date de naissance et on retourne la liste des personnes correspondantes
            if (mTBoxDateNaiss.Text != "  .  .")
            {
                //On verrifie la date
                DateTime Testdate;

                if (DateTime.TryParse(mTBoxDateNaiss.Text, out Testdate))
                {
                    //Si elle est valide....
                    DataTable dtPersonne = new DataTable();
                    dtPersonne = FonctionsAppels.RecherchePersonne("DateNaiss", null, null, null, mTBoxDateNaiss.Text);

                    int index = DialAfficheRecherche(dtPersonne);

                    //On peuple les champs en fonction de l'index retourné
                    if (index != -1)
                    {
                        PeuplerChamps(index, dtPersonne);
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez mettre une date de naissance valide.", "Recherche par date de naissance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //***********************Affichage des 10 dernières visites************************
        private void b10visites_Click(object sender, EventArgs e)
        {            
            //Si on a des enregistrements précédents, on les affiche
            if (dt20DerAppels.Rows.Count > 0)
            {
                FRechDerniersAppels fRechDerniersAppels = new FRechDerniersAppels(dt20DerAppels);               
                fRechDerniersAppels.ShowDialog(this);
                fRechDerniersAppels.Dispose();
            }         
        }


        //***************RECHERCHE DE LA PROVENANCE****************************
        private void tBoxProvenance_KeyDown(object sender, KeyEventArgs e)
        {
            toucheF5 = false;

            if (e.KeyCode == Keys.F5)
            {
                //On affiche la boite des provenances
                FRechProv fRechProv = new FRechProv();

                //On récupères les valeurs
                if (fRechProv.ShowDialog() == DialogResult.OK)
                    tBoxProvenance.Text = fRechProv.libelle;
                else
                    tBoxProvenance.Text = "";

                fRechProv.Dispose();

                toucheF5 = true;
            }            
        }

        private void tBoxProvenance_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (toucheF5 == false)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }

        private void tBoxMedTraitant_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                FRechMedecinTraitant fRechMedecinTraitant = new FRechMedecinTraitant();

                if (fRechMedecinTraitant.ShowDialog() == DialogResult.OK)
                {
                    IdMedecinTraitant = fRechMedecinTraitant.idMedecin;
                    NomMedecinTraitant = fRechMedecinTraitant.nomMedecin;
                    PrenomMedecinTraitant = fRechMedecinTraitant.prenomMedecin;

                    MajMedecinTraitantTexte = true;
                    tBoxMedTraitant.Text = (NomMedecinTraitant + " " + PrenomMedecinTraitant).Trim();
                    MajMedecinTraitantTexte = false;
                }

                fRechMedecinTraitant.Dispose();
            }
            else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                MajMedecinTraitantTexte = true;
                tBoxMedTraitant.Text = "";
                MajMedecinTraitantTexte = false;

                IdMedecinTraitant = -1;
                NomMedecinTraitant = "";
                PrenomMedecinTraitant = "";
            }
        }

        private void tBoxMedTraitant_TextChanged(object sender, EventArgs e)
        {
            if (MajMedecinTraitantTexte)
            {
                return;
            }

            if (tBoxMedTraitant.Text == "")
            {
                IdMedecinTraitant = -1;
                NomMedecinTraitant = "";
                PrenomMedecinTraitant = "";
            }
        }

        private void tBoxMedTraitant_Enter(object sender, EventArgs e)
        {
            tBoxMedTraitant.BackColor = Color.DarkGreen;
        }

        private void tBoxMedTraitant_Leave(object sender, EventArgs e)
        {
            tBoxMedTraitant.BackColor = SystemColors.ControlDark;
        }


        //**************RECHERCHE A PARTIR DU N° D'ASSURE
        private void bInfoAssurance_Click(object sender, EventArgs e)
        {

            //On recherche les infos de ce patient Si on a bien les 20 caractere du n° de la carte ET que ça commence par 80
            if (tBoxNumCarte.Text.Length == 20 && tBoxNumCarte.Text.StartsWith("80"))
            {
                //On met le curseur d'attente
                Cursor.Current = Cursors.WaitCursor;

                //On initialise les variables de retour
                NumAssure = "";
                NomAssure = "";
                PrenomAssure = "";
                DateNaissanceAssure = "";
                GenreAssure = "";

                for (int i = 0; i < 5; i++)
                {
                    AdresseDecompose[i] = "";
                }

                //On affiche la forme de la réponse et on passe en paramètre le n° de carte
                FInfoAssure fInfoAssure = new FInfoAssure(tBoxNumCarte.Text);
                fInfoAssure.ShowDialog();

                //On remet le curseur par defaut
                Cursor.Current = Cursors.Default;

                //Si on a exporté les données de retour
                if (NomAssure != "")
                {
                    //On affecte les nouvelles valeurs
                    tBoxNom.Text = NomAssure;
                    tBoxPrenom.Text = PrenomAssure;
                    mTBoxDateNaiss.Text = DateNaissanceAssure;

                    //On recalcule l'age
                    DateTime test;

                    if (DateTime.TryParse(mTBoxDateNaiss.Text, out test))
                    {
                        int Age = FonctionsAppels.CalculeAge(DateTime.Parse(mTBoxDateNaiss.Text));
                        tBoxAge.Text = Age.ToString();

                        //On regarde qu'il n'ont pas rentré n'importe quoi
                        if (Age < 0 || Age > 140)
                        {
                            MessageBox.Show("Le patient a un âge incohérent: " + Age + " ans!\r\n" +
                                         "Voulez-vous vraiment prendre cette date de naissance?", "Attention Date de Naissance Incohérente!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }


                    //  tBoxNunAssure.Text = NumAssure;

                    if (GenreAssure == "F")
                        rBFemelle.Checked = true;
                    else rBMal.Checked = true;

                    if (AdresseDecompose[0] != "")
                    {
                        tBRue.Text = AdresseDecompose[0];
                        tBoxNumRue.Text = AdresseDecompose[1];
                        tBRueCompl.Text = "";
                        tBoxCommune.Text = AdresseDecompose[2];
                        tBoxCP.Text = AdresseDecompose[3];
                        cBoxPays.Text = AdresseDecompose[4];
                    }
                }

                fInfoAssure.Dispose();
            }
        }



        //********************RECHERCHE DE LA COMMUNE*****************************
        private void tBoxCommune_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //On affiche la boite des communes
                FRechCommune fRechCommune = new FRechCommune();


                //On récupères les valeurs
                if (fRechCommune.ShowDialog() == DialogResult.OK)
                {
                    tBoxCommune.Text = fRechCommune.nomCommune;
                    tBoxCP.Text = fRechCommune.codePostal;
                    cBoxPays.Text = fRechCommune.pays;
                }
                else
                {
                    tBoxProvenance.Text = "";
                    tBoxCP.Text = "";
                    cBoxPays.Text = "";
                }

                fRechCommune.Dispose();
            }
        }


        //********************RECHERCHE DE LA COMMUNE FACTURATION*****************************
        private void tBoxCommuneFact_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //On affiche la boite des communes
                FRechCommune fRechCommune = new FRechCommune();               

                //On récupères les valeurs
                if (fRechCommune.ShowDialog() == DialogResult.OK)
                {
                    tBoxCommuneFact.Text = fRechCommune.nomCommune;
                    tBoxCPFact.Text = fRechCommune.codePostal;
                    cBoxPaysFact.Text = fRechCommune.pays;
                }
                else
                {
                    tBoxCommuneFact.Text = "";
                    tBoxCPFact.Text = "";
                    cBoxPaysFact.Text = "";
                }

                fRechCommune.Dispose();
            }
        }


        //********************RECHERCHE DE LA RUE*****************************
        private void tBRue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //On affiche la boite des rues
                FRechRue fRechRue = new FRechRue();

                if (tBoxCommune.Text != "")
                    fRechRue.communesaisie = tBoxCommune.Text;
                     
                //On récupères les valeurs
                if (fRechRue.ShowDialog() == DialogResult.OK)
                {
                    tBRue.Text = fRechRue.nomRue;
                    tBoxCP.Text = fRechRue.codepostal;
                    tBoxCommune.Text = fRechRue.commune;
                    cBoxPays.Text = fRechRue.pays;
                }
                else
                {
                    tBRue.Text = "";                  
                }

                fRechRue.Dispose();
            }

            //Pour le copier/coller
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBRue.Copy();

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
                tBRue.Paste();
        }


        //********************RECHERCHE DE LA RUE FACTURATION*****************************
        private void tBoxRueFact_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //On affiche la boite des rues
                FRechRue fRechRue = new FRechRue();

                if (tBoxCommuneFact.Text != "")
                    fRechRue.communesaisie = tBoxCommuneFact.Text;

                //On récupères les valeurs
                if (fRechRue.ShowDialog() == DialogResult.OK)
                {
                    tBoxRueFact.Text = fRechRue.nomRue;
                    tBoxCPFact.Text = fRechRue.codepostal;
                    tBoxCommuneFact.Text = fRechRue.commune;
                    cBoxPaysFact.Text = fRechRue.pays;
                }
                else
                {
                    tBoxRueFact.Text = "";
                }

                fRechRue.Dispose();
            }
        }


        //********************RECHERCHE DE L'ASSURANCE *****************************

        private void tBoxAssuranceNom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //On affiche la boite des rues
                FRechAssurance fRechAssurance = new FRechAssurance();
               
                //On récupères les valeurs
                if (fRechAssurance.ShowDialog() == DialogResult.OK)
                {
                    tBoxAssuranceNom.Text = fRechAssurance.nomAssurance;                   
                }
                else
                {
                    tBoxAssuranceNom.Text = "";
                }

                fRechAssurance.Dispose();
            }

            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBoxAssuranceNom.Copy();

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
                tBoxAssuranceNom.Paste();
        }
        

        //********************RECHERCHE DU MOTIF1*****************************
        private void tBoxMotif1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //On affiche la boite des motifs
                FRechMotif fRechMotif = new FRechMotif();

                fRechMotif.motif = "Motif1";

                //On récupères les valeurs
                if (fRechMotif.ShowDialog() == DialogResult.OK)
                {
                    tBoxMotif1.Text = fRechMotif.libelleMotif;
                    IdMotif1 = fRechMotif.idMotif;
                }
                else
                {
                    tBoxMotif1.Text = "";
                }

                fRechMotif.Dispose();
            }
        }


        private void tBoxMotif1_Leave(object sender, EventArgs e)
        {
            //Vérif de la saisie
            if (VerifSaisie() == "OK")
            {
                bValider.Enabled = true;
              //  bMailInf.Enabled = true;
            }
            else
            {
                bValider.Enabled = false;
             //   bMailInf.Enabled = false;
            }
        }


        //********************RECHERCHE DU MOTIF2*****************************
        private void tBoxMotif2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //On affiche la boite des motifs
                FRechMotif fRechMotif = new FRechMotif();

                fRechMotif.motif = "Motif2";

                //On récupères les valeurs
                if (fRechMotif.ShowDialog() == DialogResult.OK)
                {
                    tBoxMotif2.Text = fRechMotif.libelleMotif;
                    IdMotif2 = fRechMotif.idMotif;
                }
                else
                {
                    tBoxMotif2.Text = "";
                }

                fRechMotif.Dispose();
            }
        }


        private void tBoxMotif2_Leave(object sender, EventArgs e)
        {
            //Vérif de la saisie
            if (VerifSaisie() == "OK")
            {
                bValider.Enabled = true;
              //  bMailInf.Enabled = true;
            }
            else
            {
                bValider.Enabled = false;
               // bMailInf.Enabled = false;
            }
        }
            
        
        //********************RECHERCHE DU MOTIF ANNULATION*****************************
        private void tBoxMotifAnnul_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //On affiche la boite des motifs
                FRechMotif fRechMotif = new FRechMotif();

                fRechMotif.motif = "MotifAnnulation";

                //On récupères les valeurs
                if (fRechMotif.ShowDialog() == DialogResult.OK)
                {
                    tBoxMotifAnnul.Text = fRechMotif.libelleMotif;
                    IdMotifAnnul = fRechMotif.idMotif;
                }
                else
                {
                    tBoxMotifAnnul.Text = "";
                }

                fRechMotif.Dispose();
            }
        }

        private void tBoxMotifAnnul_Leave(object sender, EventArgs e)
        {
            //Vérif de la saisie
            if (VerifSaisie() == "OK")
            {
                bValider.Enabled = true;
              //  bMailInf.Enabled = true;
            }
            else
            {
                bValider.Enabled = false;
               // bMailInf.Enabled = false;
            }
        }
              

        private void cBoxAutreAdrFact_CheckedChanged(object sender, EventArgs e)
        {
            if (cBoxAutreAdrFact.Checked)
            {
                //On active les contrôles
                tBoxNomFact.Enabled = true;
                tBoxPrenomFact.Enabled = true;
                tBoxCPFact.Enabled = true;
                tBoxCommuneFact.Enabled = true;
                cBoxPaysFact.Enabled = true;
                tBoxNumRueFact.Enabled = true;
                tBoxRueFact.Enabled = true;
                tBoxRue2Fact.Enabled = true;
            }
            else
            {
                //On désactive les contrôles
                tBoxNomFact.Enabled = false;
                tBoxPrenomFact.Enabled = false;
                tBoxCPFact.Enabled = false;
                tBoxCommuneFact.Enabled = false;
                cBoxPaysFact.Enabled = false;
                tBoxNumRueFact.Enabled = false;
                tBoxRueFact.Enabled = false;
                tBoxRue2Fact.Enabled = false;
            }
        }


        //**************Pour la validation de la fiche       
        private Int32 CreationAppel(Int32 NPers)
        {
            Int32 NumAppel = -1;

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            //On déclare ici les requetes (à cause de la transaction)
            string SqlStr0 = "";
            string SqlStr1 = "";
            string SqlStr2 = "";
            string SqlStr3 = "";
            string SqlStr4 = "";
            string SqlStr5 = "";

            try
            {
                //Récup du plus grand n°
                string sqlstr = "SELECT Max(Num_Appel) FROM appels";
                cmd.CommandText = sqlstr;

                DataTable dtAppel = new DataTable();
                dtAppel.Load(cmd.ExecuteReader());    //on execute

                if (dtAppel.Rows[0][0].ToString() != "" && dtAppel.Rows[0][0] != DBNull.Value)
                    NumAppel = Int32.Parse(dtAppel.Rows[0][0].ToString()) + 1;
                else NumAppel = 3000000;

                //Puis dans une transaction, on créé les enreg pour Appels, SuiviAppel, AdrFacturation(si necessaire), 
                //AppelsEnCours, StatusVisite.

                MySqlTransaction trans;   //Déclaration d'une transaction

                //Def des requettes
                //*****Appels***
                SqlStr0 = "INSERT INTO appels ";
                SqlStr0 += " (Num_Appel, Tel_Appel, Tel_Patient, Num_Personne, Nom, Prenom, Num_Rue, Adr1, Adr2, CodePostal, Commune, Pays, Email, Digicode, InterphoneNom, ";
                SqlStr0 += " Etage, Porte, Sexe, DateNaissance, Provenance, IdMotif1, IdMotif2, Commentaire, Urgence, CodeMedecin, Termine, IdMotifAnnulation, TA, Remarquable, ";
                SqlStr0 += " CondParticuliere, Assurance, NumCarte, Export, IdUnilab, DateAccord) ";
                SqlStr0 += " VALUES('" + NumAppel.ToString() + "','" + tBAppellant.Text + "','" + tBtelPatient.Text + "','" + NPers.ToString() + "','" + tBoxNom.Text.Replace("'", "''").Replace(",", ",,").Trim() + "','";
                SqlStr0 += tBoxPrenom.Text.Replace("'", "''").Replace(",", ",,").Trim() + "','" + tBoxNumRue.Text + "','" + tBRue.Text.Replace("'", "''").Replace(",", ",,");
                SqlStr0 += "','" + tBRueCompl.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBoxCP.Text + "','" + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,");
                SqlStr0 += "','" + cBoxPays.Text + " ','" + tBoxEmail.Text + "','" + tBDigicode.Text + "','" + tBoxNomPorte.Text.Replace("'", "''").Replace(",",",,");
                SqlStr0 += "','" + tBoxEtage.Text + "','" + tBoxPorte.Text.Replace("'", "''");

                if (rBMal.Checked)
                    SqlStr0 += "','H','";
                else SqlStr0 += "','F','";

                SqlStr0 += FonctionsAppels.convertDateMaria(mTBoxDateNaiss.Text + " 00:00:00", "MariaDb") + "','" + tBoxProvenance.Text + "','" + IdMotif1 + "','" + IdMotif2 + "',@Commentaire";
                SqlStr0 += ",'" + tBoxUrgence.Text + "', -1, false,'" + tBoxMotifAnnul.Text.Replace("'", "''").Replace(",", ",,");
                SqlStr0 += "'," + TA + "," + Remarquable + ",@CondParticuliere,'" + tBoxAssuranceNom.Text.Replace("'", "''").Replace(",", ",,");

                if (tBoxNumCarte.Text.Length > 0 && tBoxNumCarte.Text.TrimStart(' ').Substring(0, 1) == "8")
                    SqlStr0 += "','" + tBoxNumCarte.Text.Replace("'", "''").Replace(" ", "").Replace(".", "") + "'";
                else
                    SqlStr0 += "','Numeroinconnu'";

                SqlStr0 += ", 0, '" + IdUnilab + "'";

                if (DateAccord == null)
                    SqlStr0 += ", NULL)";
                else
                    SqlStr0 += ", '" + FonctionsAppels.convertDateMaria(DateAccord.ToString(), "MariaDb") + "')";


                //****SuiviAppel***
                SqlStr1 = "INSERT INTO suiviappel ";
                SqlStr1 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                SqlStr1 += " VALUES(" + NumAppel.ToString() + ",'Création', -1, '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";
                

                //****AdrFacturation***
                if (cBoxAutreAdrFact.Checked)
                {
                    SqlStr2 = "INSERT INTO adrfacturation ";
                    SqlStr2 += " (Num_Appel,Nom, Prenom, Num_Rue, Adr1, Adr2, CodePostal, Commune, Pays) VALUES(" + NumAppel.ToString() + ",'" + tBoxNomFact.Text.Replace("'", "''").Replace(",",",,");
                    SqlStr2 += "','" + tBoxPrenomFact.Text.Replace("'", "''").Replace(",",",,") + "','" + tBoxNumRueFact.Text + "','" + tBoxRueFact.Text.Replace("'", "''").Replace(",",",,") + "','" + tBoxRue2Fact.Text.Replace("'", "''").Replace(",",",,") + "','" + tBoxCPFact.Text;
                    SqlStr2 += "','" + tBoxCommuneFact.Text.Replace("'", "''").Replace(",",",,") + "','" + cBoxPaysFact.Text + "')";
                }

                //****AppelsEnCours
                //On géocode d'abord l'adresse
                string Adresse = tBRue.Text + ", " + tBoxNumRue.Text + ", " + tBoxCP.Text + ", " + tBoxCommune.Text + ", " + cBoxPays.Text;

                PointLatLng pt = GeocodeAdresse(Adresse);

                SqlStr3 = "INSERT INTO appelsencours ";
                SqlStr3 += " (Num_Appel, Tel_Appel, Tel_Patient, Num_Personne, Nom, Prenom, Num_Rue, Adr1, Adr2, CodePostal, Commune, Pays, Email, Digicode, InterphoneNom, ";
                SqlStr3 += " Etage, Porte, Sexe, DateNaissance, Provenance, Motif1, Motif2, Commentaire, Urgence, CodeMedecin, CondParticuliere, Assurance, Latitude, Longitude, PatientRappel) ";
                SqlStr3 += " VALUES(" + NumAppel.ToString() + ",'" + tBAppellant.Text + "','" + tBtelPatient.Text + "','" + NPers.ToString() + "','" + tBoxNom.Text.Replace("'", "''").Replace(",",",,").Trim() + "','" + tBoxPrenom.Text.Replace("'", "''").Replace(",",",,").Trim();
                SqlStr3 += " ','" + tBoxNumRue.Text + "','" + tBRue.Text.Replace("'", "''").Replace(",",",,") + "','" + tBRueCompl.Text.Replace("'", "''").Replace(",",",,") + "','" + tBoxCP.Text + "','" + tBoxCommune.Text.Replace("'", "''").Replace(",",",,") + "','" + cBoxPays.Text;
                SqlStr3 += " ','" + tBoxEmail.Text + "','" + tBDigicode.Text + "','" + tBoxNomPorte.Text.Replace("'", "''").Replace(",",",,") + "','" + tBoxEtage.Text + "','" + tBoxPorte.Text.Replace("'", "''");

                if (rBMal.Checked)
                    SqlStr3 += "','H','";
                else SqlStr3 += "','F','";

                SqlStr3 += FonctionsAppels.convertDateMaria(mTBoxDateNaiss.Text + " 00:00:00", "MariaDb") + "','" + tBoxProvenance.Text + "','" + tBoxMotif1.Text.Replace("'", "''").Trim() + "','" + tBoxMotif2.Text.Replace("'", "''").Trim() + "',@Commentaire,";
                SqlStr3 += "'" + tBoxUrgence.Text + "',-1,@CondParticuliere,'" + tBoxAssuranceNom.Text.Replace("'", "''").Replace(",", ",,") + "','" + pt.Lat + "','" + pt.Lng;

                if (cBoxPatientRappel.Checked)
                    SqlStr3 += "',1)";
                else SqlStr3 += "',0)";

                //****status_visite
                //Status: Non attribuée, Pré-attribuée, Attribuée, Acquitée, debut de visite (NA, PR, AT, AQ, V)

                SqlStr4 = "INSERT INTO status_visite ";
                SqlStr4 += " (Num_Appel,CodeMedecin, Status, DateStatus, Ordre) VALUES(" + NumAppel.ToString() + ",-1,'NA','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', 0)";

                //****status_visite_org
                //Status: Non attribuée, Pré-attribuée, Attribuée, Acquitée, debut de visite (NA, PR, AT, V)

                SqlStr5 = "INSERT INTO status_visite_org ";
                SqlStr5 += " (Num_Appel,CodeMedOrg, Status, DateStatus, Ordre) VALUES(" + NumAppel.ToString() + ",-1,'NA','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', 0)";


                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0;
                    cmd.Transaction = trans;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("Commentaire", tBCommentaireDiag.Text);  //Pour les apostrophes et les virgules
                    cmd.Parameters.AddWithValue("CondParticuliere", tBoxCondPart.Text);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //Si on a une autre adresse
                    if (cBoxAutreAdrFact.Checked)
                    {
                        cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    }

                    cmd.CommandText = SqlStr3;cmd.Transaction = trans;
                    
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("Commentaire", tBCommentaireDiag.Text);  //Pour les apostrophes et les virgules
                    cmd.Parameters.AddWithValue("CondParticuliere", tBoxCondPart.Text);  
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = SqlStr4; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr5; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                   
                    //on valide la transaction
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la création de la visite. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    NumAppel = -1;
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
                MessageBox.Show("Erreur lors de la création de la visite. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NumAppel = -1;
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return NumAppel;
        }


        //2 - Si on est en modif        
        //Si Annulé est coché
        //Mettre à jour les Tables Appels(Terminé), SuiviAppel        
        //Et nettoyer table EnGarde (si un medecin a cette visite, on le libère), AppelsEnCours, Status_Visite (delete tous ceux qui ont le num de cet appel)
        //Création nvl enreg dans TablePersonne et TablePatient, si necessaire, TableConsult, tableActes                      
        private void MajAppel()
        {
            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            //On déclare ici les requetes (à cause de la transaction)
            string SqlStr0 = "";
            string SqlStr1 = "";
            string SqlStr2 = "";
            string SqlStr3 = "";
            string SqlStr4 = "";
            string SqlStr5 = "";
            string SqlStr6 = "";
            string SqlStr7 = "";
            int appelsAnnule = 0;

            try
            {
                //Dans une transaction, on UPDATE les enreg pour Appels, SuiviAppel, AdrFacturation(si necessaire), 
                //AppelsEnCours, StatusVisite.

                MySqlTransaction trans;   //Déclaration d'une transaction

                if (cBoxAnnulation.Checked)   //L'appel est Annulé                                
                    appelsAnnule = 1;
                else
                    appelsAnnule = 0;

                //On récupère l'ID du médecin et de l'infrnière, s'il y en a un et l'etat de la visite pour le médecin ET l'Organisation
                int NumMed = -1;
                int NumInf = -1;
                NumMed = FonctionsAppels.MedecinVisite(NumVisite);
                NumInf = FonctionsAppels.OrganisationVisite(NumVisite);

                string EtatdeVisiteMed = FonctionsAppels.StatusVisite(NumVisite);
                string EtatdeVisiteOrg = FonctionsAppels.StatusVisiteOrg(NumVisite);

                //Def des requettes
                //*****Appels***
                SqlStr0 = "UPDATE appels ";
                SqlStr0 += " SET Tel_Appel = '" + tBAppellant.Text + "', Tel_Patient = '" + tBtelPatient.Text + "', Num_Personne = " + NumPersonne + ", Nom='" + tBoxNom.Text.Replace("'", "''").Replace(",",",,").Trim() + "', Prenom = '" + tBoxPrenom.Text.Replace("'", "''").Replace(",",",,").Trim() + "', Num_Rue = '" + tBoxNumRue.Text;
                SqlStr0 += "', Adr1= '" + tBRue.Text.Replace("'", "''").Replace(",",",,") + "', Adr2= '" + tBRueCompl.Text.Replace("'", "''").Replace(",",",,") + "', CodePostal= '" + tBoxCP.Text + "', Commune= '" + tBoxCommune.Text.Replace("'", "''").Replace(",",",,") + "', Pays= '" + cBoxPays.Text;
                SqlStr0 += "', Email= '" + tBoxEmail.Text + "' , Digicode= '" + tBDigicode.Text + "', InterphoneNom= '" + tBoxNomPorte.Text.Replace("'", "''").Replace(",",",,") + "', Etage= '" + tBoxEtage.Text + "', Porte= '" + tBoxPorte.Text.Replace("'", "''");

                if (rBMal.Checked)
                    SqlStr0 += "', Sexe='H'";
                else SqlStr0 += "', Sexe='F'";

                SqlStr0 += ", DateNaissance= '" + FonctionsAppels.convertDateMaria(mTBoxDateNaiss.Text + " 00:00:00", "MariaDb") + "', Provenance= '" + tBoxProvenance.Text + "', IdMotif1= '" + IdMotif1 + "', IdMotif2= '" + IdMotif2;
                SqlStr0 += "', Commentaire= @Commentaire, Urgence= '" + tBoxUrgence.Text + "', condParticuliere= @CondParticuliere";
                SqlStr0 += ", Assurance= '" + tBoxAssuranceNom.Text.Replace("'", "''").Replace(",", ",,") + "', TA = " + TA + ", Remarquable = " + Remarquable;
              
                if (tBoxNumCarte.Text.Length > 0 && tBoxNumCarte.Text.TrimStart(' ').Substring(0, 1) == "8")
                    SqlStr0 += ", NumCarte= '" + tBoxNumCarte.Text.Replace("'", "''").Replace(" ", "").Replace(".", "") + "'";
                else
                    SqlStr0 += ", NumCarte= 'Numeroinconnu'";

                if (cBoxAnnulation.Checked)   //L'appel est terminé ET Annulé (on enlève le médecin: -1)               
                {
                    SqlStr0 += ", CodeMedecin = -1, Termine = true, IdMotifAnnulation = '" + IdMotifAnnul + "'";
                }
                // else
                //   SqlStr0 += "', Termine = false";

                SqlStr0 += ", IdUnilab = '" + IdUnilab + "'";

                if(DateAccord == null)
                    SqlStr0 += ", DateAccord = NULL";
                else
                    SqlStr0 += ", DateAccord = '" + FonctionsAppels.convertDateMaria(DateAccord.ToString(), "MariaDb") + "'";

                SqlStr0 += " WHERE Num_Appel = " + NumVisite;


                //****SuiviAppel*** Ajout d'un enregistrement                                
                SqlStr1 = "INSERT INTO suiviappel ";
                SqlStr1 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp) VALUES(" + NumVisite.ToString();

                if (appelsAnnule == 1)
                    SqlStr1 += ", 'Annulé', -1";
                else SqlStr1 += ", 'Modifié','" + NumMed + "'";

                SqlStr1 += ", '" + Form1.Utilisateur[0] + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";


                //****AdrFacturation***
                if (FonctionsAppels.ExisteEnreg(NumVisite, "adrfacturation") == "OK")
                {
                    //On a une adresse de facturation pour cette visite                    
                    if (cBoxAutreAdrFact.Checked)
                    {
                        //On la modifie
                        SqlStr2 = "UPDATE adrfacturation ";
                        SqlStr2 += " SET Nom= '" + tBoxNomFact.Text.Replace("'", "''").Replace(",",",,") + "' , Prenom = '" + tBoxPrenomFact.Text.Replace("'", "''").Replace(",",",,") + "', Num_Rue = '" + tBoxNumRueFact.Text + "', Adr1= '" + tBoxRueFact.Text.Replace("'", "''").Replace(",",",,");
                        SqlStr2 += "', Adr2= '" + tBoxRue2Fact.Text.Replace("'", "''").Replace(",",",,") + "', CodePostal= '" + tBoxCPFact.Text + "', Commune= '" + tBoxCommuneFact.Text.Replace("'", "''").Replace(",",",,") + "', Pays= '" + cBoxPaysFact.Text + "'";
                        SqlStr2 += " WHERE Num_Appel = " + NumVisite;
                    }
                    else
                    {
                        //On la vire
                        SqlStr2 = "DELETE FROM adrfacturation ";
                        SqlStr2 += " WHERE Num_Appel = " + NumVisite;
                    }
                }
                else //Pas d'adresse facturation existante pour cette visite...
                {
                    if (cBoxAutreAdrFact.Checked)    //...Là on la rajoute
                    {
                        //on la rajoute
                        SqlStr2 = "INSERT INTO adrfacturation ";
                        SqlStr2 += " (Num_Appel,Nom, Prenom, Num_Rue, Adr1, Adr2, CodePostal, Commune, Pays) VALUES(" + NumVisite.ToString() + ",'" + tBoxNomFact.Text.Replace("'", "''").Replace(",",",,");
                        SqlStr2 += "','" + tBoxPrenomFact.Text.Replace("'", "''").Replace(",",",,") + "','" + tBoxNumRueFact.Text + "','" + tBoxRueFact.Text.Replace("'", "''").Replace(",",",,") + "','" + tBoxRue2Fact.Text.Replace("'", "''").Replace(",",",,") + "','" + tBoxCPFact.Text;
                        SqlStr2 += "','" + tBoxCommuneFact.Text.Replace("'", "''").Replace(",",",,") + "','" + cBoxPaysFact.Text + "')";
                    }
                }

                //****engarde               
                //S'il est Annulé, et qu'on est pas en visite, On efface l'enreg AppelenCours
                if (appelsAnnule == 1)
                {
                    //Si l'appel est: AT, AQ, V On libère le médecin de cette visite (on soustrait également -1 au nb de ses visites)
                    if (EtatdeVisiteMed == "AT" || EtatdeVisiteMed == "AQ" || EtatdeVisiteMed == "V")
                    {                                                                       
                        SqlStr3 = "UPDATE engarde SET Nb_Visite = (CASE WHEN Nb_Visite > 0 THEN Nb_Visite - 1 ELSE 0 END), StatusGarde = 'Disponible'";

                        //Idem pour l'infirmière (Organisation)
                        if (EtatdeVisiteOrg == "AT" || EtatdeVisiteMed == "V")                        
                            SqlStr3 += " WHERE CodeMedecin in (" + NumMed + ", " + NumInf + ")";                        
                        else
                            SqlStr3 += " WHERE CodeMedecin =" + NumMed;

                    }
                    else if (EtatdeVisiteMed == "PR")  //C'est une pré attribué, on ne change pas l'état du médecin
                    {
                        SqlStr3 = "UPDATE engarde SET Nb_Visite = (CASE WHEN Nb_Visite > 0 THEN Nb_Visite - 1 ELSE 0 END)";

                        //Idem pour l'infirmière (Organisation)
                        if (EtatdeVisiteOrg == "PR")
                            SqlStr3 += " WHERE CodeMedecin in (" + NumMed + ", " + NumInf + ")";
                        else
                            SqlStr3 += " WHERE CodeMedecin =" + NumMed;
                    }

                   
                    //Puis on efface le Status de la visite puis AppelsEnCours                    
                    SqlStr4 = "DELETE FROM status_visite WHERE Num_Appel = " + NumVisite;
                    SqlStr5 = "DELETE FROM status_visite_org WHERE Num_Appel = " + NumVisite;
                    SqlStr6 = "DELETE FROM appelsencours WHERE Num_Appel = " + NumVisite;
                }
                else
                {
                    //Sinon on le met à jour
                    //On re-géocode d'abord l'adresse
                    string Adresse = tBRue.Text + ", " + tBoxNumRue.Text + ", " + tBoxCP.Text + ", " + tBoxCommune.Text + ", " + cBoxPays.Text;

                    PointLatLng pt = GeocodeAdresse(Adresse);

                    SqlStr7 = "UPDATE appelsencours ";
                    SqlStr7 += " SET Tel_Appel = '" + tBAppellant.Text + "', Tel_Patient = '" + tBtelPatient.Text + "', Num_Personne = '" + NumPersonne + "',Nom= '" + tBoxNom.Text.Replace("'", "''").Replace(",",",,").Trim() + "', Prenom = '" + tBoxPrenom.Text.Replace("'", "''").Replace(",",",,").Trim() + "',Num_Rue = '" + tBoxNumRue.Text;
                    SqlStr7 += "', Adr1= '" + tBRue.Text.Replace("'", "''").Replace(",",",,") + "', Adr2= '" + tBRueCompl.Text.Replace("'", "''").Replace(",",",,") + "', CodePostal= '" + tBoxCP.Text + "', Commune= '" + tBoxCommune.Text.Replace("'", "''").Replace(",",",,") + "', Pays= '" + cBoxPays.Text;
                    SqlStr7 += "', Email= '" + tBoxEmail.Text + "', Digicode= '" + tBDigicode.Text + "', InterphoneNom= '" + tBoxNomPorte.Text.Replace("'", "''").Replace(",",",,") + "', Etage= '" + tBoxEtage.Text + "', Porte= '" + tBoxPorte.Text.Replace("'", "''");

                    if (rBMal.Checked)
                        SqlStr7 += "', Sexe='H'";
                    else SqlStr7 += "',Sexe='F'";

                    SqlStr7 += ", DateNaissance= '" + FonctionsAppels.convertDateMaria(mTBoxDateNaiss.Text + " 00:00:00", "MariaDb") + "', Provenance= '" + tBoxProvenance.Text + "', Motif1= '" + tBoxMotif1.Text.Replace("'", "''") + "', Motif2= '" + tBoxMotif2.Text.Replace("'", "''");
                    SqlStr7 += "', Commentaire= @Commentaire, Urgence= '" + tBoxUrgence.Text + "', CondParticuliere= @CondParticuliere";
                    SqlStr7 += ", Assurance= '" + tBoxAssuranceNom.Text.Replace("'", "''").Replace(",", ",,") + "', Latitude= '" + pt.Lat + "', Longitude = '" + pt.Lng + "'";

                    if (cBoxPatientRappel.Checked)
                        SqlStr7 += ", PatientRappel = 1";
                    else SqlStr7 += ", PatientRappel = 0";

                    SqlStr7 += " WHERE Num_Appel = " + NumVisite;
                }


                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0;
                    cmd.Transaction = trans;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("Commentaire", tBCommentaireDiag.Text);  //Pour les apostrophes et les virgules
                    cmd.Parameters.AddWithValue("CondParticuliere", tBoxCondPart.Text);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = SqlStr1; cmd.ExecuteNonQuery();

                    if (SqlStr2 != "")                    //Si on a une adresse de facturation
                    {
                        cmd.CommandText = SqlStr2;
                        cmd.Transaction = trans;
                        cmd.ExecuteNonQuery();
                    }

                    if (SqlStr3 != "")                    
                    {
                        cmd.CommandText = SqlStr3; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    }

                    if (appelsAnnule == 1)
                    {
                        cmd.CommandText = SqlStr4; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                        cmd.CommandText = SqlStr5; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                        cmd.CommandText = SqlStr6; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    }

                    if (SqlStr7 != "")
                    {
                        cmd.CommandText = SqlStr7;cmd.Transaction = trans;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("Commentaire", tBCommentaireDiag.Text);  //Pour les apostrophes et les virgules
                        cmd.Parameters.AddWithValue("CondParticuliere", tBoxCondPart.Text);
                        cmd.ExecuteNonQuery();
                    }

                    //on valide la transaction
                    trans.Commit();

                    //Si on a annulé l'appel et qu'il était attribué à un médecin, on regarde s'il n'a pas de visite Pré-attribuée
                    if (appelsAnnule == 1 && NumMed != -1 && EtatdeVisiteMed == "V")
                    {
                        FonctionsAppels.AnnuleVisite(NumMed.ToString());
                    }

                    //On efface également la fiche de consultation
                    if (appelsAnnule == 1)
                    {
                        FonctionsAppels.AnnuleFicheConsultation(NumVisite);
                    }
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la Modification de la visite. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la Modification de la visite. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //On met à jour l'appel pour signaler l'envoi d'un email soit ambulancier (TA), soit infirmière (téléconsult).
        private void MajMailEnvoye(string Destinataire)
        {
            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;
            
            try
            {
                string SqlStr0 = "UPDATE appels ";
                if(Destinataire == "TA")
                    SqlStr0 += " SET EmailTAEnvoye = 1";
                else
                    SqlStr0 += " SET EmailInfEnvoye = 1";
                SqlStr0 += " WHERE Num_Appel = " + NumVisite;
                
                cmd.CommandText = SqlStr0; 
                cmd.ExecuteNonQuery();

                //On met à jour la valeur de la variable
                if (Destinataire == "TA")
                    EmailTaEnvoye = true;
                else
                    EmailInfEnvoye = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur lors de la mise à jour pour l'envoie de l'email. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //*******Création de la Fiche de consultation *****************
        private string CreerFicheConsult(int NumAppel, int NumPersonne)
        {
            string Retour = "KO";

            string nomMedTraitant = NomMedecinTraitant;
            string prenomMedTraitant = PrenomMedecinTraitant;

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_FicheVisite"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            //On déclare ici les requetes (à cause de la transaction)
            string SqlStr0 = "";
            string SqlStr1 = "";
            string SqlStr2 = "";
            string SqlStr3 = "";
                                                                    
            try
            {                
                //Dans une transaction, on créé les enreg pour visite, structconsult, correspondance, assurances                

                MySqlTransaction trans;   //Déclaration d'une transaction

                //Def des requettes
                //*****visite***
                SqlStr0 = "INSERT INTO visite ";
                SqlStr0 += " (Num_Appel, CodeMedecin, DateC, HArrivee, HDepart, ConsultMult, NomPatient, PrenomPatient, DateNaiss, Sexe, Num_Rue, Adr1, Adr2, CodePostal, Commune, Pays, Tel_Patient, ";
                SqlStr0 += " Bon_Police, ssTaxeUrg, Etat_Secretariat, Etat_Facturation) ";
                SqlStr0 += " VALUES('" + NumAppel.ToString() + "', -1, '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "','";
                SqlStr0 += FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "','";
                SqlStr0 += FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "', 0,'";
                SqlStr0 += tBoxNom.Text.Replace("'", "''").Replace(",", ",,").Trim() + "','";
                SqlStr0 += tBoxPrenom.Text.Replace("'", "''").Replace(",", ",,").Trim() + "','" + FonctionsAppels.convertDateMaria(mTBoxDateNaiss.Text + " 00:00:00", "MariaDb");

                if (rBMal.Checked)
                    SqlStr0 += "','H','";
                else SqlStr0 += "','F','";

                SqlStr0 += tBoxNumRue.Text + "','" + tBRue.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBRueCompl.Text.Replace("'", "''").Replace(",", ",,") + "','";
                SqlStr0 += tBoxCP.Text + "','" + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,");
                SqlStr0 += "','" + cBoxPays.Text + " ','" + tBtelPatient.Text + "', 0, 0, 0, 0)"; 
                    
                //****structconsult***
                SqlStr1 = "INSERT INTO structconsult ";
                SqlStr1 += " (Num_Appel, 15mn, 20mn, 25mn, 30mn, 35mn, 40mn, Autre_Duree, PsychoMn, ReaMn, TransportMn, ExaCadavMn)";
                SqlStr1 += " VALUES(" + NumAppel.ToString() + ", 0, 1, 0, 0, 0, 0, '', 0, 0, 0, 0)";

                //****correspondance***
                SqlStr2 = "INSERT INTO correspondance ";
                SqlStr2 += " (Num_Appel, NomMedTraitant, PrenomMedTraitant, ORGTA, ORGAideDom) VALUES(" + NumAppel.ToString() + ",'" + nomMedTraitant.Replace("'", "''") + "','" + prenomMedTraitant.Replace("'", "''") + "', 0,0)";                                  

                //****assurances*****                              
                SqlStr3 = "INSERT INTO assurances ";
                SqlStr3 += " (Num_Appel, Cas_Maladie, Cas_Accident, Cas_Militaire, Cas_Police, Ass_Inv, RMCAS, Rabais_10Pc, SPC, SansFacture, HG_CASS, Encaisse_Place,";
                SqlStr3 += " CertifMaladie, CertifAccident) ";
                SqlStr3 += " VALUES(" + NumAppel.ToString() + ",0,0,0,0,0,0,0,0,0,0,0,0,0)";
                
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
                    MessageBox.Show("Erreur lors de la création de la visite. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);                   
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
                MessageBox.Show("Erreur lors de la création de la visite. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);                
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


        #region Contrôles de saisie
        //********Gestion des contrôles de saisie********
        //On regarde si on rentre bien un n° ou un + Quand on appuie sur une touche
        private void tBAppellant_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumerique = false;

            //Pour les nombres du haut du clavier ET la touche + ET les nombres du pavé numérique
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) && e.KeyCode != Keys.Add &&
                 (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
            {
                nonNumerique = true;
                Console.WriteLine("KO");
            }
          

            Console.WriteLine(e.KeyValue + " " + e.KeyData);
        
            //On lance une recherche sur le n° de Tel
            if (e.KeyCode == Keys.Space)
            {
                //Si le CTI est activé, on recupère le n° de Tel
                if (Form1.ActivationCTI == true && tBAppellant.Text == "")
                {
                    string NumTel = RetourneNumeroCTI();
                    
                    //On reformate le n° retourné par le CTI
                    if (NumTel != "")
                    {
                        NumTel = FormateNumTel(NumTel);

                        //Puis on lance la recherche
                        RechPers(NumTel);

                        //On affecte le numéro formaté au controle
                        tBAppellant.Text = NumTel;
                    }
                }
                else
                {
                    //Sinon on lance la recherche si le texte box n'est pas vide
                    if (tBAppellant.Text != "")
                    {                                                
                        //On commence par formater le n° de Tel
                        string NumTel = FormateNumTel(tBAppellant.Text);

                        //On affecte le numéro formaté au controle
                        tBAppellant.Text = NumTel;

                        //On lance la recherche
                        RechPers(NumTel);                        
                    }
                }
            }

            //Pour le copier/coller
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBAppellant.Copy();

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
                tBAppellant.Paste();
        }

     
        //Quand on relache la touche....
        private void tBAppellant_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (nonNumerique == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }


        private void tBtelPatient_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumerique = false;

            //Pour les nombres du haut du clavier ET la touche + ET les nombres du pavé numérique
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) && e.KeyCode != Keys.Add &&
                 (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
            {
                nonNumerique = true;
                Console.WriteLine("KO");
            }

            Console.WriteLine(e.KeyValue + " " + e.KeyData);

            //Pour le copier/coller
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBtelPatient.Copy();

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
                tBtelPatient.Paste();
        }

        //Quand on relache la touche....
        private void tBtelPatient_KeyPress(object sender, KeyPressEventArgs e)
        {

            //Si c'est un non numérique
            if (nonNumerique == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }

        }

        private void tBtelPatient_Leave(object sender, EventArgs e)
        {
            if (VerifSaisie() == "OK")
            {
                bValider.Enabled = true;
               // bMailInf.Enabled = true;
            }
            else
            {
                bValider.Enabled = false;
               // bMailInf.Enabled = false;
            }
        }


        private void tBoxEmail_Leave(object sender, EventArgs e)
        {
            if (tBoxEmail.Text != "")
            {
                if (Regex.IsMatch(this.tBoxEmail.Text, @"^([\w-\.])+@([\w]+\.)([a-zA-Z0-9]{2,4})$"))
                { //Rien c'est ok
                }
                else MessageBox.Show("L'adresse email est mal formatée.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (VerifSaisie() == "OK")
            {
                bValider.Enabled = true;
               // bMailInf.Enabled = true;
            }
            else
            {
                bValider.Enabled = false;
               // bMailInf.Enabled = false;
            }
        }

        private void tBAppellant_Leave(object sender, EventArgs e)
        {
            if (VerifSaisie() == "OK")
            {
                bValider.Enabled = true;
              //  bMailInf.Enabled = true;
            }
            else
            {
                bValider.Enabled = false;
               // bMailInf.Enabled = false;
            }
        }

        private void mTBoxDateNaiss_Leave(object sender, EventArgs e)
        {
            //on calcul l'age de la personne       
            DateTime test;

            if (DateTime.TryParse(mTBoxDateNaiss.Text, out test))
            {
                int Age = FonctionsAppels.CalculeAge(DateTime.Parse(mTBoxDateNaiss.Text));
                tBoxAge.Text = Age.ToString();

                //On regarde qu'il n'ont pas rentré n'importe quoi
                if (Age < 0 || Age > 140)
                {
                    MessageBox.Show("Le patient a un âge incohérent: " + Age + " ans!\r\n" +
                                 "Voulez-vous vraiment prendre cette date de naissance?", "Attention Date de Naissance Incohérente!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }                 
            }
        }

        private void tBoxUrgence_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumeriqueUrg = false;

            //Pour les nombres du haut du clavier ET les nombres du pavé numérique
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) &&
                      (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)// && tBoxUrgence.Text.Length > 1)
            {
                nonNumeriqueUrg = true;
                Console.WriteLine("KO");
            }

            Console.WriteLine(e.KeyValue + " " + e.KeyData);
        }

        private void tBoxUrgence_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (nonNumeriqueUrg == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }

        private void tBoxCP_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumeriqueCP1 = false;

            //Pour les nombres du haut du clavier ET les nombres du pavé numérique
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) &&
                      (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
            {
                nonNumeriqueCP1 = true;
                Console.WriteLine("KO");
            }

            Console.WriteLine(e.KeyValue + " " + e.KeyData);
        }

        private void tBoxCP_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (nonNumeriqueCP1 == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }

        private void tBoxCPFact_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumeriqueCP2 = false;

            //Pour les nombres du haut du clavier ET les nombres du pavé numérique
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) &&
                      (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
            {
                nonNumeriqueCP2 = true;
                Console.WriteLine("KO");
            }

            Console.WriteLine(e.KeyValue + " " + e.KeyData);
        }

        private void tBoxCPFact_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (nonNumeriqueCP2 == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }
              

        private void tBoxNom_KeyDown(object sender, KeyEventArgs e)
        {
            TropLong = false;
          
            if (tBoxNom.Text.Length > 100)
            {
                if (e.KeyCode != Keys.Back)
                {
                    TropLong = true;
                    Console.WriteLine("KO");
                }
            }

            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBoxNom.Copy();

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
                tBoxNom.Paste();
        }

        private void tBoxNom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) == false)
            {
                if (e.KeyChar != 32)    //Espace               
                    if (e.KeyChar != 45)   // -
                        if (e.KeyChar != 8)  // <--
                            if (e.KeyChar != 39)   //'
                                e.Handled = true;        //On empêche la saisie du caractère        
            }
            else 
            {
                e.Handled = false;
            }        

            //Si c'est trop long                                    
            if (TropLong == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }

        private void tBoxPrenom_KeyDown(object sender, KeyEventArgs e)
        {           
            TropLong = false;

            if (tBoxPrenom.Text.Length > 100)
            {
                if (e.KeyCode != Keys.Back)
                {
                    TropLong = true;
                    Console.WriteLine("KO");
                }
            }

            //Pour le copier/coller
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBoxPrenom.Copy();

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
                tBoxPrenom.Paste();
        }

        private void tBoxPrenom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) == false)
            {
                if (e.KeyChar != 32)    //Espace               
                    if (e.KeyChar != 45)   // -
                        if (e.KeyChar != 8)  // <--
                            if (e.KeyChar != 39)   //'
                                e.Handled = true;   //On empêche la saisie du caractère
            }
            else
            {
                e.Handled = false;
            }

            //Si c'est trop long
            if (TropLong == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }


        private void tBoxNumCarte_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumeriqueUrg = false;

            //Pour les nombres du haut du clavier ET les nombres du pavé numérique
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) &&
                      (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
            {
                nonNumeriqueUrg = true;
                Console.WriteLine("KO");
            }

            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBoxNumCarte.Copy();

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
                tBoxNumCarte.Paste();

            Console.WriteLine(e.KeyValue + " " + e.KeyData);
        }

        private void tBoxNumCarte_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (nonNumeriqueUrg == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }

        private void tBoxNomPorte_KeyDown(object sender, KeyEventArgs e)
        {
            //Pour le copier/coller
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBoxNomPorte.Copy();

           // if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
             //   tBoxNomPorte.Paste();
        }

        private void tBDigicode_KeyDown(object sender, KeyEventArgs e)
        {
            //Pour le copier/coller
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBDigicode.Copy();

           // if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
             //   tBDigicode.Paste();
        }

        private void tBoxPorte_KeyDown(object sender, KeyEventArgs e)
        {
            //Pour le copier/coller
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBoxPorte.Copy();

           // if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
             //   tBoxPorte.Paste();
        }

        private void tBCommentaireDiag_KeyDown(object sender, KeyEventArgs e)
        {
            //Pour le copier/coller
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBCommentaireDiag.Copy();

           // if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
             //   tBCommentaireDiag.Paste();
        }

        private void tBoxCondPart_KeyDown(object sender, KeyEventArgs e)
        {
            //Pour le copier/coller
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBoxCondPart.Copy();

          //  if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            //    tBoxCondPart.Paste();
        }
        //*********Fin gestion des contrôles de saisie*******

        #endregion


        private void bFermer_Click(object sender, EventArgs e)
        {
            FonctionsCTI.ReactiveProxyHIN();
            Close();
        }

        private void FVisite_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (NumVisite == -1)    //Nouvelle fiche uniquement
            {
                //On appelle une fonction de la form parent (Form1) en quittant cette forme
                Form1 Parent = (Form1)this.Owner;
                Parent.QuitteFicheVisite("OK");
            }
        }

        //Pour les couleurs des onglets
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Rectangle paddedBounds = e.Bounds;
            TabPage page = tabControl1.TabPages[e.Index];

            //Selon l'onglet
            switch (e.Index)
            {
                case 0:
                case 1: if (e.Index == tabControl1.SelectedIndex)     //s'il est sélectionné               
                        e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, new Font(tabControl1.Font, FontStyle.Bold), SystemBrushes.ControlText, new PointF(e.Bounds.X + 3, e.Bounds.Y + 3));
                    else    //Sinon pas sélectionné
                        TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, SystemColors.ControlText);
                    break;

                case 2: if (e.Index == tabControl1.SelectedIndex)
                        e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, new Font(tabControl1.Font, FontStyle.Bold), Brushes.DarkRed, new PointF(e.Bounds.X + 3, e.Bounds.Y + 3));
                    else
                        TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, Color.DarkRed);
                    break;

                case 3: if (e.Index == tabControl1.SelectedIndex)
                        e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, new Font(tabControl1.Font, FontStyle.Bold), Brushes.DarkGreen, new PointF(e.Bounds.X + 3, e.Bounds.Y + 3));
                    else
                        TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, Color.DarkGreen);
                    break;

                default: break;
            }
        }

        //Gestion des onglet (enlever les libellé et tester: si != "" alors on clique dessus sinon on revient au 1er onglet)
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Si on a selectionné l'onglet TA et que ce dernier n'a pas de libellé... on repart sur l'onglet 1
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPageTA"] && tabControl1.TabPages["tabPageTA"].Text == "")
            {
                tabControl1.SelectedTab = tabControl1.TabPages["tabPageAdresse"];
            }

            //Si on a selectionné l'onglet Remarque et que ce dernier n'a pas de libellé... on repart sur l'onglet 1
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPageRemarque"] && tabControl1.TabPages["tabPageRemarque"].Text == "")
            {
                tabControl1.SelectedTab = tabControl1.TabPages["tabPageAdresse"];
            }
        }
       

        #region CTI
        //Retourne le N° de tel a partir du pabx
        private string RetourneNumeroCTI()
        {
            string Telephone = "";
            string[] InfosAppel = new string[4];
            string[] Reponse = new string[2];

            if (FonctionsCTI.toujoursConnecte(Form1.Token, Form1.Ligne) == "OK")
            {
                InfosAppel = FonctionsCTI.RecupInfosAppel(Form1.Token, Form1.Ligne);
                Telephone = InfosAppel[2].ToString();
            }
            else   //On se reconnecte
            {
                Reponse = FonctionsCTI.LoguePoste(Form1.Utilisateur[5], Form1.Utilisateur[7]);
                if (Reponse[0] != "KO")
                {
                    Form1.Token = Reponse[0];
                    Form1.Ligne = Reponse[1];

                    try
                    {
                        //Puis on remonte le n° de Tel
                        InfosAppel = FonctionsCTI.RecupInfosAppel(Form1.Token, Form1.Ligne);
                        Telephone = InfosAppel[2].ToString();
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show("erreur : " + e.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Erreur lors de la connexion au CTI. Vous pouvez continuer à travailler en désactivant le CTI dans le menu.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return Telephone;
        }

       

        //Recherche une personne à partir du champs Tel de la fiche
        private void RechPers(string NumTel)
        {
            //On recherche dans SmartRapport le n° correspondant et on retourne la liste des personnes correspondantes
            DataTable dtPersonne = new DataTable();
            dtPersonne = FonctionsAppels.RecherchePersonne("Tel", NumTel, null, null, null);

            //Si on a quelque chose
            if (dtPersonne.Rows.Count > 0)
            {
                int index = DialAfficheRecherche(dtPersonne);

                //On peuple les champs en fonction de l'index retourné
                if (index != -1)
                {
                    PeuplerChamps(index, dtPersonne);
                }
            }
        }

        //Rappel le n° affiché si CTI actif
        private void bRappeler_Click(object sender, EventArgs e)
        {
            if (Form1.ActivationCTI == true && tBAppellant.Text != "")
            {
                RappelleCTI(tBAppellant.Text);
            }
        }

        //Rappel le n° patient si CTI actif
        private void bRappeler2_Click(object sender, EventArgs e)
        {
            if (Form1.ActivationCTI == true && tBtelPatient.Text != "")
            {
                RappelleCTI(tBtelPatient.Text);
            }
        }


        private void RappelleCTI(string NumTel)
        {
            //Si le CTI est actif
            if (Form1.ActivationCTI == true)
            {
                string[] Reponse = new string[2];
                
                if (FonctionsCTI.toujoursConnecte(Form1.Token, Form1.Ligne) == "OK")
                {   //On appelle
                    FonctionsCTI.Appeler(NumTel, Form1.Token, Form1.Ligne);
                }
                else   //On se reconnecte (On est déconnecté)
                {
                    Reponse = FonctionsCTI.LoguePoste(Form1.Utilisateur[5], Form1.Utilisateur[7]);

                    if (Reponse[0] != "KO")
                    {
                        Form1.Token = Reponse[0];
                        Form1.Ligne = Reponse[1];

                        //Puis on passe l'appel
                        FonctionsCTI.Appeler(NumTel, Form1.Token, Form1.Ligne);
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la connexion au CTI. Vous pouvez continuer à travailler en désactivant le CTI dans le menu.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
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

        #endregion                  
       
        #region Coloration des controles actifs
        //Coloration des contrôles actifs

        #region Entrée des contrôles
        private void tBAppellant_Enter(object sender, EventArgs e)
        {
            tBAppellant.BackColor = Color.DarkGreen;
        }

        private void tBtelPatient_Enter(object sender, EventArgs e)
        {
            tBtelPatient.BackColor = Color.DarkGreen;
        }

        private void tBoxProvenance_Enter(object sender, EventArgs e)
        {
            tBoxProvenance.BackColor = Color.DarkGreen;
        }       

        private void tBoxNom_Enter(object sender, EventArgs e)
        {           
            tBoxNom.BackColor = Color.DarkGreen;
        }

        private void tBoxPrenom_Enter(object sender, EventArgs e)
        {
            tBoxPrenom.BackColor = Color.DarkGreen;
        }

        private void mTBoxDateNaiss_Enter(object sender, EventArgs e)
        {
            mTBoxDateNaiss.BackColor = Color.DarkGreen;

            //Positionnement du curseur au début du champs: une selection à 0,0         
            this.BeginInvoke((MethodInvoker)delegate ()
            {
                mTBoxDateNaiss.Select(0, 0);
            });
        }

        private void mTBoxDateNaiss_Click(object sender, EventArgs e)
        {
            //Positionnement du curseur au début du champs: une selection à 0,0         
            this.BeginInvoke((MethodInvoker)delegate ()
            {
                mTBoxDateNaiss.Select(0, 0);
            });
        }

        private void tBoxAssuranceNom_Enter(object sender, EventArgs e)
        {
            tBoxAssuranceNom.BackColor = Color.DarkGreen;
        }

        private void tBoxCommune_Enter(object sender, EventArgs e)
        {
            tBoxCommune.BackColor = Color.DarkGreen;
        }

        private void tBoxCP_Enter(object sender, EventArgs e)
        {
            tBoxCP.BackColor = Color.DarkGreen;
        }

        private void cBoxPays_Enter(object sender, EventArgs e)
        {
            cBoxPays.BackColor = Color.DarkGreen;
        }

        private void tBoxNumRue_Enter(object sender, EventArgs e)
        {
            tBoxNumRue.BackColor = Color.DarkGreen;
        }

        private void tBRue_Enter(object sender, EventArgs e)
        {
            tBRue.BackColor = Color.DarkGreen;
        }

        private void tBRueCompl_Enter(object sender, EventArgs e)
        {
            tBRueCompl.BackColor = Color.DarkGreen;
        }

        private void cBoxInterphone_Enter(object sender, EventArgs e)
        {
            cBoxInterphone.BackColor = Color.DarkGreen;
        }

        private void tBoxNomPorte_Enter(object sender, EventArgs e)
        {
            tBoxNomPorte.BackColor = Color.DarkGreen;
        }

        private void tBDigicode_Enter(object sender, EventArgs e)
        {
            tBDigicode.BackColor = Color.DarkGreen;
        }

        private void tBoxEtage_Enter(object sender, EventArgs e)
        {
            tBoxEtage.BackColor = Color.DarkGreen;
        }

        private void tBoxPorte_Enter(object sender, EventArgs e)
        {
            tBoxPorte.BackColor = Color.DarkGreen;
        }

        private void tBoxEmail_Enter(object sender, EventArgs e)
        {
            tBoxEmail.BackColor = Color.DarkGreen;
        }

        private void rBMal_Enter(object sender, EventArgs e)
        {
            rBMal.BackColor = Color.DarkGreen;
        }

        private void rBFemelle_Enter(object sender, EventArgs e)
        {
            rBFemelle.BackColor = Color.DarkGreen;
        }

        private void tBoxMotif1_Enter(object sender, EventArgs e)
        {
            tBoxMotif1.BackColor = Color.DarkGreen;
        }

        private void tBoxMotif2_Enter(object sender, EventArgs e)
        {
            tBoxMotif2.BackColor = Color.DarkGreen;
        }

        private void tBoxMotifAnnul_Enter(object sender, EventArgs e)
        {
            tBoxMotifAnnul.BackColor = Color.DarkGreen;
        }

        private void tBCommentaireDiag_Enter(object sender, EventArgs e)
        {
            tBCommentaireDiag.BackColor = Color.DarkGreen;
        }

        private void tBoxCondPart_Enter(object sender, EventArgs e)
        {
            tBoxCondPart.BackColor = Color.DarkGreen;
        }

        private void tBoxUrgence_Enter(object sender, EventArgs e)
        {
            tBoxUrgence.BackColor = Color.DarkGreen;
        }

        private void cBoxAnnulation_Enter(object sender, EventArgs e)
        {
            cBoxAnnulation.BackColor = Color.DarkGreen;
        }       

        //Onglet Adr facturation
        private void tBoxNomFact_Enter(object sender, EventArgs e)
        {
            tBoxNomFact.BackColor = Color.DarkGreen;
        }

        private void tBoxPrenomFact_Enter(object sender, EventArgs e)
        {
            tBoxPrenomFact.BackColor = Color.DarkGreen;
        }

        private void tBoxCPFact_Enter(object sender, EventArgs e)
        {
            tBoxCPFact.BackColor = Color.DarkGreen;
        }

        private void tBoxCommuneFact_Enter(object sender, EventArgs e)
        {
            tBoxCommuneFact.BackColor = Color.DarkGreen;
        }

        private void cBoxPaysFact_Enter(object sender, EventArgs e)
        {
            cBoxPaysFact.BackColor = Color.DarkGreen;
        }

        private void tBoxNumRueFact_Enter(object sender, EventArgs e)
        {
            tBoxNumRueFact.BackColor = Color.DarkGreen;
        }

        private void tBoxRueFact_Enter(object sender, EventArgs e)
        {
            tBoxRueFact.BackColor = Color.DarkGreen;
        }

        private void tBoxRue2Fact_Enter(object sender, EventArgs e)
        {
            tBoxRue2Fact.BackColor = Color.DarkGreen;
        }
        #endregion
        #endregion

        #region Sortie des contrôles
        private void tBoxProvenance_Leave(object sender, EventArgs e)
        {
            tBoxProvenance.BackColor = SystemColors.ControlDark;
        }

        private void cBoxInterphone_Leave(object sender, EventArgs e)
        {
            cBoxInterphone.BackColor = SystemColors.ControlDarkDark;
        }

        private void tBoxNomPorte_Leave(object sender, EventArgs e)
        {
            tBoxNomPorte.BackColor = SystemColors.ControlDark; 
        }

        private void tBDigicode_Leave(object sender, EventArgs e)
        {
            tBDigicode.BackColor = SystemColors.ControlDark; 
        }

        private void tBoxEtage_Leave(object sender, EventArgs e)
        {
            tBoxEtage.BackColor = SystemColors.ControlDark; 
        }

        private void tBoxPorte_Leave(object sender, EventArgs e)
        {
            tBoxPorte.BackColor = SystemColors.ControlDark; 
        }

        private void tBoxAssuranceNom_Leave(object sender, EventArgs e)
        {           
            tBoxAssuranceNom.BackColor = SystemColors.ControlDark;

            //On passe le focus au Controle suivant
            this.ActiveControl = rBMal;
        }

        private void rBMal_Leave(object sender, EventArgs e)
        {
            rBMal.BackColor = SystemColors.ControlDarkDark;
        }

        private void rBFemelle_Leave(object sender, EventArgs e)
        {
            rBFemelle.BackColor = SystemColors.ControlDarkDark;
        }

        private void cBoxAnnulation_Leave(object sender, EventArgs e)
        {
            cBoxAnnulation.BackColor = SystemColors.ControlDarkDark;
        }

        private void tBCommentaireDiag_Leave(object sender, EventArgs e)
        {
            tBCommentaireDiag.BackColor = SystemColors.ControlDark; 
        }

        //Recherche de la personne à partir du Tel Patient
        private void bRechTelPatient_Click(object sender, EventArgs e)
        {
            //On lance la recherche si le texte box n'est pas vide
            if (tBtelPatient.Text != "")
            {
                //On commence par formater le n° de Tel
                string NumTel = FormateNumTel(tBtelPatient.Text);

                //On affecte le numéro formaté au controle
                tBtelPatient.Text = NumTel;

                //On lance la recherche
                RechPers(NumTel);
            }
        }       

        private void tBoxCondPart_Leave(object sender, EventArgs e)
        {
            tBoxCondPart.BackColor = SystemColors.ControlDark; 
        }

        private void label8_MouseClick(object sender, MouseEventArgs e)
        {
            //Pour le copier coller du label
            if (e.Button == MouseButtons.Right)
            {
                string texte = label8.Text;  //On récupére le texte du label

                if (!string.IsNullOrEmpty(texte))
                {
                    Clipboard.SetText(texte);  //On copie le texte dans le presse-papiers
                }
            }
        }

        private void bConsentement_Click(object sender, EventArgs e)
        {
            //Pour le consentement, on affiche une boite pour lire un texte sur le consentement           
            DialogResult dialogResult = MessageBox.Show("Est-ce que vous êtes d’accord pour que vos données\r\nsoient conservées électroniquement\r\n" +
                                                        "et transmises aux médecins et assurances?",
                                                        "Consentement", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                //On met la date du jour à DateAccord
                DateAccord = DateTime.Now;
                bConsentement.Enabled = false;   //On désactive l'accord (plus beson c'est ok)
            }          
        }

        private void tBoxRue2Fact_Leave(object sender, EventArgs e)
        {
            tBoxRue2Fact.BackColor = SystemColors.ControlDark;
        }
        #endregion          


        #region Géocodage
        //Fonction qui géocode une adresse et retourne les coordonnées
        public static PointLatLng GeocodeAdresse(string Adresse)
        {
            try
            {
                PointLatLng Coordonnees = new PointLatLng();
                Coordonnees.Lat = 0.0;
                Coordonnees.Lng = 0.0;

                CoordLatLng coordLatLng = new CoordLatLng();

                //On indique la validation automatique des demandes d'acceptation des certificats
                FonctionsCTI.SSLValidator.OverrideValidation();

                GeoCoderStatusCode status = GeoCoderStatusCode.UNKNOWN_ERROR;
                GeocodingProvider gp = GMapProviders.OpenStreetMap;


                if (gp == null)    //On veut géocoder avec GMapProvider
                {
                    gp = GMapProviders.OpenStreetMap as GeocodingProvider;
                }

                if (gp != null)
                {
                    var pt = gp.GetPoint(Adresse, out status);

                    if (status == GeoCoderStatusCode.OK && pt.HasValue)
                    {
                        Coordonnees = pt.Value;
                    }
                    else
                    {
                        //on ré-essaie une autre fois
                        gp = GMapProviders.OpenStreetMap as GeocodingProvider;
                        pt = gp.GetPoint(Adresse, out status);
                        if (status == GeoCoderStatusCode.OK && pt.HasValue)
                        {
                            Coordonnees = pt.Value;
                        }
                    }
                }

                //si malgrès tout ça ne marche toujours pas, on essaie d'une autre manière
                if (status != GeoCoderStatusCode.OK)
                {
                    var result = GetCoordonneeDAddress(Adresse);

                    //On affecte les données
                    Coordonnees.Lat = result.Lat;
                    Coordonnees.Lng = result.Lng;
                }

                //Puis on vérifie si elles sont dans le cadre de la carte
                PointLatLng Coord = new PointLatLng();

                if (VerifGeocodage(Coordonnees) == "OK")
                {
                    Coord.Lat = Coordonnees.Lat;
                    Coord.Lng = Coordonnees.Lng;
                }
                else
                {
                    Coord.Lat = ptNonGeocode.Lat;
                    Coord.Lng = ptNonGeocode.Lng;
                }

                return Coord;
            }
            catch(Exception ex)
            {
                MessageBox.Show("GeocodeAdresse():  " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception("Geocode");
            }
        }


        //Fonction qui appel un serveur de géocodage et parse la réponse MAIS de manière asynchrone (donc ça ne nous convient pas ici)     
        /*  public static async Task<CoordLatLng> GetCoordonneeDAddress(string addresse)
          {
             CoordLatLng coordLatLng = new CoordLatLng();   //Pour le retour de la réponse

             HttpClient httpClient = new HttpClient { BaseAddress = new Uri("https://nominatim.openstreetmap.org/") };

             httpClient.Timeout = TimeSpan.FromMilliseconds(500);     //On met un timeout
             httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Regul V1".ToString());   //Pour l'autorisation, il faut mettre une version de soft

             HttpResponseMessage httpResult = await httpClient.GetAsync(String.Format("search?q={0}&format=json&addressdetails=1", addresse));

             var result = await httpResult.Content.ReadAsStringAsync();
             var r = (JArray)JsonConvert.DeserializeObject(result);
             var lat = ((JValue)r[0]["lat"]).Value;
             var lng = ((JValue)r[0]["lon"]).Value;
             coordLatLng.Lat = (double)lat;
             coordLatLng.Lng = (double)lng;

             return coordLatLng;//string.Format("{0};{1}", latString, longString);
          }*/

        //Fonction qui appel un serveur de géocodage et parse la réponse     
        public static CoordLatLng GetCoordonneeDAddress(string addresse)
        {
            CoordLatLng coordLatLng = new CoordLatLng();   //Pour le retour de la réponse
            coordLatLng.Lat = 0;
            coordLatLng.Lng = 0;

            HttpWebRequest WRequete = (HttpWebRequest)WebRequest.Create(String.Format("https://nominatim.openstreetmap.org/search?q={0}&format=json&addressdetails=1", addresse));

            WRequete.Method = "GET";           
            WRequete.Timeout = 1000;
            WRequete.UserAgent = "Regul V1";     //Pour l'autorisation, il faut mettre une version de soft

            try
            {
                HttpWebResponse Reponse = (HttpWebResponse)WRequete.GetResponse();

                //On récupère la réponse du serveur
                Stream dataStream = Reponse.GetResponseStream();
                //On charge le flux dans un StreamReader.
                StreamReader reader = new StreamReader(dataStream);
                //Chargement dans une chaine du flux
                string responseFromServer = reader.ReadToEnd();

                reader.Close();    //Fermeture du Flux

                if (responseFromServer != "[]")
                {
                    //On déserialise la chaine obtenue dans un tableau (plusieurs enregistrements peuvent être retournées)
                    var rep = (JArray)JsonConvert.DeserializeObject(responseFromServer);   //
                    var lat = ((JValue)rep[0]["lat"]).Value.ToString();    //On prend le 1er enregistrement
                    var lng = ((JValue)rep[0]["lon"]).Value.ToString();    //On prend le 1er enregistrement
                    coordLatLng.Lat = double.Parse(lat);
                    coordLatLng.Lng = double.Parse(lng);
                }
                else
                {
                    coordLatLng.Lat = 0;
                    coordLatLng.Lng = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return coordLatLng;            
        }

        //Fonction qui vérifie que le géocodage de l'adresse reste bien dans la région définie en paramètre
        public static string VerifGeocodage(PointLatLng pt)
        {            
            string retour = "KO";

            if (pt.Lat < LatN && pt.Lat > LatS && pt.Lng > LngW && pt.Lng < LngE)
                retour = "OK";          //c'est dans les clous
            else
                retour = "KO";

            return retour;
        }
        #endregion

       
    }

    //Cette classe retourne un type pour les coordonnées de géolocalisation
    public partial class CoordLatLng
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }


}

//A faire:

//Voir si date naissance vide DateTime?
