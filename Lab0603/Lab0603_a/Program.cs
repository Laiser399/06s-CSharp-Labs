using System;

namespace Lab0603
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter chars: ");
            string[] items = Console.ReadLine().Split(" ");

            double sum = 0;
            int count = 0;
            Console.WriteLine("Input:");
            foreach (string item in items)
            {
                if (item.Length > 1)
                    Console.WriteLine($"{item} - ignored");
                else if (item.Length == 1)
                {
                    Console.WriteLine($"{item} ({(int) item[0]})");
                    sum += item[0];
                    count++;
                }
            }

            Console.WriteLine();
            if (count == 0)
                Console.WriteLine("Nothing to compute.");
            else
                Console.WriteLine($"Average: {sum / count}");
        }
    }
}
