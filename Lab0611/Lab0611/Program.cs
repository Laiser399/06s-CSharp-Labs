using System;
using System.Numerics;

namespace Lab0611
{
    public class Double1 : ISqrtable<Double1>, IMathFieldElement<Double1>, IComparable<Double1>
    {
        public double Value { get; private set; }

        public Double1() : this(0)
        { }

        public Double1(double value)
        {
            Value = value;
        }

        public static implicit operator Double1(double value)
        {
            return new Double1(value);
        }

        public override string ToString() => Value.ToString();
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj is Double1 another)
                return Value.Equals(another.Value);
            else
                return false;
        }
        public int CompareTo(Double1 another) => Value.CompareTo(another.Value);
        public object Clone() => new Double1(Value);
        public Double1 Sqrt() => Math.Sqrt(Value);
        public void Multiply(Double1 another) => Value *= another.Value;
        public void Divide(Double1 another) => Value /= another.Value;
        public void Add(Double1 another) => Value += another.Value;
        public void Subtract(Double1 another) => Value -= another.Value;
        public void InverseAdditive() => Value = -Value;
        public bool IsZero() => Value == 0.0;
        public Double1 Abs() => Math.Abs(Value);
        
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Vector<Complex1>");

            Vector<Complex1> v1 = Vector<Complex1>.FromValues(new Complex1(1, 1), new Complex1(-2, 3));
            Vector<Complex1> v2 = Vector<Complex1>.FromValues(new Complex1(0, 2), new Complex1(-3, 1));
            Complex1 coef = new Complex1(2, -1);
            Console.WriteLine($"v1 = {v1}");
            Console.WriteLine($"v2 = {v2}");
            Console.WriteLine($"coef = {coef}");

            Console.WriteLine($"v1 + v2 = {v1 + v2}");
            Console.WriteLine($"v1 - v2 = {v1 - v2}");
            Console.WriteLine($"v1 * coef = {v1 * coef}");
            Console.WriteLine($"(v1, v2) = {v1.ScalarMultiply(v2)}");
            Console.WriteLine($"|v1| = {v1.Abs()}");




            //TestComplex1();
            //TestVector();
            //TestVectorComplex1();
        }

        static void TestComplex1()
        {
            Console.WriteLine("Complex1");

            Complex1 val1 = new Complex1(-1, 1);
            Complex1 val2 = new Complex1(3, 2);
            Console.WriteLine($"val1 = {val1}");
            Console.WriteLine($"val2 = {val2}");

            Console.WriteLine($"val1 Magnitude = {val1.Magnitude}");
            Console.WriteLine($"val1 Phase = {val1.Phase}");
            Console.WriteLine($"val1 + val2 = {val1 + val2}");
            Console.WriteLine($"val1 - val2 = {val1 - val2}");
            Console.WriteLine($"val1 * val2 = {val1 * val2}");
            Console.WriteLine($"val1 / val2 = {val1 / val2}");

            Console.WriteLine();
        }

        static void TestVector()
        {
            Console.WriteLine("Vector<Double1>");

            Vector<Double1> vec1 = Vector<Double1>.FromValues(1, 2, 3);
            Vector<Double1> vec2 = Vector<Double1>.FromValues(0, 3, -5);
            Vector<Double1> vec3 = Vector<Double1>.FromValues(-1, 6, -2);
            Console.WriteLine($"vec1 = {vec1}");
            Console.WriteLine($"vec2 = {vec2}");
            Console.WriteLine($"vec3 = {vec3}");

            Console.WriteLine($"vec1 + vec2 = {vec1 + vec2}");
            Console.WriteLine($"vec1 - vec2 = {vec1 - vec2}");
            Console.WriteLine($"Scalar multiply vec1 * vec2 = {vec1.ScalarMultiply(vec2)}");
            Vector<Double1>[] res = Vector<Double1>.Orthogonalize(vec1, vec2, vec3);
            Console.WriteLine("Ortogonalized:");
            foreach (var vec in res)
                Console.WriteLine($"   {vec}");

            Console.WriteLine();
        }

        static void TestVectorComplex1()
        {
            Console.WriteLine("Vector<Complex1>");

            Vector<Complex1> vec1 = Vector<Complex1>.FromValues(new Complex1(1, 1), new Complex1(-2, 3), new Complex1(6, 0));
            Vector<Complex1> vec2 = Vector<Complex1>.FromValues(new Complex1(0, 2), new Complex1(-3, 1), new Complex1(2, 2));
            Complex1 coef = new Complex1(2, -1);
            Console.WriteLine($"vec1 = {vec1}");
            Console.WriteLine($"vec2 = {vec2}");
            Console.WriteLine($"coef = {coef}");

            Console.WriteLine($"vec1 + vec2 = {vec1 + vec2}");
            Console.WriteLine($"vec1 - vec2 = {vec1 - vec2}");
            Console.WriteLine($"vec1 * coef = {vec1 * coef}");
            //Console.WriteLine($"vec1 + vec2 = {Vector<Complex1>.Add(vec1, vec2)}");
            Console.WriteLine($"(vec1, vec2) = {vec1.ScalarMultiply(vec2)}");

            Console.WriteLine();
        }
    }
}
