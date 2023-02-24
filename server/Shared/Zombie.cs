using System ;
using System.Security.Cryptography ; 


namespace BotnetAPP.Shared {
    public class Zombie {


        private string _name ;
        private TypeAction _action ;
        private Order _ordre ;

        private readonly string _publickey ;
        private readonly string _symKey ; 


        // GETTERS !!

        public String PublicKey {
            get {

                Console.WriteLine("CLE PUBLIQUE COTE SERVEUR") ; 
                Console.WriteLine(_publickey) ; 
                return _publickey ; 
            }
        }

        public String SharedSymKey {
            get {
                return _symKey ; 
            }
        }


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

        public Zombie(string name, string publickey, string symKey) {
            _name = name ;
            _publickey = publickey ;
            _symKey = symKey ;
        }
    }
}