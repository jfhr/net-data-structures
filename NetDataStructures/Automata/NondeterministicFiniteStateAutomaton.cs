using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NetDataStructures.Automata.Internal;

namespace NetDataStructures.Automata
{
    public class NondeterministicFiniteStateAutomaton
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
        public IEnumerable<string> StartStates => _startStates;

        /// <summary>
        /// Returns the transition table as a read-only version.
        /// </summary>
        public IReadOnlyDictionary<(string State, char Symbol), HashSet<string>> Delta => _delta;

        /// <summary>
        /// Returns the name of all accepting states.
        /// </summary>
        public IEnumerable<string> AcceptStates => _acceptStates;

        private ImmutableHashSet<char> _alphabet;
        private HashSet<string> _states;
        private HashSet<string> _startStates;
        private Dictionary<(string State, char Symbol), HashSet<string>> _delta;
        private HashSet<string> _acceptStates;

        /// <summary>
        /// Creates an empty automaton. Call Initialize() with the necessary arguments to make it usable.
        /// </summary>
        private NondeterministicFiniteStateAutomaton()
        {
        }

        /// <summary>
        /// Creates a new non-deterministic finite state automaton.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet contains all possible input symbols.
        /// </param>
        /// <param name="states">
        /// The names of all states.
        /// </param>
        /// <param name="startStates">
        /// The initial states, all must be included in <paramref name="states"/>.
        /// </param>
        /// <param name="delta">
        /// The transition table. Must contain the name of the following state for each pair of states and symbols.
        /// </param>
        /// <param name="acceptStates">
        /// The accepting states, all must be included in <paramref name="states"/>.
        /// </param>
        public NondeterministicFiniteStateAutomaton(IEnumerable<char> alphabet, IEnumerable<string> states,
            IEnumerable<string> startStates, IDictionary<(string State, char Symbol), IEnumerable<string>> delta,
            IEnumerable<string> acceptStates)
        {
            Initialize(
                alphabet.ToImmutableHashSet(),
                states.ToHashSet(),
                startStates.ToHashSet(),
                delta.ToDictionary(x => x.Key, x => x.Value.ToHashSet()),
                acceptStates.ToHashSet()
            );
        }

        private void Initialize(ImmutableHashSet<char> alphabet, HashSet<string> states,
            HashSet<string> startStates, Dictionary<(string State, char Symbol), HashSet<string>> delta,
            HashSet<string> acceptStates)
        {
            ValidateArguments(alphabet, states, startStates, delta, acceptStates);
            _alphabet = alphabet;
            _states = states;
            _startStates = startStates;
            _delta = delta;
            _acceptStates = acceptStates;
        }

        private static void ValidateArguments(ISet<char> alphabet, HashSet<string> states,
            HashSet<string> startStates, Dictionary<(string State, char Symbol), HashSet<string>> delta,
            HashSet<string> acceptStates)
        {
            if (alphabet == null) throw new ArgumentNullException(nameof(alphabet));
            if (states == null) throw new ArgumentNullException(nameof(states));
            if (startStates == null) throw new ArgumentNullException(nameof(startStates));
            if (delta == null) throw new ArgumentNullException(nameof(delta));
            if (acceptStates == null) throw new ArgumentNullException(nameof(acceptStates));

            // Start state must be included in states
            foreach (var startState in startStates)
            {
                if (!states.Contains(startState))
                {
                    throw new ArgumentException(
                        $"Start state {startState} not contained in the array of states. " +
                        "Either add it to the states, or choose a different start state.",
                        nameof(startStates));
                }
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
        }

        /// <summary>
        /// Returns the derived deterministic automaton from this non-deterministic one.
        /// </summary>
        public DeterministicFiniteStateAutomaton DeriveDeterministic()
        {
            // The combinations that will form the states of the DFA
            var knownCombinedStates = new HashSet<EquatableHashSet<string>> {new EquatableHashSet<string>(_startStates)};
            // The newly discovered combinations
            var newCombinedStates = new HashSet<EquatableHashSet<string>> {new EquatableHashSet<string>(_startStates)};
            // Name of the initial state of the DFA
            var startStateNames = newCombinedStates.Single().ContentsToString();
            // The transition table of the DFA
            var dDelta = new Dictionary<(string State, char Symbol), string>();

            // Every iteration, we process the newly discovered states
            do
            {
                // Reset newly discovered states
                var newCombinedStatesCopy = newCombinedStates.ToImmutableArray();
                newCombinedStates = new HashSet<EquatableHashSet<string>>();
                foreach (var currentCombinedState in newCombinedStatesCopy)
                {
                    foreach (var symbol in _alphabet)
                    {
                        // Get all the following states for this symbol
                        var newCombinedState = new EquatableHashSet<string>();
                        foreach (var state in currentCombinedState)
                        {
                            newCombinedState.UnionWith(_delta.GetValueOrEmptySet((state, symbol)));
                        }

                        // Add the relationship to the DFA transition table
                        dDelta[(currentCombinedState.ContentsToString(), symbol)] = newCombinedState.ContentsToString();
                        newCombinedStates.Add(newCombinedState);
                    }
                }

                // Stop if no new states were discovered
                newCombinedStates.ExceptWith(knownCombinedStates);
                if (!newCombinedStates.Any())
                {
                    break;
                }

                knownCombinedStates.UnionWith(newCombinedStates);
            } while (true);

            // Get the names of the accepting states
            // A combined state is accepting iff it any of its underlying states are accepting
            var acceptingDStates = knownCombinedStates
                .Where(includedStates => includedStates
                    .Any(state => _acceptStates.Contains(state)))
                .Select(x => x.ContentsToString())
                .ToHashSet();

            // Get names of all states
            var dStateNames = knownCombinedStates
                .Select(x => x.ContentsToString())
                .ToHashSet();

            return new DeterministicFiniteStateAutomaton(
                _alphabet,
                dStateNames,
                startStateNames,
                dDelta,
                acceptingDStates
            );
        }

        /// <summary>
        /// Returns a new NFA that accepts any word accepted by this one followed by any word accepted by
        /// <paramref name="other"/>.
        /// </summary>
        public NondeterministicFiniteStateAutomaton ConcatWith(NondeterministicFiniteStateAutomaton other)
        {
            // Copy states, initial states, accept states
            var concatStates = new HashSet<string>(_states);
            concatStates.UnionWith(other._states);

            var concatStartStates = new HashSet<string>(_startStates);
            var concatAcceptStates = new HashSet<string>(other._acceptStates);

            // Copy delta, collisions are handled below
            var concatDelta = new Dictionary<(string State, char Symbol), HashSet<string>>(_delta);

            // Find state names to replace and generate corresponding new names
            var replaceNames = GenerateReplaceNames(concatStates, other._states);

            // Add new state names
            concatStates.UnionWith(replaceNames.Values);

            // Copy transitions from other into combined delta
            foreach (var ((state, symbol), value) in other._delta)
            {
                var newState = replaceNames.GetValueOrDefault(state) ?? state;
                var newValue = new HashSet<string>(value);
                newValue.ReplaceValues(replaceNames);
                concatDelta[(newState, symbol)] = newValue;
            }

            // Apply replacements to acceptStates
            foreach (var (oldName, newName) in replaceNames)
            {
                if (other._acceptStates.Contains(oldName))
                {
                    concatAcceptStates.Remove(oldName);
                    concatAcceptStates.Add(newName);
                }
            }

            // Connect each of our accept states with each state following an initial state of other
            foreach (var otherStartStateOldName in other._startStates)
            {
                string otherStartState = replaceNames.GetValueOrDefault(otherStartStateOldName)
                                         ?? otherStartStateOldName;
                foreach (var symbol in _alphabet)
                {
                    foreach (var ourAcceptState in _acceptStates)
                    {
                        concatDelta[(ourAcceptState, symbol)].UnionWith(concatDelta[(otherStartState, symbol)]);
                    }
                }
            }
            
            // Construct new NFA
            var result = new NondeterministicFiniteStateAutomaton();
            result.Initialize(
                _alphabet,
                concatStates,
                concatStartStates,
                concatDelta,
                concatAcceptStates
            );
            return result;
        }

        /// <summary>
        /// Returns a new NFA that accepts any sequence of 0 or more words accepted by this NFA.
        /// </summary>
        public NondeterministicFiniteStateAutomaton Repeating()
        {
            var repeatingStates = new HashSet<string>(_states);
            var repeatingStartStates = new HashSet<string>(_startStates);
            var repeatingAcceptStates = new HashSet<string>(_acceptStates);

            // Add 1 state accepting the empty word, if it is not already accepted
            if (!Run(""))
            {
                string emptyWordAcceptState = GetNameNotInSet(repeatingStates, "e");
                repeatingStates.Add(emptyWordAcceptState);
                repeatingStartStates.Add(emptyWordAcceptState);
                repeatingAcceptStates.Add(emptyWordAcceptState);
            }
            
            // Build new delta
            var repeatingDelta = new Dictionary<(string State, char Symbol), HashSet<string>>(_delta);

            foreach (var acceptState in _acceptStates)
            {
                foreach (var startState in _startStates)
                {
                    foreach (var symbol in _alphabet)
                    {
                        repeatingDelta.UnionValueWith((acceptState, symbol),
                            _delta.GetValueOrEmptySet((startState, symbol)));
                    }
                }
            }

            var result = new NondeterministicFiniteStateAutomaton();
            result.Initialize(
                _alphabet,
                repeatingStates,
                repeatingStartStates,
                repeatingDelta,
                repeatingAcceptStates
            );
            return result;
        }

        /// <summary>
        /// Returns a new NFA that implements the union of this and <paramref name="other"/>s languages.
        /// </summary>
        public NondeterministicFiniteStateAutomaton UnionWith(NondeterministicFiniteStateAutomaton other)
        {
            // Copy states, initial states, accept states
            var unionStates = new HashSet<string>(_states);
            unionStates.UnionWith(other._states);

            var unionStartStates = new HashSet<string>(_startStates);
            unionStartStates.UnionWith(other._startStates);

            var unionAcceptStates = new HashSet<string>(_acceptStates);
            unionAcceptStates.UnionWith(other._acceptStates);

            // Copy delta, collisions are handled below
            var unionDelta = new Dictionary<(string State, char Symbol), HashSet<string>>(_delta);
            foreach (var (key, value) in other._delta)
            {
                if (!unionDelta.ContainsKey(key))
                {
                    unionDelta[key] = value;
                }
            }

            // Find state names to replace and generate corresponding new names
            var replaceNames = GenerateReplaceNames(unionStates, other._states);

            // Add new state names
            unionStates.UnionWith(replaceNames.Values);

            foreach (var (oldName, newName) in replaceNames)
            {
                if (other._startStates.Contains(oldName))
                {
                    unionStartStates.Add(newName);
                }

                if (other._acceptStates.Contains(oldName))
                {
                    unionAcceptStates.Add(newName);
                }

                foreach (var symbol in _alphabet)
                {
                    var deltaValue = other._delta.GetValueOrEmptySet((oldName, symbol));
                    deltaValue.ReplaceValues(replaceNames);
                    unionDelta[(newName, symbol)] = deltaValue;
                }
            }

            var result = new NondeterministicFiniteStateAutomaton();
            result.Initialize(
                _alphabet,
                unionStates,
                unionStartStates,
                unionDelta,
                unionAcceptStates
            );
            return result;
        }

        /// <summary>
        /// Generates a new, unique name for each value present in both sets.
        /// </summary>
        private Dictionary<string, string> GenerateReplaceNames(ICollection<string> origin, IEnumerable<string> other)
        {
            var replaceNames = new Dictionary<string, string>();

            foreach (var ambState in _states.Intersect(other))
            {
                replaceNames[ambState] = GetNameNotInSet(origin, ambState);
            }

            return replaceNames;
        }

        /// <summary>
        /// Returns a name not in the <paramref name="origin"/> set.
        /// </summary>
        private static string GetNameNotInSet(ICollection<string> origin, string baseName)
        {
            string newName;
            int i = 0;
            do
            {
                i++;
                newName = $"{baseName} ({i})";
            } while (origin.Contains(newName));

            return newName;
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
            var currentStates = _startStates.ToHashSet();
            var newStates = new HashSet<string>();

            foreach (var c in input)
            {
                if (!_alphabet.Contains(c))
                {
                    throw new ArgumentException(
                        $"Input character '{c}' not found in alphabet. " +
                        $"Either add '{c}' to the alphabet, or remove it from the input.");
                }

                foreach (var currentState in currentStates)
                {
                    newStates.UnionWith(_delta.GetValueOrEmptySet((currentState, c)));
                }

                if (!newStates.Any())
                {
                    return false;
                }

                currentStates = newStates;
                newStates = new HashSet<string>();
            }

            currentStates.IntersectWith(_acceptStates);
            return currentStates.Any();
        }
    }
}