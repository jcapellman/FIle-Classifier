﻿using System;

using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Clustering;

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
            var options = new TrainerCommandLineOptions() { FolderOfData = args[0], Verbose = true };
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