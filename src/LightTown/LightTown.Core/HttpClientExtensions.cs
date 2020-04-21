using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LightTown.Core
{
    public static class HttpClientExtensions
    {

        private const string ConnectionString = "https://localhost:5001/";

        public static async Task<T> PostJsonAsync<T>(this HttpClient httpClient, string url, object data) => await httpClient.SendJsonAsync<T>(HttpMethod.Post, url, data);

        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient, string url) => await httpClient.SendJsonAsync<T>(HttpMethod.Get, url, null);

        public static async Task<T> SendJsonAsync<T>(this HttpClient httpClient, HttpMethod method, string url, object data)
        {
            var response = await httpClient.SendAsync(new HttpRequestMessage(method, ConnectionString + url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            });

            var stringContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringContent);
        }
    }
}

