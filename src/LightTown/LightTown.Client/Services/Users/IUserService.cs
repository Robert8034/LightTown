using System.Collections.Generic;
using System.Threading.Tasks;
using LightTown.Core.Models.Users;

namespace LightTown.Client.Services.Users
{
    public interface IUserService
    {
        Task<List<User>> SearchUsers(string searchValue);
    }
}
