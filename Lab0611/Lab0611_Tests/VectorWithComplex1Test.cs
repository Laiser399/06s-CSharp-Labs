using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab0611;

namespace Lab0611_Tests
{
    [TestClass]
    public class VectorWithComplex1Test
    {
        [TestMethod]
        public void ScalarMultiplyTest()
        {
            Vector<Complex1> vec1 = Vector<Complex1>.FromValues(new Complex1(1, 1), new Complex1(-2, 3), new Complex1(6, 0));
            Vector<Complex1> vec2 = Vector<Complex1>.FromValues(new Complex1(0, 2), new Complex1(-3, 1), new Complex1(2, 2));
            Complex1 expected = new Complex1(13, 3);

            Assert.AreEqual(expected, vec1.ScalarMultiply(vec2));
        }
    }
}
