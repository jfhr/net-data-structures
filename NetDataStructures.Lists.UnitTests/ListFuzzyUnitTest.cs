using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetDataStructures.Lists.UnitTests
{
    /*
     * We use fuzz testing for all list types.
     * This will run random operations on the list before running an actual test, such as 
     * adding, searching and removing items. The list will be cleared before the actual 
     * unit test begins. In a faulty implementation, this might create an invalid internal 
     * state that would fail the unit test, which might not happen when running only the 
     * actual test.
     */
    [TestClass]
    public class ListFuzzyUnitTest : ListUnitTest
    {
        private static IEnumerable<object[]> TypesUnderTest()
        {
            IList<object> arrayList = new ArrayList<object>();
            Fuzz(arrayList, 25, 50);
            yield return new object[] { arrayList };

            IList<object> linkedList = new LinkedList<object>();
            Fuzz(linkedList, 25, 50);
            yield return new object[] { linkedList };
        }

        #region Fuzzy Testing

        private static readonly object referenceObject = new object();
        private static readonly Random random = new Random();

        public static Action<IList<object>>[] FuzzActions(object referenceObject)
        {
            return new Action<IList<object>>[]
            {
                list => list.Add(new object()),
                list => list.Add(referenceObject),
                list => list.Clear(),
                list => list.CopyTo(new object[list.Count], 0),
                list => list.Contains(new object()),
                list => list.Contains(referenceObject),
                list => list.GetEnumerator(),
                list => list.Equals(new object()),
                list => list.Equals(referenceObject),
                list => list.GetHashCode(),
                list => list.GetType(),
                list => list.ToString(),
                list => list.Remove(new object()),
                list => list.Remove(referenceObject),
                list =>
                {
                    if (list.Count > 0)
                    {
                        list.RemoveAt(random.Next(0, list.Count));
                    }
                },
            };
        }

        protected static void Fuzz(IList<object> target, int minIterations, int maxIterations)
        {
            var fuzzActions = FuzzActions(referenceObject);
            int iterations = random.Next(minIterations, maxIterations);

            for (int i = 0; i < iterations; i++)
            {
                var action = fuzzActions[random.Next(fuzzActions.Length)];
                action(target);
            }

            target.Clear();
        }

        #endregion
    }
}