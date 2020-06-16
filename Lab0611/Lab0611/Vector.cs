using System;
using System.Collections.Generic;
using System.Text;

namespace Lab0611
{
    public class Vector<T> : ICloneable, IComparable<Vector<T>>
        where T : ISqrtable<T>, IMathFieldElement<T>, IComparable<T>, new()
    {
        public int Length { get => _values.Length; }
        private T[] _values;

        public Vector(int length) : this(length, new T())
        { }

        public Vector(int length, T defaultValue)
        {
            if (length < 1)
                throw new CreationException($"Length of vector = {length}. Must be > 0.");

            _values = new T[length];
            for (int i = 0; i < Length; ++i)
                _values[i] = (T)defaultValue.Clone();
        }

        private Vector(params T[] values)
        {
            if (values.Length < 1)
                throw new CreationException($"Length of vector = {values.Length}. Must be > 0.");

            _values = new T[values.Length];
            Array.Copy(values, _values, Length);
        }

        // ICloneable
        public object Clone()
        {
            Vector<T> result = new Vector<T>(Length);
            for (int i = 0; i < result.Length; ++i)
                result[i] = (T)this[i].Clone();
            return result;
        }

        // IComparable
        public int CompareTo(Vector<T> another) => Abs().CompareTo(another.Abs());

        // methods
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("(");
            for (int i = 0; i < Length; ++i)
            {
                if (i > 0)
                    builder.Append("; ");
                builder.Append(_values[i]);
            }
            builder.Append(")");
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj is Vector<T> another)
            {
                if (Length != another.Length)
                    return false;
                for (int i = 0; i < Length; ++i)
                    if (!_values[i].Equals(another._values[i]))
                        return false;
                return true;
            }
            else
                return false;
        }

        public bool IsZero()
        {
            foreach (var value in _values)
                if (!value.IsZero())
                    return false;
            return true;
        }

        public T Abs()
        {
            T result = (T)_values[0].Clone();
            result.Multiply(_values[0]);

            for (int i = 1; i < Length; ++i)
            {
                T addValue = (T)_values[i].Clone();
                addValue.Multiply(_values[i]);
                result.Add(addValue);
            }
            return result.Sqrt().Abs();
        }

        public void Normalize()
        {
            T coef = Abs();
            if (coef.IsZero())
                return;
            foreach (var value in _values)
                value.Divide(coef);
        }

        public T ScalarMultiply(Vector<T> another)
        {
            if (Length != another.Length)
                throw new DifferentLengthException<T>(this, another);

            T result = (T)this[0].Clone();
            result.Multiply(another[0]);

            for (int i = 1; i < Length; ++i)
            {
                T addValue = (T)this[i].Clone();
                addValue.Multiply(another[i]);
                result.Add(addValue);
            }
            return result;
        }

        public void Multiply(T coef)
        {
            foreach (var value in _values)
                value.Multiply(coef);
        }

        public void Add(Vector<T> another)
        {
            if (Length != another.Length)
                throw new Exception("Length of vectors are not equals.");

            for (int i = 0; i < Length; ++i)
                this[i].Add(another[i]);
        }

        public void Subtract(Vector<T> another)
        {
            if (Length != another.Length)
                throw new Exception("Length of vectors are not equals.");

            for (int i = 0; i < Length; ++i)
                this[i].Subtract(another[i]);
        }

        public void InverseAdditive()
        {
            foreach (var value in _values)
                value.InverseAdditive();
        }
        
        // operators
        public T this[Index i]
        {
            get { return _values[i]; }
            set { _values[i] = value; }
        }

        public static Vector<T> operator *(Vector<T> first, T second) => Multiply(first, second);

        public static Vector<T> operator *(T first, Vector<T> second) => Multiply(first, second);

        public static Vector<T> operator +(Vector<T> first, Vector<T> second) => Add(first, second);

        public static Vector<T> operator -(Vector<T> first, Vector<T> second) => Subtract(first, second);

        public static Vector<T> operator -(Vector<T> vector) => Negate(vector);

        public static bool operator ==(Vector<T> first, Vector<T> second) => first.Equals(second);

        public static bool operator !=(Vector<T> first, Vector<T> second) => !first.Equals(second);

        public static bool operator >(Vector<T> first, Vector<T> second) => first.CompareTo(second) > 0;

        public static bool operator >=(Vector<T> first, Vector<T> second) => first.CompareTo(second) >= 0;

        public static bool operator <(Vector<T> first, Vector<T> second) => first.CompareTo(second) < 0;

        public static bool operator <=(Vector<T> first, Vector<T> second) => first.CompareTo(second) <= 0;

        // static methods
        public static Vector<T> Normalized(Vector<T> vector)
        {
            Vector<T> result = (Vector<T>)vector.Clone();
            result.Normalize();
            return result;
        }

        public static Vector<T>[] Orthogonalize(params Vector<T>[] vectors)
        {
            List<Vector<T>> result = new List<Vector<T>>();
            foreach (var vector in vectors)
            {
                if (vector.IsZero())
                    continue;

                Vector<T> nextVector = vector;
                foreach (var ortogonilized in result)
                    nextVector.Subtract(Proj(vector, ortogonilized));
                if (!nextVector.IsZero())
                    result.Add(nextVector);
            }
            return result.ToArray();
        }

        private static Vector<T> Proj(Vector<T> a, Vector<T> _b)
        {
            T coef = a.ScalarMultiply(_b);
            coef.Divide(_b.ScalarMultiply(_b));
            return Multiply(coef, _b);
        }

        public static Vector<T> FromArray(T[] values) => new Vector<T>(values);

        public static T[] ToArray(Vector<T> vector)
        {
            T[] result = new T[vector.Length];
            for (int i = 0; i < vector.Length; ++i)
                result[i] = (T)vector[i].Clone();
            return result;
        }

        public static Vector<T> FromValues(params T[] values) => new Vector<T>(values);

        public static Vector<T> VectorMultiply3D(Vector<T> first, Vector<T> second)
        {
            // вычисление через определитель матрицы
            if (first.Length != 3 || second.Length != 3)
                throw new WrongLengthException("Wrong length of vectors for vector multiply. It must be 3.");

            T x = CalcCoefVectorMultiply3D(first, second, 1, 2);
            T y = CalcCoefVectorMultiply3D(first, second, 0, 2, true);
            T z = CalcCoefVectorMultiply3D(first, second, 0, 1);

            return FromValues(x, y, z);
        }

        private static T CalcCoefVectorMultiply3D(Vector<T> first, Vector<T> second, 
            int index1, int index2, bool inverse = false)
        {
            T result = (T)first[index1].Clone();
            result.Multiply(second[index2]);
            T tmVal = (T)second[index1].Clone();
            tmVal.Multiply(first[index2]);
            result.Subtract(tmVal);
            if (inverse)
                result.InverseAdditive();
            return result;
        }


        
        public static Vector<T> Multiply(Vector<T> vector, T coef)
        {
            Vector<T> result = (Vector<T>)vector.Clone();
            result.Multiply(coef);
            return result;
        }

        public static Vector<T> Multiply(T coef, Vector<T> vector)
        {
            Vector<T> result = new Vector<T>(vector.Length, coef);
            for (int i = 0; i < vector.Length; ++i)
                result[i].Multiply(vector[i]);
            return result;
        }

        public static Vector<T> Add(Vector<T> first, Vector<T> second)
        {
            Vector<T> result = (Vector<T>)first.Clone();
            result.Add(second);
            return result;
        }

        public static Vector<T> Subtract(Vector<T> first, Vector<T> second)
        {
            Vector<T> result = (Vector<T>)first.Clone();
            result.Subtract(second);
            return result;
        }

        public static Vector<T> Negate(Vector<T> vector)
        {
            Vector<T> result = (Vector<T>)vector.Clone();
            result.InverseAdditive();
            return result;
        }






    }


    // Exceptions
    public class CreationException : Exception
    {
        public CreationException(string message) : base(message) { }
    }

    public class DifferentLengthException<T> : Exception
        where T : ISqrtable<T>, IMathFieldElement<T>, IComparable<T>, new()
    {
        public Vector<T> First { get; private set; }
        public Vector<T> Second { get; private set; }

        public DifferentLengthException(Vector<T> first, Vector<T> second) : 
            base($"Length of vectors {first}, {second} are different.")
        {
            First = first;
            Second = second;
        }
    }


    public class WrongLengthException : Exception
    {
        public WrongLengthException(string message) : base(message)
        {

        }
    }



}
