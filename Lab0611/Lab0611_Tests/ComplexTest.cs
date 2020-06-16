using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab0611;
using System;

namespace Lab0611_Tests
{
    [TestClass]
    public class ComplexTest
    {
        [TestMethod]
        public void ParamsTest()
        {
            Complex1[] numbers = new Complex1[]
            {
                new Complex1(3, 4), new Complex1(-2, 6), new Complex1(5, -4), new Complex1(-3, -6)
            };
            double[] expectedRadius = new double[]
            {
                5, 6.324, 6.403, 6.708
            };
            double[] expectedPhase = new double[]
            {
                0.927, 1.892, -0.674, -2.034
            };


            for (int i = 0; i < numbers.Length; ++i)
            {
                Assert.AreEqual(expectedRadius[i], numbers[i].Magnitude, 0.01, $"Radius test number {i}");
                Assert.AreEqual(expectedPhase[i], numbers[i].Phase, 0.01, $"Phase test number {i}");
            }

        }

        [TestMethod]
        public void PowTest()
        {
            Complex1[] numbers = new Complex1[]
            {
                new Complex1(6, -8), new Complex1(3, 22), new Complex1(-6, -34)
            };
            double[] powers = new double[]
            {
                2, 3.14, -2
            };
            Complex1[] expected = new Complex1[]
            {
                new Complex1(-28, -96), new Complex1(-3450.04, -16539.4), new Complex1(-35.0/44402, -51.0/177608)
            };

            for (int i = 0; i < numbers.Length; ++i)
            {
                Complex1 result = numbers[i].Pow(powers[i]);
                Assert.AreEqual(expected[i].Real, result.Real, 0.1, $"Real in test #{i}");
                Assert.AreEqual(expected[i].Imaginary, result.Imaginary, 0.1, $"Imaginary in test #{i}");
            }
        }

        [TestMethod]
        public void MultiplyTest()
        {
            Complex1[] first = new Complex1[]
            {
                new Complex1(6, -8), new Complex1(3, 22), new Complex1(-6, -34)
            };
            Complex1[] second = new Complex1[]
            {
                new Complex1(0, -2), new Complex1(5, 2), new Complex1(2, -2)
            };
            Complex1[] expected = new Complex1[]
            {
                new Complex1(-16, -12), new Complex1(-29, 116), new Complex1(-80, -56)
            };

            for (int i = 0; i < first.Length; ++i)
            {
                Assert.AreEqual(expected[i], Complex1.Multiply(first[i], second[i]), $"Test #{i}");
            }

        }

        [TestMethod]
        public void DivideTest()
        {
            Complex1[] first = new Complex1[]
            {
                new Complex1(6, -8), new Complex1(3, 22), new Complex1(-6, -34)
            };
            Complex1[] second = new Complex1[]
            {
                new Complex1(0, -2), new Complex1(5, 2), new Complex1(2, -2)
            };
            Complex1[] expected = new Complex1[]
            {
                new Complex1(4, 3), new Complex1(59.0/29, 104.0/29), new Complex1(7, -10)
            };

            for (int i = 0; i < first.Length; ++i)
            {
                Assert.AreEqual(expected[i], Complex1.Divide(first[i], second[i]), $"Test #{i}");
            }
        }





    }
}
