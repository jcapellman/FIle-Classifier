using System;
using System.IO;

using FileClassifier.lib.Common;
using FileClassifier.lib.Helpers;

namespace FileClassifier.lib
{
    public class Classifier
    {
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

        public static ClassifierResponseItem Classify(Options option)
        {
            SanityCheckOptions(option);

            var response = new ClassifierResponseItem();

            if (option.Verbose)
            {
                Console.WriteLine($"Classifying {option.FileName}...");
            }

            var data = File.ReadAllBytes(option.FileName);

            response.SHA1Hash = data.ToSHA1();

            Console.WriteLine($"Computed SHA1 to {response.SHA1Hash}...");

            return response;
        }
    }
}