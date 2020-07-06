﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlternateWebRootUtilities.UnitTests.AlternateWebRootTests
{
    [TestClass]
    public class ApplyShould
    {
        [TestMethod]
        public void ReturnSiteRelativeAddressGivenBaseUrlNull()
        {
            AlternateWebRoot.BaseUrl = null;
            var address = "~/myfile.png";
            var result = AlternateWebRoot.Apply(address);
            Assert.AreEqual(address.Substring(1), result);
        }

        [TestMethod]
        public void ReturnOriginalValueGivenNotWebRootRelativeAddress()
        {
            AlternateWebRoot.BaseUrl = new Uri("http://localhost:5784");
            var address = "/myfile2.png";
            var result = AlternateWebRoot.Apply(address);
            Assert.AreEqual(address, result);
        }

        [DataRow("http://acme.com", "~/test.jpg", "http://acme.com/test.jpg")]
        [DataRow("http://acme.com/", "~/test.jpg", "http://acme.com/test.jpg")]
        [DataRow("http://acme.com", "~/subfolder/test.jpg", "http://acme.com/subfolder/test.jpg")]
        [DataRow("https://contoso.com", "~/test.jpg?q=123", "https://contoso.com/test.jpg?q=123")]
        [DataTestMethod]
        public void ReturnValueGivenSpecifiedBaseUrlAndAddress(string baseAddress, string relativeAddress, string expectedAddress)
        {
            AlternateWebRoot.BaseUrl = new Uri(baseAddress);
            var result = AlternateWebRoot.Apply(relativeAddress);
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