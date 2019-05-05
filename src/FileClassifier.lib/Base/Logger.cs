using System;

using FileClassifier.lib.Enums;
using FileClassifier.lib.Options.Base;

namespace FileClassifier.lib.Base
{
    public static class Logger<T> where T : BaseCommandLineOptions
    {
        public static void Error(Exception exception, T options) => Log($"{exception} (Options: {options})", LogLevels.ERROR, options);

        public static void Debug(string message, T options) => Log(message, LogLevels.DEBUG, options);

        private static void Log(string message, LogLevels logLevel, T options)
        {
            if (options.LogLevel < logLevel)
            {
                return;
            }

            Console.WriteLine($"{DateTime.Now}::{logLevel}:{message}");
        }
    }
}