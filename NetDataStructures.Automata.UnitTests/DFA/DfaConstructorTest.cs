using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.Automata.UnitTests.DFA
{
    [TestClass]
    public class DfaConstructorTest
    {
        [TestMethod]
        public void Constructor_Expect_Ok()
        {
            _ = new DeterministicFiniteStateAutomaton(
                alphabet: new HashSet<char> {'a', 'b'},
                states: new [] {"s0", "s1", "s2"},
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
                acceptStates: new [] {"s0"});
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AlphabetIsNull_Expect_Exception()
        {
            _ = new DeterministicFiniteStateAutomaton(
                alphabet: null,
                states: new [] {"s0", "s1", "s2"},
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
                acceptStates: new [] {"s0"});
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StatesIsNull_Expect_Exception()
        {
            _ = new DeterministicFiniteStateAutomaton(
                alphabet: new HashSet<char> {'a', 'b'},
                states: null,
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
                acceptStates: new [] {"s0"});
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartStateIsNull_Expect_Exception()
        {
            _ = new DeterministicFiniteStateAutomaton(
                alphabet: new HashSet<char> {'a', 'b'},
                states: new [] {"s0", "s1", "s2"},
                startState: null,
                delta: new Dictionary<(string, char), string>
                {
                    {("s0", 'a'), "s1"},
                    {("s0", 'b'), "s1"},
                    {("s1", 'a'), "s2"},
                    {("s1", 'b'), "s2"},
                    {("s2", 'a'), "s0"},
                    {("s2", 'b'), "s0"},
                },
                acceptStates: new [] {"s0"});
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeltaIsNull_Expect_Exception()
        {
            _ = new DeterministicFiniteStateAutomaton(
                alphabet: new HashSet<char> {'a', 'b'},
                states: new [] {"s0", "s1", "s2"},
                startState: "s0",
                delta: null,
                acceptStates: new [] {"s0"});
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AcceptStatesIsNull_Expect_Exception()
        {
            _ = new DeterministicFiniteStateAutomaton(
                alphabet: new HashSet<char> {'a', 'b'},
                states: new [] {"s0", "s1", "s2"},
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
                acceptStates: null);
        }
    }
}