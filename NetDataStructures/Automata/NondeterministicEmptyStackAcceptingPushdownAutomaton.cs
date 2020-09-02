using System.Collections.Generic;
using NetDataStructures.Automata.Internal;

namespace NetDataStructures.Automata
{
    /// <summary>
    /// A pushdown automaton that accepts an input iif it is in an accepting state and the complete input has been
    /// consumed. 
    /// </summary>
    public class NondeterministicEmptyStackAcceptingPushdownAutomaton : NondeterministicPushdownAutomatonBase
    {
        public NondeterministicEmptyStackAcceptingPushdownAutomaton(
            IEnumerable<char> alphabet,
            IEnumerable<char> stackAlphabet,
            char initialStackItem,
            IEnumerable<string> states,
            string startState,
            IDictionary<(string State, char? Symbol, char Pop), IEnumerable<(string State, string Push)>> delta
        ) : base(alphabet, stackAlphabet, initialStackItem, states, startState, delta)
        {
        }

        /// <summary>
        /// We accept iif we are in an accepting state and have consumed the entire input. 
        /// </summary>
        protected override bool IsAcceptingConfiguration(List<char> stack, string state, string input, int index)
        {
            return stack.Count == 0 && index == input.Length;
        }
    }
}