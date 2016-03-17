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
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public Rank Rank { get; private set; }
        public static User currentUser = new User(0, "Test", "Test", Rank.Administrateur, 0);
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

        public void setId(int id, int newId)
        {
            DB.Update(id, new List<string> { "id" }, new List<string> { newId.ToString() }, "Users");
        }

        public void setPassword(string password) {
            Password = password.ToSHA1();
            DB.Update(Id, new List<string> { "Password" }, new List<string> { Password }, "Users");
        }

        public void setUsername(string username)
        {
            Username = username;
            DB.Update(Id, new List<string> { "Username" }, new List<string> { Username }, "Users");
        }

        public void setEmail(string email)
        {
            Email = email;
            DB.Update(Id, new List<string> { "Email" }, new List<string> { Email }, "Users");
        }

        public static User Get(int id)
        {
            return new User(DB.Get(id, "Users"));
        }

        public void setRank(int id, Rank rank)
        {
            DB.Update(id, new List<string> { "rank" }, new List<string> { rank.ToString() }, "users");
        }

        public static List<User> GetAll()
        {
            List<User> allUsers = new List<User>();
            List<Dictionary<string, string>> r = DB.Select("*", "Users");
            foreach (Dictionary<string, string> aUser in r)
            {
                allUsers.Add(new User(Int32.Parse(aUser["Id"]), (String)aUser["Username"], (String)aUser["Email"], (Rank)Int32.Parse(aUser["Rank"]), DB.getTimestamp()));
            }
            return allUsers;
        }

        public static void Delete(int id)
        {
            DB.Delete(id, "users");
        }

        public static User Connect(string email, string password)
        {
            List<Dictionary<string, string>> result = DB.SelectWhere("*", string.Format("Email = '{0}' AND Password = '{1}'", email, password), "Users");

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
                            return true;
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
                    return true;
                case Permission.projectViewOwn:
                    break;
                case Permission.projectUpdate:
                    break;
                case Permission.projectDelete:
                    break;
                case Permission.userView:
                    User thisUser = (User)param;
                    switch (this.Rank)
                    {
                        case Rank.Administrateur:
                            return true;
                        case Rank.Manager:
                            if (thisUser.Rank != Rank.Administrateur)
                                 return true;
                            break;
                        case Rank.Operator:
                            if (thisUser.Rank != Rank.Administrateur)
                                return true;
                            break;
                        case Rank.Client:
                            if (thisUser.Id != this.Id)
                                return true;
                            break;
                        default:
                            break;
                    }
                    return true;
                case Permission.userUpdate:
                    return true;
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
                case Permission.ticketView:
                    Ticket thisTicket = ((Ticket)param);

                    switch (this.Rank)
                    {
                        case Rank.Administrateur:
                            return true;
                        case Rank.Manager:
                            if (this.hasPermissionTo(Permission.projectView, ((Ticket)param).Project))
                                return true;
                            break;
                        case Rank.Operator:
                            if (thisTicket.Assignees.Exists(assigned => assigned.Id == this.Id) && (thisTicket.State == StateEnum.Open || thisTicket.State == StateEnum.Commented))
                                return true;
                            break;
                        case Rank.Client:
                            if (thisTicket.Project.Client.Id == this.Id && (thisTicket.State == StateEnum.Open || thisTicket.State == StateEnum.Commented))
                                return true;
                            break;
                        default:
                            break;
                    }
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
                    switch (this.Rank)
                    {
                        case Rank.Administrateur:
                            return true;
                        case Rank.Manager:
                            return true;
                        case Rank.Operator:
                            return false;
                        case Rank.Client:
                            return false;
                        default:
                            break;
                    }
                    break;
                case Permission.ticketUpdateStateToResolve:
                    return true;
                case Permission.ticketUpdateStateToClosed:
                    switch (this.Rank)
                    {
                        case Rank.Administrateur:
                            return true;
                        case Rank.Manager:
                            return true;
                        case Rank.Operator:
                            return false;
                        case Rank.Client:
                            return false;
                        default:
                            break;
                    }
                    break;
                case Permission.ticketCreate:
                    switch (this.Rank)
                    {
                        case Rank.Administrateur:
                            return true;
                        case Rank.Manager:
                            break;
                        case Rank.Operator:
                            break;
                        case Rank.Client:
                            if (((Projet)param).Client.Id == this.Id)
                                return true;
                            break;
                        default:
                            break;
                    }
                    break;
                case Permission.ticketCreateForOwnProject:
                    break;
                case Permission.ticketDelete:
                    break;
                case Permission.ticketComment:
                    return true;
                case Permission.ticketCommentOwnProject:
                    break;
                case Permission.ticketValidate:
                    switch (this.Rank)
                    {
                        case Rank.Administrateur:
                            return true;
                        case Rank.Manager:
                            break;
                        case Rank.Operator:
                            break;
                        case Rank.Client:
                            if (((Ticket)param).State == StateEnum.Resolve)
                                return true;
                            break;
                        default:
                            break;
                    }
                    break;
                case Permission.ticketValidateOwnProject:
                    break;
                default:
                    break;
            }

            return false;
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
        userView = 31,
        userUpdate = 33,
        userManagerView = 5,
        userManagerViewOwnProject = 23,
        userManagerUpdate = 6,
        userManagerUpdateOwnProfile = 7,
        userOperatorView = 8,
        userOperatorUpdate = 19,
        userOperatorUpdateOwnProfile = 18,
        userClientView = 22,
        userClientUpdateOwnProfile = 24,
        ticketView = 30,
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
