﻿using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Engine.Updates
{
    public class PlayerUpdate : GameUpdate
    {
        public PlayerUpdate(UserSession target) : base(target)
        {
        }

        public override void Update()
        {
            // Player update includes:
            //      Target.Send(new Requests.PlayerInfo(username, level, experience, resources, owned-node ids))       

            var playerInfo = new Requests.PlayerInfo(
                Target.Player.Username,
                Target.Player.Level,
                Target.Player.Experience,
                Target.Player.Position,
                Target.Player.Appearance);

            Target.Send(playerInfo);
        }
    }
}