using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shinobytes.Terrible.Auth;
using Shinobytes.Terrible.Engine;
using Shinobytes.Terrible.Handlers;
using Shinobytes.Terrible.Logging;
using Shinobytes.Terrible.Managers;
using Shinobytes.Terrible.Providers;
using Shinobytes.Terrible.Repositories;
using Shinobytes.Terrible.Serializers;

namespace Shinobytes.Terrible
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin()); //builder.WithOrigins("http://localhost:8080", "*"));// builder.AllowAnyOrigin());
                options.AddPolicy("AllowAllMethods", builder => builder.AllowAnyMethod());
                options.AddPolicy("AllowAllHeaders", builder => builder.AllowAnyHeader());
            });

            services.AddSingleton<IPlayerSessionProvider, UserSessionProvider>();
            services.AddSingleton<IUserSessionManager, UserSessionManager>();
            services.AddSingleton<IPlayerSessionBinder, PlayerSessionBinder>();
            services.AddSingleton<IPlayerAuthenticator, PlayerAuthenticator>();
            services.AddSingleton<IPlayerConnectionHandler, PlayerConnectionHandler>();
            services.AddSingleton<IPlayerRepository, MemoryBasedPlayerRepository>();
            services.AddSingleton<IConnectionProvider, ConnectionProvider>();
            services.AddSingleton<IPacketDataSerializer, JsonPacketDataSerializer>();
            services.AddSingleton<IPlayerPacketHandler, PlayerPacketHandler>();
            
            //services.AddSingleton<Shinobytes.Core.ILogger, Shinobytes.Core.SyntaxHighlightedConsoleLogger>();


            services.AddSingleton<ILogger, ConsoleLogger>();
            services.AddSingleton<IWorld, World>();
            services.AddSingleton<IGame, Game>();

            services.AddTransient<AuthMiddleware>();

            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(360);
                options.Cookie.HttpOnly = true;
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var game = app.ApplicationServices.GetService<IGame>();

            game.Begin(); // let the game begin!

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };

            app.UseCors(builder =>
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());



            var sessionOptions = new SessionOptions();
            app.UseSession(sessionOptions);

            app.UseMiddleware<AuthMiddleware>();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseWebSockets(webSocketOptions);
            app.UseMvc();

            app.Use(async (context, next) =>
            {
                await context.Session.LoadAsync();
                context.Session.SetInt32("active", 1);

                if (context.Request.Path == "/ws")
                {
                    var playerSessionProvider = context.RequestServices.GetService<IPlayerSessionProvider>();

                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        var playerSession = await playerSessionProvider.GetAsync(webSocket);
                        if (playerSession == null)
                        {
                            await webSocket.CloseAsync(
                                WebSocketCloseStatus.InternalServerError,
                                "Nope",
                                CancellationToken.None);
                            return;
                        }

                        await context.Session.CommitAsync();
                        await playerSession.KeepAlive();
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });
        }
    }


    public class AuthMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // block access to html pages other than index and login if user is not logged in.
            if (context.Request.Path.HasValue)
            {
                var path = context.Request.Path.Value;
                if (path.IndexOf(".html", StringComparison.OrdinalIgnoreCase) >= 0
                    && path.IndexOf("404.html", StringComparison.OrdinalIgnoreCase) == -1
                    && path.IndexOf("index.html", StringComparison.OrdinalIgnoreCase) == -1
                    && path.IndexOf("login.html", StringComparison.OrdinalIgnoreCase) == -1)
                {
                    var sessionManager = context.RequestServices.GetService<IUserSessionManager>();
                    if (!sessionManager.TryGet(context.Session.Id, out var session))
                    {
                        context.Response.StatusCode = 405;
                        await context.Response.WriteAsync("Not authorized");
                        return;
                    }
                }
            }

            await next.Invoke(context);
        }
    }
}
