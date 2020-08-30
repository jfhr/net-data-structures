using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NetDataStructures.LinearAlgebra;
using NetDataStructures.Lists;

namespace NetDataStructures.ReadmeExamples
{
    [TestClass]
    public class ReadmeExampleTest
    {
        [TestMethod]
        public void LinkedListExample()
        {
            IList<string> list = new RecursiveSinglyLinkedList<string>
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
            DoubleMatrix even = new DoubleMatrix(new double[,] {
                { 2, 2 },
                { 4, 4 },
            });

            DoubleMatrix odd = new DoubleMatrix(new double[,] {
                { 1, 1 },
                { 3, 3 },
            });

            DoubleMatrix product = even * odd;

            Assert.AreEqual(16, product[1, 1]);
        }
    }
}
