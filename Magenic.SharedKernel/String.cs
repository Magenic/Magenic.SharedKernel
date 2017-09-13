using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Provides a set of static methods on strings and string builders.
    /// </summary>
    public static class StringEx
    {
        #region Constants

        /// <summary>
        /// String representation of null.
        /// </summary>
        public const string NULL = "<null>";
        /// <summary>
        /// String representation of empty.
        /// </summary>
        public const string EMPTY = "<empty>";
        /// <summary>
        /// String representation of white space.
        /// </summary>
        public const string WHITE_SPACE = "<white space>";

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates string comprised of c repeated count times.
        /// </summary>
        public static string CreateString(char c, int count) => new string(c, count);

        /// <summary>
        /// Creates String Builder.
        /// </summary>
        public static StringBuilder CreateSB(string value = null)
            => (!string.IsNullOrEmpty(value))
                ? new StringBuilder(value)
                : new StringBuilder();

        /// <summary>
        /// Returns true if string is empty.
        /// </summary>
        public static bool IsEmpty(this string source)
            => (source.CompareTo(string.Empty) == 0);

        /// <summary>
        /// Determines whether source contains value using comparisonType.
        /// </summary>
        public static bool Contains(
            this string source,
            string value,
            StringComparison comparisonType)
            => (source.IndexOf(value, comparisonType) != -1);

        /// <summary>
        /// Handles null, empty, white space and set strings for display.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <returns>Display string.</returns>
        public static string Display(this string source)
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                return source;
            }
            else if (source == null)
            {
                return NULL;
            }
            else if (source.IsEmpty())
            {
                return EMPTY;
            }
            else
            {
                return WHITE_SPACE;
            }
        }

        /// <summary>
        /// Returns true if passed in string sequence is null or empty.
        /// Returns true if passed in string sequence is comprised of null or empty string(s).
        /// Otherwise returns false.
        /// </summary>
        /// <param name="source">A sequence of strings.</param>
        /// <returns>Boolean.</returns>
        public static bool IsNullOrEmpty(IEnumerable<string> source)
            => WithIsNullOrEmptyOrAll(source, string.IsNullOrEmpty);

        /// <summary>
        /// Returns true if passed in string sequence is null or empty.
        /// Returns true if passed in string sequence is comprised of null or whitespace string(s).
        /// Otherwise returns false.
        /// </summary>
        /// <param name="source">A sequence of strings.</param>
        /// <returns>Boolean.</returns>
        public static bool IsNullOrWhiteSpace(IEnumerable<string> source)
            => WithIsNullOrEmptyOrAll(source, string.IsNullOrWhiteSpace);

        /// <summary>
        /// Generates a new string comprised of value and source in that order.
        /// </summary>
        public static string Prepend(this string source, string value)
            => $"{value}{source}";

        /// <summary>
        /// Clears source and then appends value to it.
        /// </summary>
        public static StringBuilder Set(this StringBuilder source, string value)
            => source.Clear().Append(value);

        /// <summary>
        /// Trims passed in string if it is not null or empty.
        /// </summary>
        /// <param name="text">String.</param>
        /// <returns>Trimmed string.</returns>
        public static string TrimIfSet(this string text)
            => (!string.IsNullOrEmpty(text))
                ? text.Trim()
                : text;

        #endregion

        #region Private Methods

        private static bool WithIsNullOrEmptyOrAll(
            IEnumerable<string> source,
            Func<string, bool> fnAll)
            => (Seq.IsNullOrEmpty(source) || source.All(fnAll));

        #endregion
    }
}