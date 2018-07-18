using Shinobytes.Terrible.Models;

namespace Shinobytes.Terrible.Repositories
{
    public interface IPlayerRepository
    {
        Player Find(string username);
    }
}