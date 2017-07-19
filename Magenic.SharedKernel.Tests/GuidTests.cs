using System;

using Xunit;

namespace Magenic.SharedKernel.Tests
{
    /// <summary>
    /// This class implements unit tests for GuidEx utility class.
    /// </summary>
    public class GuidUnitTests
    {
        #region Public Methods

        /// <summary>
        /// A test for IsEmpty.
        ///</summary>
        [Fact]
        public void Guid_IsEmpty()
        {
            Guid guid0 = Guid.Empty;
            Guid guid1 = Guid.NewGuid();

            Assert.Equal(Guid.Empty, guid0);
            Assert.NotEqual(Guid.Empty, guid1);

            Assert.True(guid0.IsEmpty());
            Assert.True(Guid.Empty.IsEmpty());
            Assert.False(guid1.IsEmpty());
        }

        /// <summary>
        /// A test for IsNullOrEmpty.
        ///</summary>
        [Fact]
        public void Guid_IsNullOrEmpty()
        {
            Guid guid0 = Guid.Empty;
            Guid guid1 = Guid.NewGuid();
            Guid? guid2 = null;
            Guid? guid3 = new Guid?(Guid.NewGuid());
            Guid? guid4 = new Guid?(Guid.Empty);

            Assert.Equal(Guid.Empty, guid0);
            Assert.NotEqual(Guid.Empty, guid1);
            Assert.Null(guid2);
            Assert.NotNull(guid3);
            Assert.NotNull(guid4);
            Assert.NotEqual(Guid.Empty, guid3.Value);
            Assert.Equal(Guid.Empty, guid4.Value);
            
            Assert.True(GuidEx.IsNullOrEmpty(guid0));
            Assert.False(GuidEx.IsNullOrEmpty(guid1));
            Assert.True(GuidEx.IsNullOrEmpty(guid2));
            Assert.False(GuidEx.IsNullOrEmpty(guid3));
            Assert.True(GuidEx.IsNullOrEmpty(guid4));
        }

        /// <summary>
        /// A test for ToUpper.
        ///</summary>
        [Fact]
        public void Guid_ToUpper()
        {
            Guid guid0 = Guid.Empty;
            Guid guid1 = Guid.NewGuid();

            Assert.Equal(Guid.Empty, guid0);
            Assert.NotEqual(Guid.Empty, guid1);

            Assert.Equal(Guid.Empty.ToString().ToUpper(), guid0.ToUpper());
            Assert.Equal(guid1.ToString().ToUpper(), guid1.ToUpper());
        }

        #endregion
    }
}
