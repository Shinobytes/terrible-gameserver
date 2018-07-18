﻿using System.Threading.Tasks;
using Shinobytes.Terrible.Managers;
using Shinobytes.Terrible.Models;
using Shinobytes.Terrible.Sessions;

namespace Shinobytes.Terrible.Auth
{
    public class PlayerSessionBinder : IPlayerSessionBinder
    {
        private readonly IUserSessionManager sessionManager;

        public PlayerSessionBinder(IUserSessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
        }

        public async Task<UserSession> BindAsync(Connection connection)
        {
            var token = await connection.ReceiveAsync<string>();

            if (sessionManager.TryGet(token, out var session))
            {
                session.SetConnection(connection);
                return session;
            }
            // no such session exists, user not logged in. 
            return null;
        }
    }
}