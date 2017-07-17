using System;

/// <summary>
/// Namespace for CBS NorthStar Order Entry API Shared Kernel.
/// </summary>
namespace Magenic.SharedKernel
{
    /// <summary>
    /// Contains Uri helper methods.
    /// </summary>
    public static class UriEx
    {
        #region Public Methods

        /// <summary>
        /// Adds port to Uri.
        /// </summary>
        /// <param name="baseUri">Base Uri.</param>
        /// <param name="port">Port number.</param>
        /// <returns>Uri.</returns>
        public static Uri AddPort(string baseUri, int port)
        {
            UriBuilder builder = new UriBuilder(baseUri);

            builder.Port = port;

            return builder.Uri;
        }

        /// <summary>
        /// Builds Uri from Base Uri and Relative Uri.
        /// </summary>
        /// <param name="baseUri">Base Uri.</param>
        /// <param name="relativeUri">Relative Uri.</param>
        /// <returns>Combined Uri.</returns>
        public static Uri Create(string baseUri, string relativeUri)
        {
            return new Uri(
                new Uri(baseUri.EndsWith("/") ? baseUri : $"{baseUri}/"),
                relativeUri);
        }

        /// <summary>
        /// Builds Uri from Base Uri and Relative Uri.
        /// </summary>
        /// <param name="baseUri">Base Uri.</param>
        /// <param name="relativeUri">Relative Uri.</param>
        /// <returns>Combined Uri.</returns>
        public static Uri Create(Uri baseUri, string relativeUri)
        {
            return Create(baseUri.AbsoluteUri, relativeUri);
        }

        /// <summary>
        /// Sets scheme.
        /// </summary>
        /// <param name="baseUri">Base Uri.</param>
        /// <param name="scheme">Scheme.</param>
        /// <returns>Uri with scheme set.</returns>
        public static Uri SetScheme(
            Uri baseUri,
            string scheme = "HTTP")
        {
            UriBuilder builder = new UriBuilder(baseUri);

            builder.Scheme = scheme;

            return builder.Uri;
        }

        #endregion
    }
}
