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
            SqlDataReader r = DB.Get(id, "users");
            User theUser = new User();
            while (r.Read())
            {
                theUser = new User((int)r["id"], (String)r["username"], (String)r["email"], (Rank)r["rank"]);
            }
            return theUser;
        }

        public static List<User> GetAll()
        {
            List<User> allUsers = new List<User>();
            SqlDataReader r = DB.Select("*", "users");
            while (r.Read()){
                allUsers.Add(new User((int)r["id"], (String)r["username"], (String)r["email"], (Rank)r["rank"]));
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
