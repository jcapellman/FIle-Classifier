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
            new Classifier(null).Classify();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullFileNameOptions()
        {
            new Classifier(new Options()).Classify();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FileDoesntExistOptions()
        {
            new Classifier(new Options
            {
                FileName = DateTime.Now.Ticks.ToString()
            });
        }

        #region InitialResponse

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InitialResponse_Null()
        {
            var classifier = new Classifier(new Options {FileName = "test"});

            classifier.InitializeResponse(null);
        }
        #endregion
    }
}