using System;
using System.Collections.Generic;
using Xunit;
using static Magenic.SharedKernel.Parser;

namespace Magenic.SharedKernel.Tests
{
    public class ParserTests
    {
        #region Public Methods

        public static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { "845", (TryParseDelegate<int>) int.TryParse };
            yield return new object[] { "-845", (TryParseDelegate<int>) int.TryParse };
            yield return new object[] { "abc", (TryParseDelegate<int>) int.TryParse };
            yield return new object[] { "1.8", (TryParseDelegate<int>) int.TryParse };
            yield return new object[] { "1.8", (TryParseDelegate<double>) double.TryParse };
            yield return new object[] { "1.8", (TryParseDelegate<float>) float.TryParse };
            yield return new object[] { "abc", (TryParseDelegate<DateTime>) DateTime.TryParse };
            yield return new object[] { "2017-01-01", (TryParseDelegate<DateTime>) DateTime.TryParse };
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Parser_SameAsTryParse<T>(string str, TryParseDelegate<T> method) where T: struct
        {
            var actual = ParseToNullable(str, method);
            T expected;
            if (method(str, out expected))
            {
                Assert.Equal(expected, actual);
            }
            else
            {
                Assert.Equal(null, actual);
            }
        }
        #endregion
    }
}
