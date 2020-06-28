using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Schema;

namespace NetDataStructures.Automata
{
    /// <summary>
    /// A pushdown automaton that accepts an input iif it is in an accepting state and the complete input has been
    /// consumed. 
    /// </summary>
    public class NondeterministicFinalStatePushdownAutomaton
    {
        /// <summary>
        /// Returns the underlying alphabet.
        /// </summary>
        public IEnumerable<char> Alphabet => _alphabet;

        /// <summary>
        /// Returns the alphabet of stack items.
        /// </summary>
        public IEnumerable<string> StackAlphabet => _stackAlphabet;

        /// <summary>
        /// Returns the initial stack item.
        /// </summary>
        public string InitialStackItem => _initialStackItem;
        
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
        public IReadOnlyDictionary<(string State, char Symbol, string Pop), (string State, HashSet<string> Push)> Delta
            => _delta;

        /// <summary>
        /// Returns the accepting states.
        /// </summary>
        public IEnumerable<string> AcceptStates => _acceptStates;

        private ImmutableHashSet<char> _alphabet;
        private ImmutableHashSet<string> _stackAlphabet;
        private string _initialStackItem;
        private HashSet<string> _states;
        private string _startState;
        private Dictionary<(string State, char Symbol, string Pop), (string State, HashSet<string> Push)> _delta;
        private HashSet<string> _acceptStates;

        public NondeterministicFinalStatePushdownAutomaton(IEnumerable<char> alphabet,
            IEnumerable<string> stackAlphabet, string initialStackItem, IEnumerable<string> states, string startState,
            IDictionary<(string State, char Symbol, string Pop), (string State, IEnumerable<string> Push)> delta,
            IEnumerable<string> acceptStates)
        {
            Initialize(
                alphabet.ToImmutableHashSet(),
                stackAlphabet.ToImmutableHashSet(),
                initialStackItem,
                states.ToHashSet(),
                startState,
                delta.ToDictionary(x => x.Key, x => (x.Value.State, x.Value.Push.ToHashSet())),
                acceptStates.ToHashSet()
            );
        }

        private void Initialize(ImmutableHashSet<char> alphabet, ImmutableHashSet<string> stackAlphabet,
            string initialStackItem, HashSet<string> states, string startState,
            Dictionary<(string State, char Symbol, string Pop), (string State, HashSet<string> Push)> delta,
            HashSet<string> acceptStates)
        {
            ValidateArguments(alphabet, stackAlphabet, initialStackItem, states, startState, delta, acceptStates);
            _alphabet = alphabet;
            _stackAlphabet = stackAlphabet;
            _initialStackItem = initialStackItem;
            _states = states;
            _startState = startState;
            _delta = delta;
            _acceptStates = acceptStates;
        }

        private void ValidateArguments(ImmutableHashSet<char> alphabet, ImmutableHashSet<string> stackAlphabet,
            string initialStackItem, HashSet<string> states, string startState,
            Dictionary<(string State, char Symbol, string Pop), (string State, HashSet<string> Push)> delta,
            HashSet<string> acceptStates)
        {
            if (alphabet == null) throw new ArgumentNullException(nameof(alphabet));
            if (stackAlphabet == null) throw new ArgumentNullException(nameof(stackAlphabet));
            if (initialStackItem == null) throw new ArgumentNullException(nameof(initialStackItem));
            if (states == null) throw new ArgumentNullException(nameof(states));
            if (startState == null) throw new ArgumentNullException(nameof(startState));
            if (delta == null) throw new ArgumentNullException(nameof(delta));
            if (acceptStates == null) throw new ArgumentNullException(nameof(acceptStates));

            // Start state must be included in states
            if (!states.Contains(startState))
            {
                throw new ArgumentException(
                    $"Start state {startState} not contained in the array of states. " +
                    "Either add it to the states, or choose a different start state.",
                    nameof(startState));
            }

            // All accept states must be included in states
            foreach (var acceptState in acceptStates)
            {
                if (!states.Contains(acceptState))
                {
                    throw new ArgumentException(
                        $"Accept state {acceptState} not contained in the array of states. " +
                        "Either add it to the states, or choose different accept states.",
                        nameof(acceptStates));
                }
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
    }
}