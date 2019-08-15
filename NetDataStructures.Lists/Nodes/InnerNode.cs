namespace NetDataStructures.Lists.Nodes
{
    internal class InnerNode<T> : INode<T>
    {
        internal INode<T> Next { get; set; }

        public T Value { get; set; }

        public InnerNode(T value)
        {
            Next = null;
            Value = value;
        }
        
        public int Count(int previousCount)
        {
            return Next.Count(previousCount + 1);
        }

        public T GetElementAt(int index)
        {
            if (index == 0)
            {
                return Value;
            }
            else 
            {
                return Next.GetElementAt(index - 1);
            }
        }

        public void SetElementAt(int index, T value)
        {
            if (index == 0)
            {
                Value = value;
            }
            else
            {
                Next.SetElementAt(index - 1, value);
            }
        }

        public INode<T> Add(InnerNode<T> innerNode)
        {
            Next = Next.Add(innerNode);
            return this;
        }

        public bool Contains(T item)
        {
            if (Value.Equals(item))
            {
                return true;
            }
            else
            {
                return Next.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            array[arrayIndex] = Value;
            Next.CopyTo(array, arrayIndex + 1);
        }

        public void Insert(int index, InnerNode<T> innerNode)
        {
            if (index == 0)
            {
                // insert after me
                innerNode.Next = Next;
                Next = innerNode;
            }
            else
            {
                Next.Insert(index - 1, innerNode);
            }
        }

        public int IndexOf(T item, int thisIndex)
        {
            if (Value.Equals(item))
            {
                return thisIndex;
            }
            else
            {
                return Next.IndexOf(item, thisIndex + 1);
            }
        }

        public INode<T> Remove(T item)
        {
            if (Value.Equals(item))
            {
                // previous node will set its Next to my Next
                return Next;
            }
            else
            {
                Next = Next.Remove(item);
                return Next;
            }
        }

        public INode<T> RemoveAt(int index)
        {
            if (index == 0)
            {
                return Next;
            }
            else
            {
                Next = Next.RemoveAt(index - 1);
                return Next;
            }
        }
    }
}
