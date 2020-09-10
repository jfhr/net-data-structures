using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.Regex
{
    [TestClass]
    public class ComplexRegexAlpha : RegexAutomatonTestBase
    {
        protected override IEnumerable<(string Input, bool Expected)> TestData => new[]
        {
            ("a", true),
            ("abb", true),
            ("abab", true),
            ("aabb", true),
            ("aabab", true),
            ("ababbb", true),
            ("aababbb", true),
            ("abbbab", true),
            ("aabbbab", true),
            ("abbbb", true),
            ("aabbbb", true),
            ("ababbab", true),
            ("aababbab", true),
            ("abbbbbb", true),
            ("ababbbbab", true),
            ("abbbabbab", true),
            ("aabbbabbab", true),

            ("", false),
            ("b", false),
            ("ab", false),
            ("bb", false),
            ("abba", false),
            ("abaaab", false),
        };
        
        protected override RegularExpression Target => new RegularExpression(new [] {'a', 'b'}, "a(a∪ε)((bab)*∪bb)*");
    }
}