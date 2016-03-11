using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
        
        public DBConfig()
        {
            switch ( _env )
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

                case Environment.Local:
                    Host = "(localdb)\\MSSQLLocalDb";
                    break;
            }
        }
    }

    static class DB
    {
        //Done:
        //  Select(List, string)
        //  Select(string, string)
        //  SelectWhere(string, string, string)
        //  Insert(List, List, string)
        //  Get(int, string)
        //  Insert<T>(T instance, string)

        //TODO:
        //  SelectWhere(List, List, string)
        //  SelectWhere(string, List, string)
        //  SelectWhere(List, string, string)
        //  Delete(int id, string table)
        //  DeleteWhere(string whereClause, string table)
        //  DeleteWhere(List whereClauses, string table)
        //  Update<T>(T instance, string table)
        
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
            
            SqlCommand cmd = new SqlCommand("SELECT " + string.Join(",", fields) + " FROM " + table, _connection);

            return getRequestResult(cmd);
        }

        static public List<Dictionary<string, string>> Select(string fields, string table)
        {
            return Select(new List<String>(fields.Split(',')), table);
        }

        static public List<Dictionary<string, string>> SelectWhere(string fields, string whereClause, string table)
        {
            if (_connection == null) _connectToDb();

            SqlCommand cmd = new SqlCommand("SELECT " + fields + " FROM " + table + " WHERE " + whereClause, _connection);

            return getRequestResult(cmd);
        }

        static public int Insert(List<string> fields, List<string> values, string table)
        {
            if (_connection == null) _connectToDb();
            string reqValues = string.Join(",", values.ToArray());
            string reqFields = string.Join(",", fields.ToArray());

            SqlCommand cmd = new SqlCommand("INSERT INTO " + table + " (" + reqFields + ") VALUES (" + reqValues + ")", _connection);
            try
            {
                int affectedRows = cmd.ExecuteNonQuery();
                return affectedRows;
            }
            catch ( Exception e )
            {
                logRequestError(cmd, e);
                return 0;
            }
        }

        static public int Insert<T>(T instance, string table)
        {
            Dictionary<string, List<string>> properties = getObjectProperties<T>(instance);

            Insert(properties["fields"], properties["values"], table);

            return 0;
        }

        static public int Delete(int id, string table)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM " + table + " WHERE id = " + id.ToString(), _connection);
            try
            {
                int affectedRows = cmd.ExecuteNonQuery();
                return affectedRows;
            }
            catch( Exception e )
            {
                logRequestError(cmd, e);
                return 0;
            }
        }

        static public Dictionary<string, string> Get(int id, string table)
        {
            if (_connection == null) _connectToDb();

            return SelectWhere("*", "id =" + id.ToString(), table)[0];
        }

        static public void Migrate()
        {
            if (_connection == null) _connectToDb();
            List<string> files = new List<string>();
            files.Add(@"../../../SQL/dbo.Users.sql");
            files.Add(@"../../../SQL/dbo.Projets.sql");
            files.Add(@"../../../SQL/dbo.Tickets.sql");
            files.Add(@"../../../SQL/dbo.Ticket_comms.sql");
            files.Add(@"../../../SQL/dbo.Projet_managers.sql");
            files.Add(@"../../../SQL/dbo.Projet_operators.sql");

            SqlCommand cmd = null;
            for ( int i = 0 ; i < files.Count ; i++ )
            {
                cmd = new SqlCommand(System.IO.File.ReadAllText(files[i]), _connection);
                cmd.ExecuteNonQuery();
            }
        }

        private static Dictionary<string, List<string>> getObjectProperties<T>(T instance)
        {
            Dictionary<string, List<string>> ret = new Dictionary<string, List<string>>();

            List<string> fields = new List<string>();
            List<string> values = new List<string>();

            foreach (PropertyInfo p in typeof(T).GetProperties())
            {
                if (p.Name.ToLower() == "id") continue;

                fields.Add(p.Name);

                Type valueType = p.GetValue(instance).GetType();

                //TODO : Etre sur de prendre toutes les énums en compte
                if (valueType == typeof(string))
                    values.Add("'" + p.GetValue(instance).ToString() + "'");
                else if (valueType == typeof(Rank) || valueType == typeof(StateEnum))
                    values.Add(((int)p.GetValue(instance)).ToString());
                else
                    values.Add(p.GetValue(instance).ToString());
            }

            ret.Add("fields", fields);
            ret.Add("values", values);

            return ret;
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

            r.Close();

            //Et maintenant que la liste est prête on la renvoie
            return ret;
        }

        private static List<Dictionary<string, string>> getRequestResult(SqlCommand cmd)
        {
            try
            {
                SqlDataReader r = cmd.ExecuteReader();
                List<Dictionary<string, string>> ret = r.ToList();

                r.Close();

                return ret;
            }
            catch (Exception e)
            {
                Console.WriteLine("/!\\ Erreur: Une requête n'a pas abouti.");
                Console.WriteLine("\tTexte: " + cmd.CommandText);
                Console.WriteLine("\tErreur: " + e.Message);
                Console.ReadLine();

                return null;
            }
        }

        private static void logRequestError(SqlCommand cmd, Exception e)
        {
            Console.WriteLine("/!\\ Erreur: La requête n'a pas abouti");
            Console.WriteLine("\tTexte: " + cmd.CommandText);
            Console.WriteLine("\tErreur: " + e.Data);
            Console.WriteLine("\tMessage d'erreur: " + e.Message);
        }
    }
}
