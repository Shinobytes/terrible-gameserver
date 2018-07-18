using System.Threading.Tasks;
using Shinobytes.Terrible.Models;
using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Auth
{
    public interface IPlayerSessionBinder
    {
        // bind session id to connection to associate a socket with a user that has logged in.

        Task<UserSession> BindAsync(Connection connection);
    }
}