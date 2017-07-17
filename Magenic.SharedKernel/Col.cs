using System;
using System.Collections.Generic;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Provides a set of static methods on collections.
    /// </summary>
    public static class ColEx
    {
        #region Public Static Methods

        /// <summary>
        /// Creates set based on passed in sequence and comparer.  
        /// </summary>
        /// <typeparam name="TSource">Generic data type of source.</typeparam>
        /// <param name="seq">The source collection of the set.</param>
        /// <param name="comparer">
        /// Equality comparer to quickly determine whether an object is already in the set or not.
        ///</param>
        ///<returns>Set.</returns>
        public static ISet<TSource> CreateSet<TSource>(
             IEnumerable<TSource> seq = null,
             IEqualityComparer<TSource> comparer = null)
        {
            if (seq != null)
            {
                return (comparer != null)
                     ? new HashSet<TSource>(seq, comparer)
                     : new HashSet<TSource>(seq);
            }
            else
            {
                return (comparer != null)
                     ? new HashSet<TSource>(comparer)
                     : new HashSet<TSource>();
            }
        }

        /// <summary>
        /// Creates a map from passed in map and comparer.
        /// </summary>
        /// <typeparam name="TKey">Generic data type of key.</typeparam>
        /// <typeparam name="TValue">Generic data type of value.</typeparam>
        /// <param name="map">Key-value pair collection.</param>
        /// <param name="comparer">Equality comparer of the collection</param>
        /// <returns>A new instance of map.</returns>
        public static IDictionary<TKey, TValue> CreateMap<TKey, TValue>(
             IDictionary<TKey, TValue> map = null,
             IEqualityComparer<TKey> comparer = null)
        {
            if (map != null)
            {
                return (comparer != null)
                     ? new Dictionary<TKey, TValue>(map, comparer)
                     : new Dictionary<TKey, TValue>(map);
            }
            else
            {
                return (comparer != null)
                     ? new Dictionary<TKey, TValue>(comparer)
                     : new Dictionary<TKey, TValue>();
            }
        }

        /// <summary>
        /// Creates a map with capacity.
        /// </summary>
        /// <typeparam name="TKey">Generic data type of key.</typeparam>
        /// <typeparam name="TValue">Generic data type of value.</typeparam>
        /// <param name="capacity"> 
        ///	Initial capacity eliminates the need to perform a number of resizing operations while adding elements.
        /// </param>
        /// <returns>A new instance of generic dictionary.</returns>
        public static IDictionary<TKey, TValue> CreateMap<TKey, TValue>(int capacity)
            => new Dictionary<TKey, TValue>(capacity);

        /// <summary>
        /// Converts an IEnumerable of type KeyValuePair into IDictionary collection if the source is not null.
        /// </summary>
        /// <typeparam name="TKey">Generic data type of key.</typeparam>
        /// <typeparam name="TValue">Generic data type of value.</typeparam>
        /// <param name="source">IEnumerable collection of KeyValuePair.</param>
        /// <returns>A new instance of dictionary.</returns>
        public static IDictionary<TKey, TValue> CreateMap<TKey, TValue>(
             IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            if (source != null)
            {
                IDictionary<TKey, TValue> map = CreateMap<TKey, TValue>();

                if (!Seq.IsNullOrEmpty(source))
                {
                    source.Apply(kvp => map.UpdateOrAdd(kvp.Key, kvp.Value));
                }

                return map;
            }
            else
            {
                throw new ArgumentNullException(nameof(source));
            }
        }

        /// <summary>
        /// Creates an empty vector.
        /// </summary>
        /// <typeparam name="TSource">Type of the strongly-typed collection.</typeparam>
        /// <returns>A new instance of list.</returns>
        public static IList<TSource> CreateVector<TSource>() => new List<TSource>();

        /// <summary>
        /// Creates a vector with capacity.
        /// </summary>
        /// <typeparam name="TSource">Type of the strongly-typed collection.</typeparam>
        /// <param name="capacity">
        ///	Capacity is the number of elements that the list can store before resizing is required.
        /// </param>
        /// <returns>A new instance of list.</returns>
        public static IList<TSource> CreateVector<TSource>(int capacity)
            => new List<TSource>(capacity);

        /// <summary>
        /// Creates a vector from sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the strongly-typed collection.</typeparam>
        /// <param name="seq">The source collection of the vector.</param>
        /// <returns>A new instance of list based on the values in an IEnumerable collection.</returns>
        public static IList<TSource> CreateVector<TSource>(IEnumerable<TSource> seq)
            => new List<TSource>(seq);

        /// <summary>
        /// Creates a key/value pair.
        /// </summary>
        /// <typeparam name="TKey">Generic data type of key.</typeparam>
        /// <typeparam name="TValue">Generic data type of value.</typeparam>
        /// <param name="key">Object to be used as Key.</param>
        /// <param name="value">Object to be associated to the Key.</param>
        /// <returns>A new instance of KeyValuePair initialized by Key and a Value from the input parameters.</returns>
        public static KeyValuePair<TKey, TValue> CreateKVP<TKey, TValue>(
             TKey key,
             TValue value) => new KeyValuePair<TKey, TValue>(key, value);

        /// <summary>
        /// Returns a vector with count elements each resulting from calling fn.
        /// </summary>
        /// <typeparam name="TResult">Generic data type of the expected result.</typeparam>
        /// <param name="fn">Predefined delegate type for a method that returns some value of the type TResult.</param>
        /// <param name="count">Number of repetitions.</param>
        /// <returns>A new instance of a list.</returns>
        public static IList<TResult> Repeat<TResult>(
             Func<TResult> fn,
             int count)
        {
            IList<TResult> vector = CreateVector<TResult>(count);

            Util.Repeat(() => vector.Add(fn()), count);

            return vector;
        }

        /// <summary>
        /// Returns a vector with count elements each resulting from calling fn.
        /// </summary>
        /// <typeparam name="TResult">Generic data type of the expected result.</typeparam>
        /// <param name="fn">Predefined delegate type for a method that returns some value of the type TResult while accepting an int as a parameter.</param>
        /// <param name="count">Number of repetition.</param>
        /// <returns>A new instance of a list.</returns>
        public static IList<TResult> Repeat<TResult>(
             Func<int, TResult> fn,
             int count)
        {
            IList<TResult> vector = CreateVector<TResult>(count);

            Util.Repeat(i => vector.Add(fn(i)), count);

            return vector;
        }

        /// <summary>
        /// Returns key's value if map contains key, value otherwise.
        /// </summary>
        /// <typeparam name="TKey">Generic data type of key.</typeparam>
        /// <typeparam name="TValue">Generic data type of value.</typeparam>
        /// <param name="map">Collection object to search for the given key</param>
        /// <param name="key">Object to be used as Key.</param>
        /// <param name="value">Object to be used as return value if key was not found in the collection.</param>
        /// <returns>
        /// The value corresponding to the key or the value passed as parameter if the key was not found in the map.
        /// </returns>
        public static TValue TryGetValue<TKey, TValue>(
             this IDictionary<TKey, TValue> map,
             TKey key,
             TValue value) => (map.ContainsKey(key))
                ? map[key]
                : value;

        /// <summary>
        /// Update value in dictionary corresponding to key if found, else add new entry.
        /// </summary>
        /// <typeparam name="TKey">Generic data type of key.</typeparam>
        /// <typeparam name="TValue">Generic data type of value.</typeparam>
        /// <param name="map">Collection object to search for the given key.</param>
        /// <param name="key">Object to be used as Key.</param>
        /// <param name="value">The value to be used to Update or Add an item to the dictionary.</param>
        /// <returns>True if entry was updated, false if entry was added.</returns>
        public static bool UpdateOrAdd<TKey, TValue>(
             this IDictionary<TKey, TValue> map,
             TKey key,
             TValue value)
        {
            bool ret = map.ContainsKey(key);

            if (ret)
            {
                map[key] = value;
            }
            else
            {
                map.Add(key, value);
            }

            return ret;
        }

        /// <summary>
        /// If key already had a value in map, set old value into passed in output parameter.
        /// </summary>
        /// <typeparam name="TKey">Generic data type of key.</typeparam>
        /// <typeparam name="TValue">Generic data type of value.</typeparam>
        /// <param name="map">Collection object to search for the given key.</param>
        /// <param name="key">Object to be used as Key.</param>
        /// <param name="value">The value to be used to Update or Add an item to map.</param>
        /// <param name="oldValue">
        /// If map already contains a key, the value in the collection will be assigned to this output parameter.
        /// </param>
        /// <returns>True if entry was updated, false if entry was added.</returns>
        public static bool UpdateOrAdd<TKey, TValue>(
             this IDictionary<TKey, TValue> map,
             TKey key,
             TValue value,
             out TValue oldValue)
        {
            bool ret = map.ContainsKey(key);

            if (ret)
            {
                oldValue = map[key];
                map[key] = value;
            }
            else
            {
                oldValue = default(TValue);
                map.Add(key, value);
            }

            return ret;
        }

        /// <summary>
        /// Update the value if the Key in the KeyValuePair is existing, otherwise Add the item to map.
        /// </summary>
        /// <typeparam name="TKey">Generic data type of key.</typeparam>
        /// <typeparam name="TValue">Generic data type of value.</typeparam>
        /// <param name="map">Map.</param>
        /// <param name="kvp">Key value pair.</param>
        /// <returns>True if entry was updated, false if entry was added.</returns>
        public static bool UpdateOrAdd<TKey, TValue>(
             this IDictionary<TKey, TValue> map,
             KeyValuePair<TKey, TValue> kvp) => map.UpdateOrAdd(kvp.Key, kvp.Value);

        /// <summary>
        /// Update value in map for each Key in KeyValuePair sequence if it is existing, otherwise Add the item to map.
        /// </summary>
        /// <typeparam name="TKey">Generic data type of key.</typeparam>
        /// <typeparam name="TValue">Generic data type of value.</typeparam>
        /// <param name="map">Map.</param>
        /// <param name="kvpSeq">Sequence of Key Value Pairs.</param>
        /// <returns>A sequence of booleans.  True if entry was updated, false if entry was added.</returns>
        public static IEnumerable<bool> UpdateOrAdd<TKey, TValue>(
             this IDictionary<TKey, TValue> map,
             IEnumerable<KeyValuePair<TKey, TValue>> kvpSeq)
            => kvpSeq.Map(kvp => map.UpdateOrAdd(kvp));

        /// <summary>
        /// Returns true if collection is empty.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of the collection.</typeparam>
        /// <param name="col">An object with the type of ICollection<typeparamref name="T>"/>.</param>
        /// <returns>Returns true if collection is empty.</returns>
        public static bool IsEmpty<TSource>(ICollection<TSource> col)
            => (col.Count == 0);

        /// <summary>
        /// Returns true if collection is null or empty.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of collection.</typeparam>
        /// <param name="col">An object with the type of ICollection<typeparamref name="T>"/>.</param>
        /// <returns>Returns true if collection is Null or empty.</returns>
        public static bool IsNullOrEmpty<TSource>(ICollection<TSource> col)
            => (col == null || IsEmpty(col));

        /// <summary>
        /// Formats map contents similar to C5 ToString formatting.
        /// </summary>
        /// <typeparam name="TKey">Generic data type of key.</typeparam>
        /// <typeparam name="TValue">Generic data type of value.</typeparam>
        /// <param name="map">Map.</param>
        /// <returns>Contents of map formatted as C5 ToString formatting of map.</returns>
        public static string ToC5String<TKey, TValue>(
             this IDictionary<TKey, TValue> map)
        {
            if (!IsNullOrEmpty(map))
            {
                return string.Format(
                     "[{0}]",
                     map
                        .Map(kvp => $"{kvp.Key} => {kvp.Value}")
                        .Join("|"));
            }
            else
            {
                return (map != null)
                     ? StringEx.EMPTY
                     : StringEx.NULL;
            }
        }

        #endregion
    }
}