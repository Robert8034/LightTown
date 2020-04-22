using System;
using System.Net.Http;
using System.Threading.Tasks;
using LightTown.Core.Domain.Users;

namespace LightTown.Client.Services.Users
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [Obsolete]
        public async Task<User> GetUserData(int userid)
        {
            return await _httpClient.PostJsonAsync<User>("api/auth/login", new
            {
                userid
            });
        }
    }
}
