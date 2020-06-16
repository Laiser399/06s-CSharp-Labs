using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab0614.Binary;
using Lab0614.Binary.FormulaElements;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Lab0614;

namespace Lab0614_tests
{
    [TestClass]
    public class FormulaParserTest
    {
        [TestMethod]
        public void ValidParseTest()
        {
            string[] formules =
            {
                "xorin xor c",
                "  !ABC|crab=paa  ",
                "not   notable",
                "[((a))]",
                "(a=b)or a&b",
                "a=!bb|cc",
                "!!b",
            };
            Formula[] expected =
            {
                new Formula(new Variable("xorin"), new Variable("c"), BinOp.Xor),
                new Formula(new Variable("ABC"), UnaryOp.Inversion, new Variable("crab"), BinOp.Disjunction, new Variable("paa"), BinOp.Equivalent),
                new Formula(new Variable("notable"), UnaryOp.Inversion),
                new Formula(new Variable("a")),
                new Formula(new Variable("a"), new Variable("b"), BinOp.Equivalent, new Variable("a"), new Variable("b"), BinOp.Conjunction, BinOp.Disjunction),
                new Formula(new Variable("a"), new Variable("bb"), UnaryOp.Inversion, new Variable("cc"), BinOp.Disjunction, BinOp.Equivalent),
                new Formula(new Variable("b"), UnaryOp.Inversion, UnaryOp.Inversion),
            };

            for (int i = 0; i < formules.Length; ++i)
            {
                bool parsed = FormulaParser.TryParse(formules[i], out Formula res);
                Assert.IsTrue(parsed, $"Formula: {formules[i]}");
                Assert.AreEqual(expected[i], res);
            }

        }

        [TestMethod]
        public void InvalidParseTest()
        {
            string[] formules =
            {
                "[(a])",
                "a xo val",
                "a- >b",
                "a ->| b",
                "(",
                ")",
                "(a|b",
                "a|b)",
                "! or b",
            };

            for (int i = 0; i < formules.Length; ++i)
            {
                bool parsed = FormulaParser.TryParse(formules[i], out var _);
                Assert.IsFalse(parsed, $"Formula: {formules[i]}");
            }

        }

        [TestMethod]
        public void ValidCalcTest()
        {
            string[] formules =
            {
                "not a | b&c"
            };
            Dictionary<string, BitArray>[] args =
            {
                new Dictionary<string, BitArray>
                {
                    ["a"]=BitArrayEx.From(false, false, false, false, true, true, true, true),
                    ["b"]=BitArrayEx.From(false, false, true, true, false, false, true, true),
                    ["c"]=BitArrayEx.From(false, true, false, true, false, true, false, true)
                },

            };
            BitArray[] expected =
            {
                BitArrayEx.From(true, true, true, true, false, false, false, true)
            };

            for (int i = 0; i < formules.Length; ++i)
            {
                bool parsed = FormulaParser.TryParse(formules[i], out Formula formula);
                Assert.IsTrue(parsed);
                BitArray res = formula.Calc(args[i]);
                Assert.IsTrue(expected[i].EqualsEx(res), $"expected: {expected[i].ToStringEx()}, result: {res.ToStringEx()}");
            }
        }
    }
}
