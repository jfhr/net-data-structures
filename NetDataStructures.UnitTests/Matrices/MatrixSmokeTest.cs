using Microsoft.VisualStudio.TestTools.UnitTesting;

using NetDataStructures.Matrices;

namespace NetDataStructures.UnitTests.Matrices
{
    [TestClass]
    public class MatrixSmokeTest
    {
        /// <summary>
        /// I can create a matrix from an int[,].
        /// </summary>
        [TestMethod]
        public void CreateMatrix()
        {
            // Arrange
            var numbers = new int[,]
            {
                { 1, 2 },
                { 3, 4 },
            };

            // Act
            Matrix matrix = new Matrix(numbers);

            // Assert
            Assert.AreEqual(2, matrix.SizeX);
            Assert.AreEqual(2, matrix.SizeY);
        }

        /// <summary>
        /// I can create a new, bigger matrix from an existing one.
        /// </summary>
        [TestMethod]
        public void ExpandMatrix()
        {
            // Arrange
            Matrix matrix = new Matrix(new int[,]
            {
                { 1, 2 },
                { 3, 4 },
            });

            // Act
            Matrix expandedMatrix = new Matrix(matrix, 2, 4);

            // Assert
            Assert.That.MatrixMatches(new int[,]
            {
                { 1, 2, 0, 0 },
                { 3, 4, 0, 0 },
            }, expandedMatrix);
        }

        /// <summary>
        /// I can access any element in the matrix by two zero-based indices.
        /// </summary>
        [TestMethod]
        public void MatrixNumberAccess()
        {
            // Arrange
            var numbers = new int[,]
            {
                { 1, 2 },
                { 3, 4 },
            };
            var matrix = new Matrix(numbers);

            // Act
            int upperLeft = numbers[0, 0];
            int upperRight = numbers[0, 1];
            int lowerLeft = numbers[1, 0];
            int lowerRight = numbers[1, 1];

            // Assert
            Assert.AreEqual(1, upperLeft);
            Assert.AreEqual(2, upperRight);
            Assert.AreEqual(3, lowerLeft);
            Assert.AreEqual(4, lowerRight);
        }

        /// <summary>
        /// I can get the scalar product of a matrix.
        /// </summary>
        [TestMethod]
        public void Scalarproduct()
        {
            // Arrange
            var matrix = new Matrix(new int[,]
            {
                { 1, 2 },
                { 3, 4 },
            });
            var scalar = 4;

            // Act
            Matrix result = scalar * matrix;

            // Assert
            Assert.That.MatrixMatches(new int[,]
            {
                { 4, 8, },
                { 12, 16, },
            }, result);
        }

        /// <summary>
        /// I can get the scalar product of a matrix.
        /// </summary>
        [TestMethod]
        public void Negate()
        {
            // Arrange
            var matrix = new Matrix(new int[,]
            {
                { 1, 2 },
                { 3, 4 },
            });

            // Act
            Matrix result = -matrix;

            // Assert
            Assert.That.MatrixMatches(new int[,]
            {
                { -1, -2 },
                { -3, -4 },
            }, result);
        }

        /// <summary>
        /// I can get the scalar product of a matrix in-place.
        /// </summary>
        [TestMethod]
        public void ScalarproductInPlace()
        {
            // Arrange
            var matrix = new Matrix(new int[,]
            {
                { 1, 2 },
                { 3, 4 },
            });
            var scalar = 4;

            // Act
            matrix.MultiplyInPlace(scalar);

            // Assert
            Assert.That.MatrixMatches(new int[,]
            {
                { 4, 8, },
                { 12, 16, },
            }, matrix);
        }

        /// <summary>
        /// I can get a zero matrix by multiplying an existing matrix with 0.
        /// </summary>
        [TestMethod]
        public void ScalarproductBy0()
        {
            // Arrange
            var matrix = new Matrix(new int[,]
            {
                { 1, 2 },
                { 3, 4 },
            });
            var scalar = 0;

            // Act
            Matrix result = scalar * matrix;

            // Assert
            Assert.That.MatrixMatches(new int[,]
            {
                { 0, 0, },
                { 0, 0, },
            }, result);
        }

        /// <summary>
        /// I can add two matrices together.
        /// </summary>
        [TestMethod]
        public void AddMatrices()
        {
            // Arrange
            var matrix0 = new Matrix(new int[,]
            {
                { 1, 2 },
                { 3, 4 },
            });

            var matrix1 = new Matrix(new int[,]
            {
                { 5, 3 },
                { 1, -1 },
            });

            // Act
            Matrix result = matrix0 + matrix1;

            // Assert
            Assert.That.MatrixMatches(new int[,]
            {
                { 6, 5, },
                { 4, 3, },
            }, result);
        }

        /// <summary>
        /// I can add two matrices together in-place.
        /// </summary>
        [TestMethod]
        public void AddMatricesInPlace()
        {
            // Arrange
            var matrix0 = new Matrix(new int[,]
            {
                { 1, 2 },
                { 3, 4 },
            });

            var matrix1 = new Matrix(new int[,]
            {
                { 5, 3 },
                { 1, -1 },
            });

            // Act
            matrix0.AddInPlace(matrix1);

            // Assert
            Assert.That.MatrixMatches(new int[,]
            {
                { 6, 5, },
                { 4, 3, },
            }, matrix0);
        }

        /// <summary>
        /// When I try to add two matrices of different lengths,
        /// I get a <see cref="MatrixMathException"/>.
        /// </summary>
        [TestMethod, ExpectedException(typeof(MatrixMathException))]
        public void AddMatrices_DifferentLengths_Exception()
        {
            // Arrange
            var matrix0 = new Matrix(new int[,]
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            });

            var matrix1 = new Matrix(new int[,]
            {
                { 5, 3 },
                { 1, -1 },
            });

            // Act
            Matrix result = matrix0 + matrix1;
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
            var matrix0 = new Matrix(new int[,]
            {
                { 1, 2 },
                { 3, 4 },
            });

            var matrix1 = new Matrix(new int[,]
            {
                { 5, 3 },
                { 1, -1 },
            });

            // Act
            Matrix result = matrix0 * matrix1;

            // Assert
            Assert.That.MatrixMatches(new int[,]
            {
                { 7, 1, },
                { 19, 5, },
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
            var matrix0 = new Matrix(new int[,]
            {
                { 1, 2 },
                { 3, 4 },
            });

            var matrix1 = new Matrix(new int[,]
            {
                { 5, 3, 1 },
                { 1, -1, -3 },
            });

            // Act
            Matrix result = matrix0 * matrix1;
        }
    }
}
