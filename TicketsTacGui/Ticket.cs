﻿using System;
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
        public string Name { get; set; }
        public string ProblemDescription { get; set; }
        public List<Commentaire> AdditionnalNotes { get; set; }
        public int Created { get; private set; }
        public Projet Project { get; set; }

        public StateEnum State { get; set; }

        public List<User> Assignees { get; set; }

        public User Auteur { get; set; }

        public Ticket(int id, string name, string problemDescription, Projet projet, User auteur)
        {
            Id = id;
            Name = name;
            ProblemDescription = problemDescription;
            Project = projet;
            State = StateEnum.Open;
            Assignees = new List<User>();
            AdditionnalNotes = new List<Commentaire>();
            Created = DB.getTimestamp();
            Auteur = auteur;
        }

        public Ticket(Dictionary<string, string> ticket)
         {
            Id = int.Parse(ticket["Id"]);
            Name = ticket["Name"];
            ProblemDescription = ticket["Description"];

            Project = Projet.GetProjetFromBDD(int.Parse(ticket["Projet_Id"]));
            State = (StateEnum)int.Parse(ticket["State"]);
            Created = int.Parse(ticket["Created"]);
            Auteur = User.Get(int.Parse(ticket["Auteur_Id"]));
            Assignees = new List<User>();
            AdditionnalNotes = Commentaire.GetAllForTicket(Id);

            List<Dictionary<String, String>> ticketAssignees = DB.SelectWhere("*", "Ticket_Id = " + Id, "Ticket_Assignee");
            foreach (Dictionary<String, String> ticketAssignee in ticketAssignees)
            {
                User selectedUser = User.Get(int.Parse(ticketAssignee["Assignee_Id"]));
                Assignees.Add(selectedUser);
                //ViewModel.Items.Add(selectedUser.Username, selectedUser.Username);
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
            int id=DB.Insert(fields, values, "Ticket_Assignee");
        }

        public void ConsultTickets()
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
            fieldList.Add("state");

            List<string> ValueList = new List<string>();
            if ((this.AdditionnalNotes != null))
            {
                this.State = StateEnum.Commented;
            }

            ValueList.Add(this.ProblemDescription);
            ValueList.Add(this.Project.GetIDToString());
            ValueList.Add(this.State.ToString());
           
            DB.Update(this.Id, fieldList, ValueList, "Tickets");
        }

        private Commentaire AddComment(string noteContent)
        {
            Console.WriteLine("edition d'un nouveau commentaire");
            Commentaire comms = new Commentaire(noteContent, this.Id, User.currentUser, DB.getTimestamp());
            comms.InsertIntoBDD();
            this.AdditionnalNotes.Add(comms);

            return comms;
        }

        public static Ticket GetFromDb(int id)
        {
            return new Ticket(DB.Get(id, "Tickets"));
        }

        public void ChangeState(StateEnum state)
        {
            this.State = state;

            List<string> keys = new List<string>();
            keys.Add("State");

            List<string> values = new List<string>();
            values.Add(((int)state).ToString());

            DB.Update(this.Id, keys, values, "Tickets");
        }

    }
}