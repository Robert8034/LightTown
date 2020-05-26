using System.IO;
using System.Threading.Tasks;
using LightTown.Core.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace LightTown.Server.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Try to modify the user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldUser"></param>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public bool TryModifyUser(Core.Models.Users.User user, User oldUser, out User newUser)
        {
            newUser = null;

            if (!IsUserValid(user, oldUser))
                return false;

            oldUser.Age = user.Age;
            oldUser.About = user.About;
            oldUser.Hometown = user.Hometown;
            oldUser.Job = user.Job;
            oldUser.Email = user.Email;
            oldUser.Fullname = user.Fullname;

            _userManager.UpdateAsync(oldUser).Wait();

            newUser = _userManager.FindByIdAsync(oldUser.Id.ToString()).Result;

            return true;
        }

        public async Task<bool> TryModifyUserAvatar(User user, Stream fileStream, long? contentLength,
            string contentType)
        {
            if (contentLength > 8000000)
                return false;

            if (contentType != "image/jpeg" && contentType != "image/png")
                return false;

            string extension = contentType == "image/jpeg" ? ".jpg" : ".png";

            if (user.HasAvatar)
                File.Delete(Path.Combine(Config.UserAvatarPath, $"{user.AvatarFilename}"));

            using (var stream = File.Create(Path.Combine(Config.UserAvatarPath, $"{user.Id}{extension}")))
            {
                await fileStream.CopyToAsync(stream);
            }

            user.HasAvatar = true;
            user.AvatarFilename = $"{user.Id}{extension}";

            await _userManager.UpdateAsync(user);

            return true;
        }

        /// <summary>
        /// Try to get the user's avatar from the disk.
        /// </summary>
        /// <param name="avatarFilename"></param>
        /// <param name="avatarBytes"></param>
        /// <returns></returns>
        public bool TryGetUserAvatar(string avatarFilename, out byte[] avatarBytes)
        {
            avatarBytes = new byte[0];

            if (File.Exists(Path.Combine(Config.UserAvatarPath, avatarFilename)))
            {
                avatarBytes = File.ReadAllBytes(Path.Combine(Config.UserAvatarPath, avatarFilename));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns if the user model is considered valid, assuming the user id's are correct.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldUser"></param>
        /// <returns></returns>
        private bool IsUserValid(Core.Models.Users.User user, User oldUser)
        {
            if (user.Username != oldUser.UserName)
                return false;
            if (user.Age < 0)
                return false;

            return true;
        }
    }
}
