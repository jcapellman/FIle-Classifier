using System;
using System.Collections.Generic;
using System.Linq;

using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.lib.Databases.Tables;

using LiteDB;

namespace FileClassifier.JobManager.lib.Databases
{
    public class LiteDBDatabase : IDatabase
    {
        private const string DbFilename = "data.db";

        public bool DeleteJob(Guid id)
        {
            using (var db = new LiteDatabase(DbFilename))
            {
                return db.GetCollection<Jobs>().Delete(a => a.ID == id) > 0;
            }
        }

        public bool AddJob(Jobs item)
        {
            using (var db = new LiteDatabase(DbFilename))
            {
                return db.GetCollection<Jobs>().Insert(item) != null;
            }
        }

        public bool UpdateJob(Jobs item)
        {
            using (var db = new LiteDatabase(DbFilename))
            {
                return db.GetCollection<Jobs>().Update(item);
            }
        }

        public Jobs GetJob(Guid id)
        {
            using (var db = new LiteDatabase(DbFilename))
            {
                return db.GetCollection<Jobs>().FindOne(a => a.ID == id);
            }
        }

        public List<Jobs> GetJobs()
        {
            using (var db = new LiteDatabase(DbFilename))
            {
                return db.GetCollection<Jobs>().FindAll().ToList();
            }
        }

        public void AddUpdateHost(Hosts host)
        {
            using (var db = new LiteDatabase(DbFilename))
            {
                db.GetCollection<Hosts>().Upsert(host);
            }
        }

        public void DeleteHost(Guid id)
        {
            using (var db = new LiteDatabase(DbFilename))
            {
                db.GetCollection<Hosts>().Delete(a => a.ID == id);
            }
        }

        public List<Hosts> GetHosts()
        {
            using (var db = new LiteDatabase(DbFilename))
            {
                return db.GetCollection<Hosts>().FindAll().ToList();
            }
        }
    }
}