using System;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Provides a set of static methods on arrays.
    /// </summary>
    public static class ArrayEx
    {
        #region Public Static Methods

        /// <summary>
        /// Returns an empty array of type TResult.
        /// </summary>
        /// <typeparam name="TResult">Generic data type of array.</typeparam>
        /// <returns>Returns a new empty instance of an array.</returns>
        public static TResult[] Empty<TResult>() => new TResult[0];

        /// <summary>
        /// Returns true if array is empty.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of array.</typeparam>
        /// <param name="array">Array to be checked if empty or not.</param>
        /// <returns>True if array contains no elements.</returns>
        public static bool IsEmpty<TSource>(this TSource[] array)
            => (array.Length == 0);

        /// <summary>
        /// Returns true if array is null or empty.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of array.</typeparam>
        /// <param name="array">Collection to be checked if null or empty or not.</param>
        /// <returns>True if array is null or empty.</returns>
        public static bool IsNullOrEmpty<TSource>(TSource[] array)
            => (array == null || array.IsEmpty());

        /// <summary>
        /// Returns a random reference into array.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of array.</typeparam>
        /// <param name="array">Collection out of which a random reference is returned.</param>
        /// <param name="random">An instance of an object of type Random.</param>
        /// <returns>Random reference from array.</returns>
        public static TSource RandomRef<TSource>(
             this TSource[] array,
             Random random) => array[random.Next(0, array.Length)];

        #endregion
    }
}