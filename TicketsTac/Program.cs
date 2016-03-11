using System;

namespace TicketsTac
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("coucou");
            // DB.Migrate();                // Décommenter pour que l'exécution prépare la BDD

            /* Tests Pierrick */
            User u = new User(0, "Zozo", "Zozoleclown1@gmail.com", Rank.Administrateur);
            DB.Insert<User>(u, "users");
            // Fin tests Pierrick */

            Console.ReadLine();
        }
    }
}