using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetDataStructures.Automata
{
    public class RegularExpression
    {
        private ImmutableHashSet<char> _alphabet;
        private string _expr;

        private static ImmutableHashSet<char> _reservedSymbols =
            new[] {'(', ')', '*', '⋅', '∪', '∅', 'ε'}.ToImmutableHashSet();

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

        /**
         * Returns an automaton that implements the same language as this regular expression.
         */
        public NondeterministicFiniteStateAutomaton DeriveAutomaton()
        {
            var reversePolish = ConvertToReversePolishNotation(_expr);
            return ParseReversePolish(reversePolish);
        }

        /**
         * Converts a regular expression in infix notation into reverse polish notation.
         */
        private string ConvertToReversePolishNotation(string expr)
        {
            var output = new StringBuilder(expr.Length);
            var operatorStack = new Stack<char>();
            // Indicates if the previous symbol can be on the left side of a juxtaposition 
            // (implicit complex product, i.e.  "ab" which is equivalent to "a⋅b")
            var possibleJuxtaposition = false;

            foreach (var c in expr)
            {
                ProcessToken(c, ref possibleJuxtaposition, operatorStack, output);
            }

            // Push remaining tokens onto the output
            PopTokens(new[] {'*', '⋅', '∪'}, operatorStack, output);

            return output.ToString();
        }

        private void ProcessToken(char c, ref bool possibleJuxtaposition, Stack<char> operatorStack,
            StringBuilder output)
        {
            switch (c)
            {
                // If the token is a left parenthesis, push it onto the operator stack
                case '(':
                    // If a complex product by juxtaposition is encountered, push the complex product operator
                    // onto the operator stack
                    if (possibleJuxtaposition)
                    {
                        ProcessToken('⋅', ref possibleJuxtaposition, operatorStack, output);
                    }

                    possibleJuxtaposition = false;
                    operatorStack.Push(c);
                    break;

                // If the token is a right parenthesis:
                case ')':
                    possibleJuxtaposition = true;
                    // Pop tokens from the operator stack to the output, until a left parenthesis is found
                    PopTokens(new[] {'*', '⋅', '∪'}, operatorStack, output);

                    // Discard the left parenthesis on the operator stack
                    // If no left parenthesis is found, there are mismatched parentheses
                    if (operatorStack.Pop() != '(')
                    {
                        throw new MismatchedParenthesesException();
                    }

                    break;

                // If the token is a union operator:
                case '∪':
                    possibleJuxtaposition = false;
                    // Pop operators with higher precedence from the operator stock to the output
                    PopTokens(new[] {'*', '⋅'}, operatorStack, output);
                    // Push it onto the operator stack
                    operatorStack.Push(c);
                    break;

                // If the token is a complex product operator:
                case '⋅':
                    possibleJuxtaposition = false;
                    // Pop operators with higher precedence from the operator stock to the output
                    PopTokens(new[] {'*'}, operatorStack, output);
                    // Push it onto the operator stack
                    operatorStack.Push(c);
                    break;

                // If the token is a Kleene operator (highest precedence):
                case '*':
                    possibleJuxtaposition = true;
                    // Push it onto the operator stack
                    operatorStack.Push(c);
                    break;

                // Otherwise, the token is a sub-expression and is pushed to the output
                default:
                    // If a complex product by juxtaposition is encountered, push the complex product operator
                    // onto the operator stack
                    if (possibleJuxtaposition)
                    {
                        ProcessToken('⋅', ref possibleJuxtaposition, operatorStack, output);
                    }

                    possibleJuxtaposition = true;
                    output.Append(c);
                    break;
            }
        }

        /**
         * Parse a regular expression in reverse polish notation into an automaton.
         */
        private NondeterministicFiniteStateAutomaton ParseReversePolish(string expr)
        {
            var partialAutomata = new Stack<NondeterministicFiniteStateAutomaton>();
            foreach (var c in expr)
            {
                switch (c)
                {
                    //{'(', ')', '*', '⋅', '∪', '∅', 'ε'}
                    case '∪':
                    {
                        var right = partialAutomata.Pop();
                        var left = partialAutomata.Pop();
                        partialAutomata.Push(left.UnionWith(right));
                        break;
                    }
                    case '⋅':
                    {
                        var right = partialAutomata.Pop();
                        var left = partialAutomata.Pop();
                        partialAutomata.Push(left.ConcatWith(right));
                        break;
                    }
                    case '*':
                        partialAutomata.Push(partialAutomata.Pop().Repeating());
                        break;
                    case '∅':
                        partialAutomata.Push(CreateEmptyAutomaton());
                        break;
                    case 'ε':
                        partialAutomata.Push(CreateEmptyWordAutomaton());
                        break;
                    default:
                        partialAutomata.Push(CreateAutomatonFromSymbol(c));
                        break;
                }
            }

            return partialAutomata.Single();
        }

        /**
         * While one of the given <paramref name="tokens"/> is at the top of the
         * <paramref name="from"/> stack, pop it and append it to the <paramref name="to"/>
         * StringBuilder.
         */
        private void PopTokens(char[] tokens, Stack<char> from, StringBuilder to)
        {
            while (from.TryPeek(out char token) && tokens.Contains(token))
            {
                to.Append(from.Pop());
            }
        }

        /**
         * Creates an automaton that accepts only the empty word
         */
        private NondeterministicFiniteStateAutomaton CreateEmptyWordAutomaton()
        {
            return new NondeterministicFiniteStateAutomaton(
                alphabet: _alphabet,
                states: new string[] {"s0"},
                startStates: new string[] {"s0"},
                delta: new Dictionary<(string State, char Symbol), IEnumerable<string>>(),
                acceptStates: new string[] {"s0"}
            );
        }

        /**
         * Creates an automaton that accepts nothing
         */
        private NondeterministicFiniteStateAutomaton CreateEmptyAutomaton()
        {
            return new NondeterministicFiniteStateAutomaton(
                alphabet: _alphabet,
                states: new string[0],
                startStates: new string[0],
                delta: new Dictionary<(string State, char Symbol), IEnumerable<string>>(),
                acceptStates: new string[0]
            );
        }

        /**
         * Creates an automaton that accepts the language containing only the given symbol
         */
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


    [Serializable]
    public class MismatchedParenthesesException : Exception
    {
        public MismatchedParenthesesException() : base()
        {
        }

        protected MismatchedParenthesesException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}