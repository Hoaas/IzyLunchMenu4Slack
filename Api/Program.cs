using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var env = context.HostingEnvironment;

                    config
                        .AddJsonFile("appsettings.json", false)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", false)
                        .AddUserSecrets<Startup>(false)
                        .AddEnvironmentVariables();

                    var builtConfig = config.Build();
                })
                .UseStartup<Startup>();
    }
}
