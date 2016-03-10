using System;

namespace TicketsTac
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("coucou");
            Console.ReadLine();
            DB.Migrate();
            //DB.Select("*", "users");
        }
    }
}
