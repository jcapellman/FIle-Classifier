using System;

using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Base;
using FileClassifier.lib.ML.Clustering.Objects;

namespace FileClassifier.lib.ML.Clustering
{
    public class ClusteringEngine : BasePrediction
    {
        protected override string MODEL_NAME => "clustering.mdl";

        public override ClassifierResponseItem Predict(ClassifierResponseItem response)
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