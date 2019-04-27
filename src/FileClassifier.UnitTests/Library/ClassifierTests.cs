using System;

using FileClassifier.lib;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileClassifier.UnitTests.Library
{
    [TestClass]
    public class ClassifierTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullOptions()
        {
            Classifier.IsMalicious(null);
        }
    }
}