using System.Net.WebSockets;
using System.Threading.Tasks;
using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Providers
{
    public interface IPlayerSessionProvider
    {
        Task<UserSession> GetAsync(WebSocket socket);
    }
}