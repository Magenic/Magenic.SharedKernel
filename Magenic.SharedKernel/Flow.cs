using System;
using System.Collections.Generic;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Provides a set of static methods that facilitate program flow.
    /// </summary>
    public static class Flow
    {
        #region Public Static Methods

        /// <summary>
        /// If source is not null, apply consequent.  Otherwise, apply alternate.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of the target object of this extension.</typeparam>
        /// <param name="source">The object to be checked if Null or not.</param>
        /// <param name="consequent">A method that takes a single parameter without a return value.</param>
        /// <param name="alternate">A method with no parameters without a return value.</param>
        public static void IfNotNull<TSource>(
             this TSource source,
             Action<TSource> consequent,
             Action alternate)
        {
            if (source != null)
            {
                consequent(source);
            }
            else
            {
                alternate();
            }
        }

        /// <summary>
        /// An extension method that accepts a generic action to be executed if the object being extended is not null.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of the target object of this extension.</typeparam>
        /// <param name="source">Object being extended.</param>
        /// <param name="consequent">A method that takes a single parameter without a return value.</param>
        public static void IfNotNull<TSource>(
             this TSource source,
             Action<TSource> consequent) => source.IfNotNull(consequent, () => { });

        public static void IfNull<TSource>(
             this TSource source,
             Action consequent) => source.IfNotNull(a => { }, () => consequent());

        /// <summary>
        /// An extension method that accepts a predefined delegate type for a method that returns an object of type TResult.
        /// If the target object is Null then the alternative delegate type will be executed.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of the target object of this extension.</typeparam>
        /// <typeparam name="TResult">Generic data type of the result of this extension.</typeparam>
        /// <param name="source">Object being extended.</param>
        /// <param name="consequent">
        /// A generic predefined delegate type for a method that returns an object of type TSource.
        /// </param>
        /// <param name="alternate"> A predefined delegate type for a method that returns an object of type TSource.</param>
        /// <returns>
        /// Returns a modified TResult resulting for execution of consequent if the source is not Null.
        /// If the target object is Null then the alternative delegate type will be executed.
        /// </returns>
        public static TResult IfNotNull<TSource, TResult>(
             this TSource source,
             Func<TSource, TResult> consequent,
             Func<TResult> alternate) => (source != null)
                ? consequent(source)
                : alternate();

        /// <summary>
        /// If sequence is not null or empty, apply consequent.  Otherwise, apply alternate.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of the target object of this extension method.</typeparam>
        /// <param name="seq">A sequence comprised of TSource which will be checked if IsNullOrEmpty.</param>
        /// <param name="consequent">A method that takes a single parameter of type TSource without a return value.</param>
        /// <param name="alternate">A method with no parameters without a return value.</param>
        public static void IfNotNullOrEmpty<TSource>(
             this IEnumerable<TSource> seq,
             Action<TSource> consequent,
             Action alternate)
        {
            if (!Seq.IsNullOrEmpty(seq))
            {
                seq.Apply(consequent);
            }
            else
            {
                alternate();
            }
        }

        /// <summary>
        /// If sequence is not null or empty, apply consequent.  Otherwise, NoOp.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of the target object of this extension method.</typeparam>
        /// <param name="seq">A sequence comprised of TSource which will be checked if IsNullOrEmpty.</param>
        /// <param name="consequent">A method that takes a single parameter of type TSource without a return value.</param>
        public static void IfNotNullOrEmpty<TSource>(
             this IEnumerable<TSource> seq,
             Action<TSource> consequent) => seq.IfNotNullOrEmpty(consequent, () => { });

        /// <summary>
        /// Apply fn onto source and then return source.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of the target object of this extension.</typeparam>
        /// <param name="source">Object being extended.</param>
        /// <param name="fn">A method that has a single parameter.  (Does not return a value).</param>
        /// <returns>Returns a modified TResult based on fn definition.</returns>
        public static TSource WithSideEffects<TSource>(
             this TSource source,
             Action<TSource> fn)
        {
            fn(source);

            return source;
        }

        /// <summary>
        /// Apply fn onto source and then return result.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of the target object of this extension.</typeparam>
        /// <typeparam name="TResult">Generic data type of the expected result.</typeparam>
        /// <param name="source">Object being extended.</param>
        /// <param name="fn">
        /// A generic predefined delegate type for a method that returns an object of the type TResult.
        /// </param>
        /// <returns>Returns a TResult based on fn definition.</returns>
        public static TResult WithSideEffectsMap<TSource, TResult>(
             this TSource source,
             Func<TSource, TResult> fn) => fn(source);

        /// <summary>
        /// Put source in using statement to ensure proper disposal and apply fn.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of source.</typeparam>
        /// <param name="source">Target object to apply a (using) statement.</param>
        /// <param name="fn">A method that accepts the source as the parameter.  (Does not return a value).</param>
        public static void Using<TSource>(
             TSource source,
             Action<TSource> fn)
             where TSource : IDisposable
        {
            using (source)
            {
                fn(source);
            }
        }

        /// <summary>
        /// Put source in using statement to ensure proper disposal and apply fn to return the expected result.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of source.</typeparam>
        /// <typeparam name="TResult">Generic data type of result.</typeparam>
        /// <param name="source">Target object to apply a (using) statement.</param>
        /// <param name="fn">A method that accepts the source as the parameter.  (Does not return a value).</param>
        /// <returns>TResult after executing fn.</returns>
        public static TResult Using<TSource, TResult>(
             TSource source,
             Func<TSource, TResult> fn)
             where TSource : IDisposable
        {
            using (source)
            {
                return fn(source);
            }
        }

        /// <summary>
        /// Put the two generic object in using statement to ensure proper disposal and apply fn.
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the Object in the parent using statement.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the nested Object in the second using statement.</typeparam>
        /// <param name="first">Object in the parent using statement.</param>
        /// <param name="second">Object in the second using statement.</param>
        /// <param name="fn">A method that accepts TFirst and TSecond as the parameter.  (Does not return a value).</param>
        public static void Using<TFirst, TSecond>(
             TFirst first,
             TSecond second,
             Action<TFirst, TSecond> fn)
            where TFirst : IDisposable
            where TSecond : IDisposable
        {
            using (first)
            {
                using (second)
                {
                    fn(first, second);
                }
            }
        }

        /// <summary>
        /// Put the two generic object in using statement to ensure proper disposal and apply fn to return the expected result.
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the Object in the parent using statement.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the nested Object in the second using statement.</typeparam>
        /// <param name="first">Object in the parent using statement.</param>
        /// <param name="second">Object in the second using statement.</param>
        /// <param name="fn">
        /// A method that accepts TFirst and TSecond as the parameter and returns an object of type TResult.
        /// </param>
        /// <returns>TResult after executing fn.</returns>
        public static TResult Using<TFirst, TSecond, TResult>(
             TFirst first,
             TSecond second,
             Func<TFirst, TSecond, TResult> fn)
            where TFirst : IDisposable
            where TSecond : IDisposable
        {
            using (first)
            {
                using (second)
                {
                    return fn(first, second);
                }
            }
        }

        /// <summary>
        /// Put the two generic object in using statement to ensure proper disposal and applies bodyFn to first object (TFirst),
        /// then applies fn to the second object (TSecond).
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the Object in the parent using statement.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the nested Object in the second using statement.</typeparam>
        /// <param name="first">Object in the parent using statement.</param>
        /// <param name="bodyFn"> A method that accepts TFirst as the parameter and returns an object of type TSecond.</param>
        /// <param name="fn">A method that accepts TFirst and TSecond object.  (Does not return a value).</param>
        public static void Using<TFirst, TSecond>(
             TFirst first,
             Func<TFirst, TSecond> bodyFn,
             Action<TFirst, TSecond> fn)
            where TFirst : IDisposable
            where TSecond : IDisposable
        {
            using (first)
            {
                using (TSecond second = bodyFn(first))
                {
                    fn(first, second);
                }
            }
        }
        /// <summary>
        /// Put the two generic object in using statement to ensure proper disposal and applies bodyFn to first object (TFirst),
        /// then applies fn to the second object (TSecond).
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the Object in the parent using statement.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the nested Object in the second using statement.</typeparam>
        /// <typeparam name="TResult">Generic data type of the returned value.</typeparam>
        /// <param name="first">Object in the parent using statement.</param>
        /// <param name="bodyFn">A method that accepts TFirst as parameter and returns an object of type TSecond.</param>
        /// <param name="fn">A method that accepts TFirst and TSecond as the parameter and returns an object of type TResult.</param>
        /// <returns>TResult after executing fn.</returns>
        public static TResult Using<TFirst, TSecond, TResult>(
             TFirst first,
             Func<TFirst, TSecond> bodyFn,
             Func<TFirst, TSecond, TResult> fn)
            where TFirst : IDisposable
            where TSecond : IDisposable
        {
            using (first)
            {
                using (TSecond second = bodyFn(first))
                {
                    return fn(first, second);
                }
            }
        }

        /// <summary>
        /// Put the two generic object in using statement to ensure proper disposal and applies bodyFn to first object (TFirst),
        /// then applies fn to the second object (TSecond).
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the Object in the parent using statement.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the nested Object in the second using statement.</typeparam>
        /// <param name="first">Object in the parent using statement.</param>
        /// <param name="bodyFn">A method that accepts TFirst as parameter and returns an object of type TSecond.</param>
        /// <param name="fn">A method that accepts TSecond object.  (Does not return a value).</param>
        public static void Using<TFirst, TSecond>(
             TFirst first,
             Func<TFirst, TSecond> bodyFn,
             Action<TSecond> fn)
            where TFirst : IDisposable
            where TSecond : IDisposable
        {
            using (first)
            {
                using (TSecond second = bodyFn(first))
                {
                    fn(second);
                }
            }
        }

        /// <summary>
        /// Put the two generic object in using statement to ensure proper disposal and applies bodyFn to first object (TFirst),
        /// then applies fn to the second object (TSecond).
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the Object in the parent using statement.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the nested Object in the second using statement.</typeparam>
        /// <typeparam name="TResult">Generic data type of the returned value.</typeparam>
        /// <param name="first">Object in the parent using statement.</param>
        /// <param name="bodyFn">A method that accepts TFirst as parameter and returns an object of type TSecond.</param>
        /// <param name="fn">A method that accepts TSecond object. and returns an object of type TResult.</param>
        /// <returns>TResult after executing fn.</returns>
        public static TResult Using<TFirst, TSecond, TResult>(
             TFirst first,
             Func<TFirst, TSecond> bodyFn,
             Func<TSecond, TResult> fn)
            where TFirst : IDisposable
            where TSecond : IDisposable
        {
            using (first)
            {
                using (TSecond second = bodyFn(first))
                {
                    return fn(second);
                }
            }
        }

        /// <summary>
        /// Put the three generic object in using statement to ensure proper disposal.
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the Object in the parent using statement.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the nested Object in the second using statement.</typeparam>
        /// <typeparam name="TThird">Generic data type of the nested Third object in using statement.</typeparam>
        /// <param name="first">Object in the parent using statement.</param>
        /// <param name="second">Object in the second using statement.</param>
        /// <param name="third">Object in the third using statement.</param>
        /// <param name="fn">A method that accepts TFirst, TSecond and TThird as parameter.  (Does not return a value).</param>
        public static void Using<TFirst, TSecond, TThird>(
             TFirst first,
             TSecond second,
             TThird third,
             Action<TFirst, TSecond, TThird> fn)
            where TFirst : IDisposable
            where TSecond : IDisposable
            where TThird : IDisposable
        {
            using (first)
            {
                using (second)
                {
                    using (third)
                    {
                        fn(first, second, third);
                    }
                }
            }
        }
        /// <summary>
        /// Put the three generic object in using statement to ensure proper disposal.
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the Object in the parent using statement.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the nested Object in the second using statement.</typeparam>
        /// <typeparam name="TThird">Generic data type of the nested Third object in using statement.</typeparam>
        /// <typeparam name="TResult">>Generic data type of the return value.</typeparam>
        /// <param name="first">Object in the parent using statement.</param>
        /// <param name="second">Object in the second using statement.</param>
        /// <param name="third">Object in the third using statement.</param>
        /// <param name="fn">A method that accepts TFirst, TSecond and TThird as parameter and returns an object of type TResult.</param>
        /// <returns>TResult after executing fn.</returns>
        public static TResult Using<TFirst, TSecond, TThird, TResult>(
             TFirst first,
             TSecond second,
             TThird third,
             Func<TFirst, TSecond, TThird, TResult> fn)
            where TFirst : IDisposable
            where TSecond : IDisposable
            where TThird : IDisposable
        {
            using (first)
            {
                using (second)
                {
                    using (third)
                    {
                        return fn(first, second, third);
                    }
                }
            }
        }

        /// <summary>
        /// Put the three generic object in using statement to ensure proper disposal.
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the Object in the parent using statement.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the nested Object in the second using statement.</typeparam>
        /// <typeparam name="TThird">Generic data type of the nested Third object in using statement.</typeparam>
        /// <param name="first">Object in the parent using statement.</param>
        /// <param name="bodyFn1">>A method that accepts TFirst as parameter and returns an object of type TSecond.</param>
        /// <param name="bodyFn2">A method that accepts TFirst and TSecond  as parameter and returns an object of type TThird.</param>
        /// <param name="fn">A method that accepts TFirst, TSecond and TThird as parameter. (Does not return a value).</param>
        public static void Using<TFirst, TSecond, TThird>(
             TFirst first,
             Func<TFirst, TSecond> bodyFn1,
             Func<TFirst, TSecond, TThird> bodyFn2,
             Action<TFirst, TSecond, TThird> fn)
            where TFirst : IDisposable
            where TSecond : IDisposable
            where TThird : IDisposable
        {
            using (first)
            {
                using (TSecond second = bodyFn1(first))
                {
                    using (TThird third = bodyFn2(first, second))
                    {
                        fn(first, second, third);
                    }
                }
            }
        }

        /// <summary>
        /// Put the three generic object in using statement to ensure proper disposal.
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the Object in the parent using statement.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the nested Object in the second using statement.</typeparam>
        /// <typeparam name="TThird">Generic data type of the nested Third object in using statement.</typeparam>
        /// <typeparam name="TResult">Generic data type of the return value.</typeparam>
        /// <param name="first">Object in the parent using statement.</param>
        /// <param name="bodyFn1">A method that accepts TFirst as parameter and returns an object of type TSecond.</param>
        /// <param name="bodyFn2">A method that accepts TFirst and TSecond  as parameter and returns an object of type TThird.</param>
        /// <param name="fn">A method that accepts TFirst, TSecond and TThird as parameter and returns an object of type TResult.</param>
        /// <returns>TResult after executing fn.</returns>
        public static TResult Using<TFirst, TSecond, TThird, TResult>(
             TFirst first,
             Func<TFirst, TSecond> bodyFn1,
             Func<TFirst, TSecond, TThird> bodyFn2,
             Func<TFirst, TSecond, TThird, TResult> fn)
            where TFirst : IDisposable
            where TSecond : IDisposable
            where TThird : IDisposable
        {
            using (first)
            {
                using (TSecond second = bodyFn1(first))
                {
                    using (TThird third = bodyFn2(first, second))
                    {
                        return fn(first, second, third);
                    }
                }
            }
        }

        /// <summary>
        /// Put the three generic object in using statement to ensure proper disposal.
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the Object in the parent using statement.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the nested Object in the second using statement.</typeparam>
        /// <typeparam name="TThird">Generic data type of the nested Third object in using statement.</typeparam>
        /// <param name="first">Object in the parent using statement.</param>
        /// <param name="bodyFn1">A method that accepts TFirst as parameter and returns an object of type TSecond.</param>
        /// <param name="bodyFn2">A method that accepts TSecond  as parameter and returns an object of type TThird.</param>
        /// <param name="fn">A method that accepts TThird as parameter. (Does not return a value).</param>
        public static void Using<TFirst, TSecond, TThird>(
             TFirst first,
             Func<TFirst, TSecond> bodyFn1,
             Func<TSecond, TThird> bodyFn2,
             Action<TThird> fn)
            where TFirst : IDisposable
            where TSecond : IDisposable
            where TThird : IDisposable
        {
            using (first)
            {
                using (TSecond second = bodyFn1(first))
                {
                    using (TThird third = bodyFn2(second))
                    {
                        fn(third);
                    }
                }
            }
        }

        /// <summary>
        /// Put the three generic object in using statement to ensure proper disposal.
        /// </summary>
        /// <typeparam name="TFirst">Generic data type of the Object in the parent using statement.</typeparam>
        /// <typeparam name="TSecond">Generic data type of the nested Object in the second using statement.</typeparam>
        /// <typeparam name="TThird">Generic data type of the nested Third object in using statement.</typeparam>
        /// <param name="first">Object in the parent using statement.</param>
        /// <param name="bodyFn1">A method that accepts TFirst as parameter and returns an object of type TSecond.</param>
        /// <param name="bodyFn2">A method that accepts TSecond  as parameter and returns an object of type TThird.</param>
        /// <param name="fn">A method that accepts TThird as parameter and returns an object of type TResult.</param>
        public static TResult Using<TFirst, TSecond, TThird, TResult>(
             TFirst first,
             Func<TFirst, TSecond> bodyFn1,
             Func<TSecond, TThird> bodyFn2,
             Func<TThird, TResult> fn)
            where TFirst : IDisposable
            where TSecond : IDisposable
            where TThird : IDisposable
        {
            using (first)
            {
                using (TSecond second = bodyFn1(first))
                {
                    using (TThird third = bodyFn2(second))
                    {
                        return fn(third);
                    }
                }
            }
        }

        #endregion
    }
}