using MySqlConnector;
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
    public partial class FMedecin : Form
    {
        public static DataTable dtMedecin = new DataTable();
        private static string Etat = "Lecture";
        private static byte[] Signature = null;

        public FMedecin()
        {
            InitializeComponent();

            //Def des colonnes du Listeview
            listView1.Columns.Add("Code Medecin", 50);
            listView1.Columns.Add("Nom", 160);
            listView1.Columns.Add("Titre d'emploi", 80);
            listView1.Columns.Add("Début activité", 100);     
            listView1.Columns.Add("Fin activité", 100);       
            listView1.View = View.Details;    //Pour afficher les subItems  

            //On vide les champs et initialise les boutons
            VideChamps();
        }
     

        private void FMedecin_Load(object sender, EventArgs e)
        {
            dtMedecin.Clear();
            dtMedecin = FonctionsAppels.ChargeListeMedecins("tous");

            //On vide la liste pour la rafraichir                
            listView1.BeginUpdate();
            listView1.Items.Clear();            

            for (int i = 0; i < dtMedecin.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(dtMedecin.Rows[i]["CodeMedecin"].ToString());                    
                item.SubItems.Add(dtMedecin.Rows[i]["Nom"].ToString() + " " + dtMedecin.Rows[i]["Prenom"].ToString());                               
                item.SubItems.Add(dtMedecin.Rows[i]["TitreEmployee"].ToString());
                item.SubItems.Add(dtMedecin.Rows[i]["DateDeb"].ToString());
                item.SubItems.Add(dtMedecin.Rows[i]["DateFin"].ToString());                 

                listView1.Items.Add(item);              
            }

            listView1.EndUpdate();  //Rafraichi le controle            
        }

        private void VideChamps()
        {
            tBoxCodeMed.Text = "";
            cBoxOrganisation.Text = "";
            cBoxTitreEmployee.Text = "";
            tBoxNom.Text = "";
            tBoxPrenom.Text = "";
            mTBoxDateDeb.Text = "";
            mTBox1DateFin.Text = "";
            tBoxConcordat.Text = "";
            tBoxEmail.Text = "";
            tBoxTitre.Text = "";
            pictureBox1.Image = null;
            Signature = null;

            //Pour les boutons
            bAjouter.ImageIndex = 0;
            bAjouter.Visible = true;

            bValider.ImageIndex = 4;
            bValider.Enabled = false;

            bSupprimer.ImageIndex = 6;
            bSupprimer.Enabled = false;

            bCancel.Visible = false;

            bExit.ImageIndex = 7;
        }

        //Selection d'un médecin dans la liste
        private void listView1_DoubleClick(object sender, EventArgs e)
        {                       
            int index = listView1.SelectedItems[0].Index;

            //On peuple les champs
            tBoxCodeMed.Text = dtMedecin.Rows[index]["CodeMedecin"].ToString();
            cBoxOrganisation.Text = dtMedecin.Rows[index]["LibelleOrg"].ToString();
            cBoxTitreEmployee.Text = dtMedecin.Rows[index]["TitreEmployee"].ToString();
            tBoxNom.Text = dtMedecin.Rows[index]["Nom"].ToString();
            tBoxPrenom.Text = dtMedecin.Rows[index]["Prenom"].ToString();
            mTBoxDateDeb.Text = dtMedecin.Rows[index]["DateDeb"].ToString();
            mTBox1DateFin.Text = dtMedecin.Rows[index]["DateFin"].ToString();
            tBoxConcordat.Text = dtMedecin.Rows[index]["Concordat"].ToString();
            tBoxEmail.Text = dtMedecin.Rows[index]["Email"].ToString();
            tBoxTitre.Text = dtMedecin.Rows[index]["Titre"].ToString();

            if (dtMedecin.Rows[index]["GMS"].ToString() == "1")
                cBoxGMS.Checked = true;
            else
                cBoxGMS.Checked = false;

            if (dtMedecin.Rows[index]["Signature"] != DBNull.Value)
            {
                Byte[] Sign = new Byte[0];
                Sign = (Byte[])(dtMedecin.Rows[index]["Signature"]);

                Image imgSign = byteArrayToImage(Sign);
              
                //On affiche l'image en la redimentionnant (pour que ça rentre dans le picture box sans être déformée)                           
                ResizeImage(imgSign);               
            }
            else
                pictureBox1.Image = null;

            //Gestion des boutons: On passe en modif
            Etat = "Modif";

            bAjouter.Visible = false;
            bAjoutSign.Enabled = true;

            bValider.Enabled = true;
            bCancel.Visible = true;
            bSupprimer.Enabled = true;
        }


        //Gestion des boutons
        private void bAjouter_Click(object sender, EventArgs e)
        {
            //Gestion des boutons: On passe en Ajout
            Etat = "Ajout";

            VideChamps();

            bAjouter.Visible = false;
            bValider.Enabled = true;
            bCancel.Visible = true;
            bSupprimer.Enabled = false;
        }

        //On valide en fonction de l'état
        private void bValider_Click(object sender, EventArgs e)
        {
            if (VerifSaisie() == "OK")
            {
                if (Etat == "Ajout")
                {
                    if (AjoutEnreg() == "OK")
                    {
                        VideChamps();

                        //Gestion des boutons
                        bAjouter.Visible = true;
                        bValider.Enabled = false;
                        bCancel.Visible = false;
                        bSupprimer.Enabled = false;

                        Etat = "Lecture";

                        //On rafraichi la liste des SmartPhones
                        FMedecin_Load(sender, e);
                    }
                    else
                    {
                        //Gestion des boutons
                        bAjouter.Visible = false;
                        bValider.Enabled = true;
                        bCancel.Visible = true;
                        bSupprimer.Enabled = false;
                    }
                }
                else if (Etat == "Modif")
                {
                    if (UpdateEnreg() == "OK")
                    {
                        VideChamps();
                        //Gestion des boutons
                        bAjouter.Visible = true;
                        bValider.Enabled = false;
                        bCancel.Visible = false;
                        bSupprimer.Enabled = false;

                        Etat = "Lecture";

                        //On rafraichi la liste des SmartPhones
                        FMedecin_Load(sender, e);
                    }
                    else
                    {
                        //Gestion des boutons
                        bAjouter.Visible = false;
                        bValider.Enabled = true;
                        bCancel.Visible = true;
                        bSupprimer.Enabled = false;
                    }
                }
            }

        }

        //vérification des saisies
        private string VerifSaisie()
        {
            string retour = "KO";
            int V1 = 0;
         
            if (tBoxCodeMed.Text != "")                              
                V1 += 1;            
            else
                MessageBox.Show("Vous devez saisir un code médecin. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (cBoxOrganisation.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner une organisation. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (cBoxTitreEmployee.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner une titre employé. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (tBoxNom.Text != "")
                V1 += 1;
            else
                MessageBox.Show("Vous devez renseigner le nom du médecin. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (mTBoxDateDeb.Text != "")
            {
                //On vérifie le format
                DateTime Dated;
           
                if (DateTime.TryParse(mTBoxDateDeb.Text, out Dated) == true)
                    V1 += 1;
                else
                {
                    MessageBox.Show("Vous devez saisir une date de début d'activité valide. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else MessageBox.Show("Vous devez saisir une date de début d'activité. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (mTBox1DateFin.Text != "  .  .")
            {
                //On vérifie le format
                DateTime DateF;

                if (DateTime.TryParse(mTBox1DateFin.Text, out DateF) == true)
                    V1 += 1;
                else
                {
                    MessageBox.Show("La date de fin d'activité n'est pas valide. ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else V1 += 1;

            //En fonction des résultats
            switch (V1)
            {
                case 6: retour = "OK"; break;
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

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Def de la requete
                //*****medecin***
                SqlStr0 = "INSERT INTO medecins ";
                SqlStr0 += " (CodeMedecin, Nom, Prenom, DateDebActiv, DateFinActiv, GMS, Concordat, Email, Titre, Signature, LibelleOrg, TitreEmployee)";
                SqlStr0 += " VALUES('" + tBoxCodeMed.Text + "','" + tBoxNom.Text.Replace("'", "''") + "','" + tBoxPrenom.Text.Replace("'", "''") + "','";               
                SqlStr0 += FonctionsAppels.convertDateMaria(mTBoxDateDeb.Text + " 00:00:00", "MariaDb") + "',";

                if (mTBox1DateFin.Text != "  .  .")                
                    SqlStr0 += "'" + FonctionsAppels.convertDateMaria(mTBox1DateFin.Text + " 00:00:00", "MariaDb") + "',";
                else SqlStr0 += " NULL,";

                if (cBoxGMS.Checked)
                    SqlStr0 += "1,";
                else
                    SqlStr0 += "0,";

                SqlStr0 += "'" + tBoxConcordat.Text;
                SqlStr0 += "','" + tBoxEmail.Text;
                SqlStr0 += "','" + tBoxTitre.Text + "',";

                if (Signature != null)
                    SqlStr0 += "@Signature)";
                else
                    SqlStr0 += " NULL,";

                SqlStr0 += "'" + cBoxOrganisation.Text + "',";
                SqlStr0 += "'" + cBoxTitreEmployee.Text + "')";

                //*****pref_medecin***
                SqlStr1 = "INSERT INTO pref_medecin ";
                SqlStr1 += " (CodeMedecin, Appli_GPS)";
                SqlStr1 += " VALUES('" + tBoxCodeMed.Text + "','Google Maps')";               
               
                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0;
                    cmd.Transaction = trans;

                    if (Signature != null)
                        cmd.Parameters.Add("@Signature", MySqlDbType.LongBlob).Value = Signature; 
                    
                    cmd.ExecuteNonQuery();
                    
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();

                    Retour = "OK";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la création du médecin (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Erreur lors de la création du médecin. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Def de la requete
                //*****Médecin***
                SqlStr0 = "UPDATE medecins ";
                SqlStr0 += " SET Nom = '" + tBoxNom.Text.Replace("'", "''") + "', Prenom = '" + tBoxPrenom.Text.Replace("'", "''");
                SqlStr0 += "', DateDebActiv= '" + FonctionsAppels.convertDateMaria(mTBoxDateDeb.Text + " 00:00:00", "MariaDb") + "'";

                if (mTBox1DateFin.Text != "  .  .")
                    SqlStr0 += ", DateFinActiv = '" + FonctionsAppels.convertDateMaria(mTBox1DateFin.Text + " 00:00:00", "MariaDb") + "'";
                else
                    SqlStr0 += ", DateFinActiv = null";

                if (cBoxGMS.Checked)
                    SqlStr0 += ", GMS = 1";
                else
                    SqlStr0 += ", GMS = 0";

                SqlStr0 += ", Concordat = '" + tBoxConcordat.Text + "'";
                SqlStr0 += ", Email = '" + tBoxEmail.Text + "'";
                SqlStr0 += ", Titre = '" + tBoxTitre.Text + "'";

                if (Signature != null)
                    SqlStr0 += ", Signature = @Signature";

                SqlStr0 += ", LibelleOrg = '" + cBoxOrganisation.Text + "'";
                SqlStr0 += ", TitreEmployee = '" + cBoxTitreEmployee.Text + "' ";

                SqlStr0 += " WHERE CodeMedecin = '" + tBoxCodeMed.Text + "'";
                
                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0;
                    cmd.Transaction = trans;

                    if (Signature != null)                    
                        cmd.Parameters.Add("@Signature", MySqlDbType.LongBlob).Value = Signature;                    
                    
                    cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();
                    Retour = "OK";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    MessageBox.Show("Erreur lors de la modification du médecin (transaction). " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Erreur lors de la modification du médecin. " + e.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

        private void bSupprimer_Click(object sender, EventArgs e)
        {
            //On essaie de supprimer le médecin...S'il n'a jamais pris de garde (HistoGarde)
            DataTable dtExiste = FonctionsAppels.RechercheHisto("CodeMedecin", tBoxCodeMed.Text);
            if (dtExiste.Rows.Count > 0)
                MessageBox.Show("Impossible de supprimer ce médecin car il a déjà pris des gardes. ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //Suppression de l'enregistrement
                if (FonctionsAppels.SupprMedecin(tBoxCodeMed.Text) == "Ok")
                {
                    MessageBox.Show("Suppression du médecin => OK ", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    VideChamps();
                    //Gestion des boutons
                    bAjouter.Visible = true;
                    bValider.Enabled = false;
                    bCancel.Visible = false;
                    bSupprimer.Enabled = false;

                    Etat = "Lecture";

                    //On rafraichi la liste des SmartPhones
                    FMedecin_Load(sender, e);
                }
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            //on annule
            VideChamps();
            //Gestion des boutons
            bAjouter.Visible = true;
            bValider.Enabled = false;
            bCancel.Visible = false;
            bSupprimer.Enabled = false;

            Etat = "Lecture";

            //On rafraichi la liste des médecins
            FMedecin_Load(sender, e);
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

        private void tBoxCodeMed_Click(object sender, EventArgs e)
        {
            //si on est en lecture, on passe en ajout
            if (Etat == "Lecture")
            {
                bAjouter_Click(sender, e);
            }
        }

        private void mTBoxDateDeb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bValider_Click(sender, e);
            }
        }

        //Ajout de la signature du médecin
        private void bAjout_Click(object sender, EventArgs e)
        {
            //On va chercher le fichier à ajouter                                   
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Tous les fichiers (*.*) | *.*";
           
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;              

                //Convertion de l'image en byteArray
                Image img = Image.FromFile(filename);
                Signature = imgToByteConverter(img);
               
                //On affiche l'image en la redimentionnant (pour que ça rentre dans le picture box sans être déformée)            
                Image img1 = byteArrayToImage(Signature);
                ResizeImage(img1);                                      
            }           
        }
      

        //Convertion d'une image en tableau d'octets (Bytes)
        public static byte[] imgToByteConverter(Image imgSignature)
        {
            ImageConverter imageConverter = new ImageConverter();           
            
            byte[] ByteSignature = (byte[])imageConverter.ConvertTo(imgSignature, typeof(byte[]));
            
            return ByteSignature;         
        }

        //Convertion d'un tableau d'octets en image (Bytes)
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream mStream = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(mStream);
            }
        }
      

        #region Fonction pour afficher l'image au bon format
        private void ResizeImage(Image MonImage)
        {
            //réglages des valeurs servant au calcul
            //La pictureBox
            int Lmax = pictureBox1.Width;
            int Hmax = pictureBox1.Height;
            double ratio = (double)Lmax / Hmax;     //Ratio de la picturebox

            //L'image
            double ImgLng = MonImage.Width;
            double ImgHaut = MonImage.Height;
            double ratioImage = (double)MonImage.Width / MonImage.Height;    //Ratio de base pour rentrer correctement dans la picturebox

            //Si l'image est plus grande que la pictureBox
            if (ImgLng > Lmax || ImgHaut > Hmax)           
            {
                if (ImgLng > Lmax)            //Si la longueur est plus longue
                {
                    if (1 > ratioImage)     //..et si la largueur est plus longue
                    {
                        //Alors la Haut. de l'image prend la haut. de la pictureBox
                        ImgHaut = Hmax;         

                        //On détermine la longueur
                        if (ImgLng > MonImage.Height) 
                            ImgLng = ImgHaut / ratioImage;
                        else ImgLng = ImgHaut * ratioImage; 
                    }
                    else //Seule la largeur est plus longue
                    {
                        //Alors la larg. de l'image prend la larg. de la pictureBox
                        ImgLng = Lmax;

                        //Calcul de la hauteur
                        if (ImgHaut > MonImage.Width) 
                            ImgHaut = ImgLng / ratioImage;
                        else ImgHaut = ImgLng / ratioImage;
                    }
                }
                else //Seule la largeur est plus longue
                {
                    ImgHaut = Hmax;
                    ImgLng = ImgHaut * ratioImage;
                }

                pictureBox1.Image = MonImage.GetThumbnailImage(Convert.ToInt32 (ImgLng), Convert.ToInt32(ImgHaut), null, IntPtr.Zero); // j'en tire une miniature
            }
            else pictureBox1.Image = MonImage; // sinon j'affiche l'image de base
        }
        #endregion

        //On clique sur la liste des organisations
        private void cBoxOrganisation_DropDown(object sender, EventArgs e)
        {
            //si la liste est vide, on la charge
            if(cBoxOrganisation.Items.Count == 0)
            {
                DataTable dtOrganisation = FonctionsAppels.ChargeListeOrganisation();

                cBoxOrganisation.BeginUpdate();

                for (int i = 0; i < dtOrganisation.Rows.Count; i++)
                {
                    cBoxOrganisation.Items.Add(dtOrganisation.Rows[i]["LibelleOrg"].ToString());                                        
                }

                cBoxOrganisation.EndUpdate();  //Rafraichi le controle        
            }
        }
    }
}
