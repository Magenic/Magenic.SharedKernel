using System;
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
        public static string CreateString(char c, int count)
        {
            return new string(c, count);
        }

        /// <summary>
        /// Creates String Builder.
        /// </summary>
        public static StringBuilder CreateSB(string value = null)
        {
            return (!string.IsNullOrEmpty(value))
                ? new StringBuilder(value)
                : new StringBuilder();
        }

        /// <summary>
        /// Returns true if string is empty.
        /// </summary>
        public static bool IsEmpty(this string source)
        {
            return (source.CompareTo(string.Empty) == 0);
        }

        /// <summary>
        /// Determines whether source contains value using comparisonType.
        /// </summary>
        public static bool Contains(
            this string source,
            string value,
            StringComparison comparisonType)
        {
            return (source.IndexOf(value, comparisonType) != -1);
        }

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
        /// Clears source and then appends value to it.
        /// </summary>
        public static StringBuilder Set(
            this StringBuilder source,
            string value)
        {
            return source.Clear().Append(value);
        }

        /// <summary>
        /// Generates a new string comprised of value and source in that order.
        /// </summary>
        public static string Prepend(
            this string source,
            string value)
        {
            return $"{value}{source}";
        }

        /// <summary>
        /// Trims passed in string if it is not null or empty.
        /// </summary>
        /// <param name="text">String.</param>
        /// <returns>Trimmed string.</returns>
        public static string TrimIfSet(this string text)
        {
            return (!string.IsNullOrEmpty(text))
                ? text.Trim()
                : text;
        }

        #endregion
    }
}