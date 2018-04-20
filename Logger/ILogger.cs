namespace EmploApiSDK.Logger
{
    public interface ILogger
    {
        void WriteLine(string line);
        void WriteLine(string message, LogLevelEnum level);
    }
}
