using LightTown.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LightTown.Web.Services.Users
{
    public interface IUserService
    {
        Task<User> GetUserData(int userid);
    }
}
