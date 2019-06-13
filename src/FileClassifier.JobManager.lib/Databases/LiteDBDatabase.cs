using System;
using System.Collections.Generic;
using System.Linq;

using FileClassifier.JobManager.lib.Databases.Base;
using FileClassifier.JobManager.lib.Databases.Tables;

using LiteDB;
using Newtonsoft.Json;

using NLog;

namespace FileClassifier.JobManager.lib.Databases
{
    public class LiteDBDatabase : IDatabase
    {
        private readonly NLog.Logger Log = LogManager.GetCurrentClassLogger();

        private const string DbFilename = "data.db";

        public bool DeleteJob(Guid id)
        {
            try
            {
                using (var db = new LiteDatabase(DbFilename))
                {
                    return db.GetCollection<Jobs>().Delete(a => a.ID == id) > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to delete {id}");

                return false;
            }
        }

        public bool AddJob(Jobs item)
        {
            try
            {
                using (var db = new LiteDatabase(DbFilename))
                {
                    return db.GetCollection<Jobs>().Insert(item) != null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to add job {item.ID}");

                return false;
            }
        }

        public bool UpdateJob(Jobs item)
        {
            try
            {
                using (var db = new LiteDatabase(DbFilename))
                {
                    return db.GetCollection<Jobs>().Update(item);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to update Job ({item.ID})");

                return false;
            }
        }

        public Jobs GetJob(Guid id)
        {
            try
            {
                using (var db = new LiteDatabase(DbFilename))
                {
                    return db.GetCollection<Jobs>().FindOne(a => a.ID == id);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to get job ({id})");

                return null;
            }
        }

        public List<Jobs> GetJobs()
        {
            try
            {
                using (var db = new LiteDatabase(DbFilename))
                {
                    return db.GetCollection<Jobs>().FindAll().ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to obtain jobs");

                return null;
            }
        }

        public void AddUpdateHost(Hosts host)
        {
            try
            {
                using (var db = new LiteDatabase(DbFilename))
                {
                    host.LastConnected = DateTime.Now;

                    var dbHost = db.GetCollection<Hosts>().FindOne(a => a.Name == host.Name);

                    if (dbHost == null)
                    {
                        db.GetCollection<Hosts>().Insert(host);
                    }
                    else
                    {
                        host.ID = dbHost.ID;

                        db.GetCollection<Hosts>().Update(host);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to add host {host.ID}");
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

        public List<PendingSubmissions> GetPendingSubmissions()
        {
            using (var db = new LiteDatabase(DbFilename))
            {
                return db.GetCollection<PendingSubmissions>().FindAll().ToList();
            }
        }

        public void AddOfflineSubmission(Jobs job)
        {
            using (var db = new LiteDatabase(DbFilename))
            {
                var pendingSubmission = new PendingSubmissions
                {
                    ID = Guid.NewGuid(),
                    JobJSON = JsonConvert.SerializeObject(job)
                };

                db.GetCollection<PendingSubmissions>().Insert(pendingSubmission);
            }
        }

        public void RemoveOfflineSubmission(Guid id)
        {
            using (var db = new LiteDatabase(DbFilename))
            {
                db.GetCollection<PendingSubmissions>().Delete(a => a.ID == id);
            }
        }
    }
}