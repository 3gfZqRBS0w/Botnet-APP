using System ;
using System.Net ; 
using System.Net.Sockets ;
using System.Collections.Generic ;
using BotnetAPP.Shared; 


namespace BotnetAPP.Shared {
    public class Zombie {

        public enum Action {
        WAIT,
        ATTACK,
        OFFLINE,
        STOP, 
    }


        private string _name {get; set; } 
        private Action _action {get; set;}
        private Order _ordre {get; set;}


        public String GetAction {
            get {
                return Enum.GetName(_action) ; 
            }
        }

        public String GetName {
            get {
                return _name ; 
            }
        }
        public Zombie() {
            _name = "UNKNOWN" ;
            _action = Action.WAIT ; 
        }

        public Zombie(string name) {
            _name = name ; 
            _action = Action.WAIT ;
        }
    }
}