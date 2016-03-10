using System;

namespace TicketsTac
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("coucou");
            DB.Select("*", "users");
            Console.ReadLine();
        }
    }
}