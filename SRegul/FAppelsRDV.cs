using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using MySqlConnector;
using System.Text.RegularExpressions;
using GMap.NET;
using System.Globalization;
using System.Net;
using GMap.NET.MapProviders;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace SRegulV2
{

    public partial class FAppelsRDV : Form
    {
        public Int32 NumAppelRDV = -1;
        public Int32 NumPersonne = -1;
        public string provenance = "MainForm";   //On vient de la form...
        private bool nonNumerique = false;    //Pour s'assurer qu'on rentre bien un caractere numerique, ou le +
        private bool nonNumeriqueUrg = false;  //Pour l'urgence
        private bool nonNumeriqueCP1 = false;  //Pour le cp adresse
        private bool nonNumeriqueCP2 = false;  //Pour le cp adresse facturation
        private bool toucheF5 = false;  //Pour la touche F5 seulement autorisée
        private bool TropLong = false;
        private string IdMotif1 = "";          //Pour mémoriser les motifs
        private string IdMotif2 = "";
        private string CodeMedecin = "";
        private bool TA = false;                     //Est ce un TA?
        private bool Remarquable = false;            //Est ce un patient remarquable?   
        private bool FactureImpayee = false;            //Y a t'il des factures impayées?    
        private DateTime DateRDV = DateTime.Now;

        private DataTable dt10DerAppels = new DataTable();
        private DataTable dt20DerAppels = new DataTable();
        private DataTable dtMedecin = new DataTable();

        public static PointLatLng ptNonGeocode = new PointLatLng();         //Point par défaut pour les adresses non géocodées                

        public static double LatN;     //On détermine le bornage autour de genève
        public static double LatS;
        public static double LngW;
        public static double LngE;


        public FAppelsRDV()
        {
            InitializeComponent();

            //Pour la securité
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //On défini le point comme séparateur décimal
            CultureInfo CultureFr = new CultureInfo("fr-CH");
            CultureFr.NumberFormat.NumberDecimalSeparator = ".";

            //On place le curseur dans le n° de Tel
            tBAppellant.Focus();
        }

        private void bAbandonner_Click(object sender, EventArgs e)
        {
            //On abandonne le rendez-vous
            DialogResult dialogResult = MessageBox.Show("Voulez vous abandonner ce rendez-vous?", "Abandon de rendez-vous", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                //On réactive le proxy HIN
                FonctionsCTI.ReactiveProxyHIN();

                //On ferme la form
                this.Close();
            }
        }

        private void bValider_Click(object sender, EventArgs e)
        {
            //On valide la visite                                                                    
            //1 - Si on est en création
            //Ajout un enreg dans AppelsRDV, AdrFacturation (s'il y a )
            if (VerifSaisie() == "OK")
            {
                //On incrémente le compteur de rafraichissement (base)
                FonctionsAppels.IncrementeRafraichissement();

                //Si c'est un nouvel AppelRDV
                if (NumAppelRDV == -1)
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
                                int NumAppelRDV = CreationAppelRDV(NumPersonne);

                                if (NumAppelRDV != -1)  //Tout c'est bien passé
                                {
                                    mouchard.evenement("Création d''un doublon! pour la prise de RDV, appelRDV n°: " + NumAppelRDV +
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
                            int NumAppelRDV = CreationAppelRDV(NumPersonne);

                            if (NumAppelRDV != -1)  //Tout c'est bien passé
                            {
                                mouchard.evenement("Création appelRDV n°: " + NumAppelRDV +
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
                        int NumAppelRDV = CreationAppelRDV(NumPersonne);

                        if (NumAppelRDV != -1)  //Tout c'est bien passé
                        {
                            mouchard.evenement("Création appelRDV n°: " + NumAppelRDV +
                                " de " + tBoxNom.Text + " " + tBoxPrenom.Text + " n° pers. :" + NumPersonne + " né le " + mTBoxDateNaiss.Text + " Adr: " + tBRue.Text.Replace("'", "''").Replace(",", ",,") + " " + tBoxNumRue.Text + " " +
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
                                MajAppelRDV();   //Maj de l'appel

                                mouchard.evenement("Création d''un doublon pendant la maj de l''appel RDV!, appel n°: " + NumAppelRDV +
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
                            MajAppelRDV();   //Maj de l'appel                                                      

                            mouchard.evenement("Maj de l'appel n° " + NumAppelRDV + " concernant " + tBoxNom.Text + " " + tBoxPrenom.Text + " n° pers. :" + NumPersonne + " né le " + mTBoxDateNaiss.Text + " Adr: " + tBRue.Text.Replace("'", "''").Replace(",", ",,") + " " + tBoxNumRue.Text + " " +
                                    tBoxCP.Text + " " + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,"), Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log                        

                            //On réactive le proxy HIN
                            FonctionsCTI.ReactiveProxyHIN();

                            //Puis on ferme la form
                            this.Close();
                        }
                    }
                    else   //personne déjà connue
                    {
                        MajAppelRDV();  //Maj de l'appel

                        mouchard.evenement("Maj de l'appel n° " + NumAppelRDV + " concernant " + tBoxNom.Text + " " + tBoxPrenom.Text + " n° pers. :" + NumPersonne + " né le " + mTBoxDateNaiss.Text + " Adr: " + tBRue.Text.Replace("'", "''").Replace(",", ",,") + " " + tBoxNumRue.Text + " " +
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
      

        private void FAppelsRDV_Load(object sender, EventArgs e)
        {
            //On prépare la fiche de rdv en fonction du n° AppelRDV...Si numero = -1 => Nvx RDV sinon Modification
            if (NumAppelRDV == -1)     //Nouveau RDV
            {
                //Nouvelle fiche         
                this.Text = "Nouvel appel pour un RDV";

                //On enlève les onglets TA et Remarque par défaut             
                tabControl1.TabPages["tabPageTA"].Text = "";
                tabControl1.TabPages["tabPageRemarque"].Text = "";

                labelTA.Text = "";
                labelRemarque.Text = "";
                lRemMedicales.Text = "";
                labelFactureImp.Text = "";

                cBoxMedecins.Text = "";

                //initialiser quelques champs
                cBoxPays.Text = "Suisse";

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
                beffaceAdresse.Visible = false;  //Effacement adresse existante

                bAbandonner.Visible = true;
                bAbandonner.Enabled = true;
                bValider.Enabled = false;
                bFermer.Visible = false;

                //On met la date d'aujourd'hui dans les controles
                dtDateRDV.Value = DateTime.Today;
                dtHeureRDV.Value = DateTime.Now;

                //On donne le focus au telephone
                this.ActiveControl = tBAppellant;
            }
            else if (NumAppelRDV != -1)   //Fiche RDV existante
            {
                //Chargement d'un appel RDV depuis la form listes appelsRDV

                //On enlève les onglets TA et Remarque par défaut (quitte à les remettre après)                          
                tabControl1.TabPages["tabPageTA"].Text = "";
                tabControl1.TabPages["tabPageRemarque"].Text = "";

                labelTA.Text = "";
                labelRemarque.Text = "";
                lRemMedicales.Text = "";
                labelFactureImp.Text = "";

                //On charge ici l'enregistrement AppelRDV correspondant
                DataSet DsAppelRDV = new DataSet();
                DsAppelRDV = FonctionsAppels.RetourneVisiteRDV(NumAppelRDV);

                beffaceAdresse.Visible = true;    //on peut effacer l'adresse de la fiche par défaut.

                PeuplerChampsAppelRDV(DsAppelRDV);

                this.Text = "Modification du Rendez vous " + NumAppelRDV.ToString();

                bAbandonner.Visible = false;  //On ne peut plus abandonner une fiche déjà créée Mais on peut la fermer
                bAbandonner.Enabled = false;
                bValider.Visible = true;
                bValider.Enabled = true;

                bFermer.Visible = true;

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

            if (tBAppellant.Text != "")     //Formatage du Telephone
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

            if (tBoxNom.Text != "")
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


            //Si autre adresse est cochée, on vérifie les champs
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


            if (cBoxMedecins.Text != "")
            {
                //Récupération du code médecin a partir du nom
                CodeMedecin = FonctionsAppels.CodeMedecin(cBoxMedecins.Text);
                if (CodeMedecin != "Non Trouvé")
                {
                    V1 += 1;
                    cBoxMedecins.BackColor = SystemColors.ControlDark;
                }
                else
                {
                    CodeMedecin = "-1";
                    cBoxMedecins.BackColor = Color.Red;
                }
            }
            else
                cBoxMedecins.BackColor = Color.Red;


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


            //on passe également les dates en parametre
            DateRDV = dtDateRDV.Value.Date + dtHeureRDV.Value.TimeOfDay;

            //En fonction des résultats
            switch (V1)
            {
                case 12:
                case 24:
                case 29:
                case 30:
                case 34:
                case 39: retour = "OK"; break;
                default: retour = "KO"; break;
            }

            //Puis pour certains contrôles non obligatoires, on met la couleur SystemColors.ControlDark
            tBoxPrenom.BackColor = SystemColors.ControlDark;
            mTBoxDateNaiss.BackColor = SystemColors.ControlDark;
            tBoxAssuranceNom.BackColor = SystemColors.ControlDark;
            tBRueCompl.BackColor = SystemColors.ControlDark;            
            tBoxMotif2.BackColor = SystemColors.ControlDark;
            tBoxRue2Fact.BackColor = SystemColors.ControlDark;

            return retour;
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
                tBoxPorte.Text = dtPersonne.Rows[index]["Porte"].ToString().Substring(0, 14);

            //Pour le sexe
            switch (dtPersonne.Rows[index]["Sexe"].ToString())
            {
                case "H": rBMal.Checked = true; rBFemelle.Checked = false; break;
                case "F": rBMal.Checked = false; rBFemelle.Checked = true; break;
                default: rBMal.Checked = false; rBFemelle.Checked = true; break;
            }


            //****Précédentes visites par rapport à MAINTENANT****
            int Npers = NumPersonne;

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
            //*******Fin de recherche des précédentes visites                                                                 

            //Pour l'adresse de facturation...Si la rue est différente, on coche Autre et on renseigne les champs
            if (dtPersonne.Rows[index]["Rue"].ToString() != dtPersonne.Rows[index]["RueFact"].ToString())
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
            }

            //Si c'est un patient remarquable, on rempli les remarques
            if (dtPersonne.Rows[index]["PatientRemarquable"].ToString() == "O")
            {
                PeuplerChampsRemarque(NumPersonne);
            }


            //Enfin pour le n° de carte d'assurance, on bloque la saisie si un n° existe déjà
            if (dtPersonne.Rows[index]["Num_Carte"].ToString().Length > 0 && dtPersonne.Rows[index]["Num_Carte"].ToString().Substring(0, 1) == "8")
            {
                tBoxNumCarte.Text = dtPersonne.Rows[index]["Num_Carte"].ToString();
                tBoxNumCarte.ReadOnly = true;
            }
            else
            {
                tBoxNumCarte.Text = "Inconnu";
                tBoxNumCarte.ReadOnly = false;
            }

        }


        private void PeuplerChampsAppelRDV(DataSet DsAppelRDV)
        {
            //Le RDV                                        
            if (DsAppelRDV.Tables["AppelsRDV"].Rows.Count > 0)
            {
                //On affect le numpersonne et codeMedecin à la variable globale
                NumPersonne = int.Parse(DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Num_Personne"].ToString());
                CodeMedecin = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["CodeMedecin"].ToString();                               
                DateRDV = dtDateRDV.Value.Date + dtHeureRDV.Value.TimeOfDay;    //on passe également les dates du RDV en parametre

                //affectation des dateTimeBox
                DateTime parsedDateTime = DateTime.Parse(DsAppelRDV.Tables["AppelsRDV"].Rows[0]["DateRDV"].ToString());
                dtDateRDV.Value = parsedDateTime.Date;
                dtHeureRDV.Value = parsedDateTime;

                //le médecin qui doit aller au RDV
                cBoxMedecins.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Medecin"].ToString();

                tBAppellant.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Tel_Appel"].ToString();
                tBtelPatient.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Tel_Patient"].ToString();
                tBoxNom.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Nom"].ToString();
                tBoxPrenom.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Prenom"].ToString();
                tBoxProvenance.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Provenance"].ToString();

                try
                {
                    mTBoxDateNaiss.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["DateNaissance"].ToString();
                }
                catch (Exception)
                {

                }

                //Calcul de l'âge
                DateTime DateNaiss;
                try
                {
                    DateNaiss = DateTime.Parse(DsAppelRDV.Tables["AppelsRDV"].Rows[0]["DateNaissance"].ToString());
                    int Age = FonctionsAppels.CalculeAge(DateNaiss);
                    tBoxAge.Text = Age.ToString();
                }
                catch (Exception)
                {
                    tBoxAge.Text = "0";
                }

                tBoxCP.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["CodePostal"].ToString();
                tBoxCommune.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Commune"].ToString();
                if (DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Pays"].ToString() == "")
                    cBoxPays.Text = "Suisse";
                else
                    cBoxPays.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Pays"].ToString();

                tBoxNumRue.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Num_Rue"].ToString();
                tBRue.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Adr1"].ToString();
                tBRueCompl.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Adr2"].ToString();
                tBDigicode.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Digicode"].ToString();

                //Interphone
                if (DsAppelRDV.Tables["AppelsRDV"].Rows[0]["InterphoneNom"].ToString() != "" && DsAppelRDV.Tables["AppelsRDV"].Rows[0]["InterphoneNom"] != DBNull.Value)
                {
                    tBoxNomPorte.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["InterphoneNom"].ToString();
                    cBoxInterphone.Checked = true;
                }
                else
                {
                    tBoxNomPorte.Text = "";
                    cBoxInterphone.Checked = false;
                }

                tBoxEtage.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Etage"].ToString();

                //On tronque la chaine
                if (DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Porte"].ToString().Length > 19)
                    tBoxPorte.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Porte"].ToString().Substring(0, 19);
                else tBoxPorte.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Porte"].ToString();

                //Pour les assurances       
                tBoxAssuranceNom.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Assurance"].ToString();
                tBoxNumCarte.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["NumCarte"].ToString();

                tBoxEmail.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Email"].ToString();

                //Pour le sexe
                switch (DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Sexe"].ToString())
                {
                    case "H": rBMal.Checked = true; rBFemelle.Checked = false; break;
                    case "F": rBMal.Checked = false; rBFemelle.Checked = true; break;
                    default: rBMal.Checked = false; rBFemelle.Checked = true; break;
                }

                //Pour les motifs 
                if (DsAppelRDV.Tables["AppelsRDV"].Rows[0]["IdMotif1"].ToString() != "")
                {
                    DataTable DtMotif = FonctionsAppels.InfosMotif(DsAppelRDV.Tables["AppelsRDV"].Rows[0]["IdMotif1"].ToString());
                    if (DtMotif.Rows.Count > 0 && DtMotif.Rows[0][0] != DBNull.Value)
                    {
                        tBoxMotif1.Text = DtMotif.Rows[0][1].ToString();
                        IdMotif1 = DtMotif.Rows[0][0].ToString();
                    }
                }

                if (DsAppelRDV.Tables["AppelsRDV"].Rows[0]["IdMotif2"].ToString() != "")
                {
                    DataTable DtMotif = FonctionsAppels.InfosMotif(DsAppelRDV.Tables["AppelsRDV"].Rows[0]["IdMotif2"].ToString());
                    if (DtMotif.Rows.Count > 0 && DtMotif.Rows[0][0] != DBNull.Value)
                    {
                        tBoxMotif2.Text = DtMotif.Rows[0][1].ToString();
                        IdMotif2 = DtMotif.Rows[0][0].ToString();
                    }
                }


                //Le commentaire
                tBCommentaireDiag.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Commentaire"].ToString();

                tBoxCondPart.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["CondParticuliere"].ToString();


                //****Précédentes visites****
                string Nom = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Nom"].ToString();
                string Prenom = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Prenom"].ToString();
                string DateNaissance = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["DateNaissance"].ToString();

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


                //Pour l'adresse de facturation...On regarde si on a un enregistrement
                if (DsAppelRDV.Tables["adrfacturationRDV"].Rows.Count > 0)
                {
                    //On active l'adresse de Facturation
                    cBoxAutreAdrFact.Checked = true;
                    tBoxNomFact.Text = DsAppelRDV.Tables["adrfacturationRDV"].Rows[0]["Nom"].ToString();
                    tBoxPrenomFact.Text = DsAppelRDV.Tables["adrfacturationRDV"].Rows[0]["Prenom"].ToString();

                    tBoxCPFact.Text = DsAppelRDV.Tables["adrfacturationRDV"].Rows[0]["CodePostal"].ToString();
                    tBoxCommuneFact.Text = DsAppelRDV.Tables["adrfacturationRDV"].Rows[0]["Commune"].ToString();
                    if (DsAppelRDV.Tables["adrfacturationRDV"].Rows[0]["Pays"].ToString() == "")
                        cBoxPaysFact.Text = "Suisse";
                    else
                        cBoxPaysFact.Text = DsAppelRDV.Tables["adrfacturationRDV"].Rows[0]["Pays"].ToString();

                    tBoxNumRueFact.Text = DsAppelRDV.Tables["adrfacturationRDV"].Rows[0]["Num_Rue"].ToString();
                    tBoxRueFact.Text = DsAppelRDV.Tables["adrfacturationRDV"].Rows[0]["Adr1"].ToString();
                    tBoxRue2Fact.Text = DsAppelRDV.Tables["adrfacturationRDV"].Rows[0]["Adr2"].ToString();
                }    //Fin d'on a une adresse de facturation

                //Est ce un TA? si oui va chercher les infos dans la base SmartRapport
                if (DsAppelRDV.Tables["AppelsRDV"].Rows[0]["TA"].ToString() == "True")
                {
                    PeuplerChampsTA(int.Parse(DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Num_Personne"].ToString()));

                    //On ne peut pas modifier l'adresse d'un TA
                    beffaceAdresse.Visible = false;
                }

                //A t'il des remarques? si oui va chercher les infos dans la base SmartRapport
                if (DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Remarquable"].ToString() == "True")
                {
                    PeuplerChampsRemarque(int.Parse(DsAppelRDV.Tables["AppelsRDV"].Rows[0]["Num_Personne"].ToString()));
                }

                //Pour le n° de carte d'assurance, on bloque la saisie si un n° existe déjà
                if (DsAppelRDV.Tables["AppelsRDV"].Rows[0]["NumCarte"].ToString().Length > 0 && DsAppelRDV.Tables["AppelsRDV"].Rows[0]["NumCarte"].ToString().Substring(0, 1) == "8")
                {
                    tBoxNumCarte.Text = DsAppelRDV.Tables["AppelsRDV"].Rows[0]["NumCarte"].ToString();
                    // tBoxNumCarte.ReadOnly = true;
                }
                else
                {
                    tBoxNumCarte.Text = "Inconnu";
                    //tBoxNumCarte.ReadOnly = false;
                }

                //Pour les factures impayées
                facturesImpayes(NumPersonne);
            }
        }

        //on peuple les champs TA
        private void PeuplerChampsTA(int NumPersonne)
        {
            DataSet dsTA = FonctionsAppels.GetInfoTA(NumPersonne);

            if (dsTA.Tables["Abonnement"].Rows.Count > 0)
            {
                //On active l'onglet TA et on redéfini les colonnes des listview               
                tabControl1.TabPages["tabPageTA"].Text = "TA/Médicalerte";

                //ainsi que le Label TA
                labelTA.Text = "TA";

                TA = true;

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

                //ainsi que le label Remarque
                labelRemarque.Text = "R";
                Remarquable = true;

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
            if (dt10DerAppels.Rows.Count > 0)
            {
                FRechDerniersAppels fRechDerniersAppels = new FRechDerniersAppels(dt10DerAppels);
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
                //On affiche la boite des provenances
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

            //Pour le copier/coller
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBoxAssuranceNom.Copy();

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
                tBoxAssuranceNom.Paste();
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
                bValider.Enabled = true;
            else bValider.Enabled = false;
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
                bValider.Enabled = true;
            else bValider.Enabled = false;
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
        private int CreationAppelRDV(int NPers)
        {
            int NumAppelRDV = -1;

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            //On déclare ici les requetes (à cause de la transaction)
            string SqlStr0 = "";
            string SqlStr1 = "";

            try
            {
                string sqlstr = "SELECT Max(Num_Appel_RDV) FROM appelsRDV";
                cmd.CommandText = sqlstr;

                DataTable dtAppelRDV = new DataTable();
                dtAppelRDV.Load(cmd.ExecuteReader());    //on execute

                if (dtAppelRDV.Rows[0][0].ToString() != "" && dtAppelRDV.Rows[0][0] != DBNull.Value)
                    NumAppelRDV = int.Parse(dtAppelRDV.Rows[0][0].ToString()) + 1;
                else NumAppelRDV = 1;

                //Puis dans une transaction, on créé les enreg pour AppelsRDV, AdrFacturation(si necessaire),                

                MySqlTransaction trans;   //Déclaration d'une transaction

                //Def des requettes
                //*****AppelsRDV***
                SqlStr0 = "INSERT INTO appelsRDV ";
                SqlStr0 += " (Num_Appel_RDV, Tel_Appel, Tel_Patient, Num_Personne, Nom, Prenom, Num_Rue, Adr1, Adr2, CodePostal, Commune, Pays, Email, Digicode, InterphoneNom, ";
                SqlStr0 += " Etage, Porte, Sexe, DateNaissance, Provenance, IdMotif1, IdMotif2, Commentaire, CodeMedecin, TA, Remarquable, ";
                SqlStr0 += " CondParticuliere, Assurance, NumCarte, DateRDV, Convertir_Appel) ";
                SqlStr0 += " VALUES('" + NumAppelRDV.ToString() + "','" + tBAppellant.Text + "','" + tBtelPatient.Text + "','" + NPers.ToString() + "','" + tBoxNom.Text.Replace("'", "''").Replace(",", ",,").Trim() + "','";
                SqlStr0 += tBoxPrenom.Text.Replace("'", "''").Replace(",", ",,").Trim() + " ','" + tBoxNumRue.Text + "','" + tBRue.Text.Replace("'", "''").Replace(",", ",,");
                SqlStr0 += "','" + tBRueCompl.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBoxCP.Text + "','" + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,");
                SqlStr0 += "','" + cBoxPays.Text + " ','" + tBoxEmail.Text + "','" + tBDigicode.Text + "','" + tBoxNomPorte.Text.Replace("'", "''").Replace(",", ",,");
                SqlStr0 += "','" + tBoxEtage.Text + "','" + tBoxPorte.Text.Replace("'", "''");

                if (rBMal.Checked)
                    SqlStr0 += "','H','";
                else SqlStr0 += "','F','";

                SqlStr0 += FonctionsAppels.convertDateMaria(mTBoxDateNaiss.Text + " 00:00:00", "MariaDb") + "','" + tBoxProvenance.Text + "','" + IdMotif1 + "','" + IdMotif2 + "',";
                SqlStr0 += "@Commentaire," + CodeMedecin + ",";
                SqlStr0 += TA + "," + Remarquable + ",@CondParticuliere,'" + tBoxAssuranceNom.Text.Replace("'", "''").Replace(",", ",,");

                if (tBoxNumCarte.Text.Length > 0 && tBoxNumCarte.Text.TrimStart(' ').Substring(0, 1) == "8")
                    SqlStr0 += "','" + tBoxNumCarte.Text.Replace("'", "''").Replace(" ", "").Replace(".", "") +
                               "', '" + FonctionsAppels.convertDateMaria(dtDateRDV.Text + " " + dtHeureRDV.Text, "MariaDb") + "', false)";
                else
                    SqlStr0 += "','Numeroinconnu','" + FonctionsAppels.convertDateMaria(dtDateRDV.Text + " " + dtHeureRDV.Text, "MariaDb") + "', false)";


                //****AdrFacturation***
                if (cBoxAutreAdrFact.Checked)
                {
                    SqlStr1 = "INSERT INTO adrfacturationRDV ";
                    SqlStr1 += " (Num_Appel_RDV,Nom, Prenom, Num_Rue, Adr1, Adr2, CodePostal, Commune, Pays) VALUES(" + NumAppelRDV.ToString() + ",'" + tBoxNomFact.Text.Replace("'", "''").Replace(",", ",,");
                    SqlStr1 += "','" + tBoxPrenomFact.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBoxNumRueFact.Text + "','" + tBoxRueFact.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBoxRue2Fact.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBoxCPFact.Text;
                    SqlStr1 += "','" + tBoxCommuneFact.Text.Replace("'", "''").Replace(",", ",,") + "','" + cBoxPaysFact.Text + "')";
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

                    //Si on a une autre adresse
                    if (cBoxAutreAdrFact.Checked)
                    {
                        cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    }

                    //on valide la transaction
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la création de la visite sur RDV. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    NumAppelRDV = -1;
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
                MessageBox.Show("Erreur lors de la création de la visite sur RDV. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NumAppelRDV = -1;
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return NumAppelRDV;
        }

        private void MajAppelRDV()
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

            try
            {
                //Dans une transaction, on UPDATE les enreg pour AppelsRDV, AdrFacturation(si necessaire),                 
                MySqlTransaction trans;   //Déclaration d'une transaction

                //Def des requettes
                //*****AppelsRDV***
                SqlStr0 = "UPDATE appelsRDV ";
                SqlStr0 += " SET Tel_Appel = '" + tBAppellant.Text + "', Tel_Patient = '" + tBtelPatient.Text + "', Num_Personne = " + NumPersonne + ", Nom='" + tBoxNom.Text.Replace("'", "''").Replace(",", ",,").Trim() + "', Prenom = '" + tBoxPrenom.Text.Replace("'", "''").Replace(",", ",,").Trim() + "', Num_Rue = '" + tBoxNumRue.Text;
                SqlStr0 += "', Adr1= '" + tBRue.Text.Replace("'", "''").Replace(",", ",,") + "', Adr2= '" + tBRueCompl.Text.Replace("'", "''").Replace(",", ",,") + "', CodePostal= '" + tBoxCP.Text + "', Commune= '" + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,") + "', Pays= '" + cBoxPays.Text;
                SqlStr0 += "', Email= '" + tBoxEmail.Text + "' , Digicode= '" + tBDigicode.Text + "', InterphoneNom= '" + tBoxNomPorte.Text.Replace("'", "''").Replace(",", ",,") + "', Etage= '" + tBoxEtage.Text + "', Porte= '" + tBoxPorte.Text.Replace("'", "''");

                if (rBMal.Checked)
                    SqlStr0 += "', Sexe='H'";
                else SqlStr0 += "', Sexe='F'";

                SqlStr0 += ", DateNaissance= '" + FonctionsAppels.convertDateMaria(mTBoxDateNaiss.Text + " 00:00:00", "MariaDb") + "', Provenance= '" + tBoxProvenance.Text + "', IdMotif1= '" + IdMotif1 + "', IdMotif2= '" + IdMotif2;
                SqlStr0 += "', Commentaire= @Commentaire, CodeMedecin = " + CodeMedecin + ", condParticuliere = @CondParticuliere";
                SqlStr0 += ", Assurance = '" + tBoxAssuranceNom.Text.Replace("'", "''").Replace(",", ",,") + "', TA = " + TA + ", Remarquable = " + Remarquable;

                if (tBoxNumCarte.Text.Length > 0 && tBoxNumCarte.Text.TrimStart(' ').Substring(0, 1) == "8")
                    SqlStr0 += ", NumCarte = '" + tBoxNumCarte.Text.Replace("'", "''").Replace(" ", "").Replace(".", "") + "'";
                else
                    SqlStr0 += ", NumCarte = 'Numeroinconnu'";

                SqlStr0 += ", DateRDV = '" + FonctionsAppels.convertDateMaria(dtDateRDV.Text + " " + dtHeureRDV.Text, "MariaDb") + "', Convertir_Appel = false";

                SqlStr0 += " WHERE Num_Appel_RDV = " + NumAppelRDV;

                //****AdrFacturation***
                if (FonctionsAppels.ExisteEnreg(NumAppelRDV, "adrfacturationRDV") == "OK")
                {
                    //On a une adresse de facturation pour cette visite                    
                    if (cBoxAutreAdrFact.Checked)
                    {
                        //On la modifie
                        SqlStr1 = "UPDATE adrfacturationRDV ";
                        SqlStr1 += " SET Nom= '" + tBoxNomFact.Text.Replace("'", "''").Replace(",", ",,") + "' , Prenom = '" + tBoxPrenomFact.Text.Replace("'", "''").Replace(",", ",,") + "', Num_Rue = '" + tBoxNumRueFact.Text + "', Adr1= '" + tBoxRueFact.Text.Replace("'", "''").Replace(",", ",,");
                        SqlStr1 += "', Adr2= '" + tBoxRue2Fact.Text.Replace("'", "''").Replace(",", ",,") + "', CodePostal= '" + tBoxCPFact.Text + "', Commune= '" + tBoxCommuneFact.Text.Replace("'", "''").Replace(",", ",,") + "', Pays= '" + cBoxPaysFact.Text + "'";
                        SqlStr1 += " WHERE Num_Appel_RDV = " + NumAppelRDV;
                    }
                    else
                    {
                        //On la vire
                        SqlStr1 = "DELETE FROM adrfacturationRDV ";
                        SqlStr1 += " WHERE Num_Appel_RDV = " + NumAppelRDV;
                    }
                }
                else //Pas d'adresse facturation existante pour cette visite...
                {
                    if (cBoxAutreAdrFact.Checked)    //...Là on la rajoute
                    {
                        //on la rajoute
                        SqlStr1 = "INSERT INTO adrfacturationRDV ";
                        SqlStr1 += " (Num_Appel_RDV,Nom, Prenom, Num_Rue, Adr1, Adr2, CodePostal, Commune, Pays) VALUES(" + NumAppelRDV.ToString() + ",'" + tBoxNomFact.Text.Replace("'", "''").Replace(",", ",,");
                        SqlStr1 += "','" + tBoxPrenomFact.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBoxNumRueFact.Text + "','" + tBoxRueFact.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBoxRue2Fact.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBoxCPFact.Text;
                        SqlStr1 += "','" + tBoxCommuneFact.Text.Replace("'", "''").Replace(",", ",,") + "','" + cBoxPaysFact.Text + "')";
                    }
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

                    if (SqlStr1 != "")                    //Si on a une adresse de facturation
                    {
                        cmd.CommandText = SqlStr1;
                        cmd.Transaction = trans;
                        cmd.ExecuteNonQuery();
                    }

                    //on valide la transaction
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la Modification de la visite sur Rendez-vous. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Erreur lors de la Modification de la visite sur rendez-vous. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    NumTel = FormateNumTel(NumTel);

                    //Puis on lance la recherche
                    RechPers(NumTel);

                    //On affecte le numéro formaté au controle
                    tBAppellant.Text = NumTel;
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
                bValider.Enabled = true;
            else bValider.Enabled = false;
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
                bValider.Enabled = true;
            else bValider.Enabled = false;
        }

        private void tBAppellant_Leave(object sender, EventArgs e)
        {
            if (VerifSaisie() == "OK")
                bValider.Enabled = true;
            else bValider.Enabled = false;
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
                if (Age < 0 || Age > 130)
                {
                    MessageBox.Show("Le patient a un âge incohérent: " + Age + " ans!\r\n" +
                                 "Voulez-vous vraiment prendre cette date de naissance?", "Attention Date de Naissance Incohérente!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
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

            //Pour le copier/coller
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

            Console.WriteLine(e.KeyValue + " " + e.KeyData);

            //Pour le copier/coller
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBoxNumCarte.Copy();

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
                tBoxNumCarte.Paste();
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

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
                tBoxNomPorte.Paste();
        }

        private void tBoxPorte_KeyDown(object sender, KeyEventArgs e)
        {
            //Pour le copier/coller
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBoxPorte.Copy();

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
                tBoxPorte.Paste();
        }

        private void tBCommentaireDiag_KeyDown(object sender, KeyEventArgs e)
        {
            //Pour le copier/coller
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBCommentaireDiag.Copy();

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
                tBCommentaireDiag.Paste();
        }

        private void tBoxCondPart_KeyDown(object sender, KeyEventArgs e)
        {
            //Pour le copier/coller
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
                tBoxCondPart.Copy();

            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
                tBoxCondPart.Paste();
        }

        //*********Fin gestion des contrôles de saisie*******

        #endregion


        private void bFermer_Click(object sender, EventArgs e)
        {
            //On réactive le proxy HIN
           // FonctionsCTI.ReactiveProxyHIN();

            Close();
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
                case 1:
                    if (e.Index == tabControl1.SelectedIndex)     //s'il est sélectionné               
                        e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, new Font(tabControl1.Font, FontStyle.Bold), SystemBrushes.ControlText, new PointF(e.Bounds.X + 3, e.Bounds.Y + 3));
                    else    //Sinon pas sélectionné
                        TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, SystemColors.ControlText);
                    break;

                case 2:
                    if (e.Index == tabControl1.SelectedIndex)
                        e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, new Font(tabControl1.Font, FontStyle.Bold), Brushes.DarkRed, new PointF(e.Bounds.X + 3, e.Bounds.Y + 3));
                    else
                        TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, Color.DarkRed);
                    break;

                case 3:
                    if (e.Index == tabControl1.SelectedIndex)
                        e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, new Font(tabControl1.Font, FontStyle.Bold), Brushes.DarkOrange, new PointF(e.Bounds.X + 3, e.Bounds.Y + 3));
                    else
                        TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, Color.DarkOrange);
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


        //Chargement de la liste de médecins
        private void cBoxMedecins_DropDown(object sender, EventArgs e)
        {
            dtMedecin.Clear();
            dtMedecin = FonctionsAppels.ChargeListeMedecins("actifs");

            //On vide la liste pour la rafraichir                
            cBoxMedecins.Items.Clear();

            for (int i = 0; i < dtMedecin.Rows.Count; i++)
            {
                cBoxMedecins.Items.Add(dtMedecin.Rows[i]["Nom"].ToString() + " " + dtMedecin.Rows[i]["Prenom"].ToString());
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

                    //Puis on remonte le n° de Tel
                    InfosAppel = FonctionsCTI.RecupInfosAppel(Form1.Token, Form1.Ligne);
                    Telephone = InfosAppel[2].ToString();
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

        //Rappel le n° tel patient si CTI actif
        private void bRappeler2_Click(object sender, EventArgs e)
        {
            if (Form1.ActivationCTI == true && bRappeler2.Text != "")
            {
                RappelleCTI(bRappeler2.Text);
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

        private void tBCommentaireDiag_Enter(object sender, EventArgs e)
        {
            tBCommentaireDiag.BackColor = Color.DarkGreen;
        }

        private void tBoxCondPart_Enter(object sender, EventArgs e)
        {
            tBoxCondPart.BackColor = Color.DarkGreen;
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


        private void cBoxMedecins_Enter(object sender, EventArgs e)
        {
            cBoxMedecins.BackColor = Color.DarkGreen;
        }
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

        private void tBCommentaireDiag_Leave(object sender, EventArgs e)
        {
            tBCommentaireDiag.BackColor = SystemColors.ControlDark;
        }

        private void tBoxCondPart_Leave(object sender, EventArgs e)
        {
            tBoxCondPart.BackColor = SystemColors.ControlDark;
        }

        private void tBoxRue2Fact_Leave(object sender, EventArgs e)
        {
            tBoxRue2Fact.BackColor = SystemColors.ControlDark;
        }


        private void cBoxMedecins_Leave(object sender, EventArgs e)
        {
            cBoxMedecins.BackColor = SystemColors.ControlDark;

            if (VerifSaisie() == "OK")
                bValider.Enabled = true;
            else bValider.Enabled = false;
        }
        #endregion

        #endregion

       
        //On converti le Rendez-vous en visite
        //1- Création d'un nouvel appel, et Fiche d'appel
        //2- Si med en garde et libre, attribution de la visite, sinon si en visite => préattribution, sinon dans le dispatch
        //3- Suppression du RDV
        private void bConvertir_Click(object sender, EventArgs e)
        {
            //On commence par créer un nouvel appel
            int NumAppel = ConvertionRDVenAppel(NumPersonne);
            if (NumAppel != -1)
            {
                //c'est ok, on créer la fiche d'appel
                //On créer maintenant la fiche de consultation
                if (CreerFicheConsult(NumAppel, NumPersonne, CodeMedecin) == "KO")
                    MessageBox.Show("Un problème est survenu lors de la création de la Fiche de consultation pour cette visite.", "Création de la fiche de consultation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    mouchard.evenement("Conversion d'un RDV en appel n°: " + NumAppel + ", ainsi que sa fiche de visite, " +
                    " de " + tBoxNom.Text + " " + tBoxPrenom.Text + " n° pers. :" + NumPersonne + " né le " + mTBoxDateNaiss.Text + " Adr: " + tBRue.Text.Replace("'", "''").Replace(",", ",,") + " " + tBoxNumRue.Text + " " +
                    tBoxCP.Text + " " + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,"), Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log                        
                             
                //On peut effacer l'AppelsRDV, il est converti en visite
                if (EffaceRDV(NumAppelRDV) == "K0")
                {
                    MessageBox.Show("Un problème est survenu lors de la suppression du RDV.", "Suppression du RDV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //On réactive le proxy HIN
                    FonctionsCTI.ReactiveProxyHIN();

                    //Puis on ferme la form
                    this.Close();
                }                
            }
            else
            {
                MessageBox.Show("Un problème est survenu lors de la conversion de ce rendez-vous en visite.", "Création de la fiche de consultation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        //On converti le rendez vous en appel
        private int ConvertionRDVenAppel(int NPers)
        {
            Int32 NumAppel = -1;
            string Status = "";

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

            try
            {
                //Déjà, on regarde si quelqu'un n'a pas entre temps déjà converti le RDV en Appel
                string sqlstring = "SELECT * FROM appelsRDV WHERE Num_Appel_RDV = " + NumAppelRDV;
                cmd.CommandText = sqlstring;

                DataTable dtRDV = new DataTable();
                dtRDV.Load(cmd.ExecuteReader());    //on execute

                if (dtRDV.Rows.Count == 0)  //Si le RDV n'est plus là, c'est que quelqu'un l'a effacé...on sort
                {
                    return -1;
                }

                //Puis on vérifie que le médecin est en garde et qu'il n'a pas de visite attribué sinon on lui pré-attribue               
                if (FonctionsAppels.RechEtatDispoMedecin(CodeMedecin) != "Pas en garde")
                {
                    if (FonctionsAppels.RechSiMedVisEnCours(CodeMedecin) == "Libre")
                        Status = "AT";
                    else
                        Status = "PR";
                }
                else
                {
                    MessageBox.Show("Le médecin auquel ce rendez-vous est attribué, n'est pas en garde.\n\r Veuillez en choisir un autre dans le RDV.");

                    return -1;   //on sort
                }

                //C'est ok..on continue: récup du plus grand n°
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
                SqlStr0 += " Etage, Porte, Sexe, DateNaissance, Provenance, IdMotif1, IdMotif2, Commentaire, Urgence, CodeMedecin, Termine, TA, Remarquable, ";
                SqlStr0 += " CondParticuliere, Assurance, NumCarte, Export, DateAccord) ";
                SqlStr0 += " VALUES('" + NumAppel.ToString() + "','" + tBAppellant.Text + "','" + tBtelPatient.Text + "','" + NPers.ToString() + "','" + tBoxNom.Text.Replace("'", "''").Replace(",", ",,").Trim() + "','";
                SqlStr0 += tBoxPrenom.Text.Replace("'", "''").Replace(",", ",,").Trim() + "','" + tBoxNumRue.Text + "','" + tBRue.Text.Replace("'", "''").Replace(",", ",,");
                SqlStr0 += "','" + tBRueCompl.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBoxCP.Text + "','" + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,");
                SqlStr0 += "','" + cBoxPays.Text + " ','" + tBoxEmail.Text + "','" + tBDigicode.Text + "','" + tBoxNomPorte.Text.Replace("'", "''").Replace(",", ",,");
                SqlStr0 += "','" + tBoxEtage.Text + "','" + tBoxPorte.Text.Replace("'", "''");

                if (rBMal.Checked)
                    SqlStr0 += "','H','";
                else SqlStr0 += "','F','";

                SqlStr0 += FonctionsAppels.convertDateMaria(mTBoxDateNaiss.Text + " 00:00:00", "MariaDb") + "','" + tBoxProvenance.Text + "','" + IdMotif1 + "','" + IdMotif2 + "',@Commentaire";
                SqlStr0 += ",0," + CodeMedecin + ", false,";
                SqlStr0 += TA + "," + Remarquable + ",@CondParticuliere,'" + tBoxAssuranceNom.Text.Replace("'", "''").Replace(",", ",,");

                if (tBoxNumCarte.Text.Length > 0 && tBoxNumCarte.Text.TrimStart(' ').Substring(0, 1) == "8")
                    SqlStr0 += "','" + tBoxNumCarte.Text.Replace("'", "''").Replace(" ", "").Replace(".", "") + "', 0, NULL)";
                else
                    SqlStr0 += "','Numeroinconnu', 0, NULL)";

                //****SuiviAppel***
                SqlStr1 = "INSERT INTO suiviappel ";
                SqlStr1 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                SqlStr1 += " VALUES(" + NumAppel.ToString() + ",'Création'," + CodeMedecin + ", '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";


                //****AdrFacturation***
                if (cBoxAutreAdrFact.Checked)
                {
                    SqlStr2 = "INSERT INTO adrfacturation ";
                    SqlStr2 += " (Num_Appel,Nom, Prenom, Num_Rue, Adr1, Adr2, CodePostal, Commune, Pays) VALUES(" + NumAppel.ToString() + ",'" + tBoxNomFact.Text.Replace("'", "''").Replace(",", ",,");
                    SqlStr2 += "','" + tBoxPrenomFact.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBoxNumRueFact.Text + "','" + tBoxRueFact.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBoxRue2Fact.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBoxCPFact.Text;
                    SqlStr2 += "','" + tBoxCommuneFact.Text.Replace("'", "''").Replace(",", ",,") + "','" + cBoxPaysFact.Text + "')";
                }

                //****AppelsEnCours
                //On géocode d'abord l'adresse
                string Adresse = tBRue.Text + ", " + tBoxNumRue.Text + ", " + tBoxCP.Text + ", " + tBoxCommune.Text + ", " + cBoxPays.Text;

                PointLatLng pt = GeocodeAdresse(Adresse);

                SqlStr3 = "INSERT INTO appelsencours ";
                SqlStr3 += " (Num_Appel, Tel_Appel, Tel_Patient, Num_Personne, Nom, Prenom, Num_Rue, Adr1, Adr2, CodePostal, Commune, Pays, Email, Digicode, InterphoneNom, ";
                SqlStr3 += " Etage, Porte, Sexe, DateNaissance, Provenance, Motif1, Motif2, Commentaire, Urgence, CodeMedecin, CondParticuliere, Assurance, Latitude, Longitude, PatientRappel) ";
                SqlStr3 += " VALUES(" + NumAppel.ToString() + ",'" + tBAppellant.Text + "','" + tBtelPatient.Text + "','" + NPers.ToString() + "','" + tBoxNom.Text.Replace("'", "''").Replace(",", ",,").Trim() + "','" + tBoxPrenom.Text.Replace("'", "''").Replace(",", ",,").Trim();
                SqlStr3 += " ','" + tBoxNumRue.Text + "','" + tBRue.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBRueCompl.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBoxCP.Text + "','" + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,") + "','" + cBoxPays.Text;
                SqlStr3 += " ','" + tBoxEmail.Text + "','" + tBDigicode.Text + "','" + tBoxNomPorte.Text.Replace("'", "''").Replace(",", ",,") + "','" + tBoxEtage.Text + "','" + tBoxPorte.Text.Replace("'", "''");

                if (rBMal.Checked)
                    SqlStr3 += "','H','";
                else SqlStr3 += "','F','";

                SqlStr3 += FonctionsAppels.convertDateMaria(mTBoxDateNaiss.Text + " 00:00:00", "MariaDb") + "','" + tBoxProvenance.Text + "','" + tBoxMotif1.Text.Replace("'", "''").Trim() + "','" + tBoxMotif2.Text.Replace("'", "''").Trim() + "',@Commentaire";
                SqlStr3 += ", 0, " + CodeMedecin + ", @CondParticuliere,'" + tBoxAssuranceNom.Text.Replace("'", "''").Replace(",", ",,") + "','" + pt.Lat + "','" + pt.Lng;
                SqlStr3 += "',0)";

                //****status_visite
                SqlStr4 = "INSERT INTO status_visite ";
                SqlStr4 += " (Num_Appel,CodeMedecin, Status, DateStatus, Ordre) VALUES(" + NumAppel.ToString() + "," + CodeMedecin + ",'" + Status + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', 0)";
                                                                        
                //****status_visite_org
                //Status: Non attribuée, Pré-attribuée, Attribuée, Acquitée, debut de visite (NA, PR, AT, V)

                SqlStr5 = "INSERT INTO status_visite_org ";
                SqlStr5 += " (Num_Appel,CodeMedOrg, Status, DateStatus, Ordre) VALUES(" + NumAppel.ToString() + ",-1,'" + Status + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', 0)";

                //Maj engarde (+1 au nb de visite, Attribuée)...Si medecin dispo: Status "AT"
                if (Status == "AT")
                {
                    SqlStr6 = "UPDATE engarde SET Nb_Visite = Nb_Visite + 1, StatusGarde = 'Attribuée'";
                    SqlStr6 += " WHERE CodeMedecin = '" + CodeMedecin + "'";
                }

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0;
                    cmd.Transaction = trans;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("Commentaire", "RDV " + tBCommentaireDiag.Text);  //Pour les apostrophes et les virgules
                    cmd.Parameters.AddWithValue("CondParticuliere", tBoxCondPart.Text);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //Si on a une autre adresse
                    if (cBoxAutreAdrFact.Checked)
                    {
                        cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    }

                    cmd.CommandText = SqlStr3; cmd.Transaction = trans;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("Commentaire", tBCommentaireDiag.Text);  //Pour les apostrophes et les virgules
                    cmd.Parameters.AddWithValue("CondParticuliere", tBoxCondPart.Text);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = SqlStr4; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr5; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    if(Status == "AT")   //Seulement si medecin dispo
                        cmd.CommandText = SqlStr6; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la conversion en visite. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Erreur lors de la conversion en visite. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NumAppel = -1;
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return NumAppel;
        }


        //*******Création de la Fiche de consultation *****************
        private string CreerFicheConsult(int NumAppel, int NumPersonne, string CodeMedecin)
        {
            string Retour = "KO";

            //On commence par rechercher le médecin traitant de la personne (Stopé à la demande du secrétariat)
            string[,] MedTraitant = new string[1, 2];
            MedTraitant[0, 0] = "";
            MedTraitant[0, 1] = "";
            //MedTraitant = FonctionsAppels.RechercheMedTraitant(NumPersonne);

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
                SqlStr0 += " VALUES('" + NumAppel.ToString() + "', CodeMedecin, '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "','";
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
                SqlStr2 += " (Num_Appel, NomMedTraitant, PrenomMedTraitant, ORGTA, ORGAideDom) VALUES(" + NumAppel.ToString() + ",'" + MedTraitant[0, 0] + "','" + MedTraitant[0, 1] + "', 0,0)";

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

        //On efface les 2 tables du RDV
        private string EffaceRDV(int NumAppelRDV)
        {
            string retour = "KO";
            
            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            //On déclare ici les requetes (à cause de la transaction)
            string SqlStr0 = "";
            string SqlStr1 = "";

            try
            {                                            
                MySqlTransaction trans;   //Déclaration d'une transaction

                //Def des requettes               
                SqlStr0 = "DELETE FROM appelsRDV WHERE Num_Appel_RDV = " + NumAppelRDV;         
                SqlStr1 = "DELETE FROM adrfacturationRDV WHERE Num_Appel_RDV = " + NumAppelRDV;   

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();                   

                    //on valide la transaction
                    trans.Commit();
                    retour = "OK";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la suppression de la visite sur RDV. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);                     
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
                MessageBox.Show("Erreur lors de la suppression de la visite sur RDV. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return retour;
        }

        private void bPoubelle_Click(object sender, EventArgs e)
        {
            if (NumAppelRDV != -1)
            {
                //On efface le RDV
                if (EffaceRDV(NumAppelRDV) == "OK")
                {
                    mouchard.evenement("Suppression du RDV n° " + NumAppelRDV + " , " + tBoxNom.Text + " " + tBoxPrenom.Text + " n° pers. :" + NumPersonne + " né le " + mTBoxDateNaiss.Text + " Adr: " + tBRue.Text.Replace("'", "''").Replace(",", ",,") + " " + tBoxNumRue.Text + " " +
                               tBoxCP.Text + " " + tBoxCommune.Text.Replace("'", "''").Replace(",", ",,"), Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log                        

                    //On réactive le proxy HIN
                    FonctionsCTI.ReactiveProxyHIN();

                    //Puis on ferme la form
                    this.Close();
                }
            }
        }

        private void beffaceAdresse_Click(object sender, EventArgs e)
        {
            tBoxCP.Text = "";
            tBoxCommune.Text = "";
            cBoxPays.Text = "Suisse";
            tBoxNumRue.Text = "";
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

            //Puis on désactive Bvalidé (s'il était actif)
            bValider.Enabled = false;
        }    

        #region Géocodage
        //Fonction qui géocode une adresse et retourne les coordonnées
        public static PointLatLng GeocodeAdresse(string Adresse)
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
}


//A faire:


