using LightTown.Core;
using LightTown.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LightTown.Web.Services.Users
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User> GetUserData(int userid)
        {
            return await _httpClient.PostJsonAsync<User>("api/auth/login", new
            {
                userid
            });
        }
    }
}
