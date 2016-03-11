using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace TicketsTac
{
    class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public Rank Rank { get; private set; }

        private string _password;

        public User(int id, string username, string email, Rank rank)
        {
            Id = id;
            Username = username;
            Email = email;
            Rank = rank;
        }

        public User() { }

        public void setId(int id) {
            //code
        }

        public void setUsername(String username)
        {
            //code
        }

        public void setEmail(String email)
        {
            //code
        }

        public void setRank(Rank rank)
        {
            //code
        }

        public static User Get(int id)
        {
            Dictionary<string, string> r = DB.Get(id, "users");
            User theUser = new User();
            theUser = new User(Int32.Parse(r["id"]), (String)r["username"], (String)r["email"], new Rank(["rank"]);
            return theUser;
        }

        public static List<User> GetAll()
        {
            List<User> allUsers = new List<User>();
            List<Dictionary<string, string>> r = DB.Select("*", "users");
            foreach (Dictionary<string, string> aUser in r)
            {
                allUsers.Add(new User(Int32.Parse(aUser["id"]), (String)aUser["username"], (String)aUser["email"], (Rank) Int32.Parse(aUser["rank"])));
            }
            return allUsers;
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
