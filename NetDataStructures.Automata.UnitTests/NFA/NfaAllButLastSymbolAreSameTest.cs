using System.Collections.Generic;
using NetDataStructures.Automata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetDataStructures.Automata.UnitTests.NFA
{
    /// <summary>
    /// NFA that allows words where all but the last symbol (or all symbols) are the same.
    /// This implementation has multiple start states.
    /// </summary>
    [TestClass]
    public class NfaAllButLastSymbolAreSameTest
    {
        private NondeterministicFiniteStateAutomaton _target;

        [TestInitialize]
        public void Initialize()
        {
            _target = new NondeterministicFiniteStateAutomaton(
                new [] {'a', 'b', 'c'},
                new [] {"q0", "q1", "q2", "q3"},
                new [] {"q0", "q1", "q2"},
                new Dictionary<(string State, char Symbol), IEnumerable<string>>
                {
                    {("q0", 'a'), new [] {"q0", "q3"}},
                    {("q0", 'b'), new [] {"q3"}},
                    {("q0", 'c'), new [] {"q3"}},

                    {("q1", 'a'), new [] {"q3"}},
                    {("q1", 'b'), new [] {"q1", "q3"}},
                    {("q1", 'c'), new [] {"q3"}},

                    {("q2", 'a'), new [] {"q3"}},
                    {("q2", 'b'), new [] {"q3"}},
                    {("q2", 'c'), new [] {"q2", "q3"}},
                },
                new [] {"q3"});
        }

        public static IEnumerable<object[]> TestData => new[]
        {
            new object[] {"a", true}, 
            new object[] {"c", true}, 
            new object[] {"ac", true}, 
            new object[] {"ab", true}, 
            new object[] {"aaaac", true}, 
            new object[] {"bbbbbb", true}, 
            new object[] {"bbbbbba", true},
            
            new object[] {"", false}, 
            new object[] {"abc", false}, 
            new object[] {"bbbbbbaa", false}, 
            new object[] {"bbbbbbac", false}, 
            new object[] {"acacbbba", false}, 
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