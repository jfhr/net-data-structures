using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.DFA
{
    [TestClass]
    public class DfaLengthIsDivisibleByThreeTest
    {
        private DeterministicFiniteStateAutomaton _target;

        [TestInitialize]
        public void Initialize()
        {
            _target = new DeterministicFiniteStateAutomaton(
                alphabet: new[] {'a', 'b'},
                states: new[] {"s0", "s1", "s2"},
                startState: "s0",
                delta: new Dictionary<(string, char), string>
                {
                    {("s0", 'a'), "s1"},
                    {("s0", 'b'), "s1"},
                    {("s1", 'a'), "s2"},
                    {("s1", 'b'), "s2"},
                    {("s2", 'a'), "s0"},
                    {("s2", 'b'), "s0"},
                },
                acceptStates: new[] {"s0"});
        }

        public static IEnumerable<object[]> TestData => new[]
        {
            new object[] {"aba", true},
            new object[] {"aab", true},
            new object[] {"aaa", true},
            new object[] {"", true},
            new object[] {"aaabbbabaaababb", true},
            new object[] {"aaaaaabbbbbb", true},

            new object[] {"ab", false},
            new object[] {"aa", false},
            new object[] {"a", false},
            new object[] {"b", false},
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
            _ = _target.Run("abax");
        }
    }
}