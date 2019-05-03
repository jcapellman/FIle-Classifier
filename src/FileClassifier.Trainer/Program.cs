using System;

using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Clustering;

namespace FileClassifier.Trainer
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
            var options = new Options { FileName = args[0], Verbose = true };
#endif

            try
            {
                var result = new ClusteringEngine().TrainModel(options);

                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}