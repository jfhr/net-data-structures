using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.NFA
{
    /// <summary>
    /// NFA that checks for the following regular expression:
    /// <c> a*(ba*c ∪ c) </c>
    /// This implementation also has some redundant states.
    /// </summary>
    [TestClass]
    public class NfaRegexATest
    {
        private NondeterministicFiniteStateAutomaton _target;

        [TestInitialize]
        public void Initialize()
        {
            _target = new NondeterministicFiniteStateAutomaton(
                new [] {'a', 'b', 'c'},
                new [] {"q0", "q1", "q2", "q3", "q4"},
                new [] {"q0"},
                new Dictionary<(string State, char Symbol), IEnumerable<string>>
                {
                    {("q0", 'a'), new [] {"q0"}},
                    {("q0", 'b'), new [] {"q1"}},
                    {("q0", 'c'), new [] {"q2"}},

                    {("q1", 'a'), new [] {"q1"}},
                    {("q1", 'c'), new [] {"q2"}},
                    
                    {("q3", 'c'), new [] {"q2"}},

                    {("q4", 'a'), new [] {"q4"}},
                    {("q4", 'b'), new [] {"q2"}},
                    {("q4", 'c'), new [] {"q3"}},
                },
                new [] {"q2", "q3"});
        }

        public static IEnumerable<object[]> TestData => new[]
        {
            new object[] {"c", true},
            new object[] {"ac", true},
            new object[] {"aabc", true},
            new object[] {"aabaaac", true},
            new object[] {"abac", true},
            
            new object[] {"", false},
            new object[] {"baaa", false},
            new object[] {"aa", false},
            new object[] {"a", false},
            new object[] {"baab", false},
            new object[] {"baababaaabaabbbbaaaab", false},
        };

        [TestMethod, DynamicData(nameof(TestData))]
        public void Result(string input, bool expected)
        {
            var result = _target.Run(input);
            var dfa = _target.DeriveDeterministic();
            var dfaResult = dfa.Run(input);
            dfa.MinimizeInPlace();
            var minDfaResult = dfa.Run(input);

            Assert.AreEqual(expected, result, "NFA returned wrong result");
            Assert.AreEqual(expected, dfaResult, "NFA returned correct result, but derived DFA did not");
            Assert.AreEqual(expected, minDfaResult, "NFA returned correct result, " +
                                                    "but derived minimized DFA did not");
        }
    }
}