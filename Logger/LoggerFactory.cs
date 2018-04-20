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

            return Instance = new SerilogLogger();
        }
    }
}
