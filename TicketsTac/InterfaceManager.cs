using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsTac
{
    class InterfaceManager
    {
        public InterfaceManager()
        {
            

            Console.WriteLine("Bienvenue " + username + ":" + password);
        }

        public void DisplayWelcomeMessage()
        {
            Console.WriteLine("/*************************************\\");
            Console.WriteLine("|     Bienvenue sur Tickets Tac !     |");
            Console.WriteLine("\\*************************************/");
        }

        public void AskForAuthentication()
        {
            User user = null;
            while ( user == null )
            {
                Console.WriteLine("\Please authenticate to access this service.");
                Console.WriteLine("Email address:");

                string username = Console.ReadLine();

                Console.Write("Password: ");
                string password = Console.ReadLine();

                user = User.Connect(username, password);

                if (user == null) Logger.Error("Your email or password is wrong.");
            }

            Logger.info("Connection established.");
        }
    }
}
