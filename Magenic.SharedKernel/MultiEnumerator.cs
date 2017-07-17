using System;
using System.Collections;
using System.Collections.Generic;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Class for creating a single enumerator for two objects in one collection class.
    /// </summary>
    /// <typeparam name="TFirst">Generic data type of the first object in the collection.</typeparam>
    /// <typeparam name="TSecond">Generic data type of the second object in the collection.</typeparam>
    public class MultiEnumerator<TFirst, TSecond> : IEnumerator<Tuple<TFirst, TSecond>>
    {
        #region Fields

        private readonly IEnumerator<TFirst> _e1;
        private readonly IEnumerator<TSecond> _e2;

        #endregion

        public MultiEnumerator(IEnumerator<TFirst> e1, IEnumerator<TSecond> e2)
        {
            _e1 = e1;
            _e2 = e2;
        }

        #region Creation

        public MultiEnumerator(IEnumerable<TFirst> e1, IEnumerable<TSecond> e2)
            : this(e1.GetEnumerator(), e2.GetEnumerator())
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the two items in the current iteration.
        /// </summary>
        public Tuple<TFirst, TSecond> Current
        {
            get
            {
                TFirst v1 = _e1.Current;
                TSecond v2 = _e2.Current;
                Tuple<TFirst, TSecond> t = Tuple.Create(v1, v2);

                return t;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Dispose the collection of all of the generic items.
        /// </summary>
        public void Dispose()
        {
            _e1.Dispose();
            _e2.Dispose();
        }

        /// <summary>
        /// Leaves the current iteration and reads the next two items in the collection.
        /// </summary>
        /// <returns>True if the iteration has not reached the last item in the collection.</returns>
        public bool MoveNext()
        {
            return (_e1.MoveNext() && _e2.MoveNext());
        }

        /// <summary>
        /// Sets the enumerator of each of the object in the collection to its initial position. That is before the first element of each individual collection.
        /// </summary>
        public void Reset()
        {
            _e1.Reset();
            _e2.Reset();
        }

        #endregion
    }

    /// <summary>
    /// Class for creating a single enumerator for three objects in one collection class.
    /// </summary>
    /// <typeparam name="TFirst">Generic data type of the first object in the collection.</typeparam>
    /// <typeparam name="TSecond">Generic data type of the second object in the collection.</typeparam>
    /// <typeparam name="TThird">Generic data type of the third object in the collection.</typeparam>
    public class MultiEnumerator<TFirst, TSecond, TThird> : IEnumerator<Tuple<TFirst, TSecond, TThird>>
    {
        #region Fields

        private readonly IEnumerator<TFirst> _e1;
        private readonly IEnumerator<TSecond> _e2;
        private readonly IEnumerator<TThird> _e3;

        #endregion

        #region Creation

        public MultiEnumerator(
            IEnumerator<TFirst> e1,
            IEnumerator<TSecond> e2,
            IEnumerator<TThird> e3)
        {
            _e1 = e1;
            _e2 = e2;
            _e3 = e3;
        }

        public MultiEnumerator(
            IEnumerable<TFirst> e1,
            IEnumerable<TSecond> e2,
            IEnumerable<TThird> e3)
            : this(e1.GetEnumerator(), e2.GetEnumerator(), e3.GetEnumerator())
        {
        }

        #endregion

        #region Properties

        public Tuple<TFirst, TSecond, TThird> Current
        {
            get
            {
                TFirst v1 = _e1.Current;
                TSecond v2 = _e2.Current;
                TThird v3 = _e3.Current;
                Tuple<TFirst, TSecond, TThird> t = Tuple.Create(v1, v2, v3);

                return t;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Dispose the collection of all of the generic items.
        /// </summary>
        public void Dispose()
        {
            _e1.Dispose();
            _e2.Dispose();
            _e3.Dispose();
        }

        /// <summary>
        /// Leaves the current iteration and reads the next three items in the collection.
        /// </summary>
        /// <returns>True if the iteration has not reached the last item in the collection.</returns>
        public bool MoveNext()
        {
            return (_e1.MoveNext() && _e2.MoveNext() && _e3.MoveNext());
        }

        /// <summary>
        /// Sets the enumerator of each of the object in the collection to its initial position. That is before the first element of each individual collection.
        /// </summary>
        public void Reset()
        {
            _e1.Reset();
            _e2.Reset();
            _e3.Reset();
        }

        #endregion
    }

    /// <summary>
    /// Class for creating a single enumerator for four objects in one collection class.
    /// </summary>
    /// <typeparam name="TFirst">Generic data type of the first object in the collection.</typeparam>
    /// <typeparam name="TSecond">Generic data type of the second object in the collection.</typeparam>
    /// <typeparam name="TThird">Generic data type of the third object in the collection.</typeparam>
    /// <typeparam name="TFourth">Generic data type of the fourth object in the collection.</typeparam>
    public class MultiEnumerator<TFirst, TSecond, TThird, TFourth> : IEnumerator<Tuple<TFirst, TSecond, TThird, TFourth>>
    {
        #region Fields

        private readonly IEnumerator<TFirst> _e1;
        private readonly IEnumerator<TSecond> _e2;
        private readonly IEnumerator<TThird> _e3;
        private readonly IEnumerator<TFourth> _e4;

        #endregion

        #region Creation

        public MultiEnumerator(
            IEnumerator<TFirst> e1,
            IEnumerator<TSecond> e2,
            IEnumerator<TThird> e3,
            IEnumerator<TFourth> e4)
        {
            _e1 = e1;
            _e2 = e2;
            _e3 = e3;
            _e4 = e4;
        }

        public MultiEnumerator(
            IEnumerable<TFirst> e1,
            IEnumerable<TSecond> e2,
            IEnumerable<TThird> e3,
            IEnumerable<TFourth> e4)
            : this(
                  e1.GetEnumerator(),
                  e2.GetEnumerator(),
                  e3.GetEnumerator(),
                  e4.GetEnumerator())
        {
        }

        #endregion

        #region Properties

        public Tuple<TFirst, TSecond, TThird, TFourth> Current
        {
            get
            {
                TFirst v1 = _e1.Current;
                TSecond v2 = _e2.Current;
                TThird v3 = _e3.Current;
                TFourth v4 = _e4.Current;
                Tuple<TFirst, TSecond, TThird, TFourth> t = Tuple.Create(v1, v2, v3, v4);

                return t;
            }
        }

        /// <summary>
        /// Gets the three items in the current iteration.
        /// </summary>
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Dispose the collection of all of the generic items.
        /// </summary>
        public void Dispose()
        {
            _e1.Dispose();
            _e2.Dispose();
            _e3.Dispose();
            _e4.Dispose();
        }

        /// <summary>
        /// Leaves the current iteration and reads the next four items in the collection.
        /// </summary>
        /// <returns>True if the iteration has not reached the last item in the collection.</returns>
        public bool MoveNext()
        {
            return (_e1.MoveNext() && _e2.MoveNext() && _e3.MoveNext() && _e4.MoveNext());
        }

        /// <summary>
        /// Sets the enumerator of each of the object in the collection to its initial position. That is before the first element of each individual collection.
        /// </summary>
        public void Reset()
        {
            _e1.Reset();
            _e2.Reset();
            _e3.Reset();
            _e4.Reset();
        }

        #endregion
    }

    /// <summary>
    /// Helper class for creating multi enumerators.
    /// </summary>
    public static class MultiEnumerator
    {
        #region Creation

        /// <summary>
        /// Concrete implementation of MultiEnumerator collection with two generic objects.
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the first object in the collection.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the second object in the collection.</typeparam>
        /// <param name="e1">Enumerable object of type TFirst.</param>
        /// <param name="e2">Enumerable object of type TSecond.</param>
        /// <returns>An instance of MultiEnumerator with two Generic enumerable objects.</returns>
        public static MultiEnumerator<TFirst, TSecond> Create<TFirst, TSecond>(
             IEnumerable<TFirst> e1,
             IEnumerable<TSecond> e2)
        {
            return new MultiEnumerator<TFirst, TSecond>(e1, e2);
        }

        /// <summary>
        /// Concrete implementation of MultiEnumerator collection with three generic objects.
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the first object in the collection.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the second object in the collection.</typeparam>
        /// <typeparam name="TThird">Generic data type of the third object in the collection.</typeparam>
        /// <param name="e1">Enumerable object of type TFirst.</param>
        /// <param name="e2">Enumerable object of type TSecond.</param>
        /// <param name="e3">Enumerable object of type TThird.</param>
        /// <returns>An instance of MultiEnumerator with three Generic enumerable objects.</returns>
        public static MultiEnumerator<TFirst, TSecond, TThird> Create<TFirst, TSecond, TThird>(
             IEnumerable<TFirst> e1,
             IEnumerable<TSecond> e2,
             IEnumerable<TThird> e3)
        {
            return new MultiEnumerator<TFirst, TSecond, TThird>(e1, e2, e3);
        }

        /// <summary>
        /// Concrete implementation of MultiEnumerator collection with four generic objects.
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the first object in the collection.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the second object in the collection.</typeparam>
        /// <typeparam name="TThird">Generic data type of the third object in the collection.</typeparam>
        /// <typeparam name="TFourth">Generic data type of the fourth object in the collection.</typeparam>
        /// <param name="e1">Enumerable object of type TFirst.</param>
        /// <param name="e2">Enumerable object of type TSecond.</param>
        /// <param name="e3">Enumerable object of type TThird.</param>
        /// <param name="e4">Enumerable object of type TFourth.</param>
        /// <returns>An instance of MultiEnumerator with four Generic enumerable objects.</returns>
        public static MultiEnumerator<TFirst, TSecond, TThird, TFourth>
            Create<TFirst, TSecond, TThird, TFourth>(
                IEnumerable<TFirst> e1,
                IEnumerable<TSecond> e2,
                IEnumerable<TThird> e3,
                IEnumerable<TFourth> e4)
        {
            return new MultiEnumerator<TFirst, TSecond, TThird, TFourth>(e1, e2, e3, e4);
        }

        #endregion
    }
}