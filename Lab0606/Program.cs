using System;
using System.Text;

namespace Lab0606
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter words: ");
            string[] words = Console.ReadLine().Split(" ");

            Array.Sort(words);
            StringBuilder builder = new StringBuilder();
            Console.WriteLine("Sorted words:");
            foreach (string word in words)
            {
                if (word.Length > 0)
                {
                    Console.WriteLine(word);
                    builder.Append(word[^1]);
                }
            }
            Console.WriteLine($"Result: {builder}");
        }
    }
}
