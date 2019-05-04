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

            var result = predictor.Predict(data);

            return UpdateResponse(result, response);
        }

        protected string FeatureExtractFolder(TrainerCommandLineOptions options)
        {
            var fileName = Path.GetTempFileName();

            var files = Directory.GetFiles(options.FolderOfData);

            var extractions = new ConcurrentQueue<T>();

            Parallel.ForEach(files, file =>
            {
                var extraction = FeatureExtraction(new ClassifierResponseItem(File.ReadAllBytes(file), file));

                extractions.Enqueue(extraction);
            });

            File.WriteAllText(fileName, string.Join(System.Environment.NewLine, extractions.Select(a => a)));

            return fileName;
        }

        protected abstract ClassifierResponseItem UpdateResponse(TK prediction, ClassifierResponseItem response);

        public abstract T FeatureExtraction(ClassifierResponseItem response);

        public abstract bool TrainModel(TrainerCommandLineOptions options);
    }
}