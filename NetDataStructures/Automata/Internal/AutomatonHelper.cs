using System;
using System.Collections.Generic;

namespace NetDataStructures.Automata.Internal
{
    public static class AutomatonHelper
    {
        /// <summary>
        /// Throws an exception if the <paramref name="symbol"/> is not contained in the <paramref name="alphabet"/>.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static void CheckSymbolInAlphabet(ISet<char> alphabet, char symbol)
        {
            if (!alphabet.Contains(symbol))
            {
                throw new ArgumentException(
                    $"Input character '{symbol}' not found in alphabet. " +
                    $"Either add '{symbol}' to the alphabet, or remove it from the input.");
            }
        }
        
    }
}