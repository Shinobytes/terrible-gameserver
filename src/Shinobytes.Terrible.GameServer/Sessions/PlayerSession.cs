using Shinobytes.Terrible.Models;

namespace Shinobytes.Terrible.Sessions
{
    public class PlayerSession : UserSession
    {
        public PlayerSession(string sessionId, Player player)
            : base(sessionId, player, false)
        {
        }
    }
}