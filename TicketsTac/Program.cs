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

            User user = null;

            do
            {
                Console.WriteLine("Entrez votre mail ?");
                string email = Console.ReadLine();
                Console.WriteLine("Entrez votre mot de passe ?");
                string pass = Console.ReadLine();

                user = User.Connect(email, pass.ToSHA1());

                if (user == null)
                    Logger.Error("Your email or password is wrong");
            } while (user == null);

            Logger.Info("Connection etablished");

            Console.ReadLine();
        }
    }
}