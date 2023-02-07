using System ;
using System.Net ; 
using System.Net.Sockets ;
using System.Collections.Generic ;
using BotnetAPP.Shared; 


namespace BotnetAPP.Shared {
    public class Zombie {


        private string _name ;
        private TypeAction _action ;
        private Order _ordre ;


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

        public void SetAction(TypeAction ta) {
            _action = ta ; 
        }

        public Zombie() {
            _name = "UNKNOWN" ;
            _action = TypeAction.WAIT ; 
        }

        public Zombie(string name) {
            _name = name ; 
            _action = TypeAction.WAIT ;
        }
    }
}