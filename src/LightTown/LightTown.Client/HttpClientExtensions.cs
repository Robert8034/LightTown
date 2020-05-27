using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LightTown.Client
{
    public static class HttpClientExtensions
    {
        private const string ConnectionString = "https://localhost:5001/";

        public static async Task<T> PostJsonAsync<T>(this HttpClient httpClient, string url, object data) =>
            await httpClient.SendJsonAsync<T>(HttpMethod.Post, url, data);

        //we use "PutJson" here because blazor otherwise wants to use a microsoft package extension method that's exactly the same but uses System.Text.Json which gives us some deserialization errors.
        public static async Task<T> PutJson<T>(this HttpClient httpClient, string url, object data) =>
            await httpClient.SendJsonAsync<T>(HttpMethod.Put, url, data);

        public static async Task<T> PatchJsonAsync<T>(this HttpClient httpClient, string url, object data) =>
            await httpClient.SendJsonAsync<T>(new HttpMethod("PATCH"), url, data);

        public static async Task<T> DeleteJsonAsync<T>(this HttpClient httpClient, string url)
        {
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, ConnectionString + url));

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringContent);
        }

        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient, string url) 
        {
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, ConnectionString + url));

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringContent);
        }

        public static async Task<T> SendJsonAsync<T>(this HttpClient httpClient, HttpMethod method, string url, object data)
        {
            var response = await httpClient.SendAsync(new HttpRequestMessage(method, ConnectionString + url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            });

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringContent);
        }
    }
}

