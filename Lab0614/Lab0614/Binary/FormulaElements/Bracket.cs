using System;
using System.Collections.Generic;
using System.Text;

namespace Lab0614.Binary.FormulaElements
{
    public class Bracket : FormulaElement
    {
        public string Name
        {
            get;
            private set;
        }
        public bool IsOpen
        {
            get;
            private set;
        }

        private Bracket(string name, bool isOpen)
        {
            Name = name;
            IsOpen = isOpen;
        }

        // static components
        public static Bracket Bracket1Open
        {
            get;
            private set;
        } = new Bracket("(", true);
        public static Bracket Bracket1Close
        {
            get;
            private set;
        } = new Bracket(")", false);
        public static Bracket Bracket2Open
        {
            get;
            private set;
        } = new Bracket("[", true);
        public static Bracket Bracket2Close
        {
            get;
            private set;
        } = new Bracket("]", false);


        private static Bracket[][] _instances =
        {
            new Bracket[]{ Bracket1Open, Bracket1Close },
            new Bracket[]{ Bracket2Open, Bracket2Close },
        };

        public static bool CanBeBracket(string testString, out Bracket bracket)
        {
            bool canBe = false;
            foreach (var pair in _instances)
            {
                foreach (Bracket b in pair)
                {
                    canBe |= b.Name.StartsWith(testString);
                    if (b.Name.Equals(testString))
                    {
                        bracket = b;
                        return true;
                    }
                }
            }
            bracket = null;
            return canBe;
        }

        public static bool CanBeBracket(string testString)
        {
            foreach (var pair in _instances)
                foreach (Bracket bracket in pair)
                    if (bracket.Name.StartsWith(testString))
                        return true;
            return false;
        }

        public static bool IsPair(Bracket first, Bracket second)
        {
            foreach (var pair in _instances)
            {
                if ((pair[0] == first && pair[1] == second)
                    || (pair[0] == second && pair[1] == first))
                    return true;
            }
            return false;
        }
    }
}
