using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentAssertions;

using Xunit;

namespace Magenic.SharedKernel.Tests
{
    /// <summary>
    /// This class implements unit tests for StringEx utility class.
    /// </summary>
    public class StringTests
    {
        #region Public Methods

        /// <summary>
        /// A test for Contains.
        ///</summary>
        [Fact]
        public void String_Contains()
        {
            string source = "1234ABE56789";
            string value = "Abe";

            Seq.List(source, value).Apply(AssertEx.NotNullOrWhiteSpaceOfType);

            Assert.False(source.Contains(
                value,
                StringComparison.CurrentCulture));
            Assert.True(source.Contains(
                value,
                StringComparison.CurrentCultureIgnoreCase));
            Assert.False(value.Contains(
                source,
                StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// A test for CreateSB.
        ///</summary>
        [Fact]
        public void String_CreateSB()
        {
            Random random = PseudoRandom.Create(1687561716);

            AssertEx.NotNullOfType<Random>(random);

            string s = random.NextString(12);

            AssertEx.NotNullOrWhiteSpaceOfType(s);

            StringBuilder sb0 = StringEx.CreateSB();
            StringBuilder sb1 = StringEx.CreateSB(s);

            Seq.List(sb0, sb1).Apply(x =>
            {
                Assert.NotNull(x);
                Assert.IsType<StringBuilder>(x);
                Assert.NotNull(x.ToString());
            });

            Assert.Empty(sb0.ToString());
            sb0.ToString().Should().BeNullOrEmpty();

            AssertEx.NotNullOrWhiteSpaceOfType(sb1.ToString());
            Assert.Equal(s, sb1.ToString());
        }

        /// <summary>
        /// A test for CreateString.
        ///</summary>
        [Fact]
        public void String_CreateString()
        {
            Random random = PseudoRandom.Create(1001336863);

            AssertEx.NotNullOfType<Random>(random);

            char c = random.NextChar(StringComposition.Letter);
            int count = random.Next(16, 32);

            Assert.IsType<char>(c);
            Assert.IsType<int>(count);
            count.Should().BeGreaterOrEqualTo(16);
            count.Should().BeLessThan(32);

            string s = StringEx.CreateString(c, count);

            AssertEx.NotNullOrEmptyOfType<string>(s);
            s.Should().NotBeNullOrWhiteSpace();

            Assert.Equal(ColEx.Repeat(() => c, count).Join(string.Empty), s);
            Assert.Equal(new string(c, count), s);
        }

        /// <summary>
        /// A test for IsEmpty.
        ///</summary>
        [Fact]
        public void String_IsEmpty()
        {
            string nullString = null;
            string emptyString = string.Empty;

            Assert.Null(nullString);
            Assert.NotNull(emptyString);
            Assert.IsType<string>(emptyString);
            Assert.Empty(emptyString);
            emptyString.Should().BeNullOrEmpty();
            Assert.True(emptyString.IsEmpty());
            Assert.True(string.Empty.IsEmpty());
            Assert.False(" ".IsEmpty());
            Assert.False("av54".IsEmpty());
            Assert.True(string.IsNullOrEmpty(nullString));
            Assert.Throws<NullReferenceException>(() => nullString.IsEmpty());
        }

        /// <summary>
        /// A test for StringEx.IsNullOrEmpty and StringEx.IsNullOrWhiteSpace.
        ///</summary>
        [Fact]
        public void String_IsNullOrEmpty_IsNullOrWhiteSpace()
        {
            Random random = PseudoRandom.Create(526804642);

            AssertEx.NotNullOfType<Random>(random);

            string nullString = null;
            string emptyString = string.Empty;
            string whiteSpaceString = random.NextString(4, StringComposition.WhiteSpace);
            string alphaNumericString = random
                .NextString(8, StringComposition.AlphaNumeric);

            Assert.Null(nullString);
            Seq.List(emptyString, whiteSpaceString).Apply(Assert.NotNull);
            Assert.Empty(emptyString);
            Assert.NotEmpty(whiteSpaceString);
            emptyString.Should().BeNullOrEmpty();
            whiteSpaceString.Should().NotBeNullOrEmpty();
            whiteSpaceString.Should().BeNullOrWhiteSpace();
            Seq.List(whiteSpaceString, alphaNumericString)
                .Apply(s => AssertEx.NotNullOrEmptyOfType<string>(s));
            AssertEx.NotNullOrWhiteSpaceOfType(alphaNumericString);

            IEnumerable<string> seq0 = null;
            IEnumerable<string> seq1 = Enumerable.Empty<string>();
            IEnumerable<string> seq2 = Seq.List(emptyString);
            IEnumerable<string> seq3 = Seq.List(whiteSpaceString);
            IEnumerable<string> seq4 = Seq.List(emptyString, whiteSpaceString);
            IEnumerable<string> seq5 = Seq.List(alphaNumericString);

            Assert.Null(seq0);
            Assert.Empty(seq1);
            Seq.List(seq1, seq2, seq3, seq4, seq5).Apply(Assert.NotNull);
            Seq.List(seq2, seq3, seq4, seq5).Apply(Assert.NotEmpty);

            Assert.All(
                Seq.List(seq0, seq1, seq2).Map(StringEx.IsNullOrEmpty),
                Assert.True);
            Assert.All(
                Seq.List(seq0, seq1, seq2, seq3, seq4).Map(StringEx.IsNullOrWhiteSpace),
                Assert.True);
            Assert.All(
                Seq.List(seq3, seq4, seq5).Map(StringEx.IsNullOrEmpty),
                Assert.False);
            Assert.False(StringEx.IsNullOrWhiteSpace(seq5));
        }

        /// <summary>
        /// A test for Prepend.
        ///</summary>
        [Fact]
        public void String_Prepend()
        {
            Random random = PseudoRandom.Create(450441753);

            AssertEx.NotNullOfType<Random>(random);

            string s0 = random.NextString(8);
            string s1 = random.NextString(16);
            string p = s1.Prepend(s0);

            Seq.List(s0, s1, p).Apply(AssertEx.NotNullOrWhiteSpaceOfType);

            Assert.Equal(string.Format("{0}{1}", s0, s1), p);
            Assert.Equal($"{s0}{s1}", p);
        }

        /// <summary>
        /// A test for TrimIfSet.
        ///</summary>
        [Fact]
        public void String_TrimIfSet()
        {
            Random random = PseudoRandom.Create(645439842);

            AssertEx.NotNullOfType<Random>(random);

            string nullString = null;
            string emptyString = string.Empty;
            string whiteSpace = string.Format(
                "{0}{0}{1}{0}{2}{0}{3}{0}{4}{0}{0}",
                " ", "\t", "\r", "\n", Environment.NewLine);
            string source = random.NextString(32);

            Assert.Null(nullString);

            Seq.List(emptyString, whiteSpace, source).Apply(s =>
            {
                Assert.NotNull(s);
                Assert.IsType<string>(s);
            });

            Assert.Empty(emptyString);
            emptyString.Should().BeNullOrEmpty();

            Assert.NotEmpty(whiteSpace);
            whiteSpace.Should().BeNullOrWhiteSpace();

            AssertEx.NotNullOrWhiteSpaceOfType(source);

            Assert.Null(nullString.TrimIfSet());
            Assert.Equal(null, nullString.TrimIfSet());
            Assert.Empty(emptyString.TrimIfSet());
            emptyString.TrimIfSet().Should().BeNullOrEmpty();
            Assert.Equal(string.Empty, emptyString.TrimIfSet());
            Assert.Empty(whiteSpace.TrimIfSet());
            whiteSpace.TrimIfSet().Should().BeNullOrEmpty();
            Assert.Equal(string.Empty, whiteSpace.TrimIfSet());
            Assert.Equal(
                source,
                string.Format("{0}{1}{0}", whiteSpace, source).TrimIfSet());
            Assert.Equal(
                source,
                $"{whiteSpace}{source}{whiteSpace}".TrimIfSet());
        }

        /// <summary>
        /// A test for Display.
        ///</summary>
        [Fact]
        public void String_Display()
        {
            string nullString = null;
            string emptyString = string.Empty;
            string whiteSpace = "    \t \r \n \t   \t \t\t\t\n\r\r\r\n\n\t      ";
            IEnumerable<string> seq = Seq.List(
                "Dog ate my homework.",
                "0",
                "_",
                "m",
                "?",
                " q",
                "! ",
                "123456789",
                "I*8o0)-+.,~        [p]}|     ");

            Assert.Null(nullString);

            Seq.List(emptyString, whiteSpace).Apply(s =>
            {
                Assert.NotNull(s);
                Assert.IsType<string>(s);
            });

            Assert.Empty(emptyString);
            emptyString.Should().BeNullOrEmpty();

            AssertEx.NotNullOrEmptyOfType<string>(whiteSpace);

            AssertEx.NotNullOrEmpty(seq);
            AssertEx.NotNullOrEmpty(seq.Map(s => s.Display()));

            seq.Map(s => s.Display()).Should().Equal(seq);

            Seq.List(nullString, null)
                .Apply(s => Assert.Equal(StringEx.NULL, s.Display()));
            Seq.List(emptyString, string.Empty)
                .Apply(s => Assert.Equal(StringEx.EMPTY, s.Display()));
            Seq.List(
                " ",
                "\t",
                Environment.NewLine,
                "\n",
                "\r",
                StringEx.CreateString('\t', 10),
                StringEx.CreateString('\n', 20),
                StringEx.CreateString(' ', 100),
                whiteSpace)
                    .Apply(s => Assert.Equal(StringEx.WHITE_SPACE, s.Display()));
        }

        #endregion
    }
}