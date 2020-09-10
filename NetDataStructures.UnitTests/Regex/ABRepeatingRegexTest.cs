using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.Regex
{
    [TestClass]
    public class ABRepeatingRegexTest : RegexAutomatonTestBase
    {
        protected override IEnumerable<(string Input, bool Expected)> TestData => new[]
        {
            ("", true),
            ("ab", true),
            ("abab", true),
            ("ababab", true),

            ("a", false),
            ("b", false),
            ("aba", false),
            ("abba", false),
            ("bab", false),
            ("babab", false),
            ("ababa", false),
        };
        
        protected override RegularExpression Target => new RegularExpression(new [] {'a', 'b'}, "(ab)*");
    }
}