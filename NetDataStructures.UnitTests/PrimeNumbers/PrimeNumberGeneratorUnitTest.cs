using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.PrimeNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace NetDataStructures.UnitTests.PrimeNumbers
{
    [TestClass]
    public class PrimeNumberGeneratorUnitTest
    {
        /// <summary>
        /// Test cases for prime number generators.
        /// </summary>
        private static readonly TestCase[] testCases = new TestCase[]
        {
            new TestCase(1, 2, new BigInteger[] { 2 }, TestCaseType.Int32),
            new TestCase(1, 3, new BigInteger[] { 2, 3 }, TestCaseType.Int32),
        };

        /// <summary>
        /// The targets to be tested. Each test case declared above will be run on each target.
        /// </summary>
        private static readonly IPrimeNumberGenerator[] targets = new IPrimeNumberGenerator[]
        {
            new NaivePrimeNumberGenerator(),
        };

        #region Parameter generators
        private static IEnumerable<object[]> Int32Cases => testCases
            .Where(x => x.Type <= TestCaseType.Int32)
            .SelectMany(test => targets
                .Select(target => new object[] { target, test.StartAt, test.StopAt, test.Expected })
            );

        private static IEnumerable<object[]> Int64Cases => testCases
            .Where(x => x.Type <= TestCaseType.Int64)
            .SelectMany(test => targets
                .Select(target => new object[] { target, test.StartAt, test.StopAt, test.Expected })
            );

        private static IEnumerable<object[]> BigIntCases => testCases
            .SelectMany(test => targets
                .Select(target => new object[] { target, test.StartAt, test.StopAt, test.Expected })
            );
        #endregion

        #region Test methods
        [TestMethod, DynamicData(nameof(Int32Cases))]
        public void TestPrimeNumberGenerator(IPrimeNumberGenerator target, int startAt, int stopAt, int[] expected)
        {
            int[] actual = target.GetPrimes(startAt, stopAt).ToArray();
            CollectionAssert.AreEqual(expected, actual, "Did not generate the expected primes in the expected order.");
        }

        [TestMethod, DynamicData(nameof(Int64Cases))]
        public void TestLongPrimeNumberGenerator(IPrimeNumberGenerator target, long startAt, long stopAt, long[] expected)
        {
            long[] actual = target.GetLongPrimes(startAt, stopAt).ToArray();
            CollectionAssert.AreEqual(expected, actual, "Did not generate the expected primes in the expected order.");
        }

        [TestMethod, DynamicData(nameof(BigIntCases))]
        public void TestBigPrimeNumberGenerator(IPrimeNumberGenerator target, BigInteger startAt, BigInteger stopAt, BigInteger[] expected)
        {
            BigInteger[] actual = target.GetBigPrimes(startAt, stopAt).ToArray();
            CollectionAssert.AreEqual(expected, actual, "Did not generate the expected primes in the expected order.");
        }
        #endregion
    }

    public class TestCase
    {
        public BigInteger StartAt { get; }
        public BigInteger StopAt { get; }
        public BigInteger[] Expected { get; }
        public TestCaseType Type { get; }

        public TestCase(BigInteger startAt, BigInteger stopAt, BigInteger[] expected, TestCaseType type = TestCaseType.BigInt)
        {
            StartAt = startAt;
            StopAt = stopAt;
            Expected = expected ?? throw new ArgumentNullException(nameof(expected));
            Type = type;
        }
    }

    public enum TestCaseType
    {
        Int32, Int64, BigInt,
    }
}
