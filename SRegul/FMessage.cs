using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FMessage : Form
    {
        public string CodeMedecin = "-1";
        public string Destinataire;
        public string PieceJointe = "";

        public FMessage()
        {
            InitializeComponent();

            //Max 250 caractères
            textBox1.MaxLength = 250;  
         
            //on efface le libellé de la pièce jointe
            lPiecejointe.Text = "";         
        }

        private void FMessage_Load(object sender, EventArgs e)
        {
            //Affichage du destinataire du message
            label1.Text = "Envoi du message à " + Destinataire;

            //On active ou pas le bouton rappel en fct de l'état du CTI et du code médecin (pas de tel si destiné à tous le monde)
            if (Form1.ActivationCTI == true && CodeMedecin != "-1")
                bRappel.Enabled = true;
            else
                bRappel.Enabled = false;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //On compte le nombre de caractère
            labelCaractere.Text = textBox1.TextLength + " caractères sur 250";
        }

        private void bEnvoi_Click(object sender, EventArgs e)
        {
            string FileDestCourt = "";
            
            //Si on a une pièce jointe, on la renomme et on la copie sur le serveur
            if (PieceJointe != "")
            {
                System.Random rnd = new System.Random(DateTime.Now.Millisecond);     //Pour avoir un chiffre entre 1000 et 9999

                string pathDest = ConfigurationManager.AppSettings["Path_PiecesJointes"].ToString();    //Chemin où mettre les pieces jointes                                               

                string Extention = Path.GetExtension(PieceJointe);
                string NomF = Path.GetFileNameWithoutExtension(PieceJointe);

                string FileDest = pathDest + "\\" + NomF + rnd.Next(1000, 9999).ToString() + Extention;

                //On copie la pièce jointe sur le serveur
                try
                {
                    //On regarde si un fichier du meme nom existe déjà sur le serveur
                    if (File.Exists(FileDest))
                        MessageBox.Show("Un fichier du même nom existe déjà...Veuillez renomer la pièce jointe.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        File.Copy(PieceJointe, FileDest);                    

                    //Récup du nom court pour le mettre dans la base
                    FileDestCourt = Path.GetFileName(FileDest);

                    //Puis on ajoute une petit texte au message
                    textBox1.Text += "...Pièce jointe.";
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Erreur lors de la copie de la pièce jointe...Le message est : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;   //On s'arrête là
                }

            }
            
            //On envoi le message (avec Id -2 => Régul + le nom et prenom de celui qui a envoyé le message)
            if (FonctionsAppels.EcritMessage(Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString() + ": " + textBox1.Text, "-2", CodeMedecin, FileDestCourt) == "OK")
            {
                //On affiche l'icone que tout est ok puis on ferme la Form
                pictureBox1.BackgroundImage = SRegulV2.Properties.Resources.smiley_1ok;
                pictureBox1.Visible = true;
                timer1.Enabled = true;
            }
            else
            {
                pictureBox1.BackgroundImage = SRegulV2.Properties.Resources.Smiley_Embete1;
                pictureBox1.Visible = true;
            }                                 
        }

        //On envoi juste une notification de rappel
        private void bBip_Click(object sender, EventArgs e)
        {
            if (FonctionsAppels.EcritMessage(Form1.Utilisateur[2].ToString() + " " + Form1.Utilisateur[3].ToString() + ": " + "Notification de rappel", "-2", CodeMedecin, "") == "OK")
            {
                //On affiche l'icone que tout est ok puis on ferme la Form
                pictureBox1.BackgroundImage = SRegulV2.Properties.Resources.smiley_1ok;
                pictureBox1.Visible = true;
                timer1.Enabled = true;
            }
            else
            {
                pictureBox1.BackgroundImage = SRegulV2.Properties.Resources.Smiley_Embete1;
                pictureBox1.Visible = true;
            }                
        }
       

        //Pour empêcher la saisie des / et \
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            verifie_touche(e);           
        }

        private void verifie_touche(KeyPressEventArgs e)
        {
            e.Handled = false;
           // switch (e.KeyCode)
            switch (e.KeyChar)
            {
                case (Char)(Keys) 92:      // caractere \
                case (Char)(Keys) 47:  e.Handled = true; break;    //caractere /
                default: e.Handled = false; break;
            }
        }


        private void bPiecejointe_Click(object sender, EventArgs e)
        {
            //On choisi la pièce jointe à envoyer                                        
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Fichier pdf (*.pdf) | *.pdf";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                PieceJointe = openFileDialog1.FileName;
                lPiecejointe.Text = "Pièce jointe :" + Path.GetFileName(PieceJointe);                
            }
            else
            {
                lPiecejointe.Text = "";
            }

        }


        private void bRappel_Click(object sender, EventArgs e)
        {
            //Recup du n° de tel actuel du médecin
            string NumTel = FonctionsAppels.RecupNumTelMedecin(CodeMedecin);
            
            //Si le CTI est actif
            if (Form1.ActivationCTI == true && NumTel != "KO")
            {                               
                string[] Reponse = new string[2];

                //On désactive le proxy HIN
                FonctionsCTI.DesactiveProxyHIN();               

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


                        //On affiche le smiley embeté
                        pictureBox1.BackgroundImage = SRegulV2.Properties.Resources.Smiley_Embete1;
                        pictureBox1.Visible = true;                                                                                 
                    }
                }

                //Puis on réactive le proxy HIN
                FonctionsCTI.ReactiveProxyHIN();
            }
            else
            {
                MessageBox.Show("CTI non actif ou n° de Tel du médecin non trouvé.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                //On affiche le smiley embeté
                pictureBox1.BackgroundImage = SRegulV2.Properties.Resources.Smiley_Embete1;
                pictureBox1.Visible = true;       
            }
        }


        //Pour fermer après 2 secondes
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
      
                 
    }
}



//A faire:
