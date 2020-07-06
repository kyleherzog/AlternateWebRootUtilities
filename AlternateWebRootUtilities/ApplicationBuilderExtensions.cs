using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;

namespace AlternateWebRootUtilities
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAlternateWebRoot(this IApplicationBuilder appBuilder, Uri baseUrl = null)
        {
            if (appBuilder == null)
            {
                throw new ArgumentNullException(nameof(appBuilder));
            }

            if (baseUrl == null)
            {
                var config = (IConfiguration)appBuilder.ApplicationServices.GetService(typeof(IConfiguration));
                var section = config?.GetSection("AlternateWebRoot");
                var value = section?.Value;
                if (!string.IsNullOrEmpty(value))
                {
                    baseUrl = new Uri(value);
                }
            }

            if (baseUrl != null)
            {
                AlternateWebRoot.EnableGlobally(baseUrl);
            }

            return appBuilder;
        }
    }
}