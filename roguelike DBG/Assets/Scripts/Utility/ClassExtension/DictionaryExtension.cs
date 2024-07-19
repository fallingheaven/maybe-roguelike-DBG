using System.Collections.Generic;

namespace Utility.ClassExtension
{
    public static class DictionaryExtension
    {
        public static bool TryRemove<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.ContainsKey(key) && dictionary.Remove(key);
        }
        
        public static bool TryRemove<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
        {
            return dictionary.ContainsKey(key) && dictionary[key].Remove(value);
        }

        public static void TryAdd<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = new List<TValue>();
            }

            dictionary[key].Add(value);
        }
    }
}