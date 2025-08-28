using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Threading;
using GMap.NET.MapProviders;

namespace CartoGeolocMedecins
{
    public partial class Form1 : Form
    {
        public GMapOverlay CalqueRoutes = new GMapOverlay("routes");                        //calque pour afficher les itinéraires  
        public GMapOverlay CalqueMarqueur = new GMapOverlay("marqueur");                    //calque pour afficher les marqueur sur la carte
        public GMapOverlay CalqueFlotte = new GMapOverlay("calque_Flotte_Vehicules");       //calque pour afficher les marqueur des voitures
        public GMapOverlay CalqueRech1vehicule = new GMapOverlay("CalqueRecherche1voiture");//calque pour la recherche de vehicules
        CultureInfo CultureFr = new CultureInfo("fr-CH");                                   //Pour les postes qui on la virgule à la place du point 
        int ecran = Screen.AllScreens.Length;                                               //on récupère le nombre d'ecran pour afficher la carte sur le deuxieme écran

        DataSet ListeMed = new DataSet();                                                   //dataset pour la liste des médecins
        DataSet ListePos = new DataSet();
        DataSet ListeVoiture = new DataSet();
        DataSet LocalisationTel = new DataSet();

        string Ticket = "";
        string CustID = ConfigurationManager.AppSettings["CustID"].ToString();

        //pour savoir quelle carte est ouverte
        int numcarte = 0;   // 0 carte principale - 1 deuxieme carte

        

        //Form1 form1 = new Form1();


        //**************************************************ON COMMENCE ICI *******************************************************************

        public Form1()
        {

            CultureFr.NumberFormat.NumberDecimalSeparator = ".";        //On défini le point comme séparateur décimal
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //on démarre en plein écran par défaut
            WindowState = FormWindowState.Maximized;
            
            Console.WriteLine("Nombre d'écrans :" + ecran);

            //Initialisation de la carte

            //on chosi la carte openStreetMap
            gmap.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;

            //au démarrage au positionne un marqueur sur SOS
            gmap.SetPositionByKeywords("Rue louis favre, 43, 1201, Geneve, Suisse");
            Marque_adresse(gmap.Position.Lat, gmap.Position.Lng, "Rue louis favre, 43, 1201, Geneve, Suisse");

            //on met un zoom par défaut
            gmap.Zoom = 16;
         
            //Le bouton gauche souris sert à glisser la carte
            gmap.DragButton = MouseButtons.Left;

            //et on active le zoom avec la molette de la souris
            gmap.MouseWheelZoomEnabled = true;

            //on enlève la petite croix rouge au milieu de l'écran
            gmap.ShowCenter = false;

            //on ajoute la liste des médecins sur le terrain
            ListeMed = RecupListesMedecins();
            RafraichiListeMed();
            RafraichiPositions();

            RecupLocalisation();

        }

        private void brecentre_Click(object sender, EventArgs e)
        {
            //On recentre la carte sur sos
            gmap.SetPositionByKeywords("Rue louis favre, 43, 1201, Geneve, Suisse");

            //on règle le zoom
            gmap.Zoom = 12;
        }

        
        public void btnItineraire_Click(object sender, EventArgs e)
        {
            
            FItineraire itineraire = new FItineraire();

            if (itineraire.ShowDialog() == DialogResult.OK)
            {
                //on vide le calque des itinéraires et des points
                CalqueRoutes.Clear();
                CalqueMarqueur.Clear();

                //***************origine / Destination***************************
                try
                {
                    //on récupère les adresses saisies de départ et d'arrivé
                    string depart = itineraire.adrDepart.ToString();
                    string arrive = itineraire.adrArriver.ToString();

                    //on récupère la latitude et la longitude du point de départ
                    gmap.SetPositionByKeywords(depart);
                    PointLatLng start = new PointLatLng(gmap.Position.Lat, gmap.Position.Lng);

                    //on met un marqueur sur l'adresse de départ
                    Marque_adresse(gmap.Position.Lat, gmap.Position.Lng, depart);

                    //on récupère la latitude et la longitude du point d'arrivé
                    gmap.SetPositionByKeywords(arrive);
                    PointLatLng end = new PointLatLng(gmap.Position.Lat, gmap.Position.Lng);

                    //on met un marqueur sur l'adresse d'arrivé
                    Marque_adresse(gmap.Position.Lat, gmap.Position.Lng, arrive);

                    //On appelle un provider pour définir un itinéraire (ici OpenStreet...mais on peut choisir parmis les 81 proposés)
                    MapRoute route = GMap.NET.MapProviders.OpenStreetMapProvider.Instance.GetRoute(start, end, false, false, 15);

                    //récup de la liste des points du trajets renvoyés par le provider

                    GMapRoute r = new GMapRoute(route.Points, "Mon_trajet");
                    //Pour la couleur et la taille de la route empruntée
                    r.Stroke.Width = 4;
                    r.Stroke.Color = Color.Blue;
                    //On ajoute le calque de ce trajet à la carte
                    CalqueRoutes.Routes.Add(r);
                    gmap.Overlays.Add(CalqueRoutes);
                    gmap.UpdateRouteLocalPosition(r);   //Et on rafraichi

                    //pour le calcul de la distance
                    double distance = r.Distance;
                    //on laisse seulment 1 chiffre après la virgule
                    double distanceCut = Math.Truncate(distance * 100) / 100;
                    //on affiche la distance dans un messagebox
                    MessageBox.Show("Votre parcours mesure " + distanceCut + " Kilomètres", "Parcours", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception)
                {
                    MessageBox.Show("Un problème est survenur lors de la création de l'itinéraire. Peut-être une erreur dans l'adresse ?", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    brecentre_Click(sender, e);
                    BtnReinitialiser_Click(sender, e);
                }
            }
            itineraire.Dispose();// on détruit la form

            //***********************************************
        }

        //Pour mettre un marqueur sur la carte
        public void Marque_adresse(double Latitude, double Longitude, string Adresse)
        {
            

            GMarkerGoogle marqueur = new GMarkerGoogle(new PointLatLng(Latitude, Longitude), GMarkerGoogleType.red_dot);
            CalqueMarqueur.Markers.Add(marqueur);
            gmap.Overlays.Add(CalqueMarqueur);

            //On ajoute la légende du marqueurs (l'adresse ici)
            marqueur.ToolTip = new GMapToolTip(marqueur);
            marqueur.ToolTipText = Adresse;
        }

        //pour le bouton quitter
        private void BQuitter_Click(object sender, EventArgs e)
        {
            Close();
            Environment.Exit(0);
        }

        //Pour la recherche d'une adresse
        private void BtnRecherche_Click(object sender, EventArgs e)
        {
            //latitude et longitude de SOS (ne sert pas dans le programme)
            /*
            lat = 46.2119141;
            lon = 6.1379277;
            */
            
            //géocodage d'après adresse
            FDialogue Adresse = new FDialogue();

            //On affiche la boite de saisie d'adresse et on regarde le retour
            if (Adresse.ShowDialog() == DialogResult.OK)     //si c'est Ok
            {
                //Récup de l'adresse depuis la Form Adresse
                string AdrSaisie = Adresse.NvlleAdr.ToString();

                //on regarde si le panel 2 du split container a été désactivé
                //pour pouvoir afficher les coordonnées sur l'autre carte
                if (splitContainer2.Panel2Collapsed == true)
                {
                    
                }
                else
                {
                    //on récupère les coordonnées de l'adresse
                    gmap.SetPositionByKeywords(AdrSaisie);
                    //on affiche la latitude et la longitude dans la console
                    Console.WriteLine("Latitude :" + gmap.Position.Lat.ToString() + "Longitude :" + gmap.Position.Lng.ToString());
                    //on met le point sur la carte
                    Marque_adresse(gmap.Position.Lat, gmap.Position.Lng, AdrSaisie);
                }

                if(numcarte == 1)
                {
                    //on récupère les coordonnées de l'adresse
                    gmap.SetPositionByKeywords(AdrSaisie);
                    //on affiche la latitude et la longitude dans la console
                    Console.WriteLine("Latitude :" + gmap.Position.Lat.ToString() + "Longitude :" + gmap.Position.Lng.ToString());
                    //on met le point sur la carte
                    Marque_adresse(gmap.Position.Lat, gmap.Position.Lng, AdrSaisie);
                }

            }  //Fin d'on a saisie une adresse

            //On detruit la form
            Adresse.Dispose();
        }

        //pour réinitialiser l'affichage des points et des itinéraires
        private void BtnReinitialiser_Click(object sender, EventArgs e)
        {
            CalqueMarqueur.Clear();
            CalqueRoutes.Clear();
        }

        

        private void Button1_Click(object sender, EventArgs e)
        {
            
            //on détache la carte quand on appuie sur ce bouton
            DetacherCarte();
        }

        public void DetacherCarte(int position = 0)
        {
            Carte carte = new Carte();

            

            if (position == 0) //si la carte est attachée
            {
                //on change numcarte sur 1 car on passe sur l'autre carte
                numcarte = 1;

                //si il a plus d'un écran on affiche la fenetre en plein ecran sur le deuxieme écran
                if (ecran > 1)
                {
                    //on met la position de démarrage en manuel
                    carte.StartPosition = FormStartPosition.Manual;
                    //pour la mettre sur le second écran
                    carte.Bounds = Screen.AllScreens[1].Bounds;
                }

                //on affiche la form
                carte.Show();

                //on cache la carte de la première form
                splitContainer2.Panel2.Controls.Remove(gmap);

                //on enleve le panel2 du split container
                //splitContainer2.Panel2Collapsed = true;

                //on affiche la liste des médecins en plein écran à la place de la 
                //listMed.Dock = DockStyle.Fill;

                //on réduit la taille de la form principale
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(650, 115);
                

                //on désactive le bouton sur la form principale
                btnDetacherCarte.Enabled = false;
            }
            else if (position == 1)// si la carte est détachée
            {
                //on change numcarte sur 0 car on passe sur la carte principale
                numcarte = 0;

                //on met position sur 0
                position = 0;
            }

            //quand on attache la carte
            carte.FormClosed += new FormClosedEventHandler(Fermer_Carte);

        }
            void Fermer_Carte(object sender, FormClosedEventArgs e)
            {
                //on réactive le bouton pour détacher la carte
                btnDetacherCarte.Enabled = true;

                //on enleve le panel2 du split container
                //splitContainer2.Panel2Collapsed = false;

                //on remet la form principale en plein écran
                this.WindowState = FormWindowState.Maximized;

                //on remet la liste des médecins "normalement"
                //listMed.Dock = DockStyle.None;

                //on affiche la carte sur la form principale
                splitContainer2.Panel2.Controls.Add(gmap);
            }
        

        public DataSet RecupLocalisation ()
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Smart_Rapport"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);

            DataSet locTel = new DataSet();

            try
            {
                //on ouvre la connexion
                dbConnection.Open();

                SqlCommand cmd = dbConnection.CreateCommand();
                cmd.Connection = dbConnection;       //On passe les parametres query et connection

                //on définit la requette             
                string sqlstr0 = "";

                sqlstr0 += @"SELECT GPSTel.Latitude, GPSTel.Longitude
                            FROM GPSTel
                            WHERE Id=1";

                cmd.CommandText = sqlstr0;

                // Ajout des paramètres
                cmd.Parameters.Clear();
                

                locTel.Tables.Clear();               //On efface la table avant de la recharger
                locTel.Tables.Add("Localisation");      //on déclare une table pour cet ensemble de donnée  

                locTel.Tables["Localisation"].Load(cmd.ExecuteReader());       //on execute

            }
            catch(Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
            }
            finally
            {
                // Fermeture de la connexion
                if (dbConnection.State == System.Data.ConnectionState.Open)
                    dbConnection.Close();
            }

            if (dbConnection.State == System.Data.ConnectionState.Open)
                dbConnection.Close();

            return locTel;
        }

        public DataSet RecupListesMedecins()
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Horaire"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);      //Chaine de connection récupérée dans le app.config

            DataSet DSVoiture = new DataSet();

            try
            {
                //on ouvre la connexion
                dbConnection.Open();

                SqlCommand cmd = dbConnection.CreateCommand();
                cmd.Connection = dbConnection;       //On passe les parametres query et connection

                //on définit la requette             
                string sqlstr0 = "";

                if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                    sqlstr0 += @"select c.L_num_voiture as Num_Voiture, c.L_num_medecin as Num_Medecin, '     ' as NumVoitureTeksat, (m.Prenom + ' ' + m.nom) as NomMedecin 
                                from Creneaux c, medecins m where c.L_num_medecin= m.Id AND L_Date = @DateJ 
                                AND datepart(HOUR, GETDATE()) >= SUBSTRING(c.Fourchette_defaut,1,charindex('h',Fourchette_defaut)-1)
                                AND datepart(HOUR, GETDATE()) <= SUBSTRING(c.Fourchette_defaut,charindex('-',Fourchette_defaut)+1,(len(Fourchette_defaut)-charindex('-',Fourchette_defaut))-1)
                                GROUP BY L_num_voiture, L_num_medecin, m.Prenom, m.nom";
                else if (DateTime.Today.DayOfWeek == DayOfWeek.Tuesday)
                    sqlstr0 += @"select c.M_num_voiture as Num_Voiture, c.M_num_medecin as Num_Medecin, '     ' as NumVoitureTeksat, (m.Prenom + ' ' + m.nom) as NomMedecin 
                                from Creneaux c, medecins m where c.M_num_medecin= m.Id AND M_Date = @DateJ 
                                AND datepart(HOUR, GETDATE()) >= SUBSTRING(c.Fourchette_defaut,1,charindex('h',Fourchette_defaut)-1)
                                AND datepart(HOUR, GETDATE()) <= SUBSTRING(c.Fourchette_defaut,charindex('-',Fourchette_defaut)+1,(len(Fourchette_defaut)-charindex('-',Fourchette_defaut))-1)
                                GROUP BY M_num_voiture, M_num_medecin, m.Prenom, m.nom";
                else if (DateTime.Today.DayOfWeek == DayOfWeek.Wednesday)
                    sqlstr0 += @"select c.Me_num_voiture as Num_Voiture, c.Me_num_medecin as Num_Medecin, '     ' as NumVoitureTeksat, (m.Prenom + ' ' + m.nom) as NomMedecin
                                from Creneaux c, medecins m where c.Me_num_medecin= m.Id AND Me_Date = @DateJ 
                                AND datepart(HOUR, GETDATE()) >= SUBSTRING(c.Fourchette_defaut,1,charindex('h',Fourchette_defaut)-1)
                                AND datepart(HOUR, GETDATE()) <= SUBSTRING(c.Fourchette_defaut,charindex('-',Fourchette_defaut)+1,(len(Fourchette_defaut)-charindex('-',Fourchette_defaut))-1)
                                GROUP BY Me_num_voiture, Me_num_medecin, m.Prenom, m.nom";
                else if (DateTime.Today.DayOfWeek == DayOfWeek.Thursday)
                    sqlstr0 += @"select c.J_num_voiture as Num_Voiture, c.J_num_medecin as Num_Medecin, '     ' as NumVoitureTeksat, (m.Prenom + ' ' + m.nom) as NomMedecin
                                from Creneaux c, medecins m where c.J_num_medecin= m.Id AND J_Date = @DateJ 
                                AND datepart(HOUR, GETDATE()) >= SUBSTRING(c.Fourchette_defaut,1,charindex('h',Fourchette_defaut)-1)
                                AND datepart(HOUR, GETDATE()) <= SUBSTRING(c.Fourchette_defaut,charindex('-',Fourchette_defaut)+1,(len(Fourchette_defaut)-charindex('-',Fourchette_defaut))-1)
                                GROUP BY J_num_voiture, J_num_medecin, m.Prenom, m.nom";
                else if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
                    sqlstr0 += @"select c.V_num_voiture as Num_Voiture, c.V_num_medecin as Num_Medecin, '     ' as NumVoitureTeksat, (m.Prenom + ' ' + m.nom) as NomMedecin
                                from Creneaux c, medecins m where c.V_num_medecin= m.Id AND V_Date = @DateJ 
                                AND datepart(HOUR, GETDATE()) >= SUBSTRING(c.Fourchette_defaut,1,charindex('h',Fourchette_defaut)-1)
                                AND datepart(HOUR, GETDATE()) <= SUBSTRING(c.Fourchette_defaut,charindex('-',Fourchette_defaut)+1,(len(Fourchette_defaut)-charindex('-',Fourchette_defaut))-1)
                                GROUP BY V_num_voiture, V_num_medecin, m.Prenom, m.nom";
                else if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday)
                    sqlstr0 += @"select c.S_num_voiture as Num_Voiture, c.S_num_medecin as Num_Medecin, '     ' as NumVoitureTeksat, (m.Prenom + ' ' + m.nom) as NomMedecin
                                from Creneaux c, medecins m where c.S_num_medecin= m.Id AND S_Date = @DateJ
                                AND datepart(HOUR, GETDATE()) >= SUBSTRING(c.Fourchette_defaut,1,charindex('h',Fourchette_defaut)-1)
                                AND datepart(HOUR, GETDATE()) <= SUBSTRING(c.Fourchette_defaut,charindex('-',Fourchette_defaut)+1,(len(Fourchette_defaut)-charindex('-',Fourchette_defaut))-1)
                                GROUP BY S_num_voiture, S_num_medecin, m.Prenom, m.nom";
                else if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                    sqlstr0 += @"select c.D_num_voiture as Num_Voiture, c.D_num_medecin as Num_Medecin, '     ' as NumVoitureTeksat, (m.Prenom + ' ' + m.nom) as NomMedecin
                                from Creneaux c, medecins m where c.D_num_medecin= m.Id AND D_Date = @DateJ 
                                AND datepart(HOUR, GETDATE()) >= SUBSTRING(c.Fourchette_defaut,1,charindex('h',Fourchette_defaut)-1)
                                AND datepart(HOUR, GETDATE()) <= SUBSTRING(c.Fourchette_defaut,charindex('-',Fourchette_defaut)+1,(len(Fourchette_defaut)-charindex('-',Fourchette_defaut))-1)
                                GROUP BY D_num_voiture, D_num_medecin, m.Prenom, m.nom";

                cmd.CommandText = sqlstr0;

                // Ajout des paramètres
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("DateJ", DateTime.Today.ToString("dd-MM-yyyy"));

                DSVoiture.Tables.Clear();               //On efface la table avant de la recharger
                DSVoiture.Tables.Add("voiture");      //on déclare une table pour cet ensemble de donnée  

                DSVoiture.Tables["voiture"].Load(cmd.ExecuteReader());       //on execute

            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
            }
            finally
            {
                // Fermeture de la connexion
                if (dbConnection.State == System.Data.ConnectionState.Open)
                    dbConnection.Close();
            }

            if (dbConnection.State == System.Data.ConnectionState.Open)
                dbConnection.Close();

            return DSVoiture;
        }

        private void LsbMed_SelectedIndexChanged(object sender, EventArgs e)
        {
            //On efface le calque
            CalqueFlotte.Markers.Clear();

            for (int i = 0; i < ListePos.Tables["vehicule"].Rows.Count; i++)
            {
                //Ajout des marqueurs
                double Lat = Double.Parse(ListePos.Tables["vehicule"].Rows[i]["Latitude"].ToString());
                double Long = Double.Parse(ListePos.Tables["vehicule"].Rows[i]["Longitude"].ToString());

                if (lsbMed.SelectedItem.ToString() == ListePos.Tables["vehicule"].Rows[i]["NomMedecin"].ToString())
                {
                    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Lat, Long), GMarkerGoogleType.red);
                    //On ajoute la légende des marqueurs
                    marker.ToolTip = new GMapToolTip(marker);
                    marker.ToolTipText = ListePos.Tables["vehicule"].Rows[i]["NomMedecin"].ToString() + " " + ListePos.Tables["vehicule"].Rows[i]["Status_Voiture"].ToString() + " " +
                                                    "Vitesse: " + ListePos.Tables["vehicule"].Rows[i]["Vitesse"].ToString() + " Km/h";

                    CalqueFlotte.Markers.Add(marker);

                    //On recentre la carte sur le médecin sélectionné
                    gmap.Position = new PointLatLng(Lat, Long);
                }
                else
                {
                    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Lat, Long), GMarkerGoogleType.green);
                    //On ajoute la légende des marqueurs
                    marker.ToolTip = new GMapToolTip(marker);
                    marker.ToolTipText = ListePos.Tables["vehicule"].Rows[i]["NomMedecin"].ToString() + " " + ListePos.Tables["vehicule"].Rows[i]["Status_Voiture"].ToString() + " " +
                                                    "Vitesse: " + ListePos.Tables["vehicule"].Rows[i]["Vitesse"].ToString() + " Km/h";

                    CalqueFlotte.Markers.Add(marker);
                }
            }
        }

        private void RafraichiPositions()
        {
            ListePos.Clear();
            ListePos = GetPositionVehicules(Ticket, CustID, ListeVoiture, DateTime.Now.AddMinutes(-30));

            LocalisationTel.Clear();
            LocalisationTel = RecupLocalisation();

            //On efface les calques
            CalqueFlotte.Markers.Clear();
            CalqueRech1vehicule.Markers.Clear();
            CalqueRoutes.Routes.Clear();    //calque des itinéraires
            //CalqueGeoloc.Markers.Clear();  //calque pour les tests adresses


            for (int i = 0; i < ListePos.Tables["vehicule"].Rows.Count; i++)
            {
                //Ajout des marqueurs
                double Lat = Double.Parse(ListePos.Tables["vehicule"].Rows[i]["Latitude"].ToString());
                double Long = Double.Parse(ListePos.Tables["vehicule"].Rows[i]["Longitude"].ToString());


                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Lat, Long), GMarkerGoogleType.green);
                CalqueFlotte.Markers.Add(marker);
                gmap.Overlays.Add(CalqueFlotte);
                //On ajoute la légende des marqueurs
                marker.ToolTip = new GMapToolTip(marker);
                marker.ToolTipText = ListePos.Tables["vehicule"].Rows[i]["NomMedecin"].ToString() + " " + ListePos.Tables["vehicule"].Rows[i]["Status_Voiture"].ToString() + " " +
                                                    "Vitesse: " + ListePos.Tables["vehicule"].Rows[i]["Vitesse"].ToString() + " Km/h";
            }

            //TEST LOCALISATION DES TELEPHONES
            double lat = Double.Parse(LocalisationTel.Tables["Localisation"].Rows[0]["Latitude"].ToString());
            double lon = Double.Parse(LocalisationTel.Tables["Localisation"].Rows[0]["Longitude"].ToString());

            GMarkerGoogle marqueur = new GMarkerGoogle(new PointLatLng(lat, lon), GMarkerGoogleType.green);
            CalqueFlotte.Markers.Add(marqueur);
            gmap.Overlays.Add(CalqueFlotte);
            //On ajoute la légende des marqueurs
            marqueur.ToolTip = new GMapToolTip(marqueur);
            marqueur.ToolTipText = "Chez Dominique";
        }




        //*****************************geoloc des voitures************
        public DataSet GetPositionVehicules(string Ticket, string ID, DataSet DSVoiture, DateTime DateJ)
        {
            //on formate la date d'une certaine manière
            string FiltreDate = DateJ.ToString("yyyy-MM-dd_HH:mm:ss");

            //On defini en paramètre la liste des voitures à auditer
            string VehiculesID = "";
            for (int i = 0; i < DSVoiture.Tables["voiture"].Rows.Count; i++)
            {
                VehiculesID += DSVoiture.Tables["voiture"].Rows[i][2].ToString() + ",";
            }

            //On enlève la derniere ","
            VehiculesID = VehiculesID.TrimEnd(',');

            HttpWebRequest requete = (HttpWebRequest)WebRequest.Create("http://www.gps10.teksat.fr/webservices/map/get-vehicles-pos.jsp?tck=" + Ticket + "&custID=" + ID
                                                                                                                                                + "&v_ids=" + VehiculesID + "&dateFilter=" + FiltreDate);
            HttpWebResponse reponse = (HttpWebResponse)requete.GetResponse();         //on execute la requette

            string resultat = null;

            DataSet ResultDataSet = new DataSet("DataVehicule");              //Dataset que l'on va retourner            
            DataTable Voiture = ResultDataSet.Tables.Add("vehicule");     //On créer une table          
            Voiture.Columns.Add("Num_Voiture", typeof(string));        //et ses colonnes     
            Voiture.Columns.Add("Latitude", typeof(double));
            Voiture.Columns.Add("Longitude", typeof(double));
            Voiture.Columns.Add("Status_Voiture", typeof(string));
            Voiture.Columns.Add("Vitesse", typeof(double));
            Voiture.Columns.Add("DateJ", typeof(DateTime));
            Voiture.Columns.Add("NomMedecin", typeof(string));

            if (reponse.StatusCode == HttpStatusCode.OK)        //Si le webservice à répondu
            {
                //On lit les données du flux de réponse
                Stream repStream = reponse.GetResponseStream();

                string stringTemp = null;
                int cpt = 0;
                byte[] buf = new byte[8192];

                do
                {
                    //on charge le buffer
                    cpt = repStream.Read(buf, 0, buf.Length);

                    //S'il on reçoit quelque chose, on le stocke
                    if (cpt != 0)
                    {
                        //Convertion des byte en texte ASCII
                        stringTemp = Encoding.GetEncoding("ISO-8859-1").GetString(buf, 0, cpt);

                        resultat = resultat + stringTemp;
                    }
                }
                while (cpt > 0);
            }

            XmlDocument ResultXML = new XmlDocument();
            ResultXML.LoadXml(resultat);            //On charge le résultat dans un document XML

            reponse.Close();

            XmlNode node = ResultXML.DocumentElement;

            try
            {
                XmlNodeList listNoeud = node.SelectNodes("vehicle_pos");


                for (int i = 0; i < DSVoiture.Tables["voiture"].Rows.Count; i++)
                {

                    String Recherche = "Non Trouvé";

                    foreach (XmlNode objNoeud in listNoeud)
                    {
                        if (DSVoiture.Tables["voiture"].Rows[i]["NumVoitureTeksat"].ToString() == objNoeud.Attributes["v_id"].Value)
                        {
                            Recherche = "Trouvé";

                            //on stock les valeurs dans le dataset
                            DataRow row;
                            row = Voiture.NewRow();
                            row["Num_Voiture"] = objNoeud.Attributes["v_id"].Value;
                            row["Latitude"] = Double.Parse(objNoeud.Attributes["lat"].Value, CultureFr);
                            row["Longitude"] = Double.Parse(objNoeud.Attributes["lng"].Value, CultureFr);

                            string StatusV = objNoeud.Attributes["status_label"].Value;
                            if (StatusV.IndexOf("-", 0) != -1)
                                row["Status_Voiture"] = StatusV.Substring(0, StatusV.IndexOf("-", 0));
                            else row["Status_Voiture"] = objNoeud.Attributes["status_label"].Value;

                            row["Vitesse"] = Double.Parse(objNoeud.Attributes["speed"].Value, CultureFr);
                            row["DateJ"] = objNoeud.Attributes["data_time"].Value;
                            row["NomMedecin"] = DSVoiture.Tables["voiture"].Rows[i]["NomMedecin"].ToString();             //on complète le champs NomMedecin dans le dataset
                            Voiture.Rows.Add(row);
                        }
                    }

                    //Si on a pas de retour de Tecksat => véhicule hors couverture
                    if (Recherche == "Non Trouvé")
                    {

                        //on stock les valeurs dans le dataset
                        DataRow row;
                        row = Voiture.NewRow();
                        row["Num_Voiture"] = DSVoiture.Tables["voiture"].Rows[i]["NumVoitureTeksat"].ToString();
                        row["Latitude"] = 46.210346;
                        row["Longitude"] = 6.136647;        ////On positionne le pointeur (SOS)

                        row["Status_Voiture"] = "Hors Couverture";

                        row["Vitesse"] = 0;
                        row["DateJ"] = DateJ;
                        row["NomMedecin"] = DSVoiture.Tables["voiture"].Rows[i]["NomMedecin"].ToString();             //on complète le champs NomMedecin dans le dataset
                        Voiture.Rows.Add(row);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            reponse.Close();

            Console.WriteLine(ResultDataSet);
            return ResultDataSet;
        }


        private void RafraichiListeMed()
        {
            //rechargement de la liste des médecins et de leur véhicules, récup d'un nouveau ticket tecksat
            ListeMed.Clear();
            ListeMed = RecupListesMedecins();       //Medecins qui bossent

            //Rechargement de la liste
            lsbMed.BeginUpdate();          //suspend le rafraîchissement automatique

            lsbMed.Items.Clear();          //On efface la liste des medecins

            for (int i = 0; i < ListeMed.Tables["Voiture"].Rows.Count; i++)
            {
                lsbMed.Items.Add(ListeMed.Tables["Voiture"].Rows[i]["NomMedecin"].ToString());
            }

            lsbMed.EndUpdate();        // reprend le rafraîchissement

            //Puis on va chercher un identifiant teksat
            Ticket = GetTicket(CustID, "sos", "medecin2655");

            ListeVoiture.Clear();
            ListeVoiture = GetIdVehicule(Ticket, CustID, ListeMed);
        }


        //*********************OBTENTION D'UN TICKET CHEZ TECKSAT******
        public string GetTicket(string ID, string username, string pass)
        {
            //Préparation de la requette           
            HttpWebRequest requete = (HttpWebRequest)WebRequest.Create("http://www.gps10.teksat.fr/webservices/map/get-ticket.jsp?custID=" + ID + "&username=" + username + "&pw=" + pass);

            //on execute la requette
            HttpWebResponse reponse = (HttpWebResponse)requete.GetResponse();

            string resultat = null;

            if (reponse.StatusCode == HttpStatusCode.OK)        //Si le webservice à répondu
            {
                //On lit les données du flux de réponse
                Stream repStream = reponse.GetResponseStream();


                string stringTemp = null;
                int cpt = 0;
                byte[] buf = new byte[8192];

                do
                {
                    //on charge le buffer
                    cpt = repStream.Read(buf, 0, buf.Length);

                    //S'il on reçoit quelque chose, on le stocke
                    if (cpt != 0)
                    {
                        //Convertion des byte en texte ASCII
                        stringTemp = Encoding.GetEncoding("ISO-8859-1").GetString(buf, 0, cpt);

                        resultat = resultat + stringTemp;
                    }
                }
                while (cpt > 0);
            }

            reponse.Close();

            Console.WriteLine("Le resultat est: " + resultat);
            return resultat;
        }

        //****************************Affectation de l'id teksat des voitures**********
        public DataSet GetIdVehicule(string Ticket, string ID, DataSet DSVoiture)
        {
            //Récup de la flotte de véhicules************************
            HttpWebRequest requeteFlotte = (HttpWebRequest)WebRequest.Create("http://www.gps10.teksat.fr/webservices/map/get-vehicles.jsp?tck=" + Ticket + "&custID=" + ID);

            //on execute la requette
            HttpWebResponse reponseFlotte = (HttpWebResponse)requeteFlotte.GetResponse();

            string resultatFlotte = null;

            if (reponseFlotte.StatusCode == HttpStatusCode.OK)        //Si le webservice à répondu
            {
                //On lit les données du flux de réponse
                Stream repStream = reponseFlotte.GetResponseStream();

                string stringTemp = null;
                int cpt = 0;
                byte[] buf = new byte[8192];

                do
                {
                    //on charge le buffer
                    cpt = repStream.Read(buf, 0, buf.Length);

                    //S'il on reçoit quelque chose, on le stocke
                    if (cpt != 0)
                    {
                        //Convertion des byte en texte ASCII
                        stringTemp = Encoding.GetEncoding("ISO-8859-1").GetString(buf, 0, cpt);

                        resultatFlotte = resultatFlotte + stringTemp;
                    }
                }
                while (cpt > 0);
            }

            XmlDocument ResultFlotteXML = new XmlDocument();
            ResultFlotteXML.LoadXml(resultatFlotte);            //On charge le résultat dans un document XML

            reponseFlotte.Close();

            XmlNode nodeFlotte = ResultFlotteXML.DocumentElement;

            DataSet ListeIDVoiture = DSVoiture.Clone();

            try
            {
                XmlNodeList listNoeud = nodeFlotte.SelectNodes("vehicle");

                foreach (XmlNode objNoeud in listNoeud)
                {
                    for (int i = 0; i < DSVoiture.Tables["voiture"].Rows.Count; i++)
                    {
                        if (objNoeud.Attributes["special_id"].Value == "Voiture " + DSVoiture.Tables["voiture"].Rows[i][0].ToString())
                        {
                            //On rempli le DataSet                            
                            DataRow row;
                            row = ListeIDVoiture.Tables["voiture"].NewRow();                //Nvlle enregistrement
                            row["Num_Voiture"] = DSVoiture.Tables["voiture"].Rows[i][0];
                            row["Num_Medecin"] = DSVoiture.Tables["voiture"].Rows[i][1];
                            row["NumVoitureTeksat"] = objNoeud.Attributes["id"].Value.ToString();                       //on complète le champs NumVoitureTeksat dans le dataset
                            row["NomMedecin"] = DSVoiture.Tables["voiture"].Rows[i][3];
                            ListeIDVoiture.Tables["voiture"].Rows.Add(row);                 //On ajoute l'enregistrement
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ListeIDVoiture;
        }
    }
}
