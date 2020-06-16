using System;

namespace Lab0609
{
    


    class Program
    {
        
        delegate double Function(double x);

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine($"Func [0, 1000]: {SolveByDichotomy(0, 1000, Func1, 1E-6)}");
                Console.WriteLine($"Func2 [-5, -2]: {SolveByDichotomy(-5, -2, Func2, 1E-6)}");
                Console.WriteLine($"Func3 [-1, 0]: {SolveByDichotomy(-1, 0, Func3, 1E-6)}");
                Console.WriteLine($"Func4 [0.03, 0.05]: {SolveByDichotomy(0.03, 0.05, Func4, 1E-8)}");
                Console.WriteLine($"Func5 [-3, 0]: {SolveByDichotomy(-3, 0, Func5, 1E-6)}");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Wrong input parameters.");
            }
            catch (SolveException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        static double SolveByDichotomy(double x0, double x1, Function function, double error)
        {
            if (x0 >= x1)
                throw new ArgumentException("Wrong input data: a >= b.");
            if (error <= 0)
                throw new ArgumentException("Wrong input data: error <= 0.");

            double f0 = function(x0);
            if (f0 == 0)
                return x0;
            double f1 = function(x1);
            if (f1 == 0)
                return x1;
            if (f0 * f1 > 0)
                throw new SolveException(x0, x1);

            while (x1 - x0 >= error)
            {
                double xCenter = (x0 + x1) / 2;
                double fCenter = function(xCenter);
                double tmValue = f0 * fCenter;
                if (tmValue > 0)
                    x0 = xCenter;
                else if (tmValue == 0)
                    return xCenter;
                else
                    x1 = xCenter;

                f0 = function(x0);
            }

            return (x0 + x1) / 2;
        }

        class SolveException : Exception
        {
            public SolveException(double a, double b) : base($"Signs of function at x={a} and x={b} are equals.") { }
        }



        static double Func1(double x)
        {
            return 2 * Math.Pow(x, 2)- 3 * x - 2;
        }

        static double Func2(double x)
        {
            return Math.Pow(x, 3) + 3 * Math.Pow(x, 2) - 3;
        }

        static double Func3(double x)
        {
            return Math.Pow(x, 4) - Math.Pow(x, 3) - 8 * Math.Pow(x, 2) + 4;
        }

        static double Func4(double x)
        {
            return Math.Sin(100 * x);
        }

        static double Func5(double x)
        {
            return x*x - 1;
        }

    }
}
