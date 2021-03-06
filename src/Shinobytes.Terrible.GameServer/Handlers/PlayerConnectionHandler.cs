﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Shinobytes.Terrible.Engine;
using Shinobytes.Terrible.Logging;
using Shinobytes.Terrible.Models;
using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Handlers
{
    public class PlayerConnectionHandler : IPlayerConnectionHandler
    {
        private readonly IGame game;
        private readonly IPlayerPacketHandler packetHandler;
        private readonly ILogger logger;

        private readonly ConcurrentDictionary<string, Thread> writeThreads
            = new ConcurrentDictionary<string, Thread>();

        private readonly ConcurrentDictionary<string, Thread> readThreads
            = new ConcurrentDictionary<string, Thread>();

        public PlayerConnectionHandler(
            IGame game,
            IPlayerPacketHandler packetHandler,
            ILogger logger)
        {
            this.game = game;
            this.packetHandler = packetHandler;
            this.logger = logger;
        }

        public void Open(UserSession userSession, Connection socket)
        {

            // start 1 read/write thread per connection, for now. and then keep each connection single threaded.

            game.PlayerConnectionEstablished(userSession);
            writeThreads[userSession.Id] = new Thread(async () =>
            {
                await PlayerConnectionWrite(userSession, socket);
            });
            writeThreads[userSession.Id].Start();

            readThreads[userSession.Id] = new Thread(async () =>
            {
                await PlayerConnectionRead(userSession, socket);
            });
            readThreads[userSession.Id].Start();
        }

        private async Task PlayerConnectionWrite(UserSession userSession, Connection socket)
        {
            do
            {
                await ProcessSendQueueAsync(socket);

            } while (!socket.Closed);
        }

        private async Task PlayerConnectionRead(UserSession userSession, Connection socket)
        {
            do
            {
                await HandlePacketsAsync(userSession, socket);
            } while (!socket.Closed);
            game.PlayerConnectionClosed(userSession);
        }

        private async Task<bool> HandlePacketsAsync(UserSession userSession, Connection socket)
        {
            try
            {
                var result = await socket.ReceiveAsync();
                if (result == null)
                {
                    return true;
                }

                await HandlePacketAsync(userSession, socket, result);
                return true;
            }
            catch (Exception exc)
            {
                logger.WriteError("Unhandled packet: " + exc.Message);
                if (socket.Closed)
                {
                    return false;
                }
            }
            return true;
        }

        private async Task ProcessSendQueueAsync(Connection socket)
        {
            while (socket.SendQueue.TryDequeue(out var packet))
            {
                await socket.SendAsync(packet);
            }
        }

        private Task HandlePacketAsync(UserSession userSession, Connection socket, Packet packet)
        {
            if (packet == null)
            {
                throw new ArgumentNullException(nameof(packet));
            }

            return packetHandler.HandlePlayerPacketAsync(userSession, socket, packet);
        }

        public void Close(UserSession userSession)
        {
            userSession.Close();

            if (writeThreads.TryGetValue(userSession.Id, out var writeThread))
            {
                writeThread.Join();
            }

            if (readThreads.TryGetValue(userSession.Id, out var readThread))
            {
                readThread.Join();
            }
        }
    }
}