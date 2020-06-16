using System;
using System.Collections.Generic;
using System.Text;

namespace Lab0614.Binary.FormulaElements
{
    public abstract class FormulaElement
    {

        public static bool CanBeElement(string testString, out FormulaElement element)
        {
            bool canBe = false;

            canBe |= Bracket.CanBeBracket(testString, out Bracket bracket);
            if (bracket is object)
            {
                element = bracket;
                return true;
            }

            canBe |= Operation.CanBeOperation(testString, out Operation op);
            if (op is object)
            {
                element = op;
                return true;
            }

            canBe |= Variable.CanBeVariable(testString, out Variable variable);
            if (variable is object)
            {
                element = variable;
                return true;
            }

            element = null;
            return canBe;
        }

        public static bool CanBeElement(string testString)
        {
            return Bracket.CanBeBracket(testString) || Operation.CanBeOperation(testString) 
                || Variable.CanBeVariable(testString);
        }
    }
}
