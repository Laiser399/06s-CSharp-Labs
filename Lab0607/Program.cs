using System;

namespace Lab0607
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter string: ");
            Console.WriteLine($"Sorted: {SortedString(Console.ReadLine())}");
        }

        static string SortedString(string s)
        {
            char[] chars = s.ToCharArray();
            Array.Sort(chars);
            return new string(chars);
        }
    }
}
