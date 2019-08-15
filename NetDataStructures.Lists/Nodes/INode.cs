namespace NetDataStructures.Lists.Nodes
{
    internal interface INode<T>
    {
        T Value { get; set; }

        int Count(int previousCount);
        T GetElementAt(int index);
        void SetElementAt(int index, T value);
        INode<T> Add(InnerNode<T> innerNode);
        bool Contains(T item);
        void CopyTo(T[] array, int arrayIndex);
        void Insert(int index, InnerNode<T> innerNode);
        int IndexOf(T item, int thisIndex);
        INode<T> Remove(T item);
        INode<T> RemoveAt(int index);
    }
}
