using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Numerics;
using Shinobytes.Terrible.Models;
using Shinobytes.Terrible.Requests;

namespace Shinobytes.Terrible.Repositories
{
    public class MemoryBasedPlayerRepository : IPlayerRepository
    {
        private readonly ConcurrentDictionary<string, Player>
            players = new ConcurrentDictionary<string, Player>();

        public MemoryBasedPlayerRepository()
        {
            players["lichine"] = new Player
            {
                Id = 0,
                IsAdmin = true,
                Level = 1,
                Username = "Lichine",
                Password = "password",
                Created = DateTime.UtcNow
            };

            players["zerratar"] = new Player
            {
                Id = 1,
                IsAdmin = true,
                Level = 1,
                Username = "Zerratar",
                Password = "password",
                Created = DateTime.UtcNow
            };

            players["user"] = new Player
            {
                Id = 2,
                IsAdmin = false,
                Level = 1,
                Username = "User",
                Password = "password",
                Created = DateTime.UtcNow
            };
        }

        public Player Find(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            if (players.TryGetValue(username.ToLower(), out var player))
            {
                return player;
            }

            var random = new Random();

            return this.players[username.ToLower()] = new Player
            {
                Id = this.players.Max(x => x.Value.Id) + 1,
                Level = 1,
                Username = username,
                Created = DateTime.UtcNow,
                Position = new Vector2(random.Next(600), random.Next(600)),
                Appearance = Appearance.Random()
            };
        }
    }
}