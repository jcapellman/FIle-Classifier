# File Classifier
At a high level File Classifier (just a temporary name) is a dual model file classifier of both the file type and if it is benign or malicious.  For feature extraction, model training, model running, the job manager system and a simple web frontend I am using C#.  For the Machine Learning elements I'm using Microsoft's ML.NET.

Breaking down the project a bit further:

### Machine Learning
* Trainer Application (.NET Core 2.2.5) - (FileClassifier.Trainer) - Trains and builds models
* Library (.NET Core 2.2.5) - (FileClassifier.lib) - Contains all of the common code for the entire project

### Job Manager
* Job Uploader (.NET Core 2.2.5) - (FileClassifier.JobManager.Uploader) - Simple command line tool to submit new jobs
* REST Service (ASP.NET Core 2.2.5) - (FileClassifier.JobManager.REST) - REST Service and Dashboard of the Jobs
* Job Worker (.NET Core 2.2.5) - (FileClassifier.JobManager.Worker) - Self-contained console app

### End User
* Console Application (.NET Core 2.2.5)
* Web Application (ASP.NET Core 2.2.5)

### Unit Tests
* Coverage across all of the projects (.NET Core 3) - (FileClassifier.UnitTests)

## Milestones
Currently the project is broken into a couple major pieces:
* Gather samples
* Add more features to the models
* Create the Job Manager platform
* Re-train Models
* Test Models

## Deployment
Involes deployment of the Job Manager REST Service/Dashboard and then the Job Workers on the various Nodes.

Master branch deployments of the Job Manager REST Service/Dashboard are here: http://165.22.8.132/
