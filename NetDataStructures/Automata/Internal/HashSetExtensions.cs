using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDataStructures.Automata.Internal
{
    internal static class HashSetExtensions
    {
        /// <summary>
        /// Returns a string representing the contents of the set.
        /// </summary>
        public static string ContentsToString(this HashSet<string> set)
        {
            var sortedContents = set.ToArray();
            Array.Sort(sortedContents);
            return "{" + string.Join(',', sortedContents) + "}";
        }

        /// <summary>
        /// Replaces strings in the HashSet with the corresponding values from the <paramref name="replacements"/>
        /// dictionary.
        /// </summary>
        public static void ReplaceValues(this HashSet<string> set, Dictionary<string, string> replacements)
        {
            foreach (var (oldValue, newValue) in replacements)
            {
                if (set.Remove(oldValue))
                {
                    set.Add(newValue);
                }
            }
        }
    }
}