﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TicketsTacGui
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

        static private void _connectToDb()
        {
            DBConfig config = new DBConfig();
            // _connection = new SqlConnection(@"Data Source=" + config.Host + "," + config.Port.ToString() + ";Uid=" + config.Username + ";Pwd=" + config.Pass + ";");
            _connection = new SqlConnection("Data Source=" + config.Host + ";Integrated Security=True;Initial Catalog=TicketsTac;");
            try
            {
                _connection.Open();
                Logger.Info("Connexion à la base de données: OK");
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

        public static List<Dictionary<string, string>> SelectWhere(List<string> fields, List<string> values, string table)
        {
            return SelectWhere(string.Join(",", fields), string.Join(",", fields), table);
        }

        public static int Insert(List<string> fields, List<string> values, string table)
        {
            if (_connection == null) _connectToDb();
            string reqValues = string.Join(",", values.ToArray());
            string reqFields = string.Join(",", fields.ToArray());

            Console.WriteLine("INSERT INTO " + table + " (" + reqFields + ") VALUES (" + reqValues + ")");

            SqlCommand cmd = new SqlCommand("INSERT INTO " + table + " (" + reqFields + ") VALUES (" + reqValues + ")", _connection);
            int affectedRows = 0;
            Dictionary<string, string> insertedRecord = null;
            try
            {
                affectedRows = cmd.ExecuteNonQuery();
                insertedRecord = SelectWhere(reqFields, reqValues, table)[0];
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

        public static int Insert<T>(T instance, string table)
        {
            Dictionary<string, List<string>> properties = getObjectProperties<T>(instance);

            Insert(properties["fields"], properties["values"], table);

            return 0;
        }

        public static int Update(int id, List<string> fields, List<string> values, string table)
        {
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

            SqlCommand cmd = new SqlCommand(request);
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

            return SelectWhere("*", "id =" + id.ToString(), table)[0];
        }

        public static void Migrate()
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

        public static int getTimestamp()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
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