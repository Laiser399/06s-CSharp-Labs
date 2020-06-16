using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lab0614.Binary.FormulaElements
{
    public class UnaryOp : Operation
    {
        private Func<BitArray, BitArray> _func;

        private UnaryOp(string[] stringImages, Func<BitArray, BitArray> func)
            : base(stringImages)
        {
            _func = func;
        }

        public BitArray Calc(BitArray bits)
        {
            return _func.Invoke(bits);
        }

        public override string ToString()
        {
            string stringImage = _stringImages.Length > 0 ? _stringImages[0] : "";
            return $"UnaryOp({stringImage})";
        }

        // static components
        public static UnaryOp Inversion
        {
            get;
            private set;
        } = new UnaryOp(new string[] { "!", "not" }, bits => new BitArray(bits).Not());

    }
}
