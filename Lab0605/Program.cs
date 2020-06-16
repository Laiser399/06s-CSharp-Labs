using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace Lab0605
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal value;
            int numberSystem;

            if (args.Length > 0)
            {
                try {
                    using (StreamReader reader = new StreamReader(args[0]))
                    {
                        string[] items = reader.ReadLine().Split(" ");
                        if (items.Length != 2 || !ParseInputValues(items[0], items[1], out value, out numberSystem))
                        {
                            Console.WriteLine("Wrong file format.");
                            return;
                        }
                        Console.WriteLine($"Value: {value}");
                        Console.WriteLine($"Number system: {numberSystem}");
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("Error reading file: " + e.Message);
                    return;
                }
            }
            else
            {
                Console.Write("Enter value: ");
                string valueStr = Console.ReadLine();
                Console.Write("Enter number system: ");
                string numberSystemStr = Console.ReadLine();

                if (!ParseInputValues(valueStr, numberSystemStr, out value, out numberSystem))
                {
                    Console.WriteLine("Wrong values format.");
                    return;
                }
            }
            
            Console.WriteLine("Result: " + ToNumberSystem(value, numberSystem));
        }

        static bool ParseInputValues(string valueStr, string numberSystemStr, out decimal value, out int numberSystem)
        {
            numberSystem = 0;
            if (!decimal.TryParse(valueStr.Replace(".", ","), out value) || !int.TryParse(numberSystemStr, out numberSystem))
                return false;

            if (numberSystem < 2)
                return false;

            return true;
        }

        static string ToNumberSystem(decimal value, int numberSystem, int maxSigns = 15)
        {
            StringBuilder builder = new StringBuilder();
            if (value < 0)
            {
                builder.Append("-");
                value = -value;
            }
            builder.Append(ToNumberSystem((int)value, numberSystem));
            builder.Append(",");
            value -= (int)value;

            int shift = builder.Length;
            List<decimal> history = new List<decimal>();
            for (int i = 0; i < maxSigns; ++i)
            {
                value *= numberSystem;
                int index = history.FindIndex(item => item.Equals(value));
                if (index >= 0)
                {
                    builder.Insert(index + shift, '(');
                    builder.Append(')');
                    break;
                }
                history.Add(value);

                int digit = (int)value;
                value -= digit;
                builder.Append(ConvertDigit(digit));
            }
            return builder.ToString();
        }

        static string ToNumberSystem(int value, int numberSystem)
        {
            if (value == 0)
                return "0";

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

        static string ConvertDigit(int digit)
        {
            if (digit < 10)
                return digit.ToString();
            else if (digit < 36)
                return ((char)(digit - 10 + 'A')).ToString();
            else
                return "[" + digit + "]";
        }
    }
}
