using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using LightTown.Core;
using LightTown.Core.Data;
using LightTown.Core.Domain.Roles;
using LightTown.Core.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace LightTown.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UsersController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("@me")]
        [Authorization(Permissions.NONE)]
        public async Task<ApiResult> GetSelf()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var userModel = _mapper.Map<Core.Models.Users.User>(currentUser);

            return ApiResult.Success(userModel);
        }
    }
}
