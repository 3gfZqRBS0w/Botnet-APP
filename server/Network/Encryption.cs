using System ;
using System.Linq;
using System.Security.Cryptography ;
using System.Text ; 


namespace BotnetAPP.Network {

    /*
    Serverside 

    */  
    public class Encryption {

        private string _publickey ;


        private ASCIIEncoding _asen = new ASCIIEncoding();



        public Encryption() { }


        public void SetPublicKey(string key) {
            _publickey = key ; 
        }

        public string Encrypt(string message) {

            try {

            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();


            RSA.FromXmlString(_publickey) ;
            

            Console.WriteLine("le nb de byte est "+_asen.GetBytes(message).Count());

            RSA.Encrypt(_asen.GetBytes(message), false) ;

            } catch (Exception e) {

                Console.WriteLine(message) ; 
                Console.WriteLine(e.Message) ; 
            }




            
            return message ; 
        }

    }
}
