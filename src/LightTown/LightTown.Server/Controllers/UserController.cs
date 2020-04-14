using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core.Domain.Users;
using LightTown.Server.Models.Users;
using LightTown.Server.Services.UserServices;
using Microsoft.AspNetCore.Authorization;

namespace LightTown.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("@me")]
        public async Task<ApiResult> GetUserData(int userid)
        {
            //var result = await _userService.
        }
    }
}
