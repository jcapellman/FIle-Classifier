# File Classifier
At a high level File Classifier is a dual model file classifier of both the file type and if it is benign or malicious.  For feature extraction, model training, model running and a simple web frontend I am using C#.  For the Machine Learning elements I'm using Microsoft's ML.NET.

Breaking down the project a bit further:

## Machine Learning
* Trainer Application
* Library

## Job Manager
* Job Uploader
* REST Service
* Job Worker

## Frontend
* ASP.NET Core Frontend
