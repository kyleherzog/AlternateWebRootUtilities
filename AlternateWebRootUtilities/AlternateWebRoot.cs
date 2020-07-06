using System;

namespace AlternateWebRootUtilities
{
    /// <summary>
    /// A static class for applying alternate web root paths to given web paths.
    /// </summary>
    public static class AlternateWebRoot
    {
        private static Uri baseUrl;

        /// <summary>
        /// Gets or sets the <see cref="Uri"/> that should serve as the base location of the web root.
        /// </summary>
        public static Uri BaseUrl
        {
            get
            {
                return baseUrl;
            }

            set
            {
                baseUrl = value;
                AddressPrefix = baseUrl?.ToString()?.TrimEnd('/');
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the applying of the alternate web root to all supported HTML tags.
        /// </summary>
        public static bool IsGloballyEnabled { get; set; }

        /// <summary>
        /// Gets the string representation of the address that will be prefixed when applying to web root relative addresses.
        /// </summary>
        internal static string AddressPrefix { get; private set; }

        /// <summary>
        /// Applies the alternate web root path to the given address, replacing a leading "~" with the <see cref="BaseUrl"/>.
        /// </summary>
        /// <param name="address">The address onto which to apply the alternate web root</param>
        /// <returns></returns>
        public static string Apply(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (address.StartsWith("~"))
            {
                address = address.Substring(1);
                address = AlternateWebRoot.AddressPrefix + address;
            }

            return address;
        }

        /// <summary>
        /// Sets the <see cref="IsGloballyEnabled"/> property to true and sets the <see cref="BaseUrl"/> to the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="baseUrl">The value to which to set the <see cref="BaseUrl"/> property.</param>
        public static void EnableGlobally(Uri baseUrl)
        {
            BaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
            IsGloballyEnabled = true;
        }
    }
}