using System.Net.WebSockets;
using Shinobytes.Terrible.Logging;
using Shinobytes.Terrible.Models;
using Shinobytes.Terrible.Serializers;

namespace Shinobytes.Terrible.Providers
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly ILogger logger;
        private readonly IPacketDataSerializer serializer;

        public ConnectionProvider(ILogger logger, IPacketDataSerializer serializer)
        {
            this.logger = logger;
            this.serializer = serializer;
        }

        public Connection Get(WebSocket socket)
        {
            return new Connection(logger, socket, serializer);
        }
    }
}