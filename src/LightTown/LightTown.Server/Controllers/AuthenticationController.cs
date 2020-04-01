using LightTown.Server.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpPost]
        [Route("login")]
        public async Task<ApiResult> Login([FromBody] UserPost user)
        {
            if (!ModelState.IsValid)
                return ApiResult.BadRequest(ModelState.First(e => e.Value.Errors.Any()).Value.Errors.First().ErrorMessage);

            var result = await _signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);

            if (result.Succeeded)
            {
                return ApiResult.Success(null);
            }

            return ApiResult.BadRequest(result.ToString());
        }
    }
}
