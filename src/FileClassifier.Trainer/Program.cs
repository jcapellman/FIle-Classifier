using System;

using FileClassifier.lib.Enums;
using FileClassifier.lib.ML.Classification;
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
            var options = new TrainerCommandLineOptions() { FolderOfData = args[0], LogLevel = LogLevels.DEBUG, ModelType = Enum.Parse<ModelType>(args[1]) };
#endif

            try
            {
                switch (options.ModelType)
                {
                    case ModelType.CLASSIFICATION:
                        new ClassificationEngine().TrainModel(options);
                        break;
                    case ModelType.CLUSTERING:
                        new ClusteringEngine().TrainModel(options);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}