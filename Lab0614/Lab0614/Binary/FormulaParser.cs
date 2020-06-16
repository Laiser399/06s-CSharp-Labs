using Lab0614.Binary.FormulaElements;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab0614.Binary
{
    public class ParseException : Exception
    {
        public ParseException() { }
        public ParseException(string msg) : base(msg) { }
    }

    public class LogicException : Exception
    {
        public LogicException() { }
        public LogicException(string msg) : base(msg) { }
    }

    public class FormulaParser
    {
        private Stack<FormulaElement> _resultStack = new Stack<FormulaElement>();
        private Stack<FormulaElement> _operationsStack = new Stack<FormulaElement>();
        private FormulaElement _prevElement = null;

        private FormulaElement[] _result;
        protected FormulaElement[] Result
        {
            get {
                if (_result is null)
                {
                    _result = _resultStack.ToArray();
                    Array.Reverse(_result);
                }
                return _result;
            }
        }

        private FormulaParser(string formula)
        {
            StringBuilder buffer = new StringBuilder();

            foreach (char c in formula)
            {
                if (char.IsWhiteSpace(c) && buffer.Length == 0)
                    continue;

                bool canBe1 = FormulaElement.CanBeElement(buffer.ToString(), out FormulaElement elem);
                bool canBe2 = FormulaElement.CanBeElement(buffer.ToString() + c);

                if (canBe1 && !canBe2)
                {
                    if (elem is object)
                    {
                        AddNextElement(elem);
                        buffer.Clear();
                        if (!char.IsWhiteSpace(c))
                            buffer.Append(c);
                    }
                    else
                        throw new ParseException();
                }
                else if (canBe1 && canBe2)
                    buffer.Append(c);
                else
                    throw new ParseException();
            }

            if (buffer.Length > 0)
            {
                FormulaElement.CanBeElement(buffer.ToString(), out FormulaElement elem);
                if (elem is object)
                    AddNextElement(elem);
                else
                    throw new ParseException($"Error parse \"{buffer}\" at the end of formula.");
            }
            EndParse();
        }

        private bool IsOpenBracket(FormulaElement element)
        {
            return (element is Bracket bracket) && bracket.IsOpen;
        }

        private bool IsCloseBracket(FormulaElement element)
        {
            return (element is Bracket bracket) && !bracket.IsOpen;
        }

        private void AddNext(BinOp op)
        {
            if (!(_prevElement is Variable || IsCloseBracket(_prevElement)))
                throw new LogicException("Before binary operation can be only variable or open bracket.");

            while (_operationsStack.Count > 0 && (_operationsStack.Peek() is BinOp prevOp && prevOp.Priority <= op.Priority
                || _operationsStack.Peek() is UnaryOp))
            {
                _resultStack.Push(_operationsStack.Pop());
            }
            _operationsStack.Push(op);
        }

        private void AddNext(UnaryOp op)
        {
            if (_prevElement is Variable || IsCloseBracket(_prevElement))
                throw new LogicException("Before unary operation can not be variable or close bracket.");

            _operationsStack.Push(op);
        }

        private void AddNext(Variable variable)
        {
            if (_prevElement is Variable || IsCloseBracket(_prevElement))
                throw new LogicException("Before variable can not be variable or close bracket.");

            _resultStack.Push(variable);
        }

        private void AddNext(Bracket bracket)
        {
            if (bracket.IsOpen)
            {
                if (_prevElement is Variable || IsCloseBracket(_prevElement))
                    throw new LogicException("Before open bracket can not be variable or close bracket.");

                _operationsStack.Push(bracket);
            }
            else
            {
                if (!(_prevElement is Variable || IsCloseBracket(_prevElement)))
                    throw new LogicException("Before close bracket can be only variable or close bracket.");

                while (true)
                {
                    if (_operationsStack.Count == 0)
                        throw new LogicException("For close bracket not found open bracket.");

                    FormulaElement next = _operationsStack.Pop();
                    if (next is Bracket openBracket)
                    {
                        if (Bracket.IsPair(openBracket, bracket))
                            break;
                        else
                            throw new LogicException("Wrong brackets arrangement.");
                    }
                    else if (next is Operation)
                    {
                        _resultStack.Push(next);
                    }
                }
            }
        }

        private void AddNextElement(FormulaElement element)
        {
            switch (element)
            {
                case BinOp op:
                    AddNext(op);
                    break;
                case UnaryOp op:
                    AddNext(op);
                    break;
                case Variable variable:
                    AddNext(variable);
                    break;
                case Bracket bracket:
                    AddNext(bracket);
                    break;
            }
            _prevElement = element;
        }

        private void EndParse()
        {
            if (_prevElement is null)
                throw new ParseException("Empty formula.");
            if (_prevElement is BinOp || _prevElement is UnaryOp || IsOpenBracket(_prevElement))
                throw new LogicException("The last element of formula can not be binary operation, unary operation or open bracket.");

            while (_operationsStack.Count > 0)
            {
                FormulaElement next = _operationsStack.Pop();
                if (next is Bracket)
                    throw new LogicException("Open bracket without close bracket.");
                else if (next is Operation)
                    _resultStack.Push(next);
            }
        }

        // static components
        public static bool TryParse(string formula, out Formula result)
        {
            try
            {
                var elements = new FormulaParser(formula).Result;
                result = new Formula(elements);
                return true;
            }
            catch (ParseException)
            {
                result = null;
                return false;
            }
            catch (LogicException)
            {
                result = null;
                return false;
            }
            catch (ArgumentException)
            {
                result = null;
                return false;
            }
        }


    }
}
