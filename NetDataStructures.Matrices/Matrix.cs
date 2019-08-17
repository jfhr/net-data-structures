using System;
using System.Text;

namespace NetDataStructures.Matrices
{
    public struct Matrix : IMatrix
    {
        private int[,] _matrix;

        /// <summary>
        /// Gets the X-Dimension (the horizontal size) of this matrix.
        /// </summary>
        public int SizeX => _matrix.GetLength(0);
        /// <summary>
        /// Gets the Y-Dimension (the vertical size) of this matrix.
        /// </summary>
        public int SizeY => _matrix.GetLength(1);

        /// <summary>
        /// Gets the element of this matrix at index the zero-based indices
        /// <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        public int this[int x, int y]
        {
            get
            {
                return _matrix[x, y];
            }
        }

        /// <summary>
        /// Creates a new matrix based on a 2d-array of 32-bit integers.
        /// </summary>
        public Matrix(int[,] numbers)
        {
            _matrix = numbers;
        }

        /// <summary>
        /// Creates a new matrix based on the given matrix with the given dimensions.
        /// </summary>
        /// <remarks>
        /// The dimensions can be larger, but not smaller, than those of the base matrix.
        /// If they are larger, the surplus fields will be initialized to zero.
        /// </remarks>
        public Matrix(Matrix baseMatrix, int sizeX, int sizeY)
        {
            if (sizeX < baseMatrix.SizeX)
            {
                throw new ArgumentException("The new size can't be less than the base size.", nameof(sizeX));
            }
            if (sizeY < baseMatrix.SizeY)
            {
                throw new ArgumentException("The new size can't be less than the base size.", nameof(sizeY));
            }

            var newMatrix = new int[sizeX, sizeY];
            baseMatrix.ForEachElement((x, y, value) => newMatrix[x, y] = value);
            _matrix = newMatrix;
        }

        /// <summary>
        /// Runs an action on each element of the matrix in order.
        /// </summary>
        /// <param name="action">
        /// Action that takes three arguments: (x-index, y-index, value).
        /// </param>
        private void ForEachElement(Action<int, int, int> action)
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    action(x, y, _matrix[x, y]);
                }
            }
        }

        /// <summary>
        /// Applies a transformation function on each element of the matrix
        /// and returns a new matrix with the same size, consisting of the
        /// results of the transformation function.
        /// </summary>
        /// <param name="transformation">
        /// Function that takes three arguments: (x-index, y-index, value)
        /// and returns a new value.
        /// </param>
        private Matrix CopyTransformEachElement(Func<int, int, int, int> transformation)
        {
            int[,] newNumbers = new int[SizeX, SizeY];

            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    newNumbers[x, y] = transformation(x, y, _matrix[x, y]);
                }
            }

            return new Matrix(newNumbers);
        }

        /// <summary>
        /// Adds two matrices of the same dimensions and returns the result as a new matrix.
        /// </summary>
        /// <exception cref="MatrixMathException">
        /// The two matrices are not of the same dimension.
        /// </exception>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m1.SizeX != m2.SizeX)
            {
                throw new MatrixMathException($"Attempt to add two matrices with different X-Sizes of {m1.SizeX}, {m2.SizeX}.");
            }
            if (m1.SizeY != m2.SizeY)
            {
                throw new MatrixMathException($"Attempt to add two matrices with different Y-Sizes of {m1.SizeY}, {m2.SizeY}.");
            }

            return m1.CopyTransformEachElement((x, y, value) => value + m2._matrix[x, y]);
        }

        /// <summary>
        /// Calculates the product of a matrix and a scalar and returns the result as a new matrix.
        /// </summary>
        public static Matrix operator *(Matrix m, int scalar)
        {
            return m.CopyTransformEachElement((x, y, value) => value * scalar);
        }

        /// <summary>
        /// Calculates the product of a matrix and a scalar and returns the result as a new matrix.
        /// </summary>
        public static Matrix operator *(int scalar, Matrix m)
        {
            return m * scalar;
        }

        /// <summary>
        /// Calculates the matrix product of two matrices of compatible sizes.
        /// This operation is not commutative.
        /// </summary>
        /// <exception cref="MatrixMathException">
        /// The Y-Dimension of <paramref name="m1"/> is not equal to the X-Dimension of <paramref name="m2"/>.
        /// </exception>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.SizeX != m2.SizeY)
            {
                throw new MatrixMathException($"Attempt to multiply matrix with Y-Size of {m1.SizeY} with another matrix with X-Size {m1.SizeX}.");
            }

            int[,] newNumbers = new int[m1.SizeX, m2.SizeY];

            for (int x = 0; x < m1.SizeX; x++)
            {
                for (int y = 0; y < m2.SizeY; y++)
                {
                    for (int n = 0; n < m1.SizeY; n++)
                    {
                        newNumbers[x, y] += m1[x, n] * m2[n, y];
                    }
                }
            }

            return new Matrix(newNumbers);
        }

        /// <summary>
        /// Creates a <see cref="string"/>-representation of this matrix.
        /// </summary>
        public override string ToString()
        {
            int minRequiredCapacity = (SizeX + 1) * SizeY * 2 + 7;
            var builder = new StringBuilder(minRequiredCapacity);
            builder.Append("Matrix:");
            for (int x = 0; x < SizeX; x++)
            {
                builder.AppendLine();

                for (int y = 0; y < SizeY; y++)
                {
                    builder.Append($"{this[x, y]} ");
                }
            }
            return builder.ToString();
        }
    }


    [Serializable]
    public class MatrixMathException : Exception
    {
        public MatrixMathException() { }
        public MatrixMathException(string message) : base(message) { }
        public MatrixMathException(string message, Exception inner) : base(message, inner) { }
        protected MatrixMathException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
