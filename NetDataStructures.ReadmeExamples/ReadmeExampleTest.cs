using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using NetDataStructures.Lists;
using NetDataStructures.Matrices;

namespace NetDataStructures.ReadmeExamples
{
    [TestClass]
    public class ReadmeExampleTest
    {
        [TestMethod]
        public void LinkedListExample()
        {
            IList<string> list = new Lists.LinkedList<string>
            {
                "foo"
            };
            list.Add("bar");
            Assert.AreEqual(0, list.IndexOf("foo"));
        }

        [TestMethod]
        public void ArrayListExample()
        {
            IList<string> list = new ArrayList<string>(4)
            {
                "foo"
            };
            list.Add("bar");
            Assert.AreEqual(2, list.Count);
        }


        [TestMethod]
        public void MatrixExample()
        {
            Matrix even = new Matrix(new int[,] {
                { 2, 2 },
                { 4, 4 },
            });

            Matrix odd = new Matrix(new int[,] {
                { 1, 1 },
                { 3, 3 },
            });

            Matrix product = even * odd;

            Assert.AreEqual(16, product[1, 1]);
        }
    }
}
