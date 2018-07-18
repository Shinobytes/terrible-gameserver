using System;
using System.Collections.Concurrent;
using System.Threading;
using Shinobytes.Terrible.Engine.Updates;
using Shinobytes.Terrible.Logging;
using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Engine
{
    public class Game : IGame
    {
        private readonly ILogger logger;

        private readonly ConcurrentQueue<GameUpdate> updateQueue
            = new ConcurrentQueue<GameUpdate>();

        private Thread gameThread;

        public Game(ILogger logger)
        {
            this.logger = logger;
        }

        public void Begin()
        {
            // start a new thread that will forever take all updates in updatequeue and process
            gameThread = new Thread(GameProcessLoop);
            gameThread.Start();
        }

        private void GameProcessLoop()
        {
            while (true)
            {
                ProcessUpdateQueue();
            }
        }

        private void ProcessUpdateQueue()
        {
            if (updateQueue.IsEmpty)
            {
                System.Threading.Thread.Sleep(10);
            }
            else
            {
                while (updateQueue.TryDequeue(out var playerUpdater))
                {
                    playerUpdater.Update();
                }
            }
        }

        public void PlayerConnectionEstablished(UserSession userSession)
        {
            if (userSession.IsAdmin)
            {
                logger.WriteDebug($"Admin '{userSession.Id}' is ready to administrate!");
            }
            else
            {
                logger.WriteDebug($"Player '{userSession.Id}' is ready for some action!");
            }

            // register player handler
            EnqueueWorldUpdate(userSession);
        }

        public void PlayerConnectionClosed(UserSession userSession)
        {
            logger.WriteDebug($"Oh shoot! Player '{userSession.Id}' disconnected from server.");
            // unregister player handler

            // todo: logout after 10 seconds of inactivity.
        }

        public void PlayerPing(UserSession userSession, DateTime timestamp, long pid)
        {
            logger.WriteDebug($"Player '{userSession.Id}' ping with id: {pid}.");
            EnqueuePlayerPing(userSession, timestamp, pid);
        }

        private void EnqueuePlayerPing(UserSession userSession, DateTime timestamp, long pid)
        {
            this.updateQueue.Enqueue(new PlayerPingUpdate(userSession, timestamp, pid));
        }

        private void EnqueuePlayerUpdate(UserSession userSession)
        {
            this.updateQueue.Enqueue(new PlayerUpdate(userSession));
        }

        private void EnqueueWorldUpdate(UserSession userSession)
        {
            this.updateQueue.Enqueue(new WorldUpdate(userSession));
        }
    }
}
