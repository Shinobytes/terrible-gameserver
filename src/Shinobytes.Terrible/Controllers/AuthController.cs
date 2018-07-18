using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shinobytes.Terrible.Auth;
using Shinobytes.Terrible.Managers;

namespace Shinobytes.Terrible.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserSessionManager sessionManager;
        private readonly IPlayerAuthenticator auth;

        public AuthController(IUserSessionManager sessionManager, IPlayerAuthenticator auth)
        {
            this.sessionManager = sessionManager;
            this.auth = auth;
        }

        [HttpPost("login")]
        [EnableCors("AllowAllOrigins")]
        public string Login([FromBody] Credentials login)// string username, string password)
        {
            // return status or session id            
            var player = auth.Authenticate(login.username, login.password);
            if (player != null)
            {
                HttpContext.Session.SetInt32("active", 1);
                HttpContext.Session.CommitAsync();

                // check if a session exists with the username
                // if so, terminate it. cannot be logged in on multiple devices.

                if (sessionManager.TryGetByUsername(login.username, out var existingSession))
                {
                    sessionManager.EndSession(existingSession);
                }

                return sessionManager.BeginSession(HttpContext.Session.Id, player);
            }

            return null;
        }

        [HttpGet("session")]
        [EnableCors("AllowAllOrigins")]
        public string Session()
        {
            if (this.sessionManager.TryGet(this.HttpContext.Session.Id, out var session))
            {
                return session.Id;
            }

            return null;
        }

        [HttpGet("logout")]
        [EnableCors("AllowAllOrigins")]
        public void Logout()
        {
            if (this.sessionManager.TryGet(this.HttpContext.Session.Id, out var session))
            {
                HttpContext.Session.Remove("active");
                HttpContext.Session.CommitAsync();
                sessionManager.EndSession(session);
            }
        }
    }

    public class Credentials
    {
        public string username { get; set; }

        public string password { get; set; }
    }
}