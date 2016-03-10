using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsTac
{
    class DBConf
    {
        enum Environment { Prod = 0, Dev = 1 }
        private Environment _env = Environment.Dev;
        public String Host { get; set; }
        public String Username { get; set; }
        public String Pass { get; set; }
        public int Port { get; set; }
        
        public DBConf()
        {
            switch ( this._env )
            {
                case Environment.Dev:
                    Host = "192.168.50.4";
                    Pass = "#SAPassword!";
                    Username = "sa";
                    break;

                case Environment.Prod:
                    Host = "164.132.110.73";
                    Pass = "Bastille89";
                    Username = "remote";
                    break;
            }
        }
    }

    class DB
    {
        private SqlConnection _connection;
        public DB(DBConf conf)
        {
            _connection = new SqlConnection(@"DataSource=" + conf.Host + ";Initial Catalog=TicketsTac;User Id=" + conf.Username + ";Password:" + conf.Pass + ";");
        }

        public List<T> Get<T>(int id)
        {
            return new List<T>();
        }
    }
}
