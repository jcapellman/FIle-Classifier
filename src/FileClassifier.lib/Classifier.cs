using System;
using System.IO;

using FileClassifier.lib.Common;

namespace FileClassifier.lib
{
    public class Classifier
    {
        private Options _options;

        public Classifier(Options option) {
            SanityCheckOptions(option);

            _options = option;
        }

        internal static void SanityCheckOptions(Options option)
        {
            if (option is null)
            {
                throw new ArgumentNullException(nameof(option));
            }

            if (string.IsNullOrEmpty(option.FileName))
            {
                throw new ArgumentNullException(nameof(option.FileName));
            }

            if (!File.Exists(option.FileName))
            {
                throw new FileNotFoundException($"{option.FileName} was not found...");
            }
        }

        private ClassifierResponseItem InitializeResponse(string fileName)
        {
            var data = File.ReadAllBytes(fileName);

            return new ClassifierResponseItem(data);
        }

        public ClassifierResponseItem Classify()
        {
            var response = InitializeResponse(_options.FileName);

            if (_options.Verbose)
            {
                Console.WriteLine($"Classifying {_options.FileName}...");
            }

            return response;
        }
    }
}