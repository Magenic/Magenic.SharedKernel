using System;
using System.Security.Cryptography;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Cryptographically secure random class.
    /// </summary>
    /// <remarks>This class implements IDisposable.</remarks>
    public class SecureRandom : DisposableObject
    {
        #region Constants

        /// <summary>
        /// Buffer size must be a multiple of four.
        /// </summary>
        private const int BUFFER_SIZE = 1024;

        #endregion

        #region Fields

        private readonly byte[] _buffer = new byte[BUFFER_SIZE];
        private int _offset = BUFFER_SIZE;
        private readonly RandomNumberGenerator _rng;

        #endregion

        #region Creation

        /// <summary>
        /// Default constructor that creates a random number generator.
        /// </summary>
        private SecureRandom()
        {
            _rng = RandomNumberGenerator.Create();
        }

        /// <summary>
        /// Factory method.
        /// </summary>
        /// <returns>SecureRandom object.</returns>
        public static SecureRandom Create()
        {
            return new SecureRandom();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns>An integer that is greater than or equal to 0 and less than int.MaxValue.</returns>
        public int Next()
        {
            if (_offset >= _buffer.Length)
            {
                FillBuffer();
            }
            
            int val = BitConverter.ToInt32(_buffer, _offset) & int.MaxValue;

            _offset += sizeof(int);

            return val;
        }

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>An integer that is greater than or equal to 0 and less than maxValue.</returns>
        public int Next(int maxValue)
        {
            if (maxValue > 0)
            {
                return Next() % maxValue;
            }
            else if (maxValue == 0)
            {
                return 0;
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxValue),
                    "Must be greater than or equal to 0.");
            }
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned.</param>
        /// <returns>An integer that is greater than or equal to minValue and less than maxValue.</returns>
        public int Next(int minValue, int maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minValue),
                    $"The parameter {nameof(maxValue)} must be greater than or equal to " +
                    $"the parameter {nameof(minValue)}!");
            }

            int range = maxValue - minValue;

            return minValue + Next(range);
        }

        /// <summary>
        /// Extension method for Random object that returns a boolean value that is either True or False.
        /// </summary>
        /// <param name="random">Random object that is being extended.</param>
        /// <returns>Returns a boolean value that is either True or False.</returns>
        public bool NextBool() => RandomEx.WithNextBool(Next);

        /// <summary>
        /// Returns a random byte.
        /// </summary>
        /// <returns>Byte.</returns>
        public byte NextByte() => RandomEx.WithNextByte(NextBytes);

        /// <summary>
        /// Returns an array of random bytes.
        /// </summary>
        /// <param name="length">Desired length of the randomly created byte array.</param>
        /// <returns>Byte array.</returns>
        public byte[] NextBytes(int length)
            => RandomEx.WithNextBytes(_rng.GetBytes, length);

        /// <summary>
        /// Generates a random char with specified composition.
        /// </summary>
        /// <param name="charType">Specifies type of random char to be generated.</param>
        /// <returns>Char.</returns>
        public char NextChar(
             StringComposition charType = StringComposition.AlphaNumeric)
            => RandomEx.WithNextChar(NextString, charType);

        /// <summary>
        /// Generates a random decimal.
        /// </summary>
        /// <returns>Decimal.</returns>
        public decimal NextDecimal() => RandomEx.WithNextDecimal(Next, Next);

        /// <summary>
        /// Returns a random double.
        /// </summary>
        /// <returns>Double in the range 0 to 1.</returns>
        public double NextDouble() => RandomEx.WithNextDouble(Next);

        /// <summary>
        /// Generates a random long.
        /// </summary>
        /// <returns>Long.</returns>
        public long NextLong() => RandomEx.WithNextLong(NextBytes, Next);

        /// <summary>
        /// Generates a random short.
        /// </summary>
        /// <returns>Short.</returns>
        public short NextShort() => RandomEx.WithNextShort(Next);

        /// <summary>
        /// Generates a random string with specified length and composition.
        /// </summary>
        /// <param name="length">Desired length of generated string..</param>
        /// <param name="stringComposition">Enumerated type specifying string composition.</param>
        /// <returns>String.</returns>
        public string NextString(
             int length,
             StringComposition stringComposition = StringComposition.AlphaNumeric)
            => RandomEx.WithNextString(
                source => source.RandomRef(this),
                length,
                stringComposition);

        /// <summary>
        /// Generates a random string with specified length range and composition.
        /// </summary>
        /// <param name="minLength">Minimum length of randomly string generated.</param>
        /// <param name="maxLength">Maximum length of randomly string generated.</param>
        /// <param name="stringComposition">Enumeration of string composition.</param>
        /// <returns>String.</returns>
        public string NextString(
             int minLength,
             int maxLength,
             StringComposition stringComposition = StringComposition.AlphaNumeric)
            => NextString(Next(minLength, maxLength), stringComposition);

        #endregion

        #region Protected Methods

        protected override void DisposeManagedResources()
        {
            _rng.Dispose();
        }

        #endregion

        #region Private Methods

        private void FillBuffer()
        {
            _rng.GetBytes(_buffer);
            _offset = 0;
        }

        #endregion
    }
}
