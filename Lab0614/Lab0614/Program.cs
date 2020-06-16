using System;
using System.Collections;
using Lab0614.Binary;
using System.IO;

namespace Lab0614
{
    class Program
    {
        static void Main(string[] args)
        {
            //D:\_Google_Synchronized_\Synchronized\Projects_Sync\C#_Projects\Lab0614\delete_this_later\source.txt
            //D:\_Google_Synchronized_\Synchronized\Projects_Sync\C#_Projects\Lab0614\delete_this_later\results.txt


            if (args.Length != 2)
            {
                Console.WriteLine("Args count must be 2.");
                return;
            }

            Console.WriteLine("Start input, output.");
            Console.WriteLine($"Input file: {args[0]}");
            Console.WriteLine($"Output file: {args[1]}");
            try
            {
                using (StreamReader input = new StreamReader(args[0]))
                using (StreamWriter output = new StreamWriter(args[1]))
                {
                    string line;
                    while ((line = input.ReadLine()) is object)
                    {
                        Console.WriteLine($"Start parse formula: {line}");
                        output.Write($"Formula: {line}\n");

                        bool parsed = FormulaParser.TryParse(line, out Formula formula);
                        if (parsed)
                        {
                            Console.WriteLine("Formula parsed successfully");

                            formula.MakePerfectNormalForms(out string PDNF, out string PCNF);
                            output.Write($"\tPDNF: {PDNF}\n");
                            output.Write($"\tPCNF: {PCNF}\n");

                            Console.WriteLine($"PDNF: {PDNF}");
                            Console.WriteLine($"PCNF: {PCNF}");
                        }
                        else
                        {
                            Console.WriteLine("Parse error.");
                            output.Write("\tParse error.\n");
                        }
                        output.Write("\n");
                    }
                }

                Console.WriteLine("Results saved.");
            }
            catch (IOException e)
            {
                Console.WriteLine($"IO error: {e}");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("At least one path is empty.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Chtoo??");
            }
        }

        static void Print(IEnumerable arr)
        {
            foreach (var obj in arr)
            {
                Console.Write(obj);
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}
