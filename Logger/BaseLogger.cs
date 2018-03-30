using System;

namespace EmploApiSDK.Logger
{
    abstract class BaseLogger : ILogger
    {
        public abstract void WriteLine(string line);

        protected string PrependTimeToOutputLine(string line)
        {
            return String.Format("{0}: {1}", DateTime.Now, line);
        }
    }
}
