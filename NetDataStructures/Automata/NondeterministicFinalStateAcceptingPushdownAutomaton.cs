﻿using System;
using System.Collections.Generic;
using System.Linq;
using NetDataStructures.Automata.Internal;

namespace NetDataStructures.Automata
{
    /// <summary>
    /// A pushdown automaton that accepts an input iif it is in an accepting state and the complete input has been
    /// consumed. 
    /// </summary>
    public class NondeterministicFinalStateAcceptingPushdownAutomaton : NondeterministicPushdownAutomatonBase
    {
        /// <summary>
        /// Returns the accepting states.
        /// </summary>
        public IEnumerable<string> AcceptStates => _acceptStates;

        private HashSet<string> _acceptStates;

        public NondeterministicFinalStateAcceptingPushdownAutomaton(
            IEnumerable<char> alphabet,
            IEnumerable<char> stackAlphabet,
            char initialStackItem,
            IEnumerable<string> states,
            string startState,
            IDictionary<(string State, char? Symbol, char Pop), IEnumerable<(string State, string Push)>> delta,
            IEnumerable<string> acceptStates
        ) : base(alphabet, stackAlphabet, initialStackItem, states, startState, delta)
        {
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

            _acceptStates = acceptStates.ToHashSet();
        }

        /// <summary>
        /// We accept iif we are in an accepting state and have consumed the entire input. 
        /// </summary>
        protected override bool IsAcceptingConfiguration(List<char> stack, string state, string input, int index)
        {
            return _acceptStates.Contains(state) && index == input.Length;
        }
    }
}