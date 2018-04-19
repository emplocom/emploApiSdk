using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace EmploApiSDK.Logger
{
    class SerilogRollingFileLogger : ILogger
    {
        public void WriteLine(string message, LogLevelEnum level)
        {
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
