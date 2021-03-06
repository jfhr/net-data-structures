﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NetDataStructures.Automata.Internal;

namespace NetDataStructures.Automata
{
    public class NondeterministicFiniteStateAutomaton : IAutomaton
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
            var knownCombinedStates = new HashSet<EquatableHashSet<string>>
                {new EquatableHashSet<string>(_startStates)};
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
            // Find state names to replace and generate corresponding new names
            var replaceNames = GenerateReplaceNames(other._states);

            // Copy this NFA and rename the states accordingly
            var concat = Copy();
            concat.RenameStatesInPlace(replaceNames);

            // Copy the renamed accept states, we need them later for adding new connections
            var renamedAcceptStatesCopy = new HashSet<string>(concat._acceptStates);

            // Union states, use our start states and their accept states
            concat._states.UnionWith(other._states);
            concat._acceptStates = new HashSet<string>(other._acceptStates);

            // Iff the other NFA accepts the empty word, we keep our accept states
            if (other.Run(""))
            {
                concat._acceptStates.UnionWith(renamedAcceptStatesCopy);
            }

            // Copy transitions from other into combined delta
            foreach (var ((state, symbol), value) in other._delta)
            {
                concat._delta.UnionValueWith((state, symbol), value);
            }

            // Connect each of our accept states with each state following an initial state of other
            foreach (var ourAcceptState in renamedAcceptStatesCopy)
            {
                foreach (var otherStartState in other._startStates)
                {
                    foreach (var symbol in _alphabet)
                    {
                        var destinationStates = other._delta.GetValueOrEmptySet((otherStartState, symbol));
                        concat._delta.UnionValueWith((ourAcceptState, symbol), destinationStates);
                    }
                }
            }

            return concat;
        }

        /// <summary>
        /// Returns a new NFA that accepts any sequence of 0 or more words accepted by this NFA.
        /// </summary>
        public NondeterministicFiniteStateAutomaton Repeating()
        {
            var repeating = Copy();

            // Add 1 state accepting the empty word
            var emptyWordAcceptState = Utils.GetNameNotInSet(repeating._states, "e");
            repeating._states.Add(emptyWordAcceptState);
            repeating._startStates.Add(emptyWordAcceptState);
            repeating._acceptStates.Add(emptyWordAcceptState);

            // Build new delta
            foreach (var acceptState in _acceptStates)
            {
                foreach (var startState in _startStates)
                {
                    foreach (var symbol in _alphabet)
                    {
                        repeating._delta.UnionValueWith((acceptState, symbol),
                            _delta.GetValueOrEmptySet((startState, symbol)));
                    }
                }
            }

            return repeating;
        }

        /// <summary>
        /// Returns a new NFA that implements the union of this and <paramref name="other"/>s languages.
        /// </summary>
        public NondeterministicFiniteStateAutomaton UnionWith(NondeterministicFiniteStateAutomaton other)
        {
            // Find state names to replace and generate corresponding new names
            var replaceNames = GenerateReplaceNames(other._states);

            // Copy this NFA and rename the states accordingly
            var union = Copy();
            union.RenameStatesInPlace(replaceNames);

            // Copy states, initial states, accept states
            union._states.UnionWith(other._states);
            union._startStates.UnionWith(other._startStates);
            union._acceptStates.UnionWith(other._acceptStates);

            // Copy delta
            foreach (var (key, value) in other._delta)
            {
                if (!union._delta.ContainsKey(key))
                {
                    union._delta[key] = value;
                }
            }

            return union;
        }

        /// <summary>
        /// Returns an exact copy of this NFA. The resulting NFA will have a reference to the same alphabet, while all
        /// other properties (states, delta, etc.) will be copies.
        /// </summary>
        public NondeterministicFiniteStateAutomaton Copy()
        {
            var result = new NondeterministicFiniteStateAutomaton();
            result.Initialize(
                _alphabet,
                new HashSet<string>(_states),
                new HashSet<string>(_startStates),
                new Dictionary<(string State, char Symbol), HashSet<string>>(_delta),
                new HashSet<string>(_acceptStates)
            );
            return result;
        }

        /// <summary>
        /// Rename this NFA's states in-place.
        /// </summary>
        private void RenameStatesInPlace(Dictionary<string, string> mapping)
        {
            _states.ReplaceValues(mapping);
            _startStates.ReplaceValues(mapping);
            _acceptStates.ReplaceValues(mapping);
            var entries = _delta.ToImmutableArray();
            foreach (var ((state, symbol), value) in entries)
            {
                _delta.Remove((state, symbol));
                var newState = mapping.GetValueOrDefault(state, state);
                value.ReplaceValues(mapping);
                _delta[(newState, symbol)] = value;
            }
        }

        /// <summary>
        /// Generates a dictionary of replacement names for this automaton's states, so that no value present in
        /// <paramref name="other"/> is used.
        /// </summary>
        private Dictionary<string, string> GenerateReplaceNames(ISet<string> other)
        {
            var replaceNames = new Dictionary<string, string>();

            // Create a set containing the union of our and other state names
            var existingStates = new HashSet<string>(_states);
            existingStates.UnionWith(other);

            // Create a set containing the intersection of our and other state names
            var duplicateStates = new HashSet<string>(_states);
            duplicateStates.IntersectWith(other);

            // Replace all names from the intersection with new names not in the union
            foreach (var ambState in duplicateStates)
            {
                var newName = Utils.GetNameNotInSet(existingStates, ambState);
                replaceNames[ambState] = newName;
                existingStates.Add(newName);
            }

            return replaceNames;
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
            var currentStates = _startStates.ToHashSet();
            var newStates = new HashSet<string>();

            foreach (var c in input)
            {
                Utils.CheckSymbolInAlphabet(_alphabet, c);

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