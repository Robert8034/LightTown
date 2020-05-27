using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LightTown.Core.Domain.Users;
using LightTown.Core.Models.Tags;

namespace LightTown.Server.Services.Users
{
    public interface IUserService
    {
        bool TryModifyUser(Core.Models.Users.User user, User oldUser, out User newUser);
        Task<bool> TryModifyUserAvatar(User user, Stream fileStream, long? contentLength, string contentType);
        bool TryGetUserAvatar(string avatarFilename, out byte[] avatarBytes);
        List<int> GetUserTagIds(int userId);
        bool TryModifyUserTags(User user, List<Tag> tags, out List<Core.Domain.Tags.Tag> newTags);
    }
}
