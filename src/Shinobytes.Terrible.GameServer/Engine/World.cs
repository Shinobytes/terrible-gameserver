using Shinobytes.Terrible.Models;

namespace Shinobytes.Terrible.Engine
{
    public class World : IWorld
    {
        public PlayerCollection Players { get; } = new PlayerCollection();
        public NpcCollection Npcs { get; } = new NpcCollection();
        public ObjectCollection Objects { get; } = new ObjectCollection();
        public ItemCollection Items { get; } = new ItemCollection();
    }
}