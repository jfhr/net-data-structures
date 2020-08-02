using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetDataStructures.UnitTests.Utils
{
    [TestClass]
    public class AutomataUtilsTest
    {
        public static IEnumerable<object[]> GetIndexOfNumberAtEndOfStringTestData => new[]
        {
            new object[] {"0", 0, 0}, 
            new object[] {"1", 1, 0}, 
            new object[] {"500", 500, 0}, 
            new object[] {"s0", 0, 1}, 
            new object[] {"s1", 1, 1}, 
            new object[] {"s100q200", 200, 5}, 
            new object[] {"s100q", 5, 0}, 
        };
        
        [TestMethod, DynamicData(nameof(GetIndexOfNumberAtEndOfStringTestData))]
        public void TestGetIndexOfNumberAtEndOfString(string input, int expectedValue, int expectedIndex)
        {
            var (actualValue, actualIndex) = Automata.Internal.Utils.GetIndexOfNumberAtEndOfString(input);
            Assert.AreEqual(expectedValue, actualValue, "Incorrect value");
            Assert.AreEqual(expectedIndex, actualIndex, "Incorrect index");
        }
    }
}