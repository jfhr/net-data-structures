using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NetDataStructures.Automata.Internal;

namespace NetDataStructures.Automata
{
    /// <summary>
    /// A deterministic finite state automaton that accepts or rejects a given string of symbols.
    /// </summary>
    public class DeterministicFiniteStateAutomaton : IAutomaton
    {
        /// <summary>
        /// Returns the underlying alphabet.
        /// </summary>
        public IEnumerable<char> Alphabet => _alphabet;

        /// <summary>
        /// Returns the names of all states of the automaton.
        /// </summary>
        public IEnumerable<string> States => _states;

        /// <summary>
        /// Returns the name of the initial state.
        /// </summary>
        public string StartState => _startState;

        /// <summary>
        /// Returns the transition table as a read-only version.
        /// </summary>
        public IReadOnlyDictionary<(string State, char Symbol), string> Delta => _delta;

        /// <summary>
        /// Returns the name of all accepting states.
        /// </summary>
        public IEnumerable<string> AcceptStates => _acceptStates;

        private ImmutableHashSet<char> _alphabet;
        private HashSet<string> _states;
        private string _startState;
        private Dictionary<(string State, char Symbol), string> _delta;
        private HashSet<string> _acceptStates;
        
        /// <summary>
        /// Creates an empty automaton. Call Initialize() with the necessary arguments to make it usable.
        /// </summary>
        private DeterministicFiniteStateAutomaton() {}

        /// <summary>
        /// Creates a new deterministic finite state automaton.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet contains all possible input symbols.
        /// </param>
        /// <param name="states">
        /// The names of all states.
        /// </param>
        /// <param name="startState">
        /// The initial state, must be included in <paramref name="states"/>.
        /// </param>
        /// <param name="delta">
        /// The transition table. Must contain the name of the following state for each pair of states and symbols.
        /// </param>
        /// <param name="acceptStates">
        /// The accepting states, all must be included in <paramref name="states"/>.
        /// </param>
        public DeterministicFiniteStateAutomaton(IEnumerable<char> alphabet, IEnumerable<string> states,
            string startState, IDictionary<(string State, char Symbol), string> delta,
            IEnumerable<string> acceptStates)
        {
            Initialize(
                alphabet.ToImmutableHashSet(),
                states.ToHashSet(),
                startState,
                delta.ToDictionary(x => x.Key, x => x.Value),
                acceptStates.ToHashSet()
            );
        }

        private void Initialize(ImmutableHashSet<char> alphabet, HashSet<string> states, string startState,
            Dictionary<(string State, char Symbol), string> delta, HashSet<string> acceptStates)
        {
            ValidateArguments(alphabet, states, startState, delta, acceptStates);
            _alphabet = alphabet;
            _states = states;
            _startState = startState;
            _delta = delta;
            _acceptStates = acceptStates;
        }

        private static void ValidateArguments(ImmutableHashSet<char> alphabet, HashSet<string> states, string startState,
            Dictionary<(string State, char Symbol), string> delta, HashSet<string> acceptStates)
        {
            if (alphabet == null) throw new ArgumentNullException(nameof(alphabet));
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

            // Every combination of state and symbol must be included in transition table
            foreach (var state in states)
            {
                foreach (var symbol in alphabet)
                {
                    if (!delta.ContainsKey((state, symbol)))
                    {
                        throw new ArgumentException(
                            $"Transition table does not contain an entry for state {state} and symbol {symbol}. " +
                            "Either add an entry for that combination or remove the state or symbol.",
                            nameof(delta));
                    }
                }
            }
        }

        /// <summary>
        /// Returns a minimized version of this automaton.
        /// </summary>
        public DeterministicFiniteStateAutomaton Minimize()
        {
            var copy = new DeterministicFiniteStateAutomaton(_alphabet, _states, _startState, _delta, _acceptStates);
            copy.MinimizeInPlace();
            return copy;
        }

        /// <summary>
        /// Minimizes this automaton in-place.
        /// </summary>
        public void MinimizeInPlace()
        {
            _states = GetReachableStates();
            var eqGroups = ComputeNerodeEquivalencyGroups();
            foreach (var eg in eqGroups)
            {
                MergeStates(eg);
            }
            _acceptStates.IntersectWith(_states);
            IntersectDeltaWithStates();
        }

        /// <summary>
        /// Removes all relations from delta that contain states that no longer exist.
        /// </summary>
        private void IntersectDeltaWithStates()
        {
            foreach (var (state, symbol) in _delta.Keys)
            {
                if (!_states.Contains(state))
                {
                    _delta.Remove((state, symbol));
                }
            }
        }

        /// <summary>
        /// Merges multiple states into one. This will not change the DFA behavior if the states are in the same
        /// Nerode equivalence class.
        /// </summary>
        private void MergeStates(ISet<string> mergeStates)
        {
            if (mergeStates.Count == 0)
            {
                return;
            }
            
            // If the startState is in the set, keep it, otherwise keep an arbitrary one of the states
            string keepState = mergeStates.Contains(_startState)
                ? _startState
                : mergeStates.First();

            mergeStates.Remove(keepState);

            // Merge all incoming edges
            foreach (string state in _states)
            {
                foreach (char symbol in _alphabet)
                {
                    if (mergeStates.Contains(_delta[(state, symbol)]))
                    {
                        _delta[(state, symbol)] = keepState;
                    }
                }
            }
            
            // Remove the extra states
            _states.ExceptWith(mergeStates);
        }

        /// <summary>
        /// Return an enumerable of sets of states. Each set contains all states belonging to the same Nerode
        /// equivalence class.
        /// </summary>
        IEnumerable<ISet<string>> ComputeNerodeEquivalencyGroups()
        {
            // HashSets aren't guaranteed to keep the same order on every iteration,
            // we rely on the order of the alphabet here, so we copy it into an array
            var sortedAlphabet = _alphabet.ToImmutableArray();
            Dictionary<string, int> oldClasses, newClasses;

            // Run iterations until there are no new equivalence classes
            newClasses = NerodeEquivalenceFirstIteration(sortedAlphabet);
            do
            {
                oldClasses = newClasses;
                newClasses = NerodeEquivalenceIteration(sortedAlphabet, oldClasses);
            } while (newClasses.Values.Distinct().Count() != oldClasses.Values.Distinct().Count());
            
            // Now, if there are multiple states with the same class, merge them into one
            return from cls in newClasses
                group cls by cls.Value
                into gr
                select gr.Select(x => x.Key).ToHashSet();
        }

        /// <summary>
        /// Does the first iteration where states are grouped by whether or not they are an accepting state.
        /// </summary>
        /// <param name="sortedAlphabet">
        /// The alphabet in order. This can be any order, but it has to be the same throughout the minimizing operation.
        /// </param>
        Dictionary<string, int> NerodeEquivalenceFirstIteration(ImmutableArray<char> sortedAlphabet)
        {
            var classes = new Dictionary<string, int>();
            
            foreach (string state in _states)
            {
                classes[state] = _acceptStates.Contains(state) ? 1 : 0;
            }

            return classes;
        }

        /// <summary>
        /// Does one iteration where states are grouped by their own class and the classes of their direct successors.
        /// </summary>
        /// <param name="sortedAlphabet">
        /// The alphabet in order. This can be any order, but it has to be the same throughout the minimizing operation.
        /// </param>
        /// <param name="classes">
        /// The classes returned from the last iteration.
        /// </param>
        Dictionary<string, int> NerodeEquivalenceIteration(ImmutableArray<char> sortedAlphabet,
            Dictionary<string, int> classes)
        {
            var patterns = new List<EquatableArray<int>>(_states.Count);
            int patternIndex = 0;
            var newClasses = new Dictionary<string, int>();
            
            foreach (string state in _states)
            {
                // Create the pattern for this state
                var pattern = new EquatableArray<int>(_alphabet.Count + 1);
                
                // It starts with the class of this state itself...
                pattern[0] = classes[state];
                
                // ...and continues with the classes of all its direct successors
                for (int i = 0; i < sortedAlphabet.Length; i++)
                {
                    char symbol = sortedAlphabet[i];
                    pattern[i + 1] = classes[_delta[(state, symbol)]];
                }
                
                // We add this pattern to the list of patterns
                // If it is a new pattern, we increment the patternIndex
                // The new class of this state is the patternIndex
                if (patterns.Contains(pattern))
                {
                    newClasses[state] = patterns.IndexOf(pattern);
                }
                else
                {
                    newClasses[state] = patternIndex++;
                    patterns.Add(pattern);
                }
            }

            return newClasses;
        }
            

        private List<string>[] GetEquivalencyGroups(ImmutableArray<string> states, IReadOnlyList<int> classes)
        {
            var groups = new List<string>[states.Length];

            for (int i = 0; i < states.Length; i++)
            {
                var state = states[i];
                var index = classes[i];
                groups[index] ??= new List<string>();
                groups[index].Add(state);
            }

            return groups;
        }

        /// <summary>
        /// Gets all states that can be reached from the <see cref="_startState"/>.
        /// </summary>
        private HashSet<string> GetReachableStates()
        {
            var result = new HashSet<string>();
            GetStatesReachableFrom(_startState, result);
            return result;
        }

        /// <summary>
        /// Recursively determines all states that can be reached from the <paramref cref="fromState"/>
        /// and adds them to the <paramref name="destination"/>.
        /// </summary>
        private void GetStatesReachableFrom(string fromState, ISet<string> destination)
        {
            destination.Add(fromState);
            foreach (var symbol in _alphabet)
            {
                var reachableState = _delta[(fromState, symbol)];
                if (!destination.Contains(reachableState))
                {
                    GetStatesReachableFrom(reachableState, destination);
                }
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
            var currentState = _startState;
            foreach (var c in input)
            {
                Utils.CheckSymbolInAlphabet(_alphabet, c);
                currentState = _delta[(currentState, c)];
            }

            return _acceptStates.Contains(currentState);
        }
    }
}