using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LightTown.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!Config.Config.LoadConfig())
                return;

            if (!Config.Config.CheckConfig())
                return;

            CreateHostBuilder(args)
                .Initialize()
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
