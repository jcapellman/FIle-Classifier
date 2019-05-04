using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Base.Objects;

using Microsoft.ML;

namespace FileClassifier.lib.ML.Base
{
    public abstract class BasePrediction<T, TK> where T : BaseData where TK : BasePredictionData, new()
    {
        protected abstract string MODEL_NAME { get; }

        protected static readonly MLContext MlContext = new MLContext(Common.Constants.ML_SEED);

        public ClassifierResponseItem Predict(ClassifierResponseItem response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var model = MlContext.Model.Load(MODEL_NAME, out var schema);

            var predictor = MlContext.Model.CreatePredictionEngine<T, TK>(model);

            var data = FeatureExtraction(response);

            var result = predictor.Predict(data.Data);

            return UpdateResponse(result, response);
        }

        protected string FeatureExtractFolder(TrainerCommandLineOptions options)
        {
            var fileName = Path.GetTempFileName();

            var files = Directory.GetFiles(options.FolderOfData);

            var extractions = new ConcurrentQueue<string>();

            Parallel.ForEach(files, file =>
            {
                var extraction = FeatureExtraction(new ClassifierResponseItem(File.ReadAllBytes(file), file));

                extractions.Enqueue(extraction.Output);
            });

            File.WriteAllText(fileName, string.Join(System.Environment.NewLine, extractions));

            return fileName;
        }

        protected abstract ClassifierResponseItem UpdateResponse(TK prediction, ClassifierResponseItem response);

        public abstract (T Data, string Output) FeatureExtraction(ClassifierResponseItem response);

        public abstract bool TrainModel(TrainerCommandLineOptions options);
    }
}