using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public bool AddUpdateHost(Hosts host)
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

                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to add host {host.ID}");

                return false;
            }
        }

        public bool DeleteHost(Guid id)
        {
            try
            {
                using (var db = new LiteDatabase(DbFilename))
                {
                    return db.GetCollection<Hosts>().Delete(a => a.ID == id) > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to delete {id}");

                return false;
            }
        }

        public List<Hosts> GetHosts()
        {
            try
            {
                using (var db = new LiteDatabase(DbFilename))
                {
                    return db.GetCollection<Hosts>().FindAll().ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get hosts");

                return null;
            }
        }

        public List<PendingSubmissions> GetPendingSubmissions()
        {
            try
            {
                using (var db = new LiteDatabase(DbFilename))
                {
                    return db.GetCollection<PendingSubmissions>().FindAll().ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get Pending Submissions");

                return null;
            }
        }

        public bool AddOfflineSubmission(Jobs job)
        {
            try
            {
                using (var db = new LiteDatabase(DbFilename))
                {
                    var pendingSubmission = new PendingSubmissions
                    {
                        ID = Guid.NewGuid(),
                        JobJSON = JsonConvert.SerializeObject(job)
                    };

                    db.GetCollection<PendingSubmissions>().Insert(pendingSubmission);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to add to offline {job.ID}");

                return false;
            }
        }

        public bool RemoveOfflineSubmission(Guid id)
        {
            try
            {
                using (var db = new LiteDatabase(DbFilename))
                {
                    return db.GetCollection<PendingSubmissions>().Delete(a => a.ID == id) > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to remove offline submission {id}");

                return false;
            }
        }
    }
}