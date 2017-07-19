using System;
using System.Diagnostics;
using System.Threading.Tasks;

using FluentAssertions;

using Xunit;

namespace Magenic.SharedKernel.Tests
{
    /// <summary>
    /// This class implements unit tests for CodeTimer utility class.
    /// </summary>
    public class CodeTimerTests
    {
        #region Private Static Methods

        private static void LongRunningMethod()
        {
            Debug.WriteLine("Execution starts.");
            Task.Delay(TimeSpan.FromSeconds(5)).Wait();
            Debug.WriteLine("Execution ends.");
        }

        private static void AssertMessageAndToString(string text)
        {
            AssertEx.NotNullOrEmptyOfType<string>(text);
            text.Should().NotBeNullOrWhiteSpace();
            text.Should().Contain("completed");
            text.Should().NotContain("running");
        }

        #endregion

        #region Public Instance Methods

        /// <summary>
        /// A test for CodeTimer Message property.
        /// </summary>
        [Fact]
        public void CodeTimerTest_Message()
        {
            CodeTimer timer = null;
            string msg = string.Empty;

            Assert.Null(timer);
            Assert.IsType<string>(msg);
            Assert.Empty(msg);

            Flow.Using(
                CodeTimer.Step(nameof(CodeTimerTest_Message)),
                c =>
                {
                    timer = c;

                    LongRunningMethod();

                    Assert.NotNull(timer);
                    Assert.IsType<CodeTimer>(timer);

                    msg = timer.Message;

                    Seq.List(
                        msg,
                        timer.StepName).Apply(m =>
                        {
                            AssertEx.NotNullOrEmptyOfType<string>(m);
                            m.Should().NotBeNullOrWhiteSpace();
                        });

                    msg.Should().Contain("running");
                    msg.Should().NotContain("completed");

                    Assert.Equal(nameof(CodeTimerTest_Message), timer.StepName);
                });

            Assert.NotNull(timer);
            Assert.IsType<CodeTimer>(timer);

            AssertMessageAndToString(timer.Message);
        }

        /// <summary>
        /// A test for CodeTimer Elapsed property.
        /// </summary>
        [Fact]
        public void CodeTimerTest_Elapsed()
        {
            CodeTimer timer = null;

            Assert.Null(timer);

            Flow.Using(
                CodeTimer.Step(nameof(CodeTimerTest_Elapsed)),
                c =>
                {
                    timer = c;

                    LongRunningMethod();

                    Assert.NotNull(timer);
                    Assert.IsType<CodeTimer>(timer);

                    TimeSpan elapsed = timer.Elapsed;

                    Assert.IsType<TimeSpan>(elapsed);
                    elapsed.Should().BeGreaterThan(TimeSpan.MinValue);

                    Seq.List(
                        timer.ElapsedText,
                        timer.StepName).Apply(m =>
                        {
                            AssertEx.NotNullOrEmptyOfType<string>(m);
                            m.Should().NotBeNullOrWhiteSpace();
                        });

                    Assert.Equal(nameof(CodeTimerTest_Elapsed), timer.StepName);
                });
        }

        /// <summary>
        /// A test for CodeTimer ToString method.
        /// </summary>
        [Fact]
        public void CodeTimerTest_ToString()
        {
            CodeTimer timer = null;
            string timerString = string.Empty;

            Assert.Null(timer);
            Assert.IsType<string>(timerString);
            Assert.Empty(timerString);

            Flow.Using(
                CodeTimer.Step(nameof(CodeTimerTest_ToString)),
                c =>
                {
                    timer = c;

                    LongRunningMethod();

                    Assert.NotNull(timer);
                    Assert.IsType<CodeTimer>(timer);

                    timerString = timer.ToString();

                    Seq.List(
                        timerString,
                        timer.StepName).Apply(m =>
                        {
                            AssertEx.NotNullOrEmptyOfType<string>(m);
                            m.Should().NotBeNullOrWhiteSpace();
                        });

                    timerString.Should().Contain("running");
                    timerString.Should().NotContain("completed");
                    Assert.Equal(nameof(CodeTimerTest_ToString), timer.StepName);
                });

            Assert.NotNull(timer);
            Assert.IsType<CodeTimer>(timer);

            AssertMessageAndToString(timer.ToString());
        }

        #endregion
    }
}
