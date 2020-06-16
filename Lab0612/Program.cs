using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


namespace Lab0612
{
    class Program
    {
        static void Main(string[] args)
        {
            const string filename = @"D:\_Google_Synchronized_\Synchronized\Projects_Sync\C#_Projects\Lab0612\SomeText.txt";

            WordsContainer analyzeResult;
            Console.WriteLine("Analyzing...");
            if (!AnalyzeFile(filename, out analyzeResult))
            {
                Console.WriteLine("Error analyze file.");
                return;
            }

            Console.WriteLine("Analyze done.");
            if (analyzeResult.TotalWords == 0)
            {
                Console.WriteLine("No results.");
                return;
            }

            while (true)
            {
                Console.WriteLine($"\nMost frequent word: {analyzeResult.MostFrequentWord}");
                Console.WriteLine("1. Show all results.");
                Console.WriteLine("2. Show frequency of given word.");
                Console.WriteLine("3. Exit.");
                Console.Write("Press key: ");

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine(analyzeResult);
                        Console.WriteLine($"Total words: {analyzeResult.TotalWords}.");
                        break;
                    case ConsoleKey.D2:
                        Console.Write("\nEnter word: ");
                        string word = Console.ReadLine().Trim();
                        int count = analyzeResult.GetCountOf(word.ToLower());
                        Console.WriteLine($"Count: {count} ({(float)count / analyzeResult.TotalWords * 100}%)");
                        break;
                    case ConsoleKey.D3:
                        return;
                }
            }

        }

        static bool AnalyzeFile(string filename, out WordsContainer result)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string fullFile = reader.ReadToEnd();
                    Regex regex = new Regex(@"\b[a-zа-я]+\b", RegexOptions.IgnoreCase);
                    result = new WordsContainer();
                    Match match = regex.Match(fullFile);
                    while (match.Success)
                    {
                        result.Add(match.Value.ToLower());
                        match = match.NextMatch();
                    }
                }
                return true;
            }
            catch (IOException)
            {
                //Console.WriteLine($"Error reading file: {e.Message}");
                result = null;
                return false;
            }
        }
    }
}
