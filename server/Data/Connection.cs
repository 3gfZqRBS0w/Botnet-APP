using System ;
using System.Net;
using System.Net.Sockets;
using Microsoft.Data.Sqlite;


namespace BotnetAPP.Data {
    public class Connection {

        public SqliteConnection connection ;  
        public Connection() {
            using (connection = new SqliteConnection("Data Source=data.db"))
            {
                connection.Open();
            }
        }

        public void CreateDatabase() {
            string req = $@"
            CREATE TABLE IF NOT EXISTS ZOMBIES (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                IP VARCHAR(15) NOT NULL
                );
            " ;


        }

        public void AddZombie(string ip) {
            string req = $@"
            INSERT INTO ZOMBIES(IP) VALUES({ip}) 
            " ; 
        }
    }
}