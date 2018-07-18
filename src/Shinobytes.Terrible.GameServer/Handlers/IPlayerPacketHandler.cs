using System.Threading.Tasks;
using Shinobytes.Terrible.Models;
using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Handlers
{
    public interface IPlayerPacketHandler
    {
        Task HandlePlayerPacketAsync(UserSession userSession, Connection socket, Packet packet);
    }
}