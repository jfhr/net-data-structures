using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetDataStructures.Lists.UnitTests
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
            yield return new object[] { new LinkedList<object>() };
        }

        /// <summary>
        /// I can get an <see cref="IEnumerator{T}"/> from a list.
        /// </summary>
        [TestMethod, DynamicData(nameof(TypesUnderTest), DynamicDataSourceType.Method)]
        public void GetEnumerator_Expect_Ok(IList<object> target)
        {
            // Act
            var enumerator = target.GetEnumerator();
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
        public void Count_Initial_Expect_0(IList<object> target)
        {
            // Assert
            Assert.AreEqual(0, target.Count);
        }

        /// <summary>
        /// The Add() method adds an item to the list.
        /// </summary>
        [TestMethod, DynamicData(nameof(TypesUnderTest), DynamicDataSourceType.Method)]
        public void Add_Expect_AddElement(IList<object> target)
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
        public void Insert_Expect_InsertElementAtCorrectIndex(IList<object> target)
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
        public void Remove_Expect_RemoveElement(IList<object> target)
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
        public void RemoveAt_Expect_RemoveElement(IList<object> target)
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
        public void IndexOf_Expect_ReturnIndexOfElement(IList<object> target)
        {
            // Arrange
            object element = "element";
            target.Add(element);

            // Act
            int output = target.IndexOf(element);

            // Assert
            Assert.AreEqual(0, output);
        }
    }
}