using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab0610;

namespace Lab0610_Tests
{
    [TestClass]
    public class QuadMatrixTest
    {
        [TestMethod]
        public void DeterminantTest()
        {
            QuadMatrix[] matrices = new QuadMatrix[]
            {
                new QuadMatrix(new double[][]
                {
                    new double[] { 0, 1 },
                    new double[] { 1, 2 }
                }),
                new QuadMatrix(new double[][]
                {
                    new double[] { 0, 0 },
                    new double[] { 1, 2 }
                }),
                new QuadMatrix(new double[][]
                {
                    new double[] { 4, 5 },
                    new double[] { 2, 3 }
                }),
                new QuadMatrix(new double[][]
                {
                    new double[] { 0, 1, 3 },
                    new double[] { 0, 2, -6 },
                    new double[] { -2, 3, 3 }
                })
            };
            double[] expected = new double[]
            {
                -1,
                0,
                2,
                24
            };

            for (int i = 0; i < matrices.Length; ++i)
            {
                Assert.AreEqual(expected[i], matrices[i].Determinant());
            }
        }

        [TestMethod]
        public void AddTest()
        {
            QuadMatrix mtx1 = new QuadMatrix(new double[][]
            {
                new double[] { 0, 1 },
                new double[] { 1, 2 }
            });
            QuadMatrix mtx2 = new QuadMatrix(new double[][]
            {
                new double[] { 3, -4 },
                new double[] { 5, -6 }
            });

            QuadMatrix mtx1Expected = new QuadMatrix(new double[][]
            {
                new double[] { 3, -3 },
                new double[] { 6, -4 }
            });
            QuadMatrix mtx2Expected = new QuadMatrix(new double[][]
            {
                new double[] { 3, -4 },
                new double[] { 5, -6 }
            });

            mtx1.Add(mtx2);

            Assert.AreEqual(mtx1, mtx1Expected);
            Assert.AreEqual(mtx2, mtx2Expected);

        }
    
        [TestMethod]
        public void MultiplyTest()
        {
            QuadMatrix mtx1 = new QuadMatrix(new double[][]
            {
                new double[] { -2, 1 },
                new double[] { 1, 2 }
            });
            QuadMatrix mtx2 = new QuadMatrix(new double[][]
            {
                new double[] { 4, 3 },
                new double[] { -2, 0 }
            });

            QuadMatrix mtx1Exp = new QuadMatrix(new double[][]
            {
                new double[] { -10, -6 },
                new double[] { 0, 3 }
            });
            QuadMatrix mtx2Exp = new QuadMatrix(new double[][]
            {
                new double[] { 4, 3 },
                new double[] { -2, 0 }
            });

            mtx1.Multiply(mtx2);

            Assert.AreEqual(mtx1, mtx1Exp);
            Assert.AreEqual(mtx2, mtx2Exp);
        }

        [TestMethod]
        public void InverseTest()
        {
            QuadMatrix[] matrices = new QuadMatrix[]
            {
                new QuadMatrix(new double[][]
                {
                    new double[] { 3, 4 },
                    new double[] { -2, 0 }
                }),
                new QuadMatrix(new double[][]
                {
                    new double[] { 1, 4, -1 },
                    new double[] { 0, 3, 0 },
                    new double[] { -2, -4, 3 }
                })
            };
            QuadMatrix[] expected = new QuadMatrix[]
            {
                new QuadMatrix(new double[][]
                {
                    new double[] { 0, -0.5 },
                    new double[] { 0.25, 0.375 }
                }),
                new QuadMatrix(new double[][]
                {
                    new double[] { 3, -8.0 / 3, 1 },
                    new double[] { 0, 1.0/3, 0 },
                    new double[] { 2, -4.0 / 3, 1 }
                })
            };

            for (int i = 0; i < matrices.Length; ++i)
                Assert.AreEqual(expected[i], QuadMatrix.Inversed(matrices[i]));
        }


    }
}
