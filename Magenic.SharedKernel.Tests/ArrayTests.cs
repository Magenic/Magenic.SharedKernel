using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Xunit;

namespace Magenic.SharedKernel.Tests
{
    /// <summary>
    /// This class implements unit tests for ArrayEx utility class.
    /// </summary>
    public class ArrayUnitTests
    {
        #region Public Methods

        /// <summary>
        /// A test for IsEmpty.
        ///</summary>
        [Fact]
        public void Array_IsEmpty()
        {
            char[] arr = new char[0];

            AssertEx.NotNullOfType<char[]>(arr);
            Assert.Equal(0, arr.Length);
            Assert.Equal(0, arr.Count());

            Assert.True(Array.Empty<int>().IsEmpty());
            Array.Empty<int>().Should().BeEmpty();
            Assert.True(arr.IsEmpty());
            Assert.Empty(arr);
        }

        /// <summary>
        /// A test for IsNullOrEmpty.
        ///</summary>
        [Fact]
        public void Array_IsNullOrEmpty()
        {
            char[] arr = null;

            Assert.Null(arr);
            Assert.True(ArrayEx.IsNullOrEmpty(arr));
            Assert.True(ArrayEx.IsNullOrEmpty(Array.Empty<int>()));
        }

        /// <summary>
        /// A test for RandomRef.
        ///</summary>
        [Fact]
        public void Array_RandomRef()
        {
            Random random = PseudoRandom.Create(2131709272);
            IEnumerable<int> seq = Enumerable.Range(0, 10);

            AssertEx.NotNullOfType<Random>(random);
            AssertEx.NotNullOrEmpty(seq);

            int[] arr = seq.ToArray();

            AssertEx.NotNullOrEmptyOfType<int[]>(arr);
            arr.Should().Equal(seq);

            int count = random.Next(64, 128);
            
            count.Should().BeGreaterThan(0);
            count.Should().BeGreaterOrEqualTo(64);
            count.Should().BeLessThan(128);

            Util.Repeat(() => seq.Should().Contain(arr.RandomRef(random)), count);
        }

        #endregion
    }
}
