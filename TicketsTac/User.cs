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

        public void setId(int id, int newId) {
            DB.Update(id, new List<string> { "id" }, new List<string> { newId.ToString() },"users");
        }

        public void setUsername(int id, String username)
        {
            DB.Update(id, new List<string> { "username" }, new List<string> { username }, "users");
        }

        public void setEmail(int id, String email)
        {
            DB.Update(id, new List<string> { "email" }, new List<string> { email }, "users");
        }

        public void setRank(int id, Rank rank)
        {
            DB.Update(id, new List<string> { "rank" }, new List<string> { rank.ToString() }, "users");
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
