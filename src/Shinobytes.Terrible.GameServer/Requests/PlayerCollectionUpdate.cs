using System.Collections.Generic;

namespace Shinobytes.Terrible.Requests
{
    public class PlayerCollectionUpdate
    {
        public PlayerCollectionUpdate(
            IReadOnlyList<PlayerInfo> added, 
            IReadOnlyList<PlayerInfo> updated,
            IReadOnlyList<string> removed)
        {
            Added = added;
            Updated = updated;
            Removed = removed;
        }

        public IReadOnlyList<PlayerInfo> Added { get; set; }
        public IReadOnlyList<PlayerInfo> Updated { get; set; }
        public IReadOnlyList<string> Removed { get; set; }
    }
}