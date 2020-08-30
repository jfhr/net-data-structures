using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.LinearAlgebra;

namespace NetDataStructures.UnitTests.Matrices
{
    [TestClass]
    public class MatrixSmokeTest
    {
        /// <summary>
        /// I can create a matrix from a double[,].
        /// </summary>
        [TestMethod]
        public void CreateMatrix()
        {
            // Arrange
            var numbers = new double[,]
            {
                {1, 2},
                {3, 4},
            };

            // Act
            DoubleMatrix matrix = new DoubleMatrix(numbers);

            // Assert
            Assert.AreEqual(2, matrix.RowCount);
            Assert.AreEqual(2, matrix.ColumnCount);
        }

        /// <summary>
        /// I can create a new, bigger matrix from an existing one.
        /// </summary>
        [TestMethod]
        public void ExpandMatrix()
        {
            // Arrange
            DoubleMatrix matrix = new DoubleMatrix(new double[,]
            {
                {1, 2},
                {3, 4},
            });

            // Act
            DoubleMatrix expandedMatrix = new DoubleMatrix(matrix, 2, 4);

            // Assert
            Assert.That.MatrixMatches(new double[,]
            {
                {1, 2, 0, 0},
                {3, 4, 0, 0},
            }, expandedMatrix);
        }

        /// <summary>
        /// I can access any element in the matrix by two zero-based indices.
        /// </summary>
        [TestMethod]
        public void MatrixNumberAccess()
        {
            // Arrange
            var matrix = new DoubleMatrix(new double[,]
            {
                {1, 2},
                {3, 4},
            });

            // Act
            double upperLeft = matrix[0, 0];
            double upperRight = matrix[0, 1];
            double lowerLeft = matrix[1, 0];
            double lowerRight = matrix[1, 1];

            // Assert
            Assert.AreEqual(1, upperLeft, LinearAlgebraOptions.Tolerance);
            Assert.AreEqual(2, upperRight, LinearAlgebraOptions.Tolerance);
            Assert.AreEqual(3, lowerLeft, LinearAlgebraOptions.Tolerance);
            Assert.AreEqual(4, lowerRight, LinearAlgebraOptions.Tolerance);
        }

        /// <summary>
        /// I can get the scalar product of a matrix.
        /// </summary>
        [TestMethod]
        public void Scalarproduct()
        {
            // Arrange
            var matrix = new DoubleMatrix(new double[,]
            {
                {1, 2},
                {3, 4},
            });
            var scalar = 4;

            // Act
            DoubleMatrix result = scalar * matrix;

            // Assert
            Assert.That.MatrixMatches(new double[,]
            {
                {4, 8,},
                {12, 16,},
            }, result);
        }

        /// <summary>
        /// I can get a zero matrix by multiplying an existing matrix with 0.
        /// </summary>
        [TestMethod]
        public void ScalarProductBy0()
        {
            // Arrange
            var matrix = new DoubleMatrix(new double[,]
            {
                {1, 2},
                {3, 4},
            });
            var scalar = 0;

            // Act
            DoubleMatrix result = scalar * matrix;

            // Assert
            Assert.That.MatrixMatches(new double[,]
            {
                {0, 0,},
                {0, 0,},
            }, result);
        }

        /// <summary>
        /// I can add two matrices together.
        /// </summary>
        [TestMethod]
        public void AddMatrices()
        {
            // Arrange
            var matrix0 = new DoubleMatrix(new double[,]
            {
                {1, 2},
                {3, 4},
            });

            var matrix1 = new DoubleMatrix(new double[,]
            {
                {5, 3},
                {1, -1},
            });

            // Act
            DoubleMatrix result = matrix0 + matrix1;

            // Assert
            Assert.That.MatrixMatches(new double[,]
            {
                {6, 5,},
                {4, 3,},
            }, result);
        }

        /// <summary>
        /// When I try to add two matrices of different lengths,
        /// I get a <see cref="MatrixMathException"/>.
        /// </summary>
        [TestMethod, ExpectedException(typeof(MatrixMathException))]
        public void AddMatrices_DifferentLengths_Exception()
        {
            // Arrange
            var matrix0 = new DoubleMatrix(new double[,]
            {
                {1, 2},
                {3, 4},
                {5, 6},
            });

            var matrix1 = new DoubleMatrix(new double[,]
            {
                {5, 3},
                {1, -1},
            });

            // Act
            DoubleMatrix result = matrix0 + matrix1;
        }

        /// <summary>
        /// I can get the matrix product of two matrices.
        /// </summary>
        /// <remarks>
        /// +------+   +------+   +-------+
        /// | 1  2 |   | 5  3 |   |  7  1 |
        /// | 3  4 | * | 1 -1 | = | 19  5 |
        /// +------+   +------+   +-------+
        /// </remarks>
        [TestMethod]
        public void MultiplyMatrices()
        {
            // Arrange
            var matrix0 = new DoubleMatrix(new double[,]
            {
                {1, 2},
                {3, 4},
            });

            var matrix1 = new DoubleMatrix(new double[,]
            {
                {5, 3},
                {1, -1},
            });

            // Act
            DoubleMatrix result = matrix0 * matrix1;

            // Assert
            Assert.That.MatrixMatches(new double[,]
            {
                {7, 1,},
                {19, 5,},
            }, result);
        }

        /// <summary>
        /// I can get the transpose of a matrix.
        /// </summary>
        [TestMethod]
        public void TransposeMatrix()
        {
            // Arrange
            var input = new DoubleMatrix(new double[,]
            {
                {1, 2, 3},
                {4, 5, 6},
            });

            // Act
            DoubleMatrix result = input.Transpose();

            // Assert
            Assert.That.MatrixMatches(new double[,]
            {
                {1, 4},
                {2, 5},
                {3, 6},
            }, result);
        }

        /// <summary>
        /// When I try to get the product of two matrices with incompatible sizes, 
        /// an exception is thrown.
        /// </summary>
        [TestMethod, ExpectedException(typeof(MatrixMathException))]
        public void MultiplyMatrices_IncompatibleDimensions_Exception()
        {
            // Arrange
            var matrix0 = new DoubleMatrix(new double[,]
            {
                {1, 2},
                {3, 4},
            });

            var matrix1 = new DoubleMatrix(new double[,]
            {
                {5, 3, 1},
                {1, -1, -3},
            });

            // Act
            DoubleMatrix result = matrix0 * matrix1;
        }


        /// <summary>
        /// I can get the product of a matrix and a vector.
        /// </summary>
        /// <remarks>
        /// +------+   +------+   +-------+
        /// | 1  2 |   | 5  3 |   |  7  1 |
        /// | 3  4 | * | 1 -1 | = | 19  5 |
        /// +------+   +------+   +-------+
        /// </remarks>
        [TestMethod]
        public void MatrixVectorMultiplication()
        {
            // Arrange
            var a = new DoubleMatrix(new double[,]
            {
                {1, -1, 2},
                {0, -3, 1},
            });
            var v = new DoubleVector(new double[] {2, 1, 0});

            // Act
            DoubleVector result = a * v;

            // Assert
            Assert.That.VectorMatches(new double[] {1, -3}, result);
        }
    }
}