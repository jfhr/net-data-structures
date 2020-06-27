using System.Collections.Generic;

namespace NetDataStructures.Automata.Internal
{
    internal static class DictionaryExtensions
    {
        /// <summary>
        /// Returns the value of the dictionary for the given key,
        /// or an empty <see cref="HashSet{TInnerValue}"/> if no value exists.
        /// </summary>
        public static HashSet<TValue> GetValueOrEmptySet<TKey, TValue>(
            this Dictionary<TKey, HashSet<TValue>> dictionary, TKey key) 
        {
            return dictionary.GetValueOrDefault(key) ?? new HashSet<TValue>();
        }

        /// <summary>
        /// Sets the value of the dictionary at the specified key the union of the existing value and the new set,
        /// or just the new set if no value exists yet.
        /// </summary>
        public static void UnionValueWith<TKey, TValue>(this Dictionary<TKey, HashSet<TValue>> dictionary, TKey key,
            HashSet<TValue> newSet)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].UnionWith(newSet);
            }
            else
            {
                dictionary[key] = newSet;
            }
        } 
    }
}