using Microsoft.VisualStudio.TestTools.UnitTesting;

using NetDataStructures.Matrices;

namespace NetDataStructures.UnitTests.Matrices
{
    static class MatrixAssertions
    {
        public static void AreEqualM(this Assert assert, object expected, object actual, string message)
        {
            message = string.Format(message, expected, actual);
            Assert.AreEqual(expected, actual, message);
        }

        public static void MatrixMatches(this Assert assert, int[,] expected, Matrix actual)
        {
            string messageSuffix = $"\nExpected {new Matrix(expected).ToString()}\nActual {actual}\n";

            Assert.That.AreEqualM(expected.GetLength(0), actual.SizeX, "Expected matrix with SizeX = {0}, got {1}" + messageSuffix);
            Assert.That.AreEqualM(expected.GetLength(1), actual.SizeY, "Expected matrix with SizeY = {0}, got {1}" + messageSuffix);

            for (int x = 0; x < expected.GetLength(0); x++)
            {
                for (int y = 0; y < expected.GetLength(0); y++)
                {
                    Assert.That.AreEqualM(expected[x, y], actual[x, y], $"Index {x},{y}: Expected {{0}}, got {{1}}{messageSuffix}");
                }
            }
        }
    }
}
