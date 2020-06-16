using System;
using System.Collections.Generic;
using System.Text;

namespace Lab0614.Binary.FormulaElements
{
    public abstract class Operation : FormulaElement
    {
        protected string[] _stringImages;
        public string FirstStringImage
        {
            get => _stringImages.Length > 0 ? _stringImages[0] : "";
        }

        protected Operation(string[] stringImages)
        {
            _stringImages = stringImages;
        }

        public bool CanBe(string testString, out bool isReady)
        {
            bool canBe = false;
            isReady = false;
            foreach (string image in _stringImages)
            {
                canBe |= image.StartsWith(testString);
                if (image.Equals(testString))
                {
                    isReady = true;
                    return true;
                }
            }
            return canBe;
        }

        public bool CanBe(string testString)
        {
            foreach (string image in _stringImages)
                if (image.StartsWith(testString))
                    return true;
            return false;
        }

        // static components
        private static Operation[] _instances =
        {
            UnaryOp.Inversion,
            BinOp.Conjunction,
            BinOp.Disjunction,
            BinOp.Xor,
            BinOp.Implication,
            BinOp.Equivalent
        };

        public static bool CanBeOperation(string testString, out Operation operation)
        {
            bool canBe = false;
            foreach (var instance in _instances)
            {
                canBe |= instance.CanBe(testString, out bool isReady);
                if (isReady)
                {
                    operation = instance;
                    return true;
                }
            }
            operation = null;
            return canBe;
        }

        public static bool CanBeOperation(string testString)
        {
            foreach (Operation instance in _instances)
                if (instance.CanBe(testString))
                    return true;
            return false;
        }
    }
}
