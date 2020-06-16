using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lab0614
{
    public static class BitArrayEx
    {
        public static BitArray EquivalentClone(this BitArray first, BitArray second)
        {
            if (first.Length != second.Length)
                throw new ArgumentException("Array lengths must be the same");

            BitArray result = new BitArray(first.Length);
            for (int i = 0; i < first.Length; ++i)
                result[i] = first[i] == second[i];
            return result;
        }

        public static BitArray From(params bool[] values)
        {
            return new BitArray(values);
        }

        public static bool EqualsEx(this BitArray first, BitArray second)
        {
            if (first.Length != second.Length)
                return false;

            for (int i = 0; i < first.Length; ++i)
                if (first[i] != second[i])
                    return false;
            return true;
        }

        public static string ToStringEx(this BitArray bitArray)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("BitArray(");
            foreach (bool bit in bitArray)
                builder.Append(bit ? "1" : "0");
            builder.Append(")");
            return builder.ToString();
        }
    }
}
