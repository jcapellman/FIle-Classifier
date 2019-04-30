using FileClassifier.lib.Common;
using FileClassifier.lib.ML.Base.Objects;

using Microsoft.ML;

namespace FileClassifier.lib.ML.Base
{
    public abstract class BasePrediction<T, TK> where T : BaseData where TK : BasePredictionData
    {
        protected abstract string MODEL_NAME { get; }

        protected static readonly MLContext MlContext = new MLContext(Common.Constants.ML_SEED);

        public abstract ClassifierResponseItem Predict(ClassifierResponseItem response);

        public abstract T FeatureExtraction(ClassifierResponseItem response);
    }
}