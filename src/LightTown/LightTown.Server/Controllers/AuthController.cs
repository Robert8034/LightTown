using LightTown.Server.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using LightTown.Core;
using LightTown.Core.Data;
using LightTown.Core.Domain.Users;

namespace LightTown.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;

        public AuthController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        /// <summary>
        /// Login as a user.
        /// </summary>
        /// <param name="user"></param>
        /// <response code="204">Login successful.</response>
        /// <response code="400">Invalid request data given.</response>
        /// <response code="403">Invalid credentials.</response>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] UserPost user)
        {
            if (!ModelState.IsValid)
                return ApiResult.BadRequest(ModelState.First(e => e.Value.Errors.Any()).Value.Errors.First().ErrorMessage);

            var result = await _signInManager.PasswordSignInAsync(user.Username, user.Password, user.RememberMe, false);

            if (result.Succeeded)
            {
                return ApiResult.NoContent();
            }

            return ApiResult.Forbidden(result.ToString());
        }
    }
}
