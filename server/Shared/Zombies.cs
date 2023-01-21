using System ;
using System.Collections.Generic ; 

namespace BotnetAPP.Shared {
    public class Zombies {
        private List<Zombie> listOfZombie ;

        public Zombies() {
            listOfZombie = new List<Zombie>() ; 
        }

        public List<Zombie> ListOfZombie {
            get {
                return listOfZombie ; 
            }
        }

        public void AddZombie(Zombie zb) {
            listOfZombie.Add(zb) ; 
        } 
    }
}