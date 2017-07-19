using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using FluentAssertions;

using Xunit;

namespace Magenic.SharedKernel.Tests
{
    /// <summary>
    /// This class implements unit tests for Flow utility class.
    /// </summary>
    public class FlowTests
    {
        #region Private Static Methods

        private static void AssertDisposedObject(
            Action testMethod,
            string exceptionMessage = "Cannot access a closed Stream.")
        {
            Exception ex = Assert.Throws<ObjectDisposedException>(testMethod);

            Assert.Equal(exceptionMessage, ex.Message);
        }

        #endregion

        #region Public Instance Methods

        /// <summary>
        /// If Not Null test, first overload.
        /// </summary>
        [Fact]
        public void Flow_IfNotNull_1()
        {
            // Basic flow - happy path.
            IEnumerable<int> seq = Enumerable.Range(1, 9);
            bool seqIsNull = false;
            int first = 0;

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);
            Assert.IsType<bool>(seqIsNull);
            Assert.Equal(false, seqIsNull);
            Assert.IsType<int>(first);
            Assert.Equal(0, first);

            seq.IfNotNull(x => first = x.First(), () => seqIsNull = true);

            Assert.IsType<int>(first);
            Assert.Equal(1, first);
            Assert.Equal(seq.First(), first);
            Assert.Equal(false, seqIsNull);

            // Alternate flow - unhappy path.
            seq = null;
            seqIsNull = false;
            first = 0;

            Assert.Null(seq);
            Assert.IsType<bool>(seqIsNull);
            Assert.Equal(false, seqIsNull);
            Assert.IsType<int>(first);
            Assert.Equal(0, first);

            seq.IfNotNull(x => first = x.First(), () => seqIsNull = true);

            Assert.Null(seq);
            Assert.Equal(0, first);
            Assert.Equal(true, seqIsNull);
        }

        /// <summary>
        /// If Not Null test, second overload.
        /// </summary>
        [Fact]
        public void Flow_IfNotNull_2()
        {
            IEnumerable<int> seq = Enumerable.Range(1, 9);
            int first = 0;

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);
            Assert.IsType<int>(first);
            Assert.Equal(0, first);

            seq.IfNotNull(x => first = x.First());

            Assert.IsType<int>(first);
            Assert.Equal(1, first);
            Assert.Equal(seq.First(), first);
        }

        /// <summary>
        /// If Not Null test, third overload.
        /// </summary>
        [Fact]
        public void Flow_IfNotNull_3()
        {
            // Basic flow - happy path.
            IEnumerable<int> seq = Enumerable.Range(2, 8);
            int first = 0;
            int multiplier = 5;

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);

            Seq.Apply(
                Seq.List(first, multiplier),
                Seq.List(0, 5), 
                (x, y) => Assert.Equal(x, y));

            Seq.List(first, multiplier).Apply(i => Assert.IsType<int>(i));

            int expectedFirst = seq.First() * multiplier;

            first = seq.IfNotNull(x => x.First() * multiplier, () => multiplier);

            Assert.IsType<int>(first);
            Assert.Equal(expectedFirst, first);

            // Alternate flow - unhappy path.
            seq = null;
            first = 0;
            multiplier = 3;

            Assert.Null(seq);

            Seq.Apply(
                Seq.List(first, multiplier),
                Seq.List(0, 3),
                (x, y) => Assert.Equal(x, y));

            Seq.List(first, multiplier).Apply(i => Assert.IsType<int>(i));

            first = seq.IfNotNull(x => x.First() * multiplier, () => multiplier);

            Assert.IsType<int>(first);
            Assert.Equal(multiplier, first);
        }

        /// <summary>
        /// If Not Null test, fourth overload.
        /// </summary>
        [Fact]
        public void Flow_IfNotNull_4()
        {
            int arg0 = 0;
            int arg1 = 0;
            object nullObj = null;
            string oneTwoThree = "123";

            Seq.List(arg0, arg1).Apply(i =>
            {
                Assert.IsType<int>(i);
                Assert.Equal(0, i);
            });

            Assert.Null(nullObj);

            Assert.NotNull(oneTwoThree);
            Assert.IsType<string>(oneTwoThree);
            Assert.NotEmpty(oneTwoThree);
            oneTwoThree.Should().NotBeNullOrWhiteSpace();

            oneTwoThree.IfNotNull(v => arg0 = v.Length);
            nullObj.IfNotNull(v => arg0 = 754);

            Seq.Apply(
                Seq.List(3, 0),
                Seq.List(arg0, arg1),
                (x, y) => Assert.Equal(x, y));

            oneTwoThree.IfNotNull(v => arg0 = v.Length * 2, () => arg0 = 1);
            nullObj.IfNotNull(v => arg1 = 4, () => arg1 = 2);

            Seq.Apply(
                Seq.List(6, 2),
                Seq.List(arg0, arg1),
                (x, y) => Assert.Equal(x, y));

            arg0 = oneTwoThree.IfNotNull(v => v.Length * 3, () => 9);
            arg1 = nullObj.IfNotNull(v => 11, () => 13);
            
            Seq.Apply(
                Seq.List(9, 13),
                Seq.List(arg0, arg1),
                (x, y) => Assert.Equal(x, y));
        }

        /// <summary>
        /// If Not Null or Empty test, first overload.
        /// </summary>
        [Fact]
        public void Flow_IfNotNullOrEmpty_1()
        {
            IEnumerable<int> seq = Enumerable.Range(1, 9);
            bool seqIsNullOrEmpty = false;
            int sum = 0;

            AssertEx.NotNullOrEmpty(seq);
            Assert.IsType<bool>(seqIsNullOrEmpty);
            Assert.Equal(false, seqIsNullOrEmpty);
            Assert.IsType<int>(sum);
            Assert.Equal(0, sum);

            seq.IfNotNullOrEmpty(i => sum += i, () => seqIsNullOrEmpty = true);

            Assert.IsType<int>(sum);
            Assert.Equal(seq.Sum(), sum);
            Assert.Equal(false, seqIsNullOrEmpty);

            seq = Enumerable.Empty<int>();
            sum = 0;

            Assert.NotNull(seq);
            Assert.Empty(seq);

            seq.IfNotNullOrEmpty(i => sum += i, () => seqIsNullOrEmpty = true);

            Assert.IsType<int>(sum);
            Assert.Equal(0, sum);
            Assert.IsType<bool>(seqIsNullOrEmpty);
            Assert.Equal(true, seqIsNullOrEmpty);
        }

        /// <summary>
        /// If Not Null or Empty test, second overload.
        /// </summary>
        [Fact]
        public void Flow_IfNotNullOrEmpty_2()
        {
            IEnumerable<int> seq = Enumerable.Range(1, 13);
            int sum = 0;

            AssertEx.NotNullOrEmpty(seq);
            Assert.IsType<int>(sum);
            Assert.Equal(0, sum);

            seq.IfNotNullOrEmpty(i => sum += i);

            Assert.IsType<int>(sum);
            Assert.Equal(seq.Sum(), sum);

            seq = null;
            sum = 0;

            Assert.Null(seq);

            seq.IfNotNullOrEmpty(i => sum = 100);

            Assert.IsType<int>(sum);
            Assert.Equal(0, sum);
        }

        /// <summary>
        /// If Null test, first overload.
        /// </summary>
        [Fact]
        public void Flow_IfNull_1()
        {
            IEnumerable<int> seq = null;
            bool seqIsNull = false;

            Assert.Null(seq);
            Assert.IsType<bool>(seqIsNull);
            Assert.Equal(false, seqIsNull);

            seq.IfNull(() => seqIsNull = true);

            Assert.Null(seq);
            Assert.IsType<bool>(seqIsNull);
            Assert.Equal(true, seqIsNull);
        }

        /// <summary>
        /// If Null test, second overload.
        /// </summary>
        [Fact]
        public void Flow_IfNull_2()
        {
            int arg0 = 0;
            int arg1 = 0;
            object nullObj = null;
            string oneTwoThree = "123";

            Seq.List(arg0, arg1).Apply(i =>
            {
                Assert.IsType<int>(i);
                Assert.Equal(0, i);
            });

            Assert.Null(nullObj);

            Assert.NotNull(oneTwoThree);
            Assert.IsType<string>(oneTwoThree);
            Assert.NotEmpty(oneTwoThree);
            oneTwoThree.Should().NotBeNullOrWhiteSpace();

            oneTwoThree.IfNull(() => arg0 = 659);
            nullObj.IfNull(() => arg1 = 546);

            Seq.Apply(
                Seq.List(0, 546),
                Seq.List(arg0, arg1),
                (x, y) => Assert.Equal(x, y));
        }

        /// <summary>
        /// With Side Effects test.
        /// </summary>
        [Fact]
        public void Flow_With_Side_Effects_1()
        {
            IEnumerable<int> seq = Seq.List(4, 3, 1, 5, 2);
            List<int> list = new List<int>();
            
            Assert.NotNull(seq);
            Assert.NotEmpty(seq);

            Assert.NotNull(list);
            Assert.IsType<List<int>>(list);
            Assert.Empty(list);

            list.WithSideEffects(
                l => l.AddRange(seq.OrderByDescending(z => z).ToList()));

            Assert.NotNull(list);
            Assert.IsType<List<int>>(list);
            Assert.NotEmpty(list);
            list.ShouldBeEquivalentTo(seq);
            list.Should().Equal(seq.OrderByDescending(z => z));

            int first = list.First();
            
            Assert.Equal(5, first);
            Assert.IsType<int>(first);

            int last = list.Last();

            Assert.Equal(1, last);
            Assert.IsType<int>(last);
            
            Seq.List(first, last).Apply(x => Assert.Contains(x, seq));

            list.Should().NotBeAscendingInOrder();
            list.Should().BeInDescendingOrder();
        }

        /// <summary>
        /// With Side Effects test 2.
        /// </summary>
        [Fact]
        public void Flow_With_Side_Effects_2()
        {
            Random random = RandomEx.Create(245005959);
            StringBuilder sb = StringEx.CreateSB();

            Seq.List<object>(random, sb).Apply(o => Assert.NotNull(o));

            Assert.IsType<Random>(random);
            Assert.IsType<StringBuilder>(sb);
            Assert.Empty(sb.ToString());

            int i0 = random.Next();
            int i1 = random.Next();

            Seq.List(i0, i1).Apply(i =>
            {
                Assert.IsType<int>(i);
                i.Should().BeGreaterOrEqualTo(0);
            });

            string s0 = random.NextString(4, 16);
            string s1 = random.NextString(8, 32);
            string format = "{0}-{1} {2}:{3}";
            string s = string.Format(format, i0, s0, i1, s1);

            Seq.List(s0, s1, format, s).Apply(x =>
            {
                Assert.NotNull(x);
                Assert.IsType<string>(x);
                Assert.NotEmpty(x);
                x.Should().NotBeNullOrWhiteSpace();
            });

            sb.WithSideEffects(y => y.AppendFormat(format, i0, s0, i1, s1));

            Assert.NotNull(sb);
            Assert.IsType<StringBuilder>(sb);
            Assert.NotEmpty(sb.ToString());
            sb.ToString().Should().NotBeNullOrWhiteSpace();
            Assert.Equal(s, sb.ToString());

            sb.WithSideEffects(v => v.Clear());

            Assert.NotNull(sb);
            Assert.IsType<StringBuilder>(sb);
            Assert.Empty(sb.ToString());

            Assert.Equal(
                s,
                StringEx
                    .CreateSB()
                    .WithSideEffects(v => v.AppendFormat(format, i0, s0, i1, s1))
                    .ToString());
        }

        /// <summary>
        /// With Side Effects Map test.
        /// </summary>
        [Fact]
        public void Flow_With_Side_Effects_Map_1()
        {
            Random random = RandomEx.Create(535002726);
            StringBuilder sb = StringEx.CreateSB();

            Seq.List<object>(random, sb).Apply(o => Assert.NotNull(o));

            Assert.IsType<Random>(random);
            Assert.IsType<StringBuilder>(sb);
            Assert.Empty(sb.ToString());

            int i0 = random.Next();
            int i1 = random.Next();

            Seq.List(i0, i1).Apply(i =>
            {
                Assert.IsType<int>(i);
                i.Should().BeGreaterOrEqualTo(0);
            });

            string s0 = random.NextString(4, 16);
            string s1 = random.NextString(8, 32);
            string format = "{0}-{1} {2}:{3}";
            string s = string.Format(format, i0, s0, i1, s1);

            Seq.List(s0, s1, format, s).Apply(x =>
            {
                Assert.NotNull(x);
                Assert.IsType<string>(x);
                Assert.NotEmpty(x);
                x.Should().NotBeNullOrWhiteSpace();
            });

            int len = sb.WithSideEffectsMap(
                x => x.AppendFormat(format, i0, s0, i1, s1).Length);

            Assert.NotNull(sb);
            Assert.IsType<StringBuilder>(sb);
            Assert.NotEmpty(sb.ToString());
            sb.ToString().Should().NotBeNullOrWhiteSpace();
            Assert.IsType<int>(len);
            len.Should().BeGreaterThan(0);
            Assert.Equal(s, sb.ToString());
            Assert.Equal(s.Length, len);
        }

        /// <summary>
        /// With Side Effects Map test 2.
        /// </summary>
        [Fact]
        public void Flow_With_Side_Effects_Map_2()
        {
            IEnumerable<string> seq = Seq.List("quatro", "tres", "um", "cinco", "dois");
            int count = seq.Count();

            Assert.NotNull(seq);
            Assert.NotEmpty(seq);
            Assert.IsType<int>(count);
            Assert.Equal(5, count);

            int length = seq.WithSideEffectsMap(x => x.Append("seis").Count());

            Assert.IsType<int>(length);
            Assert.Equal(6, length);
            length.Should().BeGreaterThan(count);
            Assert.NotNull(seq);
            Assert.NotEmpty(seq);
            Assert.Equal(count, seq.Count());
        }
        
        /// <summary>
        /// Shared Kernel Using test, first overload.
        /// </summary>
        [Fact]
        public void Flow_Using_1()
        {
            Random random = RandomEx.Create(604374934);
            byte[] byteArray = random.NextBytes(random.Next(4, 99));
            long streamLength = 0;
            MemoryStream stream = null;

            Flow.Using(
                stream = new MemoryStream(), 
                m =>
                {
                    stream.Write(byteArray, 0, byteArray.Length);

                    Assert.NotNull(stream);
                    Assert.IsType<MemoryStream>(stream);
                    Assert.IsType<long>(stream.Length);
                    stream.Length.Should().BeGreaterThan(0L);
                    
                    streamLength = stream.Length;
                });

            Assert.IsType<long>(streamLength);
            streamLength.Should().BeGreaterThan(0L);
            Assert.Equal(byteArray.Length, streamLength);

            AssertDisposedObject(() => streamLength = stream.Length);
        }

        /// <summary>
        /// Shared Kernel Using test, second overload.
        /// </summary>
        [Fact]
        public void Flow_Using_2()
        {
            Random random = RandomEx.Create(312926648);
            byte[] byteArray = random.NextBytes(random.Next(3, 56));
            MemoryStream stream = null;
            long streamLength = Flow.Using(
                stream = new MemoryStream(), 
                m =>
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    
                    Assert.NotNull(stream);
                    Assert.IsType<MemoryStream>(stream);
                    Assert.IsType<long>(stream.Length);
                    stream.Length.Should().BeGreaterThan(0L);

                    return stream.Length;
                });

            Assert.IsType<long>(streamLength);
            streamLength.Should().BeGreaterThan(0L);
            Assert.Equal(byteArray.Length, streamLength);

            AssertDisposedObject(() => streamLength = stream.Length);
        }

        /// <summary>
        /// Shared Kernel Using test, fifth overload.
        /// </summary>
        [Fact]
        public void Flow_Using_5()
        {
            Random random = RandomEx.Create(962556634);
            byte[] byteArray = random.NextBytes(random.Next(9, 72));
            long length1 = 0;
            long length2 = 0;
            MemoryStream stream1 = null;
            MemoryStream stream2 = null;

            Flow.Using(
                stream1 = new MemoryStream(),
                m => stream2 = new MemoryStream(), 
                y =>  
                {
                    stream1.Write(byteArray, 0, byteArray.Length);

                    Assert.NotNull(stream1);
                    Assert.IsType<MemoryStream>(stream1);
                    Assert.IsType<long>(stream1.Length);
                    stream1.Length.Should().BeGreaterThan(0L);

                    length1 = stream1.Length;
                    stream2 = stream1;

                    Assert.NotNull(stream2);
                    Assert.IsType<MemoryStream>(stream2);
                    Assert.IsType<long>(stream2.Length);
                    stream2.Length.Should().BeGreaterThan(0L);

                    Assert.Equal(stream1.Length, stream2.Length);

                    length2 = stream2.Length;
                });

            Seq.List(length1, length2).Apply(l =>
            {
                Assert.IsType<long>(l);
                l.Should().BeGreaterThan(0L);
                Assert.Equal(byteArray.Length, l);
            });

            Seq.Apply(
                Seq.List(length1, length2),
                Seq.List(stream1, stream2),
                (l, s) => AssertDisposedObject(() => l = s.Length));
        }

        /// <summary>
        /// Shared Kernel Using test, eighth overload.
        /// </summary>
        [Fact]
        public void Flow_Using_8()
        {
            Random random = RandomEx.Create(748530178);
            byte[] byteArray = random.NextBytes(random.Next(9, 87));
            long length1 = 0;
            long length2 = 0;
            MemoryStream stream1 = null;
            MemoryStream stream2 = null;

            length2 = Flow.Using(
                stream1 = new MemoryStream(),
                m => stream2 = new MemoryStream(),
                y => 
                {
                    stream1.Write(byteArray, 0, byteArray.Length);

                    Assert.NotNull(stream1);
                    Assert.IsType<MemoryStream>(stream1);
                    Assert.IsType<long>(stream1.Length);
                    stream1.Length.Should().BeGreaterThan(0L);

                    length1 = stream1.Length;
                    stream2 = stream1;

                    Assert.NotNull(stream2);
                    Assert.IsType<MemoryStream>(stream2);
                    Assert.IsType<long>(stream2.Length);
                    stream2.Length.Should().BeGreaterThan(0L);

                    Assert.Equal(stream1.Length, stream2.Length);

                    return stream2.Length;
                });

            Seq.List(length1, length2).Apply(l =>
            {
                Assert.IsType<long>(l);
                l.Should().BeGreaterThan(0L);
                Assert.Equal(byteArray.Length, l);
            });
            
            Seq.Apply(
              Seq.List(length1, length2),
              Seq.List(stream1, stream2),
              (l, s) => AssertDisposedObject(() => l = s.Length));
        }

        /// <summary>
        /// Shared Kernel Using test, eleventh overload.
        /// </summary>
        [Fact]
        public void Flow_Using_11()
        {
            Random random = RandomEx.Create(443535025);
            byte[] byteArray = random.NextBytes(random.Next(5, 41));
            long length1 = 0;
            long length2 = 0;
            long length3 = 0;
            MemoryStream stream1 = null;
            MemoryStream stream2 = null;
            MemoryStream stream3 = null;

            Flow.Using(
                stream1 = new MemoryStream(),
                m => stream2 = new MemoryStream(),
                n => stream3 = new MemoryStream(),
                y => 
                {
                    stream1.Write(byteArray, 0, byteArray.Length);

                    Assert.NotNull(stream1);
                    Assert.IsType<MemoryStream>(stream1);
                    Assert.IsType<long>(stream1.Length);
                    stream1.Length.Should().BeGreaterThan(0L);

                    length1 = stream1.Length;
                    stream2 = stream1;

                    Assert.NotNull(stream2);
                    Assert.IsType<MemoryStream>(stream2);
                    Assert.IsType<long>(stream2.Length);
                    stream2.Length.Should().BeGreaterThan(0L);

                    Assert.Equal(stream1.Length, stream2.Length);

                    length2 = stream2.Length;
                    stream3 = stream2;

                    Assert.NotNull(stream3);
                    Assert.IsType<MemoryStream>(stream3);
                    Assert.IsType<long>(stream3.Length);
                    stream3.Length.Should().BeGreaterThan(0L);
                    
                    Assert.Equal(
                       1,
                       Seq.List(stream1, stream2, stream3)
                           .Map(x => x.Length)
                           .Distinct()
                           .Count());

                    length3 = stream3.Length; 
                });
            
            Seq.List(length1, length2, length3).Apply(l =>
            {
                Assert.IsType<long>(l);
                l.Should().BeGreaterThan(0L);
                Assert.Equal(byteArray.Length, l);
            });

            Seq.Apply(
              Seq.List(length1, length2, length3),
              Seq.List(stream1, stream2, stream3),
              (l, s) => AssertDisposedObject(() => l = s.Length));
        }

        /// <summary>
        /// Shared Kernel Using test, fourteenth overload.
        /// </summary>
        [Fact]
        public void Flow_Using_14()
        {
            Random random = RandomEx.Create(96375672);
            byte[] byteArray = random.NextBytes(random.Next(8, 64));
            long length1 = 0;
            long length2 = 0;
            long length3 = 0;
            MemoryStream stream1 = null;
            MemoryStream stream2 = null;
            MemoryStream stream3 = null;

            length3 = Flow.Using(
                stream1 = new MemoryStream(),
                m => stream2 = new MemoryStream(),
                n => stream3 = new MemoryStream(),
                y => 
                {
                    stream1.Write(byteArray, 0, byteArray.Length);

                    Assert.NotNull(stream1);
                    Assert.IsType<MemoryStream>(stream1);
                    Assert.IsType<long>(stream1.Length);
                    stream1.Length.Should().BeGreaterThan(0L);

                    length1 = stream1.Length;
                    stream2 = stream1;

                    Assert.NotNull(stream2);
                    Assert.IsType<MemoryStream>(stream2);
                    Assert.IsType<long>(stream2.Length);
                    stream2.Length.Should().BeGreaterThan(0L);

                    Assert.Equal(stream1.Length, stream2.Length);

                    length2 = stream2.Length;
                    stream3 = stream2;

                    Assert.NotNull(stream3);
                    Assert.IsType<MemoryStream>(stream3);
                    Assert.IsType<long>(stream3.Length);
                    stream3.Length.Should().BeGreaterThan(0L);
                    
                    Assert.Equal(
                        1,
                        Seq.List(stream1, stream2, stream3)
                            .Map(x => x.Length)
                            .Distinct()
                            .Count());

                    return stream3.Length;
                });

            Seq.List(length1, length2, length3).Apply(l =>
            {
                Assert.IsType<long>(l);
                l.Should().BeGreaterThan(0L);
                Assert.Equal(byteArray.Length, l);
            });

            Seq.Apply(
              Seq.List(length1, length2, length3),
              Seq.List(stream1, stream2, stream3),
              (l, s) => AssertDisposedObject(() => l = s.Length));
        }
        
        #endregion
    }
}