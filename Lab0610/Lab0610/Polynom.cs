using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lab0610
{

    public class Polynom<T> : ICloneable, IEnumerable<Monom<T>>, IComparable<Polynom<T>>
        where T : IMathFieldElement<T>, IComparable<T>, ICloneable
    {
        public int Length { get => _monoms.Count; }
        public int Degree { get => _monoms.Count > 0 ? _monoms[^1].Degree : 0; }
        private List<Monom<T>> _monoms = new List<Monom<T>>();

        // constructors
        public Polynom() { }

        public Polynom(params T[] coefs)
        {
            for (int i = 0; i < coefs.Length; ++i)
                if (!coefs[i].IsZero())
                    _monoms.Add(new Monom<T>(coefs[i], i));
        }

        public Polynom(Dictionary<int, T> degreeToCoef)
        {
            foreach (var pair in degreeToCoef)
                Append(pair.Value, pair.Key);
        }

        // ICloneable
        public object Clone() {
            Polynom<T> result = new Polynom<T>();
            foreach (var monom in _monoms)
                result._monoms.Add((Monom<T>)monom.Clone());
            return result;
        }

        // IEnumerable
        public IEnumerator<Monom<T>> GetEnumerator() => new PolynomEnumerator<T>(_monoms);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // IComparable
        public int CompareTo(Polynom<T> other)
        {
            int i = 0;
            while (i < Length && i < other.Length)
            {
                ++i;
                if (this[^i].Degree == other[^i].Degree)
                {
                    int compareRes = this[^i].Coef.CompareTo(other[^i].Coef);
                    if (compareRes != 0)
                        return compareRes;
                }
                else if (this[^i].Degree > other[^i].Degree)
                    return this[^i].Coef.CompareToZero();
                else
                    return -other[^i].Coef.CompareToZero();
            }

            if (i < Length)
            {
                ++i;
                return this[^i].Coef.CompareToZero();
            }
            else if (i < other.Length)
            {
                ++i;
                return -other[^i].Coef.CompareToZero();
            }

            return 0;
        }

        int IComparable<Polynom<T>>.CompareTo(Polynom<T> other) => CompareTo(other);

        // methods
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append('(');
            for (int i = 0; i < _monoms.Count; ++i)
            {
                if (i > 0)
                    builder.Append(", ");
                builder.Append(_monoms[i].ToString());
            }
            builder.Append(')');
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj is Polynom<T> another)
            {
                // нельзя заменить на CompareTo из за этого
                if (Length != another.Length)
                    return false;
                for (int i = 0; i < Length; ++i)
                    if (!this[i].Equals(another[i]))
                        return false;
                return true;
            }
            else
                return false;
        }

        private void Append(Monom<T> monom) => Append(monom.Coef, monom.Degree);

        private void Append(T coef, int degree)
        {
            if (degree < 0)
                throw new ArgumentException("Degree of monom must be >= 0.");
            if (coef.IsZero())
                return;

            int index;
            if (FindIndexOf(degree, out index))
            {
                _monoms[index].Coef.Add(coef);
                if (_monoms[index].Coef.IsZero())
                    _monoms.RemoveAt(index);
            }
            else
                _monoms.Insert(index, new Monom<T>(coef, degree));
        }

        private void RemoveByDegree(int degree)
        {
            int index;
            if (FindIndexOf(degree, out index))
                _monoms.RemoveAt(index);
        }

        private bool FindIndexOf(int degree, out int index)
        {
            // если индекс найден, возвращает true, иначе - false и в index=insert_index

            if (Length == 0)
            {
                index = 0;
                return false;
            }
            else if (Length == 1)
            {
                if (degree < _monoms[0].Degree)
                {
                    index = 0;
                    return false;
                }
                else if (degree == _monoms[0].Degree)
                {
                    index = 0;
                    return true;
                }
                else
                {
                    index = 1;
                    return false;
                }
            }
            else
            {
                if (degree < _monoms[0].Degree)
                {
                    index = 0;
                    return false;
                }
                else if (degree > _monoms[^1].Degree)
                {
                    index = Length;
                    return false;
                }
                else
                {
                    int a = 0, b = Length - 1;
                    while (b - a > 1)
                    {
                        int c = (a + b) / 2;
                        if (degree >= _monoms[a].Degree && degree <= _monoms[c].Degree)
                            b = c;
                        else
                            a = c;
                    }

                    if (degree == _monoms[a].Degree)
                    {
                        index = a;
                        return true;
                    }
                    else if (degree == _monoms[b].Degree)
                    {
                        index = b;
                        return true;
                    }
                    else
                    {
                        index = b;
                        return false;
                    }
                }
            }
        }

        private void RemoveByIndex(Index index)
        {
            int removeIndex = index.IsFromEnd ? _monoms.Count - index.Value : index.Value;
            _monoms.RemoveAt(removeIndex);
        }

        public T Calculate(T x)
        {
            if (Length == 0)
                throw new CalculationException("Polynom is empty.");

            T degreeOfX = (T)x.Clone();
            int degree = 1;

            // требуется сделать одну итерацию, чтобы определить начальное значение result
            T result = (T)_monoms[0].Coef.Clone();
            if (_monoms[0].Degree > 0)
            {
                while (degree < _monoms[0].Degree)
                {
                    degreeOfX.Multiply(x);
                    ++degree;
                }
                result.Multiply(degreeOfX);
            }

            for (int i = 1; i < Length; ++i)
            {
                while (degree < _monoms[i].Degree)
                {
                    degreeOfX.Multiply(x);
                    ++degree;
                }
                T tmValue = (T)_monoms[i].Coef.Clone();
                tmValue.Multiply(degreeOfX);
                result.Add(tmValue);
            }
            return result;
        }

        // operations
        public void Add(Polynom<T> another)
        {
            foreach (var monom in another)
                Append((T)monom.Coef.Clone(), monom.Degree);
        }

        public void Subtract(Polynom<T> another)
        {
            foreach (var monom in another)
            {
                T newCoef = (T)monom.Coef.Clone();
                newCoef.InverseAdditive();
                Append(newCoef, monom.Degree);
            }
        }

        public void InverseAdditive()
        {
            foreach (var monom in _monoms)
                monom.InverseAdditive();
        }

        public void Multiply(Polynom<T> another) => _monoms = Multiply(this, another)._monoms;

        // operators
        public Monom<T> this[Index i]
        {
            get { return _monoms[i]; }
        }

        public static implicit operator Polynom<T>(Monom<T> monom)
        {
            Polynom<T> result = new Polynom<T>();
            result.Append((T)monom.Coef.Clone(), monom.Degree);
            return result;
        }

        public static implicit operator Polynom<T>(T coef) => new Polynom<T>((T)coef.Clone());

        public static Polynom<T> operator +(Polynom<T> first, Polynom<T> second) => Add(first, second);

        public static Polynom<T> operator -(Polynom<T> first, Polynom<T> second) => Subtract(first, second);

        public static Polynom<T> operator -(Polynom<T> poly) => Negate(poly);

        public static Polynom<T> operator *(Polynom<T> first, Polynom<T> second) => Multiply(first, second);

        public static Polynom<T> operator /(Polynom<T> first, Polynom<T> second)
        {
            Polynom<T> div, mod;
            Divide(first, second, out div, out mod);
            return div;
        }

        public static Polynom<T> operator %(Polynom<T> first, Polynom<T> second)
        {
            Polynom<T> div, mod;
            Divide(first, second, out div, out mod);
            return mod;
        }

        public static bool operator ==(Polynom<T> first, Polynom<T> second) => first.Equals(second);

        public static bool operator !=(Polynom<T> first, Polynom<T> second) => !first.Equals(second);

        public static bool operator <(Polynom<T> first, Polynom<T> second) => first.CompareTo(second) == -1;

        public static bool operator <=(Polynom<T> first, Polynom<T> second) => first.CompareTo(second) <= 0;

        public static bool operator >(Polynom<T> first, Polynom<T> second) => first.CompareTo(second) == 1;

        public static bool operator >=(Polynom<T> first, Polynom<T> second) => first.CompareTo(second) >= 0;


        // static methods
        public static Polynom<T> Add(Polynom<T> first, Polynom<T> second)
        {
            Polynom<T> result = (Polynom<T>)first.Clone();
            result.Add(second);
            return result;
        }

        public static Polynom<T> Subtract(Polynom<T> first, Polynom<T> second)
        {
            Polynom<T> result = (Polynom<T>)first.Clone();
            result.Subtract(second);
            return result;
        }

        public static Polynom<T> Negate(Polynom<T> poly)
        {
            Polynom<T> result = (Polynom<T>)poly.Clone();
            result.InverseAdditive();
            return result;
        }

        public static Polynom<T> Multiply(Polynom<T> first, Polynom<T> second)
        {
            Polynom<T> result = new Polynom<T>();
            foreach (var monom1 in first)
                foreach (var monom2 in second)
                    result.Append(monom1 * monom2);
            return result;
        }
    
        public static void Divide(Polynom<T> first, Polynom<T> second, out Polynom<T> div, out Polynom<T> mod)
        {
            if (second.Length == 0)
                throw new CalculationException("Division by zero.");

            div = new Polynom<T>();
            if (first.Length == 0)
            {
                mod = new Polynom<T>();
                return;
            }

            Polynom<T> copy = (Polynom<T>)first.Clone();
            while (copy.Degree >= second.Degree && copy.Length > 0)
            {
                Monom<T> coef = copy[^1] / second[^1];
                div.Append(coef);

                Monom<T> saved = second[^1];
                second.RemoveByIndex(^1);
                copy.RemoveByIndex(^1);
                copy.Subtract(Multiply(coef, second));
                second.Append(saved);
            }
            mod = copy;
        }
    
        public static Polynom<T> Composition(Polynom<T> f_x, Polynom<T> x)
        {
            Polynom<T> result = new Polynom<T>();
            Polynom<T> degreeOfX = (Polynom<T>)x.Clone();
            int degree = 1;
            foreach (var monomF_x in f_x)
            {
                while (degree < monomF_x.Degree)
                {
                    degreeOfX.Multiply(x);
                    ++degree;
                }

                if (monomF_x.Degree == 0)
                    result.Append((Monom<T>)monomF_x.Clone());
                else
                    foreach (var monomDegreeOfX in degreeOfX)
                        result.Append(Monom<T>.Multiply(monomF_x.Coef, monomDegreeOfX));
            }
            return result;
        }
    
    
    }

    public class PolynomEnumerator<T> : IEnumerator<Monom<T>>
        where T : IMathFieldElement<T>, IComparable<T>, ICloneable
    {
        private int _index = -1;
        private List<Monom<T>> _monoms;

        public Monom<T> Current => _monoms[_index];
        object IEnumerator.Current => _monoms[_index];

        public PolynomEnumerator(List<Monom<T>> monoms)
        {
            _monoms = monoms;
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            return ++_index < _monoms.Count;
        }

        public void Reset()
        {
            _index = -1;
        }
    }

    

}
