using System ;
using System.Net ; 
using System.Net.Sockets ;
using System.Collections.Generic ;
using LegitimeAPP.Shared; 


namespace LegitimeAPP.Shared {
    public class Zombie {

        public enum TypeAction 
    {
        WAIT,
        ATTACK,
        OFFLINE,
        STOP, 
    }


        private string _name {get; set; } 
        public TypeAction Action {get; set;}
        private Order _ordre {get; set;}



        public Zombie() {
            _name = "UNKNOWN" ;
            Action = TypeAction.WAIT ; 
        }

        public Zombie(string name) {
            _name = name ; 
            Action = TypeAction.WAIT ;
        }
    }
}