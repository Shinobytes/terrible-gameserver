using System.Collections.Generic;
using Shinobytes.Terrible.Models;
using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Managers
{
    public interface IUserSessionManager
    {
        IReadOnlyList<UserSession> GetSessions();
        bool TryGet(string token, out UserSession userSession);
        bool TryGetByUsername(string username, out UserSession userSession);
        void EndSession(UserSession userSession);
        string BeginSession(string sessionId, Player player);
    }
}