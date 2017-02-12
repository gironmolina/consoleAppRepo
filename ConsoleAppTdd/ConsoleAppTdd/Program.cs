using System;
using System.Linq;

namespace ConsoleAppTdd
{
    public class Program
    {
        public static void Main()
        {
            Console.Write("Enter upper limit: ");
            string readLine = Console.ReadLine();
            if (readLine == null)
                return;

            int range = int.Parse(readLine);

            for (var i = 0; i < range; i++)
            {
                string num = i % 7 == 0 || i.ToString().Contains('7') ? "BOOM" : i.ToString();
                Console.Write("{0}, ", num);
            }
        }
    }
}
