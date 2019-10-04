using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.PrimeNumbers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace NetDataStructures.UnitTests.PrimeNumbers
{
    [TestClass]
    public class PrimeNumberGeneratorUnitTest
    {
        /// <summary>
        /// Test cases for the <see cref="IPrimeNumberGenerator.IsPrime(BigInteger)"/> method.
        /// </summary>
        private static readonly (BigInteger, bool)[] isPrimeTestCases
            = new (BigInteger, bool)[]
            {
                (-1, false),
                (0, false),
                (1, false),
                (2, true),
                (3, true),
                (4, false),
                (5, true),
            };

        /// <summary>
        /// Test cases for the <see cref="IPrimeNumberGenerator.GetPrimes(BigInteger, BigInteger)"/> method.
        /// </summary>
        private static readonly (BigInteger, BigInteger, BigInteger[])[] getPrimesTestCases 
            = new (BigInteger, BigInteger, BigInteger[])[]
        {
            (1, 2, new BigInteger[] { }),
            (1, 3, new BigInteger[] { 2 }),
            (1, 4, new BigInteger[] { 2, 3 }),
            (1, 5, new BigInteger[] { 2, 3 }),
            (1, 6, new BigInteger[] { 2, 3, 5 }),
            (5, 6, new BigInteger[] { 5 }),
        };

        /// <summary>
        /// The targets to be tested. Each test case declared above will be run on each target.
        /// </summary>
        private static readonly IPrimeNumberGenerator[] targets = new IPrimeNumberGenerator[]
        {
            new NaivePrimeNumberGenerator(),
        };

        #region Test methods
        private static IEnumerable<object[]> GetPrimesTestCasesWithTargetTypes => getPrimesTestCases
            .SelectMany(test => targets
                .Select(target => new object[] { target, test.Item1, test.Item2, test.Item3 })
            );

        [TestMethod, DynamicData(nameof(GetPrimesTestCasesWithTargetTypes))]
        public void TestGetPrimes(IPrimeNumberGenerator target, BigInteger startAt, BigInteger stopAt, BigInteger[] expected)
        {
            BigInteger[] actual = target.GetPrimes(startAt, stopAt).ToArray();
            CollectionAssert.AreEqual(expected, actual,
                $"Expected: [{string.Join(',', expected)}], but got: [{string.Join(',', actual)}]. "
            );
        }
        

        private static IEnumerable<object[]> IsPrimeTestCasesWithTargetTypes => isPrimeTestCases
            .SelectMany(test => targets
                .Select(target => new object[] { target, test.Item1, test.Item2 })
            );

        [TestMethod, DynamicData(nameof(IsPrimeTestCasesWithTargetTypes))]
        public void TestIsPrime(IPrimeNumberGenerator target, BigInteger number, bool expected)
        {
            bool actual = target.IsPrime(number);
            Assert.AreEqual(expected, actual, $"IsPrime({number}): ");
        }

        #endregion
    }
}
