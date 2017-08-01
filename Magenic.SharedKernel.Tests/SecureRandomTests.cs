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
        public void Random_NextBool()
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

        #endregion
    }
}
