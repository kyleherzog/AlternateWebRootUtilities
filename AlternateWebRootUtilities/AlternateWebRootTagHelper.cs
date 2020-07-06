using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AlternateWebRootUtilities
{
    [HtmlTargetElement("img")]
    [HtmlTargetElement("script")]
    [HtmlTargetElement("source")]
    [HtmlTargetElement("link")]
    public class AlternateWebRootTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-alternate-web-root")]
        public bool IsEnabled { get; set; }

        [HtmlAttributeName("asp-append-version")]
        public bool? IsVersioned { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            if (AlternateWebRoot.BaseUrl != null)
            {
                if (AlternateWebRoot.IsGloballyEnabled || IsEnabled)
                {
                    var attributeName = GetAttributeName(context.TagName);

                    var address = context.AllAttributes[attributeName].Value.ToString();
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

        private string AppendVersion(string requestedAddress, string alternateAddress)
        {
            EnsureFileVersionProvider();
            var versionedAddress = FileVersionProvider.AddFileVersionToPath(new PathString("/"), requestedAddress.Substring(1));

            var queryStringIndex = versionedAddress.IndexOf("?");
            if (queryStringIndex > -1)
            {
                var versionedQuery = versionedAddress.Substring(queryStringIndex);
                var alternateQueryIndex = alternateAddress.IndexOf("?");
                if (alternateQueryIndex > -1)
                {
                    alternateAddress = alternateAddress.Substring(0, alternateQueryIndex);
                }
                alternateAddress = alternateAddress + versionedQuery;
            }

            return alternateAddress;
        }

        internal IFileVersionProvider FileVersionProvider { get; private set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private void EnsureFileVersionProvider()
        {
            if (FileVersionProvider == null)
            {
                FileVersionProvider = ViewContext.HttpContext.RequestServices.GetRequiredService<IFileVersionProvider>();
            }
        }

        private string GetAttributeName(string tagName)
        {
            switch (tagName)
            {
                case "source":
                    return "srcset";

                case "link":
                    return "href";

                default:
                    return "src";
            }
        }
    }
}