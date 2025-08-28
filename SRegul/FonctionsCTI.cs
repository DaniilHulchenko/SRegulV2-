using Microsoft.Win32;
using SRegulV2.XMLApiFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SRegulV2
{
    class FonctionsCTI
    {       
        public static string[] LoguePoste(string IdCTI, string Pass)
        {
            string[] retour = new string[2];
            retour[0] = "KO";
            
            string Token = "";
            string Ligne = "";

            //On se connecte au WebService du CTI            
            //On indique la validation automatique des demandes d'acceptation des certificats
            SSLValidator.OverrideValidation();  
                                            
            //Création de l'instance du webservice           
            XmlApiFrameworkService WsLogin = new XmlApiFrameworkService();
                                 
            try
            {
                Token = WsLogin.login(IdCTI, Pass);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (Token != "")
            {
                //On est passé
                retour[0] = Token;                
                Console.WriteLine("Utililateur logué");

                //On logue le poste
                XMLPhone.XmlPhoneService WsAppel = new XMLPhone.XmlPhoneService();

                //Pour intercepter la réponse du webservice
                XMLPhone.AlcLogonResult ReponseCall = new XMLPhone.AlcLogonResult();
                try
                {
                    ReponseCall = WsAppel.login(Token, "", null);

                    if (ReponseCall.status.code == 0)
                    {
                        Ligne = ReponseCall.lineNumber.ToString();

                        Console.WriteLine("Telephone logué");
                        retour[1] = Ligne;
                    }
                    else
                    {
                        Console.WriteLine("Telephone non logué..." + ReponseCall.status.label.ToString());
                        retour[0] = "KO";
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Utililateur non logué");
                retour[0] = "KO";
            }

            return retour;
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


        //Récupère les infos de l'appel en cours sur le poste
        //public static string[] RecupInfosAppel()
        public static string[] RecupInfosAppel(string Token, string Ligne)
        {                                   
            string[] retour = new string[4];
            retour[0] = "KO";
            
            if (Token != "")            
            {
                //On logue le poste
                XMLPhone.XmlPhoneService WsAppel = new XMLPhone.XmlPhoneService();

                //Pour intercepter la réponse du webservice
                XMLPhone.CallInfoResult ReponseCall = new XMLPhone.CallInfoResult();

                try
                {
                    ReponseCall = WsAppel.getCurrentCallInfo(Token, Ligne);

                    if (ReponseCall.status.code == 0)
                    {
                        //Recup des infos de l'appel
                        retour[0] = ReponseCall.callIsPresent.ToString();    //Si on a un appel en cours true/false
                        retour[1] = ReponseCall.name.ToString();             //Le nom de la personne

                        if (ReponseCall.callIsPresent == true && ReponseCall.number.ToString() != "")
                        {                            
                            if (ReponseCall.number.ToString().Substring(0,2) == "00")
                                retour[2] = ReponseCall.number.ToString().Substring(1, ReponseCall.number.ToString().Length - 1);  //Le N°, on enlève le 1er 0
                            else 
                                retour[2] = ReponseCall.number.ToString();
                        }
                        else retour[2] = ReponseCall.number.ToString();
                        
                        retour[3] = ReponseCall.state.ToString();            //Etat de la ligne (active)
                    }
                }
                catch (Exception e)
                {
                    if (e.Message == "AlcServiceException.BAD_FRAMEWORK_SESSION_IDENTIFIER")                    
                        Console.WriteLine("Faut reloguer");
                }
            }
            
            return retour;
        }


        //Appel un n° de Tel (Ajouter 0 devant pour sortir)
        public static string Appeler(string Numero, string Token, string Ligne)
        {
            string retour = "KO";

            if (Token != "" && Numero != "")
            {
                //On compose le n° de tel
                XMLPhone.XmlPhoneService WsAppel = new XMLPhone.XmlPhoneService();

                XMLPhone.MakeCallInvoke Appels = new XMLPhone.MakeCallInvoke();  //Pour entrer les parametres d'appels

                //On reformate le n°
                //Formatage du n° de Tel: Si c'est au format International
                if (Numero.IndexOf("+") != -1)
                {
                    //On le reformate en enlevant le + (pour le PABX)
                    Numero = "00" + Numero.Replace("+", "");                                                                           
                }

                Appels.sessionId = Ligne;                                
                Appels.callee = "0" + Numero;      //Pour le n° de Tel (On ajoute 0 pour sortir)

                //Pour intercepter la réponse du webservice
                XMLPhone.AlcStatus ReponseCall = new XMLPhone.AlcStatus();

                ReponseCall = WsAppel.makeCall(Token, Appels);

                if (ReponseCall.code == 0)
                {
                    Console.WriteLine("Appel en cour...");
                    retour = "OK";
                }
                else
                {
                    Console.WriteLine("Appel échoué..." + ReponseCall.label.ToString());                   
                }
            }

            return retour;
        }


        //Pour vérifier si l'on est connecté
        public static string toujoursConnecte(string Token, string Ligne)
        {
            string retour = "KO";           

            //Création de l'instance du webservice           
            XmlApiFrameworkService WsLogin = new XmlApiFrameworkService();
                              
            try
            {
                WsLogin.getUserInfo(Token);
                retour = "OK";
            }
            catch (Exception e)
            {                                
                if (e.Message == "AlcServiceException.BAD_FRAMEWORK_SESSION_IDENTIFIER")
                        Console.WriteLine("Faut reloguer");               
            }
           
            return retour;
        }



        //Pour se déconnecter...
        public static void deconnecte(string Token, string Ligne)
        {
            try
            {
                //on déconnecte le Telephone, puis on déconnecte l'utilisateur
                XMLApiFramework.XmlApiFrameworkService WsLogin = new XMLApiFramework.XmlApiFrameworkService();
                XMLPhone.XmlPhoneService WsAppel = new XMLPhone.XmlPhoneService();
             
                try
                {
                    WsAppel.logout(Token, Ligne);
                    WsLogin.logout(Token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Message " + ex.Message);
                }
                finally
                {
                    WsLogin.logout(Token);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Message " + e.Message);
            }
        }


        //Désactivation du proxy HIN qui empêche le fonctionnement des webservices du CTI
        public static string DesactiveProxyHIN()
        {             
            string retour = "KO";

            //Si l'utilisateur n'est pas qu'un invité on déactive le proxy
            if (int.Parse(Form1.Utilisateur[1]) > 1)
            {
                RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);

                //On désactive le proxy           
                //registry.SetValue("ProxyServer", 1);
                // registry.DeleteValue("ProxyServer");
                registry.SetValue("ProxyEnable", 0);

                //Proxy Status
                int proxyStatus = (int)registry.GetValue("ProxyEnable");
                if (proxyStatus == 0)
                    retour = "OK"; //Disabled
                if (proxyStatus == 1)
                    retour = "KO"; //Enabled
            }
            else
            {
                retour = "OK";
            }

            return retour;
        }

        //Réactivation du proxy HIN qui empêche le fonctionnement des webservices du CTI
        public static string ReactiveProxyHIN()
        {
            string retour = "KO";

            //Si l'utilisateur n'est pas qu'un invité on ré-active le proxy
            if (int.Parse(Form1.Utilisateur[1]) > 1)
            {
                RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);

                //On active le proxy           
                //registry.SetValue("ProxyServer", 1);           

                // registry.SetValue("ProxyServer", "http://localhost:5016");
                registry.SetValue("ProxyEnable", 1);

                //Proxy Status
                int proxyStatus = (int)registry.GetValue("ProxyEnable");
                if (proxyStatus == 1)
                    retour = "OK"; //Activé
                if (proxyStatus == 0)
                    retour = "KO"; //dé-sactivé
            }
            else
            {
                retour = "OK";
            }

            return retour;
        }

    }
}

//A faire 
