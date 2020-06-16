using System;
using System.Collections.Generic;
using System.Text;

namespace Lab0610
{

    public interface IMathFieldElement<T>
    {
        public bool IsZero();
        public int CompareToZero();

        public void Add(T another);
        public void Subtract(T another);
        public void InverseAdditive();
        public void Multiply(T another);
        public void Divide(T another);
    }

}
