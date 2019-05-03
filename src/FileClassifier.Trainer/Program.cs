using System;

using FileClassifier.lib.ML.Clustering;

namespace FileClassifier.Trainer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var result = new ClusteringEngine().TrainModel(args[0]);

            Console.WriteLine(!result ? "Failed to train model" : "Successfully trained model");
        }
    }
}