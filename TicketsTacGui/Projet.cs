using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsTacGui
{
    class Projet
    {
        /*
        ** Propriétés d'Objet
        */

        public int Id { get; private set; }
        public User Client { get; private set; }
        public String Nom { get; private set; }
        public String Description { get; private set; }
        public List<User> Managers = new List<User>();
        public List<User> Operationnels = new List<User>();

        /*
        ** Constructeurs
        */

        // <summary>Constructeur de base</summary>
        public Projet(String nom, User manager, User client)
        {
            if (User.currentUser.hasPermissionTo(Permission.projectCreate, this))
            {
                List<String> fields = new List<String>();
                fields.Add("Name");
                fields.Add("Client_Id");
                fields.Add("Created");
                List<String> values = new List<String>();
                values.Add(nom);
                values.Add(Client.Id.ToString());
                values.Add(DB.getTimestamp().ToString());
                int id = DB.Insert(fields, values, "Projets");
                Nom = nom;
                Client = client;
                Id = id;
                Managers.Add(manager);
            }
            else
            {
                Console.WriteLine("insuficient permissions.");
            }
        }

        // <summary>Constructeur avec description</summary>
        public Projet(String nom, String description, User manager, User client)
        {
            if (User.currentUser.hasPermissionTo(Permission.projectCreate, this))
            {
                List<String> fields = new List<String>();
                fields.Add("Name");
                fields.Add("Description");
                fields.Add("Client_Id");
                fields.Add("Created");
                List<String> values = new List<String>();
                values.Add(nom);
                values.Add(description);
                values.Add(Client.Id.ToString());
                values.Add(DB.getTimestamp().ToString());
                int id = DB.Insert(fields, values, "Projets");
                Nom = nom;
                Description = description;
                Client = client;
                Id = id;
                Managers.Add(manager);
            }
            else
            {
                Console.WriteLine("Insuficient permissions");
            }
        }

        // <summary>Création d'un objet Projet depuis la base de données (après un DB.Select())</summary>
        private Projet(Dictionary<string, string> projet)
        {
            // Création de l'objet
            Id = int.Parse(projet["Id"]);
            Nom = projet["Name"];
            Description = projet["Description"];
            int IdClient = int.Parse(projet["Client_Id"]);
            // ajout du Client
            Client = new User(DB.Get(IdClient, "Users"));
            // Ajout des Managers

            List<Dictionary<String, String>> managersIds = DB.SelectWhere("*", "Projet_Id = " + this.Id, "Projet_managers");
            foreach (Dictionary<String, String> managerId in managersIds)
            {
                Dictionary<String, String> managersReal = DB.SelectWhere("*", "Id = " + managerId["Manager_Id"], "Users")[0];
                Managers.Add(new User(managersReal));
            }

            /*List<Dictionary<String, String>> managers = DB.SelectWhere("Users.Id, Users.Username, Users.Email, Users.Password, Users.Rank, Users.Created", "Projet_managers.Projet_Id = " + Id + ", Users.Id = Projet_manager.Manager_Id", "Projet_managers, Users");
            foreach (Dictionary<String, String> manager in managers)
            {
                Managers.Add(new User(manager));
            }*/
            // Ajout des opérateurs
            List<Dictionary<String, String>> operatersIds = DB.SelectWhere("*", "Projet_Id = " + this.Id, "Projet_operators");
            foreach (Dictionary<String, String> operaterId in operatersIds)
            {
                Dictionary<String, String> managersReal = DB.SelectWhere("*", "Id = " + operaterId["Operator_Id"], "Users")[0];
                Operationnels.Add(new User(managersReal));
            }

            /*List<Dictionary<String, String>> operateurs = DB.SelectWhere("Users.Id, Users.Username, Users.Email, Users.Password, Users.Rank, Users.Created", "Projet_operators.Projet_Id = " + Id + ", Users.Id = Projet_operators.Operator_Id", "Projet_operators, Users");
            foreach (Dictionary<String, String> operateur in operateurs)
            {
                Managers.Add(new User(operateur));
            }*/
        }

        /*
        ** Getters
        */

        public int GetID()
        {
                return Id;
        }

        public String GetIDToString()
        {
            return Id.ToString();
        }

        public User GetClient()
        {
            if (User.currentUser.hasPermissionTo(Permission.projectView, this))
                return Client;
            else
                Console.WriteLine("Insuficient permissions");
                return null;
        }

        public List<User> GetManagers()
        {
            if (User.currentUser.hasPermissionTo(Permission.projectView, this))
                return Managers;
            else
                return null;
        }

        public List<User> GetOperationnels()
        {
            if (User.currentUser.hasPermissionTo(Permission.projectViewAffected, this))
                return Operationnels;
            else
                return null;
        }

        public String GetNom()
        {
            return Nom;
        }

        public String getDescription()
        {
            return Description;
        }

        /*
        ** Setters
        */

        private void SetClient(User client)
        {
            if (User.currentUser.hasPermissionTo(Permission.projectUpdate, this))
            {
                List<String> fields = new List<String>();
                fields.Add("Client_Id");
                List<String> values = new List<String>();
                values.Add(client.Id.ToString());
                DB.Update(this.GetID(), fields, values, "Projets");
                Client = client;
            }
            else
            {
                Console.WriteLine("Insuficient permissions");
            }
        }

        public void SetNom(String nom)
        {
            if (User.currentUser.hasPermissionTo(Permission.projectUpdate, this))
            {
                List<String> fields = new List<String>();
                fields.Add("Nom");
                List<String> values = new List<String>();
                values.Add(GetNom());
                DB.Update(this.GetID(), fields, values, "Projets");
                Nom = nom;
            }
            else
            {
                Console.WriteLine("Insuficient permissions");
            }
        }

        public void SetDescription(String description)
        {
            if (User.currentUser.hasPermissionTo(Permission.projectUpdate, this))
            {
                List<String> fields = new List<String>();
                fields.Add("Description");
                List<String> values = new List<String>();
                values.Add(getDescription());
                DB.Update(this.GetID(), fields, values, "Projets");
                Description = description;
            }
            else
            {
                Console.WriteLine("Insuficient permissions");
            }
        }

        /*
        ** Fonctions et Méthodes d'Objet
        */

        public void AddManager(User manager)
        {
            if (User.currentUser.hasPermissionTo(Permission.projectUpdate, this))
            {
                List<String> fields = new List<String>();
                fields.Add("Manager_Id");
                fields.Add("Projet_Id");
                List<String> values = new List<String>();
                values.Add(manager.Id.ToString());
                values.Add(this.GetIDToString());
                DB.Insert(fields, values, "Projet_managers");
                Managers.Add(manager);
            }
            else
            {
                Console.WriteLine("Insuficient permissions");
            }
        }

        public void AddOperationnel(User operationnel)
        {
            if (User.currentUser.hasPermissionTo(Permission.projectUpdate, this))
            {
                List<String> fields = new List<String>();
                fields.Add("Operator_Id");
                fields.Add("Projet_Id");
                List<String> values = new List<String>();
                values.Add(operationnel.Id.ToString());
                values.Add(this.GetIDToString());
                DB.Insert(fields, values, "Projet_operators");
                Operationnels.Add(operationnel);
            }
            else
            {
                Console.WriteLine("Insuficient permissions");
            }
        }

        public void DeleteManager(int idManager)
        {
            if (User.currentUser.hasPermissionTo(Permission.projectUpdate, this))
            {
                foreach (User manager in Managers)
                {
                    if (manager.Id == idManager)
                    {
                        DB.DeleteWhere("Manager_Id = " + idManager + " AND Projet_Id " + Id, "Projet_managers");
                        Operationnels.Remove(manager);
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Insuficient permissions");
            }
        }

        public void DeleteManager(User manager)
        {
            if (User.currentUser.hasPermissionTo(Permission.projectUpdate, this))
            {
                DB.DeleteWhere("Manager_Id = " + manager.Id + " AND Projet_Id = " + Id, "Projet_managers");
                Managers.Remove(manager);
            }
            else
            {
                Console.WriteLine("Insuficient permissions");
            }
        }

        public void DeleteOperationnel(int idOperationnel)
        {
            if (User.currentUser.hasPermissionTo(Permission.projectUpdate, this))
            {
                foreach (User operationnel in Operationnels)
                {
                    if (operationnel.Id == idOperationnel)
                    {
                        DB.DeleteWhere("Operator_Id = " + idOperationnel + " AND Projet_Id " + Id, "Projet_operators");
                        Operationnels.Remove(operationnel);
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Insuficient permissions");
            }
        }

        public void DeleteOperationnel(User operationnel)
        {
            if (User.currentUser.hasPermissionTo(Permission.projectUpdate, this))
            {
                DB.DeleteWhere("Operator_Id = " + operationnel.Id.ToString() + " AND Projet_Id " + Id, "Projet_operators");
                Operationnels.Remove(operationnel);
            }
            else
            {
                Console.WriteLine("Insuficient permissions");
            }
        }

        /*
        // Fonctions et méthodes de classe
        */


        public static List<Projet> GetAllProjetsFromBDD()
        {
            List<Projet> projets = new List<Projet>();
            foreach (Dictionary<string, string> projet in DB.Select("*", "Projets"))
            {
                if (User.currentUser.hasPermissionTo(Permission.projectView, projet))
                    projets.Add(new Projet(projet));
                else
                    Console.WriteLine("Insuficient permissions to see this project. Next Project...");
            }
            return projets;
        }

        public static Projet GetProjetFromBDD(int id)
        {
            Dictionary<string, string> projet = DB.Get(id, "Projets");
            return new Projet(projet);
        }
    }
}
