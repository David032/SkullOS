namespace skullOS.Core.Interfaces
{
    public interface ISkullLogger
    {
        string GetLogLocation();
        void LogMessage(string message);
    }
}