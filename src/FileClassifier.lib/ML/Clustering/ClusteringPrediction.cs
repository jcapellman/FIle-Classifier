using System;

using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Clustering.Objects;

using Microsoft.ML;

namespace FileClassifier.lib.ML.Clustering
{
    public static class ClusteringPrediction
    {
        private static readonly MLContext MlContext = new MLContext(Common.Constants.ML_SEED);

        private const string MODEL_NAME = "clustering.mdl";

        public static ClassifierResponseItem DetermineFileGroup(this ClassifierResponseItem response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var model = MlContext.Model.Load(MODEL_NAME, out var schema);

            var predictor = MlContext.Model.CreatePredictionEngine<ClusterData, ClusterDataPrediction>(model);

            var data = new ClusterData();

            // TODO: Feature Extraction

            var result = predictor.Predict(data);

            // TODO: Map Id to Group

            // TODO: Set the Group in the response

            return response;
        }
    }
}