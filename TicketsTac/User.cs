using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsTac
{
    class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Rank Rank { get; set; }

        private string _password;

        public User(int id, string username, string email, Rank rank)
        {
            Id = id;
            Username = username;
            Email = email;
            Rank = rank;
        }

        public static User Get()
        {


            return null;
        }

        public static List<User> GetAll()
        {


            return null;
        }

        public static User Connect(string email, string password)
        {
            

            return null;
        }
    }

    enum Rank
    {
        Administrateur = 90,
        Manager = 80,
        Operator = 70,
        Client = 60
    }
}
