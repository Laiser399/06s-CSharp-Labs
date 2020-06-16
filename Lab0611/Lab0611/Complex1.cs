using System;
using System.Collections.Generic;
using System.Text;

namespace Lab0611
{
    public class Complex1 : ISqrtable<Complex1>, IMathFieldElement<Complex1>, IComparable<Complex1>
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }
        public double Magnitude { get => CalcMagnitude(); }
        public double Phase { get => CalcPhase(); }

        public Complex1() : this(0, 0) { }

        public Complex1(double real, double imaginary = 0)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public Complex1(Complex1 another)
        {
            Real = another.Real;
            Imaginary = another.Imaginary;
        }
        
        // ISqrtable
        public Complex1 Sqrt() => Pow(0.5);
        
        // IMathFieldElement
        public bool IsZero() => Real == 0 && Imaginary == 0;

        public Complex1 Abs() => Magnitude;

        public void Add(Complex1 another)
        {
            Real += another.Real;
            Imaginary += another.Imaginary;
        }
        
        public void Subtract(Complex1 another)
        {
            Real -= another.Real;
            Imaginary -= another.Imaginary;
        }

        public void InverseAdditive()
        {
            Real = -Real;
            Imaginary = -Imaginary;
        }

        public void Multiply(Complex1 another)
        {
            double real = Real, imaginary = Imaginary;
            Real = real * another.Real - imaginary * another.Imaginary;
            Imaginary = real * another.Imaginary + imaginary * another.Real;
        }

        public void Divide(Complex1 another)
        {
            double denominator = Math.Pow(another.Real, 2) + Math.Pow(another.Imaginary, 2);
            if (denominator == 0)
            {
                ZeroDivisionEvent?.Invoke(new DivisionByZeroEventArgs(this, another));
                //throw new Exception("Division by zero."); // TODO event gen
            }

            double real = Real, imaginary = Imaginary;
            Real = (real * another.Real + imaginary * another.Imaginary) / denominator;
            Imaginary = (imaginary * another.Real - real * another.Imaginary) / denominator;
        }

        // ICloneable
        public object Clone() => new Complex1(Real, Imaginary);

        // IComparable
        public int CompareTo(Complex1 another) => Magnitude.CompareTo(another.Magnitude);

        // methods
        private double CalcMagnitude()
        {
            return Math.Sqrt(Real * Real + Imaginary * Imaginary);
        }

        private double CalcPhase()
        {
            double len = Magnitude;
            if (len == 0) return 0;
            return Math.Atan2(Imaginary / len, Real / len);
        }

        public Complex1 Pow(double a)
        {
            return FromPolarCoordinates(Math.Pow(Magnitude, a), Phase * a);
        }

        public override string ToString() => $"({Real}; {Imaginary})";

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj is Complex1 another)
                return Real.Equals(another.Real) && Imaginary.Equals(another.Imaginary);
            else
                return false;
        }

        // operators
        public static implicit operator Complex1(double value) => new Complex1(value);

        public static Complex1 operator +(Complex1 first, Complex1 second) => Add(first, second);

        public static Complex1 operator -(Complex1 first, Complex1 second) => Subtract(first, second);

        public static Complex1 operator *(Complex1 first, Complex1 second) => Multiply(first, second);

        public static Complex1 operator /(Complex1 first, Complex1 second) => Divide(first, second);

        // static methods
        public static event Action<DivisionByZeroEventArgs> ZeroDivisionEvent;

        public static Complex1 FromPolarCoordinates(double magnitude, double phase)
        {
            return new Complex1(magnitude * Math.Cos(phase), magnitude * Math.Sin(phase));
        }

        public static Complex1 Add(Complex1 first, Complex1 second)
        {
            return new Complex1(first.Real + second.Real, 
                first.Imaginary + second.Imaginary);
        }
        
        public static Complex1 Subtract(Complex1 first, Complex1 second)
        {
            return new Complex1(first.Real - second.Real,
                first.Imaginary - second.Imaginary);
        }
        
        public static Complex1 Multiply(Complex1 first, Complex1 second)
        {
            return new Complex1(first.Real * second.Real - first.Imaginary * second.Imaginary,
                first.Real * second.Imaginary + first.Imaginary * second.Real);
        }

        public static Complex1 Divide(Complex1 first, Complex1 second)
        {
            double denominator = Math.Pow(second.Real, 2) + Math.Pow(second.Imaginary, 2);
            if (denominator == 0)
            {
                ZeroDivisionEvent?.Invoke(new DivisionByZeroEventArgs(first, second));
                return 0;
                //throw new Exception("Division by zero."); // TODO think about exception
            }

            return new Complex1((first.Real * second.Real + first.Imaginary * second.Imaginary) / denominator,
                (first.Imaginary * second.Real - first.Real * second.Imaginary) / denominator);
        }

        
    }

    public class DivisionByZeroEventArgs : EventArgs
    {
        public object First { get; private set; }
        public object Second { get; private set; }

        public DivisionByZeroEventArgs(Complex1 first, Complex1 second)
        {
            First = first;
            Second = second;
        }
    }

}
