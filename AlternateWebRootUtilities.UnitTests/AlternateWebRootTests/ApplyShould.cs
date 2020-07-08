using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlternateWebRootUtilities.UnitTests.AlternateWebRootTests
{
    [TestClass]
    public class ApplyShould
    {
        [TestMethod]
        public void ReturnSiteRelativeAddressGivenBaseUrlNull()
        {
            var config = new AlternateWebRootConfiguration();
            var address = "~/myfile.png";
            var result = AlternateWebRoot.Apply(address, config);
            Assert.AreEqual(address.Substring(1), result);
        }

        [DataRow("http://acme.com", "~/test.jpg", "http://acme.com/test.jpg")]
        [DataRow("http://acme.com/", "~/test.jpg", "http://acme.com/test.jpg")]
        [DataRow("http://acme.com", "~/subfolder/test.jpg", "http://acme.com/subfolder/test.jpg")]
        [DataRow("https://contoso.com", "~/test.jpg?q=123", "https://contoso.com/test.jpg?q=123")]
        [DataRow("https://contoso.com", "/test.jpg", "/test.jpg")]
        [DataRow("https://contoso.com", "//cdn.contoso.com/test.jpg", "//cdn.contoso.com/test.jpg")]
        [DataTestMethod]
        public void ReturnValueGivenNotIsIncludingSiteRelativePaths(string baseAddress, string relativeAddress, string expectedAddress)
        {
            var config = new AlternateWebRootConfiguration
            {
                BaseUrl = new Uri(baseAddress),
                IsIncludingSiteRelativePaths = false,
            };
            var result = AlternateWebRoot.Apply(relativeAddress, config);
            Assert.AreEqual(expectedAddress, result);
        }

        [DataRow("http://acme.com", "~/test.jpg", "http://acme.com/test.jpg")]
        [DataRow("http://acme.com/", "~/test.jpg", "http://acme.com/test.jpg")]
        [DataRow("http://acme.com", "~/subfolder/test.jpg", "http://acme.com/subfolder/test.jpg")]
        [DataRow("https://contoso.com", "~/test.jpg?q=123", "https://contoso.com/test.jpg?q=123")]
        [DataRow("https://contoso.com", "/test.jpg", "https://contoso.com/test.jpg")]
        [DataRow("https://contoso.com", "//cdn.contoso.com/test.jpg", "//cdn.contoso.com/test.jpg")]
        [DataTestMethod]
        public void ReturnValueGivenIsIncludingSiteRelativePaths(string baseAddress, string relativeAddress, string expectedAddress)
        {
            var config = new AlternateWebRootConfiguration
            {
                BaseUrl = new Uri(baseAddress),
                IsIncludingSiteRelativePaths = true,
            };
            var result = AlternateWebRoot.Apply(relativeAddress, config);
            Assert.AreEqual(expectedAddress, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowExceptionGivenNullAddress()
        {
            AlternateWebRoot.Apply(null);
            Assert.Fail("An exception should have been thrown.");
        }
    }
}