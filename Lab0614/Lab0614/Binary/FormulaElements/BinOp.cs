using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lab0614.Binary.FormulaElements
{
    public class BinOp : Operation
    {
        private Func<BitArray, BitArray, BitArray> _func;
        /// <summary>
        /// меньшее значение соответствует большему приоритету
        /// </summary>
        public int Priority
        {
            get;
            private set;
        }

        private BinOp(string[] stringImages, Func<BitArray, BitArray, BitArray> func, int priority)
            : base(stringImages)
        {
            _func = func;
            Priority = priority;
        }

        public BitArray Calc(BitArray first, BitArray second)
        {
            return _func.Invoke(first, second);
        }

        public override string ToString()
        {
            string stringImage = _stringImages.Length > 0 ? _stringImages[0] : "";
            return $"BinOp({stringImage})";
        }

        // static components
        public static BinOp Conjunction
        {
            get;
            private set;
        } = new BinOp(new string[] { "&", "and" }, (first, second) => new BitArray(first).And(second), 2);
        public static BinOp Disjunction
        {
            get;
            private set;
        } = new BinOp(new string[] { "|", "or" }, (first, second) => new BitArray(first).Or(second), 3);
        public static BinOp Xor
        {
            get;
            private set;
        } = new BinOp(new string[] { "xor" }, (first, second) => new BitArray(first).Xor(second), 4);
        public static BinOp Implication
        {
            get;
            private set;
        } = new BinOp(new string[] { "->" }, (first, second) => new BitArray(first).Not().Or(second), 5);
        public static BinOp Equivalent
        {
            get;
            private set;
        } = new BinOp(new string[] { "=" }, (first, second) => first.EquivalentClone(second), 6);

    }
}
