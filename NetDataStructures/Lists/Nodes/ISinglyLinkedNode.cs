namespace NetDataStructures.Lists.Nodes
{
    internal interface ISinglyLinkedNode<T>
    {
        T Value { get; set; }

        int Count(int previousCount);
        T GetElementAt(int index);
        void SetElementAt(int index, T value);
        ISinglyLinkedNode<T> Add(InnerNode<T> innerNode);
        bool Contains(T item);
        void CopyTo(T[] array, int arrayIndex);
        void Insert(int index, InnerNode<T> innerNode);
        int IndexOf(T item, int thisIndex);
        ISinglyLinkedNode<T> Remove(T item);
        ISinglyLinkedNode<T> RemoveAt(int index);
    }
}
