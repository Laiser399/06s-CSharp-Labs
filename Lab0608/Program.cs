using System;

namespace Lab0608
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose task:");
                Console.WriteLine("a) count the number of occurrences of a given word.");
                Console.WriteLine("b) replace penultimate word in string.");
                Console.WriteLine("c) find the k-th word in a string starting with a capital letter.");
                Console.WriteLine("e) exit from program.");

                Console.Write("Your choise: ");
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.E)
                    break;
                if (key != ConsoleKey.A && key != ConsoleKey.B && key != ConsoleKey.C)
                    continue;

                Console.Write("\nEnter string: ");
                string line = Console.ReadLine();
                switch (key)
                {
                    case ConsoleKey.A:
                        Console.Write("Enter word: ");
                        string word = Console.ReadLine();
                        Console.WriteLine($"Count of \"{word}\": {CountOf(word, line)}.");
                        break;
                    case ConsoleKey.B:
                        Console.Write("Enter word: ");
                        word = Console.ReadLine();
                        Console.WriteLine($"Replace result: {ReplacePrevLast(line, word)}");
                        break;
                    case ConsoleKey.C:
                        Console.Write("Enter k: ");
                        int k;
                        if (!int.TryParse(Console.ReadLine(), out k) || k <= 0)
                        {
                            Console.WriteLine("Error parse k.");
                            break;
                        }

                        string result;
                        if (FindK(line, k, out result))
                            Console.WriteLine($"Found word: {result}");
                        else
                            Console.WriteLine("Not found.");
                        break;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        static int CountOf(string word, string line)
        {
            // 1st
            int start = 0;
            int count = 0;
            while ((start = line.IndexOf(word, start, StringComparison.CurrentCultureIgnoreCase)) != -1)
            {
                start += word.Length;
                count++;
            }
            return count;
        }

        static string ReplacePrevLast(string line, string word)
        {
            // 2nd
            string[] splittedLine = line.Split(" ");
            if (splittedLine.Length >= 2)
                splittedLine[splittedLine.Length - 2] = word;
            string res = string.Join(" ", splittedLine);
            return res;
        }
    
        static bool FindK(string line, int k, out string result)
        {
            string[] splittedLine = line.Split(" ");
            foreach (string word in splittedLine)
            {
                if (word.Length == 0)
                    continue;
                if (char.IsUpper(word[0]))
                    k--;
                if (k == 0)
                {
                    result = word;
                    return true;
                }
            }
            result = "";
            return false;
        }
    
    }
}
