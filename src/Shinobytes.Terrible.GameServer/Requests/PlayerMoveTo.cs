namespace Shinobytes.Terrible.Requests
{
    public class PlayerMoveTo
    {
        public PlayerMoveTo() { }

        public PlayerMoveTo(string username, float x, float y)
        {
            this.Username = username;
            this.X = x;
            this.Y = y;
        }

        public string Username { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}