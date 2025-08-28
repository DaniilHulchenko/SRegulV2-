using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FCarto : Form
    {                 
        public static string Test;
        public static GMapControl gMapControl1 = new GMapControl();        
        
        public static GMapOverlay CalqueVisitesAtt = new GMapOverlay("CalqueVisites");
        public static GMapOverlay CalqueVisiteSelect = new GMapOverlay("CalqueVisiteSelec");
        public static GMapOverlay CalqueMedecinSelect = new GMapOverlay("CalqueMedecinSelec");
       // public static GMapOverlay CalqueMedecinPause = new GMapOverlay("CalqueMedecinPause");
        public static GMapOverlay CalqueGPSMedecins = new GMapOverlay("CalqueGPSMedecins");
      //  public static GMapOverlay CalqueLignesPreattrib = new GMapOverlay("CalqueLignesPreattrib");
        public static GMapOverlay CalqueToutesVisitesMed = new GMapOverlay("CalqueLignesVisites");
     
        public static GMapRoute CalqueLigne = new GMapRoute("CalqueLigne");     
       
        public static PointLatLng ptNonGeocode = new PointLatLng();         //Point par défaut pour les adresses non géocodées  
        public static PointLatLng ptCentreCarte = new PointLatLng();        //Point pour centrer la carte au lancement
        public static PointLatLng ptCentrale = new PointLatLng();        //Point de la centrale
        
        public static double LatN;     //On détermine le bornage autour de genève
        public static double LatS;
        public static double LngW;
        public static double LngE;

        public static string rafraichiCarte = "KO";
        public static int TimerCarte = 5;

        public static DataTable dtParam;   //Pour stocker les paramètres 
        public static DataTable dtVisiteAttrib = new DataTable();
        public static DataTable dtVisiteAttent = new DataTable();

        public FCarto()
        {
                        
            InitializeComponent();
            
            //On créer un gmap Manuellement car il faut qu'il soit en public static!
            splitContainer1.Panel2.Controls.Add(gMapControl1);
            gMapControl1.Dock = DockStyle.Fill;
            gMapControl1.CanDragMap = true;
            gMapControl1.GrayScaleMode = false;
            gMapControl1.MarkersEnabled = true;    //
            gMapControl1.MaxZoom = 18;
            gMapControl1.MinZoom = 6;
            gMapControl1.MouseWheelZoomEnabled = true;
            gMapControl1.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            gMapControl1.NegativeMode = false;
            gMapControl1.PolygonsEnabled = true;
            gMapControl1.RoutesEnabled = true;
            gMapControl1.ScaleMode = ScaleModes.Integer;
            gMapControl1.ShowTileGridLines = false;
            gMapControl1.AutoSizeMode = AutoSizeMode.GrowOnly;
            gMapControl1.Zoom = 6;
            //déclaration de l'évenement OnMarkerClick
            gMapControl1.OnMarkerClick += new MarkerClick(gMapControl1_OnMarkerClick);

            //On défini le point comme séparateur décimal
            CultureInfo CultureFr = new CultureInfo("fr-CH");
            CultureFr.NumberFormat.NumberDecimalSeparator = ".";
                        
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;    //On fixe la taille du panel1 (ou il y a les boutons)                                

            timer1.Enabled = true;
           // timer1.Enabled = false;

            //Pour la securité
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
      

        private void FCarto_Load(object sender, EventArgs e)
        {
            //Initialisation de la carte            
            //Chargement des coordonnées géographiques            
            dtParam = FonctionsAppels.ChargeParam();
         
            if (dtParam.Rows.Count > 0)
            {                
                //Zone géographique de géocodage
                LatN = double.Parse(dtParam.Rows[0]["ZGeoLatN"].ToString());
                LatS = double.Parse(dtParam.Rows[0]["ZGeoLatS"].ToString());
                LngW = double.Parse(dtParam.Rows[0]["ZGeoLngW"].ToString());
                LngE = double.Parse(dtParam.Rows[0]["ZGeoLngE"].ToString());

                //Point central de la carte

                ptCentreCarte.Lat = double.Parse(dtParam.Rows[0]["CentreCarteLat"].ToString());
                ptCentreCarte.Lng = double.Parse(dtParam.Rows[0]["CentreCarteLng"].ToString());

                //Position de la centrale
                ptCentrale.Lat = double.Parse(dtParam.Rows[0]["CentraleLat"].ToString());
                ptCentrale.Lng = double.Parse(dtParam.Rows[0]["CentraleLng"].ToString()); 

                //Position de la Zone pour adresse non Géocodées
                ptNonGeocode.Lat = double.Parse(dtParam.Rows[0]["ZStockLat"].ToString());
                ptNonGeocode.Lng = double.Parse(dtParam.Rows[0]["ZStockLng"].ToString()); 

                //Timer de rafraichissement de la carte
                TimerCarte = int.Parse(dtParam.Rows[0]["TimerCarte"].ToString()) * 1000;
                timer1.Interval = TimerCarte;
            }

            //*****Gestion des écrans ******
            Screen[] screens = Screen.AllScreens;           //On recupère tout les écrans
            int NbEcran = Screen.AllScreens.Length;         //on récupère le nombre d'ecran
            int EcranSecondaire = 0;

            if (NbEcran > 1)
            {
                //on regarde sur quel écran est la form principale
                var FormPrincipale = Application.OpenForms["Form1"];
                if (FormPrincipale != null)
                {
                    for (int i = 0; i < NbEcran; i++)
                    {
                        Point formTopLeft = new Point(FormPrincipale.Left, FormPrincipale.Top);   //On récupère le point haut à gauche

                        if (!screens[i].WorkingArea.Contains(formTopLeft))    //Pour pouvoir localiser l'écran
                        {
                            EcranSecondaire = i;          //Si ce n'est PAS celui là....c'est qu'il est libre!
                        }
                        // else
                        //   MessageBox.Show("Form principale sur l'écran " + screens[i].Primary + screens[i].DeviceName);                       
                    }
                }

                //on met la position de démarrage en manuel
                this.StartPosition = FormStartPosition.Manual;

                //On affiche la forme sur le 2eme écran (celui ou n'est pas la form principale)               
                this.Location = Screen.AllScreens[EcranSecondaire].WorkingArea.Location;
                this.WindowState = FormWindowState.Maximized;    //Met en plein écran sur le 2ème écran
            }
            else  //Pas de 2eme écran
            {
                //on met la position de démarrage en manuel
                this.StartPosition = FormStartPosition.Manual;

                this.Location = new Point(0, 0);
            }


            //On affiche la carte sur le 2eme écran s'il y en a un          
           /* if (ecran > 1)
            {
                //on met la position de démarrage en manuel
                this.StartPosition = FormStartPosition.Manual;
                
                //pour la mettre sur le second écran
                this.Bounds = Screen.AllScreens[1].Bounds;
                this.WindowState = FormWindowState.Maximized;    //Met en plein écran sur le 2ème écran
            }*/


            //On indique la validation automatique des demandes d'acceptation des certificats
            SSLValidator.OverrideValidation();


            //On désactive le proxy hin
            FonctionsCTI.DesactiveProxyHIN();

            //On choisi ici la carte (open street est rapide)               
            switch (Form1.TypeCarte)
            {
                case 1:                     
                        gMapControl1.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
                        GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
                        break;
                
                case 2: GoogleMapProvider.Instance.Version = "m,traffic@336000000";
                        gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
                        GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly; break;

                case 3: gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
                        GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly; break;

                default: gMapControl1.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance; 
                         GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache; break;
            }
                                                          
                                    
            //On centre la carte sur un point qui à l'air d'être le centre du canton...                 
            gMapControl1.Position = ptCentreCarte;                           

            gMapControl1.Zoom = 12;                             //Ainsi que le Zoom

            //Le bouton gauche souris sert à glisser la carte
            gMapControl1.DragButton = MouseButtons.Left;

            //on enlève la petite croix rouge au milieu de l'écran
            gMapControl1.ShowCenter = false;
           
            //GeoCoderStatusCode retour = SetPositionByKeywords("Rue Louis-Favre, 43, 1201, Genève, Suisse");

            //gMapControl1.SuspendLayout();
            //On affiche les points
            Rafraichi();

            //On résactive le proxy hin
            FonctionsCTI.ReactiveProxyHIN();
        }


        #region rafraichissement
        public void Rafraichi()
        {
            //On affiche le pictogramme de rafraichissement                       
            pictureBox1.Visible = true;
            pictureBox1.Enabled = true;

            dtVisiteAttent.Clear();
            dtVisiteAttrib.Clear();

            backgroundWorker1.RunWorkerAsync();
        }

        public void bRafraichi_Click(object sender, EventArgs e)
        {            
            Rafraichi();                                           
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //On rafraichi les données                               
            dtVisiteAttent = RegeocodeVisitesAttente();
            dtVisiteAttrib = RegeocodeVisiteAttibuee();           
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //On redessine la carte
            RedessineCarte(dtVisiteAttent, dtVisiteAttrib);            
            
            pictureBox1.Enabled = false;
            pictureBox1.Visible = false;          
        }


        //On rafraichi les visites
        public static DataTable RegeocodeVisitesAttente()
        {
            //on reinitialise les valeurs de "Stockage" des visites non géocodées
            ptNonGeocode.Lat = double.Parse(dtParam.Rows[0]["ZStockLat"].ToString());
            ptNonGeocode.Lng = double.Parse(dtParam.Rows[0]["ZStockLng"].ToString());  
            
            //On charge la listes des visites en attente et Pré-attribués
            DataTable dtVisiteEnAttentes = new DataTable();
            dtVisiteEnAttentes = FonctionsAppels.ChargeListeAppelsNonAttribues();                     
              
            //Création d'1 colonnes de résultat de géo codage           
            DataColumn dcGeocode = new DataColumn("Geocode", typeof(string));
           
            //Puis ajout de celles ci au datatable           
            dtVisiteEnAttentes.Columns.Add(dcGeocode);

            //Récupération des coordonnées des adresses
            for (int i = 0; i < dtVisiteEnAttentes.Rows.Count; i++)
            {                
                PointLatLng pt = new PointLatLng();
                pt.Lat = double.Parse(dtVisiteEnAttentes.Rows[i]["Latitude"].ToString());
                pt.Lng = double.Parse(dtVisiteEnAttentes.Rows[i]["Longitude"].ToString());

                if (pt != ptNonGeocode)                
                {                    
                    dtVisiteEnAttentes.Rows[i]["Geocode"] = "OK";
                }
                else
                {
                    dtVisiteEnAttentes.Rows[i]["Latitude"] = ptNonGeocode.Lat;
                    dtVisiteEnAttentes.Rows[i]["Longitude"] = ptNonGeocode.Lng;
                    dtVisiteEnAttentes.Rows[i]["Geocode"] = "KO";

                    //On incrémente la longitude pour décaler les points
                    ptNonGeocode.Lng += 0.001;
                }
            }           

            return dtVisiteEnAttentes;
        }


        public static DataTable RegeocodeVisiteAttibuee()
        {
            //on reinitialise les valeurs de "Stockage" des visites non géocodées
            ptNonGeocode.Lat = double.Parse(dtParam.Rows[0]["ZStockLat"].ToString());
            ptNonGeocode.Lng = double.Parse(dtParam.Rows[0]["ZStockLng"].ToString());  
            
            //Puis on affiche les visites attribuées
            DataTable dtVisiteAttribuees = new DataTable();
            dtVisiteAttribuees = FonctionsAppels.ChargeListeAppelsAttribCarto();

            //Création de 3 colonnes de coordonnées            
            DataColumn dcGeocode = new DataColumn("Geocode", typeof(string));

            //Puis ajout de celles ci au datatable         
            dtVisiteAttribuees.Columns.Add(dcGeocode);

            for (int i = 0; i < dtVisiteAttribuees.Rows.Count; i++)
            {               
                PointLatLng pt = new PointLatLng();                
                pt.Lat = double.Parse(dtVisiteAttribuees.Rows[i]["Latitude"].ToString());
                pt.Lng = double.Parse(dtVisiteAttribuees.Rows[i]["Longitude"].ToString());

                if (pt != ptNonGeocode)    
                {                    
                    dtVisiteAttribuees.Rows[i]["Geocode"] = "OK";
                }
                else
                {
                    dtVisiteAttribuees.Rows[i]["Latitude"] = ptNonGeocode.Lat;
                    dtVisiteAttribuees.Rows[i]["Longitude"] = ptNonGeocode.Lng;
                    dtVisiteAttribuees.Rows[i]["Geocode"] = "KO";
                    
                    //On incrémente la longitude pour décaler les points
                    ptNonGeocode.Lng += 0.001;
                }              
            }
                      
            return dtVisiteAttribuees;
        }


        //On rafraichi les visites
        public static void RedessineCarte(DataTable dtVisiteAttente, DataTable dtVisiteAttribuee)
        {            
            //On efface le calque des visites           
            CalqueVisitesAtt.Clear();

            gMapControl1.Overlays.Clear();

            //On charge la listes des visites en attente 
            for (int i = 0; i < dtVisiteAttente.Rows.Count; i++)
            {
                //Ajout des marqueurs                
                PointLatLng pt = new PointLatLng();
                pt.Lat = Double.Parse(dtVisiteAttente.Rows[i]["Latitude"].ToString());
                pt.Lng = Double.Parse(dtVisiteAttente.Rows[i]["Longitude"].ToString());

                //On prépare un nouveau marqueur
                GMarkerGoogle marqueur;

                if (dtVisiteAttente.Rows[i]["Geocode"].ToString() == "OK")                    
                    marqueur = new GMarkerGoogle(pt, new Bitmap(SRegulV2.Properties.Resources.marqueBleu));
                else
                {
                    marqueur = new GMarkerGoogle(pt, new Bitmap(SRegulV2.Properties.Resources.bouee));   //Mettre une bouée!!!
                }

                CalqueVisitesAtt.Markers.Add(marqueur);

                //On ajoute la légende du marqueurs (l'adresse ici)
                marqueur.ToolTip = new GMapToolTip(marqueur);
                marqueur.ToolTipText = dtVisiteAttente.Rows[i]["NomPrenom"].ToString() + "\n" + dtVisiteAttente.Rows[i]["Adresse"].ToString();
                // marqueur.ToolTipMode = MarkerTooltipMode.Always;
            }

            //Puis on affiche les visites attribuées           
            for (int i = 0; i < dtVisiteAttribuee.Rows.Count; i++)
            {
                //Ajout des marqueurs               
                PointLatLng pt = new PointLatLng();
                pt.Lat = Double.Parse(dtVisiteAttribuee.Rows[i]["Latitude"].ToString());
                pt.Lng = Double.Parse(dtVisiteAttribuee.Rows[i]["Longitude"].ToString());

                //On prépare un nouveau marqueur
                GMarkerGoogle marqueur = null;

                if (dtVisiteAttribuee.Rows[i]["Geocode"].ToString() == "OK")       
                {
                    //En fonction du Status de la visite
                    switch (dtVisiteAttribuee.Rows[i]["StatusGarde"].ToString())
                    {
                        case "Attribuée": marqueur = new GMarkerGoogle(pt, new Bitmap(SRegulV2.Properties.Resources.marqueJaune)); break;
                        case "Acquitée": marqueur = new GMarkerGoogle(pt, new Bitmap(SRegulV2.Properties.Resources.marqueOrange)); break;   //En faire un orange
                        case "Visite": marqueur = new GMarkerGoogle(pt, new Bitmap(SRegulV2.Properties.Resources.marqueVert)); break;

                        //En attendant la correction
                        case "Disponible": marqueur = new GMarkerGoogle(pt, new Bitmap(SRegulV2.Properties.Resources.marqueVert)); break;
                    }
                }
                else
                {
                    marqueur = new GMarkerGoogle(pt, new Bitmap(SRegulV2.Properties.Resources.bouee));   //une bouée pour les non géocodés
                }

                CalqueVisitesAtt.Markers.Add(marqueur);

                //On ajoute la légende du marqueurs (l'adresse ici)
                marqueur.ToolTip = new GMapToolTip(marqueur);
                marqueur.ToolTipText = dtVisiteAttribuee.Rows[i]["NomPrenom"].ToString() + "\n" + dtVisiteAttribuee.Rows[i]["Adresse"].ToString();
            }

            //Pour SOS
            GMarkerGoogle marqueurSos = new GMarkerGoogle(ptCentrale, new Bitmap(SRegulV2.Properties.Resources.centraleSos));

            CalqueVisitesAtt.Markers.Add(marqueurSos);

            //On ajoute la légende du marqueur SOS (l'adresse ici)
            marqueurSos.ToolTip = new GMapToolTip(marqueurSos);
            marqueurSos.ToolTipText = "SOS Médecins \n Centrale";

            //On ajoute enfin le calque à la carte
            gMapControl1.Overlays.Add(CalqueVisitesAtt);

            //Puis on rafraichi la position des médecins qui sont "En pause" et "Disponible"
            RafraichiPoseMeds();
            
            //On efface le popup de la visite
            EffacePopupVisite();

            //On efface aussi les lignes des routes pour les pre attribuées
            //EffaceLignesPR();
            EffaceLignesToutesVisites();

            CalqueVisitesAtt.IsVisibile = false;
            CalqueVisitesAtt.IsVisibile = true;
        }
        #endregion
      
      
        //On rafraichi les positions des médecins qui sont soit en pause, (Qd Géoloc par Smartphone: rajouter aussi  qd visite acquité et libre)
        public static void RafraichiPoseMeds()
        {                           
            //Géolocalisation des médecins
            //On efface le calque Medecin selectionné et en garde
            CalqueMedecinSelect.Clear();
            CalqueGPSMedecins.Clear();
           
            //On recupères les médecins en garde
            DataTable dtMedecinsGarde = FonctionsAppels.ChargeListeMedecins("en garde2");

            //On declare un nouveau point pour pour décaler à la centrale
            PointLatLng ptAlaCentrale = new PointLatLng();
            ptAlaCentrale.Lat = ptCentrale.Lat;
            ptAlaCentrale.Lng = ptCentrale.Lng;          

            for (int i = 0; i < dtMedecinsGarde.Rows.Count; i++)
            {
                PointLatLng pointMed = new PointLatLng();
                int Vitesse = 0;

                //ici recup des coord. et tester, voir si elles sont dans le cadre, si ok les afficher, sinon centrale.
                pointMed.Lat = Double.Parse(dtMedecinsGarde.Rows[i]["Lat"].ToString());
                pointMed.Lng = Double.Parse(dtMedecinsGarde.Rows[i]["Lng"].ToString());
                Vitesse = int.Parse(dtMedecinsGarde.Rows[i]["Vitesse"].ToString());

                //On vérifie que les coordonnées restent bien dans la région définie en paramètre
                if (VerifGeocodage(pointMed) == "KO")
                {
                    pointMed = ptAlaCentrale;   //Si c'est pas bon, on met la centrale

                    //On incrémente la longitude pour décaler les points
                    ptAlaCentrale.Lng += 0.001;
                }                         

                if (dtMedecinsGarde.Rows[i]["StatusGarde"].ToString() == "En pause")
                {                                       
                    GMarkerGoogle marqueur = new GMarkerGoogle(pointMed, new Bitmap(SRegulV2.Properties.Resources.cafe2));

                    CalqueGPSMedecins.Markers.Add(marqueur);

                    //On ajoute la légende du marqueurs (l'adresse ici)
                    marqueur.ToolTip = new GMapToolTip(marqueur);
                    marqueur.ToolTipText = dtMedecinsGarde.Rows[i]["NomMed"].ToString() + "\n En pause";                    
                }
                else  //On affiche sa localisation
                {
                    GMarkerGoogle marqueur = new GMarkerGoogle(pointMed, new Bitmap(SRegulV2.Properties.Resources.voiture));

                    CalqueGPSMedecins.Markers.Add(marqueur);

                    //On ajoute la légende du marqueurs (l'adresse ici)
                    marqueur.ToolTip = new GMapToolTip(marqueur);
                    marqueur.ToolTip = new GMapToolTip(marqueur);
                    marqueur.ToolTipText = dtMedecinsGarde.Rows[i]["NomMed"].ToString() + "\n " + dtMedecinsGarde.Rows[i]["StatusGarde"].ToString();                    
                }                
            }
               
            //On ajoute enfin le calque à la carte
            gMapControl1.Overlays.Add(CalqueGPSMedecins);

            //Pour rafraichir la carte
            CalqueGPSMedecins.IsVisibile = false;
            CalqueGPSMedecins.IsVisibile = true;
           
        }


        //On se recentre sur le medecin selectionné (lorsqu'il est en visite)
        public static void FocusMedecin(DataRow RowVEnCour, DataTable dtVPreAtt, string NomMed)
        {           
            //On efface le calque Medecin et les pré-attribuées         
         /*   CalqueMedecinSelect.Clear();
                                                         
            string Adresse = RowVEnCour["Adresse"].ToString() + ", " + RowVEnCour["CodePostal"].ToString()
                            + ", " + RowVEnCour["Commune"].ToString() + ", " + RowVEnCour["Pays"].ToString();

           
            PointLatLng pt = GeocodeAdresse(Adresse);
            
            //On prépare un nouveau marqueur
            GMarkerGoogle marqueur;

            if (VerifGeocodage(pt) == "OK")
            {
                //Ici SetPositionByKeywords car on veut se recentrer dessus    
                gMapControl1.SetPositionByKeywords(Adresse);

                marqueur = new GMarkerGoogle(pt, new Bitmap(SRegulV2.Properties.Resources.marqueRouge));
            }
            else    //Non géocodé
            {
                marqueur = new GMarkerGoogle(ptNonGeocode, new Bitmap(SRegulV2.Properties.Resources.bouee));

                //On incrémente la longitude pour décaler les points
               // ptNonGeocode.Lng += 0.001;
            }
            
            CalqueMedecinSelect.Markers.Add(marqueur);

            //On ajoute la légende du marqueurs (l'adresse ici)
            marqueur.ToolTip = new GMapToolTip(marqueur);
            marqueur.ToolTipText = NomMed + "\n" + Adresse;            
           
            //On ajoute enfin le calque à la carte
            gMapControl1.Overlays.Add(CalqueMedecinSelect);

            //Maintenant on marque les visites pré-attribuées par des lignes commençant du médecin aux visites les unes derrière les autres
            
            //Récup du point d'origine (position de la visite en cours du médecin)
            PointLatLng Origine = gMapControl1.Position;
            
            //on efface les précédentes lignes
            CalqueLignesPreattrib.Clear();
            CalqueLigne.Clear();
            //*****************************
             GMapOverlay CalqueItineraire = new GMapOverlay();


            //******************************

            //On choisi le style de trait ainsi que la couleur
            Pen myPen = new Pen(Color.Red, 2.0F);
            myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;

            CalqueLigne.Stroke = myPen; //on affecte le style de trait au calque           

            for (int i = 0; i < dtVPreAtt.Rows.Count; i++)
            {
                //On géocode les adresses
                string Adr = dtVPreAtt.Rows[i]["Adresse"].ToString() + ", " + dtVPreAtt.Rows[i]["CodePostal"].ToString()
                            + ", " + dtVPreAtt.Rows[i]["Commune"].ToString() + ", " + dtVPreAtt.Rows[i]["Pays"].ToString();

                PointLatLng Destination = GeocodeAdresse(Adr);
                
                //Maintenant on ajoute les points
                CalqueLigne.Points.Add(Origine);

                //Est ce qu'on peut géocoder l'adresse de visite?
                if (VerifGeocodage(Destination) == "OK")
                {
                    CalqueLigne.Points.Add(Destination);

                    //Puis on prend cette destination pour l'origine de la prochaine adresse
                    Origine = Destination;
                }
                else
                {
                    //Dans le lac
                    CalqueLigne.Points.Add(ptNonGeocode);
                    //On incrémente la longitude pour décaler les points
                    ptNonGeocode.Lng += 0.001;
                    //Puis on prend cette destination pour l'origine de la prochaine adresse
                    Origine = ptNonGeocode;
                }
             
            }
         
            //On ajoute les calques à la carte
            CalqueLignesPreattrib.Routes.Add(CalqueLigne);
            gMapControl1.Overlays.Add(CalqueLignesPreattrib);

            //on rafraichi la carte
            gMapControl1.UpdateRouteLocalPosition(CalqueLigne);

            //Pour rafraichir la carte
            //CalqueLignesPreattrib.Clear();
           // CalqueLigne.IsVisible = false;
           // CalqueLigne.IsVisible = true; 
            // CalqueVisitesAtt.IsVisibile = false; 
            // CalqueVisitesAtt.IsVisibile = true;          */           
        }


        //On se recentre sur le medecin selectionné (lorsqu'il est en visite)
        public static void FocusMedecinGPS(string CodeMedecin)
        {
            //On efface le calque Medecin et les pré-attribuées         
            CalqueMedecinSelect.Clear();

            DataTable dtMedecin = new DataTable();
            dtMedecin = FonctionsAppels.ChargePosGPSMedecins(CodeMedecin);

            //Si on a quelque chose on affiche la position du médecin
             //On declare un nouveau point pour décaler à la centrale
            PointLatLng ptAlaCentrale = new PointLatLng();
            ptAlaCentrale.Lat = ptCentrale.Lat;
            ptAlaCentrale.Lng = ptCentrale.Lng;

            if (dtMedecin.Rows.Count > 0)
            {
                PointLatLng pointMed = new PointLatLng();
                int Vitesse = 0;

                //ici recup des coord. et tester, voir si elles sont dans le cadre, si ok les afficher, sinon centrale.
                pointMed.Lat = Double.Parse(dtMedecin.Rows[0]["Lat"].ToString());
                pointMed.Lng = Double.Parse(dtMedecin.Rows[0]["Lng"].ToString());
                Vitesse = int.Parse(dtMedecin.Rows[0]["Vitesse"].ToString());

                //On vérifie que les coordonnées restent bien dans la région définie en paramètre
                if (VerifGeocodage(pointMed) == "KO")
                {
                    pointMed = ptAlaCentrale;   //Si c'est pas bon, on met la centrale

                    //On incrémente la longitude pour décaler les points
                    ptAlaCentrale.Lng += 0.001;
                }

                //On affiche sa localisation
                GMarkerGoogle marqueur = new GMarkerGoogle(pointMed, new Bitmap(SRegulV2.Properties.Resources.voiture));

                CalqueMedecinSelect.Markers.Add(marqueur);

                //On ajoute la légende du marqueurs (l'adresse ici)              
                marqueur.ToolTip = new GMapToolTip(marqueur);
                marqueur.ToolTip.Fill = new SolidBrush(Color.FromArgb(255, Color.FloralWhite));
                marqueur.ToolTip.Foreground = Brushes.Black;               
                marqueur.ToolTipText = dtMedecin.Rows[0]["NomMed"].ToString() + "\n " + dtMedecin.Rows[0]["StatusGarde"].ToString();
                marqueur.ToolTipMode = MarkerTooltipMode.Always;  //on l'affiche toujours


                //On ajoute enfin le calque à la carte
                gMapControl1.Overlays.Add(CalqueMedecinSelect);
               
                //Puis on affiche ses prochaines visites s'il en a
                //recup des infos de sa visite en cours (AT, AQ, V)
                DataTable dtvisite = FonctionsAppels.RechAppelsEnCoursMed(CodeMedecin, "ATPRAQV");

                //Récup du point d'origine (position du médecin)               
                PointLatLng Origine = pointMed;

                //on efface les précédentes lignes
                CalqueToutesVisitesMed.Clear();
                CalqueLigne.Clear();

                //calque pour les trajets (à vol d'oiseau)
                GMapOverlay CalqueItineraire = new GMapOverlay();

                //On choisi le style de trait ainsi que la couleur
                Pen myPen = new Pen(Color.Red, 2.0F);
                myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;

                CalqueLigne.Stroke = myPen; //on affecte le style de trait au calque           

                for (int i = 0; i < dtvisite.Rows.Count; i++)
                {
                    //On géocode les adresses
                    //string Adr = dtvisite.Rows[i]["Adresse"].ToString() + ", " + dtvisite.Rows[i]["CodePostal"].ToString()
                     //       + ", " + dtvisite.Rows[i]["Commune"].ToString() + ", " + dtvisite.Rows[i]["Pays"].ToString();

                    PointLatLng Destination = new PointLatLng();// = GeocodeAdresse(Adr);

                    Destination.Lat = Double.Parse(dtvisite.Rows[i]["Latitude"].ToString());
                    Destination.Lng = Double.Parse(dtvisite.Rows[i]["Longitude"].ToString());

                    //Maintenant on ajoute les points
                    CalqueLigne.Points.Add(Origine);

                    if (Destination != ptNonGeocode)
                    {
                        CalqueLigne.Points.Add(Destination);

                        //Puis on prend cette destination pour l'origine de la prochaine adresse
                        Origine = Destination;
                    }
                    else
                    {
                        //Dans le lac
                        CalqueLigne.Points.Add(ptNonGeocode);
                        //On incrémente la longitude pour décaler les points
                        ptNonGeocode.Lng += 0.001;
                        //Puis on prend cette destination pour l'origine de la prochaine adresse
                        Origine = ptNonGeocode;
                    }
                 
                }

                //On ajoute les calques à la carte
                CalqueToutesVisitesMed.Routes.Add(CalqueLigne);
                gMapControl1.Overlays.Add(CalqueToutesVisitesMed);

                //on rafraichi la carte
                gMapControl1.UpdateRouteLocalPosition(CalqueLigne);
            }                   
        }


        //On affiche le popup de la visite
        public static void FocusVisite(ListViewItem item, double Latitude, double Longitude)
        {
            //On efface le calque CalqueVisiteSelect       
            CalqueVisiteSelect.Clear();

             //item.SubItems[i]

            string Nom = item.SubItems[6].Text.ToString();

            string Adresse = item.SubItems[4].Text.ToString() + ", " + item.SubItems[5].Text.ToString();
                           
            //PointLatLng pt = GeocodeAdresse(Adresse);
            PointLatLng pt = new PointLatLng();
            pt.Lat = Latitude;
            pt.Lng = Longitude;

            //On prépare un nouveau marqueur
            GMarkerGoogle marqueur;

            if (VerifGeocodage(pt) == "OK" && pt != ptNonGeocode)
            {               
                marqueur = new GMarkerGoogle(pt, new Bitmap(SRegulV2.Properties.Resources.marqueTransparent));
            }
            else    //Non géocodé
            {
                marqueur = new GMarkerGoogle(ptNonGeocode, new Bitmap(SRegulV2.Properties.Resources.marqueTransparent));               
            }

            CalqueVisiteSelect.Markers.Add(marqueur);

            //On ajoute la légende du marqueurs (l'adresse ici)
            marqueur.ToolTip = new GMapToolTip(marqueur);
            marqueur.ToolTip.Fill = new SolidBrush(Color.FromArgb(255, Color.FloralWhite));
            marqueur.ToolTip.Foreground = Brushes.Black; 
            marqueur.ToolTipText = Nom + "\n" + Adresse;
            marqueur.ToolTipMode = MarkerTooltipMode.Always;  //on l'affiche toujours

            //On ajoute enfin le calque à la carte
            gMapControl1.Overlays.Add(CalqueVisiteSelect);                                 
        }


        public static void EffacePopupVisite()
        {
            //On efface le calque CalqueVisiteSelect       
            CalqueVisiteSelect.Clear();
        }
   

        public static void EffaceLignesToutesVisites()
        {
            //on efface les lignes des routes des pré-Attribuées
            CalqueToutesVisitesMed.Clear();
            CalqueLigne.Clear();
        }             


        //Trace une ligne entre 2 points (non utilisé pour le moment)
        public static void TraceLigne(PointLatLng Depart, PointLatLng Arrivee)
        {                  
            Pen myPen = new Pen(Color.Red, 4.0F);
            myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            
            CalqueLigne.Stroke = myPen;                     

            //Création du calque avec 2 points
            CalqueLigne.Points.Add(new PointLatLng(46.2119141, 6.1379277));
            CalqueLigne.Points.Add(new PointLatLng(46.226732, 6.119011));
                       
            CalqueToutesVisitesMed.Routes.Add(CalqueLigne);
            gMapControl1.Overlays.Add(CalqueToutesVisitesMed);
                                         
            //On rafraichie le calque
            gMapControl1.UpdateRouteLocalPosition(CalqueLigne);
           
        }


        //Fonction qui géocode une adresse et retourne les coordonnées
       /* public static PointLatLng GeocodeAdresse(string Adresse)
        {           
            PointLatLng Coordonnees = new PointLatLng();
            Coordonnees.Lat = 0.0;
            Coordonnees.Lng = 0.0;

            //On indique la validation automatique des demandes d'acceptation des certificats
            SSLValidator.OverrideValidation();  

            GeoCoderStatusCode status = GeoCoderStatusCode.UNKNOWN_ERROR;
            GeocodingProvider gp = GMapProviders.OpenStreetMap as GeocodingProvider;
            
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
                    gp = GMapProviders.OpenStreetMap as GeocodingProvider;
                    pt = gp.GetPoint(Adresse, out status);
                    if (status == GeoCoderStatusCode.OK && pt.HasValue)
                    {
                        Coordonnees = pt.Value;
                    }
                }
            }

            if (status != GeoCoderStatusCode.OK && !gp.GetType().ToString().Contains("Google"))
            {
                var pt = GMapProviders.GoogleMap.GetPoint(Adresse, out status);
                if (status == GeoCoderStatusCode.OK && pt.HasValue)
                {
                    Coordonnees = pt.Value;
                }
            }

            return Coordonnees;
        }*/

        
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


        private void FCarto_FormClosing(object sender, FormClosingEventArgs e)
        {
            //En fait on interrompe la fermeture
            e.Cancel = true;          
        }


        private void FCarto_FormClosed(object sender, FormClosedEventArgs e)
        {
          //  gMapControl1.Dispose();
          //  this.Dispose();
        }


        //Qd Clique sur le marqueur....Pas encore utilisé
        private void gMapControl1_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {         
                 
        }


        #region Choix des cartes
        private void bCarteSStrafic_Click(object sender, EventArgs e)
        {
            gMapControl1.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            //GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gMapControl1.Refresh();
        }

        private void bCarteAvTraffic_Click(object sender, EventArgs e)
        {
            GoogleMapProvider.Instance.Version = "m,traffic@336000000";
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gMapControl1.Refresh();
        }

        private void bSatellite_Click(object sender, EventArgs e)
        {
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gMapControl1.Refresh();
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Dans tout les cas, on efface le calque d'info sur une eventuelle visite selectionné
            EffacePopupVisite();


            //On regarde s'il faut rafraichir la carte
            if (rafraichiCarte == "OK")
            {
                Rafraichi();
                rafraichiCarte = "KO";
            }
            else
            {   
                RafraichiPoseMeds();    //Dans tout les cas on rafraichi la position géolocalisée des médecins
                EffaceLignesToutesVisites();    //Efface les visites et routes d'un médecin selectionné
            }
        }


        //Permet de toujours autoriser l'acceptation des certificats ***Attention faut etre sur d'ou on appelle les services****
        public static class SSLValidator
        {
            private static bool OnValidateCertificate(object sender, X509Certificate certificate, X509Chain chain,
                                                      SslPolicyErrors sslPolicyErrors)
            {
                return true;
            }
            public static void OverrideValidation()
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    OnValidateCertificate;
                ServicePointManager.Expect100Continue = true;
            }
        }


       
                          
    }
}


//A faire
