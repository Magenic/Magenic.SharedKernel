using System;
using System.Linq;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Utility class with general purpose helper methods.
    /// </summary>
    public static class Util
    {
        #region Public Methods

        /// <summary>
        /// Performed the specified action thunk for count times.
        /// </summary>
        /// <param name="thunk">Parameterless method with void return type.</param>
        /// <param name="count">Number of times to execute thunk.</param>
        public static void Repeat(Action thunk, int count)
        {
            // The boolean true is a don’t care.
            Enumerable.Repeat(true, count).Apply(t => thunk());
        }

        /// <summary>
        /// Repeats the specified action fn count times while passing the running count to each execution.
        /// </summary>
        /// <param name="fn">Method that takes an integer and is of void return type.</param>
        /// <param name="count">Number of times to execute fn.</param>
        public static void Repeat(Action<int> fn, int count)
        {
            Enumerable.Range(0, count).Apply(i => fn(i));
        }

        /// <summary>
        /// Disposes of source if it is not null.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of source that implements IDisposable.</typeparam>
        /// <param name="source">A disposable object.</param>
        public static void Dispose<TSource>(TSource source)
            where TSource : IDisposable
        {
            source?.Dispose();
        }

        /// <summary>
        /// Disposes of obj if it is not null and if it implements IDisposable.
        /// </summary>
        /// <param name="obj">Object.</param>
        public static void Dispose(object obj)
        {
            Dispose(obj.IfNotNull(o => o as IDisposable, () => null));
        }

        /// <summary>
        /// Disposes of source if it is not null and sets it to null after disposing.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of source that implements IDisposable.</typeparam>
        /// <param name="source">A disposable object.</param>
        public static void DisposeAndNull<TSource>(ref TSource source)
            where TSource : class, IDisposable
        {
            if (source != null)
            {
                source.Dispose();
                source = null;
            }
        }

        /// <summary>
        /// Disposes of obj if it is not null and if it implements IDisposable.  Then sets obj to null after disposing.
        /// </summary>
        /// <param name="obj">Object.</param>
        public static void DisposeAndNull(ref object obj)
        {
            if (obj != null)
            {
                Dispose(obj as IDisposable);

                obj = null;
            }
        }

        /// <summary>
        /// Executes fn on source if it is not null and then disposes of source.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of source that implements IDisposable.</typeparam>
        /// <param name="source">A disposable object.</param>
        /// <param name="fn">A function that takes a parameter of type TSource.</param>
        public static void Dispose<TSource>(
            TSource source,
            Action<TSource> fn)
            where TSource : IDisposable
        {
            if (source != null)
            {
                fn(source);

                source.Dispose();
            }
        }

        /// <summary>
        /// Executes fn on source if it is not null, captures result, disposes of source and returns result.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of source that implements IDisposable.</typeparam>
        /// <typeparam name="TResult">Generic data type of fn returns type.</typeparam>
        /// <param name="source">A disposable object.</param>
        /// <param name="fn">A function that takes a parameter of type TSource and returns TResult.</param>
        /// <returns>Result of executing fn.</returns>
        public static TResult Dispose<TSource, TResult>(
            TSource source,
            Func<TSource, TResult> fn)
            where TSource : IDisposable
        {
            if (source != null)
            {
                TResult ret = fn(source);

                source.Dispose();

                return ret;
            }
            else
            {
                return default(TResult);
            }
        }

        #endregion
    }
}