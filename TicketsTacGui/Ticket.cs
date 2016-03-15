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
        List<int> UserId = new List<int>();
        List<Projet> ProjectList = new List<Projet>();

        /*public void AddUserList()
        {
            List<int> fieldList = new List<int>();

            foreach (var User in Users)
            {
               int id = DB.select(User.fieldList, "users");

                UserId.Add(id);

            }
        }
        public void AddProjectList()
        {
            List<string> fieldList = new List<string>("nom");

            foreach (var Projet in Projets)
            {
                string nom = DB.select(Project.fieldList, "users");

                ProjectList.Add(nom);

            }
        }
        private Ticket CreateTicket()
        {
            Projet project;
            string problemDescription = "";

            List<string> fieldList = new List<string>("users", "problem_description","projet", "state");         
            Console.WriteLine("Ouverture d'un nouveau ticket");

            
            Boolean isChoiceProjectCorrect = false;
            while (isChoiceProjectCorrect != true)
            {

               

            }

            Console.WriteLine("Veuillez saisir la description du probleme, pour arreter saisissez 'EOF'.");
            Console.WriteLine("Description : ");

            string saisieDescription = Console.ReadLine();
            while (problemDescription != "EOF" )
            {
                string.Concat(problemDescription, saisieDescription);
            }
            
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


            //List<string> ValueList = new List<string>(user.id, problemDescription);
            Ticket ticket = new Ticket(problemDescription, project);
            //DB::insert(fieldList, ValueList, "tickets");

            Console.ReadLine();

            return ticket;
        }

        private void chooseUser()
        {
            Boolean stopChoice = false;
            while( stopChoice != true)
            {
                Console.WriteLine("A quel collaborateur souhaitez vous assigner le ticket ?");
                Console.WriteLine("Saisissez l'id correspondant au collaborateur.");
                Console.WriteLine("Choix du collaborateur :");
                int choiceCollab = Console.ReadLine();
                if (choiceCollab <= ProjectList.Count)
                {
                    foreach (User user in UserList)
                    {
                        if (user.getID() == choiceCollab)
                        {
                            userSelected = user;
                            Console.WriteLine("Souhaitez vous attribuer un autre collaborateur ?");
                            string choiceAssignement;
                            do
                            {
                                Console.WriteLine("'Y' oui 'N' non ");
                                choiceAssignement = Console.ReadLine();
                                if (choiceAssignement.ToLower() == "y")
                                {
                                    stopChoice = false;
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
        }*/
    }
}
