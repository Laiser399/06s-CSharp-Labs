using System;
using System.Collections.Generic;
using System.Text;

namespace Lab0611
{


    public interface IMathFieldElement<T> : ICloneable
    {
        public bool IsZero();
        public T Abs();

        public void Add(T another);
        public void Subtract(T another);
        public void InverseAdditive();
        public void Multiply(T another);
        public void Divide(T another);
    }

    public interface ISqrtable<T>
    {
        public T Sqrt();
    }

    
}
