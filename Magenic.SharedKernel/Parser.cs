using System;
using System.Globalization;
using System.Numerics;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// A class to hold parser methods that return a nullable result rather than use an out argument
    /// </summary>
    public static class Parser
    {
        #region Public Methods
        /// <summary>
        /// Delegate for the TryParse methods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public delegate bool TryParseDelegate<T>(string s, out T result);

        /// <summary>
        /// Returns a nullable value based on the given tryParse delegate and string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="tryParse"></param>
        /// <returns></returns>
        public static T? ParseToNullable<T>(string s, TryParseDelegate<T> tryParse) where T : struct
        {
            if (tryParse(s, out T result))
            {
                return result;
            }
            return null;
        }
        #endregion
    }
}
