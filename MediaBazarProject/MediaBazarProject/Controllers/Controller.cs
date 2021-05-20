using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MediaBazarProject
{
    public abstract class Controller
    {
        //Fields:
        public MySqlConnection connection;
        private string connectionInfo;
        

        //Constructor:
        public Controller()
        {
            connectionInfo = "Server = studmysql01.fhict.local;" +
                                    "Database = dbi426537;" +
                                    "Uid = dbi426537;" +
                                    "Pwd = 1234; ";

            connection = new MySqlConnection(connectionInfo);
        }
    }
}
