using Lab0614.Binary.FormulaElements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace Lab0614.Binary
{
    public class Formula
    {
        private FormulaElement[] _elements;
        public Variable[] Variables
        {
            get;
            private set;
        }

        public Formula(params FormulaElement[] elements)
        {
            if (!IsValid(elements))
                throw new ArgumentException("Invalid postfix formula.");

            _elements = elements;
            InitVariables();
        }

        private void InitVariables()
        {
            List<Variable> tm = new List<Variable>();
            foreach (var element in _elements)
                if (element is Variable variable && !tm.Contains(variable))
                    tm.Add(variable);
            Variables = tm.ToArray();
        }

        public BitArray Calc(Dictionary<Variable, BitArray> args)
        {
            Stack<BitArray> calcStack = new Stack<BitArray>();
            foreach (FormulaElement element in _elements)
            {
                BitArray res;
                switch (element)
                {
                    case Variable variable:
                        if (!args.ContainsKey(variable))
                            throw new ArgumentException($"Not found variable {variable.Name}.");
                        calcStack.Push(args.GetValueOrDefault(variable));
                        break;
                    case UnaryOp unaryOp:
                        res = unaryOp.Calc(calcStack.Pop());
                        calcStack.Push(res);
                        break;
                    case BinOp binOp:
                        BitArray second = calcStack.Pop();
                        res = binOp.Calc(calcStack.Pop(), second);
                        calcStack.Push(res);
                        break;
                }
            }


            // TODO del later
            Console.WriteLine("Truth table:");
            BitArray result = calcStack.Pop();
            foreach (var pair in args)
                Console.WriteLine($"\t{pair.Key}: {pair.Value.ToStringEx()}");
            Console.WriteLine($"\t{result.ToStringEx()}");

            return result;
        }

        public BitArray Calc(Dictionary<string, BitArray> args)
        {
            var newArgs = new Dictionary<Variable, BitArray>();
            foreach (var pair in args)
                newArgs.Add(new Variable(pair.Key), pair.Value);
            return Calc(newArgs);
        }

        private string MakePDNF(BitArray[] args, BitArray calcResult)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < calcResult.Length; ++i)
            {
                if (calcResult[i])
                {
                    if (builder.Length > 0)
                        builder.Append($" {BinOp.Disjunction.FirstStringImage} ");
                    builder.Append(Bracket.Bracket1Open.Name);
                    for (int varIndex = 0; varIndex < Variables.Length; ++varIndex)
                    {
                        if (varIndex > 0)
                            builder.Append($" {BinOp.Conjunction.FirstStringImage} ");

                        if (args[varIndex][i])
                            builder.Append(Variables[varIndex].Name);
                        else
                            builder.Append($"{UnaryOp.Inversion.FirstStringImage} {Variables[varIndex].Name}");
                    }
                    builder.Append(Bracket.Bracket1Close.Name);
                }
            }
            return builder.ToString();
        }

        private string MakePCNF(BitArray[] args, BitArray calcResult)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < calcResult.Length; ++i)
            {
                if (!calcResult[i])
                {
                    if (builder.Length > 0)
                        builder.Append($" {BinOp.Conjunction.FirstStringImage} ");
                    builder.Append(Bracket.Bracket1Open.Name);
                    for (int varIndex = 0; varIndex < Variables.Length; ++varIndex)
                    {
                        if (varIndex > 0)
                            builder.Append($" {BinOp.Disjunction.FirstStringImage} ");

                        if (!args[varIndex][i])
                            builder.Append(Variables[varIndex].Name);
                        else
                            builder.Append($"{UnaryOp.Inversion.FirstStringImage} {Variables[varIndex].Name}");
                    }
                    builder.Append(Bracket.Bracket1Close.Name);
                }
            }
            return builder.ToString();
        }

        public void MakePerfectNormalForms(out string PDNF, out string PCNF)
        {
            BitArray[] args = CreateArgs(Variables.Length);
            var dictArgs = new Dictionary<Variable, BitArray>();
            for (int i = 0; i < Variables.Length; ++i)
                dictArgs.Add(Variables[i], args[i]);
            BitArray calcResult = Calc(dictArgs);

            PDNF = MakePDNF(args, calcResult);
            PCNF = MakePCNF(args, calcResult);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj is Formula another)
            {
                if (_elements.Length != another._elements.Length)
                    return false;
                for (int i = 0; i < _elements.Length; ++i)
                    if (!_elements[i].Equals(another._elements[i]))
                        return false;
                return true;
            }
            else
                return false;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            for (int i = 0; i < _elements.Length; ++i)
            {
                if (i > 0)
                    builder.Append(", ");
                builder.Append(_elements[i]);
            }

            builder.Append("]");
            return builder.ToString();
        }

        // static components
        private static bool IsValid(FormulaElement[] elements)
        {
            List<FormulaElement> template = CreateTemplate(elements);
            while (template.Count > 1)
            {
                bool removed = false;
                for (int i = 0; i < template.Count; ++i)
                {
                    if (template[i] is UnaryOp)
                    {
                        if (i > 0)
                        {
                            template.RemoveAt(i);
                            removed = true;
                            break;
                        }
                        else
                            return false;
                    }
                    else if (template[i] is BinOp)
                    {
                        if (i > 1)
                        {
                            template.RemoveRange(i - 1, 2);
                            removed = true;
                            break;
                        }
                        else
                            return false;
                    }
                }

                if (!removed)
                    return false;
            }
            return true;
        }

        private static List<FormulaElement> CreateTemplate(FormulaElement[] elements)
        {
            return elements.Select(element =>
            {
                if (element is Operation)
                    return element;
                else
                    return null;
            }).ToList();
        }

        private static BitArray[] CreateArgs(int varsCount)
        {
            BitArray[] result = new BitArray[varsCount];
            int length = (int)Math.Pow(2, varsCount);
            for (int i = 0; i < varsCount; ++i)
                result[i] = new BitArray(length);

            for (int varIndex = 0; varIndex < varsCount; ++varIndex)
            {
                int stepLength = (int)Math.Pow(2, varIndex + 1);
                for (int stepIndex = 0, end = length / stepLength; stepIndex < end; ++stepIndex)
                {
                    for (int i = 0, iEnd = stepLength / 2; i < iEnd; ++i)
                    {
                        result[varIndex][stepIndex * stepLength + i] = false;
                        result[varIndex][stepIndex * stepLength + i + iEnd] = true;
                    }
                }
            }
            return result;
        }

    }
}
