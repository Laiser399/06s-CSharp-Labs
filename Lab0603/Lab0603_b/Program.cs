using System;

namespace Lab0603_b
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter numbers: ");
            string[] strings = Console.ReadLine().Split(" ");
            if (strings.Length == 0)
            {
                Console.WriteLine("Nothing to compute.");
                return;
            }

            double multi = 1;
            for (int i = 0; i < strings.Length; ++i)
            {
                if (!double.TryParse(strings[i].Replace(".", ","), out double value))
                {
                    Console.WriteLine($"Error parse number \"{strings[i]}\".");
                    return;
                }
                if (value < 0)
                {
                    Console.WriteLine($"All values must be >= 0, but {value} < 0.");
                    return;
                }

                multi *= value;
                if (multi == 0)
                    break;
            }

            Console.WriteLine($"Geometric average: {Math.Pow(multi, 1.0 / strings.Length)}");
        }

        
    }
}
