using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using MySqlConnector;


namespace SRegulV2
{
    class FonctionsAppels
    {

        //**************Pour la crypto********************************
        /*public static string ProtectPassword(string clearPassword)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(clearPassword);
            byte[] protectedBytes = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(protectedBytes);
        }

        public static string UnprotectPassword(string protectedPassword)
        {                                       
            byte[] protectedBytes = Convert.FromBase64String(protectedPassword);
            
            byte[] bytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
     
            return Encoding.UTF8.GetString(bytes);
           }*/

        //*********************************************************************
        
        
        //Retourne les éléments de la visite en fonction de son n°
        public static DataSet RetourneVisite(int NumAppel)
        {
            DataSet DSResult = new DataSet();

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Appels
                string sqlstr0 = "SELECT * FROM appels WHERE Num_Appel = @Appel";
                cmd.CommandText = sqlstr0;
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("Appel", NumAppel);

                DSResult.Tables.Add("Appels");
                DSResult.Tables["Appels"].Load(cmd.ExecuteReader());    //on execute

                //AdrFacturation
                sqlstr0 = "SELECT * FROM adrfacturation WHERE Num_Appel = @Appel";
                cmd.CommandText = sqlstr0;
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("Appel", NumAppel);

                DSResult.Tables.Add("adrfacturation");
                DSResult.Tables["adrfacturation"].Load(cmd.ExecuteReader());    //on execute

                //SuiviAppel
                sqlstr0 = "SELECT * FROM suiviappel WHERE Num_Appel = @Appel ORDER BY Id";
                cmd.CommandText = sqlstr0;
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("Appel", NumAppel);

                DSResult.Tables.Add("SuiviAppel");
                DSResult.Tables["SuiviAppel"].Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la récupération de la visite. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                //Console.WriteLine("Erreur lors de la récupération de la visite. " + e.Message);
                DSResult = null;
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return DSResult;
        }


        //Retourne les éléments du Rendez-vous en fonction de son n°
        public static DataSet RetourneVisiteRDV(int NumVisiteRDV)
        {
            DataSet DSResult = new DataSet();

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Appels
                string sqlstr0 = "SELECT a.*, CONCAT(m.Nom, ' ', m.Prenom) as Medecin " +
                                 " FROM appelsRDV a LEFT JOIN medecins m ON m.CodeMedecin = a.CodeMedecin " +
                                 " WHERE Num_Appel_RDV = @NumVisiteRDV";

                cmd.CommandText = sqlstr0;
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("NumVisiteRDV", NumVisiteRDV);

                DSResult.Tables.Add("AppelsRDV");
                DSResult.Tables["AppelsRDV"].Load(cmd.ExecuteReader());    //on execute

                //AdrFacturation
                sqlstr0 = "SELECT * FROM adrfacturationRDV WHERE Num_Appel_RDV = @NumVisiteRDV";
                cmd.CommandText = sqlstr0;
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("NumVisiteRDV", NumVisiteRDV);

                DSResult.Tables.Add("adrfacturationRDV");
                DSResult.Tables["adrfacturationRDV"].Load(cmd.ExecuteReader());    //on execute                      
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la récupération de la visite. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                //Console.WriteLine("Erreur lors de la récupération de la visite. " + e.Message);
                DSResult = null;
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return DSResult;
        }

        //Nettoie les RDV antérieur à aujourd'hui
        //Suppression des rendez vous de la veille
        public static string SupprimeRDVPerime()
        {
            string retour = "KO";

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string SqlStr0 = "";
            string SqlStr1 = "";

            try
            {
                MySqlTransaction trans;   //Déclaration d'une transaction

                //Def des requettes
                //On supprimer d'abord adrfacturationRDV, puis appelsRDV                
                SqlStr0 = "DELETE FROM adrfacturationRDV " +
                          "WHERE Num_Appel_RDV in (SELECT Num_Appel_RDV FROM appelsRDV " +
                                                   " WHERE DATE(DateRDV) <= CURDATE() - INTERVAL 1 DAY)";
                SqlStr1 = "DELETE FROM appelsRDV WHERE DATE(DateRDV) <= CURDATE() - INTERVAL 1 DAY";               

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
                    Console.WriteLine("Erreur lors de la suppression de la visite sur RDV. " + e.Message);
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
                Console.WriteLine("Erreur lors de la suppression de la visite sur RDV. " + e.Message);
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


        public static int NombreRDV()
        {
            int NombreRDV = 0;

            //On affiche tous les rendez-vous à venir          
            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //AppelsRDV
                string sqlstr0 = "SELECT count(*)" +
                                 " FROM appelsRDV a LEFT JOIN medecins m ON m.CodeMedecin = a.CodeMedecin " +
                                 " WHERE DateRDV >= CURDATE() ORDER BY DateRDV DESC";

                cmd.CommandText = sqlstr0;

                DataTable dtNbRdv = new DataTable();

                dtNbRdv.Load(cmd.ExecuteReader());    //on execute

                if (dtNbRdv != null && dtNbRdv.Rows.Count > 0)
                    NombreRDV = int.Parse(dtNbRdv.Rows[0][0].ToString());               

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la récupération de la liste des rendez-vous. " + ex.Message);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return NombreRDV;
        }


        //Vérification si c'est un doublon
        public static DataTable VerifSiDoublon(string Nom, string Prenom, string DateNaiss)
        {
            DataTable dtRetour = new DataTable();

            //Chaine de connection                                  
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();

            SqlConnection dbConnection = new SqlConnection(connex);

            dbConnection.Open();

            SqlCommand cmd = new SqlCommand();

            cmd.Connection = dbConnection;

            //On lance la recherche
            string sqlstr0 = "SELECT p.Nom, p.Prenom, p.DateNaissance, p.NumeroDansRue, p.Rue, p.CodePostal, p.Commune, ISNULL(ab.TeleAlarm, 'N') TeleAlarme";
            sqlstr0 += " FROM tablepersonne p INNER JOIN tablepatient tp ON p.IdPersonne = tp.IdPersonne ";
            sqlstr0 += "                      LEFT JOIN (SELECT IdPatient, 'O' as TeleAlarm FROM ta_abonnement WHERE archive = 0) as ab ON tp.IdPatient = ab.IdPatient";
            sqlstr0 += " WHERE p.Nom = @Nom AND p.Prenom = @Prenom AND p.DateNaissance = @DateNaiss";

            cmd.CommandText = sqlstr0;

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Nom", Nom);
            cmd.Parameters.AddWithValue("Prenom", Prenom);
            cmd.Parameters.AddWithValue("DateNaiss", convertDateMaria(DateNaiss, "Texte"));

            dtRetour.Load(cmd.ExecuteReader());    //on execute

            return dtRetour;
        }

        //Fonction qui retourne les 10 derniers appels pour un N° de Tel et N° de personnes données
        //Si rien n'est trouvé Dans Base Regul, chercher dans Smart
        public static DataTable Derniers10Appels(string NumTel, int NumPersonne, string DateJ)
        {
            DataTable dtRetour = new DataTable();
 
            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Les 10 derniers appels
                string sqlstr0 = "SELECT s.DateOp, m.LibelleMotif, CONCAT(d.Nom, ' ', d.Prenom) as NomP,";
                sqlstr0 += "       a.Num_Appel as NConsultation";
                sqlstr0 += " FROM appels a INNER JOIN motif m ON m.IdMotif = a.IdMotif1";
                sqlstr0 += "               INNER JOIN suiviappel s ON s.Num_Appel = a.Num_Appel";
                sqlstr0 += "               LEFT JOIN medecins d ON d.CodeMedecin = a.CodeMedecin";
                sqlstr0 += " WHERE s.Type_Operation = 'Création'";
                sqlstr0 += " AND a.Tel_Appel = @Tel";
                sqlstr0 += " AND a.Num_Personne = @NumPersonne";
                sqlstr0 += " AND s.DateOp < @Actuel";
                sqlstr0 += " ORDER BY s.DateOp DESC";
                sqlstr0 += " LIMIT 10";
             
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Tel", NumTel);
                cmd.Parameters.AddWithValue("NumPersonne", NumPersonne);
                cmd.Parameters.AddWithValue("Actuel", convertDateMaria(DateJ, "MariaDb"));
          
                dtRetour.Load(cmd.ExecuteReader());    //on execute

                //SI pas de résultats dans la base régul...On cherche dans la base SmartRapport
                if (dtRetour.Rows.Count == 0)  
                {
                    dtRetour = Derniers10AppelsSmart(NumTel, NumPersonne, DateJ);
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la récupération des 10 derniers appels (Regul). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);               
                dtRetour = null;
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtRetour;
        }


        //Fonction qui retourne les 20 derniers appels pour un Nom, prénom  et date de naissance d'une personnes données
        //Si rien n'est trouvé Dans Base Regul, chercher dans Smart
        public static DataTable Derniers20Appels(string Nom, string Prenom, string DateNaiss)
        {
            DataTable dtRetour = new DataTable();

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Les 20 derniers appels
                string sqlstr0 = "SELECT s.DateOp, m.LibelleMotif, CONCAT(d.Nom, ' ', d.Prenom) as NomP,";
                sqlstr0 += "       a.Num_Appel as NConsultation";
                sqlstr0 += " FROM appels a INNER JOIN motif m ON m.IdMotif = a.IdMotif1";
                sqlstr0 += "               INNER JOIN suiviappel s ON s.Num_Appel = a.Num_Appel";
                sqlstr0 += "               LEFT JOIN medecins d ON d.CodeMedecin = a.CodeMedecin";
                sqlstr0 += " WHERE s.Type_Operation = 'Création'";
                sqlstr0 += " AND a.Nom = @Nom";
                sqlstr0 += " AND a.Prenom = @Prenom";
                sqlstr0 += " AND a.DateNaissance = @DateNaiss";                
                sqlstr0 += " ORDER BY s.DateOp DESC";
                sqlstr0 += " LIMIT 20";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();               
                cmd.Parameters.AddWithValue("Nom", Nom);
                cmd.Parameters.AddWithValue("Prenom", Prenom);
                cmd.Parameters.AddWithValue("DateNaiss", convertDateMaria(DateNaiss, "MariaDb"));

                dtRetour.Load(cmd.ExecuteReader());    //on execute

                //SI pas de résultats dans la base régul...On cherche dans la base SmartRapport
                if (dtRetour.Rows.Count == 0)
                {
                    dtRetour = Derniers20AppelsSmart(Nom, Prenom, DateNaiss);
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la récupération des 20 derniers appels (Regul). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                dtRetour = null;
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtRetour;
        }


        public static DateTime? RechercheAccord(string Nom, string Prenom, string DateNaissance)
        {
            DateTime? dtRetour = null;
                            
            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {                 
                string sqlstr0 = "SELECT a.DateAccord";
                sqlstr0 += " FROM appels a ";
                sqlstr0 += " WHERE a.Nom = @Nom";
                sqlstr0 += " AND a.Prenom = @Prenom";
                sqlstr0 += " AND a.DateNaissance = @DateNaiss";
                sqlstr0 += " AND a.DateAccord is not NULL";                

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Nom", Nom);
                cmd.Parameters.AddWithValue("Prenom", Prenom);
                cmd.Parameters.AddWithValue("DateNaiss", convertDateMaria(DateNaissance, "MariaDb"));

                DataTable dtReponse = new DataTable();
                dtReponse.Load(cmd.ExecuteReader());    //on execute

                //SI on a un résultat
                if (dtReponse.Rows.Count > 0)
                {
                    dtRetour = DateTime.Parse(dtReponse.Rows[0]["DateAccord"].ToString());
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche de l'accord (Regul). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                dtRetour = null;
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtRetour;
        }


        #region recup diverses données
        //******************   calcul du temps epoch ou UnixTime (temps écoulé en seconde depuis le 01.01.1970)
        public static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        //Fonction qui calcule l'âge à partir de la date de naissance                                       
        public static int CalculeAge(DateTime anniversaire)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - anniversaire.Year;
            if (anniversaire > now.AddYears(-age))
                age--;
            return age;
        }


        //Fonction qui vérifie que ce smartphone n'est pas déjà attribué à quelqu'un qui est en garde actuellement
        public static string VerifSmartphone(int IdSmartphone)
        {
            string retour = "KO";
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Table Engarde
                string sqlstr0 = "SELECT * FROM engarde WHERE IdSmartphone = " + IdSmartphone;
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();             

                DataTable Result = new DataTable();
                Result.Load(cmd.ExecuteReader());    //on execute

                if (Result.Rows.Count == 0)   //Si on a rien (il est libre)             
                    retour = "OK";
                
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche de la disponibilité du smartphone la table engarde :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return retour;
        }

        //Nouvel IdMotif
        public static string NvxIdMotif()
        {
            string Retour = "";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Table Motif
                string sqlstr0 = "select max(CONVERT(idMotif, INTEGER)) +1  from motif";
                cmd.CommandText = sqlstr0;

                DataTable Result = new DataTable();
                Result.Load(cmd.ExecuteReader());    //on execute

                if (Result.Rows.Count > 0)   //Si on a un enregistrement
                {
                    if (Result.Rows[0][0] != DBNull.Value)
                        Retour = Result.Rows[0][0].ToString();
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche d'un nouvel IdMotif dans la table Motif:" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        
        //Remonte le libellé du motif à partir de l'IdMotif
        public static string LibelleMotif(string IdMotif)
        {
            string Retour = "";

            if (IdMotif != "")
            {
                string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
                MySqlConnection dbConnection = new MySqlConnection(connex);

                dbConnection.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = dbConnection;

                try
                {
                    //Table Motif
                    string sqlstr0 = "SELECT LibelleMotif FROM motif WHERE IdMotif = @Motif";
                    cmd.CommandText = sqlstr0;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("Motif", IdMotif);

                    DataTable Result = new DataTable();
                    Result.Load(cmd.ExecuteReader());    //on execute

                    if (Result.Rows.Count > 0)   //Si on a un enregistrement
                    {
                        if (Result.Rows[0][0] != DBNull.Value)
                            Retour = Result.Rows[0][0].ToString();
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche du libellé dans la table Motif:" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            return Retour;
        }


        //Remonte les infos du motif à partir de l'IdMotif
        public static DataTable InfosMotif(string IdMotif)
        {
            DataTable dtRetour = new DataTable();

            if (IdMotif != "")
            {
                string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
                MySqlConnection dbConnection = new MySqlConnection(connex);

                dbConnection.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = dbConnection;

                try
                {
                    //Table motif
                    string sqlstr0 = "SELECT * FROM motif WHERE IdMotif = @Motif";
                    cmd.CommandText = sqlstr0;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("Motif", IdMotif);

                    dtRetour.Load(cmd.ExecuteReader());    //on execute
                   
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche des infos du motif dans la table Motif:" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            return dtRetour;
        }


        //Vérifie l'existence d'un enregistrement
        public static string ExisteEnreg(Int32 Critere, string Table)
        {
            string Retour = "KO";

            //On recherche l'enregistrement selon les critères
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                if (Table == "adrfacturation")
                {
                    //Table AdrFacturation
                    string sqlstr0 = "SELECT * FROM adrfacturation WHERE Num_Appel = @Appel";
                    cmd.CommandText = sqlstr0;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("Appel", Critere);

                    DataTable dtNunAppel = new DataTable();
                    dtNunAppel.Load(cmd.ExecuteReader());    //on execute

                    if (dtNunAppel.Rows.Count > 0)   //Si on a un enregistrement
                        Retour = "OK";
                    else Retour = "KO";
                }
                else if(Table == "adrfacturationRDV")
                {
                    //Table AdrFacturationRDV
                    string sqlstr0 = "SELECT * FROM adrfacturationRDV WHERE Num_Appel_RDV = @Appel";
                    cmd.CommandText = sqlstr0;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("Appel", Critere);

                    DataTable dtNunAppel = new DataTable();
                    dtNunAppel.Load(cmd.ExecuteReader());    //on execute

                    if (dtNunAppel.Rows.Count > 0)   //Si on a un enregistrement
                        Retour = "OK";
                    else Retour = "KO";
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche dans la table " + Critere + " :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Recup du nom des utilisateurs à partir du codeUtilisateur
        public static string NomUtilisateur(string CodeUtilisateur)
        {
            string Retour = "";
   
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {                
                string sqlstr0 = "SELECT CONCAT(Prenom, ' ', Nom) as NomUtilisateur FROM utilisateur WHERE IdUtilisateur = @CodeUtilisateur ";
                sqlstr0 += " UNION ";
                sqlstr0 += " SELECT CONCAT(Prenom, ' ', Nom) as NomUtilisateur FROM medecins WHERE CodeMedecin = @CodeUtilisateur ";
                sqlstr0 += " GROUP by CONCAT(Prenom, ' ', Nom)";
                
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeUtilisateur", CodeUtilisateur);

                DataTable NomPrenomUtil = new DataTable();
                NomPrenomUtil.Load(cmd.ExecuteReader());    //on execute

                if (NomPrenomUtil.Rows.Count > 0)   //Si on a un enregistrement
                {
                    if (NomPrenomUtil.Rows[0][0] != DBNull.Value)
                        Retour = NomPrenomUtil.Rows[0][0].ToString();
                    else Retour = "Anomyme";
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche du Nom de l'utilisateur dans la table Utilisateur :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Recup du nom d'un médecin à partir de son CodeMedecin
        public static string NomMedecin(string CodeMedecin)
        {
            string Retour = "";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Table medecins
                string sqlstr0 = "SELECT CONCAT(Prenom, ' ', Nom) as NomMedecin FROM medecins WHERE CodeMedecin = @CodeMedecin";
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeMedecin", CodeMedecin);

                DataTable NomPrenomMed = new DataTable();
                NomPrenomMed.Load(cmd.ExecuteReader());    //on execute

                if (NomPrenomMed.Rows.Count > 0)   //Si on a un enregistrement
                {
                    if (NomPrenomMed.Rows[0][0] != DBNull.Value)
                        Retour = NomPrenomMed.Rows[0][0].ToString();
                    else Retour = "Anomyme";
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche du Nom du médecin dans la table medecin :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Recup du CodeMedecin d'un médecin à partir de son Nom Prénom
        public static string CodeMedecin(string NomPrenom)
        {
            string Retour = "";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Table medecins
                string sqlstr0 = "SELECT CodeMedecin FROM medecins WHERE CONCAT(Nom, ' ', Prenom) = @NomPrenom";
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("NomPrenom", NomPrenom);

                DataTable CodeMed = new DataTable();
                CodeMed.Load(cmd.ExecuteReader());    //on execute

                if (CodeMed.Rows.Count > 0)   //Si on a un enregistrement
                {
                    if (CodeMed.Rows[0][0] != DBNull.Value)
                        Retour = CodeMed.Rows[0][0].ToString();
                    else Retour = "Non Trouvé";
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche du CodeMedecin à partir de son nom table medecin :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Recup du tel du médecin à partir de son CodeMedecin
        public static string RecupNumTelMedecin(string CodeMedecin)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Table medecins
                string sqlstr0 = "SELECT NumTel FROM smartphone WHERE CodeMedecin = @CodeMedecin";
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeMedecin", CodeMedecin);

                DataTable TelMed = new DataTable();
                TelMed.Load(cmd.ExecuteReader());    //on execute

                if (TelMed.Rows.Count > 0 && TelMed != null)   //Si on a un enregistrement
                {
                    Retour = TelMed.Rows[0][0].ToString();                    
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche du Telephone du médecin dans la table smarphone :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Retourne la date/heure d'une opération dans l'historique d'un appel
        public static DateTime? HeureOP(Int32 NumAppel, string TypeOp)
        {
            DateTime? Retour = null;

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT MAX(DATE_FORMAT(DateOp, '%H:%i')) AS heureOp FROM suiviappel";
                sqlstr0 += " WHERE Num_Appel = @NumAppel AND Type_Operation = @Operation";
                cmd.CommandText = sqlstr0;

                cmd.Parameters.AddWithValue("NumAppel", NumAppel);
                cmd.Parameters.AddWithValue("Operation", TypeOp);

                DataTable dtDateOp = new DataTable();
                dtDateOp.Load(cmd.ExecuteReader());    //on execute

                if (dtDateOp.Rows.Count > 0)   //Si on a un enregistrement
                {
                    if (dtDateOp.Rows[0][0] != DBNull.Value)
                        Retour = DateTime.Parse(dtDateOp.Rows[0][0].ToString());
                }
                else
                    Retour = null;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche de la date de " + TypeOp + " dans la table SuiviAppel :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


       //Fonction qui converti la date vers MariaDB (yyyy-MM-dd HH:mm:ss) et vis versa
       public static string convertDateMaria(string DateAconvertir, string Sens)
        {
           string Retour = "";
           
           if (DateAconvertir != "" && DateAconvertir != "  .  ." && DateAconvertir != "  .  . 00:00:00")
           {
               if (Sens == "MariaDb")
               {
                   try
                   {
                       DateTime MyDate = DateTime.Parse(DateAconvertir);                                          
                       Retour = MyDate.ToString("yyyy-MM-dd HH:mm:ss");
                   }
                   catch(Exception e)
                   {
                       System.Windows.Forms.MessageBox.Show("Erreur lors de la convertion de la date " + DateAconvertir + " :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                   }                   
               }
               else if (Sens == "Texte")
               {
                   try
                   {
                       DateTime MyDate = DateTime.Parse(DateAconvertir);
                       MyDate.ToString("dd.MM.yyyy HH:mm:ss");
                       Retour = MyDate.ToString("dd.MM.yyyy HH:mm:ss");
                   }
                   catch (Exception e)
                   {
                       System.Windows.Forms.MessageBox.Show("Erreur lors de la convertion de la date " + DateAconvertir + " :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                   }                   
               }
           }
           else
            {
                //Pas de date, on en met une par défaut 
                if (Sens == "MariaDb")
                {
                    Retour = "1900-01-01 00:00:00";
                }
                else if (Sens == "Texte")
                {
                    Retour = "01.01.1900 00:00:00";
                }                   
            }

           return Retour;
        }

       //Retourne les coordonnées GPS de l'adresse
       public static DataTable RecupCoordonnees(string NumAppel)
       {
           DataTable dtResult = new DataTable();

           string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
           MySqlConnection dbConnection = new MySqlConnection(connex);

           dbConnection.Open();
           MySqlCommand cmd = new MySqlCommand();

           cmd.Connection = dbConnection;

           string sqlstr0 = "SELECT a.Latitude, a.Longitude FROM appelsencours a WHERE a.Num_Appel = @NumAppel";
          
           try
           {
               cmd.CommandText = sqlstr0;

               cmd.Parameters.Clear();
               cmd.Parameters.AddWithValue("NumAppel", NumAppel);

               dtResult.Load(cmd.ExecuteReader());    //on execute              
           }
           catch (Exception e)
           {
               System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche dans la table AppelsEnCours :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
           }
           finally
           {
               //fermeture des connexions
               if (dbConnection.State == System.Data.ConnectionState.Open)
               {
                   dbConnection.Close();
               }
           }

           return dtResult;
       }

        //Recupère les infos d'un patient, à partir du n° de visite
        public static DataTable RecupInfosPatient(string NumAppel)
        {
            DataTable dtResult = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT a.Nom, a.Prenom, a.DateNaissance FROM appels a";
            sqlstr0 += "  WHERE a.Num_Appel = @NumAppel";
            
            try
            {
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("NumAppel", NumAppel);

                dtResult.Load(cmd.ExecuteReader());    //on execute              
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche dans la table Appels :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtResult;
        }
    

        #endregion

        #region rafraichissement
        //On utilise un compteur qui s'incrémente à chaque nvlle operation (y compris distante) donc penser à l'appeler à chaque action
        //Cela permet de ne rafraichir que lorsque c'est nécessaire.
        public static void IncrementeRafraichissement()
        {
           //Chaine de connection... ici on attaque MariaDB
           string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
           MySqlConnection dbConnection = new MySqlConnection(connex);

           dbConnection.Open();
           MySqlCommand cmd = new MySqlCommand();

           cmd.Connection = dbConnection;

           //Dans une transaction
           MySqlTransaction trans;
           
           //Ouverture d'une transaction
           trans = dbConnection.BeginTransaction();
                   
           //MAJ de la table Rafraichissement
           try
           {               
               //on incrémente le compteur (s'il dépasse 3000000 on remet à 1)
               string SqlStr0 = "UPDATE rafraichissement SET Compteur = CASE WHEN Compteur > 3000000 THEN 1 ELSE Compteur + 1 END";                                        
              
               //on execute les requettes                                       
               cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                  
               //on valide la transaction
               trans.Commit();              
           }
           catch (Exception)
           {
               trans.Rollback();
               //System.Windows.Forms.MessageBox.Show("Erreur lors de la maj du compteur(transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

       //Si le compteur de la base est différent de celui stocké en local, alors on rafraichi et on met à jour la valeur local du cpt
       public static string Rafraichir(int cpt)
       {
           string retour = "Non";     //Par défaut, pour ne pas bloquer le prog en cas de problèmes

           string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
           MySqlConnection dbConnection = new MySqlConnection(connex);

           dbConnection.Open();
           MySqlCommand cmd = new MySqlCommand();

           cmd.Connection = dbConnection;

           try
           {
               //Table rafraichissement
               string sqlstr0 = "SELECT Compteur FROM rafraichissement";
               cmd.CommandText = sqlstr0;             

               DataTable dtCompteur = new DataTable();
               dtCompteur.Load(cmd.ExecuteReader());    //on execute

               if (dtCompteur.Rows.Count > 0)   //Si on a un enregistrement
               {
                   //Il est différent donc on le met à jour en local
                   if (int.Parse(dtCompteur.Rows[0][0].ToString()) != cpt)
                   {
                       Form1.compteur = int.Parse(dtCompteur.Rows[0][0].ToString());

                       retour = "Oui";
                   }
                   else retour = "Non";
               }
           }
           catch (Exception)
           {
              // System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche du médecin dans la table Status_Visite :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
           }
           finally
           {
               //fermeture des connexions
               if (dbConnection.State == System.Data.ConnectionState.Open)
               {
                   dbConnection.Close();
               }
           }

           return retour;
       }
       #endregion


       #region Chargement des différentes listes
       //Charge la liste des SmartPhones
        public static DataTable ChargeListeSmarphone()
        {
            DataTable dtSmartphone = new DataTable();
            
            //On charge la liste des Smartphones
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT * FROM smartphone";

                cmd.CommandText = sqlstr0;
              
                dtSmartphone.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des smartphones :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtSmartphone;
        }
       

        //Chargement de la liste des téléphones utils
        public static DataTable ChargeListeTelsUtils()
        {
            DataTable dtTelUtils = new DataTable();

            //On charge la liste des Smartphones
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT * FROM telephones_utils ORDER BY Categorie, Sous_Categorie, Libelle";

                cmd.CommandText = sqlstr0;

                dtTelUtils.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement de la liste des téléphones utils :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtTelUtils;
        }

        //Charge la liste des médecins selon le critère (tous, seulement les actifs, non garde, en garde)
        public static DataTable ChargeListeMedecins(string critere)
        {
            DataTable dtMedecin = new DataTable();

            //On charge la liste des Smartphones
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT m.CodeMedecin, m.LibelleOrg, m.Nom, m.Prenom, DATE_FORMAT(m.DateDebActiv,'%d.%m.%Y') AS DateDeb,";
                sqlstr0 += " DATE_FORMAT(m.DateFinActiv,'%d.%m.%Y') AS DateFin, GMS, Concordat, Email, Titre, Signature, m.TitreEmployee ";

                if (critere == "tous")
                    sqlstr0 += " FROM medecins m ORDER BY m.Nom";

                if (critere == "actifs")
                    sqlstr0 += " FROM medecins m WHERE m.LibelleOrg = 'Sos Médecins' AND m.DateFinActiv IS NULL";

                if (critere == "actifs toutes les org")
                    sqlstr0 += " FROM medecins m WHERE m.DateFinActiv IS NULL ORDER BY m.LibelleOrg, m.Nom";

                if (critere == "non garde")
                {
                    sqlstr0 += " FROM medecins m WHERE m.LibelleOrg = 'Sos Médecins' AND m.DateFinActiv IS NULL AND m.codeMedecin <> '0'";
                    sqlstr0 += " AND m.CodeMedecin not in (select codeMedecin FROM engarde)";
                    sqlstr0 += " ORDER BY m.Nom ";
                }

                if (critere == "en garde")
                {
                    //On refait la requete
                    sqlstr0 = "SELECT e.IdStatusGarde, e.IdGarde, e.CodeMedecin, e.IdSmartphone, e.StatusGarde, e.Nb_Visite, e.DateDebGarde, ";
                    sqlstr0 += " m.Nom, m.Prenom, g.TypeGarde, g.HeureDeb, g.HeureFin, s.NomSmartphone ";
                    sqlstr0 += " FROM medecins m INNER JOIN engarde e ON e.CodeMedecin = m.CodeMedecin ";
                    sqlstr0 += "                 INNER JOIN garde g ON g.IdGarde = e.IdGarde ";
                    sqlstr0 += "                 INNER JOIN smartphone s ON s.IdSmartphone = e.IdSmartphone ";
                    sqlstr0 += " WHERE m.LibelleOrg = 'Sos Médecins'";
                    sqlstr0 += " ORDER BY e.Nb_Visite desc";
                }

                if (critere == "en garde2")
                {
                    //On refait la requete (mais en concaténant les noms prénoms et en recupérant la géolocalisation)
                    sqlstr0 = "SELECT e.IdStatusGarde, e.IdGarde, e.CodeMedecin, e.IdSmartphone, e.StatusGarde, e.Nb_Visite, e.DateDebGarde, ";
                    sqlstr0 += " CONCAT(m.Nom, ' ', m.Prenom) as NomMed, g.TypeGarde, g.HeureDeb, g.HeureFin, s.NomSmartphone, o.Lat, o.Lng, o.Vitesse, o.Alarme ";
                    sqlstr0 += " FROM medecins m INNER JOIN engarde e ON e.CodeMedecin = m.CodeMedecin ";
                    sqlstr0 += "                 INNER JOIN garde g ON g.IdGarde = e.IdGarde ";
                    sqlstr0 += "                 INNER JOIN smartphone s ON s.IdSmartphone = e.IdSmartphone ";
                    sqlstr0 += "                 INNER JOIN geoloc o ON o.CodeMedecin = m.CodeMedecin ";                    
                    sqlstr0 += " WHERE m.LibelleOrg = 'Sos Médecins'";                    
                    sqlstr0 += " ORDER BY e.Nb_Visite desc";
                }

                if (critere == "en pause")
                {
                    //On refait la requete
                    sqlstr0 = "SELECT e.IdStatusGarde, e.IdGarde, e.CodeMedecin, e.IdSmartphone, e.StatusGarde, e.Nb_Visite, e.DateDebGarde, ";
                    sqlstr0 += " CONCAT(m.Nom, ' ', m.Prenom) as NomMed, g.TypeGarde, g.HeureDeb, g.HeureFin, s.NomSmartphone ";
                    sqlstr0 += " FROM medecins m INNER JOIN engarde e ON e.CodeMedecin = m.CodeMedecin ";
                    sqlstr0 += "                 INNER JOIN garde g ON g.IdGarde = e.IdGarde";
                    sqlstr0 += "                 INNER JOIN smartphone s ON s.IdSmartphone = e.IdSmartphone";
                    sqlstr0 += " WHERE  m.LibelleOrg = 'Sos Médecins'";
                    sqlstr0 += " AND e.StatusGarde = 'En pause'";
                    sqlstr0 += " ORDER BY e.Nb_Visite desc";
                }

                if (critere == "pause et dispo")
                {
                    //On refait la requete
                    sqlstr0 = "SELECT e.IdStatusGarde, e.IdGarde, e.CodeMedecin, e.IdSmartphone, e.StatusGarde, e.Nb_Visite, e.DateDebGarde, ";
                    sqlstr0 += " CONCAT(m.Nom, ' ', m.Prenom) as NomMed, g.TypeGarde, g.HeureDeb, g.HeureFin, s.NomSmartphone ";
                    sqlstr0 += " FROM medecins m INNER JOIN engarde e ON e.CodeMedecin = m.CodeMedecin";
                    sqlstr0 += "                 INNER JOIN garde g ON g.IdGarde = e.IdGarde";
                    sqlstr0 += "                 INNER JOIN smartphone s ON s.IdSmartphone = e.IdSmartphone";
                    sqlstr0 += " WHERE m.LibelleOrg = 'Sos Médecins'";
                    sqlstr0 += " AND (e.StatusGarde = 'En pause' OR e.StatusGarde = 'Disponible')";
                    sqlstr0 += " ORDER BY e.Nb_Visite desc";
                }
                

                cmd.CommandText = sqlstr0;

                dtMedecin.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des médecins :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtMedecin;
        }


        //Charge la liste des infirmières selon le critère (tous, seulement les actifs, seulement en garde avec visite, en garde)
        public static DataTable ChargeListeInfirmiere(string critere)
        {
            DataTable dtMedecin = new DataTable();

            //On charge la liste des Smartphones
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {                                  
                string sqlstr0 = "SELECT m.CodeMedecin, m.LibelleOrg, m.Nom, m.Prenom, DATE_FORMAT(m.DateDebActiv,'%d.%m.%Y') AS DateDeb,";
                sqlstr0 += " DATE_FORMAT(m.DateFinActiv,'%d.%m.%Y') AS DateFin, Email ";

                if (critere == "tous")
                    sqlstr0 += " FROM medecins m WHERE m.LibelleOrg <> 'Sos Médecins' ORDER BY m.Nom";

                if (critere == "actives")
                    sqlstr0 += " FROM medecins m WHERE m.LibelleOrg <> 'Sos Médecins' AND m.DateFinActiv IS NULL";

                if (critere == "non garde")
                {                                        
                    sqlstr0 += " FROM medecins m WHERE m.LibelleOrg <> 'Sos Médecins' AND m.DateFinActiv IS NULL AND codeMedecin <> '0'";
                    sqlstr0 += " AND m.CodeMedecin not in (select codeMedecin FROM engarde)";
                    sqlstr0 += " ORDER BY m.Nom ";
                }
                

                if (critere == "en garde")
                {
                    //On refait la requete
                    sqlstr0 = "SELECT e.IdStatusGarde, e.IdGarde, e.CodeMedecin, e.IdSmartphone, e.StatusGarde, e.Nb_Visite, e.DateDebGarde, ";
                    sqlstr0 += " m.Nom, m.Prenom, m.LibelleOrg, g.TypeGarde, g.HeureDeb, g.HeureFin, s.NomSmartphone ";
                    sqlstr0 += " FROM medecins m INNER JOIN engarde e ON e.CodeMedecin = m.CodeMedecin ";
                    sqlstr0 += "                 INNER JOIN garde g ON g.IdGarde = e.IdGarde ";
                    sqlstr0 += "                 INNER JOIN smartphone s ON s.IdSmartphone = e.IdSmartphone ";
                    sqlstr0 += " WHERE m.LibelleOrg <> 'Sos Médecins'";
                    sqlstr0 += " ORDER BY e.Nb_Visite desc";                   
                }

                if (critere == "en garde2")
                {
                    //On refait la requete (mais en concaténant les noms prénoms et en recupérant la géolocalisation)
                    sqlstr0 = "SELECT e.IdStatusGarde, e.IdGarde, e.CodeMedecin, e.IdSmartphone, e.StatusGarde, e.Nb_Visite, e.DateDebGarde, ";
                    sqlstr0 += " CONCAT(m.Nom, ' ', m.Prenom) as NomMed, m.LibelleOrg, g.TypeGarde, g.HeureDeb, g.HeureFin, s.NomSmartphone, o.Lat, o.Lng, o.Vitesse, o.Alarme ";
                    sqlstr0 += " FROM medecins m INNER JOIN engarde e ON e.CodeMedecin = m.CodeMedecin ";
                    sqlstr0 += "                 INNER JOIN garde g ON g.IdGarde = e.IdGarde ";
                    sqlstr0 += "                 INNER JOIN smartphone s ON s.IdSmartphone = e.IdSmartphone ";
                    sqlstr0 += "                 INNER JOIN geoloc geo ON geo.CodeMedecin = m.CodeMedecin ";
                    sqlstr0 += " WHERE m.LibelleOrg <> 'Sos Médecins'";
                    sqlstr0 += " ORDER BY e.Nb_Visite desc";                   
                }

                if (critere == "En garde avec details visite")
                {
                    //On refait la requete
                    sqlstr0 = "SELECT e.CodeMedecin, nb_Visite, CONCAT(m.Nom, ' ', m.Prenom) as NomMedecin, m.LibelleOrg, ";
                    sqlstr0 += " CONCAT(a.Nom, ' ', a.Prenom) as NomPrenom, CONCAT( a.Adr1, ', ', a.Num_Rue) as Adresse, a.CodePostal, a.Commune, a.Pays, ";
                    sqlstr0 += " (CASE WHEN a.Num_Appel IS NULL THEN -1 ELSE a.Num_Appel END) AS Num_Appel, a.Latitude, a.Longitude, e.StatusGarde ";
                    sqlstr0 += " FROM engarde e LEFT JOIN ";
                    sqlstr0 += "              (SELECT ac.* FROM appelsencours ac INNER JOIN status_visite_org o ON ac.Num_Appel = o.Num_Appel ";
                    sqlstr0 += "               WHERE o.Status IN ('PR', 'AT', 'V')) as a ON e.CodeMedecin = a.CodeMedecin";
                    sqlstr0 += " INNER JOIN medecins m ON m.CodeMedecin = e.CodeMedecin";
                    sqlstr0 += " INNER JOIN garde g ON e.IdGarde = g.IdGarde";
                    sqlstr0 += " WHERE m.LibelleOrg <> 'Sos Médecins' ";
                }

                if (critere == "En garde avec visite")
                {
                    //On refait la requete
                    sqlstr0 = "SELECT m.CodeMedecin, CONCAT(m.Nom, ' ', m.Prenom) as NomMedecin, m.LibelleOrg  ";                                      
                    sqlstr0 += " FROM status_visite_org o INNER JOIN medecins m ON m.CodeMedecin = o.CodeMedOrg";
                    sqlstr0 += " WHERE o.Status IN ('PR', 'AT', 'V')";                    
                    sqlstr0 += " AND o.CodeMedOrg <> -1";
                }


                /* if (critere == "dispo")
                 {
                     //On refait la requete
                     sqlstr0 = "SELECT e.IdStatusGarde, e.IdGarde, e.CodeMedecin, e.IdSmartphone, e.StatusGarde, e.Nb_Visite, e.DateDebGarde, ";
                     sqlstr0 += " CONCAT(m.Nom, ' ', m.Prenom) as NomMed, m.LibelleOrg, g.TypeGarde, g.HeureDeb, g.HeureFin, s.NomSmartphone ";
                     sqlstr0 += " FROM medecins m INNER JOIN engarde e ON e.CodeMedecin = m.CodeMedecin";
                     sqlstr0 += "                 INNER JOIN garde g ON g.IdGarde = e.IdGarde";
                     sqlstr0 += "                 INNER JOIN smartphone s ON s.IdSmartphone = e.IdSmartphone";
                     sqlstr0 += " WHERE m.LibelleOrg <> 'Sos Médecins' ";
                     sqlstr0 += " AND (e.StatusGarde = 'En pause' OR e.StatusGarde = 'Disponible')";
                     sqlstr0 += " ORDER BY e.Nb_Visite desc";
                 }*/


                cmd.CommandText = sqlstr0;

                dtMedecin.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des infirmieres :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtMedecin;
        }

        //Charge les coordonnées GPS du médecin ainsi que l'état de sa garde
        public static DataTable ChargePosGPSMedecins(string CodeMedecin)
        {
            DataTable dtMedecin = new DataTable();

            //On charge la liste des Smartphones
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT e.IdStatusGarde, e.IdGarde, e.CodeMedecin, e.IdSmartphone, e.StatusGarde, e.Nb_Visite, e.DateDebGarde, ";
                sqlstr0 += " CONCAT(m.Nom, ' ', m.Prenom) as NomMed, g.TypeGarde, g.HeureDeb, g.HeureFin, s.NomSmartphone, o.Lat, o.Lng, o.Vitesse, o.Alarme ";
                sqlstr0 += " FROM medecins m, engarde e, garde g, smartphone s, geoloc o ";
                sqlstr0 += " WHERE e.IdGarde = g.IdGarde AND e.IdSmartphone = s.IdSmartphone AND m.CodeMedecin = e.CodeMedecin";
                sqlstr0 += " AND m.CodeMedecin = o.CodeMedecin AND m.CodeMedecin = '" + CodeMedecin + "'";

                cmd.CommandText = sqlstr0;

                dtMedecin.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement du médecins :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtMedecin;
        }


        //Charge la liste des gardes
        public static DataTable ChargeListeGardes()
        {
            DataTable dtGarde = new DataTable();

            //On charge la liste des Smartphones
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT IdGarde, TypeGarde, HeureDeb, HeureFin FROM garde";        

                cmd.CommandText = sqlstr0;

                dtGarde.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des gardes :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtGarde;
        }

        //Charge la liste des organisations
        public static DataTable ChargeListeOrganisation()
        {
            DataTable dtOrganisation = new DataTable();

            //On charge la liste des communes
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT IdOrganisation, LibelleOrg, UtiliseAppli FROM organisation";

                cmd.CommandText = sqlstr0;

                dtOrganisation.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des organisations :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtOrganisation;
        }


        //Charge la liste des communes
        public static DataTable ChargeListeCommune()
        {
            DataTable dtCommune = new DataTable();

            //On charge la liste des communes
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT IdCommune, NomCommune, CodePostal, Pays FROM commune";

                cmd.CommandText = sqlstr0;

                dtCommune.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des communes :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtCommune;
        }


        //Charge la liste des rues
        public static DataTable ChargeListeRue()
        {
            DataTable dtRue = new DataTable();

            //On charge la liste des communes
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT IdRue, NomRue, Commune, CodePostal FROM rue ORDER BY Commune, NomRue";

                cmd.CommandText = sqlstr0;

                dtRue.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des rues :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtRue;
        }


        //Charge la liste des motifs
        public static DataTable ChargeListeMotif()
        {
            DataTable dtMotif = new DataTable();

            //On charge la liste des motifs
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT IdMotif, LibelleMotif, Urgence, TypeMotif FROM motif Order by LibelleMotif";

                cmd.CommandText = sqlstr0;

                dtMotif.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des motifs :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtMotif;
        }

        //Charge la liste des assurances
        public static DataTable ChargeListeAssurance()
        {
            DataTable dtAssurance = new DataTable();

            //On charge la liste des assurances
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT * FROM assurance Order by NomAssurance";

                cmd.CommandText = sqlstr0;

                dtAssurance.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des assurances :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtAssurance;
        }


        //Charge la liste des provenances
        public static DataTable ChargeListeProvenance()
        {
            DataTable dtProvenance = new DataTable();

            //On charge la liste des provenances
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT IdProvenance, LibelleProvenance, CodeProvenance FROM provenance";

                cmd.CommandText = sqlstr0;

                dtProvenance.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des provenances :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtProvenance;
        }

       
        //Charge la liste des utilisateurs selon le critère (tous ou seulement les actifs)
        public static DataTable ChargeListeUtilisateurs(string critere)
        {
            DataTable dtUtilisateur = new DataTable();

            //On charge la liste des Smartphones
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {              
                string sqlstr0 = "SELECT * FROM utilisateur";           
                if (critere == "actifs")
                    sqlstr0 += " WHERE Droit > 0";

                cmd.CommandText = sqlstr0;

                dtUtilisateur.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des utilisateurs :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtUtilisateur;
        }


        //Charge la liste des appels en cours (Non attribués) ou des pré-attribués
        public static DataTable ChargeListeAppelsNonAttribues()
        {
            DataTable dtAppelsNonAttribues = new DataTable();
            
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT a.Num_Appel, a.Urgence, DATE_FORMAT(s2.DateOp, '%H:%i') AS heure, TIMEDIFF(NOW(),s2.DateOp) as Attente, ";
                sqlstr0 += " CONCAT( a.Adr1, ', ', a.Num_Rue) as Adresse, a.CodePostal, a.Commune, a.Pays, CONCAT(a.Nom, ' ', a.Prenom) as NomPrenom, a.DateNaissance, a.Motif1, a.Motif2, a.Provenance, ";
                sqlstr0 += " CASE WHEN s.CodeMedecin = -1 THEN '' ELSE CONCAT(m.Nom,' ', m.Prenom) END as Med, s.Status, a.Assurance, a.CondParticuliere, a.Latitude, a.Longitude, a.PatientRappel ";
                sqlstr0 += " FROM appelsencours a inner join status_visite s ON a.Num_Appel = s.Num_Appel LEFT JOIN medecins m on s.CodeMedecin = m.CodeMedecin ";
                sqlstr0 += "                     inner join suiviappel s2  ON s2.Num_Appel = a.Num_Appel ";
                sqlstr0 += " WHERE a.Num_Appel = s.Num_Appel AND s2.Type_Operation = 'Création'";
                sqlstr0 += " AND s.Status in ('NA', 'PR')";

                cmd.CommandText = sqlstr0;
                
                dtAppelsNonAttribues.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des appels en cours....table AppelsEnCours :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtAppelsNonAttribues;
        }

        //Listes des médecins travaillant avec éventuellement la liste des appels qui leur sont attribués (pour Form1 Colonne droite)
        public static DataTable ChargeListeAppelsAttribues()
        {
            DataTable dtAppelsAttribues = new DataTable();
            
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT e.CodeMedecin, nb_Visite, CONCAT(m.Nom, ' ', m.Prenom) as NomMedecin, g.TypeGarde, ";
                sqlstr0 += " CONCAT(a.Nom, ' ', a.Prenom) as NomPrenom, CONCAT( a.Adr1, ', ', a.Num_Rue) as Adresse, a.CodePostal, a.Commune, a.Pays, ";
                sqlstr0 += " (CASE WHEN a.Num_Appel IS NULL THEN -1 ELSE a.Num_Appel END) AS Num_Appel, a.Latitude, a.Longitude, e.StatusGarde, m.TitreEmployee "; // Added Titre
                sqlstr0 += " FROM engarde e LEFT JOIN ";
                sqlstr0 += "              (SELECT ac.* FROM appelsencours ac INNER JOIN status_visite v ON ac.Num_Appel = v.Num_Appel ";
                sqlstr0 += "               WHERE v.Status IN ('AT', 'AQ', 'V')) as a ON e.CodeMedecin = a.CodeMedecin";
                sqlstr0 += " INNER JOIN medecins m ON m.CodeMedecin = e.CodeMedecin";
                sqlstr0 += " INNER JOIN garde g ON e.IdGarde = g.IdGarde";
                sqlstr0 += " WHERE m.LibelleOrg = 'Sos Médecins' ";

                cmd.CommandText = sqlstr0;
                
                dtAppelsAttribues.Load(cmd.ExecuteReader());    //on execute
               
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des appels en cours :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtAppelsAttribues;
        }

        //Affecte la visite Pré-attribuée pour un médecin de Disponible
        public static void AffectePreAttribuee(string CodeMedecin)
        {
            //On regarde enfin, s'il y a des visites pré-attribuées pour ce médecin....
            DataTable dtAppelsPre = ListeVisitesPreAttribuee(CodeMedecin);

            if (dtAppelsPre.Rows.Count > 0)
            {
                //on affecte la 1er de la liste                        
                if (AttributionVisitePreAttr(dtAppelsPre.Rows[0]["Num_Appel"].ToString(), CodeMedecin) == "KO")
                {
                    System.Windows.Forms.MessageBox.Show("Echec de l'attribution de la visite pré-attrbuée. ", "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }

            //On incremente le compteur de rafraichissement
            IncrementeRafraichissement();
        }

        //Listes des visites Pré-attribuées à un médecin
        public static DataTable ListeVisitesPreAttribuee(string CodeMedecin)
        {
            DataTable dtPreAttribuee = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;            

            string sqlstr0 = "SELECT * FROM status_visite ";            
            sqlstr0 += " WHERE Status ='PR' AND CodeMedecin = @CodeMedecin";
            sqlstr0 += " ORDER BY DateStatus";             

            try
            {
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeMedecin", CodeMedecin);                

                dtPreAttribuee.Load(cmd.ExecuteReader());    //on execute                 
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche des visites pré-attribués :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtPreAttribuee;
        }

        //Listes des visites Pré-attribuées à une organisation
        public static DataTable ListeVisitesPreAttribueeOrg(string CodeMedOrg)
        {
            DataTable dtPreAttribuee = new DataTable();            

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT * FROM status_visite_org ";
            sqlstr0 += " WHERE Status ='PR' AND CodeMedOrg = @CodeMedOrg";
            sqlstr0 += " ORDER BY DateStatus";

            try
            {
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeMedOrg", CodeMedOrg);

                dtPreAttribuee.Load(cmd.ExecuteReader());    //on execute                 
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche des visites pré-attribués à une organisation :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtPreAttribuee;
        }

        //Listes des visites Pré-attribuées, attribuées, Acquittées, En visite, à un médecin
        public static DataTable ListeVisitesAttribPreAttrib(string CodeMedecin)
        {
            DataTable dtAttribPreAttrib = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT a.*, s.Status, s.DateStatus, s.Ordre FROM appelsencours a INNER JOIN status_visite s ON a.Num_Appel = s.Num_Appel";
            sqlstr0 += " WHERE (s.Status ='PR' OR s.Status = 'AT' OR s.Status = 'AQ'  OR s.Status = 'V') AND s.CodeMedecin = @CodeMedecin ORDER BY s.Ordre, s.DateStatus";

            try
            {
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeMedecin", CodeMedecin);

                dtAttribPreAttrib.Load(cmd.ExecuteReader());    //on execute                 
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche des visites attribuées et pré-attribuées :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtAttribPreAttrib;
        }

        //Listes des visites Pré-attribuées, attribuées, En visite, à une Organisation
        public static DataTable ListVisitAttribPreAtOrg(string CodeMedOrg)
        {
            DataTable dtAttribPreAttrib = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT a.*, s.Status, s.DateStatus, s.Ordre FROM appelsencours a INNER JOIN status_visite_org s ON a.Num_Appel = s.Num_Appel";
            sqlstr0 += " WHERE s.Status IN ('PR','AT','V') AND s.CodeMedOrg = @CodeMedOrg ORDER BY s.Ordre, s.DateStatus";

            try
            {
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeMedOrg", CodeMedOrg);

                dtAttribPreAttrib.Load(cmd.ExecuteReader());    //on execute                 
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche des visites attribuées et pré-attribuées :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtAttribPreAttrib;
        }


        //Listes des appels attribués pour la carto
        public static DataTable ChargeListeAppelsAttribCarto()
        {
            string Erreur = "Non";

            DataTable dtAppelsAttribues = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT e.CodeMedecin, nb_Visite, CONCAT(m.Nom, ' ', m.Prenom) as NomMedecin, g.TypeGarde, ";
                sqlstr0 += " CONCAT(a.Nom, ' ', a.Prenom) as NomPrenom, CONCAT( a.Adr1, ', ', a.Num_Rue) as Adresse, a.CodePostal, a.Commune, a.Pays, ";
                sqlstr0 += " (CASE WHEN a.Num_Appel IS NULL THEN 0 ELSE a.Num_Appel END) AS Num_Appel, a.Latitude, a.Longitude, e.StatusGarde, e.IdStatusGarde ";
                sqlstr0 += " FROM engarde e INNER JOIN ";
                sqlstr0 += "              (SELECT ac.* FROM appelsencours ac INNER JOIN status_visite v ON ac.Num_Appel = v.Num_Appel ";
                sqlstr0 += "               WHERE v.Status IN ('AT', 'AQ', 'V')) as a ON e.CodeMedecin = a.CodeMedecin";
                sqlstr0 += " INNER JOIN medecins m ON m.CodeMedecin = e.CodeMedecin";
                sqlstr0 += " INNER JOIN garde g ON e.IdGarde = g.IdGarde";
                sqlstr0 += " WHERE m.LibelleOrg = 'Sos Médecins' ";

                cmd.CommandText = sqlstr0;

                dtAppelsAttribues.Load(cmd.ExecuteReader());    //on execute

                //On vérifie qu'il n'y a pas d'incoherence dans la base (Engarde statusGarde = Disponible ne doit pas exister)                     
                for (int i = 0; i < dtAppelsAttribues.Rows.Count; i++)
                {                    
                    if (dtAppelsAttribues.Rows[i]["StatusGarde"].ToString()== "Disponible")
                    {
                        //Incohérence, on remet le medecin en etat de visite
                        Corrige(dtAppelsAttribues.Rows[i]["IdStatusGarde"].ToString());
                        Erreur = "Oui";
                    }
                }

                if (Erreur == "Oui")
                {
                    //On relance la requette
                    dtAppelsAttribues.Clear();
                    dtAppelsAttribues.Load(cmd.ExecuteReader());    //on execute
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des appels en cours :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtAppelsAttribues;
        }


        //Incohérence, on remet le medecin en etat de visite
        public static void Corrige(string IdStatusGarde)
        {           
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "Update engarde SET StatusGarde = 'Visite' WHERE IdStatusGarde = " + IdStatusGarde;
              
                cmd.CommandText = sqlstr0;

                cmd.ExecuteNonQuery();//on execute                
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la correction du status en garde du médecin :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Incohérence, recherche des incohérences du médecin (Visite alors qu'il n'en a pas)
        public static DataTable RechercheIncoherence()
        {
            DataTable dtResult = new DataTable();
            
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = " SELECT e.IdStatusGarde, e.CodeMedecin, m.Nom , m.Prenom ";
                sqlstr0 += " FROM engarde e INNER JOIN medecins m ON m.CodeMedecin = e.CodeMedecin";                
                sqlstr0 += " WHERE e.StatusGarde in ('Attribuée', 'Acquitée', 'Visite')";
                sqlstr0 += " AND e.CodeMedecin not in (SELECT CodeMedecin FROM status_visite WHERE Status <> 'PR') ";
                sqlstr0 += " AND m.LibelleOrg = 'Sos Médecins' ";

                cmd.CommandText = sqlstr0;

                dtResult.Load(cmd.ExecuteReader());    //on execute           
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la correction du status en garde du médecin :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                dtResult = null;
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtResult;
        }

        //Incohérence, on remet le medecin en etat de Dispo
        public static void RemetDispoIncoherence(string IdStatusGarde)
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "Update engarde SET StatusGarde = 'Disponible' WHERE IdStatusGarde = " + IdStatusGarde;

                cmd.CommandText = sqlstr0;

                cmd.ExecuteNonQuery();//on execute                
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la correction du status en garde du médecin :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        #endregion

        #region Recherches diverses
        //Recherche Histogarde
        public static DataTable RechercheHisto(string Critere, string Valeur)
        {
            DataTable dtResult = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT h.* FROM histogarde h";

            switch(Critere)
            {
                case "IdSmartPhone": sqlstr0 += " WHERE h.IdSmartPhone = @Valeur"; break;
                case "CodeMedecin": sqlstr0 += " WHERE h.CodeMedecin = @Valeur"; break;
                case "IdGarde": sqlstr0 += " WHERE h.IdGarde = @Valeur"; break;
                case "Organisation": sqlstr0 += " INNER JOIN medecins m ON m.CodeMedecin = h.CodeMedecin WHERE m.LibelleOrg = @Valeur"; break;
                    //pour l'instant rien d'autre à compléter eventuellement si necessaire
            }
                  
            try
            {                
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Valeur", Valeur);

                dtResult.Load(cmd.ExecuteReader());    //on execute              
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche dans la table HistoGarde :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtResult;
        }


        //Recherche des Appels selon des critères
        public static DataTable RechercheAppels(string Critere, string Valeur)
        {
            DataTable dtResult = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT * FROM appels ";

            switch (Critere)
            {
                case "Commune": sqlstr0 += " WHERE Commune = @Valeur"; break;
                case "IdMotif": sqlstr0 += " WHERE IdMotif1 = @Valeur OR IdMotif2 = @Valeur"; break;
                case "Provenance": sqlstr0 += " WHERE Provenance = @Valeur"; break;
                    //pour l'instant rien d'autre à compléter eventuellement si necessaire
            }

            try
            {
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Valeur", Valeur);

                dtResult.Load(cmd.ExecuteReader());    //on execute              
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche dans la table Appels :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtResult;
        }


        //Recherche des Suivi Appels selon des critères
        public static DataTable RechercheSuiviAppels(string Critere, string Valeur)
        {
            DataTable dtResult = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT * FROM suiviappel ";

            switch (Critere)
            {
                case "NumRegul": sqlstr0 += " WHERE IdUtilisateur = @Valeur"; break;
                case "NumAppels": sqlstr0 += " WHERE Num_Appels = @Valeur"; break;

                //pour l'instant rien d'autre à compléter eventuellement si necessaire
            }

            try
            {
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Valeur", Valeur);

                dtResult.Load(cmd.ExecuteReader());    //on execute              
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche dans la table SuiviAppel :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }
            return dtResult;
        }


        //Recherche des AppelsEnCours selon des critères
        public static DataTable RechercheAppelsEnCours(string Critere, string Valeur)
        {
            DataTable dtResult = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT a.Num_Appel, a.Urgence, DATE_FORMAT(s.DateOp, '%H:%i') AS heure, TIMEDIFF(NOW(),s.DateOp) as Attente, ";
            sqlstr0 += " CONCAT(a.Num_Rue, ' ', a.Adr1) as Adresse, a.Commune, CONCAT(a.Nom, ' ', a.Prenom) as NomPrenom, a.Motif1, a.Motif2, a.Provenance ";
            sqlstr0 += " FROM appelsencours a, suiviappel s ";                  

            switch (Critere)
            {
                case "Num_Appel": sqlstr0 += " WHERE a.Num_Appel = @Valeur AND a.CodeMedecin = -1 AND s.Type_Operation = 'Création'"; break;
               
                //pour l'instant rien d'autre à compléter eventuellement si necessaire
            }

            try
            {
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Valeur", Valeur);

                dtResult.Load(cmd.ExecuteReader());    //on execute              
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche dans la table AppelsEnCours :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtResult;
        }


        //Recherche de l'Appels EnCours pour un médecin donné
        public static DataTable RechAppelsEnCoursMed(string CodeMedecin, string typeAppel)
        {
            DataTable dtResult = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT a.Num_Appel, CONCAT(a.Nom, ' ', a.Prenom) as NomPrenom, CONCAT(a.Num_Rue, ', ', a.Adr1) as Adresse, a.CodePostal, ";
            sqlstr0 += "  a.Commune, a.Pays, Latitude, Longitude, CONCAT(m.Nom, ' ', m.Prenom) as Medecin, v.Ordre ";
            sqlstr0 += " FROM appelsencours a INNER JOIN status_visite v ON v.Num_Appel = a.num_Appel INNER JOIN medecins m ON m.CodeMedecin = a.CodeMedecin ";

            switch (typeAppel)
            {
                case "ATAQV": sqlstr0 += "  WHERE a.CodeMedecin = @CodeMedecin AND v.Status in ('AT', 'AQ', 'V')"; break;
                case "AT": sqlstr0 += "  WHERE a.CodeMedecin = @CodeMedecin AND v.Status in ('AT')"; break;
                case "AQ": sqlstr0 += "  WHERE a.CodeMedecin = @CodeMedecin AND v.Status in ('AQ')"; break;
                case "V": sqlstr0 += "  WHERE a.CodeMedecin = @CodeMedecin AND v.Status in ('V')"; break;
                case "PR": sqlstr0 += "  WHERE a.CodeMedecin = @CodeMedecin AND v.Status in ('PR') ORDER BY v.Ordre"; break;
                case "ATPRAQV": sqlstr0 += "  WHERE a.CodeMedecin = @CodeMedecin AND v.Status in ('AT','PR','AQ','V') ORDER BY v.Ordre"; break;
            }
                               
            try
            {
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeMedecin", CodeMedecin);

                dtResult.Load(cmd.ExecuteReader());    //on execute              
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche dans la table AppelsEnCours :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtResult;
        }


        //Retourne l'etat de disponibilité d'un médecin
        public static string RechEtatDispoMedecin(string CodeMedecin)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT StatusGarde FROM engarde ";
            sqlstr0 += " WHERE CodeMedecin = @CodeMedecin";

            try
            {
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeMedecin", CodeMedecin);

                DataTable dtResult = new DataTable();

                dtResult.Load(cmd.ExecuteReader());    //on execute  

                if (dtResult.Rows.Count > 0)
                    Retour = dtResult.Rows[0][0].ToString();
                else Retour = "Pas en garde";

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche de l'etat de dispo d'un médecin, la table engarde :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Recherche si un médecin est disponible OK ou KO
        public static string RechSiMedecinDispo(string CodeMedecin)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT StatusGarde FROM engarde ";           
            sqlstr0 += " WHERE CodeMedecin = @CodeMedecin";
                      
            try
            {
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeMedecin", CodeMedecin);

                DataTable dtResult = new DataTable();

                dtResult.Load(cmd.ExecuteReader());    //on execute  

                if (dtResult.Rows[0][0].ToString() == "Disponible")
                    Retour = "OK";
                
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche dans la table engarde :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Recherche si un médecin a dejà une visite en cours
        public static string RechSiMedVisEnCours(string CodeMedecin)
        {
            string Retour = "Occupé";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT count(*) FROM status_visite ";
            sqlstr0 += " WHERE Status IN ('AT', 'AQ', 'V') AND CodeMedecin = @CodeMedecin";

            try
            {
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeMedecin", CodeMedecin);

                DataTable dtResult = new DataTable();

                dtResult.Load(cmd.ExecuteReader());    //on execute  

                if (int.Parse(dtResult.Rows[0][0].ToString()) == 0)
                    Retour = "Libre";
                else
                    Retour = "Occupé";

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche dans la table Status_Visite :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return Retour;
        }


        //Recherche si une infirmière (organisation) a dejà une visite en cours
        public static string RechSiOrgVisEnCours(string CodeMedOrg)
        {
            string Retour = "Occupé";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "SELECT count(*) FROM status_visite_org ";
            sqlstr0 += " WHERE Status IN ('AT', 'V') AND CodeMedOrg = @CodeMedOrg";

            try
            {
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeMedOrg", CodeMedOrg);

                DataTable dtResult = new DataTable();

                dtResult.Load(cmd.ExecuteReader());    //on execute  

                if (int.Parse(dtResult.Rows[0][0].ToString()) == 0)
                    Retour = "Libre";
                else
                    Retour = "Occupé";

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche dans la table Status_Visite_org :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Recherche des visites du jour pour un médecin donné (enfin depuis le début de sa garde)
        public static DataTable RechVisiteJour(string CodeMedecin)
        {
            DataTable dtAppelDuJour = new DataTable();
            DataTable dtDateGarde = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "";

            try
            { 
                //On commence par recupérer la date de début de garde du médecin en question
                sqlstr0 = "SELECT e.DateDebGarde FROM engarde e";        
                sqlstr0 += " WHERE e.CodeMedecin = '" + CodeMedecin + "'";

                cmd.CommandText = sqlstr0;               
                dtDateGarde.Load(cmd.ExecuteReader());    //on execute              
            
                if (dtDateGarde.Rows.Count > 0)
                {
                    //On ramène maintenant toutes les visites de ce médecin faites durant cette garde avec l'Idutilisateur de celui qui a créé l'appel (régul ou médecin)
                    sqlstr0 = "SELECT s3.*, Date(s4.DateOp) as DateTerm, Time(s4.DateOp) as HeureTerm";
                    sqlstr0 += " FROM (SELECT s2.Num_Appel, s2.IdUtilisateur, Date(s2.DateOp) as DateAppel, Time(s2.DateOp) as HeureAppel, CONCAT(u.Nom, ' ',u.Prenom) AS Util,";
                    sqlstr0 += "       a.Nom as NomPat, a.Prenom as PrenomPat, a.Adr1, a.Num_Rue, a.CodePostal, a.Commune, a.Tel_Appel";
                    sqlstr0 += "       FROM suiviappel s2 INNER JOIN (SELECT IdUtilisateur as Id, Nom, Prenom FROM utilisateur";
                    sqlstr0 += "                                      UNION";
                    sqlstr0 += "                                      SELECT CodeMedecin as Id, Nom, Prenom FROM medecins) u ON s2.IdUtilisateur = u.Id";
                    sqlstr0 += "                          INNER JOIN appels a ON a.Num_Appel = s2.Num_Appel";
                    sqlstr0 += "                          WHERE s2.Num_Appel in (SELECT Distinct(Num_Appel)";
                    sqlstr0 += "                                                 FROM suiviappel";
                    sqlstr0 += "                                                 WHERE CodeMedecin = '" + CodeMedecin + "'";
                    sqlstr0 += "                                                 AND DateOP >= '" + convertDateMaria(dtDateGarde.Rows[0]["DateDebGarde"].ToString(), "MariaDb") + "'";
                    sqlstr0 += "                                                 AND DateOP <= now())";
                    sqlstr0 += "                          AND Type_Operation = 'Création') s3 LEFT JOIN suiviappel s4 ON s3.Num_Appel = s4.Num_Appel";
                    sqlstr0 += " WHERE s4.Type_Operation = 'Terminée'";                   

                    cmd.CommandText = sqlstr0;

                    dtAppelDuJour.Load(cmd.ExecuteReader());    //on execute    
                }               
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtAppelDuJour;
        }

        //Retourne le nombre de visite faite par le médecin depuis le début de sa garde
        public static int RechNbVisiteMed(int CodeMedecin)
        {
            DataTable dtNbVisite = new DataTable();
            int NbVisite = 0;
           
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "";

            try
            {               
                sqlstr0 = "SELECT Nb_Visite FROM engarde ";
                sqlstr0 += " WHERE CodeMedecin = " + CodeMedecin;

                cmd.CommandText = sqlstr0;
                dtNbVisite.Load(cmd.ExecuteReader());    //on execute              

                if (dtNbVisite.Rows.Count > 0)
                    NbVisite = int.Parse(dtNbVisite.Rows[0][0].ToString());                
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return NbVisite;
        }

        //Récup du type de garde pour un médecin donné        
        public static string RecupTypeGarde(string CodeMedecin)
        {            
            string TypeGarde = "";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            string sqlstr0 = "";

            try
            {
                sqlstr0 = "SELECT g.TypeGarde FROM engarde e INNER JOIN garde g ON g.IdGarde = e.IdGarde ";
                sqlstr0 += " WHERE e.CodeMedecin = " + CodeMedecin;

                cmd.CommandText = sqlstr0;

                DataTable dtTypeGarde = new DataTable();
                dtTypeGarde.Load(cmd.ExecuteReader());    //on execute              

                if (dtTypeGarde.Rows.Count > 0)
                    TypeGarde = dtTypeGarde.Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche du type de garde :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return TypeGarde;
        }

        #endregion


        #region Aide à la régulation
        public static string ProchainMedecin()
        {
            string Retour = "";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {                
                string sqlstr0 = "SELECT e.CodeMedecin, CONCAT(m.Prenom , ' ', m.Nom) as NomMed, ";  
				sqlstr0 += "        		CASE WHEN Max(sa1.DateOp) is null "; 
				sqlstr0 += "        		     THEN TIMEDIFF(CURTIME(), CURTIME() + INTERVAL -48 Hour) "; 
				sqlstr0 += "        		     ELSE TIMEDIFF(NOW(), MAX(sa1.DateOp)) END as DiffOpe "; 	          
                sqlstr0 += "        FROM engarde e INNER JOIN medecins m ON m.CodeMedecin  = e.CodeMedecin "; 	
			    sqlstr0 += "        LEFT JOIN (SELECT * FROM suiviappel WHERE Type_Operation = 'Terminée' AND DateOp > CURDATE() + INTERVAL -10 DAY) as sa1 ON sa1.CodeMedecin = e.CodeMedecin "; 
			    sqlstr0 += "        LEFT JOIN appels a ON a.Num_Appel = sa1.Num_Appel "; 
                sqlstr0 += "        WHERE e.StatusGarde = 'Disponible' ";
                sqlstr0 += "        AND m.GMS = 0";
                sqlstr0 += "        AND m.LibelleOrg = 'Sos Médecins'";
                sqlstr0 += "        AND CASE WHEN a.IdMotif1 is null THEN '0' ELSE a.IdMotif1 END != 140 "; 
                sqlstr0 += "        AND CASE WHEN a.IdMotif2 is null THEN '0' ELSE a.IdMotif2 END != 140 "; 
                sqlstr0 += "        GROUP by e.CodeMedecin ";
                sqlstr0 += "        ORDER BY DiffOpe DESC "; 

                cmd.CommandText = sqlstr0;
              
                DataTable dtProchainMed = new DataTable();
                dtProchainMed.Load(cmd.ExecuteReader());    //on execute

                if (dtProchainMed.Rows.Count > 0)   //Si on a un enregistrement
                {
                    if (dtProchainMed.Rows[0][0] != DBNull.Value)
                        Retour = dtProchainMed.Rows[0]["NomMed"].ToString();
                    else Retour = "";
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur dans l'aide à la régulation -prochain médecin: " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        public static DataTable Alarme()
        { 
            DataTable dtAlarme = new DataTable();
            
            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT CONCAT(m.Prenom , ' ', m.Nom) as NomMed ";
                sqlstr0 += " FROM geoloc g INNER JOIN medecins m ON m.CodeMedecin = g.CodeMedecin ";
                sqlstr0 += " WHERE g.Alarme = 1";
                cmd.CommandText = sqlstr0;

                dtAlarme.Load(cmd.ExecuteReader());    //on execute    

                //On incremente le compteur de rafraichissement
              //  IncrementeRafraichissement();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la récupération des alarmes." + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtAlarme;
        }

        public static string ClearAlarme()
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
                string sqlstr0 = "UPDATE geoloc SET Alarme = 0 WHERE Alarme = 1";                
                cmd.CommandText = sqlstr0;

                cmd.ExecuteNonQuery();    //on execute   

                retour = "OK";
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la suppression des alarmes." + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }
            return retour;
        }

        public static string PatientRappel(int NumVisite)
        {
            string retour = "0";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT PatientRappel ";
                sqlstr0 += " FROM appelsencours ";
                sqlstr0 += " WHERE Num_Appel = " + NumVisite;
                
                cmd.CommandText = sqlstr0;

                DataTable dtPatientRappel = new DataTable();
                dtPatientRappel.Load(cmd.ExecuteReader());    //on execute    

                if (dtPatientRappel.Rows.Count > 0)
                    retour = dtPatientRappel.Rows[0]["PatientRappel"].ToString();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche si les patients ont rappelés....table AppelsEnCours :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return retour;
        }
        #endregion


        #region Parametres
        //On retourne l'Etat du CTI      
        public static bool EtatCTI()
        {
            bool Retour = false;

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT ActivationCTI FROM param_divers";

                cmd.CommandText = sqlstr0;

                DataTable dtCTI = new DataTable();
                dtCTI.Load(cmd.ExecuteReader());    //on execute       

                if (dtCTI.Rows[0][0].ToString() == "True")
                    Retour = true;
                else Retour = false;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des paramètres divers (Etat CTI):" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Chargement des preferences utilisateur
        public static DataTable ChargePreferencesUtil(string IdUtilisateur)
        {
            DataTable dtPrefs = new DataTable();
            
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

                dtPrefs.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la récupération des preferences utilisateur." + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtPrefs;
        }


        //Chargement des paramètres
        public static DataTable ChargeParam()
        {
            DataTable dtParam = new DataTable();            
            
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT * FROM param_divers";

                cmd.CommandText = sqlstr0;

                dtParam.Load(cmd.ExecuteReader());    //on execute                     
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des paramètres divers :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtParam;
        }


        #endregion

        #region Suppressions divers
        //Suppresion d'un smartphone
        public static string SupprSmartphone(string IdSmartphone)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "DELETE FROM smartphone WHERE IdSmartphone = @IdSmartphone";
                
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdSmartphone", IdSmartphone);

                cmd.ExecuteNonQuery();    //on execute     
                Retour = "OK";
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la suppression du Smartphone " + IdSmartphone + " :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        //Suppresion d'une organisation
        public static string SupprOrganisation(string IdOrganisation)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "DELETE FROM organisation WHERE IdOrganisation = @IdOrganisation";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdOrganisation", IdOrganisation);

                cmd.ExecuteNonQuery();    //on execute     
                Retour = "OK";
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la suppression d'organisation " + IdOrganisation + " :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        //Suppresion d'un Médecin
        public static string SupprMedecin(string CodeMedecin)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "DELETE FROM medecins WHERE CodeMedecin = @CodeMedecin";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeMedecin", CodeMedecin);

                cmd.ExecuteNonQuery();    //on execute     
                Retour = "OK";
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la suppression du médecin " + CodeMedecin + " :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Suppresion d'une garde
        public static string SupprGarde(string IdGarde)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "DELETE FROM garde WHERE IdGarde = @IdGarde";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdGarde", IdGarde);

                cmd.ExecuteNonQuery();    //on execute     
                Retour = "OK";
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la suppression de la garde " + IdGarde + " :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Suppresion d'une commune
        public static string SupprCommune(string IdCommune)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "DELETE FROM commune WHERE IdCommune = @IdCommune";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdCommune", IdCommune);

                cmd.ExecuteNonQuery();    //on execute     
                Retour = "OK";
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la suppression de la commune " + IdCommune + " :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Suppresion d'une rue
        public static string SupprRue(string IdRue)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "DELETE FROM rue WHERE IdRue = @IdRue";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdRue", IdRue);

                cmd.ExecuteNonQuery();    //on execute     
                Retour = "OK";
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la suppression de la rue " + IdRue + " :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Suppresion d'un motif
        public static string SupprMotif(string IdMotif)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "DELETE FROM motif WHERE IdMotif = @IdMotif";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdMotif", IdMotif);

                cmd.ExecuteNonQuery();    //on execute     
                Retour = "OK";
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la suppression du motif " + IdMotif+ " :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Suppresion d'une assurance
        public static string SupprAssurance(string IdAssurance)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "DELETE FROM assurance WHERE IdAssurance = @IdAssurance";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdAssurance", IdAssurance);

                cmd.ExecuteNonQuery();    //on execute     
                Retour = "OK";
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la suppression d'une assurance " + IdAssurance + " :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Suppresion d'une Provenance
        public static string SupprProvenance(string IdProvenance)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "DELETE FROM provenance WHERE IdProvenance = @IdProvenance";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdProvenance", IdProvenance);

                cmd.ExecuteNonQuery();    //on execute     
                Retour = "OK";
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la suppression de la provenance " + IdProvenance + " :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Suppresion d'un utilisateur
        public static string SupprUtilisateur(string IdUtilisateur)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "DELETE FROM utilisateur WHERE IdUtilisateur = @IdUtilisateur";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdUtilisateur", IdUtilisateur);

                cmd.ExecuteNonQuery();    //on execute     
                Retour = "OK";
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la suppression de l'utilisateur " + IdUtilisateur + " :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Suppresion d'un numero de telephone
        public static string SupprTelUtil(string IdTelUtil)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "DELETE FROM telephones_utils WHERE IdTelUtil = @IdTelUtil";               

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdTelUtil", IdTelUtil);

                cmd.ExecuteNonQuery();    //on execute     
                Retour = "OK";
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la suppression du N° de tel :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        #endregion

        #region Gestion de l'état d'une visite
        //########################################Gestion de l'état d'une visite############################################
        //Recherche si un medecin a été attribué à une visite donnée        
        public static int MedecinVisite(Int32 NumVisite)
        {
            int Retour = -1;

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Table Status_Visite
                string sqlstr0 = "SELECT CodeMedecin FROM status_visite WHERE Num_Appel = @Appel";
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Appel", NumVisite);

                DataTable dtStatus_Visite = new DataTable();
                dtStatus_Visite.Load(cmd.ExecuteReader());    //on execute

                if (dtStatus_Visite.Rows.Count > 0)   //Si on a un enregistrement
                {
                    if (dtStatus_Visite.Rows[0][0] != DBNull.Value)
                        Retour = int.Parse(dtStatus_Visite.Rows[0][0].ToString());
                    else Retour = -1;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche du médecin dans la table Status_Visite :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        
        //Recherche si une infirmière (organisation) a été attribuée à une visite donnée        
        public static int OrganisationVisite(Int32 NumVisite)
        {
            int Retour = -1;

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Table Status_Visite
                string sqlstr0 = "SELECT CodeMedOrg FROM status_visite_org WHERE Num_Appel = @Appel";
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Appel", NumVisite);

                DataTable dtStatus_Visite_org = new DataTable();
                dtStatus_Visite_org.Load(cmd.ExecuteReader());    //on execute

                if (dtStatus_Visite_org.Rows.Count > 0)   //Si on a un enregistrement
                {
                    if (dtStatus_Visite_org.Rows[0][0] != DBNull.Value)
                        Retour = int.Parse(dtStatus_Visite_org.Rows[0][0].ToString());
                    else Retour = -1;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche de l'infirmiere dans la table Status_Visite_org :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Renvoi le status d'une visite côté Médecin
        public static string StatusVisite(Int32 NumVisite)
        {
            string Retour = "KO";
            
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Table Status_Visite
                string sqlstr0 = "SELECT Status FROM status_visite WHERE Num_Appel = @Appel";
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Appel", NumVisite);

                DataTable dtStatus_Visite = new DataTable();
                dtStatus_Visite.Load(cmd.ExecuteReader());    //on execute

                if (dtStatus_Visite.Rows.Count > 0)   //Si on a un enregistrement
                {
                    if (dtStatus_Visite.Rows[0][0] != DBNull.Value)
                        Retour = dtStatus_Visite.Rows[0][0].ToString();                    
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche du status de la visite dans la table Status_Visite :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Renvoi le status d'une visite côté Infirmière (Organisation)
        public static string StatusVisiteOrg(Int32 NumVisite)
        {
            string Retour = "KO";

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Table Status_Visite
                string sqlstr0 = "SELECT Status FROM status_visite_org WHERE Num_Appel = @Appel";
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Appel", NumVisite);

                DataTable dtStatus_Visite_Org = new DataTable();
                dtStatus_Visite_Org.Load(cmd.ExecuteReader());    //on execute

                if (dtStatus_Visite_Org.Rows.Count > 0)   //Si on a un enregistrement
                {
                    if (dtStatus_Visite_Org.Rows[0][0] != DBNull.Value)
                        Retour = dtStatus_Visite_Org.Rows[0][0].ToString();
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche du status de la visite dans la table status_visite_org :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        //Attribution d'une visite à un médecin (MAJ des tables)
        public static string AttributionVisite(string Num_Appel, string CodeMedecin)
        {
            string Retour = "KO";
                      
            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;


            //On vérifie que la visite n'est pas déjà attribuée
            if (RechSiPasPreAttribue(Num_Appel) == "Libre")
            {
                //On vérifie qu'il est dispo
                if (RechSiMedVisEnCours(CodeMedecin) == "Libre")
                {
                    //Il est libre, donc on lui affecte la visite
                    //On déclare ici la requete (à cause de la transaction)
                    string SqlStr0 = "";
                    string SqlStr1 = "";
                    string SqlStr2 = "";
                    string SqlStr3 = "";
                    string SqlStr4 = "";

                    try
                    {
                        //Puis dans une transaction
                        MySqlTransaction trans;

                        //Maj AppelEnCours (on lui attribue le CodeMédecin)
                        SqlStr0 = "UPDATE appelsencours SET CodeMedecin = '" + CodeMedecin + "' WHERE Num_Appel = '" + Num_Appel + "'";

                        //Maj Appels (on lui attribue le CodeMédecin)
                        SqlStr1 = "UPDATE appels SET CodeMedecin = '" + CodeMedecin + "' WHERE Num_Appel = '" + Num_Appel + "'";

                        //Maj status_visite (codeMEdecin, status AT, Date)
                        SqlStr2 = "UPDATE status_visite SET CodeMedecin = '" + CodeMedecin + "', Status = 'AT', ";
                        SqlStr2 += " DateStatus = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "', Ordre = 0 ";
                        SqlStr2 += " WHERE Num_Appel = '" + Num_Appel + "'";

                        //Maj engarde (+1 au nb de visite, Attribuée)
                        SqlStr3 = "UPDATE engarde SET Nb_Visite = Nb_Visite + 1, StatusGarde = 'Attribuée'";
                        SqlStr3 += " WHERE CodeMedecin = '" + CodeMedecin + "'";

                        //Ajout d'1 enreg dans SuiviAppel
                        SqlStr4 = "INSERT INTO suiviappel ";
                        SqlStr4 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                        SqlStr4 += " VALUES('" + Num_Appel + "','Attribution','" + CodeMedecin + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                        //Ouverture d'une transaction
                        trans = dbConnection.BeginTransaction();
                        try
                        {
                            //on execute les requettes                                       
                            cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                            cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                            cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                            cmd.CommandText = SqlStr3; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                            cmd.CommandText = SqlStr4; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                            //on valide la transaction
                            trans.Commit();

                            Retour = "AT";
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            System.Windows.Forms.MessageBox.Show("Erreur lors de l'attribution de la visite (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                        System.Windows.Forms.MessageBox.Show("Erreur lors de l'attribution de la visite. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                    finally
                    {
                        //fermeture des connexions
                        if (dbConnection.State == System.Data.ConnectionState.Open)
                        {
                            dbConnection.Close();
                        }
                    }
                }  //Fin d'il est libre
                else
                {
                    //Il est occupé donc on lui fait une pré-attributiom
                    //On récupère le nombre maxi des visites PR
                    int MaxNumPR = RecupNumMaxPR(CodeMedecin);
                    
                    //On déclare ici la requete (à cause de la transaction)
                    string SqlStr0 = "";
                    string SqlStr1 = "";
                    string SqlStr2 = "";
                    string SqlStr3 = "";
                    string SqlStr4 = "";

                    try
                    {
                        //Puis dans une transaction
                        MySqlTransaction trans;

                        //Maj AppelEnCours (on lui attribue le CodeMédecin)
                        SqlStr0 = "UPDATE appelsencours SET CodeMedecin = '" + CodeMedecin + "' WHERE Num_Appel = '" + Num_Appel + "'";

                        //Maj Appels (on lui attribue le CodeMédecin)
                        SqlStr1 = "UPDATE appels SET CodeMedecin = '" + CodeMedecin + "' WHERE Num_Appel = '" + Num_Appel + "'";

                        //Maj status_visite (codeMEdecin, status PR, Date)
                        SqlStr2 = "UPDATE status_visite SET CodeMedecin = '" + CodeMedecin + "', Status = 'PR',";
                        SqlStr2 += " DateStatus = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "', Ordre = " + MaxNumPR;
                        SqlStr2 += " WHERE Num_Appel = '" + Num_Appel + "'";

                        //Maj engarde (+1 au nb de visite)
                        SqlStr3 = "UPDATE engarde SET Nb_Visite = Nb_Visite + 1";
                        SqlStr3 += " WHERE CodeMedecin = '" + CodeMedecin + "'";

                        //Ajout d'1 enreg dans SuiviAppel
                        SqlStr4 = "INSERT INTO suiviappel ";
                        SqlStr4 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                        SqlStr4 += " VALUES('" + Num_Appel + "','Pré-attribution','" + CodeMedecin + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                        //Ouverture d'une transaction
                        trans = dbConnection.BeginTransaction();
                        try
                        {
                            //on execute les requettes                                       
                            cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                            cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                            cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                            cmd.CommandText = SqlStr3; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                            cmd.CommandText = SqlStr4; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                            //on valide la transaction
                            trans.Commit();

                            Retour = "PR";
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            System.Windows.Forms.MessageBox.Show("Erreur lors de la pré-attribution de la visite (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                        System.Windows.Forms.MessageBox.Show("Erreur lors de la pré-attribution de la visite. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            }
            else
                Retour = "DPR";    //Déjà pré-attribuée, on ne fait rien

            //On incremente le compteur de rafraichissement
        //    IncrementeRafraichissement();

            return Retour;
        }


        //Attribution d'une visite à une autre Organisation (Maj des tables)        
        public static string AttributionVisiteOrg(string Num_Appel, string CodeMedOrg)
        {
            string Retour = "KO";
          
            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            //On vérifie qu'elle n'est pas déjà terminée par l'infirmière
            if (RechSiTerminee(Num_Appel) == "Terminée")
            {
                Retour = "T";   //Terminée
                return Retour;
            }

            //On vérifie que la visite n'est pas déjà attribuée
            if (RechSiPasPreAttribueOrg(Num_Appel) == "Libre")
            {
                //On vérifie qu'il est dispo
                if (RechSiOrgVisEnCours(CodeMedOrg) == "Libre")
                {
                    //Elle est libre, donc on lui affecte la visite
                    //On déclare ici la requete (à cause de la transaction)
                    string SqlStr0 = "";
                    string SqlStr1 = "";
                    string SqlStr2 = "";                    

                    try
                    {
                        //Puis dans une transaction
                        MySqlTransaction trans;                      

                        //Maj status_visite_org (codeMedecin, status AT, Date)
                        SqlStr0 = "UPDATE status_visite_org SET CodeMedOrg = '" + CodeMedOrg + "', Status = 'AT', ";
                        SqlStr0 += " DateStatus = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "', Ordre = 0 ";
                        SqlStr0 += " WHERE Num_Appel = '" + Num_Appel + "'";

                        //Maj engarde (+1 au nb de visite, Attribuée)
                        SqlStr1 = "UPDATE engarde SET Nb_Visite = Nb_Visite + 1, StatusGarde = 'Attribuée'";
                        SqlStr1 += " WHERE CodeMedecin = '" + CodeMedOrg + "'";

                        //Ajout d'1 enreg dans SuiviAppel
                        SqlStr2 = "INSERT INTO suiviappel ";
                        SqlStr2 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                        SqlStr2 += " VALUES('" + Num_Appel + "','Attribution','" + CodeMedOrg + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                        //Ouverture d'une transaction
                        trans = dbConnection.BeginTransaction();
                        try
                        {
                            //on execute les requettes                                       
                            cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                            cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                            cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                            
                            //on valide la transaction
                            trans.Commit();

                            Retour = "AT";
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            System.Windows.Forms.MessageBox.Show("Erreur lors de l'attribution de la visite à l'infirmiere (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                        System.Windows.Forms.MessageBox.Show("Erreur lors de l'attribution de la visite. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                    finally
                    {
                        //fermeture des connexions
                        if (dbConnection.State == System.Data.ConnectionState.Open)
                        {
                            dbConnection.Close();
                        }
                    }
                }  //Fin d'il est libre
                else
                {
                    //Elle est occupée donc on lui fait une pré-attributiom
                    //On récupère le nombre maxi des visites PR
                    int MaxNumPR = RecupOrgNumMaxPR(CodeMedOrg);

                    //On déclare ici la requete (à cause de la transaction)
                    string SqlStr0 = "";
                    string SqlStr1 = "";
                    string SqlStr2 = "";                   

                    try
                    {
                        //Puis dans une transaction
                        MySqlTransaction trans;                        

                        //Maj status_visite (codeMedecin, status PR, Date)
                        SqlStr0 = "UPDATE status_visite_org SET CodeMedOrg = '" + CodeMedOrg + "', Status = 'PR',";
                        SqlStr0 += " DateStatus = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "', Ordre = " + MaxNumPR;
                        SqlStr0 += " WHERE Num_Appel = '" + Num_Appel + "'";

                        //Maj engarde (+1 au nb de visite)
                        SqlStr1 = "UPDATE engarde SET Nb_Visite = Nb_Visite + 1";
                        SqlStr1 += " WHERE CodeMedecin = '" + CodeMedOrg + "'";

                        //Ajout d'1 enreg dans SuiviAppel
                        SqlStr2 = "INSERT INTO suiviappel ";
                        SqlStr2 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                        SqlStr2 += " VALUES('" + Num_Appel + "','Pré-attribution','" + CodeMedOrg + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                        //Ouverture d'une transaction
                        trans = dbConnection.BeginTransaction();
                        try
                        {
                            //on execute les requettes                                       
                            cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                            cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                            cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();                           

                            //on valide la transaction
                            trans.Commit();

                            Retour = "PR";
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            System.Windows.Forms.MessageBox.Show("Erreur lors de la pré-attribution de la visite à l'infirmière (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                        System.Windows.Forms.MessageBox.Show("Erreur lors de la pré-attribution de la visite à l'infirmière. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            }
            else
                Retour = "DPR";    //Déjà pré-attribuée, on ne fait rien

            //On incremente le compteur de rafraichissement
            //    IncrementeRafraichissement();

            return Retour;
        }
      

        //Retourne le nombre minimum des visites pre-atribuées au médecin
        public static int RecupNumMaxPR(string CodeMedecin)
        {
            int NumMax = 1;

            //Chaine de connection... ici on attaque MariaDB         
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string SqlStr0 = "SELECT MAX(Ordre) + 1 FROM status_visite";
                SqlStr0 += " WHERE CodeMedecin = " + CodeMedecin;

                //on execute les requettes                                     
                cmd.CommandText = SqlStr0;

                DataTable dtResult = new DataTable();
                dtResult.Load(cmd.ExecuteReader());    //on execute


                if (dtResult.Rows[0][0].ToString() != "" && dtResult.Rows[0][0] != DBNull.Value)
                    NumMax = int.Parse(dtResult.Rows[0][0].ToString());
                else NumMax = 1;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la recuperation du N° Max Ordre. " + e.Message);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return NumMax;
        }

        //Retourne le nombre minimum des visites pre-atribuées à l'Organisation (Infirmière)
        public static int RecupOrgNumMaxPR(string CodeMedOrg)
        {
            int NumMax = 1;

            //Chaine de connection... ici on attaque MariaDB         
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string SqlStr0 = "SELECT MAX(Ordre) + 1 FROM status_visite_org";
                SqlStr0 += " WHERE CodeMedOrg = " + CodeMedOrg;

                //on execute les requettes                                     
                cmd.CommandText = SqlStr0;

                DataTable dtResult = new DataTable();
                dtResult.Load(cmd.ExecuteReader());    //on execute


                if (dtResult.Rows[0][0].ToString() != "" && dtResult.Rows[0][0] != DBNull.Value)
                    NumMax = int.Parse(dtResult.Rows[0][0].ToString());
                else NumMax = 1;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la recuperation du N° Max Ordre Infirmière. " + e.Message);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return NumMax;
        }


        //Vérification que la visite n'est pas pré-attribuée à un autre Médecin
        public static string RechSiPasPreAttribue(string Num_Appel)
        {
            string Retour = "DPR";

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                // string sqlstr0 = "SELECT IdUtilisateur, FROM_BASE64(Password) as Password, Droit, Nom, Prenom, IdSmartRapport, IdCTI, NumPoste FROM Utilisateur";           
                string sqlstr0 = "SELECT Status FROM status_visite WHERE Num_Appel = @Num_Appel";
                
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Num_Appel", Num_Appel);

                DataTable dtStatus = new DataTable();
                dtStatus.Load(cmd.ExecuteReader());    //on execute   

                if (dtStatus.Rows[0][0].ToString() == "PR")
                    Retour = "DPR";     //Déjà Pré-attribuée
                else Retour = "Libre";
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la vérification de pré-attribution de la visite :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);                            
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


        //Vérification que la visite n'est pas déjà terminée par l'Organisation
        public static string RechSiTerminee(string Num_Appel)
        {
            string Retour = "Non terminée";

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {                
                string sqlstr0 = "SELECT Status FROM status_visite_org WHERE Num_Appel = @Num_Appel";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Num_Appel", Num_Appel);

                DataTable dtStatus = new DataTable();
                dtStatus.Load(cmd.ExecuteReader());    //on execute   

                if (dtStatus.Rows[0][0].ToString() == "T")
                    Retour = "Terminée";  
                
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la vérification si la visite est terminée par l'organisaton table Status_visite_Org:" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Vérification que la visite n'est pas pré-attribuée à une autre Organisation
        public static string RechSiPasPreAttribueOrg(string Num_Appel)
        {
            string Retour = "DPR";

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                // string sqlstr0 = "SELECT IdUtilisateur, FROM_BASE64(Password) as Password, Droit, Nom, Prenom, IdSmartRapport, IdCTI, NumPoste FROM Utilisateur";           
                string sqlstr0 = "SELECT Status FROM status_visite_org WHERE Num_Appel = @Num_Appel";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Num_Appel", Num_Appel);

                DataTable dtStatus = new DataTable();
                dtStatus.Load(cmd.ExecuteReader());    //on execute   

                if (dtStatus.Rows[0][0].ToString() == "PR")
                    Retour = "DPR";     //Déjà Pré-attribuée
                else Retour = "Libre";
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la vérification de pré-attribution de la visite table Status_visite_Org:" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Attribution d'une visite pré-attribué (MAJ des tables)
        public static string AttributionVisitePreAttr(string Num_Appel, string CodeMedecin)
        {
            string Retour = "KO";

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;


            //On vérifie qu'il est dispo
            if (RechSiMedVisEnCours(CodeMedecin) == "Libre")
            {
                //Il est libre, donc on lui affecte la visite
                //On déclare ici la requete (à cause de la transaction)                
                string SqlStr1 = "";
                string SqlStr2 = "";
                string SqlStr3 = "";
                string SqlStr4 = "";

                try
                {
                    //Puis dans une transaction
                    MySqlTransaction trans;

                    //Maj AppelsEnCours
                    SqlStr1 = "UPDATE appelsencours SET CodeMedecin = '" + CodeMedecin + "'";
                    SqlStr1 += " WHERE Num_Appel = '" + Num_Appel + "'";
                  
                    //Maj status_visite (codeMEdecin, status AT, Date)
                    SqlStr2 = "UPDATE status_visite SET CodeMedecin = '" + CodeMedecin + "', Status = 'AT',";
                    SqlStr2 += " DateStatus = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "',";
                    SqlStr2 += " Ordre = 0 ";
                    SqlStr2 += " WHERE Num_Appel = '" + Num_Appel + "'";

                    //Maj engarde (Pas d'ajout de +1 au nb de visite (déjà fait lors de la pré-attribution), Attribuée)
                    SqlStr3 = "UPDATE engarde SET StatusGarde = 'Attribuée'";
                    SqlStr3 += " WHERE CodeMedecin = '" + CodeMedecin + "'";

                    //Ajout d'1 enreg dans SuiviAppel
                    SqlStr4 = "INSERT INTO suiviappel ";
                    SqlStr4 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                    SqlStr4 += " VALUES('" + Num_Appel + "','Attribution','" + CodeMedecin + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                    //Ouverture d'une transaction
                    trans = dbConnection.BeginTransaction();
                    try
                    {
                        //on execute les requettes                       
                        cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();               
                        cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                        cmd.CommandText = SqlStr3; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                        cmd.CommandText = SqlStr4; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                        //on valide la transaction
                        trans.Commit();

                        Retour = "OK";

                        //On incremente le compteur de rafraichissement
                        IncrementeRafraichissement();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        System.Windows.Forms.MessageBox.Show("Erreur lors de l'attribution de la visite pré-attribuée (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                    System.Windows.Forms.MessageBox.Show("Erreur lors de l'attribution de la visite pré-attribuée. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                finally
                {
                    //fermeture des connexions
                    if (dbConnection.State == System.Data.ConnectionState.Open)
                    {
                        dbConnection.Close();
                    }
                }
            }  //Fin d'il est libre
           
            return Retour;
        }

        //Attribution d'une visite pré-attribué (MAJ des tables) Organisation
        public static string AttributionVisitePreAttrOrg(string Num_Appel, string CodeMedOrg)
        {
            string Retour = "KO";

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;
            
            //On vérifie qu'il est dispo
            if (RechSiOrgVisEnCours(CodeMedOrg) == "Libre")
            {
                //Il est libre, donc on lui affecte la visite
                //On déclare ici la requete (à cause de la transaction)                
                string SqlStr1 = "";
                string SqlStr2 = "";
                string SqlStr3 = "";                

                try
                {
                    //Puis dans une transaction
                    MySqlTransaction trans;
                   
                    //Maj status_visite (codeMedOrg, status AT, Date)
                    SqlStr1 = "UPDATE status_visite_org SET CodeMedOrg = '" + CodeMedOrg + "', Status = 'AT',";
                    SqlStr1 += " DateStatus = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "',";
                    SqlStr1 += " Ordre = 0 ";
                    SqlStr1 += " WHERE Num_Appel = '" + Num_Appel + "'";

                    //Maj engarde (Pas d'ajout de +1 au nb de visite (déjà fait lors de la pré-attribution), Attribuée)
                    SqlStr2 = "UPDATE engarde SET StatusGarde = 'Attribuée'";
                    SqlStr2 += " WHERE CodeMedecin = '" + CodeMedOrg + "'";

                    //Ajout d'1 enreg dans SuiviAppel
                    SqlStr3 = "INSERT INTO suiviappel ";
                    SqlStr3 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                    SqlStr3 += " VALUES('" + Num_Appel + "','Attribution','" + CodeMedOrg + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                    //Ouverture d'une transaction
                    trans = dbConnection.BeginTransaction();
                    try
                    {
                        //on execute les requettes                       
                        cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                        cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                        cmd.CommandText = SqlStr3; cmd.Transaction = trans; cmd.ExecuteNonQuery();                        

                        //on valide la transaction
                        trans.Commit();

                        Retour = "OK";

                        //On incremente le compteur de rafraichissement
                        IncrementeRafraichissement();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        System.Windows.Forms.MessageBox.Show("Erreur lors de l'attribution de la visite pré-attribuée Organisation (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                    System.Windows.Forms.MessageBox.Show("Erreur lors de l'attribution de la visite pré-attribuée Organisation. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                finally
                {
                    //fermeture des connexions
                    if (dbConnection.State == System.Data.ConnectionState.Open)
                    {
                        dbConnection.Close();
                    }
                }
            }  //Fin d'il est libre

            return Retour;
        }

        //Désattribuer une visite à un médecin Sos, remise dans le pool (MAJ des tables + ajout Suiviappel)
        public static string DesattribuerVisite(string Num_Appel, string CodeMedecin, string Status)
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
            string SqlStr2 = "";
            string SqlStr3 = "";
            string SqlStr4 = "";

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Maj AppelEnCours (on lui met -1 en CodeMédecin)
                SqlStr0 = "UPDATE appelsencours SET CodeMedecin = -1 WHERE Num_Appel = '" + Num_Appel + "'";

                //Maj Appels (on lui met -1 en CodeMédecin)
                SqlStr1 = "UPDATE appels SET CodeMedecin = -1 WHERE Num_Appel = '" + Num_Appel + "'";

                //Maj status_visite (-1, status NA, Date)
                SqlStr2 = "UPDATE status_visite SET CodeMedecin = -1, Status = 'NA',";
                SqlStr2 += " DateStatus = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "', Ordre = 0";
                SqlStr2 += " WHERE Num_Appel = '" + Num_Appel + "'";

                //Maj engarde (-1 au nb de visite mais on met 0 si plus de visite, Disponible)
                if (Status != "PR")    //Si c'est pas une Pré-Attribué, le médecin devient dispo
                {
                    SqlStr3 = "UPDATE engarde SET Nb_Visite = (CASE WHEN Nb_Visite > 0 THEN Nb_Visite - 1 ELSE 0 END), StatusGarde = 'Disponible'";
                    SqlStr3 += " WHERE CodeMedecin = '" + CodeMedecin + "'";
                }
                else  //C'est une pré attribué, on ne change pas l'état du médecin
                {
                    SqlStr3 = "UPDATE engarde SET Nb_Visite = (CASE WHEN Nb_Visite > 0 THEN Nb_Visite - 1 ELSE 0 END)";
                    SqlStr3 += " WHERE CodeMedecin = '" + CodeMedecin + "'";
                }

                //Ajout d'1 enreg dans SuiviAppel
                SqlStr4 = "INSERT INTO suiviappel ";
                SqlStr4 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                SqlStr4 += " VALUES('" + Num_Appel + "','Reprise','" + CodeMedecin + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr3; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr4; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();

                    Retour = "OK";

                    //On regarde s'il n'a pas de visite pré-attribuée
                    AnnuleVisite(CodeMedecin);

                    //On incremente le compteur de rafraichissement
                    IncrementeRafraichissement();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    System.Windows.Forms.MessageBox.Show("Erreur lors de la reprise de la visite (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                System.Windows.Forms.MessageBox.Show("Erreur lors de la reprise de la visite. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        //Désattribuer une visite à une organisation, remise dans le pool (MAJ des tables + ajout Suiviappel)
        public static string DesattribuerVisiteOrg(string Num_Appel, string CodeMedOrg, string Status)
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
            string SqlStr2 = "";

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;               

                //Maj status_visite (-1, status NA, Date)
                SqlStr0 = "UPDATE status_visite_org SET CodeMedOrg = -1, Status = 'NA',";
                SqlStr0 += " DateStatus = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "', Ordre = 0";
                SqlStr0 += " WHERE Num_Appel = '" + Num_Appel + "'";

                //Maj engarde (-1 au nb de visite mais on met 0 si plus de visite, Disponible)
                if (Status != "PR")    //Si c'est pas une Pré-Attribué, le médecin devient dispo
                {
                    SqlStr1 = "UPDATE engarde SET Nb_Visite = (CASE WHEN Nb_Visite > 0 THEN Nb_Visite - 1 ELSE 0 END), StatusGarde = 'Disponible'";
                    SqlStr1 += " WHERE CodeMedecin = '" + CodeMedOrg + "'";
                }
                else  //C'est une pré attribué, on ne change pas l'état du médecin
                {
                    SqlStr1 = "UPDATE engarde SET Nb_Visite = (CASE WHEN Nb_Visite > 0 THEN Nb_Visite - 1 ELSE 0 END)";
                    SqlStr1 += " WHERE CodeMedecin = '" + CodeMedOrg + "'";
                }

                //Ajout d'1 enreg dans SuiviAppel
                SqlStr2 = "INSERT INTO suiviappel ";
                SqlStr2 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                SqlStr2 += " VALUES('" + Num_Appel + "','Reprise','" + CodeMedOrg + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();                    

                    //on valide la transaction
                    trans.Commit();

                    Retour = "OK";

                    //On regarde s'il n'a pas de visite pré-attribuée
                    AnnuleVisiteOrg(CodeMedOrg);

                    //On incremente le compteur de rafraichissement
                    IncrementeRafraichissement();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    System.Windows.Forms.MessageBox.Show("Erreur lors de la reprise de la visite à une organisation (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                System.Windows.Forms.MessageBox.Show("Erreur lors de la reprise de la visite à une organisation. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        //Acquitement par le regulateur d'une visite à un médecin 
        public static string AcquitementVisite(string Num_Appel, string CodeMedecin)
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
            string SqlStr2 = "";
            
            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;
             
                //Maj status_visite (status AQ, Date)
                SqlStr0 = "UPDATE status_visite SET Status = 'AQ', DateStatus = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "'";
                SqlStr0 += " WHERE Num_Appel = '" + Num_Appel + "'";

                //Maj engarde 
                SqlStr1 = "UPDATE engarde SET StatusGarde = 'Acquitée'";
                SqlStr1 += " WHERE CodeMedecin = '" + CodeMedecin + "'";

                //Ajout d'1 enreg dans SuiviAppel
                SqlStr2 = "INSERT INTO suiviappel ";
                SqlStr2 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                SqlStr2 += " VALUES('" + Num_Appel + "','Acquitement','" + CodeMedecin + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();                   

                    //on valide la transaction
                    trans.Commit();

                    Retour = "OK";

                    //On incremente le compteur de rafraichissement
                    IncrementeRafraichissement();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    System.Windows.Forms.MessageBox.Show("Erreur lors de l'acquittement de la visite (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                System.Windows.Forms.MessageBox.Show("Erreur lors de l'acquittement de la visite. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        //Début de visite par le regulateur d'une visite à un médecin 
        public static string DebutVisite(string Num_Appel, string CodeMedecin)
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
            string SqlStr2 = "";

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Maj status_visite (status V, Date)
                SqlStr0 = "UPDATE status_visite SET Status = 'V', DateStatus = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "'";
                SqlStr0 += " WHERE Num_Appel = '" + Num_Appel + "'";

                //Maj engarde 
                SqlStr1 = "UPDATE engarde SET StatusGarde = 'Visite'";
                SqlStr1 += " WHERE CodeMedecin = '" + CodeMedecin + "'";

                //Ajout d'1 enreg dans SuiviAppel
                SqlStr2 = "INSERT INTO suiviappel ";
                SqlStr2 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                SqlStr2 += " VALUES('" + Num_Appel + "','Début de visite','" + CodeMedecin + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();

                    Retour = "OK";

                    //On incremente le compteur de rafraichissement
                    IncrementeRafraichissement();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    System.Windows.Forms.MessageBox.Show("Erreur lors du debut de visite (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                System.Windows.Forms.MessageBox.Show("Erreur lors du début de visite. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        //Début de visit par le regulateur d'une visite à une Organisation
        public static string DebutVisiteOrg(string Num_Appel, string CodeMedOrg)
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
            string SqlStr2 = "";

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Maj status_visite (status V, Date)
                SqlStr0 = "UPDATE status_visite_org SET Status = 'V', DateStatus = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "'";
                SqlStr0 += " WHERE Num_Appel = '" + Num_Appel + "'";

                //Maj engarde 
                SqlStr1 = "UPDATE engarde SET StatusGarde = 'Visite'";
                SqlStr1 += " WHERE CodeMedecin = '" + CodeMedOrg + "'";

                //Ajout d'1 enreg dans SuiviAppel
                SqlStr2 = "INSERT INTO suiviappel ";
                SqlStr2 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                SqlStr2 += " VALUES('" + Num_Appel + "','Début de visite','" + CodeMedOrg + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();

                    Retour = "OK";

                    //On incremente le compteur de rafraichissement
                    IncrementeRafraichissement();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    System.Windows.Forms.MessageBox.Show("Erreur lors du debut de visite Organisation (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                System.Windows.Forms.MessageBox.Show("Erreur lors du début de visite Organisation. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        //Annule Entrée en visite par le regulateur d'une visite à un médecin
        public static string AnnuleDebutVisite(string Num_Appel, string CodeMedecin)
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
            string SqlStr2 = "";

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Maj status_visite (status V, Date)
                SqlStr0 = "UPDATE status_visite SET Status = 'AQ', DateStatus = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "'";
                SqlStr0 += " WHERE Num_Appel = '" + Num_Appel + "'";

                //Maj engarde 
                SqlStr1 = "UPDATE engarde SET StatusGarde = 'Acquitée'";
                SqlStr1 += " WHERE CodeMedecin = '" + CodeMedecin + "'";

                //Ajout d'1 enreg dans SuiviAppel
                SqlStr2 = "INSERT INTO suiviappel ";
                SqlStr2 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                SqlStr2 += " VALUES('" + Num_Appel + "','Annulation Entrée Visite','" + CodeMedecin + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();

                    Retour = "OK";

                    //On incremente le compteur de rafraichissement
                    IncrementeRafraichissement();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    System.Windows.Forms.MessageBox.Show("Erreur lors de l'annulation du début de visite (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                System.Windows.Forms.MessageBox.Show("Erreur lors de l'annulation du début de visite. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Annule Entrée en visite par le regulateur d'une visite à une Organisation
        public static string AnnuleDebutVisiteOrg(string Num_Appel, string CodeMedOrg)
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
            string SqlStr2 = "";

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Maj status_visite (status AT, Date)
                SqlStr0 = "UPDATE status_visite_org SET Status = 'AT', DateStatus = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "'";
                SqlStr0 += " WHERE Num_Appel = '" + Num_Appel + "'";

                //Maj engarde 
                SqlStr1 = "UPDATE engarde SET StatusGarde = 'Attribuée'";
                SqlStr1 += " WHERE CodeMedecin = '" + CodeMedOrg + "'";

                //Ajout d'1 enreg dans SuiviAppel
                SqlStr2 = "INSERT INTO suiviappel ";
                SqlStr2 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                SqlStr2 += " VALUES('" + Num_Appel + "','Annulation Entrée Visite','" + CodeMedOrg + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();

                    Retour = "OK";

                    //On incremente le compteur de rafraichissement
                    IncrementeRafraichissement();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    System.Windows.Forms.MessageBox.Show("Erreur lors de l'annulation du début de visite Organisation (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                System.Windows.Forms.MessageBox.Show("Erreur lors de l'annulation du début de visite Organisation. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //Suite à l'annulation d'une visite à un médecin, on regarde pour attribuer une éventuelle visite pré-attribuée 
        public static string AnnuleVisite(string CodeMedecin)
        {
            string Retour = "KO";
           
            //On regarde enfin, s'il y a des visites pré-attribuées pour ce médecin....
            DataTable dtAppelsPre = ListeVisitesPreAttribuee(CodeMedecin);

            if (dtAppelsPre.Rows.Count > 0)
            {
                //on affecte la 1er de la liste                        
                if (AttributionVisitePreAttr(dtAppelsPre.Rows[0]["Num_Appel"].ToString(), CodeMedecin) == "KO")
                {
                    System.Windows.Forms.MessageBox.Show("Echec de l'attribution de la visite pré-attrbuée. ", "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else
                    Retour = "OK";
            }
            else
                Retour = "OK";

            //On incremente le compteur de rafraichissement
            IncrementeRafraichissement();

            return Retour;
        }


        //Suite à l'annulation d'une visite à une Organisation, on regarde pour attribuer une éventuelle visite pré-attribuée 
        public static string AnnuleVisiteOrg(string CodeMedOrg)
        {
            string Retour = "KO";

            //On regarde enfin, s'il y a des visites pré-attribuées pour ce médecin....
            DataTable dtAppelsPre = ListeVisitesPreAttribueeOrg(CodeMedOrg);

            if (dtAppelsPre.Rows.Count > 0)
            {
                //on affecte la 1er de la liste                        
                if (AttributionVisitePreAttrOrg(dtAppelsPre.Rows[0]["Num_Appel"].ToString(), CodeMedOrg) == "KO")
                {
                    System.Windows.Forms.MessageBox.Show("Echec de l'attribution de la visite pré-attrbuée à l'organisation. ", "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else
                    Retour = "OK";
            }
            else
                Retour = "OK";

            //On incremente le compteur de rafraichissement
            IncrementeRafraichissement();

            return Retour;
        }

        //Fin de visite par le regulateur d'une visite à un médecin 
        public static string FinVisite(string Num_Appel, string CodeMedecin)
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
            string SqlStr2 = "";
            string SqlStr3 = "";
            string SqlStr4 = "";

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //on efface AppelEnCours (table de tvl)
                SqlStr0 = "DELETE FROM appelsencours WHERE Num_Appel = " + Num_Appel;

                //Maj Appels (on lui met true à Termine)
                SqlStr1 = "UPDATE appels SET Termine = True WHERE Num_Appel = " + Num_Appel;

                //on efface status_visite (table de Tvl)
                SqlStr2 = "DELETE FROM status_visite WHERE Num_Appel = " + Num_Appel;

                //Maj engarde (Disponible)
                SqlStr3 = "UPDATE engarde SET StatusGarde = 'Disponible'";
                SqlStr3 += " WHERE CodeMedecin = '" + CodeMedecin + "'";

                //Ajout d'1 enreg dans SuiviAppel
                SqlStr4 = "INSERT INTO suiviappel ";
                SqlStr4 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                SqlStr4 += " VALUES('" + Num_Appel + "','Terminée','" + CodeMedecin + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr3; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr4; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();
                    Retour = "OK";                                          
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    System.Windows.Forms.MessageBox.Show("Erreur lors de la reprise de la visite (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                System.Windows.Forms.MessageBox.Show("Erreur lors de la reprise de la visite. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }          
                                     
            //On regarde enfin, s'il y a des visites pré-attribuées pour ce médecin....
            DataTable dtAppelsPre = ListeVisitesPreAttribuee(CodeMedecin);

            if (dtAppelsPre.Rows.Count > 0)
            {
                //on affecte la 1er de la liste                        
                if (AttributionVisitePreAttr(dtAppelsPre.Rows[0]["Num_Appel"].ToString(), CodeMedecin) == "KO")
                {
                    System.Windows.Forms.MessageBox.Show("Echec de l'attribution de la visite pré-attrbuée. ", "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }

            //On incremente le compteur de rafraichissement
            IncrementeRafraichissement();

            return Retour;
        }


        //Fin de visite par le regulateur d'une visite à une Organisation
        public static string FinVisiteOrg(string Num_Appel, string CodeMedOrg)
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
            string SqlStr2 = "";

            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Maj status_visite (status F, Date) ....On n'efface pas cette enregistrement! il le sera quand le médecin aura terminé cette visite.
                SqlStr0 = "UPDATE status_visite_org SET Status = 'T', DateStatus = '" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "'";
                SqlStr0 += " WHERE Num_Appel = '" + Num_Appel + "'";

                //Ajout d'1 enreg dans SuiviAppel
                SqlStr1 = "INSERT INTO suiviappel ";
                SqlStr1 += " (Num_Appel,Type_Operation, CodeMedecin, IdUtilisateur, DateOp)";
                SqlStr1 += " VALUES('" + Num_Appel + "','Terminée','" + CodeMedOrg + "', '" + Form1.Utilisateur[0] + "','" + FonctionsAppels.convertDateMaria(DateTime.Now.ToString(), "MariaDb") + "')";

                //Maj engarde (Disponible)
                SqlStr2 = "UPDATE engarde SET StatusGarde = 'Disponible'";
                SqlStr2 += " WHERE CodeMedecin = '" + CodeMedOrg + "'";

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();

                    Retour = "OK";

                    //On incremente le compteur de rafraichissement
                    IncrementeRafraichissement();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    System.Windows.Forms.MessageBox.Show("Erreur lors de la fin de visite Organisation (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                System.Windows.Forms.MessageBox.Show("Erreur lors de la fin de visite Organisation. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }
          
            //On regarde enfin, s'il y a des visites pré-attribuées pour ce médecin....
            DataTable dtAppelsPre = ListeVisitesPreAttribueeOrg(CodeMedOrg);

            if (dtAppelsPre.Rows.Count > 0)
            {
                //on affecte la 1er de la liste                        
                if (AttributionVisitePreAttrOrg(dtAppelsPre.Rows[0]["Num_Appel"].ToString(), CodeMedOrg) == "KO")
                {
                    System.Windows.Forms.MessageBox.Show("Echec de l'attribution de la visite pré-attrbuée à une organisation. ", "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }

            //On incremente le compteur de rafraichissement
            IncrementeRafraichissement();

            return Retour;
        }

        //Maj du N° de personne dans Appels (lorsque que la visite est terminée)
        /*  public static string MajNumPers(Int32 NumPersonne, Int32 Num_Appel)
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

                  //Maj Appels (on lui met true à Termine)
                  SqlStr0 = "UPDATE appels SET Num_Personne = '" + NumPersonne + "' WHERE Num_Appel = '" + Num_Appel + "'";

                  //Ouverture d'une transaction
                  trans = dbConnection.BeginTransaction();
                  try
                  {
                      //on execute les requettes                                       
                      cmd.CommandText = SqlStr0; cmd.ExecuteNonQuery();                 

                      //on valide la transaction
                      trans.Commit();

                      Retour = "OK";
                  }
                  catch (Exception e)
                  {
                      trans.Rollback();
                      System.Windows.Forms.MessageBox.Show("Erreur lors de la Mise à jour du n° de personne, Table Appels (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                  System.Windows.Forms.MessageBox.Show("Erreur lors de la Mise à jour du n° de personne, Table Appels. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
          }*/


        //Mise en pause d'un médecin par le regulateur
        public static string MiseEnPause(string CodeMedecin)
        {
            string Retour = "KO";
            //On regarde si le médecin est Dispo (pas de mise en pause s'il a une visite en poche)
            if (RechSiMedecinDispo(CodeMedecin) == "OK")
            {
                //On le met en pause

                //Chaine de connection... ici on attaque MariaDB
                string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
                MySqlConnection dbConnection = new MySqlConnection(connex);

                dbConnection.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = dbConnection;

                try
                {
                    //Maj engarde (Disponible)
                    String SqlStr0 = "UPDATE engarde SET StatusGarde = 'En pause'";
                    SqlStr0 += " WHERE CodeMedecin = @CodeMedecin";
                    cmd.CommandText = SqlStr0;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("CodeMedecin", CodeMedecin);

                    //on execute les requettes                                       
                    cmd.ExecuteNonQuery();

                    Retour = "OK";

                    //On incremente le compteur de rafraichissement
                    IncrementeRafraichissement();
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Erreur lors de la mise en pause. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            
            return Retour;
        }

        //Dé-pause d'un médecin par le regulateur
        public static string SortiePause(string CodeMedecin)
        {
            string Retour = "KO";
            //On regarde si le médecin est en pause
            if (RechEtatDispoMedecin(CodeMedecin) == "En pause")
            {
                //On le met en pause

                //Chaine de connection... ici on attaque MariaDB
                string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
                MySqlConnection dbConnection = new MySqlConnection(connex);

                dbConnection.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = dbConnection;

                try
                {
                    //Maj engarde (Disponible)
                    String SqlStr0 = "UPDATE engarde SET StatusGarde = 'Disponible'";
                    SqlStr0 += " WHERE CodeMedecin = @CodeMedecin";
                    cmd.CommandText = SqlStr0;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("CodeMedecin", CodeMedecin);

                    //on execute les requettes                                       
                    cmd.ExecuteNonQuery();

                    Retour = "OK";

                    //On incremente le compteur de rafraichissement
                    IncrementeRafraichissement();
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Erreur lors de la sortie de pause. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            return Retour;
        }

        //On termine la garde en cours
        public static string TermineGarde(string CodeMedecin)
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
            string SqlStr2 = "";

            DataTable dtgarde = new DataTable();

            //On récupères les infos de la garde du médecin
            string SqlStr = "SELECT e.*, g.TypeGarde FROM engarde e INNER JOIN garde g ON g.IdGarde = e.IdGarde";
            SqlStr += " WHERE CodeMedecin = @CodeMedecin";

            try
            {
                cmd.CommandText = SqlStr;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeMedecin", CodeMedecin);

                dtgarde.Load(cmd.ExecuteReader());    //on execute                 
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la sortie de garde d'un médecin, la table engarde :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
           
            //Puis MAJ de la table histogarde avec les infos de EnGarde
            try
            {
                //Puis dans une transaction
                MySqlTransaction trans;

                //Ajout d'un enregistrement dans HistoGarde
                SqlStr0 = "INSERT INTO histogarde ";
                SqlStr0 += " (CodeMedecin, IdSmartphone, IdGarde, TypeGarde, DebGarde, FinGarde, Nb_Visite)";
                SqlStr0 += " VALUES(@CodeMedecin, @IdSmartPhone, @IdGarde, @TypeGarde, @DebGarde, @FinGarde, @Nb_visite)";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("CodeMedecin", CodeMedecin);
                cmd.Parameters.AddWithValue("IdSmartphone", dtgarde.Rows[0]["IdSmartPhone"].ToString());
                cmd.Parameters.AddWithValue("IdGarde", dtgarde.Rows[0]["IdGarde"].ToString());
                cmd.Parameters.AddWithValue("TypeGarde", dtgarde.Rows[0]["TypeGarde"].ToString());
                cmd.Parameters.AddWithValue("DebGarde", convertDateMaria(dtgarde.Rows[0]["DateDebGarde"].ToString(), "MariaDb"));
                cmd.Parameters.AddWithValue("FinGarde", convertDateMaria(DateTime.Now.ToString(), "MariaDb"));                
                cmd.Parameters.AddWithValue("Nb_Visite", dtgarde.Rows[0]["Nb_Visite"].ToString());                
               

                //Puis suppression de l'enregistrement engarde
                SqlStr1 = "DELETE FROM engarde ";
                SqlStr1 += " WHERE CodeMedecin = '" + CodeMedecin + "'";

                //et Geoloc
                SqlStr2 = "DELETE FROM geoloc ";
                SqlStr2 += " WHERE CodeMedecin = '" + CodeMedecin + "'";

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                                           
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();
                    Retour = "OK";
                    
                    //On incremente le compteur de rafraichissement
                    IncrementeRafraichissement();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    System.Windows.Forms.MessageBox.Show("Erreur lors de la fin de garde du médecin (transaction). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                System.Windows.Forms.MessageBox.Show("Erreur lors de la fin de garde du médecin. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
        
        //########################################FIN Gestion de l'état d'une visite############################################
        #endregion
        #region Fonction travaillant avec SmartRapport
        //###############################################FONCTIONS INTERROGEANT BASE SMARTRAPPORT############################################################
        public static DataTable RecherchePersonne(String ModeRecherche, String Tel, String Nom, String Prenom, String DateNaiss)
        {
            string sqlstr = "";          //initialisation de la chaine sql
            string selonchoix = "";      //idem pour la partie selon le choix 
            string selonchoix1 = "";      //idem pour la partie selon le choix          
            DataTable dtRetourOk = new DataTable();  //Table de retour

            //Chaine de connection                                  
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
            
            SqlConnection dbConnection = new SqlConnection(connex);
           
            dbConnection.Open();
            
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = dbConnection;
            
            //en fonction du choix
            switch (ModeRecherche)
            {
                case "Tel":

                    //Pour le téléphone, on rajoute l'indicatif international s'il n'y en a pas
                    if (Tel != null && Tel != "")
                    {
                        //Y a t'il un +?
                        if (Tel.IndexOf("+") == -1)
                        {
                            //Ya t'il un 00?
                            if (Tel.Substring(0, 2) == "00")
                                Tel = "+" + Tel.Remove(0, 2);
                            else if (Tel.Substring(0, 1) == "0")
                                     Tel = "+41" + Tel.Remove(0, 1);                            
                        }
                        else
                        {   //On le reformate quand même
                            Tel = "+" + Tel.Replace("+", "");
                        }
                    }

                    selonchoix = " AND pe.Tel = '" + Tel + "'";
                    selonchoix += @" UNION
                                     SELECT pe.IdPersonne, tp.NumTel As Telephone, pe.Nom as Nom, pe.Prenom, pe.DateNaissance, UPPER(pe.Sexe) AS Sexe,'SUISSE' as Pays, pe.CodePostal,
                                     pe.Commune, pe.NumeroDansRue, pe.Rue, pe.TexteSup as rue2, '' as Batiment, ''as Escalier, pe.Etage, pe.Digicode, pe.Internom, pe.Porte, CAST(pe.Longitude as float) / 100000 as Longitude,
                                     CAST(pe.Latitude as float) /100000 as Latitude , ISNULL(Pat_Ta.TeleAlarm, 'N') TeleAlarme, ISNULL(T_pr.PatRem,'N') PatientRemarquable, T_pr.Medical as Medical, '' AS TexteSup,
                                     Nom as NomFact,Prenom as PrenomFact, 'SUISSE' as PaysFact, Adm_CodePostal as CodePostalFact, Adm_Commune as CommuneFact, Adm_NumeroDansRue as NumeroDansRueFact,
                                     Adm_Rue as RueFact, pe.ListeNoire as Rue2Fact, pe.Num_Carte as Num_Carte, pe.IdUnilab as IdUnilab, pe.Email 
                                     FROM tablepersonne pe, Tel_Personne tp, tablepatient pa
                                          left outer join ( SELECT Distinct(IdPatient), 'O' PatRem, Medical FROM patient_remarque 
                                                            WHERE DateValidite <= GETDATE()
                                                            AND ( isnull(Economique,'N')<>'N' or isnull(Medical,'N')<>'N' or Encaisse <> 0 or Cession <> 0)) as T_pr on pa.IdPatient = T_pr.IdPatient
                                          left outer join (SELECT IdPatient, 'O' TeleAlarm FROM ta_abonnement WHERE archive = 0) as Pat_Ta on pa.IdPatient = Pat_Ta.IdPatient 
                                     WHERE pe.IdPersonne = pa.IdPersonne
                                     AND pe.IdPersonne = tp.NumPersonne 
                                     AND tp.NumTel =  '" + Tel + "'";


                    selonchoix1 = "SELECT 0 as IdPersonne, p.Telephone as Telephone, p.Nom as Nom, '' as Prenom, NULL as DateNaissance , '' as Sexe,  p.Pays, p.CodePostal, p.Commune, p.NumDansRue as NumeroDansRue, p.NomRue as Rue,";
                    selonchoix1 += "'' as rue2, '' as Batiment, ''as Escalier, '' as Etage, '' as Digicode, '' as Internom, '' as Porte, 0 as Longitude, 0 as Latitude, 'N' as TeleAlarme, 'N' as PatientRemarquable,'' as Medical, '' AS TexteSup,";
                    selonchoix1 += "'' as NomFact,'' as PrenomFact, '' as PaysFact, ' ' as CodePostalFact, '' as CommuneFact, '' as NumeroDansRueFact,";
                    selonchoix1 += "'' as RueFact, '' as Rue2Fact, '' as Num_Carte, '' as IdUnilab, '' as Email ";
                    selonchoix1 += " FROM Police_Hotel p ";
                    selonchoix1 += " WHERE 1=1 ";
                    selonchoix1 += " AND Telephone = '" + Tel + "'";
                    selonchoix1 += " UNION ";

                    break;

                case "NomPrenom":

                    if (Nom != "" && Nom != null)
                    {
                        Nom = Nom.Replace("'", "''");
                        selonchoix = " AND pe.Nom LIKE LOWER('" + Nom.ToLower().Trim() + "%')";
                    }

                    if (Prenom != "" && Prenom != null)
                        selonchoix += " AND pe.Prenom LIKE LOWER ('%" + Prenom.ToLower().Trim() + "%')";
                    break;

                case "DateNaiss":
                    if (DateNaiss != "" && DateNaiss != null)
                    {
                        //On verrifie la date
                        DateTime Testdate;

                        if (DateTime.TryParse(DateNaiss, out Testdate))
                        {
                            //Si elle est valide....
                            selonchoix += " AND pe.DateNaissance = '" + DateNaiss + "'";
                        }
                    }
                    break;              
            }
           
            
            try
            {
                sqlstr = selonchoix1;

                sqlstr += @"SELECT pe.IdPersonne, pe.Tel As Telephone, pe.Nom as Nom, pe.Prenom, pe.DateNaissance, UPPER(pe.Sexe) AS Sexe,'SUISSE' as Pays, pe.CodePostal,
                        pe.Commune, pe.NumeroDansRue, pe.Rue, pe.TexteSup as rue2, '' as Batiment, ''as Escalier, pe.Etage, pe.Digicode, pe.Internom, pe.Porte, CAST(pe.Longitude as float) / 100000 as Longitude,
                        CAST(pe.Latitude as float) /100000 as Latitude , ISNULL(Pat_Ta.TeleAlarm, 'N') TeleAlarme, ISNULL(T_pr.PatRem,'N') PatientRemarquable, T_pr.Medical as Medical, '' AS TexteSup,
                        Nom as NomFact,Prenom as PrenomFact, 'SUISSE' as PaysFact, Adm_CodePostal as CodePostalFact, Adm_Commune as CommuneFact, Adm_NumeroDansRue as NumeroDansRueFact,
                        Adm_Rue as RueFact, pe.ListeNoire as Rue2Fact, pe.Num_Carte as Num_Carte, pe.IdUnilab as IdUnilab, pe.Email 
                        FROM tablepersonne pe, tablepatient pa 
                             left outer join ( SELECT Distinct(IdPatient), 'O' PatRem, Medical 
                                               FROM patient_remarque 
                                               WHERE DateValidite <= GETDATE()
                                               AND ( isnull(Economique,'N')<>'N' or isnull(Medical,'N')<>'N' or Encaisse <> 0 or Cession <> 0)) as T_pr on pa.IdPatient = T_pr.IdPatient
                             left outer join (SELECT IdPatient, 'O' TeleAlarm FROM ta_abonnement WHERE archive = 0) as Pat_Ta on pa.IdPatient = Pat_Ta.IdPatient 
                            WHERE pe.IdPersonne = pa.IdPersonne ";
                sqlstr += selonchoix;

                sqlstr += " order by TeleAlarme Desc, Nom Asc";
              
                cmd.CommandText = sqlstr;

                //on déclare le DataTable pour recevoir les données
                DataTable dt1 = new DataTable();

                //on execute
                dt1.Load(cmd.ExecuteReader());    //on execute                     

                //dtRetour Pour le retour      
                DataTable dtRetour = new DataTable();

                dtRetour = dt1.Clone();   //On clone le dt1 initial pour n'avoir que la structure (on le rempli par la suite afin de le renvoyer)
                DataRow ligne;

                //*********Si c'est la police, Hotel, EMS référencé dans la police_hotel
                //*****(ATTENTION ne fonctionne qu'avec la recherche sur le n° de Tel!!)*****
                //on envoie l'adresse d'intervention PAS l'adresse de facturation ************
                //Est ce que le n° renvoyé est dans la table Police_hotel?
                bool present = false;       //Pour vérifier la présence ou non d'au moins 1 enregistrement

                if (ModeRecherche == "Tel")
                {
                    string sqlpolice_hotel = "SELECT Nom, Telephone As Telephone, NumDansRue, NomRue, CodePostal, Commune, CAST(Longitude as float) / 100000 as Longitude, CAST(Latitude as float) /100000 as Latitude ";
                    sqlpolice_hotel += " FROM Police_hotel WHERE Telephone =  @Numtel";

                    cmd.CommandText = sqlpolice_hotel;
                    cmd.Parameters.Clear();

                    //on prend seulement le 1er enregistrement renvoyé
                    if (dt1.Rows.Count != 0)
                    {
                        cmd.Parameters.AddWithValue("Numtel", dt1.Rows[0]["Telephone"].ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("Numtel", "+00000000000");
                    }

                    //on déclare le DataTable pour recevoir les données du résultat
                    DataTable DtTel = new DataTable();

                    //on execute
                    DtTel.Load(cmd.ExecuteReader());    //on execute       

                    if (DtTel.Rows.Count == 0)
                        present = false;
                    else present = true;


                    //S'il est dans la police Hotel ems
                    if (present == true)
                    {   //On ne rempli qu'un enregistrement

                        ligne = dtRetour.NewRow();

                        ligne[0] = -1;
                        ligne[1] = DtTel.Rows[0][1];  //tel
                        ligne[2] = DtTel.Rows[0][0];  //nom
                        ligne[3] = "";
                        ligne[4] = DBNull.Value;
                        ligne[5] = "";
                        ligne[6] = "";
                        ligne[7] = DtTel.Rows[0][4]; //cp
                        ligne[8] = DtTel.Rows[0][5]; //commune
                        ligne[9] = DtTel.Rows[0][2]; //Numrue 
                        ligne[10] = DtTel.Rows[0][3]; //Rue
                        ligne[11] = DtTel.Rows[0][0]; //Rue2
                        ligne[12] = "";
                        ligne[13] = "";
                        ligne[14] = "";
                        ligne[15] = "";
                        ligne[16] = "";
                        ligne[17] = "";                        
                        ligne[18] = DtTel.Rows[0][6];     //Longitude
                        ligne[19] = DtTel.Rows[0][7];     //Latitude
                        ligne[20] = "N";
                        ligne[21] = "N";
                        ligne[22] = "";
                        ligne[23] = "";
                        ligne[24] = "";
                        ligne[25] = "";
                        ligne[26] = "";
                        ligne[27] = DtTel.Rows[0][4];
                        ligne[28] = DtTel.Rows[0][5];
                        ligne[29] = DtTel.Rows[0][2];
                        ligne[30] = DtTel.Rows[0][3]; //Rue fact
                        ligne[31] = DtTel.Rows[0][0]; //Rue2 fact
                        ligne[32] = "";     //NumCarte
                        ligne[33] = "";     //IdUnilab
                        ligne[34] = "";     //Email

                        dtRetour.Rows.Add(ligne);
                    }
                }


                //On renvoi systematiquement <NOUVEAU> comme nom et adresse du 2eme enregistrement dans cette premiere ligne     
                if (dt1.Rows.Count > 0)   //On renvoi systematiquement <NOUVEAU>
                {
                    ligne = dtRetour.NewRow();

                    ligne[0] = -1;  //idpatient
                    ligne[1] = dt1.Rows[0][1]; //Tel
                    ligne[2] = "<NOUVEAU>"; //Nom
                    ligne[3] = "";
                    ligne[4] = DBNull.Value;
                    ligne[5] = "";
                    ligne[6] = "";
                    ligne[7] = dt1.Rows[0][7]; //cp
                    ligne[8] = dt1.Rows[0][8]; //commune
                    ligne[9] = dt1.Rows[0][9]; //NumRue
                    ligne[10] = dt1.Rows[0][10]; //Rue
                    ligne[11] = dt1.Rows[0][11]; //Rue2
                    ligne[12] = dt1.Rows[0][12];
                    ligne[13] = dt1.Rows[0][13];
                    ligne[14] = dt1.Rows[0][14];
                    ligne[15] = dt1.Rows[0][15];
                    ligne[16] = dt1.Rows[0][16];
                    ligne[17] = dt1.Rows[0][17];                    
                    ligne[18] = 0;                          //Longitude
                    ligne[19] = 0;                          //Latitude
                    ligne[20] = "N";                        //Telealarme =>N
                    ligne[21] = "N";                        //Patient Remarquable => N
                    ligne[22] = dt1.Rows[0][22];
                    ligne[23] = dt1.Rows[0][23];
                    ligne[24] = "";
                    ligne[25] = "";
                    ligne[26] = dt1.Rows[0][26];
                    ligne[27] = dt1.Rows[0][27];
                    ligne[28] = dt1.Rows[0][28];
                    ligne[29] = dt1.Rows[0][29];
                    ligne[30] = dt1.Rows[0][30]; //RueFact
                    ligne[31] = dt1.Rows[0][31]; //Rue2 fact
                    ligne[32] = ""; //NumCarte
                    ligne[33] = ""; //IdUnilab
                    ligne[34] = ""; //Email

                    dtRetour.Rows.Add(ligne);
                }
                //***************************                                                                                

                //Puis on boucle sur tout les enregistrements             
                for (int i = 0; i < (dt1.Rows.Count); i++)
                {
                    //*********Si c'est un TA on envoie l'adresse d'intervention PAS l'adresse de facturation ******************      
                    if (dt1.Rows[i]["TeleAlarme"].ToString() == "O")
                    {
                        //On vérifie que c'est pas un TA archivé
                        string sqlTaArchive = "SELECT t.Archive";
                        sqlTaArchive += " FROM ta_abonnement t, tablepatient p WHERE t.Idpatient = p.idPatient AND p.Idpersonne = @IdPersonne order by Archive asc";

                        cmd.CommandText = sqlTaArchive;
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("IdPersonne", dt1.Rows[i]["IdPersonne"].ToString());

                        //on déclare le DataTable pour recevoir les données du résultat                    
                        DataTable DtaArchive = new DataTable();

                        DtaArchive.Load(cmd.ExecuteReader());

                        //Nouvelle ligne
                        ligne = dtRetour.NewRow();

                        if (DtaArchive.Rows.Count > 0)  //Si on a un retour
                        {
                            if (DtaArchive.Rows[0][0].ToString() == "0")  //Si c'est un TA non archivé                            
                                ligne[20] = dt1.Rows[i][20];
                            else ligne[20] = "N";
                        }
                        else ligne[20] = "N";

                        ligne[0] = dt1.Rows[i][0];
                        ligne[1] = dt1.Rows[i][1];
                        ligne[2] = dt1.Rows[i][2];
                        ligne[3] = dt1.Rows[i][3];
                        ligne[4] = dt1.Rows[i][4];
                        ligne[5] = dt1.Rows[i][5];
                        ligne[6] = dt1.Rows[i][6];
                        ligne[7] = dt1.Rows[i][7];
                        ligne[8] = dt1.Rows[i][8];
                        ligne[9] = dt1.Rows[i][9];
                        ligne[10] = dt1.Rows[i][10];
                        ligne[11] = dt1.Rows[i][11];
                        ligne[12] = dt1.Rows[i][12];
                        ligne[13] = dt1.Rows[i][13];
                        ligne[14] = dt1.Rows[i][14];
                        ligne[15] = dt1.Rows[i][15];
                        ligne[16] = dt1.Rows[i][16];
                        ligne[17] = dt1.Rows[i][17];
                        ligne[18] = dt1.Rows[i][18];
                        ligne[19] = dt1.Rows[i][19];
                        //ligne[20] = dt1.Rows[i][20];
                        ligne[21] = dt1.Rows[i][21];
                        ligne[22] = dt1.Rows[i][22];
                        ligne[23] = dt1.Rows[i][23];
                        ligne[24] = dt1.Rows[i][24];
                        ligne[25] = dt1.Rows[i][25];
                        ligne[26] = dt1.Rows[i][26];
                        ligne[27] = dt1.Rows[i][7];
                        ligne[28] = dt1.Rows[i][8];
                        ligne[29] = dt1.Rows[i][9];
                        ligne[30] = dt1.Rows[i][10];
                        ligne[31] = dt1.Rows[i][11];
                        ligne[32] = dt1.Rows[i][32];
                        ligne[33] = dt1.Rows[0][33]; //IdUnilab
                        ligne[34] = dt1.Rows[0][34]; //EMail

                        dtRetour.Rows.Add(ligne);
                    }
                    else
                    {
                        //sinon c'est ni un TA ni police Hotel...
                        //On renvoi tout le contenu de DS qu'on charge dans DS1                                               
                        ligne = dtRetour.NewRow();

                        if (dt1.Rows[i][10].ToString().ToLower() == dt1.Rows[i][30].ToString().ToLower())
                        {
                            for (int j = 0; j < dtRetour.Columns.Count; j++)
                            {

                                ligne[j] = dt1.Rows[i][j];
                            }
                        }
                        else
                        {
                            ligne[0] = dt1.Rows[i][0];
                            ligne[1] = dt1.Rows[i][1];
                            ligne[2] = dt1.Rows[i][2];
                            ligne[3] = dt1.Rows[i][3];
                            ligne[4] = dt1.Rows[i][4];
                            ligne[5] = dt1.Rows[i][5];
                            ligne[6] = dt1.Rows[i][6];
                            ligne[7] = dt1.Rows[i][7];
                            ligne[8] = dt1.Rows[i][8];
                            ligne[9] = dt1.Rows[i][9];
                            ligne[10] = dt1.Rows[i][10];
                            ligne[11] = dt1.Rows[i][11];
                            ligne[12] = dt1.Rows[i][12];
                            ligne[13] = dt1.Rows[i][13];
                            ligne[14] = dt1.Rows[i][14];
                            ligne[15] = dt1.Rows[i][15];
                            ligne[16] = dt1.Rows[i][16];
                            ligne[17] = dt1.Rows[i][17];
                            ligne[18] = 0;
                            ligne[19] = 0;
                            ligne[20] = dt1.Rows[i][20];
                            ligne[21] = dt1.Rows[i][21];
                            ligne[22] = dt1.Rows[i][22];
                            ligne[23] = dt1.Rows[i][23];
                            ligne[24] = dt1.Rows[i][24];
                            ligne[25] = dt1.Rows[i][25];
                            ligne[26] = dt1.Rows[i][26];
                            ligne[27] = dt1.Rows[i][27];
                            ligne[28] = dt1.Rows[i][28];
                            ligne[29] = dt1.Rows[i][29];
                            ligne[30] = dt1.Rows[i][30];
                            ligne[31] = dt1.Rows[i][31];
                            ligne[32] = dt1.Rows[i][32];
                            ligne[33] = dt1.Rows[0][33]; //IdUnilab
                            ligne[34] = dt1.Rows[0][34];   //Email
                        }

                        dtRetour.Rows.Add(ligne);

                    }
                }       //Fin de boucle for

                //on retourne les résultats du DataSet                                 
                DataView view = dtRetour.DefaultView;      //Creation d'un DataView Pour pouvoir Trier le DataTable dtRetour

                view.Sort = "TeleAlarme DESC";

                dtRetourOk = view.ToTable();     //On créer un nouveau DataTable pour Stocker la vue triée de dtRetour
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche d'une personne :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }
           
            return dtRetourOk;
        }

        //Recherche les 10 derniers appels dans la base Smart
        public static DataTable Derniers10AppelsSmart(string NumTel, int NumPersonne, string DateJ)
        {
            DataTable dtResult = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);

            dbConnection.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Définition de la requette
                string sqlstr0 = "SELECT TOP(10) ta.DAP as DateOp, ta.Motif1 as LibelleMotif, (m.NomGeneve + ' ' + m.PrenomGeneve) as NomP,";
                sqlstr0 += "      p.IdPersonne as IdPersonne, tc.NConsultation as NConsultation";  
                sqlstr0 += " FROM tablepersonne p INNER JOIN tablepatient pa ON p.IdPersonne = pa.IdPersonne";
                sqlstr0 += "                      INNER JOIN tableactes ta ON ta.IndicePatient = pa.IdPatient";
                sqlstr0 += "                      INNER JOIN tableconsultations tc ON tc.CodeAppel = ta.Num";
                //sqlstr0 += "                      INNER JOIN Tel_Personne tp ON tp.NumPersonne = p.IdPersonne";
                sqlstr0 += "                      INNER JOIN tablemedecin m ON m.CodeIntervenant = ta.CodeIntervenant";
                sqlstr0 += " WHERE p.IdPersonne = @IdPersonne";
                //sqlstr0 += " AND tp.NumTel = @Tel";
                sqlstr0 += " AND ta.DAP < @DateJ";
                sqlstr0 += " ORDER BY ta.DAP DESC";                           

                cmd.CommandText = sqlstr0;               

                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("Tel", NumTel);
                cmd.Parameters.AddWithValue("IdPersonne", NumPersonne.ToString());
                cmd.Parameters.AddWithValue("DateJ", DateJ);

                dtResult.Load(cmd.ExecuteReader());    //on charge la table
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la récupération des 10 derniers appels (SmartRapport) :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            //on retourne les résultats du DataSet
            return dtResult;
        }

        //Recherche les 20 derniers appels dans la base SmartRapport à partir du n° de personne
        public static DataTable Derniers20AppelsSmart(string Nom, string Prenom, string DateNaiss)
        {
            DataTable dtResult = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);

            dbConnection.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Définition de la requette
                string sqlstr0 = "SELECT TOP(20) ta.DAP as DateOp, ta.Motif1 as LibelleMotif, (m.NomGeneve + ' ' + m.PrenomGeneve) as NomP,";
                sqlstr0 += "      p.IdPersonne as IdPersonne, tc.NConsultation as NConsultation";
                sqlstr0 += " FROM tablepersonne p INNER JOIN tablepatient pa ON p.IdPersonne = pa.IdPersonne";
                sqlstr0 += "                      INNER JOIN tableactes ta ON ta.IndicePatient = pa.IdPatient";
                sqlstr0 += "                      INNER JOIN tableconsultations tc ON tc.CodeAppel = ta.Num";                
                sqlstr0 += "                      INNER JOIN tablemedecin m ON m.CodeIntervenant = ta.CodeIntervenant";
                sqlstr0 += " WHERE p.Nom = @Nom";
                sqlstr0 += " AND p.Prenom = @Prenom";
                sqlstr0 += " AND p.DateNaissance = @DateNaiss";
                sqlstr0 += " ORDER BY ta.DAP DESC";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();              
                cmd.Parameters.AddWithValue("Nom", Nom);
                cmd.Parameters.AddWithValue("Prenom", Prenom);
                cmd.Parameters.AddWithValue("DateNaiss", DateNaiss);

                dtResult.Load(cmd.ExecuteReader());    //on charge la table
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la récupération des 20 derniers appels (SmartRapport) :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            //on retourne les résultats du DataSet
            return dtResult;
        }


        //Retourne le rapport à partir de la consultation et le l'ID de la personne
        public static DataTable RetourneRapport(Int32 NConsultation)
        {

            DataTable dtRapport = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);

            dbConnection.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT r.RapConcerne, r.NRapport, CONVERT(nvarchar, ta.DAP, 104) AS DateRapport, m.Nom, r.TypeRapport, rc.Valeur as ValeurCategorie, cr.LibelleCategorie";
                sqlstr0 += " FROM tablerapports r INNER JOIN tableconsultations tc ON tc.NConsultation = r.NConsultation";
                sqlstr0 += "                      INNER JOIN tableactes ta ON ta.Num = tc.CodeAppel";
                sqlstr0 += "                      INNER JOIN tablemedecin m ON m.CodeIntervenant = ta.CodeIntervenant";
                sqlstr0 += "                      INNER JOIN tablerapportcorps rc ON rc.NRapport = r.NRapport";
                sqlstr0 += "                      INNER JOIN tablecategoriedansrapport cr on cr.IdCategorie = rc.IdCategorie";
                sqlstr0 += " WHERE tc.NConsultation = @NConsultation";
                sqlstr0 += " ORDER BY r.NRapport Desc, cr.Ordre ASC";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("NConsultation", NConsultation);

                dtRapport.Load(cmd.ExecuteReader());    //on execute                                 
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de la recherche du rapport :" + e.Message);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtRapport;
        }


        //Retourne le motif d'une visite si elle est annulée
        public static string RetourneMotifAnnulationV(Int32 NConsultation)
        {
            string Retour = "";

            DataTable dtRetour = new DataTable();

            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = " SELECT m.LibelleMotif";
                sqlstr0 += " FROM appels a INNER JOIN motif m ON m.IdMotif = a.IdMotifAnnulation";
                sqlstr0 += " WHERE a.Num_Appel = @NumAppel";
              
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("NumAppel", NConsultation);
                
                dtRetour.Load(cmd.ExecuteReader());    //on execute

                //SI pas de résultats dans la base régul...
                if (dtRetour.Rows.Count > 0)
                {
                    Retour = "Motif d'annulation: " + dtRetour.Rows[0]["LibelleMotif"].ToString();
                }
                else
                {
                    Retour = "Visite Non Annulée";    //Pas de motif d'annulation
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la récupération du motif d'annulation (Regul). " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);               
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

        //Récupère les infos TA
        public static DataSet GetInfoTA(Int32 IdPersonne)
        {
            DataSet dsResult = new DataSet();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);

            dbConnection.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Pour l'onglet abonnement                  
                string Sqlstr0 = "SELECT pe.idPersonne, pe.Tel As Telephone, pe.Nom, pe.Prenom, pe.DateNaissance, UPPER(pe.Sexe) As Sexe, pe.CodePostal,";
                Sqlstr0 += " pe.Commune, pe.NumeroDansRue, pe.Rue, pe.Etage, pe.Digicode, pe.Internom, pe.Porte,";
                Sqlstr0 += "  tab.Export AS Pret, tab.ExportMcc AS DejaExporte, tab.N_TA, tab.IDAbonnement, tab.IdContrat,";
                Sqlstr0 += "  alf.TF_Nom, alf.TF_Prenom, alf.TF_NumeroPostal, alf.TF_Localite, alf.TF_Adresse, alf.TF_Sexe,";
                Sqlstr0 += "  acle.NumeroCle";
                Sqlstr0 += " FROM tablepersonne pe, tablepatient pa, ta_abonnement tab, ta_abonnementlieufacture alf,";
                Sqlstr0 += " ta_abonnementcle acle";
                Sqlstr0 += " WHERE pe.IdPersonne = pa.IdPersonne";
                Sqlstr0 += " AND pa.IdPatient = tab.IdPatient";
                Sqlstr0 += " AND tab.IdAbonnement = alf.TF_IdAbonnement";
                Sqlstr0 += " AND alf.TF_IdAbonnement = acle.IdAbonnement";
                Sqlstr0 += " AND tab.Archive <> 1";
                Sqlstr0 += " AND pe.idPersonne = @IdPersonne";

                cmd.CommandText = Sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdPersonne", IdPersonne.ToString());

                dsResult.Tables.Add("Abonnement");
                dsResult.Tables["Abonnement"].Load(cmd.ExecuteReader());    //on charge la table

                //Pour l'onglet Contacts (liste)
                Sqlstr0 = "SELECT aurg.Lien, aurg.Nom, aurg.Prenom, aurg.Telephone, aurg.NumeroRue, aurg.Rue, aurg.Np,";
                Sqlstr0 += "  aurg.Localite, aurg.Tel2, aurg.Tel3";
                Sqlstr0 += " FROM tablepersonne pe, tablepatient pa, ta_abonnement tab, ta_abonnementurgence aurg";
                Sqlstr0 += " WHERE pe.IdPersonne = pa.IdPersonne";
                Sqlstr0 += " AND pa.IdPatient = tab.IdPatient";
                Sqlstr0 += " AND tab.IdAbonnement = aurg.IdAbonnement";
                Sqlstr0 += " AND tab.Archive <> 1";
                Sqlstr0 += " AND pe.IdPersonne =  @IdPersonne";

                cmd.CommandText = Sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdPersonne", IdPersonne.ToString());

                dsResult.Tables.Add("Contacts");
                dsResult.Tables["Contacts"].Load(cmd.ExecuteReader());    //on charge la table


                //Pour l'onglet clé
                Sqlstr0 = "SELECT acle.NumeroCle, acle.Commentaire, acle.DateAttribution,";
                Sqlstr0 += " tab.IdContrat, CASE WHEN tab.ClePresente=1 THEN 'Oui' ELSE 'Non' END AS Cle_presente,";
                Sqlstr0 += "  CASE WHEN tab.FaxFsasd=1 THEN 'Oui' ELSE 'Non' END AS Fax_IMAD,";
                Sqlstr0 += "  CASE WHEN tab.DossierBleu=1 THEN 'Oui' ELSE 'Non' END AS Dossier_bleu";
                Sqlstr0 += " FROM tablepersonne pe, tablepatient pa, ta_abonnement tab,";
                Sqlstr0 += " ta_abonnementcle acle";
                Sqlstr0 += " WHERE pe.IdPersonne = pa.IdPersonne";
                Sqlstr0 += " AND pa.IdPatient = tab.IdPatient";
                Sqlstr0 += " AND tab.IdAbonnement = acle.IdAbonnement";
                Sqlstr0 += " AND tab.Archive <> 1";
                Sqlstr0 += " AND pe.IdPersonne =  @IdPersonne";

                cmd.CommandText = Sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdPersonne", IdPersonne.ToString());

                dsResult.Tables.Add("Cle");
                dsResult.Tables["Cle"].Load(cmd.ExecuteReader());    //on charge la table

                //Pour l'onglet Journal (Liste)
                Sqlstr0 = "SELECT aj.TypeOp, aj.EnvoiDe, aj.TexteA, aj.DateOp, aj.NbCle, aj.ICE, aj.Commentaire";
                Sqlstr0 += " FROM tablepersonne pe, tablepatient pa, ta_abonnement tab,";
                Sqlstr0 += " ta_abonnementjournal aj";
                Sqlstr0 += " WHERE pe.IdPersonne = pa.IdPersonne";
                Sqlstr0 += " AND pa.IdPatient = tab.IdPatient";
                Sqlstr0 += " AND tab.IdAbonnement = aj.IdAbonnement";
                Sqlstr0 += " AND pe.IdPersonne =  @IdPersonne";

                cmd.CommandText = Sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdPersonne", IdPersonne.ToString());

                dsResult.Tables.Add("Journal");
                dsResult.Tables["Journal"].Load(cmd.ExecuteReader());    //on charge la table

                //Pour l'onglet facture (Liste)
                Sqlstr0 = "SELECT tf.NFacture, tf.Date_facture, tf.Montant, tf.Début_periode, tf.Fin_période,";
                Sqlstr0 += " tf.Payé, tf.Moyen, CASE WHEN tf.Acquité=1 THEN 'Oui' ELSE 'Non' END AS Acquité,";
                Sqlstr0 += " tf.Tarif_mensuel, CASE WHEN tf.Imprimé=1 THEN 'Oui' ELSE 'Non' END AS Imprimé,";
                Sqlstr0 += " CASE WHEN tf.SBVR=1 THEN 'Oui' ELSE 'Non' END AS SBVR, tf.Remarque";
                Sqlstr0 += " FROM tablepersonne pe, tablepatient pa, ta_abonnement tab,";
                Sqlstr0 += " ta_factures tf";
                Sqlstr0 += " WHERE pe.IdPersonne = pa.IdPersonne";
                Sqlstr0 += " AND pa.IdPatient = tab.IdPatient";
                Sqlstr0 += " AND tab.IdAbonnement = tf.IdAbonnement";
                Sqlstr0 += " AND pe.IdPersonne =  @IdPersonne";

                cmd.CommandText = Sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdPersonne", IdPersonne.ToString());

                dsResult.Tables.Add("Facture");
                dsResult.Tables["Facture"].Load(cmd.ExecuteReader());    //on charge la table


                //Pour les infos de l'onglet Dossiers Médicaux                   
                Sqlstr0 = " SELECT CASE WHEN td.TD_RisqueChute=1 THEN 'Oui' ELSE 'Non' END AS RisqueChute,";
                Sqlstr0 += " td.TD_PbMedicaux, td.TD_Traitements, td.TD_Attitudes, td.TD_Poids,";
                Sqlstr0 += " CASE WHEN td.TD_FS_Inf=1 THEN 'Oui' ELSE 'Non' END AS Infirmiere,";
                Sqlstr0 += " CASE WHEN td.TD_FS_Ergo=1 THEN 'Oui' ELSE 'Non' END AS Ergotherapie,";
                Sqlstr0 += " CASE WHEN td.TD_FS_AideSoins=1 THEN 'Oui' ELSE 'Non' END AS Aide_soins,";
                Sqlstr0 += " CASE WHEN td.TD_FS_AideMenag=1 THEN 'Oui' ELSE 'Non' END AS Aide_menagere,";
                Sqlstr0 += " CASE WHEN td.TD_FS_AideFam=1 THEN 'Oui' ELSE 'Non' END AS Aide_familiale,";
                Sqlstr0 += " CASE WHEN td.TD_FS_Repas=1 THEN 'Oui' ELSE 'Non' END AS Repas_domicile,";
                Sqlstr0 += " CASE td.TD_TypeAppareil WHEN 1 THEN 'Chez Télécontact' WHEN 2 THEN 'Chez Suisscom'";
                Sqlstr0 += " WHEN 3 THEN 'Pas d''appareil' END AS Appareil";
                Sqlstr0 += " FROM tablepersonne pe, tablepatient pa, ta_abonnement tab,";
                Sqlstr0 += " ta_abonnementdossier td ";     //LEFT OUTER JOIN medecinsville mv ON td.TD_ListeMedecinsTTT = mv.Num";
                Sqlstr0 += " WHERE pe.IdPersonne = pa.IdPersonne";
                Sqlstr0 += " AND pa.IdPatient = tab.IdPatient";
                Sqlstr0 += " AND tab.IdAbonnement = td.IdAbonnement";
                Sqlstr0 += " AND pe.IdPersonne =  @IdPersonne";

                cmd.CommandText = Sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdPersonne", IdPersonne.ToString());

                dsResult.Tables.Add("DosMed");
                dsResult.Tables["DosMed"].Load(cmd.ExecuteReader());    //on charge la table

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche dans les TA :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            //on retourne les résultats du DataSet
            return dsResult;
        }


        //Récupération des remarques des patients Remarquables
        public static DataTable GetPatientRemarquable(Int32 IdPersonne)
        {
            DataTable dtResult = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);

            dbConnection.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Définition de la requette
                string sqlstr0 = "SELECT CASE WHEN Encaisse='0' THEN 'Non' ELSE 'Oui' END AS Encaisse1,";
                sqlstr0 += "CASE WHEN Cession='0' THEN 'Non' ELSE 'Oui' END AS Cession1,";
                sqlstr0 += "Medical, Economique";
                sqlstr0 += " FROM tablepersonne pe, tablepatient pa, patient_remarque pr";
                sqlstr0 += " WHERE pe.IdPersonne = pa.IdPersonne";
                sqlstr0 += " AND pa.IdPatient = pr.IdPatient";
                sqlstr0 += " AND pe.IdPersonne =  @IdPersonne";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdPersonne", IdPersonne.ToString());

                dtResult.Load(cmd.ExecuteReader());    //on charge la table

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche dans les patients remarquables :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }
           

            //on retourne les résultats du DataSet
            return dtResult;
        }
       
        //Retourne un nvx n° de personne
        private static Int32 NvxNum(string table)
        {
            Int32 NMax = -1;

            //On recherche le plus grand id dans SmartRapport            
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);

            dbConnection.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = dbConnection;
            try
            {
                string sqlstr0 = "";

                //IdPersonne
                switch (table)
                {
                    case "tablepersonne": sqlstr0 = "SELECT MAX(IdPersonne) FROM tablepersonne"; break;
                    case "tablepatient": sqlstr0 = "SELECT MAX(IdPatient) FROM tablepatient"; break;
                    case "tableactes": sqlstr0 = "SELECT MAX(Num) FROM tableactes"; break;
                }

                cmd.CommandText = sqlstr0;

                DataTable dtNum = new DataTable();
                dtNum.Load(cmd.ExecuteReader());    //on execute

                NMax = Int32.Parse(dtNum.Rows[0][0].ToString()) + 1;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la récupération du n° de " + table + ": " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return NMax;
        }


        //On récupère le num Patient dans table Patient de SmartRapport
        private static Int32 RecupNumPatient(Int32 Pers)
        {
            int Retour = -1;

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);

            dbConnection.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Table Status_Visite
                string sqlstr0 = "SELECT IdPatient FROM tablepatient WHERE IdPersonne = @Personne";
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Personne", Pers);

                DataTable dtPatient = new DataTable();
                dtPatient.Load(cmd.ExecuteReader());    //on execute

                if (dtPatient.Rows.Count > 0)   //Si on a un enregistrement
                {
                    if (dtPatient.Rows[0][0] != DBNull.Value)
                        Retour = Int32.Parse(dtPatient.Rows[0][0].ToString());
                    else Retour = -1;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche du n° dans tablepatient :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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


        //On vérifie si c'est un TA
        public static string VerifTA(int NumP)
        {
            string retour = "KO";

            //On recherche Si c'est un TA
            //Chaine de connection...
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);

            dbConnection.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //TablePatient
                string sqlstr0 = "SELECT TypeAbonnement FROM tablepatient WHERE IdPersonne = @NumPers";
                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("NumPers", NumP);

                DataTable dtTa = new DataTable();
                dtTa.Load(cmd.ExecuteReader());    //on execute

                if (dtTa.Rows.Count > 0)
                {
                    if (dtTa.Rows[0][0].ToString() == "TA")
                        retour = "OK";
                    else retour = "KO";
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la vérification du TA." + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                retour = "KO";
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return retour;
        }

        //On vérifie s'il y a des factures impayées
        public static DataTable GetFacturesImpayees(int NumPersonne)
        {
            string listeMedecinsNonLamal = "";      //Liste des médecins non lamal
            string listeFactureException = "948138,949274,950690";      //Liste des factures à exclure pour diverses raisons

            //On commence par récupérer les médecins non LAMAL
            DataTable dtMedNonLamal = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Horaire"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);

            dbConnection.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Définition de la requette
                string sqlstr0 = "SELECT m.Id FROM Horairemed.dbo.Medecins m WHERE m.LAMAL = 'false'";
               
                cmd.CommandText = sqlstr0;

                dtMedNonLamal.Load(cmd.ExecuteReader());    //on charge la table
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche des médecins non lamal :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }            

            for(int i = 0; i < dtMedNonLamal.Rows.Count; i++)
            {
                listeMedecinsNonLamal += dtMedNonLamal.Rows[i][0].ToString() + ",";
            }

            //On enlève la dernière virgule
            if (listeMedecinsNonLamal.Length > 0)
                listeMedecinsNonLamal = listeMedecinsNonLamal.Remove(listeMedecinsNonLamal.Length - 1);

            //*********Fin recup medecins non lamal***
            DataTable dtResult = new DataTable();

            connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
            dbConnection = new SqlConnection(connex);

            dbConnection.Open();
            cmd = new SqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                //Définition de la requette
                string sqlstr0 = "SELECT p.IdPersonne, p.NomPatient, NFacture , CONVERT(VARCHAR(10), DateCreation, 104) as DateFacture,";
                sqlstr0 += "             CAST(ROUND(SUM(solde), 2) AS NUMERIC(12,2)) as MONTANT_DU";
                sqlstr0 += " FROM(SELECT tm.Nom as NomMED, tm.Mail, ta.DAP AS FacDateEnvoyee, f.NFacture, (f.TotalFacture - sum(fe.Montant)) as TotalFacture,";
                sqlstr0 += "      f.Solde, f.DateCreation, (pe.Nom + ' ' + pe.prenom) as NomPatient, pe.IdPersonne, f.AdresseDestinataire, pe.Tel";
                sqlstr0 += "      FROM facture f INNER JOIN facture_etats fe ON f.NFacture = fe.NFacture";
                sqlstr0 += "                     INNER JOIN factureConsultation fc ON fc.NFacture = f.NFacture";
                sqlstr0 += "                     INNER JOIN facture_status fs ON fs.NFacture = f.NFacture";
                sqlstr0 += "                     INNER JOIN tableconsultations tc ON fc.NConsultation = tc.NConsultation";
                sqlstr0 += "                     INNER JOIN tableactes ta ON tc.CodeAppel = ta.Num";
                sqlstr0 += "                     INNER JOIN tablemedecin tm ON ta.CodeIntervenant = tm.CodeIntervenant";
                sqlstr0 += "                     INNER JOIN tablepatient pa ON tc.IndicePatient = pa.IdPatient";
                sqlstr0 += "                     INNER JOIN tablepersonne pe ON pa.IdPersonne = pe.IdPersonne";
                sqlstr0 += "      WHERE pe.IdPersonne = @IdPersonne";
                sqlstr0 += "      AND fs.FacDateCession is Null";
                sqlstr0 += "      AND fs.FacDateAnnulee is Null";
                sqlstr0 += "      AND f.TotalFacture > 0";
                sqlstr0 += "      AND f.Solde > 10";
                //On enlève les arrangements de paiement
                sqlstr0 += "      AND (fs.LimiteStopRappel < getdate() OR fs.LimiteStopRappel is null) ";
                //Délais laissé pour le payement 
                // sqlstr0 += "      AND(fs.FacDateAcquittee > DATEADD(month, -4, GETDATE()) or fs.FacDateAcquittee is null)";                
                sqlstr0 += "      AND(fs.FacDateAcquittee is null)";
                sqlstr0 += "      AND fe.DateOp <= DATEADD(month, -4, GETDATE())";
                sqlstr0 += "      AND ta.DAP >= DATEADD(Year, -5, GETDATE()) AND ta.DAP <= DATEADD(month, -4, GETDATE())";

                //On enleve ceux qui releve du HPR (Les réfugiés)
                sqlstr0 += "      AND f.NFacture not in (SELECT fa.NFacture ";
                sqlstr0 += "                             FROM facture fa INNER JOIN factureConsultation fac ON fac.NFacture = fa.NFacture";
                sqlstr0 += "                                             INNER JOIN tableconsultations tac ON tac.NConsultation = fac.NConsultation";
                sqlstr0 += "                                             INNER JOIN tableactes taa ON tac.CodeAppel = taa.Num";
                sqlstr0 += "                             WHERE fa.CodeDestinataire in (15))";

                //Blocage Helsana/Progres 16.05.2018 au 10.07.2019 inclus (pour enlever l'affichage des soldes négatifs...les médecins comprennent rien!!!)
                sqlstr0 += "      AND f.NFacture not in (SELECT fa.NFacture";
                sqlstr0 += "                             FROM facture fa INNER JOIN factureConsultation fac ON fac.NFacture = fa.NFacture";
                sqlstr0 += "                                             INNER JOIN tableconsultations tac ON tac.NConsultation = fac.NConsultation";
                sqlstr0 += "                                             INNER JOIN tableactes taa ON tac.CodeAppel = taa.Num";
                sqlstr0 += "                             WHERE taa.DAP >= '16.05.2018' and taa.DAP <= '10.07.2019' and fa.CodeDestinataire in (130, 194))";
                //Blocage Groupe mutuel
                sqlstr0 += "      AND f.NFacture not in (SELECT fa.NFacture";
                sqlstr0 += "                             FROM facture fa INNER JOIN factureConsultation fac ON fac.NFacture = fa.NFacture";
                sqlstr0 += "                                             INNER JOIN tableconsultations tac ON tac.NConsultation = fac.NConsultation";
                sqlstr0 += "                                             INNER JOIN tableactes taa ON tac.CodeAppel = taa.Num";
                sqlstr0 += "                                             INNER JOIN tablemedecin tm ON tm.CodeIntervenant = taa.CodeIntervenant";
                sqlstr0 += "                             WHERE (taa.DAP >= '16.05.2018' and taa.DAP <= '10.07.2019')";
                sqlstr0 += "                             OR (taa.DAP >= '01.01.2024' and taa.DAP <= '30.05.2024')";
                sqlstr0 += "                             AND fa.CodeDestinataire in (59))";               
                //sqlstr0 += "                             AND tm.CodeIntervenant in ("+ listeMedecinsNonLamal + "))";
                //Exceptions pour raisons diverses
                sqlstr0 += "      AND f.NFacture not in (" + listeFactureException + ")";
                //************************************************Fin blocage*****************************************
                sqlstr0 += "     GROUP BY tm.Nom, tm.Mail, f.NFacture, f.DateCreation, f.AdresseDestinataire, pe.Tel,f.TotalFacture, f.Solde, pe.Nom, pe.prenom,ta.DAP, pe.IdPersonne) AS p";
                sqlstr0 += " GROUP BY p.IdPersonne, p.NomPatient, p.NFacture, p.DateCreation";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdPersonne", NumPersonne.ToString());

                dtResult.Load(cmd.ExecuteReader());    //on charge la table
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche des factures impayées :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            //on retourne les résultats du DataSet
            return dtResult;
        }


        //Gestion des Tels   ########A voir
        private static void AjouteTel(int IdPersonne, string tel)
        {
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);

            try
            {
                dbConnection.Open();

                SqlCommand cmd = dbConnection.CreateCommand();
                cmd.Connection = dbConnection;

                //Gestion des n° de Tel...On regarde s'il existe dans la table Tel_Personne                                                                       
                string sqlstr0 = "SELECT NumPersonne, NumTel";
                sqlstr0 += " FROM Tel_Personne";
                sqlstr0 += " WHERE NumPersonne = @IdPersonne";
                sqlstr0 += " AND NumTel = @Tel";

                cmd.CommandText = sqlstr0;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdPersonne", IdPersonne);
                cmd.Parameters.AddWithValue("Tel", tel);

                DataTable Telephone = new DataTable();
                Telephone.Load(cmd.ExecuteReader());

                //Si on ne l'a pas trouvé, on l'ajoute à la table
                if (Telephone.Rows.Count == 0)
                {
                    sqlstr0 = "INSERT INTO Tel_Personne";
                    sqlstr0 += " VALUES(@NumPersonne, @Tel, GetDate())";

                    cmd.CommandText = sqlstr0;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("NumPersonne", IdPersonne);
                    cmd.Parameters.AddWithValue("Tel", tel);

                    cmd.ExecuteReader();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(" Erreur : " + e.Message);
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

        //On recherche le médecin traitant du patient
        public static string[,] RechercheMedTraitant(int NumPersonne)
        {
            string[,] MedTraitant = new string[1, 2];

            //On initialise le tableau à 2 Dim
            MedTraitant[0, 0] = "";
            MedTraitant[0, 1] = "";

            if (NumPersonne != -1)
            {
                //Chaine de connection... ici on attaque SmartRapport
                string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
                SqlConnection dbConnection = new SqlConnection(connex);

                dbConnection.Open();
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = dbConnection;

                //pour le médecin traitant
                string SqlStr0 = "";
                DataTable dtMedTraitant = new DataTable();

                //Si c'est un patient déjà connu, on recherche son médecin traitant
                try
                {
                    SqlStr0 = "SELECT mv.Nom, mv.Prenom FROM tablepersonne p INNER JOIN tablepatient pa ON p.IdPersonne = pa.IdPersonne";
                    SqlStr0 += "                                             INNER JOIN tablepatientmedttt tt ON pa.IdPatient = tt.IdPatient";
                    SqlStr0 += "                                             INNER JOIN medecinsville mv ON tt.IdMedecin = mv.Num";
                    SqlStr0 += " WHERE p.IdPersonne = @Idpersonne";

                    cmd.CommandText = SqlStr0;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("Idpersonne", NumPersonne);

                    dtMedTraitant.Load(cmd.ExecuteReader());

                    if (dtMedTraitant.Rows.Count > 0)
                    {
                        MedTraitant[0, 0] = dtMedTraitant.Rows[0]["Nom"].ToString();
                        MedTraitant[0, 1] = dtMedTraitant.Rows[0]["Prenom"].ToString();
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche du médecins traitant :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

            return MedTraitant;
        }

        public static bool GetMedecinTraitant(int NumPersonne, out int IdMedecin, out string Nom, out string Prenom)
        {
            IdMedecin = -1;
            Nom = "";
            Prenom = "";

            if (NumPersonne == -1)
            {
                return false;
            }

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);

            try
            {
                dbConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbConnection;

                string SqlStr0 = "SELECT tt.IdMedecin, mv.Nom, mv.Prenom FROM tablepersonne p INNER JOIN tablepatient pa ON p.IdPersonne = pa.IdPersonne";
                SqlStr0 += "                                             INNER JOIN tablepatientmedttt tt ON pa.IdPatient = tt.IdPatient";
                SqlStr0 += "                                             INNER JOIN medecinsville mv ON tt.IdMedecin = mv.Num";
                SqlStr0 += " WHERE p.IdPersonne = @Idpersonne";

                cmd.CommandText = SqlStr0;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Idpersonne", NumPersonne);

                DataTable dtMedTraitant = new DataTable();
                dtMedTraitant.Load(cmd.ExecuteReader());

                if (dtMedTraitant.Rows.Count > 0)
                {
                    IdMedecin = int.Parse(dtMedTraitant.Rows[0]["IdMedecin"].ToString());
                    Nom = dtMedTraitant.Rows[0]["Nom"].ToString();
                    Prenom = dtMedTraitant.Rows[0]["Prenom"].ToString();
                    return true;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche du médecins traitant :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return false;
        }

        public static DataTable ChargeListeMedecinsTraitants()
        {
            DataTable dtMedecin = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);

            try
            {
                dbConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbConnection;

                string sqlstr0 = "SELECT * FROM medecinsville WHERE LTRIM(RTRIM(ISNULL(Nom, ''))) <> '' ORDER BY Nom";

                cmd.CommandText = sqlstr0;

                dtMedecin.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement de la liste des médecins traitants :" + ex.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtMedecin;
        }

        public static void MajMedecinTraitant(int NumPersonne, int IdMedecin)
        {
            if (NumPersonne == -1)
            {
                return;
            }

            int IdPatient = RecupNumPatient(NumPersonne);

            if (IdPatient == -1)
            {
                return;
            }

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Smart"].ToString();
            SqlConnection dbConnection = new SqlConnection(connex);

            try
            {
                dbConnection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = dbConnection;

                if (IdMedecin <= 0)
                {
                    cmd.CommandText = "DELETE FROM tablepatientmedttt WHERE IdPatient = @IdPatient";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("IdPatient", IdPatient);
                    cmd.ExecuteNonQuery();
                    return;
                }

                cmd.CommandText = "SELECT IdPatient FROM tablepatientmedttt WHERE IdPatient = @IdPatient";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdPatient", IdPatient);

                DataTable dtMedTraitant = new DataTable();
                dtMedTraitant.Load(cmd.ExecuteReader());

                if (dtMedTraitant.Rows.Count > 0)
                {
                    cmd.CommandText = "UPDATE tablepatientmedttt SET IdMedecin = @IdMedecin WHERE IdPatient = @IdPatient";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO tablepatientmedttt (IdPatient, IdMedecin) VALUES(@IdPatient, @IdMedecin)";
                }

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("IdPatient", IdPatient);
                cmd.Parameters.AddWithValue("IdMedecin", IdMedecin);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la mise à jour du médecin traitant :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }
        }


        //############################################FIN FONCTIONS INTERROGEANT BASE SMARTRAPPORT###########################################################
        #endregion

        #region Fonction travaillant avec FicheVisite
        //###############################################FONCTIONS INTERROGEANT BASE FICHEVISITE############################################################
        //On supprime la Fiche car la visite e été annulée
        public static void AnnuleFicheConsultation(int NumVisite)
        {
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
            string SqlStr4 = "";

            try
            {
                //Dans une transaction, on créé les enreg pour visite, structconsult, correspondance, assurances                

                MySqlTransaction trans;   //Déclaration d'une transaction

                //Def des requettes
                //*****visite***
                SqlStr0 = "DELETE FROM visite ";
                SqlStr0 += " WHERE Num_Appel = " + NumVisite;

                //****structconsult***
                SqlStr1 = "DELETE FROM structconsult ";
                SqlStr1 += " WHERE Num_Appel = " + NumVisite;

                //****correspondance***
                SqlStr2 = "DELETE FROM correspondance ";
                SqlStr2 += " WHERE Num_Appel = " + NumVisite;

                //****assurances*****                              
                SqlStr3 = "DELETE FROM assurances ";
                SqlStr3 += " WHERE Num_Appel = " + NumVisite;

                //****ListeAMM*****                              
                SqlStr4 = "DELETE FROM listeamm ";
                SqlStr4 += " WHERE Num_Appel = " + NumVisite;

                //Ouverture d'une transaction
                trans = dbConnection.BeginTransaction();
                try
                {
                    //on execute les requettes                                       
                    cmd.CommandText = SqlStr0; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr1; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr2; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr3; cmd.Transaction = trans; cmd.ExecuteNonQuery();
                    cmd.CommandText = SqlStr4; cmd.Transaction = trans; cmd.ExecuteNonQuery();

                    //on valide la transaction
                    trans.Commit();                   
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    System.Windows.Forms.MessageBox.Show("Erreur lors de la suppression de la fiche de consultation. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                System.Windows.Forms.MessageBox.Show("Erreur lors de la suppression de la fiche de consultation. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        //Attribution de la fiche de consultation lorsque le régulation Acquitte la visite à la place du médecin
        public static void AttribueFicheAQ(int NumVisite, int CodeMedecin)
        {
            //Chaine de connection... ici on attaque MariaDB
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_FicheVisite"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;
                         
            try
            {                              
                //*****visite***
                string SqlStr0 = "UPDATE visite SET CodeMedecin = " + CodeMedecin;
                SqlStr0 += " WHERE Num_Appel = " + NumVisite;

                //on execute la requette
                cmd.CommandText = SqlStr0; cmd.ExecuteNonQuery();                
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de l'attribution la fiche de consultation au médecin. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        //############################################FIN FONCTIONS INTERROGEANT BASE FICHEVISITE###########################################################
        #endregion

        #region Fonctions copiant les appels Audios de l'enregistreur vers le serveur Nginx
        //#########################Recupération de l'appel audio de la visite en cours##########################################        
        //on vérifie la présence d'un fichier audio
        public static void VerifSiAudioPresent()
        {
            //On charge la liste des appels non terminées
            DataTable dtAppelsNonTermines = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {
                string sqlstr0 = "SELECT a.Num_Appel, a.Tel_Appel, s.DateOp";                
                sqlstr0 += " FROM appelsencours a inner join suiviappel s ON a.Num_Appel = s.Num_Appel ";
                sqlstr0 += " WHERE s.Type_Operation = 'Création'";
                cmd.CommandText = sqlstr0;

                dtAppelsNonTermines.Load(cmd.ExecuteReader());    //on execute               
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche des appels non terminées...table appelsencours/suiviappel :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            string CheminAudio = ConfigurationManager.AppSettings["Path_Audio_Dest"].ToString(); 

            //Pour chaque visite on vérifie la présence du fichier audio
            for (int i = 0; i < dtAppelsNonTermines.Rows.Count; i++)
            {
                 if (!File.Exists( System.IO.Path.Combine(CheminAudio, dtAppelsNonTermines.Rows[i]["Num_Appel"].ToString() + ".wav")))
                 {
                     //il n'existe pas donc on va le chercher
                     string Num_Appel = dtAppelsNonTermines.Rows[i]["Num_Appel"].ToString();
                     string Tel_Appel = dtAppelsNonTermines.Rows[i]["Tel_Appel"].ToString();
                     DateTime DateAppel = DateTime.Parse(dtAppelsNonTermines.Rows[i]["DateOp"].ToString());
                     
                     RecupBandeAudio(Num_Appel, Tel_Appel, DateAppel);
                 }
            } 
            
            //Puis on efface les autres fichiers (ils ne servent plus à rien)
            NettoyeFichierAudio(dtAppelsNonTermines);
        }
        
        
        public static void RecupBandeAudio(string Num_Appel, string Num_tel, DateTime DateAppel)
        {
            string pathSource = ConfigurationManager.AppSettings["Path_Audio_Source"].ToString();          
            string pathDest = ConfigurationManager.AppSettings["Path_Audio_Dest"].ToString();            
            string FichierSource = "";
            string FichierDest = "";

            //Recherche de l'enregistrement
            //on formate les dates....On enlève les secondes et 20 minutes pour la date de début
            DateTime Datedeb = DateTime.ParseExact(DateAppel.AddMinutes(-20).ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null);
            DateTime DateFin = DateTime.ParseExact(DateAppel.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null);

            //puis on format correctement pour MySql en rajoutant l'heure que l'on a défini           
            String Datedebut = String.Format("{0:yyyy.MM.dd HH:mm:ss}", Datedeb);
            String DatedeFin = String.Format("{0:yyyy.MM.dd HH:mm:ss}", DateFin);

            //Chaine de connection... ici on attaque Mysql de l'enregistreur
            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Audio"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;                        

            String sqlstr0 = "SELECT CVSKEY as idappel, CVSC00 as appelant, CVSC01 as appele, CVSDIR as direction, CVSSEC as duree, CVSLCT as fichier, CVSSDT as debutAppel, CVSEDT as finAppel FROM CVS";
            sqlstr0 += " WHERE CVSSDT between '" + Datedebut + "' and '" + DatedeFin + "'";
            sqlstr0 += " AND CVSC01 in('4950','4949','4901','4902') AND CVSDIR = 0";       //A destination de ces seuls n°
            sqlstr0 += " AND CVSC00 = '" + Num_tel + "'";
            sqlstr0 += " ORDER BY CVSSDT DESC";

            cmd.CommandText = sqlstr0;

            DataTable dtEnregistrement = new DataTable();
           
            dtEnregistrement.Load(cmd.ExecuteReader());       //on execute  

            //On prend le 1er appel
            if (dtEnregistrement.Rows.Count > 0)
            {              
                //On copie l'enregistrement depuis l'enregistreur vers le dossier du server Nginx                         
                FichierSource = System.IO.Path.Combine(pathSource, dtEnregistrement.Rows[0]["fichier"].ToString());
                FichierDest = System.IO.Path.Combine(pathDest, Num_Appel + ".wav");

                File.Copy(FichierSource, FichierDest, false);                                 
            }
            else //Sinon vérifier si c'est pas un TA ou un n° caché
            {
                sqlstr0 = "SELECT CVSKEY as idappel, CVSC00 as appelant, CVSC01 as appele, CVSDIR as direction, CVSSEC as duree, CVSLCT as fichier, CVSSDT as debutAppel, CVSEDT as finAppel FROM CVS";
                sqlstr0 += " WHERE CVSSDT between '" + Datedebut + "' and '" + DatedeFin + "'";
                sqlstr0 += " AND CVSC01 in('4950','4949','4901','4902') AND CVSDIR = 0";       //A destination de ces seuls n°
                sqlstr0 += " AND (CVSC00 = '0229791090' OR CVSC00 = '')";
                sqlstr0 += " ORDER BY CVSSDT DESC";

                cmd.CommandText = sqlstr0;

                dtEnregistrement.Clear();
                dtEnregistrement.Load(cmd.ExecuteReader());       //on execute  

                //On prend le 1er appel
                if (dtEnregistrement.Rows.Count > 0)
                {
                    if (dtEnregistrement.Rows[0]["appelant"].ToString() == "")
                        Console.WriteLine("Numero masqué");
                    else
                        Console.WriteLine(dtEnregistrement.Rows[0]["appelant"].ToString());
                                    
                    //On copie l'enregistrement depuis l'enregistreur vers le dossier du server Nginx                         
                    FichierSource = System.IO.Path.Combine(pathSource, dtEnregistrement.Rows[0]["fichier"].ToString());
                    FichierDest = System.IO.Path.Combine(pathDest, Num_Appel + ".wav");

                    File.Copy(FichierSource, FichierDest, false);                                    
                }

                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }               
            }
        }
                      

        //Nottoyage des fichiers audio sur le serveur Nginx lorsqu'il ne sont plus dans une visite en cours
        public static void NettoyeFichierAudio(DataTable dtListeFichiersAudio)
        {
            string pathDest = ConfigurationManager.AppSettings["Path_Audio_Dest"].ToString();
            
            DirectoryInfo di = new DirectoryInfo(pathDest);
            FileInfo[] ListeFichiersAudio = di.GetFiles("*.wav");        //Liste des fichiers du répertoire
            
            if (ListeFichiersAudio.Length > 0)         //Si on a des fichiers dans le répertoire
            {
                foreach (var fi in ListeFichiersAudio)
                {
                    string NomFichier = fi.Name;     //Récup du nom court du fichier
                    int cpt = 0;

                    //On regarde s'il est dans la liste des visites en cours
                    for(int i = 0; i < dtListeFichiersAudio.Rows.Count; i++)
                    {
                        if(dtListeFichiersAudio.Rows[i]["Num_Appel"].ToString() + ".wav" == NomFichier)
                        {
                            cpt += 1;     //c'est le cas, on incrémente un compteur
                        }
                    }

                    //Si on ne la pas trouvé dans la liste des visites en cours 
                    if (cpt == 0)   
                    {
                        //On l'efface
                        try
                        {
                            File.Delete(fi.FullName);
                        }
                        catch (System.IO.IOException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }  //Fin foreach
            }      
        }

        #endregion


        #region Fonction de Messagerie
        public static string EcritMessage(string Message, string Expediteur, string CodeMedecin, string piecejointe)
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
                String SqlStr0 = "INSERT INTO messages (Message, Auteur, Destinataire, PieceJointe) VALUES('" + Message.Replace("'", "''") + "','" + Expediteur + "','" + CodeMedecin + "','" + piecejointe + "')";                
                cmd.CommandText = SqlStr0;

                //on execute les requettes                                       
                cmd.ExecuteNonQuery();

                retour = "OK";

                //On incremente le compteur de rafraichissement
                IncrementeRafraichissement();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de l'envoi du message. " + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                retour = "KO";
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }                            

            return retour;
        }


        //Chargement de la liste des messages
        public static DataTable ChargeListeMessages(int DureeAffichageMessages)
        {
            DataTable dtMessages = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {                
               /* string sqlstr0 = "SELECT m.*, a.NomAuteur AS NomAuteur, CASE m.Destinataire WHEN '-1' THEN 'Tous' ";
				sqlstr0 += " 						                                        WHEN '-2' THEN 'Régulation' ";
				sqlstr0 += " 						                                        ELSE d.NomDestinataire ";
				sqlstr0 += " 						                                        END AS NomDestinataire ";
                sqlstr0 += " FROM messages m INNER JOIN ";
                sqlstr0 += " 			        (SELECT  as Id, 'Régulation' AS NomAuteur FROM utilisateur ";  
  				sqlstr0 += " 			         UNION ";
  				sqlstr0 += " 		             SELECT CodeMedecin as Id, CONCAT(Prenom, ' ', Nom) AS NomAuteur FROM medecins ";
  				sqlstr0 += " 		             GROUP BY Id) AS a ON a.Id = m.Auteur ";
				sqlstr0 += "                 LEFT JOIN ";
                sqlstr0 += " 			        (SELECT '-2' as Id, 'Régulation' AS NomDestinataire FROM utilisateur ";
  				sqlstr0 += " 			         UNION ";
  				sqlstr0 += " 		             SELECT CodeMedecin as Id, CONCAT(Prenom, ' ', Nom) AS NomDestinataire FROM medecins ";
  				sqlstr0 += " 		             GROUP BY Id) AS d ON d.Id = m.Destinataire ";
                sqlstr0 += " WHERE m.DateM > DATE_ADD(NOW(), INTERVAL - @DureeAffichageMessages HOUR) ";
                sqlstr0 += " AND (m.Destinataire in ('-1', '-2') ";
                sqlstr0 += " OR m.Auteur in (SELECT IdUtilisateur FROM utilisateur)) ";
                sqlstr0 += " ORDER BY DateM DESC ";*/

                string sqlstr0 = " SELECT m.*, CASE a.NomAuteur WHEN '-2' THEN 'Régulation' ";
				sqlstr0 += "		                            ELSE a.NomAuteur ";
				sqlstr0 += "		                            END AS NomAuteur, ";
	            sqlstr0 += "                   CASE m.Destinataire WHEN '-1' THEN 'Tous' ";
			    sqlstr0 += "		                               WHEN '-2' THEN 'Régulation' ";
				sqlstr0 += "	                                   ELSE d.NomDestinataire ";
				sqlstr0 += "	                                   END AS NomDestinataire ";
                sqlstr0 += " FROM messages m LEFT JOIN (SELECT CodeMedecin as Id, CONCAT(Prenom, ' ', Nom) AS NomAuteur FROM medecins ";
			    sqlstr0 += "                           UNION  ";
                sqlstr0 += "                           SELECT '-2' as Id, 'Régulation' AS NomAuteur FROM medecins ";
  		        sqlstr0 += "                           GROUP BY Id) AS a ON a.Id = m.Auteur ";
                sqlstr0 += "                LEFT JOIN (SELECT CodeMedecin as Id, CONCAT(Prenom, ' ', Nom) AS NomDestinataire FROM medecins ";
  		        sqlstr0 += "   	                       UNION ";
                sqlstr0 += "   	                       SELECT '-2' as Id, 'Régulation' AS NomDestinataire FROM utilisateur ";
 		        sqlstr0 += "                           GROUP BY Id) AS d ON d.Id = m.Destinataire ";
                sqlstr0 += " WHERE m.DateM > DATE_ADD(NOW(), INTERVAL - @DureeAffichageMessages HOUR) ";               
                sqlstr0 += " AND (m.Auteur in (-1, -2) OR m.Destinataire in (-1, -2)) ";
                sqlstr0 += " ORDER BY DateM DESC ";

                cmd.CommandText = sqlstr0;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("DureeAffichageMessages", DureeAffichageMessages);

                dtMessages.Load(cmd.ExecuteReader());    //on execute
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors du chargement des messages :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtMessages;
        }


        //Recherche de messages
        public static DataTable RechercheMessages(DateTime DateDeb, DateTime DateFin)
        {
            DataTable dtMessages = new DataTable();

            string connex = ConfigurationManager.ConnectionStrings["Connection_Base_Regul"].ToString();
            MySqlConnection dbConnection = new MySqlConnection(connex);

            dbConnection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = dbConnection;

            try
            {                
                string sqlstr0 = "SELECT m.*, CASE m.Auteur WHEN '-2' THEN 'Régulation' ELSE concat(m2.Nom, ' ', m2.Prenom) END AS NomAuteur, ";
                sqlstr0 += "                  CASE m.Destinataire WHEN '-1' THEN 'Tous' WHEN '-2' THEN 'Régulation' ELSE concat(m3.Nom, ' ', m3.Prenom) END AS NomDestinataire ";
                sqlstr0 += " FROM messages m ";
                sqlstr0 += " LEFT JOIN medecins m2 ON m2.CodeMedecin = m.Auteur";
                sqlstr0 += " LEFT JOIN medecins m3 ON m3.CodeMedecin = m.Destinataire";
                sqlstr0 += " WHERE cast(m.DateM as Date) >= Date('" + convertDateMaria(DateDeb.ToString(), "MariaDb") + "')";
                sqlstr0 += " AND cast(m.DateM as Date) <= Date('" + convertDateMaria(DateFin.ToString(), "MariaDb") + "')";
                sqlstr0 += " AND (m.Destinataire in ('-1', '-2') ";
                sqlstr0 += " OR m.Auteur in ('-1', '-2')) ";
                sqlstr0 += " ORDER BY DateM DESC ";

                cmd.CommandText = sqlstr0;               

                dtMessages.Load(cmd.ExecuteReader());    //on execute
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erreur lors de la recherche des messages :" + e.Message, "Erreur", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                //fermeture des connexions
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }

            return dtMessages;
        }


        #endregion
    }

}


//A faire: 
//
//Remettre pour la publication ET changer le chemin de la base de donnée dans appconfig
//gestion de fiche d'appel qd validation par regul (désactivé pour ne pas en créer une pendant le DEV)=>  AttribueFicheAQ
