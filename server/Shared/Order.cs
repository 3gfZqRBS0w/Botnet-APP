using System ;
using System.Collections.Generic ;
using System.Net ;
using System.Net.Sockets ;

using System.Xml;
using System.Xml.Serialization;

namespace BotnetAPP.Shared {
    public class Order {


        [XmlElement("port")]
        public int Port {get; set;}

        [XmlElement("victimIP")]
        public IPAddress VictimIP {get; set;}

        [XmlElement("nbSecond")]
        public int nbSecond ;
        
         
        public Order() {}

        public Order(int Port, string VictimIP, int nbSecond) {
            this.Port = Port ;
            this.VictimIP = IPAddress.Parse(VictimIP) ;
            this.nbSecond = nbSecond ; 
        }
    }
}