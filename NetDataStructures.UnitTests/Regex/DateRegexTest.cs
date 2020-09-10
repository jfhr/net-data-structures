using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Automata;

namespace NetDataStructures.UnitTests.Regex
{
    [TestClass]
    public class DateRegexTest : RegexAutomatonTestBase
    {
        protected override IEnumerable<(string Input, bool Expected)> TestData => new[]
        {
            ("01.01", true),
            ("01.02", true),
            ("01.03", true),
            ("01.30", true),
            ("01.31", true),
            ("02.01", true),
            ("02.27", true),
            ("02.28", true),
            ("03.01", true),
            ("03.31", true),
            ("04.30", true),
            ("05.31", true),
            ("06.30", true),
            ("07.31", true),
            ("08.31", true),
            ("09.30", true),
            ("10.31", true),
            ("11.30", true),
            ("12.31", true),
            

            ("", false),
            ("1.01", false),
            ("01.1", false),
            ("01.00", false),
            ("00.01", false),
            ("00.00", false),
            ("01.32", false),
            ("01.33", false),
            ("01.40", false),
            ("01.50", false),
            ("02.29", false),
            ("02.30", false),
            ("02.31", false),
            ("02.32", false),
            ("03.32", false),
            ("04.31", false),
            ("05.32", false),
            ("06.31", false),
            ("07.32", false),
            ("08.32", false),
            ("09.31", false),
            ("10.32", false),
            ("11.31", false),
            ("12.32", false),
            ("13.00", false),
            ("13.01", false),
            ("13.02", false),
            ("13.30", false),
        };

        protected override RegularExpression Target
        {
            get
            {
                const string _1to9 = "(1∪2∪3∪4∪5∪6∪7∪8∪9)";
                const string _0to8 = "(0∪1∪2∪3∪4∪5∪6∪7∪8)";
                const string _0to9 = "(0∪1∪2∪3∪4∪5∪6∪7∪8∪9)";

                string feb = $"02.(0{_1to9}∪1{_0to9}∪2{_0to8})";
                string m30 = $"(04∪06∪09∪11).(0{_1to9}∪1{_0to9}∪2{_0to9}∪30)";
                string m31 = $"(01∪03∪05∪07∪08∪10∪12).(0{_1to9}∪1{_0to9}∪2{_0to9}∪30∪31)";

                return new RegularExpression(
                    new [] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'}, 
                    $"{feb}∪{m30}∪{m31}"
                );
            }
        }
    }
}