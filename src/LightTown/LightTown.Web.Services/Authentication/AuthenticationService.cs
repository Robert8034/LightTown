using LightTown.Core;
using LightTown.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LightTown.Web.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly HttpClient _httpClient;

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<User> Login(string username, string password)
        {
            return await _httpClient.PostJsonAsync<User>("api/auth/login", new
            {
                username,
                password
            });
        }
    }
}
