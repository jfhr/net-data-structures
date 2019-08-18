using System;
using System.Collections;
using System.Collections.Generic;
using NetDataStructures.Lists;
using NetDataStructures.Matrices;

namespace NetDataStructures.Graph
{
    public class WeightedDirectedGraph<T> : IWeightedDirectedGraph<T>
    {
        private readonly IList<T> _items;
        private Matrix _adjacency;

        public int Count => _items.Count;
        public bool IsReadOnly => false;

        /// <summary>
        /// Creates a new <see cref="WeightedDirectedGraph{T}"/> with an initial capacity.
        /// </summary>
        public WeightedDirectedGraph(int capacity = 0)
        {
            _items = new ArrayList<T>(capacity);
            _adjacency = new Matrix(new int[capacity, capacity]);
        }

        public void Add(T item) => _items.Add(item);
        public bool Contains(T item) => _items.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
        public bool Remove(T item) => _items.Remove(item);
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_items).GetEnumerator();

        public void Clear()
        {
            _items.Clear();
            _adjacency *= 0;
        }

        private int GetIndexOrThrow(T item, string paramName)
        {
            int index = _items.IndexOf(item);
            if (index == -1)
            {
                throw new ArgumentException($"Did not find item: <{item}>.", paramName);
            }
            return index;
        }

        public int GetVerticeWeight(T from, T to)
        {
            int fromIndex = GetIndexOrThrow(from, nameof(from));
            int toIndex = GetIndexOrThrow(to, nameof(to));
            if (fromIndex >= _adjacency.SizeX || toIndex >= _adjacency.SizeY)
            {
                // items are outside the bounds of the adj matrix.
                // the matrix will only be expanded once a vertice is actually added
                return 0;
            }
            return _adjacency[fromIndex, toIndex];
        }

        public void AddVertice(T from, T to, int weight)
        {
            if (weight == 0)
            {
                throw new ArgumentException("The vertice weight must be non-zero.", nameof(weight));
            }
            int fromIndex = GetIndexOrThrow(from, nameof(from));
            int toIndex = GetIndexOrThrow(to, nameof(to));
            if (fromIndex >= _adjacency.SizeX || toIndex >= _adjacency.SizeY)
            {
                // expand the adj matrix
                _adjacency = new Matrix(_adjacency, Count, Count);
            }
            _adjacency[fromIndex, toIndex] = weight;
        }

        public void RemoveVertice(T from, T to)
        {
            int fromIndex = GetIndexOrThrow(from, nameof(from));
            int toIndex = GetIndexOrThrow(to, nameof(to));
            if (fromIndex < _adjacency.SizeX && toIndex < _adjacency.SizeY)
            {
                _adjacency[fromIndex, toIndex] = 0;
            }
        }
    }
}
