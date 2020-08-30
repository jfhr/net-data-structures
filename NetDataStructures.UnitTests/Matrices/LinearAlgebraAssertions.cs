using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.LinearAlgebra;

namespace NetDataStructures.UnitTests.Matrices
{
    internal static class LinearAlgebraAssertions
    {
        public static void MatrixMatches(this Assert assert, double[,] expected, DoubleMatrix actual)
        {
            string messageSuffix = $"\nExpected {new DoubleMatrix(expected).ToString()}\nActual {actual}\n";

            Assert.AreEqual(expected.GetLength(0), actual.RowCount,
                $"Expected matrix with RowCount = {expected.GetLength(0)}, got {actual.RowCount}" + messageSuffix);
            Assert.AreEqual(expected.GetLength(1), actual.ColumnCount,
                $"Expected matrix with ColumnCount = {expected.GetLength(1)}, got {actual.ColumnCount}" +
                messageSuffix);

            for (int x = 0; x < expected.GetLength(0); x++)
            {
                for (int y = 0; y < expected.GetLength(1); y++)
                {
                    Assert.AreEqual(expected[x, y], actual[x, y], LinearAlgebraOptions.Tolerance,
                        $"Index {x},{y}: Expected {expected[x, y]}, got {actual[x, y]}{messageSuffix}");
                }
            }
        }


        public static void VectorMatches(this Assert assert, double[] expected, DoubleVector actual)
        {
            string messageSuffix = $"\nExpected {new DoubleVector(expected).ToString()}\nActual {actual}\n";

            Assert.AreEqual(expected.GetLength(0), actual.Dimension,
                $"Expected vector with length = {expected.Length}, got {actual.Dimension}" + messageSuffix);

            for (int x = 0; x < expected.GetLength(0); x++)
            {
                Assert.AreEqual(expected[x], actual[x], LinearAlgebraOptions.Tolerance,
                    $"Index {x}: Expected {expected[x]}, got {actual[x]}{messageSuffix}");
            }
        }
    }
}