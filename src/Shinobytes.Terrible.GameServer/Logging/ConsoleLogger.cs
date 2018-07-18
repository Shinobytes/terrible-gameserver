using System;

namespace Shinobytes.Terrible.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void WriteError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now:s}][ERR]: {error}");
            Console.ResetColor();
        }

        public void WriteDebug(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[{DateTime.Now:s}][DBG]: {message}");
            Console.ResetColor();
        }
    }
}