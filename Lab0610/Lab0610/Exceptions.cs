using System;
using System.Collections.Generic;
using System.Text;

namespace Lab0610
{
    class CalculationException : Exception
    {
        public CalculationException(string message) : base(message) { }
    }

    class DimensionsException : Exception
    {
        public DimensionsException() : base("Dimensions of matrices are not equals.") { }
    }




}
