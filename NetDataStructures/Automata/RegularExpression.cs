using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace NetDataStructures.Automata
{
    public class RegularExpression
    {
        private ImmutableHashSet<char> _alphabet;
        private string _expr;

        private static ImmutableHashSet<char> _reservedSymbols =
            new[] {'(', ')', '*', '∪', '∅', 'ε'}.ToImmutableHashSet();

        public RegularExpression(IEnumerable<char> alphabet, string expr)
        {
            _expr = expr;
            _alphabet = alphabet.ToImmutableHashSet();

            foreach (var symbol in _alphabet)
            {
                if (_reservedSymbols.Contains(symbol))
                {
                    throw new ArgumentException(
                        $"Alphabet contains the reserved symbol: {symbol}. " +
                        "The alphabet of a regular expression must not contain any of these reserved symbols: " +
                        string.Join(", ", _reservedSymbols) +
                        ". Remove these symbols from the alphabet.");
                }
            }


            var allowedCharacters = new HashSet<char>(_reservedSymbols);
            allowedCharacters.UnionWith(_alphabet);
            foreach (var c in expr)
            {
                if (!allowedCharacters.Contains(c))
                {
                    throw new ArgumentException(
                        $"The character '{c}' is not contained in the alphabet and is not a control character." +
                        "Either add it to the alphabet or remove it from the expression.");
                }
            }
        }

        public NondeterministicFiniteStateAutomaton DeriveAutomaton()
        {
            return DeriveAutomatonInternal(_expr);
        }

        // (A∪B)*C

        private NondeterministicFiniteStateAutomaton DeriveAutomatonInternal(string expr)
        {
            return ParseExpressionWithoutParentheses(expr);
            // TODO WIP
            while (true)
            {
                var localEndIndex = expr.IndexOf(')');
                var localStartIndex = expr.LastIndexOf('(', 0, localEndIndex);
                var subExpr = expr.Substring(localStartIndex, localEndIndex - localStartIndex);
                var subMachine = ParseExpressionWithoutParentheses(subExpr);
            }
        }

        private NondeterministicFiniteStateAutomaton ParseExpressionWithoutParentheses(string expr)
        {
            // unionMachine will implement the entire expression without parentheses
            NondeterministicFiniteStateAutomaton unionMachine = null;
            var unionParts = expr.Split('∪');
            
            foreach (var part in unionParts)
            {
                // part machine will contain one part that is being unioned
                NondeterministicFiniteStateAutomaton partMachine = null;
                for (var i = 0; i < part.Length; i++)
                {
                    // symbolMachine will contain a single symbol, optionally with the repeating operator
                    var symbol = part[i];
                    var symbolMachine = CreateAutomatonFromSymbol(symbol);
                    if (i + 1 < part.Length && part[i + 1] == '*')
                    {
                        symbolMachine = symbolMachine.Repeating();
                        i++;
                    }
                    
                    // merge each symbolMachine into partMachine
                    partMachine = partMachine?.ConcatWith(symbolMachine) ?? symbolMachine;
                }

                // merge each partMachine into unionMachine
                unionMachine = unionMachine?.UnionWith(partMachine) ?? partMachine;
            }

            return unionMachine;
        }

        private NondeterministicFiniteStateAutomaton CreateAutomatonFromSymbol(char symbol)
        {
            return new NondeterministicFiniteStateAutomaton(
                alphabet: _alphabet,
                states: new[] {"s0", "s1"},
                startStates: new[] {"s0"},
                delta: new Dictionary<(string State, char Symbol), IEnumerable<string>>
                {
                    {("s0", symbol), new[] {"s1"}}
                },
                acceptStates: new[] {"s1"}
            );
        }
    }

    internal class ParserException : Exception
    {
        public ParserException()
        {
        }

        public ParserException(string message) : base(message)
        {
        }
    }
}