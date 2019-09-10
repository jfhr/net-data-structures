using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NetDataStructures.Lists;

namespace NetDataStructures.UnitTests.Lists
{
    // Performance tests don't make sense in Debug mode since it lacks compiler optimizations
#if !DEBUG  
    [TestClass]
#endif
    public class ListPerformanceTest
    {
        private const int Iterations = 10000;

        private static IEnumerable<object[]> TypesUnderTest()
        {
            yield return new object[] { new List<object>() };
            yield return new object[] { new ArrayList<object>() };
            yield return new object[] { new RecursiveSinglyLinkedList<object>() };
        }

        private readonly Stopwatch stopwatch = new Stopwatch();

        [TestInitialize]
        public void Initialize()
        {
            stopwatch.Reset();
        }

        [TestMethod, DynamicData("TypesUnderTest", DynamicDataSourceType.Method)]
        public void PerformanceTest_Add(IList<object> target)
        {
            // Arrange

            // Act
            for (int i = 0; i < Iterations; i++)
            {
                object o = new object();
                stopwatch.Start();
                target.Add(o);
                stopwatch.Stop();
            }

            // Assert
            Console.WriteLine($"Add {Iterations} times, elapsed ms: {stopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond}");
        }

        [TestMethod, DynamicData("TypesUnderTest", DynamicDataSourceType.Method)]
        public void PerformanceTest_Fuzzy(IList<object> target)
        {
            // Arrange
            object referenceObject = "reference";
            var actions = ListFuzzyUnitTest.FuzzActions(referenceObject);
            var random = new Random();

            // Act
            for (int i = 0; i < Iterations; i++)
            {
                var action = actions[random.Next(0, actions.Length)];
                stopwatch.Start();
                action(target);
                stopwatch.Stop();
            }

            // Assert
            Console.WriteLine($"{Iterations} random actions, elapsed ms: {stopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond}");
        }


        [TestMethod, DynamicData("TypesUnderTest", DynamicDataSourceType.Method)]
        public void PerformanceTest_RandomAccess(IList<object> target)
        {
            // Arrange
            object currentObject;
            var random = new Random();

            for (int i = 0; i < Iterations; i++)
            {
                target.Add(new object());
            }

            // Act
            for (int i = 0; i < Iterations; i++)
            {
                int index = random.Next(target.Count);
                stopwatch.Start();
                currentObject = target[index];
                stopwatch.Stop();
            }

            // Assert
            Console.WriteLine($"{Iterations} random access operations, elapsed ms: {stopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond}");
        }
    }
}
