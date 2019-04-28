using FileClassifier.lib.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileClassifier.UnitTests.Library.Helpers
{
    [TestClass]
    public class HashingTests
    {
        [TestMethod]
        public void SHA1_NullBytes()
        {
            byte[] data = null;

            var result = data.ToSHA1();

            Assert.AreEqual(lib.Common.Constants.RESPONSE_ERROR, result);
        }

        [TestMethod]
        public void SHA1_EmptyBytes()
        {
            byte[] data = new byte[0];

            var result = data.ToSHA1();

            Assert.AreEqual(lib.Common.Constants.RESPONSE_ERROR, result);
        }

        [TestMethod]
        public void SHA1_TestBytes()
        {
            byte[] data = new byte[3] { 0x3c, 0x4d, 0x5e };

            var result = data.ToSHA1();

            Assert.AreEqual("2C2A964127CCF7D9A8B00362BAC95FD9EBE95238", result);
        }
    }
}