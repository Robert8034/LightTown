using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core.Data;
using LightTown.Core.Domain.Tags;
using LightTown.Core.Domain.Users;
using LightTown.Server.Services.Tags;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LightTown.Server.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<UserTag> _userTagRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly ITagService _tagService;

        public UserService(UserManager<User> userManager, IRepository<UserTag> userTagRepository, IRepository<Tag> tagRepository, ITagService tagService)
        {
            _userManager = userManager;
            _userTagRepository = userTagRepository;
            _tagRepository = tagRepository;
            _tagService = tagService;
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
            if (contentLength < 1 || contentLength > 8000000)
                return false;

            if (contentType != "image/jpeg" && contentType != "image/png")
                return false;

            string extension = contentType == "image/jpeg" ? ".jpg" : 
                contentType == "image/jpeg" ? ".png" : null;

            if (extension == null)
                return false;

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
        /// Get a list of tag ids of a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<int> GetUserTagIds(int userId)
        {
            return _userTagRepository.TableNoTracking
                .Where(userTag => userTag.UserId == userId)
                .Select(userTag => userTag.TagId)
                .ToList();
        }

        /// <summary>
        /// Modify the user's tags and add the tags if they don't exist.
        /// Return the user's tags.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public List<Tag> ModifyUserTags(User user, List<Core.Models.Tags.Tag> tags)
        {
            var userTags = _userTagRepository.Table.Where(userTag => userTag.UserId == user.Id)
                .Include(userTag => userTag.Tag).ToList();

            var removedTags = userTags.Where(userTag => tags.All(tag => tag.Id != userTag.TagId)).ToList();

            var addedTags = tags.Where(tag => userTags.All(userTag => tag.Id != userTag.TagId)).ToList();

            foreach (Core.Models.Tags.Tag tag in addedTags)
            {
                if (tag.Id == 0 || _tagRepository.GetById(tag.Id) == null)
                {
                    tag.Id = 0;
                    tag.Id = _tagService.InsertTag(tag).Id;
                }
            }

            _userTagRepository.Delete(removedTags);

            var addedUserTagEntities = addedTags.Select(tag => new UserTag
            {
                UserId = user.Id,
                TagId = tag.Id
            });

            userTags.RemoveAll(userTag => removedTags.Contains(userTag));

            var addedUserTags = addedUserTagEntities.Select(userTag => _userTagRepository.Insert(userTag)).ToList();

            userTags.AddRange(addedUserTags);

            return userTags.Select(userTag => userTag.Tag).ToList();
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

            return true;
        }
    }
}
