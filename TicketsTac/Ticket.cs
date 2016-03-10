using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsTac
{
    class Ticket
    {
        public int Id { get; private set; }
        public List<User> Users;

        public string ProblemDescription { get; set; }
        public string AdditionnalNote { get; set; }

        public Projet Projet { get; set; }

        public enum StateEnum
        {

            Open = 5,
            Assigned = 4,
            Commented = 3,
            Resolve = 2,
            Validate = 1,
            Closed = 0
        };

        public StateEnum State { get; set; }

        public Ticket(int id, string problemDescription, string additionnalNote, Projet projet)
        {
            Id = id;
            ProblemDescription = problemDescription;
            AdditionnalNote = additionnalNote;
            Projet = projet;
            Users = new List<User>();
        }

    }
}
