using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Shinobytes.Terrible
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                //.UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://*:49672")
                //.UseUrls("http://*:8080", "http://www.ABSOLUTELY-TERRIBLE.com", "http://ABSOLUTELY-TERRIBLE.com")
                .Build();
        }
    }
}
