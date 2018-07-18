using Shinobytes.Terrible.Models;
using Shinobytes.Terrible.Repositories;

namespace Shinobytes.Terrible.Auth
{
    public class PlayerAuthenticator : IPlayerAuthenticator
    {
        private readonly IPlayerRepository playerRepository;

        public PlayerAuthenticator(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public Player Authenticate(string username, string password)
        {
            var player = this.playerRepository.Find(username);
            if (player == null)
            {
                return null;
            }

            // todo: hash-it!
            if (player.Password == password)
            {
                return player;
            }

            return null;
        }
    }
}