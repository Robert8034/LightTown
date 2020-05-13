﻿using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LightTown.Client.Services.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LightTown.Client.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly HttpClient _httpClient;

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Login(string username, string password, bool rememberMe)
        {
            string jsonPost = JsonConvert.SerializeObject(new
            {
                username,
                password,
                rememberMe
            });

            var response = await _httpClient.PostAsync("api/auth/login", 
                new StringContent(jsonPost, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                return false;

            JObject jsonObject = JObject.Parse(await response.Content.ReadAsStringAsync());

            if (!jsonObject["isSuccess"]?.Value<bool>() ?? false)
                return false;

            return true;
        }
    }
}
