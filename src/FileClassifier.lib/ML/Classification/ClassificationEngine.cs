using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Base;
using FileClassifier.lib.ML.Classification.Objects;
using FileClassifier.lib.Options;

namespace FileClassifier.lib.ML.Classification
{
    public class ClassificationEngine : BasePrediction<ClassificationData, ClassificationDataPrediction>
    {
        protected override string MODEL_NAME => "classification.mdl";

        protected override ClassifierResponseItem UpdateResponse(ClassificationDataPrediction prediction, ClassifierResponseItem response, ClassifierCommandLineOptions options)
        {
            response.Confidence = prediction.Score;
            response.IsMalicious = prediction.Prediction;

            return response;
        }

        public override (ClassificationData Data, string Output) FeatureExtraction(ClassifierResponseItem response)
        {
            return (new ClassificationData(), string.Empty);
        }

        public override bool TrainModel(TrainerCommandLineOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}