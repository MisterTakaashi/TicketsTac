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
        public int Ticket_Id { get; set; }
        public Int64 Created { get; set; }
        public User Creator { get; set; }

        public Commentaire(string message, int ticketId, User creator, Int64 created)
        {
            Message = message;
            Creator = creator;
            Created = created;
            Ticket_Id = ticketId;
        }

        public static List<Commentaire> GetAllForTicket(int idTicket)
        {
            List<Commentaire> ret = new List<Commentaire>();
            List<Dictionary<string, string>> replies = DB.SelectWhere("*", "Ticket_Id = " + idTicket, "Ticket_comms");
            foreach( Dictionary<string, string> reply in replies )
            {
                User creator = User.Get(int.Parse(reply["Creator_Id"]));
                Int64 created = Int64.Parse(reply["Created"]);
                string message = reply["Message"];

                ret.Add(new Commentaire(message, idTicket, creator, created));
            }
            
            return ret;
        }

        public void InsertIntoBDD()
        {
            List<string> champs = new List<string>();
            champs.Add("Ticket_Id");
            champs.Add("Message");
            champs.Add("Created");
            champs.Add("User_Id");
            List<string> values = new List<string>();
            values.Add(Ticket_Id.ToString());
            values.Add(Message);
            values.Add(Created.ToString());
            values.Add(Creator.Id.ToString());
            DB.Insert(champs, values, "Ticket_comms");
        }
    }
}
