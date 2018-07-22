using System.Collections.Generic;
using System.Linq;
using Shinobytes.Terrible.Requests;
using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Engine.Updates
{
    public class PlayerPositionUpdate : GameUpdate
    {
        private readonly IWorld world;

        public PlayerPositionUpdate(IWorld world, UserSession target) : base(target)
        {
            this.world = world;
        }

        public override void Update()
        {
            var playerInfo = new PlayerMoveTo(
                Target.Player.Username,
                Target.Player.Position.X,
                Target.Player.Position.Y);

            var otherPlayers = world.Players
                .Except(x => x.Id == Target.Player.Id);

            // broadcast to all other players that this one was added.
            foreach (var session in otherPlayers.Select(x => x.Session))
            {
                session.Send(playerInfo);
            }
        }
    }
}