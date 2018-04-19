using System;
using System.Configuration;
using System.IO;
using Serilog;

namespace EmploApiSDK.Logger
{
    public class LoggerFactory
    {
        public static ILogger Instance { get; private set; }

        public static ILogger CreateLogger(string[] args)
        {
            var log = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();

            Log.Logger = log;

            //CreateIfMissing(path);
            return Instance = new SerilogRollingFileLogger();
        }

        private static void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }
    }
}
