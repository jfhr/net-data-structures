using System;
using System.Collections;
using System.Collections.Generic;

using NetDataStructures.Matrices;

namespace NetDataStructures.Graph
{
    public class WeightedDirectedGraph<T> : IWeightedDirectedGraph<T>
    {
        public int Count => throw new NotImplementedException();
        public bool IsReadOnly => false;

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void AddVertice(int from, int to, int weight)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void GetVerticeWeight(int from, int to)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveVertice(int from, int to, int weight)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
