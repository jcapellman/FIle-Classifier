using FileClassifier.lib;

namespace FileClassifier.app
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = CommandLineParser.Parse(args);
            
            if (options == null)
            {
                return;
            }

            Classifier.IsMalicious(options);
        }
    }
}