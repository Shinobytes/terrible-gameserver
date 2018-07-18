using Shinobytes.Terrible.Models;

namespace Shinobytes.Terrible.Sessions
{
    public class AdminSession : UserSession
    {
        public AdminSession(string sessionId, Player player)
            : base(sessionId, player, true)
        {
        }
    }
}