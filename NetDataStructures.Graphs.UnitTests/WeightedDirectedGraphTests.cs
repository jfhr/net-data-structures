using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetDataStructures.Graph;

namespace NetDataStructures.Graphs.UnitTests
{
    [TestClass]
    public class WeightedDirectedGraphTests
    {
        private IWeightedDirectedGraph<string> target;

        [TestInitialize]
        public void Initialize()
        {
            target = new WeightedDirectedGraph<string>();
        }

        [TestMethod]
        public void Add_Expect_AddElement()
        {
            // Act
            target.Add("foo");

            // Assert
            Assert.IsTrue(target.Contains("foo"));
            Assert.IsFalse(target.Contains("bar"));
        }

        [TestMethod]
        public void Remove_Expect_RemoveElement()
        {
            // Arrange
            target.Add("foo");
            target.Add("bar");

            // Act
            target.Remove("bar");

            // Assert
            Assert.IsTrue(target.Contains("foo"));
            Assert.IsFalse(target.Contains("bar"));
        }

        [TestMethod]
        public void Clear_Expect_RemoveAllElements()
        {
            // Arrange
            target.Add("foo");
            target.Add("bar");

            // Act
            target.Clear();

            // Assert
            Assert.IsFalse(target.Contains("foo"));
            Assert.IsFalse(target.Contains("bar"));
            Assert.AreEqual(0, target.Count);
        }

        [TestMethod]
        public void Clear_Expect_RemoveAllVertices()
        {
            // Arrange
            target.Add("foo");
            target.Add("bar");
            target.AddVertice(from: "foo", to: "bar", 1);

            // Act
            target.Clear();
            target.Add("foo");
            target.Add("bar");
            int weight = target.GetVerticeWeight(from: "foo", to: "bar");

            // Assert
            Assert.AreEqual(0, weight);
        }

        [TestMethod]
        public void CopyTo_Expect_CopyItemsToArray()
        {
            // Arrange
            target.Add("foo");
            target.Add("bar");
            target.Add("biz");
            string[] actual = new string[3];
            string[] expected = new string[] { "foo", "bar", "biz" };

            // Act
            target.CopyTo(actual, 0);

            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void AddVertice_Expect_Ok()
        {
            // Arrange
            target.Add("foo");
            target.Add("bar");

            // Act
            target.AddVertice(from: "foo", to: "bar", 1);
        }
               
        [TestMethod]
        public void GetVerticeWeight_Expect_ReturnVerticeWeight()
        {
            // Arrange
            const int weight = 42;
            target.Add("foo");
            target.Add("bar");
            target.AddVertice(from: "foo", to: "bar", weight);

            // Act
            int actual = target.GetVerticeWeight(from: "foo", to: "bar");

            // Assert
            Assert.AreEqual(weight, actual);
        }
    }
}
