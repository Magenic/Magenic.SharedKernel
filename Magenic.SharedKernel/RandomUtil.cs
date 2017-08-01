using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magenic.SharedKernel
{
    /// <summary>/// <summary>
    /// Provides a set of internal methods that generate random values of different types.
    /// </summary>
    internal static class RandomUtil
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