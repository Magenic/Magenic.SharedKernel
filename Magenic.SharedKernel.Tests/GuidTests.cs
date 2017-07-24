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
        /// A test for ToUpper, first overload.
        ///</summary>
        ///<remarks>This tests ToUpper without argument.</remarks>
        [Fact]
        public void Guid_ToUpper_1()
        {
            Guid guid0 = Guid.Empty;
            Guid guid1 = Guid.NewGuid();

            Assert.Equal(Guid.Empty, guid0);
            Assert.NotEqual(Guid.Empty, guid1);

            Assert.Equal(Guid.Empty.ToString().ToUpper(), guid0.ToUpper());
            Assert.Equal(guid1.ToString().ToUpper(), guid1.ToUpper());
        }

        /// <summary>
        /// A test for ToUpper, second overload.
        ///</summary>
        ///<remarks>This tests ToUpper with format argument.</remarks>
        [Fact]
        public void Guid_ToUpper_2()
        {
            Guid guid0 = Guid.Empty;
            Guid guid1 = Guid.NewGuid();

            Assert.Equal(Guid.Empty, guid0);
            Assert.NotEqual(Guid.Empty, guid1);

            Seq.List("N", "D", "B", "P", "X", string.Empty, null, "").Apply(format =>
            {
                Assert.Equal(Guid.Empty.ToString(format).ToUpper(), guid0.ToUpper(format));
                Assert.Equal(guid1.ToString(format).ToUpper(), guid1.ToUpper(format));
            });

            Seq.List("Invalid", "0", "1", "A", "C", "m", "W", "y", "Z").Apply(format =>
            {
                Assert.Throws<FormatException>(() => guid0.ToString(format));
                Assert.Throws<FormatException>(() => guid0.ToUpper(format));
                Assert.Throws<FormatException>(() => guid1.ToString(format));
                Assert.Throws<FormatException>(() => guid1.ToUpper(format));
            });
        }

        #endregion
    }
}
