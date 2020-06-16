using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab0611;

namespace Lab0611_Tests
{
    [TestClass]
    public class VectorTest
    {
        [TestMethod]
        public void AbsTest()
        {
            Vector<Double1>[] vectors = new Vector<Double1>[]
            {
                Vector<Double1>.FromValues(1, 2, 3),
                Vector<Double1>.FromValues(1, 2, 3, 4),
                Vector<Double1>.FromValues(0, -2, 3),
            };
            Double1[] expected = new Double1[]
            {
                Math.Sqrt(14),
                Math.Sqrt(30),
                Math.Sqrt(13)
            };

            for (int i = 0; i < vectors.Length; ++i)
            {
                Assert.AreEqual(expected[i].Value, vectors[i].Abs().Value, 0.001);
            }

        }

        [TestMethod]
        public void OrtogonalizeTest()
        {
            Vector<Double1> vec1 = Vector<Double1>.FromValues(3, -2, 6);
            Vector<Double1> vec2 = Vector<Double1>.FromValues(7, -3, 0);
            Vector<Double1> vec3 = Vector<Double1>.FromValues(5, 0, 2);
            Vector<Double1>[] result = Vector<Double1>.Orthogonalize(vec1, vec2, vec3);
            Assert.IsTrue(result.Length > 1);
            for (int i = 0; i < result.Length; ++i)
                for (int j = 0; j < result.Length; ++j)
                    if (i != j)
                        Assert.AreEqual(0, result[i].ScalarMultiply(result[j]).Value, 0.001);
        }

        [TestMethod]
        public void VectorMultiplyTest()
        {
            Vector<Double1>[] vec1 = new Vector<Double1>[]
            {
                Vector<Double1>.FromValues(2, -3, 6),
                Vector<Double1>.FromValues(0, 0, 0),
                Vector<Double1>.FromValues(1.5, 0, 3)
            };
            Vector<Double1>[] vec2 = new Vector<Double1>[]
            {
                Vector<Double1>.FromValues(0, 7, -1),
                Vector<Double1>.FromValues(1, 2, 3),
                Vector<Double1>.FromValues(-3, -4, 6)
            };
            Vector<Double1>[] expected = new Vector<Double1>[]
            {
                Vector<Double1>.FromValues(-39, 2, 14),
                Vector<Double1>.FromValues(0, 0, 0),
                Vector<Double1>.FromValues(12, -18, -6)
            };

            for (int i = 0; i < vec1.Length; ++i)
                Assert.AreEqual(expected[i], Vector<Double1>.VectorMultiply3D(vec1[i], vec2[i]));
        }



    }
}
