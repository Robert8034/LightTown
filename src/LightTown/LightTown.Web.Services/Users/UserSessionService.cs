using System;
using System.Net.Http;
using System.Threading.Tasks;
using LightTown.Core.Models.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LightTown.Web.Services.Users
{
    public class UserSessionService
    {
        private readonly HttpClient _httpClient;

        public Func<Task> OnAuthorizationChange { get; set; }

        private User _currentUser;
        private string _authorizationCookie;
        private DateTime _cookieExpirationTime;

        public UserSessionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SetAuthorizationCookie(string cookie)
        {
            _authorizationCookie = cookie;
        }

        public string GetAuthorizationCookie()
        {
            return _authorizationCookie;
        }

        public void SetCurrentUser(User user)
        {
            _currentUser = user;

            OnAuthorizationChange?.Invoke();
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }

        public bool IsAuthorized()
        {
            Console.WriteLine("User is authorized: " + (_currentUser != null));

            return _currentUser != null;
            //todo use cookie expiration check
            return _cookieExpirationTime > DateTime.UtcNow;
        }

        public async Task LoadUser()
        {
            var result = await _httpClient.GetStringAsync("api/users/@me");

            JObject jsonObject = JObject.Parse(result);

            User user = jsonObject["data"].ToObject<User>();

            SetCurrentUser(user);
        }
    }
}
