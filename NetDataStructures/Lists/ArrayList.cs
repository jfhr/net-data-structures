using System;
using System.Collections;
using System.Collections.Generic;

namespace NetDataStructures.Lists
{
    public class ArrayList<T> : IList<T>
    {
        private const int expansionFactor = 4;

        private T[] InnerArray { get; set; }
        private bool IsArrayFull => InnerArray.Length == Count;

        public T this[int index]
        {
            get => InnerArray[index];
            set => InnerArray[index] = value;
        }

        public int Count { get; private set; }
        public bool IsReadOnly => false;

        public ArrayList(int capacity = 0)
        {
            InnerArray = new T[capacity];
        }

        public void Add(T item)
        {
            if (IsArrayFull)
            {
                T[] oldArray = InnerArray;
                int newArraySize = oldArray.Length == 0 ? expansionFactor : oldArray.Length * expansionFactor;
                InnerArray = new T[newArraySize];
                oldArray.CopyTo(InnerArray, 0);
            }
            InnerArray[Count] = item;
            Count++;
        }

        public void Clear()
        {
            Count = 0;
        }

        public bool Contains(T item)
        {
            int index = IndexOf(item);
            if (index == -1 || index >= Count)
            {
                return false;
            }

            return true;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (InnerArray == array)
            {
                return;
            }
            Array.ConstrainedCopy(InnerArray, 0, array, arrayIndex, Count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)InnerArray).GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf(InnerArray, item);
        }

        public void Insert(int index, T item)
        {
            if (index > Count)
            {
                throw new IndexOutOfRangeException($"Can't insert at index {index}");
            }

            if (index == Count)
            {
                Add(item);
            }
            else if (IsArrayFull)
            {
                ExpandAndInsertAtIndex(index, item);
                Count++;
            }
            else
            {
                InsertOnlyAtIndex(index, item);
                Count++;
            }
        }

        /// <summary>
        /// Insert w/o expanding the array
        /// </summary>
        private void InsertOnlyAtIndex(int index, T item)
        {
            Array.ConstrainedCopy(InnerArray, index, InnerArray, index + 1, InnerArray.Length - index - 1);
            InnerArray[index] = item;
        }

        /// <summary>
        /// Insert combined with expanding the array. More efficient than expanding first, then inserting.
        /// </summary>
        private void ExpandAndInsertAtIndex(int index, T item)
        {
            T[] oldArray = InnerArray;
            int newArraySize = oldArray.Length == 0 ? expansionFactor : oldArray.Length * expansionFactor;
            InnerArray = new T[newArraySize];

            Array.ConstrainedCopy(InnerArray, 0, InnerArray, 0, index);
            Array.ConstrainedCopy(InnerArray, index, InnerArray, index + 1, InnerArray.Length - index - 1);
            InnerArray[index] = item;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0 && index < Count)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException($"Can't remove at index {index}");
            }

            if (index == Count - 1)
            {
                Count--;
            }
            else
            {
                Array.ConstrainedCopy(InnerArray, index + 1, InnerArray, index, Count - index - 1);
                Count--;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InnerArray.GetEnumerator();
        }
    }
}
