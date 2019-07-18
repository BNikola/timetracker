using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TimeTrackerApp.Client.Security;

namespace TimeTrackerApp.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenAuthenticationStateProvider _authStateProvider;

        public ApiService(HttpClient httpClient, TokenAuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
        }

        // get koji enkapsulira dodjeljivanje tokena
        public async Task<T> GetTAsync<T>(string url, string token = null)
        {
            // uvijek imamo token, ili se proslijedi ili uzmemo od trenutnog korisnika
            if (string.IsNullOrWhiteSpace(token))
            {
                token = await _authStateProvider.GetTokenAsync();
            }

            // http message
            var request = new HttpRequestMessage(HttpMethod.Get, $"{Config.ApiResourceUrl}{url}");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);    // slanje na server

            var responseContent = await response.Content.ReadAsByteArrayAsync();        // moze i sa ReadAsStringAsync

            return JsonSerializer.Parse<T>(
                responseContent,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
        }
    }
}
