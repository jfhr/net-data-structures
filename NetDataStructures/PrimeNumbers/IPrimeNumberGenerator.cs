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
        /// Determines whether the given number is a prime number.
        /// </summary>
        bool IsPrime(BigInteger number);

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
        IEnumerable<BigInteger> GetPrimes(BigInteger startAt, BigInteger stopAt);
    }
}
