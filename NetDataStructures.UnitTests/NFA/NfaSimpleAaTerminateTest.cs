using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.NFA
{
    /// <summary>
    /// Simple NFA that tests if a word ends with 'aa'.
    /// </summary>
    [TestClass]
    public class NfaSimpleAaTerminateTest
    {
        private NondeterministicFiniteStateAutomaton _target;

        [TestInitialize]
        public void Initialize()
        {
            _target = new NondeterministicFiniteStateAutomaton(
                new [] {'a', 'b'},
                new [] {"q0", "q1", "q2"},
                new [] {"q0"},
                new Dictionary<(string State, char Symbol), IEnumerable<string>>
                {
                    {("q0", 'a'), new [] {"q0", "q1"}},
                    {("q0", 'b'), new [] {"q0"}},
                    {("q1", 'a'), new [] {"q2"}},
                },
                new [] {"q2"});
        }

        [TestMethod]
        public void DerivedDfaStructure()
        {
            var result = _target.DeriveDeterministic();

            Assert.AreEqual(3, result.States.Count());
            Assert.AreEqual("{q0}", result.StartState);
            Assert.AreEqual(1, result.AcceptStates.Count());
        }

        public static IEnumerable<object[]> TestData => new[]
        {
            new object[] {"baaa", true},
            new object[] {"baa", true},
            new object[] {"aa", true},
            new object[] {"abaabaa", true},
            
            new object[] {"", false},
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

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void SymbolNotInAlphabet()
        {
            _ = _target.Run("abcab");
        }
    }
}