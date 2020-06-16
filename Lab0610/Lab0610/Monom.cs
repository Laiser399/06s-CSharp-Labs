using System;
using System.Collections.Generic;
using System.Text;

namespace Lab0610
{
    public class Monom<T> : IComparable<Monom<T>>, ICloneable
        where T : IMathFieldElement<T>, IComparable<T>, ICloneable
    {
        public T Coef { get; set; }
        public int Degree { get; set; }

        public Monom(T coef, int degree = 0)
        {
            Coef = coef;
            Degree = degree;
        }

        // IComparable
        public int CompareTo(Monom<T> other)
        {
            int compareRes = Degree.CompareTo(other.Degree);
            if (compareRes != 0)
                return compareRes;
            return Coef.CompareTo(other.Coef);
        }

        int IComparable<Monom<T>>.CompareTo(Monom<T> other) => CompareTo(other);

        // ICloneable
        public object Clone() => new Monom<T>((T)Coef.Clone(), Degree);

        // methods
        public override string ToString()
        {
            return $"({Coef}; {Degree})";
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj is Monom<T> another)
                return Coef.Equals(another.Coef) && Degree.Equals(another.Degree);
            else
                return false;
        }

        public void Multiply(Monom<T> another)
        {
            Coef.Multiply(another.Coef);
            Degree += another.Degree;
        }

        public void Divide(Monom<T> another)
        {
            Coef.Divide(another.Coef);
            Degree -= another.Degree;
        }

        public void InverseAdditive() => Coef.InverseAdditive();

        // operators
        public static implicit operator Monom<T>(T coef) => new Monom<T>(coef);

        public static Monom<T> operator -(Monom<T> monom) => Negate(monom);

        public static Monom<T> operator *(Monom<T> first, Monom<T> second) => Multiply(first, second);

        public static Monom<T> operator /(Monom<T> first, Monom<T> second) => Divide(first, second);

        // static methods
        public static Monom<T> Multiply(Monom<T> first, Monom<T> second)
        {
            Monom<T> result = (Monom<T>)first.Clone();
            result.Multiply(second);
            return result;
        }

        public static Monom<T> Divide(Monom<T> first, Monom<T> second)
        {
            Monom<T> result = (Monom<T>)first.Clone();
            result.Divide(second);
            return result;
        }

        public static Monom<T> Negate(Monom<T> monom)
        {
            Monom<T> result = (Monom<T>)monom.Clone();
            result.InverseAdditive();
            return result;
        }
    }

}
