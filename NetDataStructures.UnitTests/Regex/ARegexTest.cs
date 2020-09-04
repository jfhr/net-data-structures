using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.Regex
{
    [TestClass]
    public class ARegexTest : RegexAutomatonTestBase
    {
        protected override IEnumerable<(string Input, bool Expected)> TestData => new[]
        {
            ("a", true),

            ("", false),
            ("b", false),
            ("aa", false),
            ("ab", false),
            ("ba", false),
            ("bb", false),
            ("aba", false),
            ("bba", false),
        };
        
        protected override RegularExpression Target => new RegularExpression(new [] {'a', 'b'}, "a");
    }
}