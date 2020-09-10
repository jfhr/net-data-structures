using System.Collections.Generic;

namespace NetDataStructures.Automata
{
    public interface IAutomaton
    {
        IEnumerable<char> Alphabet { get; }
        bool Run(string input);
    }
}