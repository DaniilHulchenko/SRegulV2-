using MailKit;
using MimeKit.Cryptography;
using MySqlConnector;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FMailMutuaide : Form
    {
        public IMessageSummary Email;
        public string BodyMessage;
        public string UniqueIdMail = "";
        private bool isSending = false;   //Pour la gestion du bouton d'envoi


        public FMailMutuaide()
        {
            InitializeComponent();

            labelEnvoiEnCours.Visible = false;
        }

        //Au chargement de la form, on charge les paramètres de l'email
        private void FMailMutuaide_Load(object sender, EventArgs e)
        {                        
            
            if (Email != null)
            {                
                labelDe.Text = "De : " + Email.Envelope.From.ToString();
                labelSujet.Text = "Sujet : " + Email.Envelope.Subject.ToString();
                tBoxMessage.Text = "Message : " + BodyMessage;

                UniqueIdMail = Email.UniqueId.Id.ToString();  //Récup de l'uniqueId de l'email

                tBoxMessage.Select(0, 0);   //On desselecte le contenu par défaut

                //Puis on affiche la liste des nouveaux messages
                Beeper("sms-airport.wav");
            }
            else
            {
                labelDe.Text = "De : ";
                labelSujet.Text = "Sujet : ";
                tBoxMessage.Text = "Message : ";
            }   
        }

        private void BExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void BEnvoi_Click(object sender, EventArgs e)
        {
            if (isSending) return;  //Si l'envoi est en cours, ne rien faire

            isSending = true;  //Désactiver le bouton

            labelEnvoiEnCours.Visible = true; 

            //On regarde si l'Email n'a pas déjà été répondu
            string reponse = VerifieEmailReponse();

            try
            {
                if (reponse == "KO")
                {
                    //On envoi la réponse           
                    await EnvoyeEmailReponse();
                }
                else
                {
                    isSending = false;     //On résactive le bouton

                    //On ne fait rien, on ferme la form               
                    Close();
                }
            }
            finally 
            {
                labelEnvoiEnCours.Visible = false;
                isSending = false; 
            }                        
        }


        //On envoi un email           
        private async Task EnvoyeEmailReponse()
        {
            //Récup de l'image Logo, à partir des ressources du projet
            Image image = Properties.Resources.logoSwissMedic;

            // Convertir l'image en tableau d'octets
            byte[] imageBytes = ImageToByte(image);

            //Convertir les octets en base64
            string base64Image = Convert.ToBase64String(imageBytes);

            //Formatage du message                                  
            string message;

            message = $@"<html><head><style type = 'text/css'>body, p, div {{font-family: Helvetica, Arial, sans-serif;font-size: 14px;}}a{{text-decoration: none;}}</style><title></title></head>";

            //Ici on insère le logo
            message += $@"<body><img src='data:image/png;base64,{base64Image}' alt='Logo' width='437' height='162'/>";

            message += $@"<h4>Genève, le {DateTime.Now.ToString("dd MMMM yyyy")}</h4><p></p>";

            message += $@"<p>Nous vous remercions pour votre demande concernant : {Email.Envelope.Subject}</p>";
            message += $@"<p></p>";

            if (tBoxReponse.Text.Trim(' ').Length != 0)
            {
                message += $@"<p>{tBoxReponse.Text}</p>";
            }

            message += $@"<p></p>";
            message += $@"<p>Nous vous tiendrons au courant dès que possible. Nous restons bien entendu à votre disposition si nécessaire.</p>";
            message += $@"<p></p>";
            message += $@"<p>Swiss Medical Assistance</p>";
            message += $@"</body></html>";

            string ReponseEnvoiMail = "KO";
            
            if (Form1.EmailMutuaideFrom != "")
            {
                ReponseEnvoiMail = EmailService.SendMail(
                                                    to: Email.Envelope.From.ToString(),                                                       
                                                    subject: "Re: " + Email.Envelope.Subject,
                                                    html: $@"{message}",
                                                    from: Form1.EmailMutuaideFrom,
                                                    typeDest: "Mutuaide"
                                                    );
            }

            //Si l'envoi c'est bien passé
            if (ReponseEnvoiMail == "OK")
            {
                //On signale que le mail a été envoyé
                ValideEnvoi();                
                
                mouchard.evenement("SMA Envoi d'un email à " + Email.Envelope.From.ToString() + ", en reponse au mail dont le libelle est : " + Email.Envelope.Subject.ToString(), Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString());   //log                                      

                //Ajouter une pause de 2 secondes
                await Task.Delay(2000);

                //Fermer la Form
                Close();
            }
            else   //Sinon on affiche le message d'erreur
            {
                MessageBox.Show(ReponseEnvoiMail, "SMA Erreur lors de l'envoi du mail de réponse", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //Beeper
        private void Beeper(string SonAJouer)
        {
            SoundPlayer FinVisiteSound = new SoundPlayer(Application.StartupPath + @"\Sons\" + SonAJouer);
            FinVisiteSound.Play();
        }

        //Pour convertir une image en un tableau de byte
        private byte[] ImageToByte(Image image)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }


        private string VerifieEmailReponse()
        {
            string retour = "KO";

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT * FROM emailsma WHERE IdMail = '" + UniqueIdMail + "' AND Etat = 'Repondu'" ;
                cmd.CommandText = sqlstr0;

                DataTable dtEmail = new DataTable();
                dtEmail.Load(cmd.ExecuteReader());    //on execute

                if (dtEmail.Rows.Count > 0)
                    retour = "OK";
                else 
                    retour = "KO";
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la récupération des emails " + e.Message);
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

        //On enregistre l'envoi du mail dans la base
        private void ValideEnvoi()
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;            

            try
            {
                string sqlstr0 = @"INSERT INTO emailsma (IdMail, IdUtilisateur, Etat, DateEtat) VALUES (@IdMail, @Utilisateur, @Etat, @DateEtat)";
             
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdMail", UniqueIdMail);
                cmd.Parameters.AddWithValue("Utilisateur", Form1.Utilisateur[0].ToString());
                cmd.Parameters.AddWithValue("Etat", "Repondu");
                cmd.Parameters.AddWithValue("DateEtat", DateTime.Now);

                cmd.ExecuteNonQuery();    //on execute       
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout d'un Mail SMA, table emailsma: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    }
}
