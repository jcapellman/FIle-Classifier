using System;
using System.IO;

using FileClassifier.lib.Common;

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

        public static bool IsMalicious(Options option)
        {
            SanityCheckOptions(option);

            if (option.Verbose)
            {
                Console.WriteLine($"Classifying {option.FileName}...");
            }

            return false;
        }
    }
}