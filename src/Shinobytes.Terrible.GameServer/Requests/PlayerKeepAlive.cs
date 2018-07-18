using System;

namespace Shinobytes.Terrible.Requests
{
    public class PlayerKeepAlive
    {
        public long Id { get; set; }
        public DateTime Origin { get; set; }
        public DateTime Reply { get; set; }
    }
}