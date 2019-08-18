using System.Collections.Generic;

namespace NetDataStructures.Graph
{
    public interface IWeightedDirectedGraph<T> : ICollection<T>
    {
        int GetVerticeWeight(T from, T to);
        void AddVertice(T from, T to, int weight);
        void RemoveVertice(T from, T to);
    }
}
