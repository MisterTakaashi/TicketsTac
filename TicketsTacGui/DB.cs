using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows;

namespace TicketsTacGui
{
    enum Environment { Prod = 0, Dev = 1, Local = 2 }
    class DBConfig
    {
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
                    Host = "10.0.10.10";
                    Pass = "Passw0rd";
                    Username = "Administrator";
                    break;

                case Environment.Local:
                    Host = "(localdb)\\MSSQLLocalDb";
                    break;
            }
        }

        public DBConfig(string host, string username, string password)
        {
            Host = host;
            Username = username;
            Pass = password;
        }
    }

    static class DB
    {
        //Done:
        //  Select(List, string)
        //  Select(string, string)
        //  SelectWhere(string, string, string)
        //  SelectWhere(List, List, string)
        //  Insert(List, List, string)
        //  Get(int, string)
        //  Insert<T>(T instance, string)
        //  Insert(List<string> fields, List<string> values, string table)
        //  Delete(int id, string table)
        //  Delete<T>(T instance, string table)
        //  DeleteWhere(string whereClause, string table)
        //  Update<T>(T instance, string table)
        //  Update(int id, List<string>fields, List<string> values, string table)
        
        static private SqlConnection _connection = null;
        static private DBConfig config = new DBConfig();
        static private void _connectToDb()
        {
            //_connection = new SqlConnection(@"Data Source=" + config.Host + ";Uid=" + config.Username + ";Pwd=" + config.Pass + ";");
            _connection = new SqlConnection("Data Source=" + config.Host + ";Integrated Security=True;Initial Catalog=TicketsTac;");
            try
            {
                _connection.Open();
                //MessageBox.Show("Connexion à la base de données: OK");
            }
            catch ( Exception e )
            {
                string str = "/!\\ La connexion à la base de données à échoué. Informations sur la connexion:\n\tHost: " + config.Host +
                "\n\tUser: " + config.Username +
                "\n\tPass: " + config.Pass +
                "\n\tConnection String: " + _connection.ConnectionString +
                "\n\tError: " + e.Message;

                MessageBox.Show(str);
            }
        }

        public static void testConnection(DBConfig conf)
        {
            config = conf;
            if (_connection != null) _connection.Close();

            _connectToDb();
            _connection.Close();
        }

        public static List<Dictionary<string, string>> Select(List<string> fields, string table)
        {
            if (_connection == null ) _connectToDb();
            
            SqlCommand cmd = new SqlCommand("SELECT " + string.Join(",", fields) + " FROM " + table, _connection);

            return getRequestResult(cmd);
        }

        public static List<Dictionary<string, string>> Select(string fields, string table)
        {
            return Select(new List<String>(fields.Split(',')), table);
        }

        public static List<Dictionary<string, string>> SelectWhere(string fields, string whereClause, string table)
        {
            if (_connection == null) _connectToDb();

            SqlCommand cmd = new SqlCommand("SELECT " + fields + " FROM " + table + " WHERE " + whereClause, _connection);

            return getRequestResult(cmd);
        }

        public static List<Dictionary<string, string>> SelectWhere(List<string> selectFields, List<string> fields, List<string> values, string table)
        {
            string whereClause = "";
            for ( int i = 0; i < selectFields.Count; i++ )
            {
                if (i == 0)
                    whereClause += fields[i];
                else
                    whereClause += " AND " + fields[i];

                int bufferInt;
                double bufferDouble;

                if (double.TryParse(values[i], out bufferDouble) || int.TryParse(values[i], out bufferInt)) whereClause += " = " + values[i];
                else whereClause += " LIKE '" + values[i] + "'";
            }

            return SelectWhere(string.Join(",", selectFields), whereClause, table);
        }

        public static int Insert(List<string> fields, List<string> values, string table)
        {
            if (_connection == null) _connectToDb();

            for ( int i = 0; i < values.Count; i++ )
            {
                int bufferInt;
                double bufferDouble;

                if (!int.TryParse(values[i], out bufferInt) && !double.TryParse(values[i], out bufferDouble)) values[i] = "'" + values[i] + "'";
            }

            string reqValues = string.Join(",", values.ToArray());
            string reqFields = string.Join(",", fields.ToArray());


            SqlCommand cmd = new SqlCommand("INSERT INTO " + table + " (" + reqFields + ") VALUES (" + reqValues + ")", _connection);
            int affectedRows = 0;
            Dictionary<string, string> insertedRecord = null;
            try
            {
                affectedRows = cmd.ExecuteNonQuery();
                string whereClause = "";
                for ( int i = 0; i < fields.Count; i++ )
                {
                    if (i == 0)
                        whereClause += fields[i];
                    else
                        whereClause += " AND " + fields[i];

                    if (values[i].StartsWith("'") && values[i].EndsWith("'")) whereClause += " LIKE " + values[i];
                    else whereClause += " = " + values[i];
                }
                insertedRecord = SelectWhere("*", whereClause, table)[0];
            }
            catch ( Exception e )
            {
                logRequestError(cmd, e);
                return 0;
            }

            if ( affectedRows != 0 && insertedRecord.ContainsKey("Id"))
            {
                return int.Parse(insertedRecord["Id"]);
            }
            else
            {
                throw new Exception("Insertion failed for the data you provided.");
            }
        }

        public static int Insert(string fields, string values, string table)
        {
            List<string> fieldsReq = new List<string>(fields.Split(','));
            List<string> valuesReq = new List<string>(values.Split(','));

            return DB.Insert(fieldsReq, valuesReq, table);
        }

        public static int Insert<T>(T instance, string table)
        {
            Dictionary<string, List<string>> properties = getObjectProperties<T>(instance);

            Insert(properties["fields"], properties["values"], table);

            return 0;
        }

        public static int Update(int id, List<string> fields, List<string> values, string table)
        {
            if (_connection == null) _connectToDb();

            string request = "UPDATE " + table + " SET ";
            for ( int i = 0; i < fields.Count; i++ )
            {
                int intBuffer;
                double doubleBuffer;

                if (int.TryParse(values[i], out intBuffer) || double.TryParse(values[i], out doubleBuffer))
                    request += (fields[i] + " = " + values[i]);
                else
                    request += (fields[i] + " = '" + values[i] + "'");
            }
            request += " WHERE id = " + id.ToString();

            SqlCommand cmd = new SqlCommand(request, _connection);
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

        public static int Update<T>(T instance, string table)
        {
            if (_connection == null) _connectToDb();

            Dictionary<string, List<string>> ret = getObjectProperties<T>(instance);

            int id = -1;
            for (int i = 0; i < ret["fields"].Count; i++)
                if (ret["fields"][i].ToLower() == "id")
                    id = int.Parse(ret["values"][i]);

            if (id == -1)
                throw new Exception("id fields does not seem to be set for the instance you want to use in update");

            return Update(id, table);
        }

        public static int Delete(int id, string table)
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

        public static int Delete<T>(T instance, string table)
        {
            int id = -1;
            Dictionary<string, List<string>> properties = getObjectProperties<T>(instance);
            for ( int i = 0; i < properties["fields"].Count; i++ )
            {
                if (properties["fields"][i].ToLower() == "id") id = int.Parse(properties["values"][i]);
            }

            //Si on a pas trouvé l'id, on envoie une exception pour éviter de Delete la table entière en passant une clause where invalide
            if (id == -1) throw new Exception("Instance given to Delete method seems to have no public 'id' property.");

            //Donc on arrive ici que si on a un id valide
            return Delete(id, table);
        }

        public static int DeleteWhere(string whereClause, string table)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM " + table + " WHERE " + whereClause, _connection);

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

        public static Dictionary<string, string> Get(int id, string table)
        {
            if (_connection == null) _connectToDb();
            List<Dictionary<string, string>> ret = SelectWhere("*", "id = " + id.ToString(), table);
            return ret[0];
        }

        public static void Migrate()
        {
            if (_connection == null) _connectToDb();
            Console.WriteLine("Début du DB.Migrate");
            List<string> files = new List<string>();
            files.Add(@"../../../SQL/dbo.Users.sql");
            files.Add(@"../../../SQL/dbo.Projets.sql");
            files.Add(@"../../../SQL/dbo.Tickets.sql");
            files.Add(@"../../../SQL/dbo.Ticket_comms.sql");
            files.Add(@"../../../SQL/dbo.Projet_managers.sql");
            files.Add(@"../../../SQL/dbo.Projet_operators.sql");
            files.Add(@"../../../SQL/dbo.Ticket_assignee.sql");

            SqlCommand cmd = null;
            for ( int i = 0 ; i < files.Count ; i++ )
            {
                Console.WriteLine("Lecture du fichier " + files[i]);
                cmd = new SqlCommand(System.IO.File.ReadAllText(files[i]), _connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch ( Exception e)
                {
                    MessageBox.Show("L'appel au script " + files[i] + " a déclenché une erreur.\n" + e.Message);
                }
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

                if (p.GetValue(instance) != null)
                {
                    Type valueType = p.GetValue(instance).GetType(); //NULLReferenceException
                    if (valueType == typeof(string))
                        values.Add("'" + p.GetValue(instance).ToString() + "'");
                    else if (valueType == typeof(Rank) || valueType == typeof(StateEnum))
                        values.Add(((int)p.GetValue(instance)).ToString());
                    else
                        values.Add(p.GetValue(instance).ToString());
                    //TODO : Etre sur de prendre toutes les énums en compte
                }
                else continue;
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
                Console.WriteLine("/!\\ Requête SQL: " + cmd.CommandText);
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

        public static int getTimestamp()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
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
