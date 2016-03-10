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
        enum Environment { Prod = 0, Dev = 1, Local = 2 }
        private Environment _env = Environment.Local;
        public String Host { get; set; }
        public String Username { get; set; }
        public String Pass { get; set; }
        public int Port { get; set; }
        
        public DBConfig()
        {
            switch ( _env )
            {
                case Environment.Dev:
                    Host = "192.168.50.4";
                    Pass = "#SAPassword!";
                    Username = "sa";
                    Port = 1433;
                    break;

                case Environment.Prod:
                    Host = "164.132.110.73";
                    Pass = "Bastille89";
                    Username = "remote";
                    Port = 1433;
                    break;

                case Environment.Local:
                    Host = "(localdb)\\MSSQLLocalDb";
                    break;
            }
        }
    }

    static class DB
    {
        static private SqlConnection _connection = null;

        static private void _connectToDb()
        {
            DBConfig config = new DBConfig();
            // _connection = new SqlConnection(@"Data Source=" + config.Host + "," + config.Port.ToString() + ";Uid=" + config.Username + ";Pwd=" + config.Pass + ";");
            _connection = new SqlConnection("Data Source=" + config.Host + ";Integrated Security=True;Initial Catalog=TicketsTac;");
            try
            {
                _connection.Open();
                Console.WriteLine("Connexion à la base de données: ok");
            }
            catch ( Exception e )
            {
                Console.WriteLine("/!\\ La connexion à la base de données à échoué. Informations sur la connexion:");
                Console.WriteLine("\tHost: " + config.Host);
                Console.WriteLine("\tUser: " + config.Username);
                Console.WriteLine("\tPass: " + config.Pass);
                Console.WriteLine("\tConnection String: " + _connection.ConnectionString);
                Console.WriteLine("\tError: " + e.Message);
                Console.ReadLine();
                System.Environment.Exit(-1);
            }
        }

        static public List<Dictionary<string, string>> Select(List<string> fields, string table)
        {
            if (_connection == null ) _connectToDb();
            
            Console.WriteLine("Requête: SELECT " + string.Join(",", fields.ToArray()) + " FROM " + table);

            SqlCommand cmd = new SqlCommand("SELECT @fields FROM @table", _connection);
            cmd.Prepare();
            cmd.Parameters.Add(new SqlParameter("@fields", string.Join(",", fields.ToArray())));
            cmd.Parameters.Add(new SqlParameter("@table", table));

            try
            {
                SqlDataReader r = cmd.ExecuteReader();
                //Notre reader est prêt, on récupère les noms des colonnes
                List<string> columns = new List<string>();
                for (int i = 0; i < r.FieldCount ; i++)
                {
                    columns.Add(r.GetName(i));
                }

                //On a les noms des colonnes, maintenant on prépare le dico
                List<Dictionary<int, String>> ret = new List<Dictionary<int, string>>();
                while ( r.Read() )
                {

                }

                r.Close();
                return r.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("/!\\ Erreur: La requuête n'a pas abouti.");
                Console.WriteLine("\tTexte: " + cmd.CommandText);
                Console.WriteLine("\tErreur: " + e.Data);
                Console.ReadLine();

                return null;
            }

        }

        static public List<Dictionary<string, string>> Select(string fields, string table)
        {
            return Select(new List<String>(fields.Split(',')), table);
        }

        static public int Insert(List<string> fields, List<string> values, string table)
        {
            if (_connection == null) _connectToDb();

            SqlCommand cmd = new SqlCommand("INSERT INTO @table (@fields) VALUES (@values)", _connection);
            cmd.Prepare();
            cmd.Parameters.Add(new SqlParameter("@table", table));
            cmd.Parameters.Add(new SqlParameter("@fields", string.Join(",", fields.ToArray())));
            cmd.Parameters.Add(new SqlParameter("@values", string.Join(",", values.ToArray())));

            return cmd.ExecuteNonQuery();
        }

        static public void Migrate()
        {
            //if (_connection == null) _connectToDb();

            string text = System.IO.File.ReadAllText(@"..\..\..\ticketstac.sql");
        }

        static public List<Dictionary<string, string>> SelectWhere(string fields, string whereClause, string table)
        {
            if (_connection == null) _connectToDb();

            SqlCommand cmd = new SqlCommand("SELECT @fields FROM @table WHERE @where", _connection);
            cmd.Prepare();
            cmd.Parameters.Add(new SqlParameter("@fields", fields));
            cmd.Parameters.Add(new SqlParameter("@table", table));
            cmd.Parameters.Add(new SqlParameter("@where", whereClause));

            return cmd.ExecuteReader().ToList();
        }

        static public List<Dictionary<string, string>> Get(int id, string table)
        {
            if (_connection == null) _connectToDb();

            return SelectWhere("*", "id =" + id.ToString(), table);
        }

        public static List<Dictionary<string, string>> ToList(this SqlDataReader r)
        {
            //On prépare la liste des noms des colonnes
            List<string> cols = new List<string>();
            for ( int i = 0; i < r.FieldCount; i++ )
            {
                cols.Add(r.GetName(i));
            }

            //La liste que l'on va retourner
            List<Dictionary<string, string>> ret = new List<Dictionary<string, string>>();
            while ( r.Read() )
            {
                //Le dictionnaire à ajouter à la liste
                Dictionary<string, string> tmp = new Dictionary<string, string>();
                for ( int i = 0; i < cols.Count; i++ )
                {
                    string col = cols[i];
                    tmp.Add(col, r[col].ToString());
                }

                //On ajoute le dico à la liste
                ret.Add(tmp);
            }

            //Et maintenant que la liste est prête on la renvoie
            return ret;
        }
    }
}
