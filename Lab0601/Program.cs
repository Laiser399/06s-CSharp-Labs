using System;
using System.Numerics;

namespace Lab0601
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Wrong number of parameters!");
                return;
            }

            double[] values;
            if (!ParseValues(args, out values))
            {
                Console.WriteLine("Error parse input parameters.");
                return;
            }

            Console.WriteLine($"a={values[0]}   b={values[1]}   c={values[2]}   d={values[3]}");

            if (values[0] == 0)
            {
                Console.WriteLine("Error: \"a\" must be non-zero.");
                return;
            }

            Complex[] result;
            Complex[] testResult;
            SolveByKardano(values[0], values[1], values[2], values[3], out result);
            Calc(values[0], values[1], values[2], values[3], result, out testResult);
            Console.WriteLine("Result Kardano");
            for (int i = 0; i < result.Length; ++i)
            {
                Console.WriteLine($"x{i + 1} = {result[i]}");
                //Console.WriteLine($"Test: {testResult[i]}");
            }

            SolveByViet(values[0], values[1], values[2], values[3], out result);
            Calc(values[0], values[1], values[2], values[3], result, out testResult);
            Console.WriteLine("\nResult Viet");
            for (int i = 0; i < result.Length; ++i)
            {
                Console.WriteLine($"x{i + 1} = {result[i]}");
                //Console.WriteLine($"Test: {testResult[i]}");
            }

        }

        static bool ParseValues(string[] args, out double[] values)
        {
            values = new double[args.Length];
            for (int i = 0; i < args.Length; ++i)
            {
                if (!double.TryParse(args[i].Replace(".", ","), out values[i]))
                    return false;
            }
            return true;
        }

        static void SolveByKardano(double a, double b, double c, double d, out Complex[] result)
        {
            result = new Complex[3];

            double p = (3 * a * c - b * b) / (3 * a * a);
            double q = (2 * Math.Pow(b, 3) - 9 * a * b * c + 27 * a * a * d) / 
                (27 * Math.Pow(a, 3));
            double Q = Math.Pow(p / 3, 3) + Math.Pow(q / 2, 2);

            //Console.WriteLine($"Comment: p={p}   q={q}   Q={Q}");//TODO del
            Complex A = Complex.Pow(Complex.Sqrt(Q) - q / 2, 1.0 / 3);
            if (A == 0)
                A = Complex.Pow(-Complex.Sqrt(Q) - q / 2, 1.0 / 3);
            if (A == 0)
            {
                result[0] = result[1] = result[2] = 0;
                return;
            }
            Complex B = (-p / 3) / A;

            Complex y1 = A + B;
            Complex y2 = -(A + B) / 2 + Complex.ImaginaryOne * (A - B) * Math.Sqrt(3) / 2;
            Complex y3 = -(A + B) / 2 - Complex.ImaginaryOne * (A - B) * Math.Sqrt(3) / 2;

            result[0] = y1 - b / (3 * a);
            result[1] = y2 - b / (3 * a);
            result[2] = y3 - b / (3 * a);
        }

        static void SolveByViet(double a, double b, double c, double d, out Complex[] result)
        {
            b /= a; c /= a; d /= a;
            a = b; b = c; c = d;

            double Q = (a * a - 3 * b) / 9;
            double R = (2 * Math.Pow(a, 3) - 9 * a * b + 27 * c) / 54;
            double S = Math.Pow(Q, 3) - Math.Pow(R, 2);

            result = new Complex[3];
            if (S > 0)
            {
                //Console.WriteLine("Comment: S > 0 (Tested)");//TODO del
                Complex fi = Complex.Acos(R / Complex.Pow(Q, 3.0 / 2)) / 3;
                result[0] = -2 * Complex.Sqrt(Q) * Complex.Cos(fi) - a / 3;
                result[1] = -2 * Complex.Sqrt(Q) * Complex.Cos(fi + 2.0 / 3 * Math.PI) - a / 3;
                result[2] = -2 * Complex.Sqrt(Q) * Complex.Cos(fi - 2.0 / 3 * Math.PI) - a / 3;
            }
            else if (S < 0)
            {
                if (Q > 0)
                {
                    //Console.WriteLine("Comment: S < 0   Q > 0 (Tested)");//TODO del
                    Complex fi = Arch(Math.Abs(R) / Math.Pow(Q, 3.0 / 2)) / 3;
                    Complex tmValue1 = Math.Sign(R) * Math.Sqrt(Q) * Complex.Cosh(fi);
                    Complex tmValue2 = Complex.ImaginaryOne * Math.Sqrt(3) * 
                        Math.Sqrt(Q) * Complex.Sinh(fi);

                    result[0] = -2 * tmValue1 - a / 3;
                    result[1] = tmValue1 - a / 3 + tmValue2;
                    result[2] = tmValue1 - a / 3 - tmValue2;
                }
                else if (Q < 0)
                {
                    //Console.WriteLine("Comment: S < 0   Q < 0 (Tested)");//TODO del
                    Complex fi = Arsh(Math.Abs(R) / Math.Pow(Math.Abs(Q), 3.0 / 2)) / 3;
                    Complex tmValue1 = Math.Sign(R) * Math.Sqrt(Math.Abs(Q)) * Complex.Sinh(fi);
                    Complex tmValue2 = Complex.ImaginaryOne * Math.Sqrt(3) * 
                        Math.Sqrt(Math.Abs(Q)) * Complex.Cosh(fi);

                    result[0] = -2 * tmValue1 - a / 3;
                    result[1] = tmValue1 - a / 3 + tmValue2;
                    result[2] = tmValue1 - a / 3 - tmValue2;
                }
                else
                {
                    //Console.WriteLine("Comment: S < 0   Q = 0 (Tested)");//TODO del
                    double tmValue = c - Math.Pow(a, 3) / 27;
                    result[0] = -Math.Sign(tmValue) * Math.Pow(Math.Abs(tmValue), 1.0 / 3) - 
                        a / 3;
                    result[1] = -(a + result[0]) / 2 + Complex.ImaginaryOne / 2 *
                        Math.Sqrt(Complex.Abs((a - 3 * result[0]) * (a + result[0]) - 4 * b));
                    result[2] = -(a + result[0]) / 2 - Complex.ImaginaryOne / 2 *
                        Math.Sqrt(Complex.Abs((a - 3 * result[0]) * (a + result[0]) - 4 * b));
                }
            }
            else
            {
                //Console.WriteLine("Comment: S = 0 (Tested)");
                result[0] = -2 * Complex.Pow(R, 1.0 / 3) - a / 3;
                result[1] = Complex.Pow(R, 1.0 / 3) - a / 3;
                result[2] = result[1];
            }
        }

        static Complex Arsh(Complex value)
        {
            return Complex.Log(value + Complex.Sqrt(value * value + 1));
        }

        static Complex Arch(Complex value)
        {
            return Complex.Log(value + Complex.Sqrt(value * value - 1));
        }

        static void Calc(double a, double b, double c, double d, Complex[] x, out Complex[] res)
        {
            res = new Complex[x.Length];
            for (int i = 0; i < x.Length; ++i)
                res[i] = a * Complex.Pow(x[i], 3) + b * Complex.Pow(x[i], 2) + c * x[i] + d;
        }
    }
}
