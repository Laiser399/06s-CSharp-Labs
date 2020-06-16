using System;
using System.Collections.Generic;
using System.Text;

namespace Lab0614.Binary.FormulaElements
{
    public class Variable : FormulaElement
    {
        public string Name
        {
            get;
            private set;
        }

        public Variable(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"Var({Name})";
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj is Variable another)
            {
                return Name.Equals(another.Name);
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        // static components
        public static bool CanBeVariable(string testString, out Variable variable)
        {
            variable = null;
            foreach (char c in testString)
                if (!char.IsLetter(c))
                    return false;

            if (testString.Length > 0)
                variable = new Variable(testString);
            return true;
        }

        public static bool CanBeVariable(string testString)
        {
            foreach (char c in testString)
                if (!char.IsLetter(c))
                    return false;
            return true;
        }
    }
}
