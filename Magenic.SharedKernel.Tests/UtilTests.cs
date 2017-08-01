using System;
using System.Text;

using FluentAssertions;

using Xunit;

namespace Magenic.SharedKernel.Tests
{
    /// <summary>
    /// This class implements unit tests for Util class.
    /// </summary>
    public class UtilTests
    {
        #region Public Methods

        /// <summary>
        /// A test for Util.Repeat.
        /// </summary>
        [Fact]
        public void Util_Repeat()
        {
            Random rnd = PseudoRandom.Create(4234895);
            int count = rnd.Next(2, 10);
            int testVal = 0;            

            //Test where count > 1
            Util.Repeat(() => testVal += 5, count);

            Assert.Equal(5 * count, testVal);

            //Test where count = 0
            Util.Repeat(() => testVal += 5, 0);

            Assert.Equal(5 * count, testVal);
        }

        /// <summary>
        /// A test for Util.Repeat using StringBuilder.
        ///</summary>
        [Fact]
        public void Util_Repeat_WithStringBuilder()
        {
            Random random = PseudoRandom.Create(158131364);
            StringBuilder sb = StringEx.CreateSB();

            Seq.List<object>(random, sb).Apply(Assert.NotNull);
            Assert.IsType<Random>(random);
            Assert.IsType<StringBuilder>(sb);

            char c = random.NextChar(StringComposition.UppercaseLetter);
            int count = random.Next(8, 16);

            Assert.IsType<char>(c);
            Assert.IsType<int>(count);
            count.Should().BeGreaterOrEqualTo(8);
            count.Should().BeLessThan(16);

            Util.Repeat(() => sb.Append(c), count);
            Assert.Equal(StringEx.CreateString(c, count), sb.ToString());
            Assert.Equal(new string(c, count), sb.ToString());

            sb.Clear();

            Util.Repeat(i => sb.AppendFormat("{0}", i), 6);
            Assert.Equal("012345", sb.ToString());
        }

        /// <summary>
        /// A Test for Util.Repeat passing iteration number as parameter.
        /// </summary>
        [Fact]
        public void Util_Repeat_WithParam()
        {
            Random rnd = PseudoRandom.Create(2324345);
            int count = rnd.Next(2, 10);            
            int testVal1 = 0;
            int testVal2 = 0;

            //Test where count > 1
            Util.Repeat(i => testVal1 += i, count);

            for (int x = 0; x < count; ++x)
            {
                testVal2 += x;
            }

            Assert.Equal(testVal2, testVal1);

            //Test where count = 0
            Util.Repeat(i => testVal1 += i, 0);

            Assert.Equal(testVal2, testVal1);
        }

        /// <summary>
        /// A test for Util.Dispose using Source.
        /// </summary>
        [Fact]
        public void Util_DisposeSource()
        {
            TestDisposableObject disposableObject = TestDisposableObject.Create();

            Assert.False(disposableObject.Disposed);

            Util.Dispose(disposableObject);

            Assert.NotNull(disposableObject);
            Assert.True(disposableObject.Disposed);

            //Test Null object. No Exception thrown.
            disposableObject = null;
                        
            Util.Dispose(disposableObject);

            Assert.Null(disposableObject);
        }

        /// <summary>
        /// A test for Util.Dispose using Object.
        /// </summary>
        [Fact]
        public void Util_DisposeObject()
        {
            object disposableObject = TestDisposableObject.Create();

            Assert.False(((TestDisposableObject)disposableObject).Disposed);

            Util.Dispose(disposableObject);

            Assert.NotNull(disposableObject);
            Assert.True(((TestDisposableObject)disposableObject).Disposed);

            //Test Null object. No Exception thrown.
            disposableObject = null;

            Util.Dispose(disposableObject);

            Assert.Null(disposableObject);
        }

        /// <summary>
        /// A test for Util.DisposeAndNull using Source.
        /// </summary>
        [Fact]
        public void Util_DisposeAndNullSource()
        {
            TestDisposableObject disposableService = TestDisposableObject.Create();
            TestDisposableObject disposableObject = TestDisposableObject
                .Create(disposableService);

            Assert.False(disposableObject.Disposed);
            Assert.False(disposableService.Disposed);

            Util.DisposeAndNull(ref disposableObject);

            Assert.Null(disposableObject);
            Assert.NotNull(disposableService);
            Assert.True(disposableService.Disposed);

            //Test Null object. No Exception thrown.
            disposableObject = null;

            Util.DisposeAndNull(ref disposableObject);

            Assert.Null(disposableObject);
        }

        /// <summary>
        /// A test for Util.DisposeAndNull using Object.
        /// </summary>
        [Fact]
        public void Util_DisposeAndNullObject()
        {
            TestDisposableObject disposableService = TestDisposableObject.Create();
            object disposableObject = TestDisposableObject.Create(disposableService);

            Assert.False(((TestDisposableObject)disposableObject).Disposed);
            Assert.False(disposableService.Disposed);

            Util.DisposeAndNull(ref disposableObject);

            Assert.Null(disposableObject);
            Assert.NotNull(disposableService);
            Assert.True(disposableService.Disposed);

            //Test Null object. No Exception thrown.
            disposableObject = null;

            Util.DisposeAndNull(ref disposableObject);

            Assert.Null(disposableObject);
        }

        /// <summary>
        /// A test for Util.Dispose with Action as parameter.
        /// </summary>
        [Fact]
        public void Util_DisposeWithAction()
        {
            TestDisposableObject disposableObject = TestDisposableObject.Create();

            Assert.False(disposableObject.Disposed);

            Util.Dispose(
                disposableObject, 
                d => d.Text = "Object Disposed");

            Assert.NotNull(disposableObject);
            Assert.True(disposableObject.Disposed);
            Assert.Equal("Object Disposed", disposableObject.Text);

            //Test Null object. No Exception thrown.
            disposableObject = null;

            Util.Dispose(
                disposableObject, 
                d => d.Text = "Object Null");

            Assert.Null(disposableObject);
        }

        /// <summary>
        /// A test for Util.Dispose with Function as parameter.
        /// </summary>
        [Fact]
        public void Util_DisposeWithFunction()
        {
            TestDisposableObject disposableObject = TestDisposableObject.Create();

            Assert.False(disposableObject.Disposed);

            int retValue = Util.Dispose(
                disposableObject, 
                d =>
                {
                    d.Text = "Object Disposed";

                    return 5;
                });

            Assert.NotNull(disposableObject);
            Assert.True(disposableObject.Disposed);
            Assert.Equal("Object Disposed", disposableObject.Text);
            Assert.Equal(5, retValue);

            //Test Null object. No Exception thrown. Returns default(int)
            disposableObject = null;

            retValue = Util.Dispose(
                disposableObject, 
                d =>
                {
                    d.Text = "Object Null";

                    return 11;
                });

            Assert.Null(disposableObject);
            Assert.Equal(0, retValue);
        }

        #endregion
    }

    /// <summary>
    /// Disposable Object that is used to test Dispose methods of the Util Helper.
    /// </summary>
    internal class TestDisposableObject : DisposableObject
    {
        #region Creation

        private TestDisposableObject()
        {            
        }        
        
        public static TestDisposableObject Create()
        {
            return new TestDisposableObject();
        }

        public static TestDisposableObject Create(IDisposable obj)
        {
            return new TestDisposableObject()
            {
                DisposableService = obj
            };
        }

        #endregion

        #region Properties

        public IDisposable DisposableService { get; private set; }
        public string Text { get; set; }

        #endregion

        #region Protected Methods

        protected override void DisposeManagedResources()
        {
            DisposableService?.Dispose();
        }

        #endregion
    }
}
