using System;

using FileClassifier.lib;
using FileClassifier.lib.Common;

namespace FileClassifier.app
{
    class Program
    {
        static void Main(string[] args)
        {
#if RELEASE
            var options = CommandLineParser.Parse(args);
            
            if (options == null)
            {
                return;
            }
#else
            var options = new Options { FileName = args[0], Verbose = true };
#endif

            try
            {
                var result = Classifier.Classify(options);

                Console.WriteLine(result);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}