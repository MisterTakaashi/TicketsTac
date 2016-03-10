using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsTac
{
    class Projet
    {
        public int Id { get; private set; }
        public User Client { get; set; }
        public List<User> Managers = new List<User>();
        public List<User> Operationnels = new List<User>();
        
        public Projet(int id, User manager, User client)
        {
            Id = id;
            Managers.Add(manager);
            Client = client;
        }

        public User getManagers()
        {
            return null;
        }

        public User getClient()
        {
            return null;
        }

        public User getOperationnels()
        {
            return null;
        }

        public int getID()
        {
            return Id;
        }

        public void addOperationnel(User operationnel)
        {
            Operationnels.Add(operationnel);
        }

        public void deleteOperationnel(int id)
        {
            foreach(User opperationnel in Operationnels)
            {
                if (opperationnel.Id == id)
                {
                    Operationnels.Remove(opperationnel);
                }
            }
        }
    }
}
