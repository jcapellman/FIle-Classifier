namespace FileClassifier.JobManager.Worker.Common
{
    public class Constants
    {
        public const int LOOP_INTERVAL_MS = 60000;

        public const int LOOP_ERROR_INTERVAL_MS = LOOP_INTERVAL_MS * 5;

        public const string DEFAULT_SERVER_URL = "http://localhost:5000/api/";
    }
}