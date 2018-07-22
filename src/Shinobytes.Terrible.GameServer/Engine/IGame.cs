using System;
using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Engine
{
    public interface IGame
    {
        void Begin();

        void PlayerConnectionEstablished(UserSession userSession);
        void PlayerConnectionClosed(UserSession userSession);
        void PlayerPing(UserSession userSession, DateTime origin, long pid);
        void PlayerMoveTo(UserSession userSession, float worldX, float worldY);
    }
}