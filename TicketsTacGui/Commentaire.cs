using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsTacGui
{
    class Commentaire
    {
        public string Message { get; set; }
        public Ticket Ticket { get; set; }
        public Int64 Created { get; set; }
        public User Creator { get; set; }

        public Commentaire(string message, Ticket ticket, User creator, Int64 created)
        {
            Message = message;
            Creator = creator;
            Created = created;
        }

        public void InsertIntoBDD()
        {
            List<string> champs = new List<string>();
            champs.Add("Ticket_Id");
            champs.Add("Message");
            champs.Add("Created");
            champs.Add("User_Id");
            List<string> values = new List<string>();
            values.Add(Ticket.Id.ToString());
            values.Add(Message);
            values.Add(Created.ToString());
            values.Add(Creator.Id.ToString());
            DB.Insert(champs, values, "Ticket_comms");
        }
    }
}
