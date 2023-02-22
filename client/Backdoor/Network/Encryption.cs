using System ;
using System.Text ; 
using System.Security.Cryptography ;
using LegitimeAPP.Shared ; 

namespace LegitimeAPP.Backdoor {


    /*
    */
    public class Encryption {


        private RSACryptoServiceProvider _rsa ;

        private ASCIIEncoding enc = new ASCIIEncoding() ;
        public Encryption() {

            // Crée la pair des clés 
            _rsa = new RSACryptoServiceProvider(8192) ;
            _rsa.KeySize = 8192 ; 
            // Soit 8192 bit soit 1024 bytes soit  128 caractères  car 1 caractères est codé sur un octet
            

        }

        public string GetPublicKey {
            get {
                return _rsa.ToXmlString(false) ;  
            }
        }

        public string Decrypt( string mess ) {

           // Encoding.UTF8.GetString(EncryptedBytes).
        //byte[] encryptedBytes = Convert.FromBase64String(mess);
        byte[] decryptedBytes = _rsa.Decrypt(enc.GetBytes(mess), false);
        string decryptedText = enc.GetString(decryptedBytes);



        Console.WriteLine("Message déchiffré  :"+decryptedText) ; 

        return decryptedText;
        }
    }
}

