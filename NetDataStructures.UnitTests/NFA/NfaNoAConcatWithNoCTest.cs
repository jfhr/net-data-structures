using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.NFA
{
    [TestClass]
    public class NfaNoAConcatWithNoCTest
    {
        private NondeterministicFiniteStateAutomaton noA;
        private NondeterministicFiniteStateAutomaton noC;

        [TestInitialize]
        public void Initialize()
        {
            var alphabet = new [] {'a', 'b', 'c'};
            noA = new NondeterministicFiniteStateAutomaton(
                alphabet,
                new [] {"q0"},
                new [] {"q0"},
                new Dictionary<(string State, char Symbol), IEnumerable<string>>
                {                    
                    {("q0", 'b'), new [] {"q0"}},
                    {("q0", 'c'), new [] {"q0"}},
                },
                new [] {"q0"}
            );
            noC = new NondeterministicFiniteStateAutomaton(
                alphabet,
                new [] {"q0"},
                new [] {"q0"},
                new Dictionary<(string State, char Symbol), IEnumerable<string>>
                {
                    {("q0", 'a'), new [] {"q0"}},
                    {("q0", 'b'), new [] {"q0"}},
                },
                new [] {"q0"}
            );
        }

        public static IEnumerable<object[]> TestData => new[]
        {
            new object[] {"", true},
            new object[] {"a", true},
            new object[] {"c", true},
            new object[] {"aab", true},
            new object[] {"bab", true},
            new object[] {"cccaaa", true},
            new object[] {"cccbbbaaa", true},

            new object[] {"ac", false}, 
            new object[] {"acc", false}, 
            new object[] {"acac", false}, 
            new object[] {"bac", false},
            new object[] {"ababac", false}, 
            new object[] {"bacaccccab", false},
            new object[] {"aaabbbccc", false}, 
        };

        [TestMethod, DynamicData("TestData")]
        public void Results(string input, bool expected)
        {
            var target = noA.ConcatWith(noC);
            var result = target.Run(input);
            Assert.AreEqual(expected, result);
        }
    }
}