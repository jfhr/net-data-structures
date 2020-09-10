using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.Regex
{
    [TestClass]
    public class AOrBBRegexTest : RegexAutomatonTestBase
    {
        protected override IEnumerable<(string Input, bool Expected)> TestData => new[]
        {
            ("a", true),
            ("bb", true),

            ("", false),
            ("b", false),
            ("ab", false),
            ("aa", false),
            ("abb", false),
            ("bbbb", false),
        };

        protected override RegularExpression Target => new RegularExpression(new[] {'a', 'b'}, "a∪bb");
    }
}