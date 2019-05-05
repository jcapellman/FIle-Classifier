using System;

using FileClassifier.lib.Enums;
using FileClassifier.lib.ML.Clustering;
using FileClassifier.lib.Options;

namespace FileClassifier.Trainer
{
    public class Program
    {
        static void Main(string[] args)
        {
#if RELEASE
            var options = TrainerCommandLineParser.Parse(args);
            
            if (options == null)
            {
                return;
            }
#else
            var options = new TrainerCommandLineOptions() { FolderOfData = args[0], LogLevel = LogLevels.DEBUG };
#endif

            try
            {
                new ClusteringEngine().TrainModel(options);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}