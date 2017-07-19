using System.Collections;
using Xunit;

namespace Magenic.SharedKernel.Tests
{
    /// <summary>
    /// Extra assert functionality to ensure we are DRY.
    /// </summary>
    /// <remarks>
    /// Might be able to use xunit.assert.source nuget pkg later and extend directly 
    /// but not sure with dependencies the runner packages have on assert and updates. 
    /// http://stackoverflow.com/questions/16194952/extending-xunit-assert-class-with-new-asserts
    /// </remarks>
    public static class AssertEx
    {
        #region Public Methods

        /// <summary>
        /// Asserts that the enumerable is both not null and not empty
        /// </summary>
        /// <param name="collection">Collection to assert on.</param>
        public static void NotNullOrEmpty(IEnumerable collection)
        {
            Assert.NotNull(collection);
            Assert.NotEmpty(collection);
        }

        /// <summary>
        /// Asserts that the enumerable is: not null, not empty and of type T.
        /// </summary>
        /// <param name="collection">Collection to assert on.</param>
        public static void NotNullOrEmptyOfType<T>(IEnumerable collection)
        {
            Assert.NotNull(collection);
            Assert.IsType<T>(collection);
            Assert.NotEmpty(collection);
        }

        /// <summary>
        /// Asserts that the object is not null and of type T.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="obj">The object to be evaluated.</param>
        public static void NotNullOfType<T>(T obj)
            where T : class
        {
            Assert.NotNull(obj);
            Assert.IsType<T>(obj);
        }

        #endregion Public Methods
    }
}
