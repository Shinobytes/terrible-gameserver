namespace Shinobytes.Terrible.Logging
{
    public interface ILogger
    {
        void WriteError(string error);
        void WriteDebug(string message);
    }
}
