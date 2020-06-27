using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.NFA
{
    [TestClass]
    public class NfaStartsWithAUnionWithEndsWithCTest
    {
        private NondeterministicFiniteStateAutomaton startsWithANfa;
        private NondeterministicFiniteStateAutomaton endsWithCNfa;

        [TestInitialize]
        public void Initialize()
        {
            var alphabet = new [] {'a', 'b', 'c'};
            startsWithANfa = new NondeterministicFiniteStateAutomaton(
                alphabet,
                new [] {"q0", "q1"},
                new [] {"q0"},
                new Dictionary<(string State, char Symbol), IEnumerable<string>>
                {
                    {("q0", 'a'), new [] {"q1"}},
                    
                    {("q1", 'a'), new [] {"q1"}},
                    {("q1", 'b'), new [] {"q1"}},
                    {("q1", 'c'), new [] {"q1"}},
                },
                new [] {"q1"}
            );
            endsWithCNfa = new NondeterministicFiniteStateAutomaton(
                alphabet,
                new [] {"q0", "q1"},
                new [] {"q0"},
                new Dictionary<(string State, char Symbol), IEnumerable<string>>
                {
                    {("q0", 'a'), new [] {"q0"}},
                    {("q0", 'b'), new [] {"q0"}},
                    {("q0", 'c'), new [] {"q0", "q1"}},
                },
                new [] {"q1"}
            );
        }

        public static IEnumerable<object[]> TestData => new[]
        {
            new object[] {"a", true},
            new object[] {"c", true},
            new object[] {"aab", true},
            new object[] {"bac", true},

            new object[] {"", false},
            new object[] {"bab", false},
            new object[] {"bacaccccab", false},
        };

        [TestMethod, DynamicData("TestData")]
        public void Results(string input, bool expected)
        {
            var target = startsWithANfa.UnionWith(endsWithCNfa);
            var result = target.Run(input);
            Assert.AreEqual(expected, result);
        }
    }
}