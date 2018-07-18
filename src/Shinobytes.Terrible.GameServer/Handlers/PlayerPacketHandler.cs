using System;
using System.Threading.Tasks;
using Shinobytes.Terrible.Engine;
using Shinobytes.Terrible.Logging;
using Shinobytes.Terrible.Models;
using Shinobytes.Terrible.Requests;
using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Handlers
{
    public class PlayerPacketHandler : IPlayerPacketHandler
    {
        private readonly IGame game;
        private readonly ILogger logger;

        public PlayerPacketHandler(IGame game, ILogger logger)
        {
            this.game = game;
            this.logger = logger;
        }

        public Task HandlePlayerPacketAsync(UserSession userSession, Connection socket, Packet packet)
        {
            try
            {
                if (packet.Is<PlayerKeepAlive>(out var ping))
                {
                    this.game.PlayerPing(userSession, ping.Origin, ping.Id);
                }
            }
            catch (Exception exc)
            {
                logger.WriteError("Unhandled packet: " + exc.Message);
            }
            return Task.CompletedTask;
        }
    }
}