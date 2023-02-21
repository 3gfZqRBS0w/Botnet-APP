using System ;
using System.Security.Cryptography ;

namespace LegitimeAPP.Backdoor {


    /*
    */
    public class Encryption {


        private RSACryptoServiceProvider _rsa ;
        public Encryption() {

            // Soit 128 caractères 
            _rsa = new RSACryptoServiceProvider(1024) ;


        }

        public string GetPublicKey {
            get {
                return _rsa.ToXmlString(false) ;  
            }
        }

        public string Decrypt( string mess) {



            return "" ;
        }
    }
}

