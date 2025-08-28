using System;
using System.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace SRegulV2
{
    class EmailService
    {
        public static string SendMail(string to, string subject, string html, string from, string typeDest)
        {
            string retour = "KO";

            //Création du message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));

            //Pour les mails en copie
            if (typeDest == "TA")
                email.To.Add(MailboxAddress.Parse("ckabbaj@sos-medecins.ch"));    //On rajoute chakib en copie
            else if (typeDest == "Infirmiere")
            {
                email.To.Add(MailboxAddress.Parse("info-regulation@sos-medecins.ch"));    //On met info regulation en copie                
            }
            else if (typeDest == "Mutuaide")
            {                
                email.To.Add(MailboxAddress.Parse("info-regulation@sos-medecins.ch"));    //On met info-regulation en copie
                email.To.Add(MailboxAddress.Parse("contact@swissmedicassistance.ch"));    //ainsi que contact@swissmedicassistance.ch
            }

            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            //Envoi de l'email
            SmtpClient smtp = new SmtpClient();
            try
            {

                if (typeDest == "TA")
                {
                    smtp.Connect(ConfigurationManager.AppSettings["SmtpHost"].ToString(), int.Parse(ConfigurationManager.AppSettings["SmtpPort"].ToString()), SecureSocketOptions.StartTls);
                    smtp.Authenticate(Form1.EmailTAFrom, Form1.PassMailTA);
                }
                else if (typeDest == "Infirmiere")
                {
                    smtp.Connect(ConfigurationManager.AppSettings["SmtpHost"].ToString(), int.Parse(ConfigurationManager.AppSettings["SmtpPort"].ToString()), SecureSocketOptions.StartTls);
                    smtp.Authenticate(Form1.EmailInfFrom, Form1.PassMailInf);
                }
                else if (typeDest == "Mutuaide")
                {
                    //Config pour ovh                   
                    smtp.Connect(ConfigurationManager.AppSettings["SmtpHostOVH"].ToString(), int.Parse(ConfigurationManager.AppSettings["SmtpPortOVH"].ToString()), SecureSocketOptions.StartTls);
                    smtp.Authenticate(Form1.EmailMutuaideFrom, Form1.PassMailMutuaide);
                }

                smtp.Send(email);
                smtp.Disconnect(true);
                retour = "OK";
            }
            catch (Exception e)
            {
                Console.WriteLine("Envoi de l'email, erreur: " + e.Message);
                return e.Message;
            }

            return retour;
        }
    }
}
