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
        public List<User> Users;

        public string ProblemDescription { get; set; }
        public string AdditionnalNote { get; set; }

        public Projet Projets { get; set; }

        public StateEnum State { get; set; }

        public Ticket(string problemDescription, Projet projet)
        {
            //CreateTicket();

        }
       /* public Ticket(Dictionary<string, string> ticket)
        {
            Id = int.Parse(ticket["Id"]);
            ProblemDescription = ticket["Description"];
            Project = Projet.GetProjetFromBDD(int.Parse(ticket["Projet_Id"]));
            State = (StateEnum)int.Parse(ticket["State"]);


        }

        private Ticket CreateTicket()
        {
            string problemDescription = "";

            List<string> fieldList = new List<string>();
            fieldList.Add("users");
            fieldList.Add("problem_description");
            fieldList.Add("projet");
            fieldList.Add("state");

            Console.WriteLine("Ouverture d'un nouveau ticket");

            Console.WriteLine("Veuillez saisir la description du probleme, pour arreter saisissez 'EOF'.");
            Console.WriteLine("Description : ");

            string saisieDescription = Console.ReadLine();
            while (problemDescription != "EOF")
            {
                string.Concat(problemDescription, saisieDescription);
            }

            Ticket ticket = new Ticket(problemDescription, state, project);
            List<string> ValueList = new List<string>();
            ValueList.Add(problemDescription);
            ValueList.Add("4");
            ValueList.Add(project.GetIDToString());
            DB.Insert(fieldList, ValueList, "tickets");

            Console.ReadLine();

            return ticket;
        }

        public void AssignUser()
        {
            Boolean isChoiceAssignementCorrect = false;
            while (isChoiceAssignementCorrect != true)
            {
                Console.WriteLine("Souhaitez vous assigner le ticket à un collaborateur ?");
                Console.WriteLine("'Y' oui 'N' non ");
                string choiceAssignement = Console.ReadLine();
                if (choiceAssignement.ToLower() == "y")
                {
                    isChoiceAssignementCorrect = true;
                    chooseUser();
                }
                else if (choiceAssignement.ToLower() == "n")
                {
                    isChoiceAssignementCorrect = true;
                }
                else
                {
                    Console.WriteLine("Saisie Incorrecte veuillez recommencer");
                    isChoiceAssignementCorrect = false;
                }
            }
        }

        private void chooseUser()
        {
            List<User> UserListFromBDD = new List<User>();
            UserListFromBDD = User.GetAll();

            List<User> UserList = new List<User>();

            Boolean stopChoice = false;
            while (stopChoice != true)
            {
                Console.WriteLine("A quel collaborateur souhaitez vous assigner le ticket ?");
                Console.WriteLine("Saisissez l'id correspondant au collaborateur.");
                Console.WriteLine("Choix du collaborateur :");
                int choiceCollab = Convert.ToInt32(Console.ReadLine());
                if (choiceCollab <= UserListFromBDD.Count)
                {
                    foreach (User user in UserListFromBDD)
                    {
                        if (user.GetID() == choiceCollab)
                        {
                            Console.WriteLine("Souhaitez vous attribuer un autre collaborateur ?");
                            string choiceAssignement;
                            do
                            {
                                Console.WriteLine("'Y' oui 'N' non ");
                                choiceAssignement = Console.ReadLine();
                                if (choiceAssignement.ToLower() == "y")
                                {
                                    stopChoice = false;
                                    UserList.Add(user);
                                }
                                else if (choiceAssignement.ToLower() == "n")
                                {
                                    stopChoice = true;
                                }
                                else
                                {
                                    Console.WriteLine("Saisie Incorrecte veuillez recommencer");
                                    stopChoice = false;
                                }
                            }

                            while (choiceAssignement.ToLower() != "y" || choiceAssignement.ToLower() != "n");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Saisie Incorrecte veuillez recommencer");
                    stopChoice = false;
                }
            }
        }
        public void ConsultTicket()
        {
            List<Dictionary<string, string>> ListTicket;
            ListTicket = DB.Select("*", "tickets");

            Console.WriteLine("Selectionner un ticket a consulter");



            Boolean stopChoice = false;
            while (stopChoice != true)
            {
                Console.WriteLine("A quel collaborateur souhaitez vous assigner le ticket ?");
                Console.WriteLine("Saisissez l'id correspondant au collaborateur.");
                Console.WriteLine("Choix du collaborateur :");
                int choiceTicket = Convert.ToInt32(Console.ReadLine());
                if (choiceTicket <= ListTicket.Count)
                {
                    foreach (Dictionary<string, string> ticket in ListTicket)
                    {
                        if (ticket.id == choiceTicket)
                        {
                            Ticket tic = new Ticket(ticket);
                            Console.WriteLine(tic.id);
                            Console.WriteLine(tic.ProblemDescription);
                            Console.WriteLine(tic.Projets);
                            if (tic.AdditionnalNote != null)
                            {
                                Console.WriteLine(tic.AdditionnalNote);
                            }
                            Console.WriteLine(tic.state);
                            stopChoice = true;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Saisie Incorrecte veuillez recommencer");
                    stopChoice = false;
                }
            }

        }*/
    }
}

