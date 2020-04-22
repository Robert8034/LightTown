using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LightTown.Web.Services.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LightTown.Web.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly HttpClient _httpClient;
        private readonly UserSessionService _userSessionService;

        public AuthenticationService(HttpClient httpClient, UserSessionService userSessionService)
        {
            _httpClient = httpClient;
            _userSessionService = userSessionService;
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
