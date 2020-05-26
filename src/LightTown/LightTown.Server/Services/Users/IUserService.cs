using System.IO;
using System.Threading.Tasks;
using LightTown.Core.Domain.Users;

namespace LightTown.Server.Services.Users
{
    public interface IUserService
    {
        bool TryModifyUser(Core.Models.Users.User user, User oldUser, out User newUser);
        Task<bool> TryModifyUserAvatar(User user, Stream fileStream, long? contentLength, string contentType);
        bool TryGetUserAvatar(string avatarFilename, out byte[] avatarBytes);
    }
}
