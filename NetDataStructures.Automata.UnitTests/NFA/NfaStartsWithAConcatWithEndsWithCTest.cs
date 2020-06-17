using System.Collections.Generic;
using NetDataStructures.Automata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetDataStructures.Automata.UnitTests.NFA
{
    [TestClass]
    public class NfaStartsWithAConcatWithEndsWithCTest
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
            new object[] {"ac", true}, 
            new object[] {"acc", true}, 
            new object[] {"acac", true}, 
            new object[] {"ababac", true}, 
            new object[] {"aaabbbccc", true}, 

            new object[] {"a", false},
            new object[] {"c", false},
            new object[] {"aab", false},
            new object[] {"bac", false},
            new object[] {"", false},
            new object[] {"bab", false},
            new object[] {"bacaccccab", false},
        };

        [TestMethod, DynamicData("TestData")]
        public void Results(string input, bool expected)
        {
            var target = startsWithANfa.ConcatWith(endsWithCNfa);
            var result = target.Run(input);
            Assert.AreEqual(expected, result);
        }
    }
}