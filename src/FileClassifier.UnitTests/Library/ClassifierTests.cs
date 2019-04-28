using System;
using System.IO;

using FileClassifier.lib;
using FileClassifier.lib.Common;

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
            Classifier.Classify(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullFileNameOptions()
        {
            Classifier.Classify(new Options());
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FileDoesntExistOptions()
        {
            Classifier.Classify(new Options
            {
                FileName = DateTime.Now.Ticks.ToString()
            });
        }
    }
}