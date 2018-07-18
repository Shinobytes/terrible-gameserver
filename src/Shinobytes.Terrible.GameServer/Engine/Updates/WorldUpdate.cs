using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Engine.Updates
{
    public class WorldUpdate : GameUpdate
    {
        public WorldUpdate(
            UserSession target)
            : base(target)
        {
        }

        public override void Update()
        {
            var playerInfo = new Requests.PlayerInfo(
                Target.Player.Username,
                Target.Player.Level,
                Target.Player.Experience,
                Target.Player.Resources);

            Target.Send(playerInfo);
        }
    }
}