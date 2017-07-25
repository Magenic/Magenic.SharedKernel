using System;
using System.Security.Cryptography;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Secure random class with the capability of passing down a RandomNumberGenerator
    /// object or having one created for you.
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
        private readonly double DENOMINTAOR = Convert.ToDouble(int.MaxValue);
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
        /// Constructor that takes in a random number generator object.
        /// </summary>
        /// <param name="rng">RandomNumberGenerator object.</param>
        private SecureRandom(RandomNumberGenerator rng)
        {
            _rng = rng;
        }

        /// <summary>
        /// Factory method.
        /// </summary>
        /// <returns>SecureRandom object.</returns>
        public static SecureRandom Create()
        {
            return new SecureRandom();
        }

        /// <summary>
        /// Factory method that takes in a random number generator object.
        /// </summary>
        /// <param name="rng">RandomNumberGenerator object.</param>
        /// <returns>SecureRandom object.</returns>
        public static SecureRandom Create(RandomNumberGenerator rng)
        {
            return new SecureRandom(rng);
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
            if (maxValue < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxValue),
                    "Must be greater than or equal to 0.");
            }

            return Next() % maxValue;
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
                    $"The parameter {nameof(maxValue)} must be greater than or equal to " +
                    $"the parameter {nameof(minValue)}!");
            }

            int range = maxValue - minValue;

            return minValue + Next(range);
        }

        /// <summary>
        /// Returns an array of random bytes.
        /// </summary>
        /// <param name="length">Desired length of the randomly created byte array.</param>
        /// <returns>Byte array.</returns>
        public byte[] NextBytes(int length)
        {
            if (length > 0)
            {
                byte[] randomByteArray = new byte[length];

                _rng.GetBytes(randomByteArray);

                return randomByteArray;
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

        /// <summary>
        /// Returns a random double.
        /// </summary>
        /// <returns>Double in the range 0 to 1.</returns>
        public double NextDouble()
        {
            int next = Next();
            double numerator = Convert.ToDouble(next);
            double val = numerator / DENOMINTAOR;

            return val;
        }

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
