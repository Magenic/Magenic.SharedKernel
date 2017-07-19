using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Xunit;

namespace Magenic.SharedKernel.Tests
{
    /// <summary>
    /// This class implements unit tests for RandomEx utility class.
    /// </summary>
    public class RandomTests
    {
        #region Private Static Methods
        
        private static string NextStringTest(
            Random random,
            StringComposition stringComposition,
            Func<char, bool> fn = null)      
        {
            int length = random.Next(1024, 2048);
            string s = random.NextString(length, stringComposition);

            Assert.IsType<int>(length);
            Assert.True(length >= 1024);
            Assert.True(length < 2048);
            Assert.NotNull(s);
            Assert.IsType<string>(s);
            Assert.NotEmpty(s);
            Assert.Equal(length, s.Length);

            fn.IfNotNull(f => s.Apply(c => Assert.True(f(c))));

            return s;
        }

        private static string NextStringMinMaxTest(
            Random random,
            StringComposition stringComposition)
        {
            int minLength = random.Next(256, 512);
            int maxLength = random.Next(512, 1024);

            Assert.IsType<int>(minLength);
            Assert.True(minLength >= 256);
            Assert.True(minLength < 512);
            Assert.IsType<int>(maxLength);
            Assert.True(maxLength >= 512);
            Assert.True(maxLength < 1024);            

            string s = random.NextString(minLength, maxLength, stringComposition);

            Assert.NotNull(s);
            Assert.True(s.Length >= minLength);
            Assert.True(s.Length < maxLength);            
            Assert.IsType<string>(s);
            Assert.NotEmpty(s);            

            return s;
        }

        private static string NextCharTest(
            Random random,
            StringComposition stringComposition)
        {
            IEnumerable<char> seq = Seq.Repeat(
               () => random.NextChar(stringComposition), random.Next(512, 1024));

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);

            string s = new string(seq.ToArray());

            Assert.NotNull(s);
            Assert.IsType<string>(s);
            Assert.NotEmpty(s);            
            Assert.True(s.Length >= 512);
            Assert.True(s.Length < 1024);            

            return s;
        }

        #endregion

        #region Public Instance Methods

        /// <summary>
        /// A test for Create.
        ///</summary>
        [Fact]
        public void Random_Create()
        {
            Random random0 = RandomEx.Create(1513162799);
            Random random1 = RandomEx.Create(1918435865);

            Seq.List(random0, random1).Apply(r =>
            {
                Assert.NotNull(r);
                Assert.IsType<Random>(r);
            });

            // Tests that random objects created with different seeds 
            // generate different pseudo random sequences.
            Util.Repeat(() => Assert.NotEqual(random0.Next(), random1.Next()), 16);

            random0 = RandomEx.Create(6543);
            random1 = RandomEx.Create(6543);

            Seq.List(random0, random1).Apply(r =>
            {
                Assert.NotNull(r);
                Assert.IsType<Random>(r);
            });

            // Tests that random objects created with the same seed 
            // generate the same pseudo random sequences.
            Util.Repeat(() => Assert.Equal(
                random0.NextString(4, 16),
                random1.NextString(4, 16)), 4);
            Util.Repeat(() => Assert.Equal(random0.Next(), random1.Next()), 8);
            Util.Repeat(() => Assert.Equal(random0.NextBool(), random1.NextBool()), 16);
            Util.Repeat(() => Assert.Equal(random0.NextChar(), random1.NextChar()), 32);

            random0 = RandomEx.Create();
            random1 = RandomEx.Create();

            Seq.List(random0, random1).Apply(r =>
            {
                Assert.NotNull(r);
                Assert.IsType<Random>(r);
            });

            // Tests that random objects created without specifying a seed
            // generate different pseudo random sequences.
            Util.Repeat(() => Assert.NotEqual(random0.Next(), random1.Next()), 16);
        }

        /// <summary>
        /// A test for CreateWithTimeDependentSeed.
        ///</summary>
        [Fact]
        public void Random_CreateWithTimeDependentSeed()
        {
            Random random = RandomEx.CreateWithTimeDependentSeed();

            Assert.NotNull(random);
            Assert.IsType<Random>(random);

            IEnumerable<int> seq = Seq.Repeat(
                () => random.Next(), random.Next(16, 32));

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);

            IList<int> list = seq.ToList();

            Assert.NotNull(list);
            Assert.IsType<List<int>>(list);
            Assert.NotEmpty(list);
            list.Should().OnlyHaveUniqueItems();

            Random random0 = RandomEx.CreateWithTimeDependentSeed();
            Random random1 = new Random();

            Seq.List(random0, random1).Apply(r =>
            {
                Assert.NotNull(r);
                Assert.IsType<Random>(r);
            });

            // Tests that random object created using CreateWithTimeDependentSeed 
            // behaves the same way as new Random() and produces the same pseudo 
            // random sequences.
            Util.Repeat(() => Assert.Equal(random0.Next(), random1.Next()), 10);           
        }

        /// <summary>
        /// A test for NextBool.
        ///</summary>
        [Fact]
        public void Random_NextBool()
        {
            Random random = RandomEx.Create(42342344);

            Assert.NotNull(random);
            Assert.IsType<Random>(random);

            IEnumerable<bool> seq = Seq.Repeat(() => random.NextBool(), 
                random.Next(128, 256));

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);

            IList<bool> list = seq.ToList();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
            Assert.IsType<List<bool>>(list);

            list.Should().Contain(true);
            list.Should().Contain(false);
        }

        /// <summary>
        /// A test for NextBytes.
        ///</summary>
        [Fact]
        public void Random_NextBytes()
        {
            Random random0 = RandomEx.Create(6814614);
            Random random1 = RandomEx.Create(6814614);
            byte[] buffer = new byte[33];

            Seq.List(random0, random1).Apply(r =>
            {
                Assert.NotNull(r);
                Assert.IsType<Random>(r);
            });

            Assert.NotNull(buffer);
            Assert.IsType<byte[]>(buffer);
            Assert.NotEmpty(buffer);          

            // Tests that NextBytes(int length) produces the same result as 
            // NextBytes(byte[] buffer)
            random0.NextBytes(buffer);
            random1.NextBytes(33).Should().Equal(buffer);

            IEnumerable<byte[]> seq = Seq.Repeat(() => random0.NextBytes(20),
                random0.Next(10, 20));

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);

            IList<byte[]> list = seq.ToList();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
            Assert.IsType<List<byte[]>>(list);

            // Tests that random data generated varies
            Assert.True(list.Any(o => o != list[0]));
        }

        /// <summary>
        /// A test for NextByte.
        ///</summary>
        [Fact]
        public void Random_NextByte()
        {
            Random random0 = RandomEx.Create(28647);
            Random random1 = RandomEx.Create(28647);
            byte[] buffer = new byte[1];

            Seq.List(random0, random1).Apply(r =>
            {
                Assert.NotNull(r);
                Assert.IsType<Random>(r);
            });

            Assert.NotNull(buffer);
            Assert.IsType<byte[]>(buffer);
            Assert.NotEmpty(buffer);

            random0.NextBytes(buffer);

            Assert.Equal(buffer[0], random1.NextByte());

            IEnumerable<byte> seq = Seq.Repeat(() => random0.NextByte(),
                random0.Next(10, 20));

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);

            IList<byte> list = seq.ToList();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
            Assert.IsType<List<byte>>(list);

            // Tests that random data generated varies
            Assert.True(list.Any(o => o != list[0]));
        }

        /// <summary>
        /// A test for NextShort.
        ///</summary>
        [Fact]
        public void Random_NextShort()
        {
            Random random0 = RandomEx.Create(23424234);
            Random random1 = RandomEx.Create(23424234);
            
            Seq.List(random0, random1).Apply(r =>
            {
                Assert.NotNull(r);
                Assert.IsType<Random>(r);
            });

            Util.Repeat(() => Assert.Equal(random0.NextShort(), random1.NextShort()),
                9);

            IEnumerable<short> seq = Seq.Repeat(() => random0.NextShort(),
                random0.Next(10, 20));

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);

            IList<short> list = seq.ToList();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
            Assert.IsType<List<short>>(list);

            // Tests that random data generated varies
            Assert.True(list.Any(o => o != list[0]));
        }

        /// <summary>
        /// A test for NextLong.
        ///</summary>
        [Fact]
        public void Random_NextLong()
        {
            Random random0 = RandomEx.Create(1287654);
            Random random1 = RandomEx.Create(1287654);

            Seq.List(random0, random1).Apply(r =>
            {
                Assert.NotNull(r);
                Assert.IsType<Random>(r);
            });

            Util.Repeat(() => Assert.Equal(random0.NextLong(), random1.NextLong()),
                9);

            IEnumerable<long> seq = Seq.Repeat(() => random0.NextLong(),
                random0.Next(10, 20));

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);

            IList<long> list = seq.ToList();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
            Assert.IsType<List<long>>(list);

            // Tests that random data generated varies
            Assert.True(list.Any(o => o != list[0]));
        }

        /// <summary>
        /// A test for NextDecimal.
        ///</summary>
        [Fact]
        public void Random_NextDecimal()
        {
            Random random0 = RandomEx.Create(8745412);
            Random random1 = RandomEx.Create(8745412);

            Seq.List(random0, random1).Apply(r =>
            {
                Assert.NotNull(r);
                Assert.IsType<Random>(r);
            });
            
            Util.Repeat(() => Assert.Equal(
                random0.NextDecimal(), 
                random1.NextDecimal()),
                9);

            IEnumerable<decimal> seq = Seq.Repeat(() => random0.NextDecimal(),
                random0.Next(10, 20));

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);

            IList<decimal> list = seq.ToList();

            Assert.NotNull(list);
            Assert.NotEmpty(list);
            Assert.IsType<List<decimal>>(list);

            // Tests that succeeding calls to NextShort generates random data            
            Assert.True(list.Any(o => o != list[0]));
        }
       
        /// <summary>
        /// A test for NextString.
        ///</summary>
        [Fact]
        public void Random_NextString()
        {
            Random random = RandomEx.Create(241527863);
            string alphaNumericChars =
                "0123456789abcdefghijklmnopqrstuvqxyzABCDEFGHIJKLMNOPQRSTUVQXYZ";
            string s = random.NextString(512, 1024);

            Assert.NotNull(random);
            Assert.IsType<Random>(random);
            
            Assert.NotNull(s);
            Assert.IsType<string>(s);
            Assert.NotEmpty(s);
            s.Should().NotBeNullOrWhiteSpace();
            Assert.True(s.Length >= 512);
            Assert.True(s.Length < 1024);
            s.Apply(c => Assert.True(char.IsLetterOrDigit(c)));

            s.ToCharArray().Should().BeSubsetOf(alphaNumericChars);

            NextStringTest(random, StringComposition.LowercaseLetter)
                .ToCharArray()
                .Should()
                .BeSubsetOf("abcdefghijklmnopqrstuvqxyz");

            NextStringTest(random, StringComposition.UppercaseLetter)
                .ToCharArray()
                .Should()
                .BeSubsetOf("ABCDEFGHIJKLMNOPQRSTUVQXYZ");

            NextStringTest(
                random, 
                StringComposition.WhiteSpace,
                c => char.IsWhiteSpace(c))
                    .Should()
                    .Contain(" ");

            NextStringTest(random, StringComposition.Digit)
                .ToCharArray()
                .Should()
                .BeSubsetOf("0123456789");

            NextStringTest(random, StringComposition.Symbol)
               .ToCharArray()
               .Should()
               .BeSubsetOf("$+<=>^`|~");

            NextStringTest(random, StringComposition.PunctuationMark)
               .ToCharArray()
               .Should()
               .BeSubsetOf(@"!""#%&'()*,-./:;?@[\]{}");

            NextStringTest(random, StringComposition.Letter)
                  .ToCharArray()
                  .Should()
                  .BeSubsetOf("abcdefghijklmnopqrstuvqxyzABCDEFGHIJKLMNOPQRSTUVQXYZ");

            NextStringTest(random, StringComposition.AlphaNumeric)
                    .ToCharArray()
                    .Should()
                    .BeSubsetOf(alphaNumericChars);

            NextStringTest(random, StringComposition.AlphaNumericWhiteSpace)
                    .ToCharArray()
                    .Should()
                    .BeSubsetOf(alphaNumericChars + " ");
        }

        /// <summary>
        /// A test for NextStringWithMinMax.
        ///</summary>
        [Fact]
        public void Random_NextStringWithMinMax()
        {
            Random random = RandomEx.Create(452278);

            Assert.NotNull(random);
            Assert.IsType<Random>(random);

            //Tests default StringComposition which is AlphaNumeric
            string s = random.NextString(512, 1024);

            Assert.NotNull(s);
            Assert.IsType<string>(s);
            Assert.NotEmpty(s);
            s.Should().NotBeNullOrWhiteSpace();

            Assert.True(s.Length >= 512);
            Assert.True(s.Length < 1024);

            s.Should().MatchRegex("^[a-zA-Z0-9]+$");

            //Tests other StringCompositions
            NextStringMinMaxTest(random, StringComposition.LowercaseLetter)
                .Should().MatchRegex("^[a-z]+$");

            NextStringMinMaxTest(random, StringComposition.UppercaseLetter)
                .Should().MatchRegex("^[A-Z]+$");

            NextStringMinMaxTest(random, StringComposition.Digit)
                .Should().MatchRegex("^[0-9]+$");

            NextStringMinMaxTest(random, StringComposition.Symbol)
                .Should().MatchRegex(@"^[\$\+<=>\^`\|~]+$");

            NextStringMinMaxTest(random, StringComposition.PunctuationMark)
                .Should().MatchRegex(@"^[!""#%&'\(\)\*,\-\./:;\?@\[\\\]\{}]+$");

            NextStringMinMaxTest(random, StringComposition.WhiteSpace)
                .Should().MatchRegex(@"^[\s]+$");

            NextStringMinMaxTest(random, StringComposition.Letter)
                .Should().MatchRegex(@"^[a-zA-Z]+$");

            NextStringMinMaxTest(random, StringComposition.AlphaNumeric)
                .Should().MatchRegex(@"^[a-zA-Z0-9]+$");

            NextStringMinMaxTest(random, StringComposition.AlphaNumericWhiteSpace)
                .Should().MatchRegex(@"^[a-zA-Z0-9\s]+$");

            NextStringMinMaxTest(random, StringComposition.All)
                .Should().MatchRegex(@"^([a-zA-Z0-9\s]|[\$\+<=>\^`\|~]|" +
                    @"[!""#%&'\(\)\*,\-\./:;\?@\[\\\]\{}])+$");
        }

        /// <summary>
        /// A test for NextChar.
        ///</summary>
        [Fact]
        public void Random_NextChar()
        {
            Random random = RandomEx.Create(765673);

            Assert.NotNull(random);
            Assert.IsType<Random>(random);

            // Tests default StringComposition which is AlphaNumeric.
            IEnumerable<char> seq = Seq.Repeat(
               () => random.NextChar(), random.Next(512, 1024));

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);

            string s = new string(seq.ToArray());

            Assert.NotNull(s);
            Assert.IsType<string>(s);
            Assert.NotEmpty(s);
            Assert.True(s.Length >= 512);
            Assert.True(s.Length < 1024);

            s.Should().MatchRegex("^[a-zA-Z0-9]+$");

            //Tests other StringCompositions
            NextCharTest(random, StringComposition.LowercaseLetter)
                .Should().MatchRegex("^[a-z]+$");

            NextCharTest(random, StringComposition.UppercaseLetter)
                .Should().MatchRegex("^[A-Z]+$");

            NextCharTest(random, StringComposition.Digit)
                .Should().MatchRegex("^[0-9]+$");

            NextCharTest(random, StringComposition.Symbol)
                .Should().MatchRegex(@"^[\$\+<=>\^`\|~]+$");

            NextCharTest(random, StringComposition.PunctuationMark)
                .Should().MatchRegex(@"^[!""#%&'\(\)\*,\-\./:;\?@\[\\\]\{}]+$");

            NextCharTest(random, StringComposition.WhiteSpace)
                .Should().MatchRegex(@"^[\s]+$");

            NextCharTest(random, StringComposition.Letter)
                .Should().MatchRegex(@"^[a-zA-Z]+$");

            NextCharTest(random, StringComposition.AlphaNumeric)
                .Should().MatchRegex(@"^[a-zA-Z0-9]+$");

            NextCharTest(random, StringComposition.AlphaNumericWhiteSpace)
                .Should().MatchRegex(@"^[a-zA-Z0-9\s]+$");

            NextCharTest(random, StringComposition.All)
                .Should().MatchRegex(@"^([a-zA-Z0-9\s]|[\$\+<=>\^`\|~]|" +
                    @"[!""#%&'\(\)\*,\-\./:;\?@\[\\\]\{}])+$");
        }       

        #endregion
    }
}