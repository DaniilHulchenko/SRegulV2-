using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using MimeKit;


namespace SRegulV2
{    
    public class RecupMailImap : IDisposable
    {
        readonly string host, username, password;
        readonly SecureSocketOptions sslOptions;
        readonly int port;
        List<IMessageSummary> messages;
        CancellationTokenSource cancel;
        CancellationTokenSource done;
        FetchRequest request;
        bool messagesArrived;
        ImapClient client;

        public RecupMailImap(string host, int port, SecureSocketOptions sslOptions, string username, string password)
        {
            this.client = new ImapClient();
            this.request = new FetchRequest(MessageSummaryItems.Full | MessageSummaryItems.UniqueId);
            this.messages = new List<IMessageSummary>();
            this.cancel = new CancellationTokenSource();
            this.sslOptions = sslOptions;
            this.username = username;
            this.password = password;
            this.host = host;
            this.port = port;
        }


        //Relève les courrier grâce à l'imap
        public async void StartSurveilleImap()
        {
            using (var client = new RecupMailImap(host, port, sslOptions, username, password))
            {
                await client.RunAsync();
            }              
        }


        //Reconnection (si on s'est déconnecté)
        private async Task ReconnectAsync()
        {
            if (!client.IsConnected)
                await client.ConnectAsync(host, port, sslOptions, cancel.Token);

            if (!client.IsAuthenticated)
            {
                await client.AuthenticateAsync(username, password, cancel.Token);
              
                await client.Inbox.OpenAsync(FolderAccess.ReadWrite, cancel.Token);
            }
        }

        //On recherche les nvx messages (Ceux qu'on a pas dans notre cache)
        private async Task FetchMessageSummariesAsync(bool print)
        {
            IList<IMessageSummary> recherche = null;

            do
            {
                try
                {
                    //On récupere les informations sommaires pour les messages que nous ne possédons pas encore
                    int startIndex = messages.Count;

                    recherche = client.Inbox.Fetch(startIndex, -1, request, cancel.Token);
                    break;
                }
                catch (ImapProtocolException)
                {
                    //Les exceptions du protocole entraînent souvent la déconnexion du client donc on reconnect
                    await ReconnectAsync();
                }
                catch (IOException)
                {
                    //Les exceptions d'E/S entraînent toujours la déconnexion du client
                    await ReconnectAsync();
                }
            } while (true);

            
            //Pour chaque messages non lu...
            foreach (var message in recherche)
            {                
                bool MotTrouve = false;
                if (message.Envelope.Subject != null)
                {
                    string SujetEmail = message.Envelope.Subject.ToString();

                    string[] ListeMotsCle = { "new", "Nouveau", "Nvlle", "Nouvelle", "Nelle", "Nlle" };

                    //Si le sujet contient en début de phrase "Re:", alors on ne le traite pas
                    if (SujetEmail.StartsWith("Re:", StringComparison.OrdinalIgnoreCase))
                    {
                        //On ne le traite pas
                    }
                    else
                    {
                        //C'est pas une réponse...donc on balait la liste des mots pour voir si au moins 1 est dans le sujet du mail
                        foreach (string Mot in ListeMotsCle)
                        {
                            if (SujetEmail.IndexOf(Mot, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                MotTrouve = true;
                            }
                        }

                        //Si ça vient de mutuaide et que le message est non lu et qu'il contient au moins 1 des mots de la liste, on l'affiche
                        if (message.Envelope.From.ToString().Trim().Contains("@mutuaide.fr")
                             && message.Flags.Value != MessageFlags.Seen
                             && MotTrouve == true)
                        //if (message.Envelope.From.ToString().Trim().Contains("@free.fr") && message.Flags.Value != MessageFlags.Seen && MotTrouve == true)  //pour Test ******
                        {
                            //pour récupérer le corps du message
                            var bodyPart = message.TextBody;

                            var body = (TextPart)client.Inbox.GetBodyPart(message.UniqueId, bodyPart);   //Récup de la partie body 'text/plain'
                            var textMessage = body.Text;

                            //On affiche une boite par Email de Mutuaide en Non modale
                            FMailMutuaide fMailMutuaide = new FMailMutuaide();

                            //On passe les paramètres
                            fMailMutuaide.Email = message;
                            fMailMutuaide.BodyMessage = textMessage;

                            fMailMutuaide.Show();

                            //on met le flag à lu!
                            await client.Inbox.SetFlagsAsync(message.UniqueId, MessageFlags.Seen, true);

                            Console.WriteLine("{0}: new message: {1}", client.Inbox, message.Envelope.Subject);
                            //string uniqueId = message.UniqueId.Id.ToString();

                            messages.Add(message);     //Ajout du message à la liste des non lu
                        }
                    }
                }
            }         
        }

        private async Task WaitForNewMessagesAsync()
        {
            do
            {
                try
                {
                    if (client.Capabilities.HasFlag(ImapCapabilities.Idle))
                    {
                        //Note: Les serveurs IMAP ne sont censés interrompre la connexion qu'après 30 minutes.
                        // nous devrions rester inactifs pendant un maximum de, disons, ~29 minutes... mais GMail semble abandonner les connexions inactives après
                        //après environ 10 minutes, donc nous ne resterons en veille que 9 minutes.
                        done = new CancellationTokenSource(new TimeSpan(0, 9, 0));
                        try
                        {
                            await client.IdleAsync(done.Token, cancel.Token);
                        }
                        finally
                        {
                            done.Dispose();
                            done = null;
                        }
                    }
                    else
                    {
                        //Note : nous ne voulons pas spammer le serveur IMAP avec des commandes NOOP, donc attendons une minute entre chaque commande NOOP.
                        //entre chaque commande NOOP.

                        await Task.Delay(new TimeSpan(0, 1, 0), cancel.Token);
                        await client.NoOpAsync(cancel.Token);
                    }
                    break;
                }
                catch (ImapProtocolException)
                {
                    //les exceptions au protocole entraînent souvent la déconnexion du client
                    await ReconnectAsync();
                }
                catch (IOException)
                {
                    //Les exceptions d'E/S entraînent toujours la déconnexion du client
                    await ReconnectAsync();
                }
            } while (true);
        }

        private async Task IdleAsync()
        {
            do
            {
                try
                {
                    await WaitForNewMessagesAsync();

                    if (messagesArrived)
                    {
                        Console.WriteLine("nouveau message...");
                        await FetchMessageSummariesAsync(true);
                        messagesArrived = false;
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            } while (!cancel.IsCancellationRequested);
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Demarre la tache...");

            //On se connecte au serveur IMAP pour obtenir la liste initiale des messages
            try
            {
                await ReconnectAsync();
                await FetchMessageSummariesAsync(false);
            }
            catch (OperationCanceledException)
            {
                await client.DisconnectAsync(true);
                return;
            }

            //Note : Nous capturons client.Inbox ici parce que l'annulation de IdleAsync() *peut* nécessiter de          
            //déconnecter la connexion du client IMAP, et si c'est le cas, la propriété `client.Inbox`
            //ne sera plus accessible, ce qui signifie que nous ne pourrons pas déconnecter nos gestionnaires d'événements.
            var inbox = client.Inbox;

            //On suit l'évolution du nombre de messages dans le dossier (c'est ainsi que nous saurons si de nouveaux messages sont arrivés).
            inbox.CountChanged += OnCountChanged;

            //On garde une trace des messages qui sont expurgés afin que lorsque l'événement CountChanged se déclenche, nous puissions dire si c'est 
            //parce que de nouveaux messages sont arrivés ou parce que des messages ont été supprimés (ou une combinaison des deux).          
            inbox.MessageExpunged += OnMessageExpunged;

            //On suit les changements de drapeaux
            inbox.MessageFlagsChanged += OnMessageFlagsChanged;

            await IdleAsync();

            inbox.MessageFlagsChanged -= OnMessageFlagsChanged;
            inbox.MessageExpunged -= OnMessageExpunged;
            inbox.CountChanged -= OnCountChanged;

            await client.DisconnectAsync(true);
        }

        //l'événement CountChanged se déclenche lorsque de nouveaux messages arrivent dans le dossier et/ou lorsque des messages sont supprimés.
        private void OnCountChanged(object sender, EventArgs e)
        {
            var folder = (ImapFolder)sender;

            //Note : parce que nous suivons l'événement MessageExpunged et que nous mettons à jour notre liste des messages,
            // nous savons que si nous recevons un événement CountChanged et que folder.Count est 
            // plus grand que messages.Count, de nouveaux messages sont arrivés.           
            if (folder.Count > messages.Count)
            {
                int arrived = folder.Count - messages.Count;

                if (arrived > 1)
                    Console.WriteLine("\t{0} new messages have arrived.", arrived);
                else
                    Console.WriteLine("\t1 new message has arrived.");

                //Note : votre premier réflexe peut être de récupérer ces nouveaux messages maintenant,
                //mais vous ne pouvez pas le faire dans ce gestionnaire d'événements (ImapFolder n'est pas ré-entrant).              
                //
                //Au lieu de cela, annulez le jeton `done` et mettez à jour notre état afin de savoir que de nouveaux messages
                //sont arrivés. Nous récupérerons les résumés de ces nouveaux messages plus tard...
                messagesArrived = true;
                done?.Cancel();
            }
        }

        private void OnMessageExpunged(object sender, MessageEventArgs e)
        {
            var folder = (ImapFolder)sender;

            if (e.Index < messages.Count)
            {
                var message = messages[e.Index];

                Console.WriteLine("{0}: message #{1} has been expunged: {2}", folder, e.Index, message.Envelope.Subject);

                //Note : Si vous conservez un cache local d'informations sur les messages
                //(par exemple, les données MessageSummary) pour le dossier, vous devrez alors
                //supprimer le message à l'adresse e.Index.
                messages.RemoveAt(e.Index);
            }
            else
            {
                Console.WriteLine("{0}: message #{1} has been expunged.", folder, e.Index);
            }
        }

        private void OnMessageFlagsChanged(object sender, MessageFlagsChangedEventArgs e)
        {
            var folder = (ImapFolder)sender;

            Console.WriteLine("{0}: flags have changed for message #{1} ({2}).", folder, e.Index, e.Flags);
        }

        public void Exit()
        {
            cancel.Cancel();
        }

        public void Dispose()
        {
            client.Dispose();
            cancel.Dispose();
        }      
    }
}
