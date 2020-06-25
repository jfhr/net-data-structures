using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NetDataStructures.Lists;

namespace NetDataStructures.UnitTests.Lists
{
    [TestClass]
    public class ListUnitTest
    {
        private static IEnumerable<object[]> TypesUnderTest()
        {
            // We run all tests on the built-in List<T> type
            // if a test fails here, it means the test itself is erroneous
            yield return new object[] { new List<object>() };
            yield return new object[] { new ArrayList<object>() };
            yield return new object[] { new RecursiveSinglyLinkedList<object>() };
        }

        /// <summary>
        /// Lists are not read-only by default.
        /// </summary>
        [TestMethod, DynamicData(nameof(TypesUnderTest), DynamicDataSourceType.Method)]
        public void IsReadOnly_Expect_False(IList<object> target)
        {
            // Assert
            Assert.IsFalse(target.IsReadOnly);
        }

        /// <summary>
        /// There are no elements in a newly created list.
        /// </summary>
        [TestMethod, DynamicData(nameof(TypesUnderTest), DynamicDataSourceType.Method)]
        public void InitialCount(IList<object> target)
        {
            // Assert
            Assert.AreEqual(0, target.Count);
        }

        /// <summary>
        /// The Add() method adds an item to the list.
        /// </summary>
        [TestMethod, DynamicData(nameof(TypesUnderTest), DynamicDataSourceType.Method)]
        public void Add(IList<object> target)
        {
            // Arrange
            object element = "element";

            // Act
            target.Add(element);

            // Assert
            Assert.IsTrue(target.Contains(element));
            Assert.AreEqual(1, target.Count);
        }

        /// <summary>
        /// The Insert() method inserts an item into the list at the correct index.
        /// </summary>
        [TestMethod, DynamicData(nameof(TypesUnderTest), DynamicDataSourceType.Method)]
        public void Insert(IList<object> target)
        {
            // Arrange
            object elementBefore = "before";
            object elementInsert = "insert";
            object elementAfter = "after";
            target.Add(elementBefore);
            target.Add(elementAfter);

            // Act
            target.Insert(1, elementInsert);

            // Assert
            Assert.IsTrue(target.Contains(elementInsert));
            Assert.AreEqual(3, target.Count);
        }

        /// <summary>
        /// The Remove() method removes an item from the list.
        /// </summary>
        [TestMethod, DynamicData(nameof(TypesUnderTest), DynamicDataSourceType.Method)]
        public void Remove(IList<object> target)
        {
            // Arrange
            object element = "element";
            target.Add(element);

            // Act
            target.Remove(element);

            // Assert
            Assert.IsFalse(target.Contains(element));
            Assert.AreEqual(0, target.Count);
        }

        /// <summary>
        /// The RemoveAt() method removes the item with a given index from the list.
        /// </summary>
        [TestMethod, DynamicData(nameof(TypesUnderTest), DynamicDataSourceType.Method)]
        public void RemoveAt(IList<object> target)
        {
            // Arrange
            object element = "element";
            target.Add(element);

            // Act
            target.RemoveAt(0);

            // Assert
            Assert.IsFalse(target.Contains(element));
            Assert.AreEqual(0, target.Count);
        }

        /// <summary>
        /// The IndexOf() method correctly returns the index of an item in the list.
        /// </summary>
        [TestMethod, DynamicData(nameof(TypesUnderTest), DynamicDataSourceType.Method)]
        public void IndexOf(IList<object> target)
        {
            // Arrange
            object element = "element";
            target.Add(element);

            // Act
            int output = target.IndexOf(element);

            // Assert
            Assert.AreEqual(0, output);
        }

        /// <summary>
        /// I can clear a list.
        /// </summary>
        [TestMethod, DynamicData(nameof(TypesUnderTest), DynamicDataSourceType.Method)]
        public void Clear(IList<object> target)
        {
            // Arrange
            target.Add("elem1");
            target.Add("elem2");
            target.Add("elem3");

            // Act
            target.Clear();

            // Assert
            Assert.AreEqual(0, target.Count);
            Assert.IsFalse(target.Contains("elem1"));
            Assert.IsFalse(target.Contains("elem2"));
            Assert.IsFalse(target.Contains("elem3"));
        }

        /// <summary>
        /// I can copy a list into an array.
        /// </summary>
        [TestMethod, DynamicData(nameof(TypesUnderTest), DynamicDataSourceType.Method)]
        public void CopyTo(IList<object> target)
        {
            // Arrange
            var destination = new object[target.Count];

            // Act
            target.CopyTo(destination, 0);

            // Assert
            for (int i = 0; i < destination.Count(); i++)
            {
                Assert.AreEqual(target[i], destination[i]);
            }
        }

        /// <summary>
        /// I can copy a list into an array, starting at an offset.
        /// </summary>
        [TestMethod, DynamicData(nameof(TypesUnderTest), DynamicDataSourceType.Method)]
        public void CopyTo_WithOffset(IList<object> target)
        {
            // Arrange
            const int offset = 3;
            var destination = new object[target.Count + offset];

            // Act
            target.CopyTo(destination, offset);

            // Assert
            for (int i = offset; i < destination.Count(); i++)
            {
                Assert.AreEqual(target[i - offset], destination[i]);
            }
        }

        /// <summary>
        /// I can get an <see cref="IEnumerator{T}"/> from a list that contains the correct elements.
        /// </summary>
        [TestMethod, DynamicData(nameof(TypesUnderTest), DynamicDataSourceType.Method)]
        public void GetEnumerator(IList<object> target)
        {
            // Arrange

            // Act
            var enumerator = target.GetEnumerator();

            // Assert
            int i = 0;
            while (enumerator.MoveNext())
            {
                Assert.AreEqual(target[i], enumerator.Current);
            }
            Assert.AreEqual(target.Count, i);
        }
    }
}