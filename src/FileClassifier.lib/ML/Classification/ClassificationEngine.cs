using System;

using FileClassifier.lib.Base;
using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Base;
using FileClassifier.lib.ML.Classification.Objects;
using FileClassifier.lib.Options;

using Microsoft.ML;

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
            var classificationData = new ClassificationData
            {
                NGramText = GetStrings(response.Data, 0, 65536),
                FileGroupType = (int)response.FileGroup
            };

            return (classificationData, $"\"{classificationData.NGramText}\"\t{(int)response.FileGroup}");
        }

        public override string TrainModel(TrainerCommandLineOptions options)
        {
            var fileName = FeatureExtractFolder(options);

            var startDate = DateTime.Now;

            var dataView = MlContext.Data.LoadFromTextFile<ClassificationData>(fileName, hasHeader: false);

            var splitDataView = MlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

            var featuresColumnName = "Features";

            var estimator = MlContext.Transforms.Text.FeaturizeText(nameof(ClassificationData.NGramText))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(ClassificationData.FileGroupType)))
                .Append(MlContext.Transforms.Concatenate(featuresColumnName, nameof(ClassificationData.NGramText), nameof(ClassificationData.FileGroupType)))
                .Append(MlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "Label", featureColumnName: featuresColumnName));

            var model = estimator.Fit(splitDataView.TrainSet);

            Logger<TrainerCommandLineOptions>.Debug($"Model trained in {DateTime.Now.Subtract(startDate).TotalSeconds} seconds", options);

            var predictions = model.Transform(splitDataView.TestSet);

            var metrics = MlContext.BinaryClassification.Evaluate(predictions, "Label");

            Logger<TrainerCommandLineOptions>.Debug($"Accuracy: {metrics.Accuracy} | F1: {metrics.F1Score} | Auc: {metrics.AreaUnderRocCurve}", options);

            return SaveModel(model, splitDataView.TrainSet.Schema, options);
        }
    }
}