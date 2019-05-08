using Microsoft.Spark.Sql;

namespace FileClassifier.SparkWorker
{
    public class Program
    {
        static void Main(string[] args)
        {
            var spark = SparkSession.Builder().GetOrCreate();
        }
    }
}