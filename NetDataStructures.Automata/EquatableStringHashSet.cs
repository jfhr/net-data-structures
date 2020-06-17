using System;
using System.Collections.Generic;

namespace NetDataStructures.Automata
{
    /// <summary>
    /// Generic set that implements Equals() and GetHashCode() methods that compare contents.
    /// </summary>
    internal class EquatableHashSet<T> : HashSet<T>, IEquatable<HashSet<T>>
    {
        public EquatableHashSet()
        {
        }

        public EquatableHashSet(IEnumerable<T> items) : base(items)
        {
        }

        public bool Equals(HashSet<T> other)
        {
            return other != null && SetEquals(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return obj is EquatableHashSet<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var item in this)
            {
                hash = (hash << 1) & item.GetHashCode();
            }

            return hash;
        }
    }
}