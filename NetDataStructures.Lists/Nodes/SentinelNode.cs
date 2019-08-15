using System;

namespace NetDataStructures.Lists.Nodes
{
    internal class SentinelNode<T> : INode<T>
    {
        public T Value { get => throw new IndexOutOfRangeException(); set => throw new IndexOutOfRangeException(); }
        
        public int Count(int previousCount)
        {
            return previousCount;
        }

        public T GetElementAt(int index)
        {
            throw new IndexOutOfRangeException($"No element exists at the specified index.");
        }

        public void SetElementAt(int index, T value)
        {
            throw new IndexOutOfRangeException($"No element exists at the specified index.");
        }

        public INode<T> Add(InnerNode<T> innerNode)
        {
            innerNode.Next = this;
            return innerNode;
        }

        public bool Contains(T item)
        {
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            return;
        }

        public void Insert(int index, InnerNode<T> innerNode)
        {
            throw new IndexOutOfRangeException($"Can't insert at the specified index.");
        }

        public int IndexOf(T item, int thisIndex)
        {
            return -1;
        }

        public INode<T> Remove(T item)
        {
            return this;
        }

        public INode<T> RemoveAt(int index)
        {
            throw new IndexOutOfRangeException($"Can't remove element at the specified index.");
        }
    }
}
