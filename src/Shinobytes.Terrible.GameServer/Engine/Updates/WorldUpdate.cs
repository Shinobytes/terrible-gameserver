using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Shinobytes.Terrible.Requests;
using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Engine.Updates
{
    public class WorldUpdate : GameUpdate
    {
        private readonly IWorld world;

        public WorldUpdate(
            IWorld world,
            UserSession target)
            : base(target)
        {
            this.world = world;
        }

        public override void Update()
        {
            if (!Target.Connected)
            {
                // player was disconnected, so we wanna broadcast to everyone except this one, that we have been removed
                var otherPlayers = world.Players
                    .Except(x => x.Id == Target.Player.Id);

                var removedPlayer = new PlayerCollectionUpdate(
                    new List<PlayerInfo>(),
                    new List<PlayerInfo>(),
                    new[] { Target.Player.Username }
                );

                foreach (var session in otherPlayers.Select(x => x.Session))
                {
                    session.Send(removedPlayer);
                }
            }
            else
            {

                var playerInfo = new PlayerInfo(
                    Target.Player.Username,
                    Target.Player.Level,
                    Target.Player.Experience,
                    Target.Player.Position,
                    Target.Player.Appearance);

                Target.Send(playerInfo);

                var otherPlayers = world.Players
                    .Except(x => x.Id == Target.Player.Id);

                var otherPlayerInfos = otherPlayers
                        .Select(x => new PlayerInfo(
                            x.Username,
                            x.Level,
                            x.Experience,
                            x.Position,
                            x.Appearance))
                        .ToList();

                if (otherPlayerInfos.Count > 0)
                {
                    var existingPlayers = new PlayerCollectionUpdate(
                        otherPlayerInfos,
                        new List<PlayerInfo>(),
                        new List<string>()
                    );

                    Target.Send(existingPlayers);
                }

                var newPlayer = new PlayerCollectionUpdate(
                    new[] { playerInfo },
                    new List<PlayerInfo>(),
                    new List<string>()
                );

                // broadcast to all other players that this one was added.
                foreach (var session in otherPlayers.Select(x => x.Session))
                {
                    session.Send(newPlayer);
                }
            }
        }
    }
}