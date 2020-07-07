using System;

namespace AlternateWebRootUtilities
{
    /// <summary>
    /// Defines how the <see cref="AlternateWebRoot"/> should behave.
    /// </summary>
    public class AlternateWebRootConfiguration
    {
        private Uri baseUrl;

        /// <summary>
        /// Gets the instance of the <see cref="AlternateWebRootConfiguration"/> that will be used globally.
        /// </summary>
        public static AlternateWebRootConfiguration Global { get; internal set; }

        /// <summary>
        /// Gets or sets the <see cref="Uri"/> that should serve as the base location of the web root.
        /// </summary>
        public Uri BaseUrl
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
        public bool IsGloballyEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not to target paths that start with a '/' character.
        /// </summary>
        public bool IsIncludingSiteRelativePaths { get; set; }

        /// <summary>
        /// Gets the string representation of the address that will be prefixed when applying to web root relative addresses.
        /// </summary>
        internal string AddressPrefix { get; private set; }
    }
}