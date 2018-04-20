using System;
using System.Net.NetworkInformation;
using Serilog;

namespace EmploApiSDK.Logger
{
    class SerilogLogger : ILogger
    {
        private readonly string _prefix = $"Operation ID: {Guid.NewGuid().ToString()}";

        public void WriteLine(string message, LogLevelEnum level)
        {
            message = $"{_prefix}, Message: {message}";

            switch (level)
            {
                case LogLevelEnum.Information:
                    Log.Logger.Information(message);
                    break;
                case LogLevelEnum.Warning:
                    Log.Logger.Warning(message);
                    break;
                case LogLevelEnum.Error:
                    Log.Logger.Error(message);
                    break;
                case LogLevelEnum.Fatal:
                    Log.Logger.Fatal(message);
                    break;
                default:
                    Log.Logger.Information(message);
                    break;
            }
        }

        public void WriteLine(string message)
        {
            WriteLine(message, LogLevelEnum.Information);
        }
    }
}
