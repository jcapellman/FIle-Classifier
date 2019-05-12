using System;

using FileClassifier.lib.Base;
using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Base;
using FileClassifier.lib.ML.Classification.Objects;
using FileClassifier.lib.Options;

using Microsoft.ML;
using Microsoft.ML.Data;

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
                NGramText = GetStrings(response.Data, 0, 128),
                Label = false,
                FileGroupType = (int)response.FileGroup
            };

            return (classificationData, $"{response.IsMalicious},{(int)response.FileGroup},\"{classificationData.NGramText}\"");
        }

        public override bool TrainModel(TrainerCommandLineOptions options)
        {
            var fileName = FeatureExtractFolder(options);

            var startDate = DateTime.Now;

            var dataView = MlContext.Data.LoadFromTextFile(path: fileName,
                new[]
                {
                    new TextLoader.Column("Label", DataKind.Boolean, 0),
                    new TextLoader.Column(nameof(ClassificationData.FileGroupType), DataKind.Single, 1),
                    new TextLoader.Column(nameof(ClassificationData.NGramText), DataKind.String, 2)                    
                },
                hasHeader: false,
                separatorChar: ',');

            var splitDataView = MlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

            var featuresColumnName = "Features";

            var dataProcessPipeline = MlContext.Transforms.Text.FeaturizeText(nameof(ClassificationData.NGramText))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(ClassificationData.FileGroupType)))
                .Append(MlContext.Transforms.Concatenate(featuresColumnName, nameof(ClassificationData.NGramText), nameof(ClassificationData.FileGroupType)));
            
            var trainer = MlContext.BinaryClassification.Trainers.FastTree(labelColumnName: nameof(ClassificationData.Label), featureColumnName: featuresColumnName,
                            numberOfLeaves: 20,
                            numberOfTrees: 100,
                            minimumExampleCountPerLeaf: 10,
                            learningRate: 0.2);

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            var model = trainingPipeline.Fit(splitDataView.TrainSet);

            Logger<TrainerCommandLineOptions>.Debug($"Model trained in {DateTime.Now.Subtract(startDate).TotalSeconds} seconds", options);

            var predictions = model.Transform(splitDataView.TestSet);

            var metrics = MlContext.BinaryClassification.Evaluate(predictions, "Label");

            Logger<TrainerCommandLineOptions>.Debug($"Accuracy: {metrics.Accuracy} | F1: {metrics.F1Score} | Auc: {metrics.AreaUnderRocCurve}", options);

            SaveModel(model, splitDataView.TrainSet.Schema, options);

            return true;
        }
    }
}