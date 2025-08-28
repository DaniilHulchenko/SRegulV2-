using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Globalization;
using GMap.NET.MapProviders;

namespace CartoGeolocMedecins
{
    public partial class Carte : Form
    {
        Form1 form1 = new Form1();
        public GMapOverlay CalqueRoutes = new GMapOverlay("routes");       //calque pour afficher les itinéraires  
        public GMapOverlay CalqueMarqueur = new GMapOverlay("marqueur");   //calque pour afficher les marqueur sur la carte
        CultureInfo CultureFr = new CultureInfo("fr-CH");
        
        public Carte()
        {
            InitializeComponent();
        }

        public void Carte_Load(object sender, EventArgs e)
        {
            //on affiche la fenetre en plein ecran par défaut
            WindowState = FormWindowState.Maximized;
            

            //Initialisation de la carte

            //on chosi la carte openStreetMap
            gmap2.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;

            //au démarrage au positionne un marqueur sur SOS
            gmap2.SetPositionByKeywords("Rue louis favre, 43, 1201, Geneve, Suisse");
            Marque_adresse(gmap2.Position.Lat, gmap2.Position.Lng, "Rue louis favre, 43, 1201, Geneve, Suisse");


            //on met un zoom par défaut
            gmap2.Zoom = 17;

            //Le bouton gauche souris sert à glisser la carte
            gmap2.DragButton = MouseButtons.Left;

            //et on active le zoom avec la molette de la souris
            gmap2.MouseWheelZoomEnabled = true;

            //on enlève la petite croix rouge au milieu de l'écran
            gmap2.ShowCenter = false;

            
        }

        public void Fermer()
        {
            Close();
        }

        public void Marque_adresse(double Latitude, double Longitude, string Adresse)
        {
            GMarkerGoogle marqueur = new GMarkerGoogle(new PointLatLng(Latitude, Longitude), GMarkerGoogleType.red_dot);
            CalqueMarqueur.Markers.Add(marqueur);
            gmap2.Overlays.Add(CalqueMarqueur);

            //On ajoute la légende du marqueurs (l'adresse ici)
            marqueur.ToolTip = new GMapToolTip(marqueur);
            marqueur.ToolTipText = Adresse;
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            Fermer();
            form1.DetacherCarte(1);
            //form1.position = 1;

            
        }
    }
}
