using System;
using System.Linq;
using LightTown.Core.Data;
using LightTown.Core.Domain.Users;
using LightTown.Server.Services.Mail;

namespace LightTown.Server.Services.Users
{
    public class UserInviteService : IUserInviteService
    {
        private readonly IRepository<UserInvite> _inviteRepository;
        private readonly IMailService _emailService;

        public UserInviteService(IRepository<UserInvite> inviteRepository, IMailService emailService)
        {
            _inviteRepository = inviteRepository;
            _emailService = emailService;
        }

        /// <summary>
        /// Create an invite for the username and send the link to the email, assuming a user with the username does not already exist.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="roleId"></param>
        /// <returns>The invite code.</returns>
        public string CreateInvite(string username, string email, int roleId)
        {
            string inviteCode = Guid.NewGuid().ToString("N");

            var invite = _inviteRepository.Insert(new UserInvite
            {
                Username = username,
                InviteCode = inviteCode,
                Email = email,
                RoleId = roleId,
                CreationTime = DateTime.Now
            });

            SendInvite(email, invite.InviteCode);

            return invite.InviteCode;
        }

        /// <summary>
        /// Checks whether the invite code is valid or not based on the code and the creation time, if the invite is older than 1 day it will be removed (and return false).
        /// </summary>
        /// <param name="inviteCode"></param>
        /// <param name="invite"></param>
        /// <returns>True if the invite is valid, false otherwise.</returns>
        public bool IsValidInviteCode(string inviteCode, out UserInvite invite)
        {
            invite = _inviteRepository.TableNoTracking.FirstOrDefault(invite => invite.InviteCode == inviteCode);

            if (invite == null)
                return false;

            //invites are only valid for a day
            if (invite.CreationTime.AddDays(1) < DateTime.Now)
            {
                _inviteRepository.Delete(invite);

                invite = null;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Remove the invite based on the id.
        /// </summary>
        /// <param name="inviteId"></param>
        /// <returns>True if the invite is removed, false otherwise.</returns>
        public bool RemoveInvite(int inviteId)
        {
            UserInvite invite = _inviteRepository.GetById(inviteId);

            if (invite == null)
                return false;

            _inviteRepository.Delete(invite);

            return true;
        }

        /// <summary>
        /// Send an invite email to the email.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="inviteCode"></param>
        private void SendInvite(string email, string inviteCode)
        {
            string host = Config.Config.LightTownHost.EndsWith("/") ? Config.Config.LightTownHost : Config.Config.LightTownHost + "/";

            string subject = "Please confirm your LightTown account";
            string body = "Welcome to LightTown! Please confirm your account by clicking the link below, on the page you will be asked for your account password.\n\r\n\r" +
                          $"{host}invite/{inviteCode}";

            _emailService.SendEmail(email, subject, body);
        }
    }
}