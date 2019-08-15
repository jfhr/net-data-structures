using System.Collections.Generic;

namespace NetDataStructures.Graph
{
    public interface IWeightedDirectedGraph<T> : ICollection<T>
    {
        void GetVerticeWeight(int from, int to);
        void AddVertice(int from, int to, int weight);
        void RemoveVertice(int from, int to, int weight);
    }
}
