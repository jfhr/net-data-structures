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
    public class DeterministicFiniteStateAutomaton
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
            var copy = new DeterministicFiniteStateAutomaton(_alphabet, _states, _startState, _delta, _states);
            copy.MinimizeInPlace();
            return copy;
        }

        /// <summary>
        /// Minimizes this automaton in-place.
        /// </summary>
        public void MinimizeInPlace()
        {
            _states = GetReachableStates();
            var equivalencyGroups = GroupStatesByEquivalency();
            _states = EliminateStatesUsingEquivalencyGroups(equivalencyGroups);
            _acceptStates.IntersectWith(_states);
            _delta = _delta
                .Where(x => _states.Contains(x.Key.State))
                .ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Changes delta to ensure only one value per equivalency group is used.
        /// Also returns the list of states to keep.
        /// The start state is always kept.
        /// 
        /// WARNING: This manipulates <paramref name="equivalencyGroups"/>. 
        /// </summary>
        private HashSet<string> EliminateStatesUsingEquivalencyGroups(IEnumerable<List<string>> equivalencyGroups)
        {
            var keep = new HashSet<string>();
            bool hasEncounteredInitial = false;
            foreach (var equivalencyGroup in equivalencyGroups.Where(x => x != null))
            {
                string keepState;
                // Ensure we don't eliminate the start state
                if (!hasEncounteredInitial && equivalencyGroup.Contains(_startState))
                {
                    keepState = _startState;
                    hasEncounteredInitial = true;
                }
                // Otherwise, just use any state from the group
                else
                {
                    keepState = equivalencyGroup[0];
                    equivalencyGroup.Remove(keepState);
                }

                keep.Add(keepState);

                // We have to iterate over a copy of delta, otherwise we can't change the original
                // during the enumeration
                foreach (var (key, value) in _delta.ToImmutableDictionary())
                {
                    // Change relationships to eliminated states to instead point to the kept state 
                    if (equivalencyGroup.Contains(value))
                    {
                        _delta[key] = keepState;
                    }
                }
            }

            return keep;
        }

        /// <summary>
        /// Groups the existing states using the Nerode equivalency relation. 
        /// </summary>
        /// <returns>
        /// An array of lists. Each inner list contains the names of equivalent states.
        /// The automaton can be reduced by keeping only one state from each inner list.
        /// </returns>
        private List<string>[] GroupStatesByEquivalency()
        {
            // Copy alphabet and states to ensure the order doesn't change
            var alphabet = _alphabet.ToImmutableArray();
            var states = _states.ToImmutableArray();

            int numberOfDistinctClasses = 2;
            var classes = new int[states.Length];
            var patterns = Get2DEquatableArray(states.Length, alphabet.Length + 1);

            // Initial step: Two equivalency classes,
            // 1 => non-accepting states
            // 2 => accepting states
            for (var i = 0; i < states.Length; i++)
            {
                var state = states[i];
                patterns[i][0] = classes[i] = _acceptStates.Contains(state) ? 1 : 0;
            }

            // Iteration
            while (true)
            {
                // Fill in all patterns
                FillPatterns(states, patterns, classes, alphabet);

                // Now, give each distinct pattern a unique number 
                var distinctPatterns = patterns
                    .Distinct()
                    .Select((pattern, index) => (pattern, index))
                    .ToImmutableArray();

                if (distinctPatterns.Length == numberOfDistinctClasses)
                {
                    // No new distinct patterns found. Algorithm is complete
                    break;
                }

                numberOfDistinctClasses = distinctPatterns.Length;

                // Then fill classes with the corresponding number for each state
                CopyPatternsToClasses(states, patterns, distinctPatterns, classes);
            }

            return GetEquivalencyGroups(states, classes);
        }

        /// <summary>
        /// Returns an array of <paramref name="rowLength"/> <see cref="EquatableArray{int}"/> of length
        /// <paramref name="colLength"/>.
        /// </summary>
        private static EquatableArray<int>[] Get2DEquatableArray(int rowLength, int colLength)
        {
            var patterns = new EquatableArray<int>[rowLength];
            for (int i = 0; i < patterns.Length; i++)
            {
                patterns[i] = new EquatableArray<int>(colLength + 1);
            }

            return patterns;
        }

        private static void CopyPatternsToClasses(ImmutableArray<string> states,
            IReadOnlyList<EquatableArray<int>> patterns,
            ImmutableArray<(EquatableArray<int> pattern, int index)> distinctPatterns, IList<int> classes)
        {
            for (var i = 0; i < states.Length; i++)
            {
                var pattern = patterns[i];
                var patternNumber = distinctPatterns
                    .First(x => x.pattern.Equals(pattern))
                    .index;
                classes[i] = patternNumber;
            }
        }

        private void FillPatterns(ImmutableArray<string> states, IReadOnlyList<EquatableArray<int>> patterns,
            IReadOnlyList<int> classes, ImmutableArray<char> alphabet)
        {
            for (var i = 0; i < states.Length; i++)
            {
                // The first value is copied from classes
                var state = states[i];
                patterns[i][0] = classes[i];

                // The subsequent values are the corresponding values of _delta(state, symbol)
                // These can be copied from the corresponding position in classes
                for (var j = 0; j < alphabet.Length; j++)
                {
                    var symbol = alphabet[j];
                    var nextState = _delta[(state, symbol)];
                    var index = states.IndexOf(nextState);
                    patterns[i][j + 1] = classes[index];
                }
            }
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
        public bool Run(IEnumerable<char> input)
        {
            var currentState = _startState;
            foreach (var c in input)
            {
                AutomatonHelper.CheckSymbolInAlphabet(_alphabet, c);
                currentState = _delta[(currentState, c)];
            }

            return _acceptStates.Contains(currentState);
        }
    }
}