using System;
using System.IO;

using FileClassifier.lib.Common;

namespace FileClassifier.lib
{
    public class Classifier
    {
        private Options _options;

        public Classifier(Options option) {
            SanityCheckOptions(option);

            _options = option;
        }

        internal static void SanityCheckOptions(Options option)
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

        private ClassifierResponseItem InitializeResponse(string fileName)
        {
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

        private void Log(Exception exception) => Log(exception.ToString());

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
        
            return response;
        }
    }
}