using System;
using System.IO;
using System.Runtime.CompilerServices;

using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Clustering;

[assembly:InternalsVisibleTo("FileClassifier.UnitTests")]
namespace FileClassifier.lib
{
    public class Classifier
    {
        private readonly Options _options;

        public Classifier(Options option) {
            SanityCheckOptions(option);

            _options = option;
        }

        private static void SanityCheckOptions(Options option)
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

        internal ClassifierResponseItem InitializeResponse(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            byte[] data;

            try
            {
                data = File.ReadAllBytes(fileName);
            } catch (Exception ex)
            {
                Log(ex);

                return new ClassifierResponseItem(ex);
            }

            return new ClassifierResponseItem(data);
        }

        private void Log(Exception exception) => Log($"{exception} (Options: {_options})");

        private void Log(string message)
        {
            if (!_options.Verbose)
            {
                return;
            }

            Console.WriteLine($"{DateTime.Now}: {message}");
        }

        public ClassifierResponseItem Classify()
        {
            var response = InitializeResponse(_options.FileName);

            // If in error on initialization - bail
            if (response.Status == Enums.ClassifierStatus.ERROR)
            {
                return response;
            }

            Log($"Classifying {_options.FileName}...");
        
            // Classify which file type
            response = response.DetermineFileGroup();

            Log($"Clustering Result: {response.FileGroup} | Status: {response.Status}");

            // Classify if malicious or not based on the type

            return response;
        }
    }
}