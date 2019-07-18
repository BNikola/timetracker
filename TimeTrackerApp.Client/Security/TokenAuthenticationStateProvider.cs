using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TimeTrackerApp.Client.Models;

namespace TimeTrackerApp.Client.Security
{
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;

        public TokenAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // ocitati token i korisnika
            var token = await GetTokenAsync();
            var user = await GetUserAsync();

            // claims identity
            var identity = string.IsNullOrEmpty(token) ? new ClaimsIdentity() : new ClaimsIdentity(GetClaimsFromUserTokenAndUser(token, user), "jwt"); // tipa JSON web token

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task SetTokenAndUserAsync(string token, UserModel user)
        {
            await _jsRuntime.InvokeAsync<object>("blazorLocalStorage.set", "authToken", token);     // invoke - metod koji treba pozvati, a onda njegovi argumenti
            await _jsRuntime.InvokeAsync<object>("blazorLocalStorage.set", "user", user);     // invoke - metod koji treba pozvati, a onda njegovi argumenti

            // obavijestiti sve koji osluskuju da li se desio authentication change - login -> log out i sli
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<UserModel> GetUserAsync()
        {
            return await _jsRuntime.InvokeAsync<UserModel>("blazorLocalStorage.get", "user");      // metod na blazore objektu
        }

        public async Task<string> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("blazorLocalStorage.get", "authToken");      // metod na blazore objektu
        }


        private IEnumerable<Claim> GetClaimsFromUserTokenAndUser(string token, UserModel user)
        {
            // izvaditi payload iz jwt token-a
            var payload = token.Split('.')[1];      // jwt.io - dobili smo bajtove

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Parse<Dictionary<string, object>>(jsonBytes);    // vraca Dictionary: string : object

            // da skontamo ime od trenutno prijavljenog korisnika - po nekom standardu
            keyValuePairs.Add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Name);

            return keyValuePairs.Select(x => new Claim(x.Key, x.Value.ToString()));             // za svaki keyValuePair kreiraj novi claim
        }

        

        

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;  // add padding
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}
