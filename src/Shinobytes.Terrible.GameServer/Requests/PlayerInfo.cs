﻿using System.Numerics;

namespace Shinobytes.Terrible.Requests
{
    public class PlayerInfo
    {
        public PlayerInfo(string username, int level, long experience, Vector2 position, Appearance appearance)
        {
            Username = username;
            Level = level;
            Experience = experience;
            Position = position;
            Appearance = appearance;
        }

        public string Username { get; set; }
        public int Level { get; set; }
        public long Experience { get; set; }
        public Vector2 Position { get; set; }
        public Appearance Appearance { get; set; }
    }
}