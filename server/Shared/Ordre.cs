using System ;
using System.Collections.Generic ;
using System.Net ;
using System.Net.Sockets ;

namespace BotnetAPP.Shared {
    public class Order {
        private int _second {get; set;}
        private IPAddress _victimIP {get; set;}

        public Order() {}

        public Order(int second, string ip) {
            _second = second ;
            _victimIP = IPAddress.Parse(ip) ; 
        }
    }
}