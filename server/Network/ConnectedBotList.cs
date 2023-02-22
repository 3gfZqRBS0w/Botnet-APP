using System ;
using System.Collections;
using System.Collections.Generic ;
using BotnetAPP.Network ;
using System.Threading ;
using BotnetAPP.Shared ;
using System.Net.Sockets ;


namespace BotnetAPP.Network {

/*
Cette classe permet de gérer les accès au dictionnaire contenant les
Bot connecté au serveur.
*/
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


            lock(_connectedBot) {
                _connectedBot.Add(zombie, socket) ; 
            }


        }

        public void Remove(Zombie index) {

            lock(_connectedBot) {
                _connectedBot.Remove(index) ; 
            }

 
        }


        public void ChangeAction(Zombie index, TypeAction newAction, Socket socket) {
 
            lock (_connectedBot) {

                _connectedBot.Remove(index) ; 
            // On rajoute le nouveau
               index.SetAction(newAction) ; 
                _connectedBot[index] = socket ; 

            }
        }

    }
}