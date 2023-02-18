using System ;
using System.Collections;
using System.Collections.Generic ;
using BotnetAPP.Network ;
using System.Threading ;
using BotnetAPP.Shared ;
using System.Net.Sockets ;


namespace BotnetAPP.Network {

    public class ConnectedBotList {
        private Dictionary<Zombie, Socket> _connectedBot ;


        public ConnectedBotList() {



            _connectedBot = new Dictionary<Zombie, Socket>(){} ;
        }


        public Dictionary<Zombie, Socket> Connected {
            get {
                    return _connectedBot ;
            }
        }


        public int Count {
            get {
                return _connectedBot.Count ; 
            }
        }


        public void Add(Zombie zombie, Socket socket) {

            Console.WriteLine("ADD !!!") ;

            lock(_connectedBot) {
                _connectedBot.Add(zombie, socket) ; 
            }


        }

        public void Remove(Zombie index) {
            Console.WriteLine("REMOVE !!!") ;
            lock(_connectedBot) {
                _connectedBot.Remove(index) ; 
            }

 
        }


        public void ChangeAction(Zombie index, TypeAction newAction, Socket socket) {
            Console.WriteLine("CHANGE ACTION !!!") ;
 
            lock (_connectedBot) {

                _connectedBot.Remove(index) ; 
            // On rajoute le nouveau
               index.SetAction(newAction) ; 
                _connectedBot[index] = socket ; 

            }

            Monitor.Exit(_connectedBot);
        }

    }
}