using System;

namespace TicketsTac
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("coucou");
            DB.Migrate();
            DB.Select("*", "users");
            Console.ReadLine();
        }
    }
}