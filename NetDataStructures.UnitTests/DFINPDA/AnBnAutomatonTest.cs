using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.DFINPDA
{
    /// <summary>
    /// PDA that accepts any palindrome made out of 'a's and 'b's with a single 's' in the center.
    /// </summary>
    [TestClass]
    public class CenteredPalindromeAutomatonTest
    {
        private DeterministicFinalStateAcceptingPushdownAutomaton _target;

        [TestInitialize]
        public void Initialize()
        {
            _target = new DeterministicFinalStateAcceptingPushdownAutomaton(
                alphabet: new[] {'a', 'b', 's'},
                states: new[] {"sStart", "sBef", "sAft", "sEnd"},
                stackAlphabet: new[] {'$', 'a', 'b'},
                initialStackItem: '$',
                startState: "sStart",
                delta: new Dictionary<(string State, char? Symbol, char Pop), (string State, string Push)>
                {
                    {("sStart", 'a', '$'), ("sBef", "$a")},
                    {("sStart", 'b', '$'), ("sBef", "$b")},

                    {("sBef", 'a', 'a'), ("sBef", "aa")},
                    {("sBef", 'a', 'b'), ("sBef", "ba")},
                    {("sBef", 'b', 'a'), ("sBef", "ab")},
                    {("sBef", 'b', 'b'), ("sBef", "bb")},

                    {("sBef", 's', 'a'), ("sAft", "a")},
                    {("sBef", 's', 'b'), ("sAft", "b")},

                    {("sAft", 'a', 'a'), ("sAft", "")},
                    {("sAft", 'b', 'b'), ("sAft", "")},

                    {("sAft", null, '$'), ("sEnd", "")},
                },
                acceptStates: new[] {"sEnd"}
            );
        }

        public static IEnumerable<object[]> TestData => new[]
        {
            new object[] {"asa", true},
            new object[] {"absba", true},
            new object[] {"ababbbasabbbaba", true},

            new object[] {"", false},
            new object[] {"s", false},
            new object[] {"asaa", false},
            new object[] {"asbb", false},
            new object[] {"asb", false},
            new object[] {"aas", false},
            new object[] {"abba", false},
            new object[] {"abbsa", false},
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