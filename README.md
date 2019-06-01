# File Classifier
At a high level File Classifier (just a temporary name) is a dual model file classifier of both the file type and if it is benign or malicious.  For feature extraction, model training, model running and a simple web frontend I am using C#.  For the Machine Learning elements I'm using Microsoft's ML.NET.

Breaking down the project a bit further:

### Machine Learning
* Trainer Application (.NET Core 3)
* Library (.NET Core 3)

### Job Manager
* Job Uploader (.NET Core 3)
* REST Service (ASP.NET Core 3)
* Job Worker (.NET Core 3)

### End User
* Console Application (.NET Core 3)
* Web Application (ASP.NET Core 3)

## Milestones
Currently the project is broken into a couple major pieces:
* Gather samples
* Add more features to the models
* Create the Job Manager platform
* Re-train Models
* Test Models

## Deployment
An extremely early Web Application front end has been deployed to http://165.22.8.132/.  This contains very early models, an early UI and un-optimized code.
