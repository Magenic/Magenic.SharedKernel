using System;
using System.Collections.Concurrent;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Used to avoid repeating the calculation of results for previously processed inputs.
    /// </summary>
    public static class Memoizer
    {
        #region Public Static Methods

        /// <summary>
        /// http://codingly.com/2008/05/02/optimisation-des-invocations-dynamiques-de-methodes-en-c/
        /// </summary>
        public static Func<TSource, TResult> Memoize<TSource, TResult>(
            this Func<TSource, TResult> fn)
        {
            ConcurrentDictionary<TSource, TResult> cache =
                new ConcurrentDictionary<TSource, TResult>();  
            TResult nullCache = default(TResult);
            bool isNullCacheSet = false;

            return parameter =>
            {
                if (parameter == null && isNullCacheSet)
                {
                    return nullCache;
                }

                if (parameter == null)
                {
                    nullCache = fn(parameter);
                    isNullCacheSet = true;

                    return nullCache;
                }
                
                return cache.GetOrAdd(parameter, fn(parameter));
            };
        }

        #endregion
    }
}