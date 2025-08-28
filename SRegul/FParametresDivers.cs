using MySqlConnector;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace SRegulV2
{
    public partial class FParametresDivers : Form
    {
        private static bool nonNumerique = true;
        
        public FParametresDivers()
        {
            InitializeComponent();

            timer1.Enabled = false;
        }

        //Chargement des paramètres à louverture
        private void FParametresDivers_Load(object sender, EventArgs e)
        {
            DataTable dtParam = FonctionsAppels.ChargeParam();
            if(dtParam.Rows.Count > 0)
            {
                //Param des Timers
                tBoxTimer.Text = dtParam.Rows[0]["Timer"].ToString();
                tBoxTimerSmartphones.Text = dtParam.Rows[0]["TimerSmartphone"].ToString();

                //durée historique messages
                tBoxDureeMessages.Text = dtParam.Rows[0]["DureeMessages"].ToString();

                //On charge l'etat du CTI (Actif ou pas)
                if (dtParam.Rows[0]["ActivationCTI"].ToString() == "False")
                    cBoxCTI.Checked = false;
                else cBoxCTI.Checked = true;

                //On charge l'etat de la récupération des bandes audio (Actif ou pas)
                if (dtParam.Rows[0]["ActivationBandeAudio"].ToString() == "False")
                    cBoxBande.Checked = false;
                else cBoxBande.Checked = true;

                //Zone géographique de géocodage
                tBZGeoLatN.Text = dtParam.Rows[0]["ZGeoLatN"].ToString();
                tBZGeoLatS.Text = dtParam.Rows[0]["ZGeoLatS"].ToString();
                tBZGeoLngW.Text = dtParam.Rows[0]["ZGeoLngW"].ToString();
                tBZGeoLngE.Text = dtParam.Rows[0]["ZGeoLngE"].ToString();
                
                //Point central de la carte
                tBPtCentreLat.Text = dtParam.Rows[0]["CentreCarteLat"].ToString();
                tBPtCentreLng.Text = dtParam.Rows[0]["CentreCarteLng"].ToString();

                //Position de la centrale
                tBCentraleLat.Text = dtParam.Rows[0]["CentraleLat"].ToString();
                tBCentraleLng.Text = dtParam.Rows[0]["CentraleLng"].ToString();

                //Position de la Zone pour adresse non Géocodées
                tBptNonGeoLat.Text = dtParam.Rows[0]["ZStockLat"].ToString();
                tBptNonGeoLng.Text = dtParam.Rows[0]["ZStockLng"].ToString();   
             
                //Param Divers
                //Nombre Max de fenêtre Visite ouverte en même temps
                NbFicheVisite.Text = dtParam.Rows[0]["NbFicheVisite"].ToString();

                //Les infos pour l'envoi des emails TA
                tBoxEmailExpTA.Text = dtParam.Rows[0]["EmailTA"].ToString();
                tBoxPassEmail.Text = dtParam.Rows[0]["PassMailTA"].ToString();
                tBoxMailDest.Text = dtParam.Rows[0]["EmailTADest"].ToString();

                //Les infos pour l'envoi des emails aux infirmières               
                tBoxExpMailInf.Text = dtParam.Rows[0]["EmailInfirmiere"].ToString();
                tBoxPassMailInf.Text = dtParam.Rows[0]["PassMailInf"].ToString();
                tBoxMailDestInf.Text = dtParam.Rows[0]["EmailInfDest"].ToString();

                //Les infos du mail SMA
                tBoxMailRepSMA.Text = dtParam.Rows[0]["EmailMutuaideFrom"].ToString();
                tBoxPassMailRepSMA.Text = dtParam.Rows[0]["PassMailMutuaide"].ToString();                

                if (bool.Parse(dtParam.Rows[0]["ActivationMailSMA"].ToString()) == true)
                    cBoxMailSMA.Checked = true;
                else
                    cBoxMailSMA.Checked = false;
            }                                  
        }
    
        //Mise à jour du CTI
        private void MAJCTI(int val)
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "UPDATE param_divers SET ActivationCTI = '" + val + "'";
                cmd.CommandText = sqlstr0;

                cmd.ExecuteNonQuery();    //on execute  
     
                //Puis MAJ de la valeur globale
                if (val == 0)
                    Form1.ActivationCTI = false;
                else Form1.ActivationCTI = true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des mises à jours des paramètres divers: " + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        //Mise à jour du parametre de récupération des abandes audio de l'appel
        private void MAJBandeAudio(int val)
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "UPDATE param_divers SET ActivationBandeAudio = '" + val + "'";
                cmd.CommandText = sqlstr0;

                cmd.ExecuteNonQuery();    //on execute  

                //Puis MAJ de la valeur globale
                if (val == 0)
                    Form1.ActivationBandeAudio = false;
                else Form1.ActivationBandeAudio = true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des mises à jours des paramètres divers: " + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        //MAJ du Timer du dispatch
        private void MAJTimer(int val)
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "UPDATE param_divers SET Timer = '" + val + "'";
                cmd.CommandText = sqlstr0;

                cmd.ExecuteNonQuery();    //on execute       
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la mise à jour du timer, table param_divers: " + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            MessageBox.Show("Changements du Timer OK", "Délais du timer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        //MAJ du Timer de la carte
        private void MAJTimerCarte(int val)
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "UPDATE param_divers SET TimerCarte = '" + val + "'";
                cmd.CommandText = sqlstr0;

                cmd.ExecuteNonQuery();    //on execute       
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la mise à jour du timer de la carte, table param_divers: " + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            MessageBox.Show("Changements du Timer de la carte OK", "Délais du timer de la carte", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        private void MAJTimerSmartphones(int val)
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "UPDATE param_divers SET TimerSmartphone = '" + val + "'";
                cmd.CommandText = sqlstr0;

                cmd.ExecuteNonQuery();    //on execute       
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la mise à jour du timer des smartphones, table param_divers: " + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            MessageBox.Show("Changements du Timer des smartphones OK", "Délais du timer Smarphone", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        //MAJ de la durée d'affichage des messages sur le dispatch
        private void MAJDureeMessageDispatch(int val)
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "UPDATE param_divers SET DureeMessages = '" + val + "'";
                cmd.CommandText = sqlstr0;

                cmd.ExecuteNonQuery();    //on execute       
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la mise à jour de la durée de l'affichage des messages, table param_divers: " + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            MessageBox.Show("Changements de la durée de l'affichage des messages OK", "Durée affichage messages", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        //Validation du CTI
        private void bValiderCTI_Click(object sender, EventArgs e)
        {
            //On regarde si on a désactivé le CTI                                   
            if (!cBoxCTI.Checked)
                MAJCTI(0);               
            else
                MAJCTI(1);
            
            //On regarde si on a désactivé la récupération des bandes audio                                 
            if (!cBoxBande.Checked)
                MAJBandeAudio(0);                          
            else               
                MAJBandeAudio(1);

            //On met à jour le timer de rafraichissement du dispatch
            if (tBoxTimer.Text != "")
            {
                int val = int.Parse(tBoxTimer.Text);
                MAJTimer(val);
            }

            //On met à jour le timer de rafraichissement de la carte
            if (tBoxTimerCarte.Text != "")
            {
                int val = int.Parse(tBoxTimerCarte.Text);
                MAJTimerCarte(val);
            }

            //On met à jour la durée d'affichage des messages            
            if (tBoxDureeMessages.Text != "")
            {
                int val = int.Parse(tBoxDureeMessages.Text);
                MAJDureeMessageDispatch(val);
            }

            //On met à jour la table du Param_Divers               
            if (tBoxTimerSmartphones.Text != "")
            {
                int val = int.Parse(tBoxTimerSmartphones.Text);
                MAJTimerSmartphones(val);
            }           

            timer1.Enabled = true;     
        }


        //Timer du dispatch
        private void tBoxTimer_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumerique = false;

            //Pour les nombres du haut du clavier ET les nombres du pavé numérique
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)  &&
                 (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
            {
                nonNumerique = true;
                Console.WriteLine("KO");
            }


            if (e.KeyCode == Keys.Enter)
            {
                //On met à jour la table du Param_Divers               
                if(tBoxTimer.Text != "")
                {
                    int val = int.Parse(tBoxTimer.Text);
                    MAJTimer(val);
                }
            }
        }

        private void tBoxTimer_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (nonNumerique == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }

        //Timer de la carte
        private void tBoxTimerCarte_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumerique = false;

            //Pour les nombres du haut du clavier ET les nombres du pavé numérique
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) &&
                 (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
            {
                nonNumerique = true;
                Console.WriteLine("KO");
            }


            if (e.KeyCode == Keys.Enter)
            {
                //On met à jour la table du Param_Divers               
                if (tBoxTimerCarte.Text != "")
                {
                    int val = int.Parse(tBoxTimerCarte.Text);
                    MAJTimerCarte(val);
                }
            }
        }

        private void tBoxTimerCarte_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (nonNumerique == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }

        //Timer des smartphones
        private void tBoxTimerSmartphones_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumerique = false;

            //Pour les nombres du haut du clavier ET les nombres du pavé numérique
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) &&
                 (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
            {
                nonNumerique = true;
                Console.WriteLine("KO");
            }


            if (e.KeyCode == Keys.Enter)
            {
                //On met à jour la table du Param_Divers               
                if (tBoxTimerSmartphones.Text != "")
                {
                    int val = int.Parse(tBoxTimerSmartphones.Text);
                    MAJTimerSmartphones(val);
                }
            }
        }

        private void tBoxTimerSmartphones_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (nonNumerique == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }        


        private void bFermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }
      

        //Validation des coordonnées
        private void MAJCoordonnees()
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = @"UPDATE param_divers 
                                 SET ZGeoLatN = @ZGeoLatN, ZGeoLatS = @ZGeoLatS, ZGeoLngW = @ZGeoLngW, ZGeoLngE = @ZGeoLngE,
                                 ZStockLat = @ZStockLat, ZStockLng = @ZStockLng, CentreCarteLat = @CentreCarteLat, CentreCarteLng = @CentreCarteLng, 
                                 CentraleLat = @CentraleLat, CentraleLng = @CentraleLng";
                
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("ZGeoLatN", tBZGeoLatN.Text);
                cmd.Parameters.AddWithValue("ZGeoLatS", tBZGeoLatS.Text);
                cmd.Parameters.AddWithValue("ZGeoLngW", tBZGeoLngW.Text);
                cmd.Parameters.AddWithValue("ZGeoLngE", tBZGeoLngE.Text);
                cmd.Parameters.AddWithValue("ZStockLat", tBptNonGeoLat.Text);
                cmd.Parameters.AddWithValue("ZStockLng", tBptNonGeoLng.Text);
                cmd.Parameters.AddWithValue("CentreCarteLat",  tBPtCentreLat.Text);
                cmd.Parameters.AddWithValue("CentreCarteLng", tBPtCentreLng.Text);
                cmd.Parameters.AddWithValue("CentraleLat", tBCentraleLat.Text);
                cmd.Parameters.AddWithValue("CentraleLng", tBCentraleLng.Text);               

                cmd.ExecuteNonQuery();    //on execute       
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la mise à jour des coordonnées, table param_divers: " + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            MessageBox.Show("Mise à jour des coordonnées OK", "Coordonnées géographiques", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        //Validation du nombre de fiche visite à ouvrir en meme temps
        private void MajDivers()
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = @"UPDATE param_divers 
                                 SET NbFicheVisite = @NbFicheVisite";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("NbFicheVisite", NbFicheVisite.Text);
               

                cmd.ExecuteNonQuery();    //on execute       
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la mise à jour du nombre de fiche visite, table param_divers: " + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            MessageBox.Show("Mise à jour du nombre de fiche visite OK", "NB fiches Visite", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        //Maj des infos concernants l'envoi des email TA
        private void MajEmailTA()
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = @"UPDATE param_divers 
                                 SET EmailTA = @MailTA, PassMailTA = @PassMail, EmailTADest = @MailDest";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("MailTA", tBoxEmailExpTA.Text);
                cmd.Parameters.AddWithValue("PassMail", tBoxPassEmail.Text);
                cmd.Parameters.AddWithValue("MailDest", tBoxMailDest.Text);

                cmd.ExecuteNonQuery();    //on execute       
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la mise à jour des infos Mail TA, table param_divers: " + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            MessageBox.Show("Mise à jour des infos Mail TA OK", "Maj infos email TA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        //Maj des infos concernants l'envoi des email infirmière
        private void MajEmailInfirmiere()
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = @"UPDATE param_divers 
                                 SET EmailInfirmiere = @Mail, PassMailInf = @PassMail, EmailInfDest = @MailDest";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Mail", tBoxExpMailInf.Text);
                cmd.Parameters.AddWithValue("PassMail", tBoxPassMailInf.Text);
                cmd.Parameters.AddWithValue("MailDest", tBoxMailDestInf.Text);

                cmd.ExecuteNonQuery();    //on execute       
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la mise à jour des infos Mail Infirmière, table param_divers: " + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            MessageBox.Show("Mise à jour des infos Mail Infirmière OK", "Maj infos email Infirmière", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        //Maj des infos concernants la gestion email SMA
        private void MajEmailSMA()
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            bool activation = false;

            if(cBoxMailSMA.Checked)
                activation = true;
            else 
                activation = false;

            try
            {
                string sqlstr0 = @"UPDATE param_divers 
                                 SET EmailMutuaideFrom = @Mail, PassMailMutuaide = @PassMail, ActivationMailSMA = @Activation";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Mail", tBoxMailRepSMA.Text);
                cmd.Parameters.AddWithValue("PassMail", tBoxPassMailRepSMA.Text);
                cmd.Parameters.AddWithValue("Activation", activation);

                cmd.ExecuteNonQuery();    //on execute       
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la mise à jour des infos Mail SMA, table param_divers: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            MessageBox.Show("Mise à jour des infos Mail SMA OK", "Maj infos email SMA", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void tBZGeoLatN_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumerique = false;

            //Pour les nombres du haut du clavier ET les nombres du pavé numérique
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) &&
                 (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back && e.KeyCode != Keys.Decimal)
            {
                nonNumerique = true;
                Console.WriteLine("KO");
            }
        }

        private void tBZGeoLatN_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (nonNumerique == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }

        private void bValideCoord_Click(object sender, EventArgs e)
        {
            MAJCoordonnees();

            timer1.Enabled = true;     
        }

        private void bValideDivers_Click(object sender, EventArgs e)
        {
            //On met à jour la table du Param_Divers               
            if (NbFicheVisite.Text != "")
            {
                MajDivers();
            }

            timer1.Enabled = true;     
        }

        //Durée d'affichage des messages dans le dispatch
        private void tBoxDureeMessages_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumerique = false;

            //Pour les nombres du haut du clavier ET les nombres du pavé numérique
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) &&
                 (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
            {
                nonNumerique = true;
                Console.WriteLine("KO");
            }


            if (e.KeyCode == Keys.Enter)
            {
                //On met à jour la table du Param_Divers               
                if (tBoxDureeMessages.Text != "")
                {
                    int val = int.Parse(tBoxDureeMessages.Text);
                    MAJDureeMessageDispatch(val);
                }
            }
        }

        private void tBoxDureeMessages_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (nonNumerique == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }

        private void NbFicheVisite_KeyDown(object sender, KeyEventArgs e)
        {
            nonNumerique = false;

            //Pour les nombres du haut du clavier ET les nombres du pavé numérique
            if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) &&
                 (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back)
            {
                nonNumerique = true;
                Console.WriteLine("KO");
            }          
        }

        private void NbFicheVisite_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Si c'est un non numérique
            if (nonNumerique == true)
            {
                //On stop l'entrée du caractère dans le controle
                e.Handled = true;
            }
        }

        private void bValideMailTA_Click(object sender, EventArgs e)
        {
            MajEmailTA();
            timer1.Enabled = true;
        }

        private void bValideMailInf_Click(object sender, EventArgs e)
        {
            MajEmailInfirmiere();
            timer1.Enabled = true;
        }

        private void bValideMailSMA_Click(object sender, EventArgs e)
        {
            MajEmailSMA();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }      
    }
}
