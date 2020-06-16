using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Lab0610
{
    public class QuadMatrix : ICloneable, IEnumerable<double>, IMathFieldElement<QuadMatrix>,
        IComparable<QuadMatrix>
    {
        public int Dimension { get => _rows.Length; }
        private double[][] _rows;

        // constrcutors
        public QuadMatrix(int dimension, double mainDiagonalValue = 0)
        {
            if (dimension <= 0)
                throw new ArgumentException("Value of argument \"dimension\" <= 0.");

            _rows = new double[dimension][];
            for (int i = 0; i < dimension; ++i)
            {
                _rows[i] = new double[dimension];
                for (int j = 0; j < dimension; ++j)
                    _rows[i][j] = i == j ? mainDiagonalValue : 0;
            }
        }

        public QuadMatrix(QuadMatrix another)
        {
            _rows = new double[another.Dimension][];
            for (int i = 0; i < Dimension; ++i)
            {
                _rows[i] = new double[Dimension];
                for (int j = 0; j < Dimension; ++j)
                    _rows[i][j] = another._rows[i][j];
            }
        }

        public QuadMatrix(double[][] values)
        {
            if (values.Length == 0)
                throw new ArgumentException("Length of argument \"double[][] values\" is 0.");

            _rows = new double[values.Length][];
            for (int i = 0; i < Dimension; ++i)
            {
                if (values[i].Length != Dimension)
                    throw new ArgumentException("Wrong dimensions of argument " +
                        "\"double[][] values\".");
                
                _rows[i] = new double[Dimension];
                for (int j = 0; j < Dimension; ++j)
                    _rows[i][j] = values[i][j];
            }
        }

        // methods
        override public string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            for (int i = 0; i < _rows.Length; ++i)
            {
                if (i > 0)
                    builder.Append(", ");
                builder.Append("{");
                for (int j = 0; j < _rows[i].Length; ++j)
                {
                    if (j > 0)
                        builder.Append("; ");
                    builder.Append(_rows[i][j]);
                }
                builder.Append("}");
            }
            builder.Append("}");
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj is QuadMatrix another)
            {
                if (Dimension != another.Dimension)
                    return false;
                for (int i = 0; i < Dimension; ++i)
                    for (int j = 0; j < Dimension; ++j)
                        if (this[i, j] != another[i, j])
                            return false;
                return true;
            }
            else
                return false;
        }
        
        public double Determinant()
        {
            QuadMatrix copy = (QuadMatrix)Clone();
            double result = 1;
            for (int i = 0; i < copy.Dimension; ++i)
            {
                // для текущего столбца выставляем первым элементом ненулевой
                int notNullRow = copy.FirstNotNullRow(i, i);
                if (notNullRow == -1)
                    return 0;
                if (copy.SwapRows(i, notNullRow))
                    result = -result;

                // обнуляем элементы в текущем столбце ниже текущей строки, вычитая текущую строку из нижних строк
                for (int destroyRow = i + 1; destroyRow < copy.Dimension; ++destroyRow)
                {
                    double coef = copy[destroyRow, i] / copy[i, i];
                    copy[destroyRow, i] = 0;
                    for (int col = i + 1; col < copy.Dimension; ++col)
                        copy[destroyRow, col] -= coef * copy[i, col];
                }

                result *= copy[i, i];
            }

            return result;
        }

        private int FirstNotNullRow(int startRow, int column)
        {
            for (int i = startRow; i < Dimension; ++i)
                if (_rows[i][column] != 0) return i;
            return -1;
        }

        private bool SwapRows(int i1, int i2)
        {
            if (i1 == i2) return false;

            double[] tm = _rows[i1];
            _rows[i1] = _rows[i2];
            _rows[i2] = tm;
            return true;
        }

        public void Multiply(double another)
        {
            for (int i = 0; i < Dimension; ++i)
                for (int j = 0; j < Dimension; ++j)
                    this[i, j] *= another;
        }

        // ICloneable
        public object Clone() => new QuadMatrix(this);

        // IMathField
        public bool IsZero()
        {
            foreach (double[] row in _rows)
                foreach (double val in row)
                    if (val != 0) return false;
            return true;
        }

        public int CompareToZero()
        {
            return Determinant().CompareTo(0);
        }

        public void Add(QuadMatrix another)
        {
            if (Dimension != another.Dimension)
                throw new DimensionsException();

            for (int i = 0; i < Dimension; ++i)
                for (int j = 0; j < Dimension; ++j)
                    this[i, j] += another[i, j];
        }

        public void Subtract(QuadMatrix another)
        {
            if (Dimension != another.Dimension)
                throw new DimensionsException();

            for (int i = 0; i < Dimension; ++i)
                for (int j = 0; j < Dimension; ++j)
                    this[i, j] -= another[i, j];
        }

        public void InverseAdditive()
        {
            for (int i = 0; i < Dimension; ++i)
                for (int j = 0; j < Dimension; ++j)
                    this[i, j] = -this[i, j];
        }

        public void Multiply(QuadMatrix another)
        {
            if (Dimension != another.Dimension)
                throw new DimensionsException();

            QuadMatrix tmMatrix = new QuadMatrix(Dimension, 0);
            for (int row = 0; row < Dimension; ++row)
                for (int col = 0; col < Dimension; ++col)
                    for (int i = 0; i < Dimension; ++i)
                        tmMatrix[row, col] += this[row, i] * another[i, col];
            _rows = tmMatrix._rows;
        }

        public void Divide(QuadMatrix another) => Multiply(Inversed(another));

        // IEnumerable
        public IEnumerator<double> GetEnumerator() => new QuadMatrixEnumerator(_rows);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // IComparable
        public int CompareTo(QuadMatrix other)
        {
            double det1 = Determinant();
            double det2 = other.Determinant();
            if (det1 < det2)
                return -1;
            else if (det1 == det2)
                return 0;
            else
                return 1;
        }

        int IComparable<QuadMatrix>.CompareTo(QuadMatrix other)
        {
            return CompareTo(other);
        }

        // operators
        public double this[Index row, Index column]
        {
            get { return _rows[row][column]; }
            set { _rows[row][column] = value; }
        }

        public static QuadMatrix operator +(QuadMatrix first, QuadMatrix second)
        {
            return Add(first, second);
        }

        public static QuadMatrix operator -(QuadMatrix first, QuadMatrix second)
        {
            return Subtract(first, second);
        }

        public static QuadMatrix operator -(QuadMatrix matrix)
        {
            return Negate(matrix);
        }

        public static QuadMatrix operator *(QuadMatrix first, QuadMatrix second)
        {
            return Multiply(first, second);
        }

        public static QuadMatrix operator *(QuadMatrix first, double second)
        {
            return Multiply(first, second);
        }

        public static QuadMatrix operator *(double first, QuadMatrix second)
        {
            return Multiply(first, second);
        }

        public static QuadMatrix operator /(QuadMatrix first, QuadMatrix second)
        {
            return Divide(first, second);
        }

        public static QuadMatrix operator /(QuadMatrix mtx, double value)
        {
            if (value == 0)
                throw new CalculationException("Division by zero.");
            return mtx * (1.0 / value);
        }

        public static bool operator==(QuadMatrix first, QuadMatrix second)
        {
            return first.CompareTo(second) == 0;
        }

        public static bool operator!=(QuadMatrix first, QuadMatrix second)
        {
            return first.CompareTo(second) != 0;
        }

        public static bool operator <(QuadMatrix first, QuadMatrix second)
        {
            return first.CompareTo(second) == -1;
        }

        public static bool operator <=(QuadMatrix first, QuadMatrix second)
        {
            return first.CompareTo(second) <= 0;
        }

        public static bool operator >(QuadMatrix first, QuadMatrix second)
        {
            return first.CompareTo(second) == 1;
        }

        public static bool operator >=(QuadMatrix first, QuadMatrix second)
        {
            return first.CompareTo(second) >= 0;
        }

        // static methods
        public static QuadMatrix Inversed(QuadMatrix matrix)
        {
            double det = matrix.Determinant();
            if (det == 0)
                throw new CalculationException("Impossible to calculate inverse matrix: determinant = 0.");

            QuadMatrix result = new QuadMatrix(matrix.Dimension, 0);
            for (int i = 0; i < matrix.Dimension; ++i)
            {
                for (int j = 0; j < matrix.Dimension; ++j)
                {
                    result[i, j] = SubMatrix(matrix, j, i).Determinant() / det;
                    if ((i + j) % 2 == 1)
                        result[i, j] = -result[i, j];
                }
            }
            return result;
        }

        public static QuadMatrix Transposed(QuadMatrix maxtrix)
        {
            QuadMatrix result = new QuadMatrix(maxtrix.Dimension, 0);
            for (int i = 0; i < maxtrix.Dimension; ++i)
                for (int j = 0; j < maxtrix.Dimension; ++j)
                    result[i, j] = maxtrix[j, i];
            return result;
        }

        public static QuadMatrix SubMatrix(QuadMatrix matrix, int removedRow, int removedCol)
        {
            QuadMatrix result = new QuadMatrix(matrix.Dimension - 1);
            for (int i = 0; i < result.Dimension; ++i)
            {
                for (int j = 0; j < result.Dimension; ++j)
                {
                    int row = i >= removedRow ? i + 1 : i;
                    int col = j >= removedCol ? j + 1 : j;
                    result[i, j] = matrix[row, col];
                }
            }
            return result;
        }

        public static QuadMatrix Add(QuadMatrix first, QuadMatrix second)
        {
            QuadMatrix result = (QuadMatrix)first.Clone();
            result.Add(second);
            return result;
        }

        public static QuadMatrix Subtract(QuadMatrix first, QuadMatrix second)
        {
            QuadMatrix result = (QuadMatrix)first.Clone();
            result.Subtract(second);
            return result;
        }

        public static QuadMatrix Negate(QuadMatrix matrix)
        {
            QuadMatrix result = (QuadMatrix)matrix.Clone();
            result.InverseAdditive();
            return result;
        }

        public static QuadMatrix Multiply(QuadMatrix first, QuadMatrix second)
        {
            QuadMatrix result = (QuadMatrix)first.Clone();
            result.Multiply(second);
            return result;
        }

        public static QuadMatrix Multiply(QuadMatrix first, double second)
        {
            QuadMatrix result = (QuadMatrix)first.Clone();
            result.Multiply(second);
            return result;
        }

        public static QuadMatrix Multiply(double first, QuadMatrix second)
        {
            return Multiply(second, first);
        }

        public static QuadMatrix Divide(QuadMatrix first, QuadMatrix second)
        {
            QuadMatrix result = (QuadMatrix)first.Clone();
            result.Divide(second);
            return result;
        }

    }

    public class QuadMatrixEnumerator : IEnumerator<double>
    {
        private int _row = 0, _col = -1;
        private double[][] _rows;

        public QuadMatrixEnumerator(double[][] rows)
        {
            _rows = rows;
        }

        public bool MoveNext()
        {
            if (_col < _rows[_row].Length - 1)
            {
                ++_col;
                return true;
            }
            else if (_row < _rows.Length - 1)
            {
                _col = 0;
                ++_row;
                return true;
            }
            else
                return false;
        }

        public double Current { get => _rows[_row][_col]; }
        object IEnumerator.Current { get => Current; }

        public void Reset()
        {
            _row = 0;
            _col = -1;
        }

        public void Dispose() {}
    }


}
