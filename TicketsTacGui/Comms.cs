using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsTacGui
{
    class Comms
    {
        public int Id { get; private set; }
        public string NoteContent { get; set; }
        public User Auteur { get; set; }

        public Int64 Date { get; set; }

        public Ticket Ticket { get; set; }

        public Comms(string noteContent, User auteur, Ticket ticket)
        {
            NoteContent = noteContent;
            Auteur = auteur;
            Date = DB.getTimestamp();
            Ticket = ticket;
        }

        public void InsertIntoBDD()
        {
            List<string> fieldList = new List<string>();
            fieldList.Add("Ticket_id");
            fieldList.Add("noteContent");
            fieldList.Add("auteur");
            fieldList.Add("date");

            List<string> ValueList = new List<string>();
            ValueList.Add(Ticket.Id.ToString());
            ValueList.Add("noteContent");
            ValueList.Add(User.currentUser.ToString());
            ValueList.Add(DB.getTimestamp().ToString());
            Id=DB.Insert(fieldList, ValueList, "Ticket_comms");

            
        }
}
}
