using System ;
using System.Linq;
using System.Security.Cryptography ;
using System.Text ;
using BotnetAPP.Shared ; 


namespace BotnetAPP.Network {

    /*
    D'abord le client envoie sa clé publique au serveur puis
    le serveur encrypt la clé symétrique qu'il envoie au client 
    c'est comme ça que le chiffrement s'opére

    Les raisons de ce fonctionnement : la clé du chiffrement assymétrique est trop longue
    */
    public class Encryption {

        private string _publickey ;


        private ASCIIEncoding _asen = new ASCIIEncoding();



        public Encryption() { }


        /**
        Symétrique encryption
        */


        #region symmetric_encryption
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



        #endregion


        #region asymmetric_encryption 
        public byte[] Encrypt(string message, Zombie zombie) {
            byte[] encryptedMessage = {} ; 
            
            try {

            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(8192);
            RSA.KeySize = 8192 ;


            RSA.FromXmlString(zombie.PublicKey) ;

            Console.WriteLine("La taille de la clé est de " + RSA.KeySize) ; 

            Console.WriteLine("le nb de byte est "+_asen.GetBytes(message).Count());

            Console.WriteLine(message) ;

            encryptedMessage = RSA.Encrypt(_asen.GetBytes(message), false ) ;

            } catch (Exception e) {

              //  Console.WriteLine("Longueur de la clé : " + RSA.KeySize);
                Console.WriteLine(message) ; 
                Console.WriteLine(e.Message) ; 
            }
            return encryptedMessage  ; 
        }
        #endregion 

    }
}
