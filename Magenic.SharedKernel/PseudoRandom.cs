using System;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Static class that provides a set of extension methods on Random that generate random values of different types.
    /// </summary>
    /// <remarks>Random is a predictable sequence generator.</remarks>
    public static class PseudoRandom
    {
        #region Public Methods

        /// <summary>
        /// Creates an instance of a Random object initialized by passed in seed or hash code of a newly generated guid.
        /// </summary>
        /// <param name="seed">Seed value.</param>
        /// <returns>
        /// An object of type Random that has seed value from parameter or a unique integer value if the seed is null.
        /// </returns>
        public static Random Create(int? seed = null)
            => new Random(seed ?? Guid.NewGuid().GetHashCode());

        /// <summary>
        ///  Creates an instance of a Random object.
        /// </summary>
        /// <remarks>
        /// The default Random constructor uses a time dependent seed thus Random objects
        /// generated in succession using this factory method will have identical seeds.
        /// End result is random objects will generate same sequences.
        /// </remarks>
        /// <returns>An object of type Random.</returns>
        public static Random CreateWithTimeDependentSeed() => new Random();

        /// <summary>
        /// Extension method for Random object that returns a boolean value that is either True or False.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <returns>Returns a boolean value that is either True or False.</returns>
        public static bool NextBool(this Random random)
            => RandomUtil.WithNextBool(random.Next);

        /// <summary>
        /// Returns a random byte.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <returns>Byte.</returns>
        public static byte NextByte(this Random random)
            => RandomUtil.WithNextByte(i => random.NextBytes(i));

        /// <summary>
        /// Extension method for Random object that returns a random bytes.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <param name="length">Desired length of the randomly created byte array.</param>
        /// <returns>Byte array.</returns>
        public static byte[] NextBytes(this Random random, int length)
            => RandomUtil.WithNextBytes(random.NextBytes, length);

        /// <summary>
        /// Generates a random char with specified composition.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <param name="charType">Specifies type of random char to be generated.</param>
        /// <returns>Char.</returns>
        public static char NextChar(
             this Random random,
             StringComposition charType = StringComposition.AlphaNumeric)
            => RandomUtil.WithNextChar(random.NextString, charType);

        /// <summary>
        /// Generates a random decimal.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <returns>Decimal.</returns>
        public static decimal NextDecimal(this Random random)
            => RandomUtil.WithNextDecimal(random.Next, random.Next);

        /// <summary>
        /// Returns a random double.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <returns>Double in the range 0 to 1.</returns>
        public static double NextDouble(this Random random)
            => RandomUtil.WithNextDouble(random.Next);

        /// <summary>
        /// Generates a random long.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <returns>Long.</returns>
        public static long NextLong(this Random random)
            => RandomUtil.WithNextLong(random.NextBytes, random.Next);

        /// <summary>
        /// Generates a random short.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <returns>Short.</returns>
        public static short NextShort(this Random random)
            => RandomUtil.WithNextShort(random.Next);

        /// <summary>
        /// Generates a random string with specified length and composition.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <param name="length">Desired length of generated string..</param>
        /// <param name="stringComposition">Enumerated type specifying string composition.</param>
        /// <returns>String.</returns>
        public static string NextString(
             this Random random,
             int length,
             StringComposition stringComposition = StringComposition.AlphaNumeric)
            => RandomUtil.WithNextString(
                source => source.RandomRef(random),
                length,
                stringComposition);

        /// <summary>
        /// Generates a random string with specified length range and composition.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <param name="minLength">Minimum length of randomly string generated.</param>
        /// <param name="maxLength">Maximum length of randomly string generated.</param>
        /// <param name="stringComposition">Enumeration of string composition.</param>
        /// <returns>String.</returns>
        public static string NextString(
             this Random random,
             int minLength,
             int maxLength,
             StringComposition stringComposition = StringComposition.AlphaNumeric)
            => random.NextString(
                random.Next(minLength, maxLength),
                stringComposition);

        #endregion
    }
}