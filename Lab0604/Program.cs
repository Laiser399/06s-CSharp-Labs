using System;
using System.Text;

namespace Lab0604
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Wrong number of args. Must be 2.");
                return;
            }

            int value;
            if (!int.TryParse(args[0], out value) || value < 0)
            {
                Console.WriteLine("Error parse number.");
                return;
            }

            int numberSystem;
            if (!int.TryParse(args[1], out numberSystem))
            {
                Console.WriteLine("Error parse number system.");
                return;
            }
            if (numberSystem < 2)
            {
                Console.WriteLine($"Wrong number system: {numberSystem}. Must be > 1.");
                return;
            }

            Console.WriteLine(ToNumberSystem(value, numberSystem));
        }

        static string ToNumberSystem(int value, int numberSystem)
        {
            StringBuilder builder = new StringBuilder();
            while (value > 0)
            {
                int residual = value % numberSystem;
                value /= numberSystem;
                if (residual < 10)
                    builder.Insert(0, (char)(residual + '0'));
                else if (residual < 36)
                    builder.Insert(0, (char)(residual - 10 + 'A'));
                else
                    builder.Insert(0, $"[{residual}]");
            }
            return builder.ToString();
        }
    }
}
