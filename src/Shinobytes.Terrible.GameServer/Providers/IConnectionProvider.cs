using System.Net.WebSockets;
using Shinobytes.Terrible.Models;

namespace Shinobytes.Terrible.Providers
{
    public interface IConnectionProvider
    {
        Connection Get(WebSocket socket);
    }
}