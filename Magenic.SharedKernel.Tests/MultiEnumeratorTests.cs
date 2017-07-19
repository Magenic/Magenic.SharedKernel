using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Xunit;

namespace Magenic.SharedKernel.Tests
{
    /// <summary>
    /// This class implements unit tests for 
    /// MultiEnumerator utility class which accepts 2 Enumerables.
    /// </summary>
    public class MultiEnumeratorTest2
    {
        #region Private Static Methods

        private static void AssertSeqs<TFirst, TSecond>
            (IEnumerable<TFirst> seq, IEnumerable<TSecond> seq2)
        {
            AssertEx.NotNullOrEmpty(seq);
            AssertEx.NotNullOrEmpty(seq2);

            Seq.List(
                seq.Count(),
                seq2.Count()).Apply(m =>
                {
                    Assert.IsType<int>(m);
                    m.Should().BeGreaterThan(0);
                });
        }

        #endregion

        #region Public Instance Methods

        /// <summary>
        /// MultiEnumerator Current Test.
        /// </summary>
        [Fact]
        public void MultiEnumerator_Current()
        {
            IEnumerable<int> seq = Enumerable.Range(1, 9);
            IEnumerable<string> seq2 = Seq.List("one", "two", "three", "four");
            int counter = 0;

            AssertSeqs(seq, seq2);
            Assert.IsType<int>(counter);
            counter.ShouldBeEquivalentTo(0);

            Flow.Using(MultiEnumerator.Create(seq, seq2),
                enumerator =>
                {
                    Assert.NotNull(enumerator);

                    while (enumerator.MoveNext())
                    {
                        Tuple<int, string> curr = enumerator.Current;

                        Assert.NotNull(curr);

                        int item1 = curr.Item1;

                        Assert.IsType<int>(item1);
                        item1.Should().BeGreaterThan(0);
                        seq.Should().Contain(item1);

                        string item2 = curr.Item2;

                        Assert.IsType<string>(item2);
                        Assert.NotEmpty(item2);
                        seq2.Should().Contain(item2);

                        counter++;
                    }
                });

            counter.Should().BeGreaterThan(0);
            counter.Should().BeLessThan(seq.Count());
            counter.ShouldBeEquivalentTo(seq2.Count());
        }

        /// <summary>
        /// MultiEnumerator MoveNext Test.
        /// </summary>
        [Fact]
        public void MultiEnumerator_MoveNext()
        {
            IEnumerable<int> seq = Enumerable.Range(1, 9);
            IEnumerable<string> seq2 = Seq.List("one", "two", "three", "four");
            int counter = 0;

            AssertSeqs(seq, seq2);
            Assert.IsType<int>(counter);
            counter.ShouldBeEquivalentTo(0);

            Flow.Using(MultiEnumerator.Create(seq, seq2),
                enumerator =>
                {
                    Assert.NotNull(enumerator);

                    while (enumerator.MoveNext())
                    {
                        Assert.NotNull(enumerator.Current);
                        counter++;
                    }

                    bool canMoveNext = enumerator.MoveNext();
                    Assert.IsType<bool>(canMoveNext);
                    canMoveNext.ShouldBeEquivalentTo(false);
                });

            counter.Should().BeGreaterThan(0);
            counter.Should().BeLessThan(seq.Count());
            counter.ShouldBeEquivalentTo(seq2.Count());
        }

        /// <summary>
        /// MultiEnumerator Reset Test.
        /// </summary>
        [Fact]
        public void MultiEnumerator_Reset()
        {
            IEnumerable<int> seq = Enumerable.Range(1, 9);
            IEnumerable<string> seq2 = Seq.List("one", "two", "three", "four");

            Flow.Using(MultiEnumerator.Create(seq, seq2),
                enumerator =>
                {
                    Assert.NotNull(enumerator);

                    while (enumerator.MoveNext())
                    {
                        Tuple<int, string> curr = enumerator.Current;

                        Assert.NotNull(curr);

                        int item1 = curr.Item1;

                        Assert.IsType<int>(item1);
                        item1.Should().BeGreaterThan(0);
                        seq.Should().Contain(item1);

                        string item2 = curr.Item2;

                        Assert.IsType<string>(item2);
                        Assert.NotEmpty(item2);
                        seq2.Should().Contain(item2);

                        Exception ex = Assert.Throws<NotSupportedException>(
                            () => enumerator.Reset());

                        Assert.Equal("Specified method is not supported.", ex.Message);
                    }
                });
        }

        #endregion
    }

    /// <summary>
    /// This class implements unit tests for 
    /// MultiEnumerator utility class which accepts 3 Enumerables.
    /// </summary>
    public class MultiEnumeratorTest3
    {
        #region Private Static Methods

        private static void AssertSeqs<TFirst, TSecond, TThird>(
            IEnumerable<TFirst> seq,
            IEnumerable<TSecond> seq2,
            IEnumerable<TThird> seq3)
        {
            AssertEx.NotNullOrEmpty(seq);
            AssertEx.NotNullOrEmpty(seq2);
            AssertEx.NotNullOrEmpty(seq3);

            Seq.List(
                seq.Count(),
                seq2.Count(),
                seq3.Count()).Apply(m =>
                {
                    Assert.IsType<int>(m);
                    m.Should().BeGreaterThan(0);
                });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// MultiEnumerator Current Test.
        /// </summary>
        [Fact]
        public void MultiEnumerator_Current()
        {
            IEnumerable<int> seq = Enumerable.Range(1, 9);
            IEnumerable<string> seq2 = Seq.List("one", "two", "three", "four");
            IEnumerable<int> seq3 = Enumerable.Range(11, 29);
            int counter = 0;

            AssertSeqs(seq, seq2, seq3);
            Assert.IsType<int>(counter);
            counter.ShouldBeEquivalentTo(0);

            Flow.Using(MultiEnumerator.Create(seq, seq2, seq3),
                enumerator =>
                {
                    Assert.NotNull(enumerator);

                    while (enumerator.MoveNext())
                    {
                        Tuple<int, string, int> curr = enumerator.Current;

                        Assert.NotNull(curr);

                        Seq.Apply(
                            Seq.List(curr.Item1, curr.Item3),
                            Seq.List(seq, seq3),
                            (x, y) =>
                            {
                                Assert.IsType<int>(x);
                                x.Should().BeGreaterThan(0);
                                y.Should().Contain(x);
                            });

                        string item2 = curr.Item2;

                        Assert.IsType<string>(item2);
                        Assert.NotEmpty(item2);
                        seq2.Should().Contain(item2);

                        counter++;
                    }
                });

            counter.Should().BeGreaterThan(0);
            counter.ShouldBeEquivalentTo(seq2.Count());

            Seq.List(
                seq.Count(),
                seq3.Count()).Apply(x => x.Should().BeGreaterThan(counter));
        }

        /// <summary>
        /// MultiEnumerator MoveNext Test.
        /// </summary>
        [Fact]
        public void MultiEnumerator_MoveNext()
        {
            IEnumerable<int> seq = Enumerable.Range(1, 9);
            IEnumerable<string> seq2 = Seq.List("one", "two", "three", "four");
            IEnumerable<int> seq3 = Enumerable.Range(11, 29);
            int counter = 0;

            AssertSeqs(seq, seq2, seq3);
            Assert.IsType<int>(counter);
            counter.ShouldBeEquivalentTo(0);

            Flow.Using(MultiEnumerator.Create(seq, seq2, seq3),
                enumerator =>
                {
                    Assert.NotNull(enumerator);

                    while (enumerator.MoveNext())
                    {
                        Assert.NotNull(enumerator.Current);
                        counter++;
                    }

                    bool canMoveNext = enumerator.MoveNext();
                    Assert.IsType<bool>(canMoveNext);
                    canMoveNext.ShouldBeEquivalentTo(false);
                });

            counter.Should().BeGreaterThan(0);
            counter.ShouldBeEquivalentTo(seq2.Count());

            Seq.List(
                seq.Count(),
                seq3.Count()).Apply(x => x.Should().BeGreaterThan(counter));
        }

        /// <summary>
        /// MultiEnumerator Reset Test.
        /// </summary>
        [Fact]
        public void MultiEnumerator_Reset()
        {
            IEnumerable<int> seq = Enumerable.Range(1, 9);
            IEnumerable<string> seq2 = Seq.List("one", "two", "three", "four");
            IEnumerable<int> seq3 = Enumerable.Range(11, 29);

            Flow.Using(MultiEnumerator.Create(seq, seq2, seq3),
              enumerator =>
              {
                  Assert.NotNull(enumerator);

                  while (enumerator.MoveNext())
                  {
                      Tuple<int, string, int> curr = enumerator.Current;

                      Assert.NotNull(curr);

                      Seq.Apply(
                          Seq.List(curr.Item1, curr.Item3),
                          Seq.List(seq, seq3),
                          (x, y) =>
                          {
                              Assert.IsType<int>(x);
                              x.Should().BeGreaterThan(0);
                              y.Should().Contain(x);
                          });

                      string item2 = curr.Item2;

                      Assert.IsType<string>(item2);
                      Assert.NotEmpty(item2);
                      seq2.Should().Contain(item2);

                      Exception ex = Assert.Throws<NotSupportedException>(
                          () => enumerator.Reset());

                      Assert.Equal("Specified method is not supported.", ex.Message);
                  }
              });
        }

        #endregion
    }

    /// <summary>
    /// This class implements unit tests for 
    /// MultiEnumerator utility class which accepts 4 Enumerables.
    /// </summary>
    public class MultiEnumeratorTest4
    {
        #region Private Static Methods

        private static void AssertSeqs<TFirst, TSecond, TThird, TFourth>(
            IEnumerable<TFirst> seq,
            IEnumerable<TSecond> seq2,
            IEnumerable<TThird> seq3,
            IEnumerable<TFourth> seq4)
        {
            AssertEx.NotNullOrEmpty(seq);
            AssertEx.NotNullOrEmpty(seq2);
            AssertEx.NotNullOrEmpty(seq3);
            AssertEx.NotNullOrEmpty(seq4);

            Seq.List(
                seq.Count(),
                seq2.Count(),
                seq3.Count(),
                seq4.Count()).Apply(m =>
                {
                    Assert.IsType<int>(m);
                    m.Should().BeGreaterThan(0);
                });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// MultiEnumerator Current Test.
        /// </summary>
        [Fact]
        public void MultiEnumerator_Current()
        {
            IEnumerable<int> seq = Enumerable.Range(1, 9);
            IEnumerable<string> seq2 = Seq.List("one", "two", "three", "four");
            IEnumerable<int> seq3 = Enumerable.Range(11, 29);
            IEnumerable<string> seq4 = Seq.List("un", "dois", "tres", "quatro", "cinco");
            int counter = 0;

            AssertSeqs(seq, seq2, seq3, seq4);
            Assert.IsType<int>(counter);
            counter.ShouldBeEquivalentTo(0);

            Flow.Using(MultiEnumerator.Create(seq, seq2, seq3, seq4),
                enumerator =>
                {
                    Assert.NotNull(enumerator);

                    while (enumerator.MoveNext())
                    {
                        Tuple<int, string, int, string> curr = enumerator.Current;

                        Assert.NotNull(curr);

                        Seq.Apply(
                            Seq.List(curr.Item1, curr.Item3),
                            Seq.List(seq, seq3),
                            (x, y) =>
                            {
                                Assert.IsType<int>(x);
                                x.Should().BeGreaterThan(0);
                                y.Should().Contain(x);
                            });

                        Seq.Apply(
                            Seq.List(curr.Item2, curr.Item4),
                            Seq.List(seq2, seq4),
                            (x, y) =>
                            {
                                Assert.IsType<string>(x);
                                Assert.NotEmpty(x);
                                y.Should().Contain(x);
                            });

                        counter++;
                    }
                });

            counter.Should().BeGreaterThan(0);
            counter.ShouldBeEquivalentTo(seq2.Count());

            Seq.List(
                seq.Count(),
                seq3.Count(),
                seq4.Count()).Apply(x => x.Should().BeGreaterThan(counter));
        }

        /// <summary>
        /// MultiEnumerator MoveNext Test.
        /// </summary>
        [Fact]
        public void MultiEnumerator_MoveNext()
        {
            IEnumerable<int> seq = Enumerable.Range(1, 9);
            IEnumerable<string> seq2 = Seq.List("one", "two", "three", "four");
            IEnumerable<int> seq3 = Enumerable.Range(11, 29);
            IEnumerable<string> seq4 = Seq.List("un", "dois", "tres", "quatro", "cinco");
            int counter = 0;

            AssertSeqs(seq, seq2, seq3, seq4);
            Assert.IsType<int>(counter);
            counter.ShouldBeEquivalentTo(0);

            Flow.Using(MultiEnumerator.Create(seq, seq2, seq3, seq4),
                enumerator =>
                {
                    Assert.NotNull(enumerator);

                    while (enumerator.MoveNext())
                    {
                        Assert.NotNull(enumerator.Current);
                        counter++;
                    }

                    bool canMoveNext = enumerator.MoveNext();
                    Assert.IsType<bool>(canMoveNext);
                    canMoveNext.ShouldBeEquivalentTo(false);
                });

            counter.Should().BeGreaterThan(0);
            counter.ShouldBeEquivalentTo(seq2.Count());

            Seq.List(
                seq.Count(),
                seq3.Count(),
                seq4.Count()).Apply(x => x.Should().BeGreaterThan(counter));
        }

        /// <summary>
        /// MultiEnumerator Reset Test.
        /// </summary>
        [Fact]
        public void MultiEnumerator_Reset()
        {
            IEnumerable<int> seq = Enumerable.Range(1, 9);
            IEnumerable<string> seq2 = Seq.List("one", "two", "three", "four");
            IEnumerable<int> seq3 = Enumerable.Range(11, 29);
            IEnumerable<string> seq4 = Seq.List("un", "dois", "tres", "quatro", "cinco");

            Flow.Using(MultiEnumerator.Create(seq, seq2, seq3, seq4),
                enumerator =>
                {
                    Assert.NotNull(enumerator);

                    while (enumerator.MoveNext())
                    {
                        Tuple<int, string, int, string> curr = enumerator.Current;

                        Assert.NotNull(curr);

                        Seq.Apply(
                            Seq.List(curr.Item1, curr.Item3),
                            Seq.List(seq, seq3),
                            (x, y) =>
                            {
                                Assert.IsType<int>(x);
                                x.Should().BeGreaterThan(0);
                                y.Should().Contain(x);
                            });

                        Seq.Apply(
                            Seq.List(curr.Item2, curr.Item4),
                            Seq.List(seq2, seq4),
                            (x, y) =>
                            {
                                Assert.IsType<string>(x);
                                Assert.NotEmpty(x);
                                y.Should().Contain(x);
                            });

                        Exception ex = Assert.Throws<NotSupportedException>(
                            () => enumerator.Reset());

                        Assert.Equal("Specified method is not supported.", ex.Message);
                    }
                });
        }

        #endregion
    }
}
