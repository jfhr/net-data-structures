using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.DFA
{
    [TestClass]
    public class DfaMinimizeTest
    {
        /// <summary>
        /// Simple automaton that accepts words ending with 'a'.
        /// </summary>
        [TestMethod]
        public void SimpleFourStateAutomaton()
        {
            // q3 can be removed bc it is not reachable
            // q2 can be removed via reduction
            var target = new DeterministicFiniteStateAutomaton(
                new [] { 'a', 'b' },
                new [] { "q0", "q1", "q2", "q3" },
                "q0",
                new Dictionary<(string, char), string>
                {
                    {("q0", 'a'), "q1"},
                    {("q0", 'b'), "q2"},
                    {("q1", 'a'), "q1"},
                    {("q1", 'b'), "q0"},
                    {("q2", 'a'), "q1"},
                    {("q2", 'b'), "q2"},
                    {("q3", 'a'), "q2"},
                    {("q3", 'b'), "q0"},
                }, 
                new [] { "q1" }
                );
            
            target.MinimizeInPlace();
            
            // Remaining states should be q0 and q1
            Assert.AreEqual(2, target.States.Count());
            Assert.IsTrue(target.States.Contains("q0"));
            Assert.IsTrue(target.States.Contains("q1"));
            
            // q0 should remain initial state
            Assert.AreEqual("q0", target.StartState);
            
            // q1 should remain only accepting state
            Assert.AreEqual(1, target.AcceptStates.Count());
            Assert.IsTrue(target.AcceptStates.Contains("q1"));
            
            // delta should be reduced
            Assert.AreEqual(4, target.Delta.Count);
            foreach (var (key, value) in target.Delta)
            {
                Assert.IsTrue(target.States.Contains(key.Item1));
                Assert.IsTrue(target.States.Contains(value));
            }
        }
        
        /// <summary>
        /// Very simple automaton that accepts words with an even number of symbols.
        /// </summary>
        [TestMethod]
        public void SimpleCharacterCountAutomaton()
        {
            // q2 and q3 can be removed
            var target = new DeterministicFiniteStateAutomaton(
                new [] { 'a' },
                new [] { "q0", "q1", "q2", "q3" },
                "q0",
                new Dictionary<(string, char), string>
                {
                    {("q0", 'a'), "q1"},
                    {("q1", 'a'), "q2"},
                    {("q2", 'a'), "q3"},
                    {("q3", 'a'), "q0"},
                }, 
                new [] { "q1", "q3" }
            );
            
            target.MinimizeInPlace();
            
            // Remaining states should be q0 and (q1 or q3)
            Assert.AreEqual(2, target.States.Count());
            
            // q0 should remain initial state
            Assert.AreEqual("q0", target.StartState);
            
            // (q1 or q3) should be only accepting state
            Assert.AreEqual(1, target.AcceptStates.Count());
            Assert.IsTrue(target.AcceptStates.Contains("q1") || target.AcceptStates.Contains("q3"));
            
            // delta should be reduced
            Assert.AreEqual(2, target.Delta.Count);
            foreach (var (key, value) in target.Delta)
            {
                Assert.IsTrue(target.States.Contains(key.Item1));
                Assert.IsTrue(target.States.Contains(value));
            }
        }
    }
}