using System;
using System.Linq;

namespace NetDataStructures.Automata
{
    /// <summary>
    /// Generic array that implements Equals() and GetHashCode() methods that compare contents.
    /// </summary>
    internal class EquatableArray<T> : IEquatable<EquatableArray<T>>
    {
        public int Length => _data.Length;

        public int this[int index]
        {
            get => _data[index];
            set => _data[index] = value;
        }

        private readonly int[] _data;

        public EquatableArray(int length)
        {
            _data = new int[length];
        }

        public bool Equals(EquatableArray<T> other)
        {
            return other != null && _data.SequenceEqual(other._data);
        }

        public override bool Equals(object obj)
        {
            if (obj is EquatableArray<T> other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _data.Aggregate(0, (current, i) => (current << 1) & i);
        }
    }
}