using LightTown.Core.Domain.Users;

namespace LightTown.Server.Services.Users
{
    public interface IUserInviteService
    {
        /// <summary>
        /// Create an invite for the username and send the link to the email, assuming a user with the username does not already exist.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="roleId"></param>
        /// <returns>The invite code.</returns>
        string CreateInvite(string username, string email, int roleId);

        /// <summary>
        /// Checks whether the invite code is valid or not based on the code and the creation time, if the invite is older than 1 day it will be removed (and return false).
        /// </summary>
        /// <param name="inviteCode"></param>
        /// <param name="invite"></param>
        /// <returns>True if the invite is valid, false otherwise.</returns>
        bool IsValidInviteCode(string inviteCode, out UserInvite invite);

        /// <summary>
        /// Remove the invite based on the id.
        /// </summary>
        /// <param name="inviteId"></param>
        /// <returns>True if the invite is removed, false otherwise.</returns>
        bool RemoveInvite(int inviteId);
    }
}