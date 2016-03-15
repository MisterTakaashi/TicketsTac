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
        public List<Comms> AdditionnalNote { get; set; }

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
            //List<Dictionary<String, String>> retourSelect = DB.SelectWhere("Users.Id, Users.Username, Users.Email, Users.Password, Users.Rank, Users.Created", "ticket_assignee.Ticket_Id = " + Id + ", Users.Id = ticket_assignee.User_Id", "ticket_assignee, Users");
            List<Dictionary<String, String>> retourSelect = DB.SelectWhere("*", "Ticket_Id = " + Id, "Ticket_assignee");
            foreach (Dictionary<String, String> user in retourSelect)
            {
                User selectedUser = new User(user);
                userList.Add(selectedUser);
                UserAssign.Add(selectedUser);
            }

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

        private void EditTicket()
        {

            List<string> fieldList = new List<string>();
            fieldList.Add("problem_description");
            fieldList.Add("projet");
            fieldList.Add("state");;

            List<string> ValueList = new List<string>();
            if ((this.AdditionnalNote != null))
            {
                this.State = StateEnum.Commented;
            }

            ValueList.Add(this.ProblemDescription);
            ValueList.Add(this.Project.GetIDToString());
            ValueList.Add(this.State.ToString());
           
            DB.Update(this.Id, fieldList, ValueList, "Tickets");
        }

        private Comms AddComment(string noteContent)
        {
            Console.WriteLine("edition d'un nouveau commentaire");
            Comms comms = new Comms(noteContent, User.currentUser, this);
            comms.InsertIntoBDD();
            this.AdditionnalNote.Add(comms);

            return comms;
        }

    }
}

