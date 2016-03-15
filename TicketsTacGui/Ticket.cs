using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsTacGui
{
    enum StateEnum
    {

        Open = 4,
        Commented = 3,
        Resolve = 2,
        Validate = 1,
        Closed = 0
    };

    class Ticket
    {
        public int Id { get; private set; }
        public string ProblemDescription { get; set; }
        public string AdditionnalNote { get; set; }

        public Projet Project { get; set; }

        public StateEnum State { get; set; }

        public List<User> UserAssign { get; set; }

        public Ticket(string problemDescription, Projet projet)
        {
            //CreateTicket();

        }
         public Ticket(Dictionary<string, string> ticket)
         {
            Id = int.Parse(ticket["Id"]);
            ProblemDescription = ticket["Description"];
            Project = Projet.GetProjetFromBDD(int.Parse(ticket["Projet_Id"]));
            State = (StateEnum)int.Parse(ticket["State"]);
            List<User> userList = new List<User>();
            List<Dictionary<String, String>> retourSelect = DB.SelectWhere("Users.Id, Users.Username, Users.Email, Users.Password, Users.Rank, Users.Created", "ticket_assignee.Ticket_Id = " + Id + ", Users.Id = ticket_assignee.User_Id", "ticket_assignee, Users");
            foreach (Dictionary<String, String> user in retourSelect)
            {
                User selectedUser = new User(user);
                userList.Add(selectedUser);
                UserAssign.Add(selectedUser);
            }

         }

         private Ticket CreateTicket(problemDescription)
         {

             List<string> fieldList = new List<string>();
             fieldList.Add("users");
             fieldList.Add("problem_description");
             fieldList.Add("projet");
             fieldList.Add("state");

             Console.WriteLine("Ouverture d'un nouveau ticket");

             Ticket ticket = new Ticket(problemDescription, state, project);
             List<string> ValueList = new List<string>();
             ValueList.Add(problemDescription);
             ValueList.Add("4");
             ValueList.Add(project.GetIDToString());
             DB.Insert(fieldList, ValueList, "tickets");

             return ticket;
         }

        private void AssignUser(User user)
        {
            List < string > fields = new List<string>();
            fields.Add("User_Id");
            fields.Add("Ticket_Id");
            List<string> values = new List<string>();
            values.Add(user.Id.ToString());
            values.Add(this.Id.ToString());
            int id=DB.Insert(fields, values, "ticket_assignee");
        }

        public void ConsultTicket()
        {
            List<Dictionary<string, string>> ListTicket;
            ListTicket = DB.Select("*", "tickets");

            Console.WriteLine("Selectionner un ticket a consulter");
            DB.Get(this.Id, "Tickets");
        }

        private Ticket EditTicket(Ticket editTicket)
        {

            List<string> fieldList = new List<string>();
            fieldList.Add("users");
            fieldList.Add("problem_description");
            fieldList.Add("projet");
            fieldList.Add("state");;

            Ticket ticket = DB.Get(this.Id, "Tickets");

            List<string> ValueList = new List<string>();
            if((ticket.ProblemDescription != editTicket.ProblemDescription) || (ticket.State != editTicket.State) || (ticket.AdditionnalNote != editTicket.AdditionnalNote))
            {
                if ((ticket.AdditionnalNote != null) || (editTicket.AdditionnalNote != null))
                {
                    ValueList.Add("3");
                }
                ValueList.Add(editTicket.ProblemDescription);
                ValueList.Add("4");
                ValueList.Add(project.GetIDToString());

                DB.Update(this.Id,fieldList, ValueList, "Tickets");
            }

            return editTicket;
        }
    }
}

