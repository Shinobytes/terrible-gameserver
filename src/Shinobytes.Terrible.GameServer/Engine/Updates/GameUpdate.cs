using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Engine.Updates
{
    public abstract class GameUpdate
    {
        protected GameUpdate(UserSession target)
        {
            this.Target = target;
        }

        public UserSession Target { get; }

        public abstract void Update();
    }
}