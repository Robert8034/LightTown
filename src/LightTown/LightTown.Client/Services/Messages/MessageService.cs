using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LightTown.Client.Services.Popups;
using LightTown.Core;

namespace LightTown.Client.Services.Messages
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;
        private readonly IPopupService<BlazorPopupService.Popup> _alertService;

        public MessageService(HttpClient httpClient, IPopupService<BlazorPopupService.Popup> alertService)
        {
            _httpClient = httpClient;
            _alertService = alertService;
        }

        public async Task<bool> LikeMessage(int messageId)
        {
            ApiResult result = await _httpClient.PutJson<ApiResult>("api/messages/" + messageId + "/likes", messageId);
            Console.WriteLine("Add like check?");
            var success = result.GetData<bool>();
            
            if (!success) _alertService?.ShowErrorPopup(true, null, "Something went wrong, please try again.");
            
            return success;
        }

        public async Task<bool> RemoveMessageLike(int messageId)
        {
            ApiResult result = await _httpClient.DeleteJsonAsync<ApiResult>("api/messages/" + messageId + "/likes");
            Console.WriteLine("Remove like check");
            var success = result.GetData<bool>();
            
            if (!success) _alertService?.ShowErrorPopup(true, null, "Something went wrong, please try again.");

            return success;
        }
    }
}
