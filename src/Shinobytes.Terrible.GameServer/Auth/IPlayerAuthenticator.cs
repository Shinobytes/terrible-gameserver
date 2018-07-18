using Shinobytes.Terrible.Models;

namespace Shinobytes.Terrible.Auth
{
    public interface IPlayerAuthenticator
    {
        Player Authenticate(string username, string password);
    }
}