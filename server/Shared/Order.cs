using System ;
using System.Collections.Generic ;
using System.Net ;
using System.Net.Sockets ;

using System.Xml;
using System.Xml.Serialization;


/*

SERVERSIDE ORDERCLASS 

*/

namespace BotnetAPP.Shared {
    public class Order {

        [XmlElement("type_action")]
        public TypeAction action ;

        [XmlElement("port")]
        public int Port ;
        [XmlElement("victimIP")]
        public string VictimIP ;
        [XmlElement("nbSecond")]
        public int nbSecond ;

        [XmlElement("speed")]
        public int speed ; 

        public Order() {}
        public Order(TypeAction action) {
            this.action = action ;
        }

        public Order(int Port, string VictimIP, int nbSecond, int speed) {
            this.Port = Port ;
            this.VictimIP = VictimIP ;
            this.nbSecond = nbSecond ;
            this.speed = speed ; 
        }
    }
}