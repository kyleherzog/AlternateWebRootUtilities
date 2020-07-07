using System;

namespace AlternateWebRootUtilities
{
    /// <summary>
    /// A static class for applying alternate web root paths to given web paths.
    /// </summary>
    public static class AlternateWebRoot
    {
        /// <summary>
        /// Applies the alternate web root path to the given address, replacing a leading "~" with the configuration BaseUrl.
        /// </summary>
        /// <param name="address">The address onto which to apply the alternate web root.</param>
        /// <param name="config">The <see cref="AlternateWebRootConfiguration"/> to use.</param>
        /// <returns>
        /// If the address starts with a "~" character, the address is returned with the "~" character replaced
        /// with the alternate web root; otherwise, the original address.
        /// </returns>
        public static string Apply(string address, AlternateWebRootConfiguration config = null)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (config == null)
            {
                config = AlternateWebRootConfiguration.Global;
            }

            if (address.StartsWith("~", StringComparison.Ordinal))
            {
                address = address.Substring(1);
                address = config.AddressPrefix + address;
            }
            else if (config.IsIncludingSiteRelativePaths && address.StartsWith("/", StringComparison.Ordinal))
            {
                address = config.AddressPrefix + address;
            }

            return address;
        }
    }
}