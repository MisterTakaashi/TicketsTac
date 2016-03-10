using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsTac
{
    class Projet
    {
        public int Id { get; private set; }
        public User Client { get; private set; }
        public String Nom { get; set; }
        public String Description { get; set; }
        public List<User> Managers = new List<User>();
        public List<User> Operationnels = new List<User>();

        public Projet()
        {

        }

        public Projet(int id, String nom, User manager, User client)
        {
            Id = id;
            Nom = nom;
            Managers.Add(manager);
            Client = client;
        }

        public Projet(SqlDataReader r)
        {
            Id=(int)r["id"];
            Nom=(String)r["nom"];
            Description=(String)r["description"];

            int IdClient = (int)r["idClient"];
            // TODO recup client
        }

        public int GetID()
        {
            return Id;
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

        public void AddManager(User manager)
        {
            //TODO attaque de la BDD
            DB.Insert("","", "managers");
            Managers.Add(manager);
        }

        public void AddOperationnel(User operationnel)
        {
            //TODO attaque de la BDD
            DB.Insert("", "", "operationels");
            Operationnels.Add(operationnel);
        }

        public void DeleteManager(int id)
        {
            foreach (User manager in Managers)
            {
                if (manager.Id == id)
                {
                    // TODO attaque de la BDD
                    Operationnels.Remove(manager);
                    break;
                }
            }
        }

        public void DeleteManager(User manager)
        {
            // TODO attaque de la BDD
            Managers.Remove(manager);
        }

        public void DeleteOperationnel(int id)
        {
            foreach(User operationnel in Operationnels)
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

        public List<Projet> GetAllProjetsFromBDD()
        {
            List<Projet> projets = new List<Projet>();
            // TODO attaque de la BDD
            String sql = "SELECT * FROM projets";
            return projets;
        }



        public Projet GetProjetFromBDD(int id)
        {
            //TODO attaque de la BDD
            String sql = "SELECT * FROM projets WHERE id =" + id;
            return new Projet();
        }
    }
}
