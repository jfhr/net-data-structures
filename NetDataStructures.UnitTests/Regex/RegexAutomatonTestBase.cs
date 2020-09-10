using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.Regex
{
    public abstract class RegexAutomatonTestBase
    {
        protected abstract IEnumerable<(string Input, bool Expected)> TestData { get; }
        protected abstract RegularExpression Target { get; }
        
        [TestMethod]
        public void RunNfa()
        {
            RunTestsInternal(Target
                .DeriveAutomaton());
        }

        [TestMethod]
        public void RunDfa()
        {
            RunTestsInternal(Target
                .DeriveAutomaton()
                .DeriveDeterministic());
        }

        [TestMethod]
        public void RunDfaMinimized()
        {
            RunTestsInternal(Target
                .DeriveAutomaton()
                .DeriveDeterministic()
                .Minimize());
        }

        private void RunTestsInternal(IAutomaton nfa)
        {
            foreach (var (input, expected) in TestData)
            {
                Assert.AreEqual(expected, nfa.Run(input), $"Input: <{input}>");
            }
        }
    }
}