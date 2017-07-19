using System;
using System.Net.Http;

using FluentAssertions;

using Xunit;

namespace Magenic.SharedKernel.Tests
{
    /// <summary>
    /// This class implements unit tests for UriEx utility class.
    /// </summary>
    public class UriUnitTests
    {
        #region Public Methods

        /// <summary>
        /// A test for AddPort.
        ///</summary>
        [Fact]
        public void Uri_AddPort()
        {
            string baseUri = "baseUri";
            int port = 1;
            Uri actual = UriEx.AddPort(baseUri, port);

            Assert.NotNull(actual);
            Assert.IsType<Uri>(actual);
            Assert.Equal(port, actual.Port);
        }

        /// <summary>
        /// A test for Create.
        ///</summary>
        [Fact]
        public void Uri_CreateString()
        {
            string baseUri = "http://testbaseaddress";
            string relativeUri = "relativeUri";
            string absoluteUri = $"{baseUri}/{relativeUri}";
            Uri actual = UriEx.Create(baseUri, relativeUri);

            Assert.NotNull(actual);
            Assert.IsType<Uri>(actual);
            Assert.NotNull(actual.AbsoluteUri);
            actual.AbsoluteUri.Should().NotBeNullOrWhiteSpace();
            Assert.Equal(absoluteUri, actual.AbsoluteUri);
        }

        /// <summary>
        /// A test for Create.
        ///</summary>
        [Fact]
        public void Uri_CreateUri()
        {
            string relativeUri = "relativeUri";
            Uri baseUri = CreateBaseUri();
            Uri actual = UriEx.Create(baseUri, relativeUri);
            string absoluteUri = $"{baseUri.AbsoluteUri}/{relativeUri}";

            Assert.NotNull(actual);
            Assert.IsType<Uri>(actual);
            Assert.NotNull(actual.AbsoluteUri);
            actual.AbsoluteUri.Should().NotBeNullOrWhiteSpace();
            Assert.Equal(absoluteUri, actual.AbsoluteUri);
        }

        /// <summary>
        /// A test for SetScheme.
        ///</summary>
        [Fact]
        public void Uri_SetScheme()
        {
            string scheme = "http";
            Uri baseUri = CreateBaseUri();
            Uri actual = UriEx.SetScheme(baseUri, scheme);

            Assert.NotNull(actual);
            Assert.IsType<Uri>(actual);
            Assert.NotNull(actual.Scheme);
            actual.Scheme.Should().NotBeNullOrWhiteSpace();
            Assert.Equal(scheme, actual.Scheme);
        }

        #endregion

        #region Private Methods

        private Uri CreateBaseUri()
        {
            Random random = RandomEx.Create(7973323);
            string resourcePrefix = random.NextString(8, 16);
            string baseAddress = random.NextString(8, 16);
            string WEB_SERVICES_GROUP = random.NextString(8, 16);
            string webServiceName = random.NextString(8, 16);
            int PORT_NUMBER = random.Next(1000, 9999);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = UriEx.Create(
                    UriEx.Create(
                        UriEx.AddPort(baseAddress, PORT_NUMBER),
                        WEB_SERVICES_GROUP),
                    webServiceName);

                Assert.NotNull(client);
                Assert.IsType<HttpClient>(client);

                Uri baseUri = UriEx.Create(client.BaseAddress, resourcePrefix);

                Assert.NotNull(baseUri);
                Assert.IsType<Uri>(baseUri);

                return baseUri;
            }
        }

        #endregion
    }
}
