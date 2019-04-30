using FileClassifier.lib.Common;

using Microsoft.ML;

namespace FileClassifier.lib.ML.Base
{
    public abstract class BasePrediction
    {
        protected abstract string MODEL_NAME { get; }

        protected static readonly MLContext MlContext = new MLContext(Common.Constants.ML_SEED);

        public abstract ClassifierResponseItem Predict(ClassifierResponseItem response);
    }
}