using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TicketsTacGui
{
    class User
    {
        public static User currentUser = new User(0, "Test", "Test", Rank.Administrateur, 0);

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

        public User(Dictionary<String, String> user)
        {
            Id = int.Parse(user["Id"]);
            Username = user["Username"];
            Email = user["Email"];
            Rank = (Rank)int.Parse(user["Rank"]);
            Created = Int64.Parse(user["Created"]);
        }

        public static User Get()
        {


            return null;
        }

        public static List<User> GetAll()
        {
            List<User> ret = new List<User>();
            List<Dictionary<string, string>> users = DB.Select("*", "Users");

            foreach (Dictionary<string, string> user in users)
            {
                ret.Add(new User(user));
            }

            return ret;
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

        public bool hasPermissionTo(Permission perm, object param)
        {
            switch (perm)
            {
                case Permission.projectCreate:
                    switch (this.Rank)
                    {
                        case Rank.Administrateur:
                            return true;
                        case Rank.Manager:
                            return false;
                        case Rank.Operator:
                            return false;
                        case Rank.Client:
                            return true;
                        default:
                            break;
                    }
                    break;
                case Permission.projectView:
                    switch (this.Rank)
                    {
                        case Rank.Administrateur:
                            return true;
                        case Rank.Manager:
                            // Est il manager du projet ?
                            if (((Projet)param).GetManagers().Exists(manager => manager.Id == this.Id))
                                return true;
                            return false;
                        case Rank.Operator:
                            // Est il opérateur du projet ?
                            if (((Projet)param).GetOperationnels().Exists(operateur => operateur.Id == this.Id))
                                return true;
                            return false;
                        case Rank.Client:
                            // Est il créateur du projet ?
                            if (((Projet)param).Client.Id == this.Id)
                                return true;
                            return false;
                        default:
                            break;
                    }
                    break;
                case Permission.projectViewAffected:
                    break;
                case Permission.projectViewOwn:
                    break;
                case Permission.projectUpdate:
                    break;
                case Permission.projectDelete:
                    break;
                case Permission.userManagerView:
                    break;
                case Permission.userManagerViewOwnProject:
                    break;
                case Permission.userManagerUpdate:
                    break;
                case Permission.userManagerUpdateOwnProfile:
                    break;
                case Permission.userOperatorView:
                    break;
                case Permission.userOperatorUpdate:
                    break;
                case Permission.userOperatorUpdateOwnProfile:
                    break;
                case Permission.userClientView:
                    break;
                case Permission.userClientUpdateOwnProfile:
                    break;
                case Permission.ticketViewOwnProjects:
                    break;
                case Permission.ticketOperationnelViewOwnProjects:
                    break;
                case Permission.ticketViewStateOpen:
                    break;
                case Permission.ticketUpdate:
                    break;
                case Permission.ticketUpdateStateToOpen:
                    break;
                case Permission.ticketUpdateStateToResolve:
                    break;
                case Permission.ticketUpdateStateToClosed:
                    break;
                case Permission.ticketCreate:
                    break;
                case Permission.ticketCreateForOwnProject:
                    break;
                case Permission.ticketDelete:
                    break;
                case Permission.ticketComment:
                    break;
                case Permission.ticketCommentOwnProject:
                    break;
                case Permission.ticketValidate:
                    break;
                case Permission.ticketValidateOwnProject:
                    break;
                default:
                    break;
            }

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
