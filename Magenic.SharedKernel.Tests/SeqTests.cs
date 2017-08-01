using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentAssertions;

using Xunit;

namespace Magenic.SharedKernel.Tests
{
    /// <summary>
    /// This class contains unit tests for Shared Kernel Seq class.
    /// </summary>
    public class SeqTests
    {
        #region Private Static Methods

        /// <summary>
        /// Composes a sequence of substrings of length out of passed in value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="length">Length.</param>
        /// <example>
        /// ComposeSubstrings("Andrew", 4) returns this sequence: { "Andr", "ndre", "drew" }.
        /// </example>
        /// <returns>String sequence.</returns>
        private static IEnumerable<string> ComposeSubstrings(string value, int length)
        {
            return Seq.Repeat(
                i => value.Substring(i, length), value.Length - length + 1);
        }

        #endregion

        #region Public Instance Methods

        /// <summary>
        /// A test for RandomRef.
        ///</summary>
        [Fact]
        public void Seq_RandomRef()
        {
            Random random = PseudoRandom.Create(1957787834);

            Assert.NotNull(random);

            int count = random.Next(64, 128);

            Assert.True(count > 0);
            Assert.True(count >= 64);
            Assert.True(count < 128);

            IEnumerable<int> seq = Seq.List(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);

            Util.Repeat(() => seq.Should().Contain(seq.RandomRef(random)), count);
        }

        /// <summary>
        /// A test for Map.
        ///</summary>
        [Fact]
        public void Seq_Map()
        {
            Random random = PseudoRandom.Create(178221420);

            Assert.NotNull(random);

            int count = random.Next(16, 32);

            Assert.True(count > 0);
            Assert.True(count >= 16);
            Assert.True(count < 32);

            IList<int> lengthsList = ColEx
                .Repeat(() => random.Next(128, 256), count);

            Assert.NotNull(lengthsList);
            Assert.NotEmpty(lengthsList);
            Assert.IsType<List<int>>(lengthsList);

            IEnumerable<string> stringsSeq = lengthsList
                .Map(l => random.NextString(l));

            Assert.NotNull(stringsSeq);
            Assert.NotEmpty(stringsSeq);

            IList<string> list0 = ColEx
                .Repeat(() => random.NextString(128, 256), count);

            Assert.NotNull(list0);
            Assert.NotEmpty(list0);
            Assert.IsType<List<string>>(list0);

            IList<string> list1 = ColEx
                .Repeat(() => random.NextString(128, 256), count);

            Assert.NotNull(list1);
            Assert.NotEmpty(list1);
            Assert.IsType<List<string>>(list1);

            IList<string> list2 = ColEx
                .Repeat(() => random.NextString(128, 256), count);

            Assert.NotNull(list2);
            Assert.NotEmpty(list2);
            Assert.IsType<List<string>>(list2);

            IList<string> list3 = ColEx
                .Repeat(() => random.NextString(128, 256), count);

            Assert.NotNull(list3);
            Assert.NotEmpty(list3);
            Assert.IsType<List<string>>(list3);

            IEnumerable<string> map01 = Seq.Map(
                list0,
                list1,
                (s0, s1) => $"{s0}{s1}");

            Assert.NotNull(map01);
            Assert.NotEmpty(map01);

            IEnumerable<string> map012 = Seq.Map(
                list0,
                list1,
                list2,
                (s0, s1, s2) => $"{s0}{s1}{s2}");

            Assert.NotNull(map012);
            Assert.NotEmpty(map012);

            IEnumerable<string> map0123 = Seq.Map(
                list0,
                list1,
                list2,
                list3,
                (s0, s1, s2, s3) => $"{s0}{s1}{s2}{s3}");

            Assert.NotNull(map0123);
            Assert.NotEmpty(map0123);

            IEnumerable<int> stringsLen = stringsSeq.Map(s => s.Length);

            Assert.NotNull(stringsLen);
            Assert.NotEmpty(stringsLen);

            lengthsList.ShouldBeEquivalentTo(stringsLen);
            stringsSeq.Select(s => s.Length).ShouldBeEquivalentTo(stringsLen);

            map01.Should().Equal(
                list0.Zip(list1, (s0, s1) => $"{s0}{s1}"));
            map012.Should().Equal(
                map01.Zip(list2, (s01, s2) => $"{s01}{s2}"));
            map0123.Should().Equal(
                map012.Zip(list3, (s012, s3) => $"{s012}{s3}"));
        }

        /// <summary>
        /// A test for FlatMap.
        ///</summary>
        [Fact]
        public void Seq_FlatMap()
        {
            int length = 2;
            string s0 = "1234";
            IEnumerable<string> seq0 = Seq.List("12", "23", "34");
            string s1 = "ABC";
            IEnumerable<string> seq1 = Seq.List("AB", "BC");

            Seq.List(seq0, seq1).Apply(seq =>
            {
                Assert.NotNull(seq);
                Assert.NotEmpty(seq);
            });

            Seq.List(s0, s1).Apply(s =>
            {
                Assert.NotNull(s);
                s.Should().NotBeNullOrWhiteSpace();
            });

            seq0.Should().Equal(ComposeSubstrings(s0, length));
            seq1.Should().Equal(ComposeSubstrings(s1, length));
            seq0.Append(seq1).Should().Equal(
                Seq.List(s0, s1).FlatMap(s => ComposeSubstrings(s, length)));
        }

        /// <summary>
        /// A test for List.
        ///</summary>
        [Fact]
        public void Seq_List()
        {
            IEnumerable<string> seq = Seq.List("pouih0987", "H(*OUIh", ")K<");

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);

            List<string> list = new List<string>
            {
                "pouih0987", "H(*OUIh", ")K<"
            };

            Assert.NotNull(list);
            Assert.NotEmpty(list);
            Assert.IsType<List<string>>(list);

            seq.Should().Equal(list.AsEnumerable());
            seq.Should().Equal(list);
            seq.Should().Equal(list.ToArray());

            list.Should().Equal(seq);
            list.Should().Equal(seq.ToArray());
            list.Should().Equal(seq.ToList());

            Assert.Equal(list.Count, seq.Count());
            Assert.Equal(list[0], seq.First());
            Assert.Equal(list[1], seq.ElementAt(1));
            Assert.Equal(list[2], seq.Last());
            Assert.Equal("pouih0987", seq.First());
            Assert.Equal("H(*OUIh", seq.ElementAt(1));
            Assert.Equal(")K<", seq.Last());

            Enumerable.Repeat(76, 4).Should().Equal(Seq.List(76, 76, 76, 76));
            (new int[] {56, 123, 987, 2}).Should().Equal(
                Seq.List(56, 123, 987, 2));
            Array.Empty<int>().Should().Equal(Seq.List<int>());
            ColEx.CreateList<long>().Should().Equal(Seq.List<long>());
            Enumerable.Empty<char>().Should().Equal(Seq.List<char>());
        }

        /// <summary>
        /// A test for JoinWithOr.
        ///</summary>
        [Fact]
        public void Seq_JoinWithOr()
        {
            Assert.Equal("23.56.", Seq.List(23.56).JoinWithOr());
            Assert.Equal("This.", Seq.List("This.").JoinWithOr());
            Assert.Equal("1, 2 or 3.", Seq.List(1, 2, 3).JoinWithOr());
            Assert.Equal("A or B.", Seq.List('A', 'B').JoinWithOr());
            Assert.Equal(
                "One, two, three or four.",
                Seq.List("One", "two", "three", "four").JoinWithOr());
        }

        /// <summary>
        /// A test for JoinWithConjunction.
        ///</summary>
        [Fact]
        public void Seq_JoinWithConjunction()
        {
            Assert.Equal(
                "One, two, three but four.",
                Seq.List("One", "two", "three", "four")
                    .JoinWithConjunction("   but "));
        }

        /// <summary>
        /// A test for JoinWithAnd.
        ///</summary>
        [Fact]
        public void Seq_JoinWithAnd()
        {
            Assert.Equal(
                "One, two, three and four.",
                Seq.List("One", "two", "three", "four").JoinWithAnd());
        }

        /// <summary>
        /// A test for Join.
        ///</summary>
        [Fact]
        public void Seq_Join()
        {
            Assert.Equal(
                string.Format("One{0}two{0}three{0}four.", Environment.NewLine),
                Seq.List("One", "two", "three", "four.").Join(Environment.NewLine));
        }

        /// <summary>
        /// A test for IsSubsetOf.
        ///</summary>
        [Fact]
        public void Seq_IsSubsetOf()
        {
            Assert.True(Seq.IsSubsetOf(
                Seq.List(3, 7),
                Seq.List(1, 2, 3, 4, 5, 6, 7, 8, 9)));
            Assert.False(Seq.IsSubsetOf(
                Seq.List(3, 7, 10),
                Seq.List(1, 2, 3, 4, 5, 6, 7, 8, 9)));
        }

        /// <summary>
        /// A test for IsNullOrEmpty.
        ///</summary>
        [Fact]
        public void Seq_IsNullOrEmpty()
        {
            IEnumerable<int> seq0 = null;
            IEnumerable<long> seq1 = Enumerable.Empty<long>();
            IEnumerable<decimal> seq2 = Seq.List(300.5m);
            IEnumerable<string> seq3 = Seq.List(
                "vavasd",
                "09jp0nhi",
                "wet890hjtw0hji",
                "copkopasc");

            Assert.True(Seq.IsNullOrEmpty(seq0));
            Assert.True(Seq.IsNullOrEmpty(seq1));
            Assert.False(Seq.IsNullOrEmpty(seq2));
            Assert.False(Seq.IsNullOrEmpty(seq3));

            Assert.Null(seq0);
            Assert.NotNull(seq1);
            seq1.Should().BeEmpty();
            Assert.NotNull(seq2);
            seq2.Should().NotBeEmpty();
            Assert.NotNull(seq3);
            seq3.Should().NotBeEmpty();
        }

        /// <summary>
        /// A test for IsEmpty.
        ///</summary>
        [Fact]
        public void Seq_IsEmpty()
        {
            IEnumerable<int> seq0 = Enumerable.Empty<int>();
            IEnumerable<long> seq1 = Seq.List(464814684L);
            IEnumerable<string> seq2 = Seq.List("gsev", "3614asvd", "40w38vth");

            Assert.True(Seq.IsEmpty(seq0));
            Assert.False(Seq.IsEmpty(seq1));
            Assert.False(Seq.IsEmpty(seq2));

            seq0.Should().BeEmpty();
            seq1.Should().NotBeEmpty();
            seq2.Should().NotBeEmpty();
        }

        /// <summary>
        /// A test for Apply using int sequences.
        ///</summary>
        [Fact]
        public void Seq_ApplyInt()
        {
            IEnumerable<int> seq0 = Seq.List(1, 2, 3);
            IEnumerable<int> seq1 = Seq.List(4, 5, 6);
            IEnumerable<int> seq2 = Seq.List(7, 8, 9);
            IEnumerable<int> seq3 = Seq.List(10, 11, 12);
            int apply0 = 0;
            int apply01 = 0;
            int apply012 = 0;
            int apply0123 = 0;

            Seq.List(seq0, seq1, seq2, seq3).Apply(seq =>
            {
                Assert.NotNull(seq);
                Assert.NotEmpty(seq);
                Assert.Equal(3, seq.Count());
            });

            seq0.Apply(i => apply0 += i);
            Assert.Equal(6, apply0);

            Seq.Apply(seq0, seq1, (i, j) => apply01 += i * j);
            Assert.Equal(32, apply01);

            Seq.Apply(seq0, seq1, seq2, (i, j, k) => apply012 += i * j + k);
            Assert.Equal(56, apply012);

            Seq.Apply(
                seq0,
                seq1,
                seq2,
                seq3,
                (i, j, k, l) => apply0123 += i * j + k - l);
            Assert.Equal(23, apply0123);
        }

        /// <summary>
        /// A test for Apply using string sequences.
        ///</summary>
        [Fact]
        public void Seq_ApplyString()
        {
            IEnumerable<string> seq0 = Seq.List("<H>", "258", "Po9e56y");
            IEnumerable<string> seq1 = Seq.List("GD", "T67", "89_");
            IEnumerable<string> seq2 = Seq.List("3S", "5o+~Q", "M");
            IEnumerable<string> seq3 = Seq.List("poi76uhb", "3", "65X");
            StringBuilder sb = StringEx.CreateSB();

            Seq.List(seq0, seq1, seq2, seq3).Apply(seq =>
            {
                Assert.NotNull(seq);
                Assert.NotEmpty(seq);
                Assert.Equal(3, seq.Count());
            });

            Assert.NotNull(sb);
            sb.ToString().Should().BeEmpty();
            seq0.Apply(s => sb.Append(s));
            sb.ToString().Should().NotBeNullOrWhiteSpace();
            Assert.Equal("<H>258Po9e56y", sb.ToString());

            Assert.NotNull(sb);
            sb.Clear();
            sb.ToString().Should().BeEmpty();
            Seq.Apply(seq0, seq1, (a, b) => sb.AppendFormat("{0}{1}", a, b));
            sb.ToString().Should().NotBeNullOrWhiteSpace();
            Assert.Equal("<H>GD258T67Po9e56y89_", sb.ToString());

            Assert.NotNull(sb);
            sb.Clear();
            sb.ToString().Should().BeEmpty();
            Seq.Apply(
                seq0,
                seq1,
                seq2,
                (a, b, c) => sb.AppendFormat("{0}{1}{2}", a, b, c));
            sb.ToString().Should().NotBeNullOrWhiteSpace();
            Assert.Equal("<H>GD3S258T675o+~QPo9e56y89_M", sb.ToString());

            Assert.NotNull(sb);
            sb.Clear();
            sb.ToString().Should().BeEmpty();
            Seq.Apply(
                seq0,
                seq1,
                seq2,
                seq3,
                (a, b, c, d) => sb.AppendFormat("{0}{1}{2}{3}", a, b, c, d));
            sb.ToString().Should().NotBeNullOrWhiteSpace();
            Assert.Equal(
                "<H>GD3Spoi76uhb258T675o+~Q3Po9e56y89_M65X",
                sb.ToString());
        }

        /// <summary>
        /// A test for Apply using jagged sequences.
        ///</summary>
        ///<remarks>
        /// Purpose of this unit test is to verify that Apply stops at the shortest sequence.
        ///</remarks>
        [Fact]
        public void Seq_ApplyJagged()
        {
            IEnumerable<decimal> seq0 = Seq.List(1.2m, 2.3m, 3.4m);
            IEnumerable<decimal> seq1 = Seq.List(4.5m, 5.6m);
            IEnumerable<decimal> seq2 = Seq.List(6.5m);
            IEnumerable<decimal> seq3 = Enumerable.Empty<decimal>();
            decimal apply0 = 0m;
            decimal apply01 = 0m;
            decimal apply012 = 0m;
            decimal apply0123 = 0m;

            Seq.List(seq0, seq1, seq2, seq3).Apply(Assert.NotNull);

            seq0.Apply(i => apply0 += i);
            Assert.Equal(6.9m, apply0);

            Seq.Apply(seq0, seq1, (i, j) => apply01 += i * j);
            Assert.Equal(18.28m, apply01);

            Seq.Apply(seq0, seq1, seq2, (i, j, k) => apply012 += i * j + k);
            Assert.Equal(11.9m, apply012);

            Seq.Apply(
                seq0,
                seq1,
                seq2,
                seq3,
                (i, j, k, l) => apply0123 += i * j + k * l);
            Assert.Equal(0m, apply0123);
        }

        /// <summary>
        /// A test for AppendFormat.
        ///</summary>
        [Fact]
        public void Seq_AppendFormat()
        {
            string format = "{0}{1}{2}{3}.";
            Random random = PseudoRandom.Create(548818888);
            IList<string> list = ColEx.Repeat(() => random.NextString(4, 16), 5);
            string s0 = string.Format(
                format, list[0], list[1], list[2], list[3]);
            string s1 = string.Format(
                format, list[1], list[3], list[2], list[0]);
            IEnumerable<string> seq = Enumerable.Empty<string>().AppendFormat(
                format,
                list[0],
                list[1],
                list[2],
                list[3]);

            Assert.Equal(s0, seq.Single());

            seq = seq.AppendFormat(format, list[1], list[3], list[2], list[0]);

            Seq.List(s0, s1).Should().Equal(seq);
            Seq.List(s0, s1, list[4]).Should().Equal(
                seq.AppendFormat("{0}", list[4]));
        }

        /// <summary>
        /// A test for Append.
        ///</summary>
        [Fact]
        public void Seq_Append()
        {
            IEnumerable<string> seq = Enumerable.Empty<string>();
            Random random = PseudoRandom.Create(827400392);
            IList<string> list0 = ColEx.Repeat(() => random.NextString(4, 8), 3);
            IList<string> list1 = ColEx.Repeat(() => random.NextString(9, 16), 4);
            List<string> combo = ColEx.CreateList(list0) as List<string>;

            combo.AddRange(list1);

            Seq.List(list0[0]).Should().Equal(seq.Append(list0[0]));
            Seq.List(list0[0], list0[1]).Should().Equal(
                seq.Append(list0[0]).Append(list0[1]));
            list0.Should().Equal(
                seq.Append(list0[0]).Append(list0[1]).Append(list0[2]));
            list0.Should().Equal(seq.Append(list0));
            combo.Should().Equal(seq.Append(list0).Append(list1));

            combo.AddRange(list0);
            combo.AddRange(list1);
            combo.Add(list0[2]);

            combo.Should().Equal(seq
                .Append(list0, list1, list0)
                .Append(list1)
                .Append(list0[2]));
            combo.Should().Equal(Seq
                .Append(list0, list1, seq, list0)
                .Append(list1).Append(list0[2]));
        }

        /// <summary>
        /// A test for Prepend.
        ///</summary>
        [Fact]
        public void Seq_Prepend()
        {
            Seq.List(0, 1, 2, 3).Should().Equal(Seq.List(1, 2, 3).Prepend(0));
            Seq.List("One").Should().Equal(
                Enumerable.Empty<string>().Prepend("One"));
            Seq.List('A', 'B', 'C', 'D', 'E').Should().Equal(
                Seq.List('C', 'D', 'E').Prepend('B').Prepend('A'));
        }

        /// <summary>
        /// A test for Append and Prepend.
        ///</summary>
        [Fact]
        public void Seq_AppendPrepend()
        {
            Seq.List(0, 1, 2, 3, 4, 5, 6, 7, 8, 9).Should().Equal(
                Seq.List(4, 5).Prepend(3).Append(6).Prepend(2).Append(
                    Seq.List(7, 8, 9)).Prepend(1).Prepend(0));
        }

        /// <summary>
        /// A test for Repeat.
        ///</summary>
        [Fact]
        public void Seq_Repeat()
        {
            Random random0 = PseudoRandom.Create(380572830);
            IEnumerable<string> seq = Seq
                .Repeat(() => random0.NextString(8, 16), random0.Next(128, 256));
            Random random1 = PseudoRandom.Create(380572830);
            int count = random1.Next(128, 256);
            IList<string> list = ColEx.CreateList<string>();

            for (int i = 0; i < count; i++)
            {
                list.Add(random1.NextString(8, 16));
            }

            list.Should().Equal(seq);

            list.Clear();

            for (int i = 0; i < count; i++)
            {
                list.Add(random1.NextString(i, i + 8));
            }

            seq = Seq.Repeat(i => random0.NextString(i, i + 8), count);

            list.Should().Equal(seq);
        }

        /// <summary>
        /// A test for SequenceEquivalent.
        ///</summary>
        [Fact]
        public void Seq_SequenceEquivalent()
        {
            Seq.List(0, 1, 2, 3, 4).ShouldBeEquivalentTo(Seq.List(4, 3, 2, 1, 0));
            Assert.True(Seq.SequenceEquivalent(
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>()));
            Assert.True(Seq.SequenceEquivalent(
                Seq.List('A', 'B', 'C'),
                Seq.List('B', 'A', 'C')));
        }

        /// <summary>
        /// A test for Flatten.
        ///</summary>
        [Fact]
        public void Seq_Flatten()
        {
            IEnumerable<int> seq0 = Seq.List(0, 1, 2, 3, 4);
            IEnumerable<int> seq1 = Seq.List(5);
            IEnumerable<int> seq2 = Enumerable.Empty<int>();
            IEnumerable<int> seq3 = Seq.List(6, 7, 8, 9);
            IEnumerable<int> expected = Seq.List(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            IEnumerable<int> actual = Seq.Flatten(seq0, seq1, seq2, seq3);

            Assert.Equal(expected.Count(), actual.Count());
            expected.Should().Equal(actual);
            expected.Should().Equal(Seq.Flatten(seq0, seq1, seq3));

            seq0.Should().Equal(Seq.Flatten(seq0));
            seq1.Should().Equal(Seq.Flatten(seq1));
            seq2.Should().Equal(Seq.Flatten(seq2));
            seq3.Should().Equal(Seq.Flatten(seq3));
            Seq.List(5, 6, 7, 8, 9).Should().Equal(Seq.Flatten(seq1, seq3));
        }

        /// <summary>
        /// A test for GetTail.
        ///</summary>
        [Fact]
        public void Seq_GetTail()
        {
            IEnumerable<int> seqNull = null;
            IEnumerable<int> seqEmpty = Enumerable.Empty<int>();
            IEnumerable<int> seq12 = Seq.List(1, 2);
            IEnumerable<int> seq3456 = Seq.List(3, 4, 5, 6);
            IEnumerable<int> seq56 = Seq.List(5, 6);
            IEnumerable<string> seqABCDEFG = Seq.List("ABC", "D", "EF", "G");
            IEnumerable<string> seqXYZ = Seq.List("XYZ");
            IEnumerable<string> seqDEFG = Seq.List("D", "EF", "G");

            seq56.Should().Equal(Seq.GetTail(seq12, seq3456));

            seq56.Should().Equal(Seq.GetTail(seq3456, seq12));

            Enumerable.Range(1024, 8).Should().Equal(Seq.GetTail(
                Enumerable.Range(1, 1031),
                Enumerable.Range(1, 1023)));

            seqDEFG.Should().Equal(Seq.GetTail(seqABCDEFG, seqXYZ));

            seqDEFG.Should().Equal(Seq.GetTail(seqXYZ, seqABCDEFG));

            seqEmpty.Should().Equal(Seq.GetTail(seq12, seq56));

            seq12.Should().Equal(Seq.GetTail(seq12, seqEmpty));

            seq3456.Should().Equal(Seq.GetTail(seqEmpty, seq3456));

            seq12.Should().Equal(Seq.GetTail(seq12, seqNull));

            seq3456.Should().Equal(Seq.GetTail(seqNull, seq3456));

            seqEmpty.Should().Equal(Seq.GetTail(seqEmpty, seqEmpty));

            seqEmpty.Should().Equal(Seq.GetTail(seqEmpty, seqNull));

            seqEmpty.Should().Equal(Seq.GetTail(seqNull, seqEmpty));

            seqNull.Should().Equal(Seq.GetTail(seqNull, seqNull));
        }

        /// <summary>
        /// A test for DistinctRetainOrder.
        ///</summary>
        [Fact]
        public void Seq_DistinctRetainOrder()
        {
            char c = 'e';
            IEnumerable<int> seqNull = null;
            IEnumerable<int> seqEmpty = Enumerable.Empty<int>();
            IEnumerable<int> seqInt = Seq.List(
                1, 2, 3, 2, 2, 4, 2, 2, 5, 2, 2, 2, 6, 2, 2, 2, 2, 1,
                2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5,
                2, 2, 3, 3, 3, 7, 2, 8, 2, 2, 2, 3, 9, 2, 2, 10, 4, 5, 2,
                3, 11, 12, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 13, 2,
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 13, 13, 13, 13,
                1, 2, 1, 2, 1, 2, 3, 4, 3, 4, 3, 4, 3, 4, 3, 4, 3, 4, 3, 4,
                5, 6, 7, 8, 5, 6, 7, 8, 5, 6, 7, 8, 9, 10, 11, 12, 13, 8, 8);
            IEnumerable<string> seqString = Seq.List(
                "The", "dog", "barks loudly.");

            Enumerable.Range(1, 13).Should().Equal(
                seqInt.DistinctRetainOrder(EqualityComparer<int>.Default));

            Seq.List(c).Should().Equal(
                Enumerable.Repeat(c, 1024).DistinctRetainOrder(
                    EqualityComparer<char>.Default));

            seqString.Should().Equal(seqString.DistinctRetainOrder(
                EqualityComparer<string>.Default));

            seqEmpty.Should().Equal(
                seqEmpty.DistinctRetainOrder(EqualityComparer<int>.Default));

            Assert.Throws<ArgumentNullException>(() => seqNull.DistinctRetainOrder(
                EqualityComparer<int>.Default).Any());
        }

        /// <summary>
        /// A test for PairwiseMerge.
        ///</summary>
        [Fact]
        public void Seq_PairwiseMerge()
        {
            IEnumerable<int> seqNull = null;
            IEnumerable<int> seqEmpty = Enumerable.Empty<int>();
            IEnumerable<int> seq12 = Seq.List(1, 2);
            IEnumerable<int> seq3456 = Seq.List(3, 4, 5, 6);
            IEnumerable<int> seq56 = Seq.List(5, 6);
            IEnumerable<int> seqRange = Enumerable.Range(1, 1024);
            IEnumerable<string> seqABCDEFG = Seq.List("ABC", "D", "EF", "G");
            IEnumerable<string> seqXYZ = Seq.List("X", "Y", "Z");

            Seq.List(1, 5, 2, 6).Should().Equal(
                Seq.PairwiseMerge(seq12, seq56));

            Seq.List(1, 3, 2, 4, 5, 6).Should().Equal(
                Seq.PairwiseMerge(seq12, seq3456));

            Seq.List(3, 1, 4, 2, 5, 6).Should().Equal(
                Seq.PairwiseMerge(seq3456, seq12));

            Seq.List("ABC", "X", "D", "Y", "EF", "Z", "G").Should().Equal(
                Seq.PairwiseMerge(seqABCDEFG, seqXYZ));

            seqRange.Should().Equal(Seq.PairwiseMerge(
                seqRange.Where(i => i % 2 == 1),
                seqRange.Where(i => i % 2 == 0)));

            seq56.Should().Equal(Seq.PairwiseMerge(seqEmpty, seq56));

            seq3456.Should().Equal(Seq.PairwiseMerge(seq3456, seqEmpty));

            seqEmpty.Should().Equal(Seq.PairwiseMerge(seqEmpty, seqEmpty));

            seqEmpty.Should().Equal(Seq.PairwiseMerge(seqEmpty, seqNull));

            seqEmpty.Should().Equal(Seq.PairwiseMerge(seqNull, seqEmpty));

            seqNull.Should().Equal(Seq.PairwiseMerge(seqNull, seqNull));
        }

        #endregion
    }
}
