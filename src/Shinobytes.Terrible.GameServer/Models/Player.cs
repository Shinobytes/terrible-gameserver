using System;
using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Models
{
    public class Player : Object
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public int Level { get; set; }
        public long Experience { get; set; }

        public DateTime PositionChanged { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime Created { get; set; }

        public UserSession Session { get; set; }
    }
}