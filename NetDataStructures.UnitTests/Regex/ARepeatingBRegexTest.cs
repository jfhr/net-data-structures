using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.Regex
{
    [TestClass]
    public class ARepeatingBRegexTest : RegexAutomatonTestBase
    {
        protected override IEnumerable<(string Input, bool Expected)> TestData => new[]
        {
            ("b", true),
            ("ab", true),
            ("aab", true),
            ("aaab", true),

            ("", false),
            ("a", false),
            ("aa", false),
            ("abb", false),
            ("aba", false),
            ("bab", false),
            ("baab", false),
        };

        protected override RegularExpression Target => new RegularExpression(new [] {'a', 'b'}, "a*b");
    }
}