using System;
using System.IO;

namespace LightTown.Server
{
    public static class Config
    {
        public static string UserAvatarPath = Path.Combine(AppContext.BaseDirectory, "avatars", "users");
        public static string ProjectImagePath = Path.Combine(AppContext.BaseDirectory, "images", "projects");
    }
}
