using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NetDataStructures.PrimeNumbers
{
    public class NaivePrimeNumberGenerator : IPrimeNumberGenerator
    {
        private bool IsPrime(BigInteger integer)
        {
            if (integer < 2)
            {
                return false;
            }
            BigInteger limit = integer / 2;
            for (BigInteger i = 2; i < limit; i++)
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

        private bool IsPrime(long integer)
        {
            long limit = Convert.ToInt64(Math.Sqrt(integer));
            for (long i = 2; i < limit; i++)
            {
                if (integer % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
        
        private bool IsPrime(int integer)
        {
            int limit = Convert.ToInt32(Math.Sqrt(integer));
            for (int i = 2; i < limit; i++)
            {
                if (integer % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerable<BigInteger> GetBigPrimes(BigInteger startAt, BigInteger stopAt)
        {
            for (BigInteger i = startAt; i <= stopAt; i++)
            {
                if (IsPrime(i))
                {
                    yield return i;
                }
            }
        }

        public IEnumerable<long> GetLongPrimes(long startAt, long stopAt)
        {
            for (long i = startAt; i <= stopAt; i++)
            {
                if (IsPrime(i))
                {
                    yield return i;
                }
            }
        }

        public IEnumerable<int> GetPrimes(int startAt, int stopAt)
        {
            for (int i = startAt; i <= stopAt; i++)
            {
                if (IsPrime(i))
                {
                    yield return i;
                }
            }
        }
    }
}
