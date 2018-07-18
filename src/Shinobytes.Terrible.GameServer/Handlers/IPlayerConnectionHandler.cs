using Shinobytes.Terrible.Models;
using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Handlers
{
    public interface IPlayerConnectionHandler
    {
        void Open(UserSession userSession, Connection socket);
    }
}