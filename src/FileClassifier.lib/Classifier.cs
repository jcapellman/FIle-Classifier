using System;

using FileClassifier.lib.Common;

namespace FileClassifier.lib
{
    public class Classifier
    {
        public static bool IsMalicious(Options option)
        {
            if (option is null)
            {
                throw new ArgumentNullException(nameof(option));
            }

            if (option.Verbose)
            {
                Console.WriteLine($"Classifying {option.FileName}");
            }

            return false;
        }
    }
}