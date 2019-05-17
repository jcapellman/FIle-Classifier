using FileClassifier.lib.Common;

namespace FileClassifier.Web.Models
{
    public class ResultViewModel
    {
        public ClassifierResponseItem ClusterResult { get; set; }

        public ClassifierResponseItem ClassificationResult { get; set; }
    }
}