using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.NDFINPDA
{
    /// <summary>
    /// PDA that accepts any nonzero number of 'a's followed by an equal number of 'b's.
    /// </summary>
    [TestClass]
    public class AnBnAutomatonTest
    {
        private NondeterministicFinalStateAcceptingPushdownAutomaton _target;

        [TestInitialize]
        public void Initialize()
        {
            _target = new NondeterministicFinalStateAcceptingPushdownAutomaton(
                alphabet: new[] {'a', 'b'},
                states: new[] {"sStart", "sAB", "sEnd"},
                stackAlphabet: new[] {'$', 'a', 'b', 'S'},
                initialStackItem: '$',
                startState: "sStart",
                delta: new Dictionary<(string State, char? Symbol, char Pop), IEnumerable<(string State, string Push)>>
                {
                    {("sStart", null, '$'), new[] {("sAB", "$S")}},
                    {("sAB", null, 'S'), new[] {("sAB", "ba"), ("sAB", "bSa")}},
                    {("sAB", 'a', 'a'), new[] {("sAB", "")}},
                    {("sAB", 'b', 'b'), new[] {("sAB", "")}},
                    {("sAB", null, '$'), new[] {("sEnd", "")}},
                },
                acceptStates: new[] {"sEnd"}
            );
        }

        public static IEnumerable<object[]> TestData => new[]
        {
            new object[] {"ab", true},
            new object[] {"aabb", true},
            new object[] {"aaabbb", true},

            new object[] {"", false},
            new object[] {"abb", false},
            new object[] {"bbaa", false},
            new object[] {"a", false},
            new object[] {"aa", false},
            new object[] {"bbb", false},
            new object[] {"aaabba", false},
            new object[] {"aaabab", false},
        };

        [TestMethod, DynamicData(nameof(TestData))]
        public void Result(string input, bool expected)
        {
            var actual = _target.Run(input);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void SymbolNotInAlphabet()
        {
            _ = _target.Run("abc");
        }
    }
}