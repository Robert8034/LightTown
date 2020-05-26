using System;
using System.IO;

namespace LightTown.Server
{
    public static class Config
    {
        public static string UserAvatarPath = Path.Combine(AppContext.BaseDirectory, "avatars", "users");
    }
}
