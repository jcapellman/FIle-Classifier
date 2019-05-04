using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using FileClassifier.lib.Common;
using FileClassifier.lib.Enums;
using FileClassifier.lib.ML.Base;
using FileClassifier.lib.ML.Clustering.Objects;

using Microsoft.ML;

namespace FileClassifier.lib.ML.Clustering
{
    public class ClusteringEngine : BasePrediction<ClusterData, ClusterDataPrediction>
    {
        protected override string MODEL_NAME => "clustering.mdl";

        protected override ClassifierResponseItem UpdateResponse(ClusterDataPrediction prediction, ClassifierResponseItem response)
        {
            response.FileGroup = (FileGroupType) prediction.PredictedClusterId;

            return response;
        }

        public override ClusterData FeatureExtraction(ClassifierResponseItem response)
        {
            
            return new ClusterData();
        }

        private static FileGroupType GetGroupType(string fileName)
        {
            var groupType = Enum.GetNames(typeof(FileGroupType))
                .FirstOrDefault(a => fileName.Contains(a, StringComparison.InvariantCultureIgnoreCase));

            return string.IsNullOrEmpty(groupType) ? FileGroupType.UNKNOWN : Enum.Parse<FileGroupType>(groupType);
        }

        private string FeatureExtractFolder(TrainerCommandLineOptions options)
        {
            var fileName = Path.GetTempFileName();

            var files = Directory.GetFiles(options.FolderOfData);

            var extractions = new ConcurrentQueue<ClusterData>();

            Parallel.ForEach(files, file =>
            {
                var extraction = FeatureExtraction(new ClassifierResponseItem(File.ReadAllBytes(file)));

                extractions.Enqueue(extraction);
            });

            File.WriteAllText(fileName, string.Join(System.Environment.NewLine, extractions.Select(a => a)));

            return fileName;
        }

        public override bool TrainModel(TrainerCommandLineOptions options)
        {
            var fileName = FeatureExtractFolder(options);

            var dataView = MlContext.Data.LoadFromTextFile<ClusterData>(fileName, hasHeader: false, separatorChar: ',');

            var featuresColumnName = "Features";

            var pipeline = MlContext.Transforms
                .Concatenate(featuresColumnName, "Size", "Grams")
                .Append(MlContext.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: Enum.GetNames(typeof(FileGroupType)).Length));

            var model = pipeline.Fit(dataView);

            using (var fileStream = new FileStream(MODEL_NAME, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
            {
                MlContext.Model.Save(model, dataView.Schema, fileStream);
            }

            return true;
        }
    }
}