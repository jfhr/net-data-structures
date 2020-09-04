using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.Regex
{
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
        public void AOrBBRegex()
        {
            var regex = new RegularExpression(new[] {'a', 'b'}, "a∪bb");
            var automaton = regex.DeriveAutomaton();
            
            Assert.IsTrue(automaton.Run("a"));
            Assert.IsTrue(automaton.Run("bb"));
            
            Assert.IsFalse(automaton.Run(""));
            Assert.IsFalse(automaton.Run("b"));
            Assert.IsFalse(automaton.Run("ab"));
            Assert.IsFalse(automaton.Run("abb"));
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
        
        /// <summary>
        /// Regex that accepts a date in the form MM.DD , 02.29 is not allowed.
        /// </summary>
        [TestMethod]
        public void DateRegex()
        {
            const string _1to9 = "(1∪2∪3∪4∪5∪6∪7∪8∪9)";
            const string _0to8 = "(0∪1∪2∪3∪4∪5∪6∪7∪8)";
            const string _0to9 = "(0∪1∪2∪3∪4∪5∪6∪7∪8∪9)";

            string feb = $"02.(0{_1to9}∪1{_0to9}∪2{_0to8})";
            string m30 = $"(04∪06∪09∪11).(0{_1to9}∪1{_0to9}∪2{_0to9}∪30)";
            string m31 = $"(01∪03∪05∪07∪08∪10∪12).(0{_1to9}∪1{_0to9}∪2{_0to9}∪30∪31)";

            var regex = new RegularExpression(
                new [] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'}, 
                $"{feb}∪{m30}∪{m31}"
                );
            
            // Using dynamic rn to combine DFA and NFA, should probably introduce an interface
            // e.g. IAutomaton { bool Run(string input); ... } soon.
            var testAutomata = new dynamic[]
            {
                regex.DeriveAutomaton(),
                regex.DeriveAutomaton().DeriveDeterministic(),
                regex.DeriveAutomaton().DeriveDeterministic().Minimize(),
            };

            foreach (var automaton in testAutomata)
            {
                Assert.IsTrue(automaton.Run("01.01"));
                Assert.IsTrue(automaton.Run("01.02"));
                Assert.IsTrue(automaton.Run("01.03"));
                Assert.IsTrue(automaton.Run("01.30"));
                Assert.IsTrue(automaton.Run("01.31"));
                Assert.IsTrue(automaton.Run("02.01"));
                Assert.IsTrue(automaton.Run("02.27"));
                Assert.IsTrue(automaton.Run("02.28"));
                Assert.IsTrue(automaton.Run("03.01"));
                Assert.IsTrue(automaton.Run("03.31"));
                Assert.IsTrue(automaton.Run("04.30"));
                Assert.IsTrue(automaton.Run("05.31"));
                Assert.IsTrue(automaton.Run("06.30"));
                Assert.IsTrue(automaton.Run("07.31"));
                Assert.IsTrue(automaton.Run("08.31"));
                Assert.IsTrue(automaton.Run("09.30"));
                Assert.IsTrue(automaton.Run("10.31"));
                Assert.IsTrue(automaton.Run("11.30"));
                Assert.IsTrue(automaton.Run("12.31"));

                Assert.IsFalse(automaton.Run(""));
                Assert.IsFalse(automaton.Run("01.00"));
                Assert.IsFalse(automaton.Run("00.01"));
                Assert.IsFalse(automaton.Run("00.00"));
                Assert.IsFalse(automaton.Run("01.32"));
                Assert.IsFalse(automaton.Run("01.33"));
                Assert.IsFalse(automaton.Run("01.40"));
                Assert.IsFalse(automaton.Run("01.50"));
                Assert.IsFalse(automaton.Run("02.29"));
                Assert.IsFalse(automaton.Run("02.30"));
                Assert.IsFalse(automaton.Run("02.31"));
                Assert.IsFalse(automaton.Run("02.32"));
                Assert.IsFalse(automaton.Run("03.32"));
                Assert.IsFalse(automaton.Run("04.31"));
                Assert.IsFalse(automaton.Run("05.32"));
                Assert.IsFalse(automaton.Run("06.31"));
                Assert.IsFalse(automaton.Run("07.32"));
                Assert.IsFalse(automaton.Run("08.32"));
                Assert.IsFalse(automaton.Run("09.31"));
                Assert.IsFalse(automaton.Run("10.32"));
                Assert.IsFalse(automaton.Run("11.31"));
                Assert.IsFalse(automaton.Run("12.32"));
                Assert.IsFalse(automaton.Run("13.00"));
                Assert.IsFalse(automaton.Run("13.01"));
                Assert.IsFalse(automaton.Run("13.02"));
                Assert.IsFalse(automaton.Run("13.30"));
            }
        }
    }
}