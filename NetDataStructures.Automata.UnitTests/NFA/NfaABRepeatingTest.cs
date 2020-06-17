using System.Collections.Generic;
using NetDataStructures.Automata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetDataStructures.Automata.UnitTests.NFA
{
    [TestClass]
    public class NfaABRepeatingTest
    {
        private NondeterministicFiniteStateAutomaton abNfa;

        [TestInitialize]
        public void Initialize()
        {
            abNfa = new NondeterministicFiniteStateAutomaton(
                new[] {'a', 'b', 'c'},
                new[] {"q0", "q1", "q2"},
                new[] {"q0"},
                new Dictionary<(string State, char Symbol), IEnumerable<string>>
                {
                    {("q0", 'a'), new[] {"q1"}},
                    {("q1", 'b'), new[] {"q2"}},
                },
                new[] {"q2"}
            );
        }
        
        public static IEnumerable<object[]> TestData => new[]
        {
            new object[] {"", true}, 
            new object[] {"ab", true}, 
            new object[] {"abab", true}, 
            new object[] {"ababab", true}, 
            
            new object[] {"a", false}, 
            new object[] {"c", false}, 
            new object[] {"ac", false}, 
            new object[] {"abc", false}, 
            new object[] {"aaaac", false}, 
            new object[] {"bbbbbb", false}, 
            new object[] {"bbbbbba", false},
            new object[] {"bbbbbbaa", false}, 
            new object[] {"bbbbbbac", false}, 
            new object[] {"acacbbba", false}, 
        };

        [TestMethod, DynamicData(nameof(TestData))]
        public void Result(string input, bool expected)
        {
            var target = abNfa.Repeating();
            var result = target.Run(input);
            Assert.AreEqual(expected, result);
        }
    }
}