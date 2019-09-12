using System;
using System.Text;

namespace NetDataStructures.Matrices
{
    public struct Matrix
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
        /// Gets or sets the element of this matrix at the zero-based indices
        /// <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        public int this[int x, int y]
        {
            get => _matrix[x, y];
            set => _matrix[x, y] = value;
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
        private void TransformEachElement(Func<int, int, int, int> transformation, int[,] targetArray)
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    targetArray[x, y] = transformation(x, y, _matrix[x, y]);
                }
            }
        }



        /// <summary>
        /// Validates that the given matrix can be added to this matrix.
        /// </summary>
        /// <exception cref="MatrixMathException">
        /// The two matrices do not have the same dimensions.
        /// </exception>
        private void ValidateAdd(Matrix matrix)
        {
            if (SizeX != matrix.SizeX)
            {
                throw new MatrixMathException($"Attempt to add two matrices with different X-Sizes of {SizeX}, {matrix.SizeX}.");
            }
            if (SizeY != matrix.SizeY)
            {
                throw new MatrixMathException($"Attempt to add two matrices with different Y-Sizes of {SizeY}, {matrix.SizeY}.");
            }
        }

        /// <summary>
        /// Adds another matrix to the current matrix and returns
        /// a new matrix containing the resulting values.
        /// </summary>
        /// <param name="matrix">
        /// The matrix to add to the current matrix.
        /// </param>
        /// <exception cref="MatrixMathException">
        /// The two matrices have different dimensions.
        /// </exception>
        public Matrix Add(Matrix matrix)
        {
            ValidateAdd(matrix);
            int[,] newArray = new int[SizeX, SizeY];
            TransformEachElement((x, y, value) => value + matrix[x, y], newArray);
            return new Matrix(newArray);
        }

        /// <summary>
        /// Adds another matrix to the current matrix and writes the resulting 
        /// values in this matrix. This does not create a new matrix.
        /// </summary>
        /// <param name="matrix">
        /// The matrix to add to the current matrix.
        /// </param>
        /// <exception cref="MatrixMathException">
        /// The two matrices have different dimensions.
        /// </exception>
        public void AddInPlace(Matrix matrix)
        {
            ValidateAdd(matrix);
            TransformEachElement((x, y, value) => value + matrix[x, y], _matrix);
        }

        /// <summary>
        /// Adds two matrices of the same dimensions and returns the result as
        /// a new matrix.
        /// </summary>
        /// <exception cref="MatrixMathException">
        /// The two matrices are not of the same dimension.
        /// </exception>
        public static Matrix operator +(Matrix m1, Matrix m2) => m1.Add(m2);



        /// <summary>
        /// Calculates the product of this matrix and a scalar and returns the
        /// result as a new matrix.
        /// </summary>
        public Matrix Multiply(int scalar)
        {
            int[,] newArray = new int[SizeX, SizeY];
            TransformEachElement((x, y, value) => value * scalar, newArray);
            return new Matrix(newArray);
        }

        /// <summary>
        /// Calculates the product of this matrix and a scalar and writes the 
        /// result into the current matrix. This will not create a new matrix.
        /// </summary>
        public void MultiplyInPlace(int scalar)
        {
            TransformEachElement((x, y, value) => value * scalar, _matrix);
        }

        /// <summary>
        /// Calculates the product of a matrix and a scalar and returns the 
        /// result as a new matrix.
        /// </summary>
        /// <remarks>
        /// This is equivalent to calling <see cref="Matrix.Multiply(int)"/>.
        /// </remarks>
        public static Matrix operator *(Matrix m, int scalar) => m.Multiply(scalar);

        /// <summary>
        /// Calculates the product of a matrix and a scalar and returns the 
        /// result as a new matrix.
        /// </summary>
        /// <remarks>
        /// This is equivalent to calling <see cref="Matrix.Multiply(int)"/>.
        /// </remarks>
        public static Matrix operator *(int scalar, Matrix m) => m.Multiply(scalar);



        /// <summary>
        /// Validates that the given matrix can be multiplied with this matrix.
        /// </summary>
        /// <exception cref="MatrixMathException">
        /// The multiplication can't be performed because the y-size of this matrix 
        /// is different from the x-size of the target matrix.
        /// </exception>
        private void ValidateMultiply(Matrix matrix)
        {
            if (SizeX != matrix.SizeY)
            {
                throw new MatrixMathException($"Attempt to multiply matrix with Y-Size of {SizeY} with another matrix with X-Size {matrix.SizeX}.");
            }
        }

        /// <summary>
        /// Calculates the matrix product of this matrix and the given matrix 
        /// and return a new matrix containing the resulting values.
        /// This operation is not commutative.
        /// </summary>
        /// <exception cref="MatrixMathException">
        /// The Y-Dimension of this matrix is not equal to the X-Dimension of
        /// the given matrix.
        /// </exception>
        public Matrix Multiply(Matrix matrix)
        {
            ValidateMultiply(matrix);
            int[,] newArray = new int[SizeX, SizeY];
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < matrix.SizeY; y++)
                {
                    for (int n = 0; n < SizeY; n++)
                    {
                        newArray[x, y] += this[x, n] * matrix[n, y];
                    }
                }
            }
            return new Matrix(newArray);
        }

        /// <summary>
        /// Calculates the matrix product of two matrices of compatible sizes.
        /// This operation is not commutative.
        /// </summary>
        /// <exception cref="MatrixMathException">
        /// The Y-Dimension of <paramref name="m1"/> is not equal to the X-Dimension of <paramref name="m2"/>.
        /// </exception>
        public static Matrix operator *(Matrix m1, Matrix m2) => m1.Multiply(m2);



        /// <summary>
        /// Returns a new matrix that is the result of multiplying this matrix with -1.
        /// </summary>
        public static Matrix operator -(Matrix matrix) => matrix * -1;



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
