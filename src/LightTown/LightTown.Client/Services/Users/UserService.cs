using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LightTown.Client.Services.Popups;
using LightTown.Core;
using LightTown.Core.Models.Users;

namespace LightTown.Client.Services.Users
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IPopupService<BlazorPopupService.Popup> _alertService;

        public UserService(HttpClient httpClient, IPopupService<BlazorPopupService.Popup> alertService)
        {
            _httpClient = httpClient;
            _alertService = alertService;
        }

        public async Task<List<User>> SearchUsers(string searchValue)
        {
            ApiResult result = await _httpClient.GetJsonAsync<ApiResult>("api/users/search/" + searchValue);

            return result.GetData<List<User>>();
        }
    }
}
