using System;

using FileClassifier.lib;
using FileClassifier.lib.Enums;
using FileClassifier.lib.Options;

namespace FileClassifier.app
{
    public class Program
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
            var options = new ClassifierCommandLineOptions { FileName = args[0], LogLevel = LogLevels.DEBUG };
#endif

            try
            {
                var classifier = new Classifier(options);

                var result = classifier.Classify();

                Console.WriteLine(result);
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}