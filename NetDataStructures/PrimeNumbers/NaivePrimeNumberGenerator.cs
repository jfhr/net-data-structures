using System;
using System.Collections.Generic;
using System.Numerics;

namespace NetDataStructures.PrimeNumbers
{
    /// <summary>
    /// Provides a naive implementation of a prime number generator 
    /// that finds primes by checking all potential divisors of a number.
    /// </summary>
    public class NaivePrimeNumberGenerator : IPrimeNumberGenerator
    {
        public bool IsPrime(BigInteger integer)
        {
            if (integer < 2)
            {
                return false;
            }
            BigInteger limit = integer / 2;
            for (BigInteger i = 2; i <= limit; i++)
            {
                // division with remainder
                BigInteger.DivRem(integer, i, out BigInteger rem);
                // if remainder is 0, then integer is divisible by i, thus not a prime.
                if (rem.IsZero)
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerable<BigInteger> GetPrimes(BigInteger startAt, BigInteger stopAt)
        {
            if (stopAt <= startAt)
            {
                throw new ArgumentException("stopAt must be larger than startAt.", nameof(stopAt));
            }

            if (stopAt < 2)
            {
                yield break;
            }

            for (BigInteger i = startAt; i < stopAt; i++)
            {
                if (IsPrime(i))
                {
                    yield return i;
                }
            }
        }
    }
}
