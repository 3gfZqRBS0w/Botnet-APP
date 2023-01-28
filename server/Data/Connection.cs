using System ;
using System.Net;
using System.Net.Sockets;
using System.Data ;
using Microsoft.Data.Sqlite;


namespace BotnetAPP.Data {
    public class Connection {

        private SqliteConnection _connection ;  
        public Connection() {
            using ( _connection = new SqliteConnection("Data Source=data.db"))
            {
               _connection.Open();
            }

            this.CreateDatabase() ; 
        }

        public void CreateDatabase() {
            if (_connection.State !=  ConnectionState.Open) {
                _connection.Open() ;
                }

            var command = _connection.CreateCommand();

            command.CommandText = $@"
            CREATE TABLE IF NOT EXISTS ZOMBIES (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                IP VARCHAR(15) NOT NULL
                );
            " ;

            command.ExecuteNonQuery();
        }

        public void AddZombie(string ip) {
            if (_connection.State != ConnectionState.Open) {
                _connection.Open() ; 
            }
            string req = $@"
            INSERT INTO ZOMBIES(IP) VALUES({ip}) 
            " ; 
        }
    }
}