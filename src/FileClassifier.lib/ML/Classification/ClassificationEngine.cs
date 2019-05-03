using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Base;
using FileClassifier.lib.ML.Classification.Objects;

namespace FileClassifier.lib.ML.Classification
{
    public class ClassificationEngine : BasePrediction<ClassificationData, ClassificationDataPrediction>
    {
        protected override string MODEL_NAME => "classification.mdl";

        protected override ClassifierResponseItem UpdateResponse(ClassificationDataPrediction prediction, ClassifierResponseItem response)
        {
            response.Confidence = prediction.Score;
            response.IsMalicious = prediction.Prediction;

            return response;
        }

        public override ClassificationData FeatureExtraction(ClassifierResponseItem response)
        {
            return new ClassificationData();
        }

        public override bool TrainModel(string dateFile)
        {
            throw new System.NotImplementedException();
        }
    }
}