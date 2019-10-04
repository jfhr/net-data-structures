using System.Collections.Generic;
using System.Numerics;

namespace NetDataStructures.PrimeNumbers
{
    /// <summary>
    /// Interface for algorithms that generate prime numbers.
    /// </summary>
    public interface IPrimeNumberGenerator
    {
        /// <summary>
        /// Generates all prime numbers starting at the given starting number,
        /// that are not larger than the given stop number.
        /// </summary>
        /// <param name="startAt">
        /// Start number, only prime numbers not smaller than this number will be returned.
        /// </param>
        /// <param name="stopAt">
        /// Stop number, only prime numbers not larger than this number will be returned.
        /// </param>
        /// <returns>
        /// An ordered sequence containing all prime numbers within the given limits.
        /// </returns>
        IEnumerable<int> GetPrimes(int startAt, int stopAt);

        /// <summary>
        /// Generates all prime numbers starting at the given starting number,
        /// that are not larger than the given stop number.
        /// </summary>
        /// <remarks>
        /// Like <see cref="GetPrimes(int, int)"/>, but returns <see langword="long"/>.
        /// Use this for large numbers.
        /// </remarks>
        IEnumerable<long> GetLongPrimes(long startAt, long stopAt);

        /// <summary>
        /// Generates all prime numbers starting at the given starting number,
        /// that are not larger than the given stop number.
        /// </summary>
        /// <remarks>
        /// Like <see cref="GetPrimes(int, int)"/>, but returns <see cref="BigInteger"/>.
        /// Use this for very large numbers.
        /// </remarks>
        IEnumerable<BigInteger> GetBigPrimes(BigInteger startAt, BigInteger stopAt);
    }
}
