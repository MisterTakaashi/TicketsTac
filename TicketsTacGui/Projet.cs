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
            DB.Insert<Projet>(this, "projet");
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

        // <summary>Constructeur avec description</summary>
        public Projet(String nom, String description, User manager, User client)
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
            List<Dictionary<String, String>> managers = DB.SelectWhere("Users.Id, Users.Username, Users.Email, Users.Password, Users.Rank, Users.Created", "Projet_managers.Projet_Id = " + Id + ", Users.Id = Projet_manager.Manager_Id", "Projet_managers, Users");
            foreach (Dictionary<String, String> manager in managers)
            {
                Managers.Add(new User(manager));
            }
            // Ajout des opérateurs
            List<Dictionary<String, String>> operateurs = DB.SelectWhere("Users.Id, Users.Username, Users.Email, Users.Password, Users.Rank, Users.Created", "Projet_operators.Projet_Id = " + Id + ", Users.Id = Projet_operators.Operator_Id", "Projet_operators, Users");
            foreach (Dictionary<String, String> operateur in operateurs)
            {
                Managers.Add(new User(operateur));
            }
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
            return Client;
        }

        public List<User> GetManagers()
        {
            return Managers;
        }

        public List<User> GetOperationnels()
        {
            return Operationnels;
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

        public void SetId(int id)
        {
            Id = id;
            // TODO atq bdd
        }

        public void SetClient(User client)
        {
            Client = client;
            // TODO atq bdd
        }

        public void SetNom(String nom)
        {
            Nom = nom;
            // TODO atq bdd
        }

        public void SetDescription(String description)
        {
            Description = description;
            // TODO atq bdd
        }

        /*
        ** Fonctions et Méthodes d'Objet
        */

        public void AddManager(User manager)
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

        public void AddOperationnel(User operationnel)
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

        public void DeleteManager(int idManager)
        {
            foreach (User manager in Managers)
            {
                if (manager.Id == idManager)
                {
                    /*List<String> where = new List<String>();
                    where.Add("Manager_Id = " + idManager);
                    where.Add("Projet_Id = " + Id);*/
                    DB.DeleteWhere("Manager_Id = " + idManager + " AND Projet_Id " + Id, "Projet_operators");
                    Operationnels.Remove(manager);
                    break;
                }
            }
        }

        public void DeleteManager(User manager)
        {
            /*List<String> where = new List<String>();
            where.Add("Manager_Id = " + manager.Id);
            where.Add("Projet_Id = " + Id);*/
            DB.DeleteWhere("Manager_Id = " + manager.Id + " AND Projet_Id = " + Id, "Projet_operators");
            Managers.Remove(manager);
        }

        public void DeleteOperationnel(int id)
        {
            foreach (User operationnel in Operationnels)
            {
                if (operationnel.Id == id)
                {
                    // TODO attaque de la BDD
                    Operationnels.Remove(operationnel);
                    break;
                }
            }
        }

        public void DeleteOperationnel(User operationnel)
        {
            // TODO attaque de la BDD
            Operationnels.Remove(operationnel);
        }
        /*
        public void gestionProjet()
        {
            Console.WriteLine("voulez vous ajouter un nouveau ticket ?");
            Ticket ticket = new Ticket("description", this);
        }
        */

        /*
        ** Fonctions et Méthodes statiques
        */

        public static List<Projet> GetAllProjetsFromBDD()
        {
            List<Projet> projets = new List<Projet>();
            foreach (Dictionary<string, string> projet in DB.Select("*", "Projets"))
            {
                projets.Add(new Projet(projet));
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
