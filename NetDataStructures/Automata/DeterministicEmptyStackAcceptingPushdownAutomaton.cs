using System.Collections.Generic;
using NetDataStructures.Automata.Internal;

namespace NetDataStructures.Automata
{
    public class DeterministicEmptyStackAcceptingPushdownAutomaton : DeterministicPushdownAutomatonBase
    {
        public DeterministicEmptyStackAcceptingPushdownAutomaton(
            IEnumerable<char> alphabet,
            IEnumerable<char> stackAlphabet,
            char initialStackItem,
            IEnumerable<string> states,
            string startState,
            IDictionary<(string State, char? Symbol, char Pop), (string State, string Push)> delta
        ) : base(alphabet, stackAlphabet, initialStackItem, states, startState, delta)
        {
        }

        protected override bool IsAcceptingConfiguration(List<char> stack, string state, string input, int index)
        {
            return stack.Count == 0 && index == input.Length;
        }
    }
}