using System ;
using System.Net ; 
using System.Net.Sockets ;
using System.Collections.Generic ;
using BotnetAPP.Shared; 


namespace BotnetAPP.Shared {

    public enum Action {
        ISWAITING,
        ISATTACKING,
        ISOFFLINE,
    }


    public class Zombie {
        private IPAddress _ip {get; set;}
        private Action _action {get; set;}
        private Order _ordre {get; set;}

        public String IP {
            get {
                return _ip.ToString() ; 
            }
        }

        public String Action {
            get {
                return Enum.GetName(_action) ; 
            }
        }

        public Zombie() {}

        public Zombie(string ip, Action action ) {
            _ip = IPAddress.Parse(ip) ;
            _action = action ; 
        }
    }
}