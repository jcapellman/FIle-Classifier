using System;

using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Base;
using FileClassifier.lib.ML.Classification.Objects;

namespace FileClassifier.lib.ML.Classification
{
    public class ClassificationEngine : BasePrediction<ClassificationData, ClassificationDataPrediction>
    {
        protected override string MODEL_NAME => "classification.mdl";

        public override ClassifierResponseItem Predict(ClassifierResponseItem response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var model = MlContext.Model.Load(MODEL_NAME, out var schema);

            var predictor = MlContext.Model.CreatePredictionEngine<ClassificationData, ClassificationDataPrediction>(model);

            var data = FeatureExtraction(response);

            var result = predictor.Predict(data);

            response.Confidence = result.Score;
            response.IsMalicious = result.Prediction;

            return response;
        }

        public override ClassificationData FeatureExtraction(ClassifierResponseItem response)
        {
            return new ClassificationData();
        }
    }
}