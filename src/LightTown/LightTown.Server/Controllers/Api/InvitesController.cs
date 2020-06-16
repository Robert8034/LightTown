using System.Threading.Tasks;
using AutoMapper;
using LightTown.Core;
using LightTown.Core.Domain.Roles;
using LightTown.Core.Domain.Users;
using LightTown.Server.Models.Invites;
using LightTown.Server.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LightTown.Server.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitesController : ControllerBase
    {
        private readonly IUserInviteService _userInviteService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public InvitesController(IUserInviteService userInviteService, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userInviteService = userInviteService;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        /// <summary>
        /// Creates the account from the invite.
        /// </summary>
        /// <response code="204">Account is created.</response>
        /// <response code="400">The invite is invalid or expired (1 day or older) or body data is invalid.</response>
        [HttpPost]
        [Route("{inviteCode}")]
        public async Task<ApiResult> CreateAccount(string inviteCode, [FromBody] InvitePost invitePost)
        {
            var isValid = _userInviteService.IsValidInviteCode(inviteCode, out UserInvite invite);

            if (!isValid)
                return ApiResult.BadRequest();

            var result = await _userManager.CreateAsync(new User(invite.Username)
            {
                Email = invite.Email
            }, invitePost.Password);

            if(!result.Succeeded)
                return ApiResult.BadRequest();

            var user = await _userManager.FindByNameAsync(invite.Username);
            var role = await _roleManager.FindByIdAsync(invite.RoleId.ToString());

            await _userManager.AddToRoleAsync(user, role.Name);

            _userInviteService.RemoveInvite(invite.Id);

            return ApiResult.NoContent();
        }

        /// <summary>
        /// Returns whether the invite code is valid.
        /// </summary>
        /// <response code="204">Invite is valid.</response>
        /// <response code="404">The invite is invalid or expired (1 day or older).</response>
        [HttpGet]
        [Route("{inviteCode}")]
        public ApiResult CheckInvite(string inviteCode)
        {
            var isValid = _userInviteService.IsValidInviteCode(inviteCode, out UserInvite invite);

            if (!isValid)
                return ApiResult.NotFound();

            var inviteModel = _mapper.Map<Core.Models.Users.UserInvite>(invite);

            return ApiResult.Success(inviteModel);
        }
    }
}
