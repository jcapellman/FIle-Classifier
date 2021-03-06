﻿using FileClassifier.lib.Enums;
using FileClassifier.lib.Options.Base;

namespace FileClassifier.lib.Options
{
    public class TrainerCommandLineOptions : BaseCommandLineOptions
    {
        public string FolderOfData { get; set; }

        public ModelType ModelType { get; set; }

        public override string ToString() => $"Folder Containing Data: {FolderOfData} | Log Level: {LogLevel}";
    }
}