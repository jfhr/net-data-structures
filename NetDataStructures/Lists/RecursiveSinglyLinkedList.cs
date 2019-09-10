using System.Collections;
using System.Collections.Generic;

using NetDataStructures.Lists.Nodes;

namespace NetDataStructures.Lists
{
    public class RecursiveSinglyLinkedList<T> : IList<T>
    {
        internal ISinglyLinkedNode<T> Root { get; set; }

        public RecursiveSinglyLinkedList()
        {
            Root = new SentinelNode<T>();
        }

        public T this[int index] { get => Root.GetElementAt(index); set => Root.SetElementAt(index, value); }

        public int Count => Root.Count(0);

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            Root = Root.Add(new InnerNode<T>(item));
        }

        public void Clear()
        {
            Root = new SentinelNode<T>();
        }

        public bool Contains(T item)
        {
            return Root.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Root.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            ISinglyLinkedNode<T> currentNode = Root;
            InnerNode<T> currentInner;
            while (currentNode is InnerNode<T>)
            {
                currentInner = (InnerNode<T>) currentNode;
                yield return currentInner.Value;
                currentNode = currentInner.Next;
            }
        }

        public int IndexOf(T item)
        {
            return Root.IndexOf(item, 0);
        }

        public void Insert(int index, T item)
        {
            Root.Insert(index - 1, new InnerNode<T>(item));
        }

        public bool Remove(T item)
        {
            return HandleInternalChange(Root.Remove(item));
        }

        public void RemoveAt(int index)
        {
            HandleInternalChange(Root.RemoveAt(index));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Called after any operation on <see cref="Root"/> has finished that might change
        /// the contents of the list. Sets the Root to its new value.
        /// </summary>
        /// <param name="newRoot">
        /// Potential new root of the list.
        /// </param>
        /// <returns>
        /// A <see langword="bool"/> indicating if the operation was successful.
        /// </returns>
        private bool HandleInternalChange(ISinglyLinkedNode<T> newRoot)
        {
            if (newRoot == null)
            {
                return false;
            }
            else
            {
                Root = newRoot;
                return true;
            }
        }
    }
}
