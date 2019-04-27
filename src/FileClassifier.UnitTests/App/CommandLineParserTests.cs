using FileClassifier.app;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileClassifier.UnitTests.App
{
    [TestClass]
    public class CommandLineParserTests
    {
        [TestMethod]
        public void NullArguments()
        {
            var result = CommandLineParser.Parse(null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void NoFileArgument()
        {
            var result = CommandLineParser.Parse(new string[] { "--verbose" });

            Assert.IsNull(result);
        }
    }
}