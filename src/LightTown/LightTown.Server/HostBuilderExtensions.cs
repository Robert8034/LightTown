using System.IO;
using Microsoft.Extensions.Hosting;

namespace LightTown.Server
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder Initialize(this IHostBuilder hostBuilder)
        {
            Directory.CreateDirectory(Config.UserAvatarPath);

            return hostBuilder;
        }
    }
}