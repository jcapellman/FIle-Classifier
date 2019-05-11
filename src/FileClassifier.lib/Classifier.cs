using System;
using System.IO;
using System.Runtime.CompilerServices;

using FileClassifier.lib.Base;
using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Classification;
using FileClassifier.lib.ML.Clustering;
using FileClassifier.lib.Options;

[assembly:InternalsVisibleTo("FileClassifier.UnitTests")]
namespace FileClassifier.lib
{
    public class Classifier
    {
        private readonly ClassifierCommandLineOptions _options;

        public Classifier(ClassifierCommandLineOptions option) {
            SanityCheckOptions(option);

            _options = option;
        }

        private static void SanityCheckOptions(ClassifierCommandLineOptions option)
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
                Logger<ClassifierCommandLineOptions>.Error(ex, _options);

                return new ClassifierResponseItem(ex);
            }

            return new ClassifierResponseItem(data, fileName, false);
        }

        public ClassifierResponseItem Classify()
        {
            var response = InitializeResponse(_options.FileName);

            // If in error on initialization - bail
            if (response.Status == Enums.ClassifierStatus.ERROR)
            {
                return response;
            }

            Logger<ClassifierCommandLineOptions>.Debug($"Classifying {_options.FileName}...", _options);
        
            // Classify which file type
            response = new ClusteringEngine().Predict(response, _options);

            Logger<ClassifierCommandLineOptions>.Debug($"Clustering Result: {response.FileGroup} | Status: {response.Status}", _options);

            // Classify if malicious or not based on the type
            response = new ClassificationEngine().Predict(response, _options);

            Logger<ClassifierCommandLineOptions>.Debug($"Classification Confidence: {response.Confidence} | Malicious: {response.IsMalicious} | Status: {response.Status}", _options);

            return response;
        }
    }
}