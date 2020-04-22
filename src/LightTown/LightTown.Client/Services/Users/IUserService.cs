using System.Threading.Tasks;
using LightTown.Core.Domain.Users;

namespace LightTown.Client.Services.Users
{
    public interface IUserService
    {
        Task<User> GetUserData(int userid);
    }
}
