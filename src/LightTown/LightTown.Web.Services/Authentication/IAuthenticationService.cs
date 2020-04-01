using LightTown.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LightTown.Web.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<User> Login(string username, string password);
    }
}
