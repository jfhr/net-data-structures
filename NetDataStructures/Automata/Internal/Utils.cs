using System;
using System.Collections.Generic;

namespace NetDataStructures.Automata.Internal
{
    internal static class Utils
    {
        /// <summary>
        /// Returns a name not in the <paramref name="origin"/> set.
        /// </summary>
        public static string GetNameNotInSet(ICollection<string> origin, string baseName)
        {
            var newName = baseName;
            var (value, index) = GetIndexOfNumberAtEndOfString(baseName);
            while (origin.Contains(newName))
            {
                value++;
                newName = newName.Substring(0, index) + value;
            }
            return newName;
        }

        /// <summary>
        /// Returns the index of the first character of the number at the end of a string,
        /// and the value of that number, or (s.Length, 0) if the string does not end with a number.
        /// </summary>
        public static (int Value, int Index) GetIndexOfNumberAtEndOfString(string s)
        {
            int i = s.Length - 1;
            while (i >= 0 && IsDigit(s[i]))
            {
                i--;
            }

            if (i == s.Length - 1)
            {
                return (s.Length, 0);
            }

            // forwardtrack by 1
            i++;
            return (Int32.Parse(s.Substring(i)), i);
        }

        /// <summary>
        /// Indicates if the character is an arabic (0-9) digit.
        /// </summary>
        public static bool IsDigit(char c) => ('0' <= c && c <= '9');

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