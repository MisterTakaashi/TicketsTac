using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TicketsTac
{
    class User
    {
        public int Id { get; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Rank Rank { get; set; }
        public Int64 Created { get; set; }

        public string Password { get; set; }

        public User(int id, string username, string email, Rank rank, Int64 created)
        {
            Id = id;
            Username = username;
            Email = email;
            Rank = rank;
            Created = created;
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
            List<Dictionary<string, string>> result = DB.SelectWhere("*", string.Format("Email = '{0}' AND Password = '{1}'", email, password), "users");

            foreach (var user in result)
            {
                return new User(int.Parse(user["Id"]), user["Username"], user["Email"], (Rank)int.Parse(user["Rank"]), Int64.Parse(user["Created"]));
            }

            return null;
        }

        public string getPasswordHash()
        {
            return this.Password.ToSHA1();
        }

        public bool hasPermissionTo(Permission perm)
        {
            return true;
        }
    }

    enum Rank
    {
        Administrateur = 90,
        Manager = 80,
        Operator = 70,
        Client = 60
    }

    enum Permission
    {
        projectCreate = 1,
        projectView = 2,
        projectViewAffected = 17,
        projectViewOwn = 21,
        projectUpdate = 3,
        projectDelete = 4,
        userManagerView = 5,
        userManagerViewOwnProject = 23,
        userManagerUpdate = 6,
        userManagerUpdateOwnProfile = 7,
        userOperatorView = 8,
        userOperatorUpdate = 19,
        userOperatorUpdateOwnProfile = 18,
        userClientView = 22,
        userClientUpdateOwnProfile = 24,
        ticketViewOwnProjects = 9,
        ticketOperationnelViewOwnProjects = 25,
        ticketViewStateOpen = 20,
        ticketUpdate = 10,
        ticketUpdateStateToOpen = 11,
        ticketUpdateStateToResolve = 12,
        ticketUpdateStateToClosed = 13,
        ticketCreate = 14,
        ticketCreateForOwnProject = 15,
        ticketDelete = 16,
        ticketComment = 26,
        ticketCommentOwnProject = 27,
        ticketValidate = 28,
        ticketValidateOwnProject = 29,
    }
}
