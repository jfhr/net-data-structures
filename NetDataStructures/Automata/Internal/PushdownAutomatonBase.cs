using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace NetDataStructures.Automata.Internal
{
    public abstract class PushdownAutomatonBase
    {

        /// <summary>
        /// Returns the underlying alphabet.
        /// </summary>
        public IEnumerable<char> Alphabet => _alphabet;

        /// <summary>
        /// Returns the alphabet of stack items.
        /// </summary>
        public IEnumerable<char> StackAlphabet => _stackAlphabet;

        /// <summary>
        /// Returns the initial stack item.
        /// </summary>
        public char InitialStackItem => _initialStackItem;

        /// <summary>
        /// Returns all states of the automaton.
        /// </summary>
        public IEnumerable<string> States => _states;

        /// <summary>
        /// Returns the initial state.
        /// </summary>
        public string StartState => _startState;

        /// <summary>
        /// Returns the transition table as a read-only version.
        /// </summary>
        public IReadOnlyDictionary<(string State, char? Symbol, char Pop), HashSet<(string State, string Push)>>
            Delta => _delta;
        
        private ImmutableHashSet<char> _alphabet;
        private ImmutableHashSet<char> _stackAlphabet;
        private char _initialStackItem;
        private HashSet<string> _states;
        private string _startState;
        private Dictionary<(string State, char? Symbol, char Pop), HashSet<(string State, string Push)>> _delta;

        protected PushdownAutomatonBase(
            IEnumerable<char> alphabet,
            IEnumerable<char> stackAlphabet,
            char initialStackItem,
            IEnumerable<string> states,
            string startState,
            IDictionary<(string State, char? Symbol, char Pop), IEnumerable<(string State, string Push)>> delta
        )
        {
            Initialize(
                alphabet.ToImmutableHashSet(),
                stackAlphabet.ToImmutableHashSet(),
                initialStackItem,
                states.ToHashSet(),
                startState,
                delta.ToDictionary(x => x.Key, x => x.Value.ToHashSet())
            );
        }

        private void Initialize(ImmutableHashSet<char> alphabet, ImmutableHashSet<char> stackAlphabet,
            char initialStackItem, HashSet<string> states, string startState,
            Dictionary<(string State, char? Symbol, char Pop), HashSet<(string State, string Push)>> delta)
        {
            ValidateArguments(alphabet, stackAlphabet, initialStackItem, states, startState, delta);
            _alphabet = alphabet;
            _stackAlphabet = stackAlphabet;
            _initialStackItem = initialStackItem;
            _states = states;
            _startState = startState;
            _delta = delta;
        }

        private void ValidateArguments(ImmutableHashSet<char> alphabet, ImmutableHashSet<char> stackAlphabet,
            char initialStackItem, HashSet<string> states, string startState,
            Dictionary<(string State, char? Symbol, char Pop), HashSet<(string State, string Push)>> delta)
        {
            if (alphabet == null) throw new ArgumentNullException(nameof(alphabet));
            if (stackAlphabet == null) throw new ArgumentNullException(nameof(stackAlphabet));
            if (states == null) throw new ArgumentNullException(nameof(states));
            if (startState == null) throw new ArgumentNullException(nameof(startState));
            if (delta == null) throw new ArgumentNullException(nameof(delta));

            // Start state must be included in states
            if (!states.Contains(startState))
            {
                throw new ArgumentException(
                    $"Start state {startState} not contained in the array of states. " +
                    "Either add it to the states, or choose a different start state.",
                    nameof(startState));
            }

            // Initial stack item must be included in stack alphabet
            if (!stackAlphabet.Contains(initialStackItem))
            {
                throw new ArgumentException(
                    $"Initial stack item {initialStackItem} not contained in the stack alphabet." +
                    "Either add it to the stack alphabet, or choose a different initial stack item.",
                    nameof(initialStackItem));
            }
        }

        /// <summary>
        /// Run the automaton on the given input.
        /// </summary>
        /// <param name="input">
        /// A string of symbols from the alphabet.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the automaton accepts the input, <see langword="false"/> if it rejects the input.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// One of the input characters was not found in the alphabet.
        /// </exception>
        public bool Run(string input)
        {
            var stack = new List<char> {_initialStackItem};
            return RunInternal(stack, _startState, input, 0);
        }

        private bool RunInternal(List<char> stack, string state, string input, int index)
        {
            if (IsAcceptingConfiguration(stack, state, input, index))
            {
                return true;
            }

            // If the stack is empty, we can't go further
            if (stack.Count == 0)
            {
                return false;
            }

            // Try a consuming edge, if one exists and we haven't consumed the entire input yet
            if (index < input.Length)
            {
                var symbol = input[index];
                AutomatonHelper.CheckSymbolInAlphabet(_alphabet, symbol);

                if (_delta.TryGetValue((state, symbol, stack[^1]), out var edges))
                {
                    // Run recursively for every edge. Not the index is incremented bc we consumed a symbol
                    if (edges.Any(edge => RunInternal(stack, input, index + 1, edge)))
                    {
                        return true;
                    }
                }
            }

            // Try a non-consuming edge if one exists
            if (_delta.TryGetValue((state, null, stack[^1]), out var ncEdges))
            {
                // Run recursively for every edge
                if (ncEdges.Any(edge => RunInternal(stack, input, index, edge)))
                {
                    return true;
                }
            }

            return false;
        }

        protected abstract bool IsAcceptingConfiguration(List<char> stack, string state, string input, int index);

        private bool RunInternal(List<char> stack, string input, int index, (string State, string Push) result)
        {
            var newStack = new List<char>(stack.Take(stack.Count - 1).Concat(result.Push));
            return RunInternal(newStack, result.State, input, index);
        }
    }
}