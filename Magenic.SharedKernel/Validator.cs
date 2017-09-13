using System;
using System.Collections.Generic;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Provides a set of static methods used to validate input.
    /// </summary>
    public static class Validator
    {
        #region Public Methods

        public static Guid ThrowIfEmpty(Guid guid, string name)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException($"{name} is an empty guid!");
            }
            else
            {
                return guid;
            }
        }

        public static T ThrowIfNull<T>(T obj, string name)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
            else
            {
                return obj;
            }
        }

        public static IEnumerable<T> ThrowIfNullOrEmpty<T>(IEnumerable<T> seq, string name)
        {
            if (seq == null)
            {
                throw new ArgumentNullException(name);
            }
            else if (Seq.IsEmpty(seq))
            {
                throw new ArgumentException($"{name} is an empty sequence!");
            }
            else
            {
                return seq;
            }
        }

        public static string ThrowIfNullOrEmpty(string str, string name)
        {
            if (str == null)
            {
                throw new ArgumentNullException(name);
            }
            else if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException($"{name} is an empty string!");
            }
            else
            {
                return str;
            }
        }

        public static string ThrowIfNullOrWhiteSpace(string str, string name)
        {
            ThrowIfNullOrEmpty(str, name);

            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException($"{name} is comprised of white space!");
            }
            else
            {
                return str;
            }
        }

        #endregion
    }
}
