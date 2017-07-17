using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Provides a set of static methods on sequences.
    /// </summary>
    /// <remarks>A sequence is any data structure that implements the IEnumerable interface.</remarks>
    public static class Seq
    {
        #region Fields

        /// <summary>
        /// String representation of comma followed by space.
        /// </summary>
        private const string COMMA = ", ";

        /// <summary>
        /// String representation of period.
        /// </summary>
        private const string PERIOD = ".";

        /// <summary>
        /// Session Id.
        /// </summary>
        private static readonly string SESSION_ID = GenerateSessionId();

        #endregion

        #region Public Methods

        /// <summary>
        /// Extension method variant of Append.
        /// </summary>
        /// <typeparam name="TSource">Generic data type in an IEnumerable object.</typeparam>
        /// <param name="source">Generic IEnumerable object that is being extended.</param>
        /// <param name="list">Collection of items to be appended to an existing collection (source).</param>
        /// <returns>IEnumerable collection of type TSource.</returns>
        public static IEnumerable<TSource> Append<TSource>(
             this IEnumerable<TSource> source,
             params IEnumerable<TSource>[] list)
        {
            IEnumerable<TSource> combo = Enumerable
                 .Empty<TSource>()
                 .Concat(source);

            foreach (IEnumerable<TSource> seq in list)
            {
                combo = combo.Concat(seq);
            }

            return combo;
        }

        /// <summary>
        /// Appends element to source.
        /// </summary>
        /// <typeparam name="TSource">Generic data type in an IEnumerable object.</typeparam>
        /// <param name="source">Generic IEnumerable object that is being extended.</param>
        /// <param name="element">Item of type TSource to be appended to an existing collection (source).</param>
        /// <returns></returns>
        public static IEnumerable<TSource> Append<TSource>(
             this IEnumerable<TSource> source,
             TSource element)
            => source.Concat(List(element));

        /// <summary>
        /// Appends a formatted string to source.
        /// </summary>
        /// <remarks>See StringBuilder.AppendFormat.</remarks>
        /// <param name="source">Generic IEnumerable object that is being extended.</param>
        /// <param name="format">
        /// Replaces the format item in a specified string with the string representation
        //  of a corresponding object in a specified array.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns></returns>
        public static IEnumerable<string> AppendFormat(
             this IEnumerable<string> source,
             string format,
             params object[] args)
            => source.Append(string.Format(format, args));

        /// <summary>
        /// Applies fn onto each element in source.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of source.</typeparam>
        /// <param name="source">Generic IEnumerable object that is being extended.</param>
        /// <param name="fn">A method that accepts and object of type TSource. (Does not return a value).</param>
        public static void Apply<TSource>(
             this IEnumerable<TSource> source,
             Action<TSource> fn)
        {
            foreach (TSource element in source)
            {
                fn(element);
            }
        }

        /// <summary>
        /// Applies fn onto corresponding elements of first and second sequences.
        /// </summary>
        /// <remarks>Advancement is halted once end of collection is reached on one of the sequences.</remarks>
        /// <typeparam name="TFirst">Generic IEnumerable object of type TFirst.</typeparam>
        /// <typeparam name="TSecond">Generic IEnumerable object of type TSecond.</typeparam>
        /// <param name="first">An IEnumerable instance of TFirst.</param>
        /// <param name="second">An IEnumerable instance of TSecond.</param>
        /// <param name="fn">A method that accepts TFirst and TSecond as parameters. (Does not return a value).</param>
        public static void Apply<TFirst, TSecond>(
             IEnumerable<TFirst> first,
             IEnumerable<TSecond> second,
             Action<TFirst, TSecond> fn)
        {
            using (MultiEnumerator<TFirst, TSecond> me =
                      MultiEnumerator.Create(first, second))
            {
                while (me.MoveNext())
                {
                    Tuple<TFirst, TSecond> t = me.Current;

                    fn(t.Item1, t.Item2);
                }
            }
        }

        /// <summary>
        /// Applies fn onto corresponding elements of first, second and third sequences.
        /// </summary>
        /// <typeparam name="TFirst">Generic IEnumerable object of type TFirst.</typeparam>
        /// <typeparam name="TSecond">Generic IEnumerable object of type TSecond.</typeparam>
        /// <typeparam name="TThird">Generic IEnumerable object of type TThird.</typeparam>
        /// <param name="first">An IEnumerable instance of TFirst.</param>
        /// <param name="second">An IEnumerable instance of TSecond.</param>
        /// <param name="third">An IEnumerable instance of TThird.</param>
        /// <param name="fn">A method that accepts TFirst, TSecond and TThird as parameters. (Does not return a value).</param>
        public static void Apply<TFirst, TSecond, TThird>(
             IEnumerable<TFirst> first,
             IEnumerable<TSecond> second,
             IEnumerable<TThird> third,
             Action<TFirst, TSecond, TThird> fn)
        {
            using (MultiEnumerator<TFirst, TSecond, TThird> me =
                      MultiEnumerator.Create(first, second, third))
            {
                while (me.MoveNext())
                {
                    Tuple<TFirst, TSecond, TThird> t = me.Current;

                    fn(t.Item1, t.Item2, t.Item3);
                }
            }
        }

        /// <summary>
        /// Applies fn onto corresponding elements of first, second, third and fourth sequences.
        /// </summary>
        /// <typeparam name="TFirst">Generic IEnumerable object of type TFirst.</typeparam>
        /// <typeparam name="TSecond">Generic IEnumerable object of type TSecond.</typeparam>
        /// <typeparam name="TThird">Generic IEnumerable object of type TThird.</typeparam>
        /// <typeparam name="TFourth">Generic IEnumerable object of type TFourth.</typeparam>
        /// <param name="first">An IEnumerable instance of TFirst.</param>
        /// <param name="second">An IEnumerable instance of TSecond.</param>
        /// <param name="third">An IEnumerable instance of TThird.</param>
        /// <param name="fourth">An IEnumerable instance of TFourth.</param>
        /// <param name="fn">A method that accepts TFirst, TSecond, TThird and TFourth as parameters. (Does not return a value).</param>
        public static void Apply<TFirst, TSecond, TThird, TFourth>(
             IEnumerable<TFirst> first,
             IEnumerable<TSecond> second,
             IEnumerable<TThird> third,
             IEnumerable<TFourth> fourth,
             Action<TFirst, TSecond, TThird, TFourth> fn)
        {
            using (MultiEnumerator<TFirst, TSecond, TThird, TFourth> me =
                      MultiEnumerator.Create(first, second, third, fourth))
            {
                while (me.MoveNext())
                {
                    Tuple<TFirst, TSecond, TThird, TFourth> t = me.Current;

                    fn(t.Item1, t.Item2, t.Item3, t.Item4);
                }
            }
        }

        /// <summary>
        /// Projects each element of a sequence to an IEnumerable of type TResult and flattens the resulting sequences into one sequence.
        /// </summary>
        /// <remarks>Equivalent to C# Enumerable Class SelectMany extension method.</remarks>
        /// <typeparam name="TSource">Generic IEnumerable object that is being extended.</typeparam>
        /// <typeparam name="TResult">Generic IEnumerable object that is expected as the return value.</typeparam>
        /// <param name="source">An IEnumerable instance of TSource.</param>
        /// <param name="fn">A method that accepts TSource as parameter and return an IEnumerable object of type TResult.</param>
        /// <returns>Lazy sequence.</returns>
        public static IEnumerable<TResult> FlatMap<TSource, TResult>(
             this IEnumerable<TSource> source,
             Func<TSource, IEnumerable<TResult>> fn)
        {
            foreach (TSource element in source)
            {
                IEnumerable<TResult> seq = fn(element);

                foreach (TResult result in seq)
                {
                    yield return result;
                }
            }
        }
        /// <summary>
        ///  Projects each element of a sequence to an IEnumerable<TResult> and flattens the resulting sequences into one sequence.
        /// </summary>
        /// <typeparam name="TFirst">Generic IEnumerable object of type TFirst.</typeparam>
        /// <typeparam name="TSecond">Generic IEnumerable object of type TSecond.</typeparam>
        /// <typeparam name="TResult">Generic IEnumerable object that is expected as the return value.</typeparam>
        /// <param name="first">An IEnumerable instance of TFirst.</param>
        /// <param name="second">An IEnumerable instance of TSecond.</param>
        /// <param name="fn">A method that accepts TFirst, TSecond as parameters and return an IEnumerable object of type TResult.</param>
        /// <returns>IEnumerable of type TResult.</returns>
        public static IEnumerable<TResult> FlatMap<TFirst, TSecond, TResult>(
             IEnumerable<TFirst> first,
             IEnumerable<TSecond> second,
             Func<TFirst, TSecond, IEnumerable<TResult>> fn)
        {
            using (MultiEnumerator<TFirst, TSecond> me =
                      MultiEnumerator.Create(first, second))
            {
                while (me.MoveNext())
                {
                    Tuple<TFirst, TSecond> t = me.Current;
                    IEnumerable<TResult> seq = fn(t.Item1, t.Item2);

                    foreach (TResult result in seq)
                    {
                        yield return result;
                    }
                }
            }
        }
        /// <summary>
        /// Projects each element of a sequence to an IEnumerable<TResult> and flattens the resulting sequences into one sequence.
        /// </summary>
        /// <typeparam name="TFirst">Generic IEnumerable object of type TFirst.</typeparam>
        /// <typeparam name="TSecond">Generic IEnumerable object of type TSecond.</typeparam>
        /// <typeparam name="TThird">Generic IEnumerable object of type TThird.</typeparam>
        /// <typeparam name="TResult">Generic IEnumerable object that is expected as the return value.</typeparam>
        /// <param name="first">An IEnumerable instance of TFirst.</param>
        /// <param name="second">An IEnumerable instance of TSecond.</param>
        /// <param name="third">An IEnumerable instance of TThird.</param>
        /// <param name="fn">A method that accepts TFirst, TSecond and TThird as parameters and return an IEnumerable object of type TResult.</param>
        /// <returns>IEnumerable of type TResult.</returns>
        public static IEnumerable<TResult> FlatMap<TFirst, TSecond, TThird, TResult>(
             IEnumerable<TFirst> first,
             IEnumerable<TSecond> second,
             IEnumerable<TThird> third,
             Func<TFirst, TSecond, TThird, IEnumerable<TResult>> fn)
        {
            using (MultiEnumerator<TFirst, TSecond, TThird> me =
                      MultiEnumerator.Create(first, second, third))
            {
                while (me.MoveNext())
                {
                    Tuple<TFirst, TSecond, TThird> t = me.Current;
                    IEnumerable<TResult> seq = fn(t.Item1, t.Item2, t.Item3);

                    foreach (TResult result in seq)
                    {
                        yield return result;
                    }
                }
            }
        }
        /// <summary>
        /// Projects each element of a sequence to an IEnumerable<TResult> and flattens the resulting sequences into one sequence.
        /// </summary>
        /// <typeparam name="TFirst">Generic IEnumerable object of type TFirst.</typeparam>
        /// <typeparam name="TSecond">Generic IEnumerable object of type TSecond.</typeparam>
        /// <typeparam name="TThird">Generic IEnumerable object of type TThird.</typeparam>
        /// <typeparam name="TFourth">Generic IEnumerable object of type TFourth.</typeparam>
        /// <typeparam name="TResult">Generic IEnumerable object that is expected as the return value.</typeparam>
        /// <param name="first">An IEnumerable instance of TFirst.</param>
        /// <param name="second">An IEnumerable instance of TSecond.</param>
        /// <param name="third">An IEnumerable instance of TThird.</param>
        /// <param name="fourth">An IEnumerable instance of TFourth.</param>
        /// <param name="fn">A method that accepts TFirst, TSecond, TThird and TFourth as parameters and return an IEnumerable object of type TResult.</param>
        /// <returns>IEnumerable of type TResult.</returns>
        public static IEnumerable<TResult> FlatMap<TFirst, TSecond, TThird, TFourth, TResult>(
             IEnumerable<TFirst> first,
             IEnumerable<TSecond> second,
             IEnumerable<TThird> third,
             IEnumerable<TFourth> fourth,
             Func<TFirst, TSecond, TThird, TFourth, IEnumerable<TResult>> fn)
        {
            using (MultiEnumerator<TFirst, TSecond, TThird, TFourth> me =
                      MultiEnumerator.Create(first, second, third, fourth))
            {
                while (me.MoveNext())
                {
                    Tuple<TFirst, TSecond, TThird, TFourth> t = me.Current;
                    IEnumerable<TResult> seq = fn(t.Item1, t.Item2, t.Item3, t.Item4);

                    foreach (TResult result in seq)
                    {
                        yield return result;
                    }
                }
            }
        }

        /// <summary>
        /// Concatenates passed in sequences into a single sequence.
        /// </summary>
        /// <remarks>Flattens all sequences into a single sequence.</remarks>
        /// <typeparam name="TSource">Generic object to be used as the type constraint in the parameter.</typeparam>
        /// <param name="list">An array of IEnumerable object of type of TSource.</param>
        /// <returns>Returns a combo sequence of all passed in sequences concatenated.</returns>
        public static IEnumerable<TSource> Flatten<TSource>(
             params IEnumerable<TSource>[] list)
        {
            IEnumerable<TSource> combo = Enumerable.Empty<TSource>();

            foreach (IEnumerable<TSource> seq in list)
            {
                combo = combo.Concat(seq);
            }

            return combo;
        }

        /// <summary>
        /// Checks if sequence is empty.
        /// </summary>
        /// <typeparam name="TSource">Generic object that is the target of this extension.</typeparam>
        /// <param name="source">Instance of IEnumerable object of type TSource.</param>
        /// <returns>True if empty otherwise False.</returns>
        public static bool IsEmpty<TSource>(IEnumerable<TSource> source)
        {
            return !source.Any();
        }

        /// <summary>
        /// Checks if sequence is null or empty.
        /// </summary>
        /// <returns>True if source is null or empty otherwise False.</returns>
        public static bool IsNullOrEmpty<TSource>(IEnumerable<TSource> source)
        {
            return (source == null || IsEmpty(source));
        }

        /// <summary>
        /// Determines whether subset sequence is a subset of superset sequence.
        /// </summary>
        /// <typeparam name="TSource">Generic type to be used as type constraint.</typeparam>
        /// <param name="subset">An instance of IEnumerable of type TSource.</param>
        /// <param name="superset">An instance of IEnumerable of type TSource.</param>
        /// <returns>True if subject sequence is a subset of the superset sequences otherwise False.</returns>
        public static bool IsSubsetOf<TSource>(
             IEnumerable<TSource> subset,
             IEnumerable<TSource> superset)
        {
            return ColEx.CreateSet(subset).IsSubsetOf(superset);
        }

        /// <summary>
        /// Concatenates all the elements of source using the specified separator between each element. 
        /// </summary>
        /// <typeparam name="TSource">Generic type of the target object for this extension.</typeparam>
        /// <param name="source">An instance of IEnumerable object of type TSource.</param>
        /// <param name="separator">String delimiter.</param>
        /// <returns>A string that consists of the elements in source delimited by separator.</returns>
        public static string Join<TSource>(
             this IEnumerable<TSource> source,
             string separator)
        {
            return string.Join(separator, source);
        }

        /// <summary>
        /// Concatenates all elements in the source using NewLine as string separator. 
        /// </summary>
        /// <typeparam name="TSource">Generic type of the target object for this extension.</typeparam>
        /// <param name="source">An instance of IEnumerable object of type TSource.</param>
        /// <returns>A string that consists of the elements in source delimited by new line.</returns>
        public static string JoinWithNewLine<TSource>(
             this IEnumerable<TSource> source)
        {
            return source.Join(Environment.NewLine);
        }

        /// <summary>
        /// Concatenates all elements in the source using specified conjunction term ex. And, Or etc.
        /// </summary>
        /// <typeparam name="TSource">Generic type of the target object for this extension.</typeparam>
        /// <param name="source">An instance of IEnumerable object of type TSource.</param>
        /// <param name="conjunction">The word or term to be used as conjunction.</param>
        /// <returns>A string that consists of the elements in source delimited by comma and conjunction at the end.</returns>
        public static string JoinWithConjunction<TSource>(
             this IEnumerable<TSource> source,
             string conjunction)
        {
            // Probably overkill but the idea is to distinguish commas that are part of the sequence and commas to be added as separators.
            string seperator = string.Format("{0}{1}{0}", SESSION_ID, COMMA);
            StringBuilder sentence = StringEx.CreateSB(source.Join(seperator));

            // Replace the last occurrence of comma added with conjunction.
            if (sentence.ToString().Contains(seperator))
            {
                sentence.Replace(
                     seperator,
                     $" {conjunction.Trim()} ",
                     sentence.ToString().LastIndexOf(seperator),
                     seperator.Length);
            }

            // Add a period at the end of sentence.
            if (!sentence.ToString().EndsWith(PERIOD))
            {
                sentence.Append(PERIOD);
            }

            // Now that the joining is complete, remove all occurrences of Session Id.
            return sentence.ToString().Replace(SESSION_ID, string.Empty);
        }

        /// <summary>
        /// Concatenates all elements using "and" as the conjunction term.
        /// </summary>
        /// <typeparam name="TSource">Generic type of the target object for this extension.</typeparam>
        /// <param name="source">An instance of IEnumerable object of type TSource.</param><returns>IEnumerable object of type TSource</returns>
        /// <returns>A string that consists of the elements in source delimited by comma and and at the end.</returns>
        public static string JoinWithAnd<TSource>(
             this IEnumerable<TSource> source)
            => source.JoinWithConjunction("and");

        /// <summary>
        /// Concatenates all elements using "or" as the conjunction term.
        /// </summary>
        /// <typeparam name="TSource">Generic type of the target object for this extension.</typeparam>
        /// <param name="source">An instance of IEnumerable object of type TSource.</param>
        /// <returns>A string that consists of the elements in source delimited by comma and or at the end.</returns>
        public static string JoinWithOr<TSource>(
             this IEnumerable<TSource> source)
            => source.JoinWithConjunction("or");

        /// <summary>
        /// Returns the input parameters as a sequence.
        /// </summary>
        /// <typeparam name="TSource">Generic type to be used as type of an IEnumerable object.</typeparam>
        /// <param name="list">An array of type TSource.</param>
        /// <returns>IEnumerable object of type TSource</returns>
        public static IEnumerable<TSource> List<TSource>(
             params TSource[] list)
        {
            foreach (TSource element in list)
            {
                yield return element;
            }
        }

        /// <summary>
        /// Projects each element from source sequence into result sequence by applying fn.
        /// </summary>
        /// <remarks>Equivalent to C# Enumerable Class Select extension method.</remarks>
        /// <returns>Lazy sequence.</returns>
        /// <typeparam name="TSource">Generic type of the target object of this extension.</typeparam>
        /// <typeparam name="TResult">>Generic type of the return value.</typeparam>
        /// <param name="source">An IEnumerable instance of type TSource.</param>
        /// <param name="fn">A method that accepts TSource as parameter and return an IEnumerable object of type TResult.</param>
        /// <returns>IEnumerable of type TResult.</returns>
        public static IEnumerable<TResult> Map<TSource, TResult>(
             this IEnumerable<TSource> source,
             Func<TSource, TResult> fn)
        {
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    TSource c = e.Current;

                    yield return fn(c);
                }
            }
        }

        /// <summary>
        /// Applies fn to the corresponding elements from first and second sequences, producing results sequence.
        /// </summary>
        /// <remarks>Equivalent to C# Enumerable Class Zip extension method.</remarks>
        /// <returns>Lazy sequence.</returns>
        /// <typeparam name="TFirst">Generic IEnumerable object of type TFirst.</typeparam>
        /// <typeparam name="TSecond">Generic IEnumerable object of type TSecond.</typeparam>
        /// <typeparam name="TResult">Generic IEnumerable object of type TResult.</typeparam>
        /// <param name="first">An IEnumerable instance of TFirst.</param>
        /// <param name="second">An IEnumerable instance of TSecond.</param>
        /// <param name="fn"></param>
        /// <param name="fn">A method that accepts TFirst and TSecond as parameters and return an IEnumerable object of type TResult.</param>
        /// <returns>IEnumerable of type TResult.</returns>
        public static IEnumerable<TResult> Map<TFirst, TSecond, TResult>(
             IEnumerable<TFirst> first,
             IEnumerable<TSecond> second,
             Func<TFirst, TSecond, TResult> fn)
        {
            using (MultiEnumerator<TFirst, TSecond> me =
                      MultiEnumerator.Create(first, second))
            {
                while (me.MoveNext())
                {
                    Tuple<TFirst, TSecond> t = me.Current;

                    yield return fn(t.Item1, t.Item2);
                }
            }
        }

        /// <summary>
        /// Applies fn to the corresponding elements from first, second and third sequences, producing results sequence.
        /// </summary>
        /// <returns>Lazy sequence.</returns>
        /// <typeparam name="TFirst">Generic IEnumerable object of type TFirst.</typeparam>
        /// <typeparam name="TSecond">Generic IEnumerable object of type TSecond.</typeparam>
        /// <typeparam name="TThird">Generic IEnumerable object of type TThird.</typeparam>
        /// <typeparam name="TResult">Generic IEnumerable object of type TResult.</typeparam>
        /// <param name="first">An IEnumerable instance of TFirst.</param>
        /// <param name="second">An IEnumerable instance of TSecond.</param>
        /// <param name="third">An IEnumerable instance of TThird.</param>
        /// <param name="fn">A method that accepts TFirst, TSecond and TThird as parameters and return an IEnumerable object of type TResult.</param>
        /// <returns>IEnumerable of type TResult.</returns>
        public static IEnumerable<TResult> Map<TFirst, TSecond, TThird, TResult>(
             IEnumerable<TFirst> first,
             IEnumerable<TSecond> second,
             IEnumerable<TThird> third,
             Func<TFirst, TSecond, TThird, TResult> fn)
        {
            using (MultiEnumerator<TFirst, TSecond, TThird> me =
                      MultiEnumerator.Create(first, second, third))
            {
                while (me.MoveNext())
                {
                    Tuple<TFirst, TSecond, TThird> t = me.Current;

                    yield return fn(t.Item1, t.Item2, t.Item3);
                }
            }
        }

        /// <summary>
        /// Applies fn to the corresponding elements from first, second, third and fourth sequences, producing results sequence.
        /// </summary>
        /// <returns>Lazy sequence.</returns>
        /// <typeparam name="TFirst">Generic IEnumerable object of type TFirst.</typeparam>
        /// <typeparam name="TSecond">Generic IEnumerable object of type TSecond.</typeparam>
        /// <typeparam name="TThird">Generic IEnumerable object of type TThird.</typeparam>
        /// <typeparam name="TFourth">Generic IEnumerable object of type TFourth.</typeparam>
        /// <typeparam name="TResult">Generic IEnumerable object of type TResult.</typeparam>
        /// <param name="first">An IEnumerable instance of TFirst.</param>
        /// <param name="second">An IEnumerable instance of TSecond.</param>
        /// <param name="third">An IEnumerable instance of TThird.</param>
        /// <param name="fourth">An IEnumerable instance of TFourth.</param>
        /// <param name="fn">A method that accepts TFirst, TSecond, TThird and TFourth as parameters and return an IEnumerable object of type TResult.</param>
        /// <returns>IEnumerable of type TResult.</returns>
        public static IEnumerable<TResult> Map<TFirst, TSecond, TThird, TFourth, TResult>(
             IEnumerable<TFirst> first,
             IEnumerable<TSecond> second,
             IEnumerable<TThird> third,
             IEnumerable<TFourth> fourth,
             Func<TFirst, TSecond, TThird, TFourth, TResult> fn)
        {
            using (MultiEnumerator<TFirst, TSecond, TThird, TFourth> me =
                      MultiEnumerator.Create(first, second, third, fourth))
            {
                while (me.MoveNext())
                {
                    Tuple<TFirst, TSecond, TThird, TFourth> t = me.Current;

                    yield return fn(t.Item1, t.Item2, t.Item3, t.Item4);
                }
            }
        }

        /// <summary>
        /// Prepends element to source.
        /// </summary>
        public static IEnumerable<TSource> Prepend<TSource>(
             this IEnumerable<TSource> source,
             TSource element)
            => List(element).Concat(source);

        /// <summary>
        /// Returns a random reference into source.
        /// </summary>
        public static TSource RandomRef<TSource>(
             this IEnumerable<TSource> source,
             Random random)
            => source.ElementAt(random.Next(0, source.Count()));

        /// <summary>
        /// Generates a lazy sequence resulting from calling fn count times.
        /// </summary>
        public static IEnumerable<TResult> Repeat<TResult>(
             Func<TResult> fn,
             int count)
            => Enumerable.Repeat(true, count).Map(t => fn());

        public static IEnumerable<TResult> Repeat<TResult>(
             Func<int, TResult> fn,
             int count)
            => Enumerable.Range(0, count).Map(i => fn(i));

        /// <summary>
        /// Returns true if first and second contain the same elements irrespective of order.
        /// </summary>
        public static bool SequenceEquivalent<TSource>(
             this IEnumerable<TSource> first,
             IEnumerable<TSource> second)
        {
            if (first != null)
            {
                return (second != null && first.Count() == second.Count())
                     ? ColEx.CreateSet(first).SetEquals(second)
                     : false;
            }
            else
            {
                return (second == null);
            }
        }

        /// <summary>
        /// Returns tail of two sequences.
        /// </summary>
        /// <typeparam name="TSource">Source generic type.</typeparam>
        /// <param name="first">First sequence.</param>
        /// <param name="second">Second sequence.</param>
        /// <returns>Tail.</returns>
        public static IEnumerable<TSource> GetTail<TSource>(
            IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            if (List(first, second).All(seq => seq != null))
            {
                int firstCount = first.Count();
                int secondCount = second.Count();

                if (firstCount == secondCount)
                {
                    return Enumerable.Empty<TSource>();
                }
                else if (firstCount > secondCount)
                {
                    return first.Skip(secondCount);
                }
                else
                {
                    return second.Skip(firstCount);
                }
            }
            else
            {
                return first ?? second;
            }
        }

        /// <summary>
        /// Returns distinct elements from a sequence by using specified IEqualityComparer<TSource> to compare values. 
        /// </summary>
        /// <typeparam name="TSource">Source generic type.</typeparam>
        /// <param name="source">Sequence.</param>
        /// <param name="comparer">Equality comparer object.</param>
        /// <exception cref="ArgumentNullException">Source is null.</exception>
        /// <remarks>This method retains order of elements as it performs Distinct operation.</remarks>
        /// <returns>Distinct sequence with order of elements retained.</returns>
        public static IEnumerable<TSource> DistinctRetainOrder<TSource>(
            this IEnumerable<TSource> source,
            IEqualityComparer<TSource> comparer)
        {
            if (source != null)
            {
                ISet<TSource> set = ColEx.CreateSet(comparer: comparer);

                foreach (TSource elem in source)
                {
                    if (set.Add(elem))
                    {
                        yield return elem;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(source));
            }
        }

        /// <summary>
        /// Performs pairwise merge on two sequences .
        /// </summary>
        /// <typeparam name="TSource">Source generic type.</typeparam>
        /// <param name="first">First sequence.</param>
        /// <param name="second">Second sequence.</param>
        /// <returns>Merged sequence</returns>
        public static IEnumerable<TSource> PairwiseMerge<TSource>(
            IEnumerable<TSource> first,
            IEnumerable<TSource> second)
        {
            return (List(first, second).All(seq => seq != null))
                ? Map(first, second, Tuple.Create)
                    .FlatMap(t => List(t.Item1, t.Item2))
                    .Append(GetTail(first, second))
                : first ?? second;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates random/unique session Id.
        /// </summary>
        /// <remarks>Probably overkill.</remarks>
        /// <returns>Session Id.</returns>
        private static string GenerateSessionId()
        {
            string randomString = RandomEx
                 .Create()
                 .NextString(4, 8, StringComposition.Letter);
            long ticks = DateTime.UtcNow.Ticks;
            string guid = Guid.NewGuid().ToUpper();

            return $"[{randomString}{ticks}{guid}]";
        }

        #endregion
    }
}
