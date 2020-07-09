using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace AlternateWebRootUtilities
{
    /// <summary>
    /// A razor page tag helper that maps web root relative paths with a specified alternative location.
    /// </summary>
    [HtmlTargetElement("img")]
    [HtmlTargetElement("script")]
    [HtmlTargetElement("source")]
    [HtmlTargetElement("link")]
    public class AlternateWebRootTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the alternate web root should be applied to this in tag.
        /// </summary>
        [HtmlAttributeName("asp-alternate-web-root")]
        public bool? IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the link should include a version querystring parameter.
        /// </summary>
        [HtmlAttributeName("asp-append-version")]
        public bool? IsVersioned { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ViewContext"/> from the containing razor view.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Gets the <see cref="IFileVersionProvider"/> to use for building versioned file links.
        /// </summary>
        internal IFileVersionProvider FileVersionProvider { get; private set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AlternateWebRootConfiguration.Global.BaseUrl != null)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                if (output == null)
                {
                    throw new ArgumentNullException(nameof(output));
                }

                if ((AlternateWebRootConfiguration.Global.IsGloballyEnabled && (!IsEnabled.HasValue || IsEnabled.Value))
                    || (IsEnabled.HasValue && IsEnabled.Value))
                {
                    var attributeName = GetAttributeName(context.TagName);

                    if (context.AllAttributes.ContainsName(attributeName))
                    {
                        var address = context.AllAttributes[attributeName].Value.ToString();
                        if (!string.IsNullOrEmpty(address))
                        {
                            var alternateAddress = AlternateWebRoot.Apply(address);
                            if (address != alternateAddress)
                            {
                                if (IsVersioned.HasValue && IsVersioned.Value)
                                {
                                    alternateAddress = AppendVersion(address, alternateAddress);
                                }

                                output.Attributes.RemoveAll(attributeName);
                                output.Attributes.Add(attributeName, alternateAddress);
                            }
                        }
                    }
                }
            }

            base.Process(context, output);
        }

        private static string GetAttributeName(string tagName)
        {
            return tagName switch
            {
                "source" => "srcset",
                "link" => "href",
                _ => "src",
            };
        }

        private string AppendVersion(string requestedAddress, string alternateAddress)
        {
            EnsureFileVersionProvider();
            var versionedAddress = FileVersionProvider.AddFileVersionToPath(new PathString("/"), requestedAddress.Substring(1));

            var queryStringIndex = versionedAddress.IndexOf("?", StringComparison.Ordinal);
            if (queryStringIndex > -1)
            {
                var versionedQuery = versionedAddress.Substring(queryStringIndex);
                var alternateQueryIndex = alternateAddress.IndexOf("?", StringComparison.Ordinal);
                if (alternateQueryIndex > -1)
                {
                    alternateAddress = alternateAddress.Substring(0, alternateQueryIndex);
                }

                alternateAddress += versionedQuery;
            }

            return alternateAddress;
        }

        private void EnsureFileVersionProvider()
        {
            if (FileVersionProvider == null)
            {
                FileVersionProvider = ViewContext.HttpContext.RequestServices.GetRequiredService<IFileVersionProvider>();
            }
        }
    }
}