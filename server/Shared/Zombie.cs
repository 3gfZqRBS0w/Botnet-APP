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

        private Action _action {get; set;}
        private Order _ordre {get; set;}


        public String GetAction {
            get {
                return Enum.GetName(_action) ; 
            }
        }

        public Zombie() {}

        public Zombie(Action action ) {
             
        }
    }
}