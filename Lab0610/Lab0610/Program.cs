using System;
using System.Collections.Generic;
using System.Numerics;

namespace Lab0610
{
    public class Double1: IMathFieldElement<Double1>, IComparable<Double1>, ICloneable
    {
        public double Value { get; private set; }
        public Double1() { }

        public Double1(double value)
        {
            Value = value;
        }

        public object Clone() => new Double1(Value);

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj is Double1 another)
                return Value.Equals(another.Value);
            else
                return false;
        }

        public static implicit operator Double1(double value)
        {
            return new Double1(value);
        }

        public bool IsZero() => Value == 0.0;
        public int CompareToZero() => Value.CompareTo(0);
        public void Add(Double1 another) => Value += another.Value;
        public void Subtract(Double1 another) => Value -= another.Value;
        public void InverseAdditive() => Value = -Value;
        public void Multiply(Double1 another) => Value *= another.Value;
        public void Divide(Double1 another) => Value /= another.Value;

        int IComparable<Double1>.CompareTo(Double1 other)
        {
            if (Value < other.Value)
                return -1;
            else if (Value == other.Value)
                return 0;
            else
                return 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Polynom<Double1> poly1 = new Polynom<Double1>(new Dictionary<int, Double1>
            //{
            //
            //});


            Polynom<QuadMatrix> poly1 = new Polynom<QuadMatrix>(new Dictionary<int, QuadMatrix>
            { 
                [1] = new QuadMatrix(new double[][]
                {
                    new double[] {1, 2},
                    new double[] {0, -2},
                }),
                [0] = new QuadMatrix(new double[][]
                {
                    new double[] {3, -1},
                    new double[] {1, 2},
                }),
            });
            Polynom<QuadMatrix> poly2 = new Polynom<QuadMatrix>(new Dictionary<int, QuadMatrix>
            {
                [1] = new QuadMatrix(new double[][]
                {
                    new double[] {1, 0},
                    new double[] {0, 1},
                }),
                [0] = new QuadMatrix(new double[][]
                {
                    new double[] {-4, 0},
                    new double[] {1, 3},
                }),
            });
            QuadMatrix x = new QuadMatrix(new double[][]
            {
                new double[] {1, 0},
                new double[] {0, 2}
            });
            Console.WriteLine($"poly1: {poly1}");
            Console.WriteLine($"poly2: {poly2}");
            Console.WriteLine($"x: {x}");
            Console.WriteLine($"poly1 + poly2 = {poly1 + poly2}");
            Console.WriteLine($"poly1 * poly2 = {poly1 * poly2}");
            Console.WriteLine($"poly1(x) = {poly1.Calculate(x)}");
            Polynom<QuadMatrix>.Divide(poly1, poly2, out var div, out var mod);
            Console.WriteLine($"div = poly1 / poly2 = {div}");
            Console.WriteLine($"mod = poly1 % poly2 = {mod}");
            Console.WriteLine($"div * poly2 + mod = {div * poly2 + mod} = poly1");
        }

        

        
        
    }
}
