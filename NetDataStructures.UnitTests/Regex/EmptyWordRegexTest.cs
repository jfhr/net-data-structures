using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.Regex
{
    [TestClass]
    public class EmptyWordRegexTest : RegexAutomatonTestBase
    {
        protected override IEnumerable<(string Input, bool Expected)> TestData => new[]
        {
            ("", true),
            
            ("a", false),
            ("b", false),
            ("aa", false),
            ("ab", false),
            ("ba", false),
            ("bb", false),
            ("aba", false),
            ("bba", false),
            ("abbba", false),
        };
        
        protected override RegularExpression Target => new RegularExpression(new [] {'a', 'b'}, "ε");
    }
}