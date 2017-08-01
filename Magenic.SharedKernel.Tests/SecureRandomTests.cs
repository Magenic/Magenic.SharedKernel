using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Xunit;

namespace Magenic.SharedKernel.Tests
{
    /// <summary>
    /// This class implements unit tests for SecureRandom class.
    /// </summary>
    public class SecureRandomTests
    {
        #region Public Methods

        /// <summary>
        /// A test for NextBool.
        ///</summary>
        [Fact]
        public void SecureRandom_NextBool()
        {
            using (SecureRandom sr = SecureRandom.Create())
            {
                AssertEx.NotNullOfType<SecureRandom>(sr);

                IEnumerable<bool> seq = Seq.Repeat(
                    () => sr.NextBool(),
                    sr.Next(128, 256));

                AssertEx.NotNullOrEmpty(seq);

                IList<bool> list = seq.ToList();

                AssertEx.NotNullOrEmptyOfType<List<bool>>(list);

                list.Should().Contain(true);
                list.Should().Contain(false);
            }
        }

        /// <summary>
        /// A test for Next.
        ///</summary>
        [Fact]
        public void SecureRandom_Next()
        {
            int minValue = 64;
            int maxValue = 4194304;
            
            using (SecureRandom sr = SecureRandom.Create())
            {
                AssertEx.NotNullOfType<SecureRandom>(sr);

                int count = sr.Next(16, 32);

                IEnumerable<int> seq = Seq.Repeat(
                    () => sr.Next(),
                    count);

                AssertEx.NotNullOrEmpty(seq);

                IList<int> list = seq.ToList();

                AssertEx.NotNullOrEmptyOfType<List<int>>(list);

                Assert.Equal(count, list.Count);
                Assert.Equal(list.Count, seq.Distinct().Count());

                Util.Repeat(
                    () => sr.Next(maxValue).Should().BeLessThan(maxValue),
                    count);
                Util.Repeat(
                    () => sr.Next(minValue, maxValue).Should().BeInRange(minValue, maxValue),
                    count);
            }
        }

        #endregion
    }
}
