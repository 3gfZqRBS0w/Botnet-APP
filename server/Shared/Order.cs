using System ;
using System.Collections.Generic ;
using System.Net ;
using System.Net.Sockets ;

using System.Xml;
using System.Xml.Serialization;


/*

SERVERSIDE ORDERCLASS 
<?xml version="1.0" encoding="utf-16"?><Order xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><type_action>ATTACK</type_action><port>0</port><nbSecond>0</nbSecond><speed>0</speed></Order><?xml version="1.0" encoding="utf-16"?><Order xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><type_action>WAIT</type_action><port>0</port><nbSecond>0</nbSecond><speed>0</speed></Order>

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