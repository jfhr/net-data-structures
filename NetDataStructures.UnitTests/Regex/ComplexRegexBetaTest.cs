using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.Regex
{
    [TestClass]
    public class ComplexRegexBetaTest : RegexAutomatonTestBase
    {
        protected override IEnumerable<(string Input, bool Expected)> TestData => new[]
        {
            ("", true),
            ("bc", true),
            ("abc", true),
            ("aabc", true),
            ("aaabc", true),

            ("a", false),
            ("b", false),
            ("c", false),
            ("ab", false),
            ("ac", false),
            ("bac", false),
            ("abac", false),
            ("cc", false),
            ("cbc", false),
            ("cabc", false),
            ("caabc", false),
            ("abbc", false),
            ("aaabca", false),
        };
        
        protected override RegularExpression Target => new RegularExpression(new [] {'a', 'b', 'c'}, "(a*bc)∪ε∪∅");
    }
}