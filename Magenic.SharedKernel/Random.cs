using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Provides a set of extension methods on Random and internal methods that generate random values of different types.
    /// </summary>
    /// <remarks>Random is a predictable sequence generator whereas SecureRandom is cryptographically secure.</remarks>
    public static class RandomEx
    {
        #region Fields

        private static readonly char[] LOWERCASE_LETTERS =
            "abcdefghijklmnopqrstuvqxyz".ToCharArray();
        private static readonly char[] UPPERCASE_LETTERS =
            "ABCDEFGHIJKLMNOPQRSTUVQXYZ".ToCharArray();
        private static readonly char[] DIGITS = "0123456789".ToCharArray();
        private static readonly char[] SYMBOLS = "$+<=>^`|~".ToCharArray();
        private static readonly char[] PUNCTUATION_MARKS =
            @"!""#%&'()*,-./:;?@[\]{}".ToCharArray();
        private static readonly char[] WHITE_SPACE = Enumerable
            .Repeat(' ', 6)
            .ToArray();
        private static readonly double INT_MAX_VALUE = Convert.ToDouble(int.MaxValue);
        private static readonly Func<StringComposition, char[]>
            _GenerateSourceMemoizerFn = Memoizer
                .Memoize<StringComposition, char[]>(sc => GenerateSource(sc));
        private static readonly int BYTE_HALFWAY_VALUE = Convert.ToInt32(
            Math.Ceiling((double)byte.MaxValue / 2));

        #endregion

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
        /// generated in succession using this constructor will have identical seeds thus
        /// generating the same sequences.
        /// </remarks>
        /// <returns>An object of type Random.</returns>
        public static Random CreateWithTimeDependentSeed() => new Random();

        /// <summary>
        /// Extension method for Random object that returns a boolean value that is either True or False.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <returns>Returns a boolean value that is either True or False.</returns>
        public static bool NextBool(this Random random) => WithNextBool(random.Next);

        /// <summary>
        /// Returns a random byte.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <returns>Byte.</returns>
        public static byte NextByte(this Random random)
            => WithNextByte(i => random.NextBytes(i));

        /// <summary>
        /// Extension method for Random object that returns a random bytes.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <param name="length">Desired length of the randomly created byte array.</param>
        /// <returns>Byte array.</returns>
        public static byte[] NextBytes(this Random random, int length)
            => WithNextBytes(random.NextBytes, length);

        /// <summary>
        /// Generates a random char with specified composition.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <param name="charType">Specifies type of random char to be generated.</param>
        /// <returns>Char.</returns>
        public static char NextChar(
             this Random random,
             StringComposition charType = StringComposition.AlphaNumeric)
            => WithNextChar(random.NextString, charType);

        /// <summary>
        /// Generates a random decimal.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <returns>Decimal.</returns>
        public static decimal NextDecimal(this Random random)
            => WithNextDecimal(random.Next, random.Next);

        /// <summary>
        /// Returns a random double.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <returns>Double in the range 0 to 1.</returns>
        public static double NextDouble(this Random random) => WithNextDouble(random.Next);

        /// <summary>
        /// Generates a random long.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <returns>Long.</returns>
        public static long NextLong(this Random random)
            => WithNextLong(random.NextBytes, random.Next);

        /// <summary>
        /// Generates a random short.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <returns>Short.</returns>
        public static short NextShort(this Random random) => WithNextShort(random.Next);

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
            => WithNextString(source => source.RandomRef(random), length, stringComposition);

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

        #region Internal Methods

        internal static bool WithNextBool(Func<int, int> fnNext) => (fnNext(2) == 1);

        internal static byte WithNextByte(Func<int, byte[]> fnNextByte) => fnNextByte(1)[0];

        internal static byte[] WithNextBytes(Action<byte[]> fnGetBytes, int length)
        {
            if (length > 0)
            {
                byte[] buffer = new byte[length];

                fnGetBytes(buffer);

                return buffer;
            }
            else if (length == 0)
            {
                return Array.Empty<byte>();
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    nameof(length),
                    "Must be greater than or equal to 0.");
            }
        }

        internal static char WithNextChar(
             Func<int, StringComposition, string> fnNextString,
             StringComposition charType = StringComposition.AlphaNumeric)
            => fnNextString(1, charType)[0];

        internal static decimal WithNextDecimal(
            Func<int> fnNext,
            Func<int, int, int> fnNextMinMax)
            => new decimal(
                    fnNext(),
                    fnNext(),
                    fnNext(),
                    false,
                    Convert.ToByte(fnNextMinMax(0, 29)));

        internal static double WithNextDouble(Func<int> fnNext)
        {
            int next = fnNext();
            double numerator = Convert.ToDouble(next);
            double val = numerator / INT_MAX_VALUE;

            return val;
        }

        internal static long WithNextLong(
            Func<int, byte[]> fnNextBytes,
            Func<int, int, int> fnNext)
        {
            byte[] arr = fnNextBytes(sizeof(long) - 1);
            byte ub = Convert.ToByte(fnNext(0, BYTE_HALFWAY_VALUE));
            long val = BitConverter.ToInt64(arr.Append(ub).ToArray(), 0);

            return val;
        }

        internal static short WithNextShort(Func<int, int, int> fnNext)
            => Convert.ToInt16(fnNext(0, short.MaxValue + 1));

        internal static string WithNextString(
            Func<char[], char> fnRandomRef,
            int length,
            StringComposition stringComposition)
        {
            if (length > 0)
            {
                char[] source = _GenerateSourceMemoizerFn(stringComposition);
                StringBuilder sb = StringEx.CreateSB();

                Util.Repeat(() => sb.Append(fnRandomRef(source)), length);

                return sb.ToString();
            }
            else if (length == 0)
            {
                return string.Empty;
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    nameof(length),
                    "Must be greater than or equal to 0.");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates pool from which random characters are pulled.
        /// </summary>
        /// <param name="stringComposition">Specifies composition.</param>
        /// <returns>Char array.</returns>
        private static char[] GenerateSource(
             StringComposition stringComposition)
        {
            IEnumerable<char> source = Enumerable.Empty<char>();

            if ((stringComposition & StringComposition.LowercaseLetter) != 0)
            {
                source = source.Append(LOWERCASE_LETTERS);
            }
            if ((stringComposition & StringComposition.UppercaseLetter) != 0)
            {
                source = source.Append(UPPERCASE_LETTERS);
            }
            if ((stringComposition & StringComposition.Digit) != 0)
            {
                source = source.Append(DIGITS);
            }
            if ((stringComposition & StringComposition.Symbol) != 0)
            {
                source = source.Append(SYMBOLS);
            }
            if ((stringComposition & StringComposition.PunctuationMark) != 0)
            {
                source = source.Append(PUNCTUATION_MARKS);
            }
            if ((stringComposition & StringComposition.WhiteSpace) != 0)
            {
                source = source.Append(WHITE_SPACE);
            }

            if (source.Any())
            {
                return source.ToArray();
            }
            else
            {
                throw new ArgumentException(
                    $"String composition {stringComposition} is invalid!");
            }
        }

        #endregion
    }
}