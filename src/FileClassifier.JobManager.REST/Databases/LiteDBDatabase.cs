using System;

using FileClassifier.JobManager.REST.Databases.Base;
using FileClassifier.JobManager.REST.Databases.Tables;

using LiteDB;

namespace FileClassifier.JobManager.REST.Databases
{
    public class LiteDBDatabase : IDatabase
    {
        private const string DB_FILENAME = "data.db";

        public bool DeleteJob(Guid id)
        {
            using (var db = new LiteDatabase(DB_FILENAME))
            {
                return db.GetCollection<Jobs>().Delete(a => a.ID == id) > 0;
            }
        }

        public bool AddJob(Jobs item)
        {
            using (var db = new LiteDatabase(DB_FILENAME))
            {
                return db.GetCollection<Jobs>().Insert(item) != null;
            }
        }

        public bool UpdateJob(Jobs item)
        {
            using (var db = new LiteDatabase(DB_FILENAME))
            {
                return db.GetCollection<Jobs>().Update(item);
            }
        }
    }
}