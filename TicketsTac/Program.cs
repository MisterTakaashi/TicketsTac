using System;

namespace TicketsTac
{
    class Program
    {
        static void Main(string[] args)
        {
            // DB.Migrate();                // Décommenter pour que l'exécution prépare la BDD

            /* Tests Pierrick 
            User u = new User(0, "Zozo", "Zozoleclown1@gmail.com", Rank.Administrateur, 1457682682);
            u.Password = "toto".ToSHA1();
            DB.Insert<User>(u, "users");*/


            //DB.Migrate();                // Décommenter pour que l'exécution prépare la BDD

            /* Tests Pierrick */
            InterfaceManager im = new InterfaceManager();
            im.DisplayWelcomeMessage();
            im.AskForAuthentication();
            // Fin tests Pierrick */

            Console.ReadLine();
        }
    }
}