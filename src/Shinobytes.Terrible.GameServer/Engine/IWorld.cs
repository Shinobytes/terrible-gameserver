using Shinobytes.Terrible.Models;

namespace Shinobytes.Terrible.Engine
{
    public interface IWorld
    {
        PlayerCollection Players { get; }
        NpcCollection Npcs { get; }
        ObjectCollection Objects { get; }
        ItemCollection Items { get; }
    }
}