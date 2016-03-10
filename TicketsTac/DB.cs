using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsTac
{
    class DBConfig
    {
        enum Environment { Prod = 0, Dev = 1 }
        private Environment _env = Environment.Dev;
        public String Host { get; set; }
        public String Username { get; set; }
        public String Pass { get; set; }
        public int Port { get; set; }
        
        public DBConfig()
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

    static class DB
    {
        static private SqlConnection _connection;

        static private void _connectToDb()
        {
            DBConfig config = new DBConfig();
            _connection = new SqlConnection(@"DataSource=" + config.Host + ";Initial Catalog=TicketsTac;User Id=" + config.Username + ";Password:" + config.Pass + ";");
            try
            {
                _connection.Open();
            }
            catch ( Exception e )
            {
                Console.WriteLine("/!\\ La connexion à la base de données à échoué. Informations sur la connexion:");
                Console.WriteLine("\tHost:" + config.Host);
                Console.WriteLine("\tUser:" + config.Username);
                Console.WriteLine("\tPass:" + config.Pass);
                Console.WriteLine("\tError: " + e.Data);
            }
        }

        static public SqlDataReader Select(List<string> fields, string table)
        {
            if (_connection == null ) _connectToDb();
            
            Console.WriteLine("Requête: SELECT " + string.Join(",", fields.ToArray()) + " FROM " + table);

            SqlCommand cmd = new SqlCommand("SELECT @fields FROM @table", _connection);
            cmd.Parameters.Add(new SqlParameter("@fields", string.Join(",", fields.ToArray())));
            cmd.Parameters.Add(new SqlParameter("@table", table));

            SqlDataReader r = cmd.ExecuteReader();

            return r;
        }

        static public SqlDataReader Select(string fields, string table)
        {
            return Select(new List<string> { fields }, table);
        }

        static public int Insert(List<string> fields, List<string> values, string table)
        {
            if (_connection == null) _connectToDb();

            SqlCommand cmd = new SqlCommand("INSERT INTO @table (@fields) VALUES (@values)");
            cmd.Parameters.Add(new SqlParameter("@table", table));
            cmd.Parameters.Add(new SqlParameter("@fields", string.Join(",", fields.ToArray())));
            cmd.Parameters.Add(new SqlParameter("@values", string.Join(",", values.ToArray())));

            return cmd.ExecuteNonQuery();
        }
    }
}
