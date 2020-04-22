using System.Threading.Tasks;

namespace LightTown.Client.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> Login(string username, string password, bool rememberMe);
    }
}
