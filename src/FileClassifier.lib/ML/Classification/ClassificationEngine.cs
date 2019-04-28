using System;

using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Classification.Objects;

using Microsoft.ML;

namespace FileClassifier.lib.ML.Classification
{
    public static class ClassificationPrediction
    {
        private static readonly MLContext MlContext = new MLContext(Common.Constants.ML_SEED);

        private const string MODEL_NAME = "classification.mdl";

        public static ClassifierResponseItem DetermineClassification(this ClassifierResponseItem response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var model = MlContext.Model.Load(MODEL_NAME, out var schema);

            var predictor = MlContext.Model.CreatePredictionEngine<ClassificationData, ClassificationDataPrediction>(model);

            var data = new ClassificationData();

            // TODO: Feature Extraction

            var result = predictor.Predict(data);

            response.Confidence = result.Score;
            response.IsMalicious = result.Prediction;

            return response;
        }
    }
}