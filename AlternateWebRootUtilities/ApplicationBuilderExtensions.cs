using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace AlternateWebRootUtilities
{
    /// <summary>
    /// Extension methods for use on <see cref="IApplicationBuilder"/> objects.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Initializes the AlternateWebRoot as being enabled globally for use in the application.
        /// </summary>
        /// <param name="appBuilder">The current <see cref="IApplicationBuilder"/> being configured.</param>
        /// <param name="baseUrl">The <see cref="Uri"/> to use as the alternative for web root relative paths.</param>
        /// <returns>The original <see cref="IApplicationBuilder"/> passed in.</returns>
        public static IApplicationBuilder UseAlternateWebRoot(this IApplicationBuilder appBuilder, Uri baseUrl)
        {
            var config = new AlternateWebRootConfiguration
            {
                BaseUrl = baseUrl,
            };

            return UseAlternateWebRoot(appBuilder, config);
        }

        /// <summary>
        /// Initializes the AlternateWebRoot for use in the application.
        /// </summary>
        /// <param name="appBuilder">The current <see cref="IApplicationBuilder"/> being configured.</param>
        /// <param name="altConfig">The <see cref="AlternateWebRootConfiguration"/> to use globally.</param>
        /// <returns>The original <see cref="IApplicationBuilder"/> passed in.</returns>
        public static IApplicationBuilder UseAlternateWebRoot(this IApplicationBuilder appBuilder, AlternateWebRootConfiguration altConfig = null)
        {
            if (appBuilder == null)
            {
                throw new ArgumentNullException(nameof(appBuilder));
            }

            if (altConfig == null)
            {
                var configuration = (IConfiguration)appBuilder.ApplicationServices.GetService(typeof(IConfiguration));
                altConfig = configuration?.GetSection("AlternateWebRoot")?.Get<AlternateWebRootConfiguration>();
            }

            AlternateWebRootConfiguration.Global = altConfig ?? new AlternateWebRootConfiguration();

            return appBuilder;
        }
    }
}