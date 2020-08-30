using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.Regex
{
    [TestClass]
    public class RegexToAutomatonTests
    {
        [TestMethod]
        public void EmptySetRegex()
        {
            var regex = new RegularExpression(new [] {'a', 'b'}, "∅");
            var automaton = regex.DeriveAutomaton();
            
            Assert.IsFalse(automaton.Run(""));
            Assert.IsFalse(automaton.Run("a"));
            Assert.IsFalse(automaton.Run("b"));
            Assert.IsFalse(automaton.Run("abba"));
        }
        
        [TestMethod]
        public void EmptyWordRegex()
        {
            var regex = new RegularExpression(new [] {'a', 'b'}, "ε");
            var automaton = regex.DeriveAutomaton();
            
            Assert.IsTrue(automaton.Run(""));
            Assert.IsFalse(automaton.Run("a"));
            Assert.IsFalse(automaton.Run("b"));
            Assert.IsFalse(automaton.Run("abba"));
        }
        
        [TestMethod]
        public void ARegex()
        {
            var regex = new RegularExpression(new [] {'a', 'b'}, "a");
            var automaton = regex.DeriveAutomaton();
            
            Assert.IsFalse(automaton.Run(""));
            Assert.IsTrue(automaton.Run("a"));
            Assert.IsFalse(automaton.Run("b"));
            Assert.IsFalse(automaton.Run("abba"));
        }
        
        [TestMethod]
        public void ABRepeatingRegex()
        {
            var regex = new RegularExpression(new [] {'a', 'b'}, "(ab)*");
            var automaton = regex.DeriveAutomaton();
            
            Assert.IsFalse(automaton.Run("a"));
            Assert.IsFalse(automaton.Run("b"));
            Assert.IsFalse(automaton.Run("abba"));
            Assert.IsFalse(automaton.Run("abaaab"));
            
            Assert.IsTrue(automaton.Run(""));
            Assert.IsTrue(automaton.Run("ab"));
            Assert.IsTrue(automaton.Run("abab"));
            Assert.IsTrue(automaton.Run("ababab"));
        }
        
        [TestMethod]
        public void ARepeatingBRegex()
        {
            var regex = new RegularExpression(new [] {'a', 'b'}, "a*b");
            var automaton = regex.DeriveAutomaton();
            
            Assert.IsFalse(automaton.Run(""));
            Assert.IsFalse(automaton.Run("a"));
            Assert.IsFalse(automaton.Run("aa"));
            Assert.IsFalse(automaton.Run("ba"));
            
            Assert.IsTrue(automaton.Run("b"));
            Assert.IsTrue(automaton.Run("ab"));
            Assert.IsTrue(automaton.Run("aab"));
        }
        
        // ⋅ ∪ ∅ ε 
        
        [TestMethod]
        public void ComplexRegexAlpha()
        {
            var regex = new RegularExpression(new [] {'a', 'b'}, "a(a∪ε)((bab)*∪bb)*");
            var automaton = regex.DeriveAutomaton();
            
            Assert.IsFalse(automaton.Run(""));
            Assert.IsFalse(automaton.Run("b"));
            Assert.IsFalse(automaton.Run("ab"));
            Assert.IsFalse(automaton.Run("bb"));
            Assert.IsFalse(automaton.Run("abba"));
            Assert.IsFalse(automaton.Run("abaaab"));
            
            Assert.IsTrue(automaton.Run("a"));
            Assert.IsTrue(automaton.Run("abb"));
            Assert.IsTrue(automaton.Run("abab"));
            Assert.IsTrue(automaton.Run("aabb"));
            Assert.IsTrue(automaton.Run("aabab"));
            Assert.IsTrue(automaton.Run("ababbb"));
            Assert.IsTrue(automaton.Run("aababbb"));
            Assert.IsTrue(automaton.Run("abbbab"));
            Assert.IsTrue(automaton.Run("aabbbab"));
            Assert.IsTrue(automaton.Run("abbbb"));
            Assert.IsTrue(automaton.Run("aabbbb"));
            Assert.IsTrue(automaton.Run("ababbab"));
            Assert.IsTrue(automaton.Run("aababbab"));
            Assert.IsTrue(automaton.Run("abbbbbb"));
            Assert.IsTrue(automaton.Run("ababbbbab"));
            Assert.IsTrue(automaton.Run("abbbabbab"));
            Assert.IsTrue(automaton.Run("aabbbabbab"));
        }
        
        [TestMethod]
        public void ComplexRegexBeta()
        {
            var regex = new RegularExpression(new [] {'a', 'b', 'c'}, "(a*bc)∪ε∪∅");
            var automaton = regex.DeriveAutomaton();
            
            Assert.IsFalse(automaton.Run("a"));
            Assert.IsFalse(automaton.Run("b"));
            Assert.IsFalse(automaton.Run("c"));
            Assert.IsFalse(automaton.Run("ab"));
            Assert.IsFalse(automaton.Run("ac"));
            Assert.IsFalse(automaton.Run("bac"));
            Assert.IsFalse(automaton.Run("abac"));
            Assert.IsFalse(automaton.Run("cc"));
            Assert.IsFalse(automaton.Run("cbc"));
            Assert.IsFalse(automaton.Run("cabc"));
            Assert.IsFalse(automaton.Run("caabc"));
            Assert.IsFalse(automaton.Run("abbc"));
            Assert.IsFalse(automaton.Run("aaabca"));
            
            Assert.IsTrue(automaton.Run(""));
            Assert.IsTrue(automaton.Run("bc"));
            Assert.IsTrue(automaton.Run("abc"));
            Assert.IsTrue(automaton.Run("aabc"));
            Assert.IsTrue(automaton.Run("aaabc"));
        }
    }
}