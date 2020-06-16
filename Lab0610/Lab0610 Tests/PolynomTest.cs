using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab0610;

namespace Lab0610_Tests
{
    [TestClass]
    public class PolynomTest
    {

        [TestMethod]
        public void CalculateTest()
        {
            Polynom<Double1>[] polynoms = new Polynom<Double1>[]
            {
                new Polynom<Double1>(new Dictionary<int, Double1>
                {
                    [0] = 1,
                    [2] = 3,
                    [3] = -4
                }),
                new Polynom<Double1>(new Dictionary<int, Double1>
                {
                    [2] = -3,
                    [5] = 6
                })
            };
            Double1[] values = new Double1[]
            {
                -2,
                3
            };
            Double1[] expected = new Double1[]
            {
                45,
                1431
            };

            for (int i = 0; i < polynoms.Length; ++i)
                Assert.AreEqual(expected[i], polynoms[i].Calculate(values[i]));

        }
    
        [TestMethod]
        public void CompareToTest()
        {
            Polynom<Double1>[] poly1 = new Polynom<Double1>[]
            {
                new Polynom<Double1>(new Dictionary<int, Double1>
                {
                    [0] = 1, [2] = 3
                }),
                new Polynom<Double1>(new Dictionary<int, Double1>
                {
                    [0] = 2, [1] = -3, [3] = 4
                }),
                new Polynom<Double1>(new Dictionary<int, Double1>
                {
                    [0] = -1, [1] = 1
                }),
                new Polynom<Double1>(new Dictionary<int, Double1>
                {
                    [0] = 0, [1] = 6, [3] = -5
                }),
                new Polynom<Double1>(),
                new Polynom<Double1>(),
                new Polynom<Double1>(new Dictionary<int, Double1>
                {
                    [1] = 1
                })
            };
            Polynom<Double1>[] poly2 = new Polynom<Double1>[]
            {
                new Polynom<Double1>(new Dictionary<int, Double1>
                {
                    [2] = 3
                }),
                new Polynom<Double1>(new Dictionary<int, Double1>
                {
                    [0] = 2, [1] = -3, [3] = 4, [4] = 1
                }),
                new Polynom<Double1>(new Dictionary<int, Double1>
                {
                    [1] = 1
                }),
                new Polynom<Double1>(new Dictionary<int, Double1>
                {
                    [1] = 6, [3] = -5
                }),
                new Polynom<Double1>(new Dictionary<int, Double1>
                {
                    [1] = 1
                }),
                new Polynom<Double1>(),
                new Polynom<Double1>(new Dictionary<int, Double1>
                {
                    [1] = 1, [2] = -1
                })
            };
            int[] expected = new int[]
            {
                1,
                -1,
                -1,
                0,
                -1,
                0,
                1
            };

            for (int i = 0; i < poly1.Length; ++i)
                Assert.AreEqual(expected[i], poly1[i].CompareTo(poly2[i]),
                    $"first: {poly1[i]}, second: {poly2[i]}");

        }
    
    
    }
}
