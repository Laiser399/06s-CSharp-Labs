using System;

namespace LAB0602
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"e={CalcExp(10E-15)}");
            Console.WriteLine($"PI={CalcPI(10E-15)}");
            Console.WriteLine($"Ln(2)={CalcLn2(10E-15)}");//=0,693147180559944
        }

        static double CalcExp(double error)
        {
            // ряд Тейлора
            double result = 0, term = 1;
            for (int i = 1; term >= error; ++i)
            {
                result += term;
                term /= i;
            }
            return result;
        }

        static double CalcPI(double error)
        {
            // формула Бэйли — Боруэйна — Плаффа
            double result = 0, term = 1;
            for (double i = 0; term >= error; ++i)
            {
                term = (4 / (8 * i + 1) - 2 / (8 * i + 4) - 1 / (8 * i + 5) - 1 / (8 * i + 6)) /
                    Math.Pow(16, i);
                result += term;
            }
            return result;
        }

        static double CalcLn2(double error)
        {
            // вывод из ряда Тейлора
            double multiplier = 2.0 / 3.0, result = multiplier, term;
            double k = 3;
            do
            {
                multiplier /= 9;
                term = multiplier / k;
                result += term;
                k += 2;
            } while (term >= error);
            return result;
        }
    }
}
