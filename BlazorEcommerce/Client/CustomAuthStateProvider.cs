using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorEcommerce.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _http;

        public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient http)
        {
            _localStorageService = localStorageService;
            _http = http;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //Gets authToken From the localstorage
            string authToken = await _localStorageService.GetItemAsStringAsync("authToken");

            //Creates a new claims identity & sets the authorization header of our request to null (user is not authorized), if there
            //is no authToken from the localstorage
            var identity = new ClaimsIdentity();
            _http.DefaultRequestHeaders.Authorization = null;

            //If there is an authToken, it parse and gets the claims, sets default authorization header to a bearer string
            if (!string.IsNullOrEmpty(authToken))
            {
                try
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
                    _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", ""));
                }
                //removes authToken from local storage and sets an unauthorized user
                catch
                {
                    await _localStorageService.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                }
            }
            //Creates a new user with an empty identity and sets the state to the user
            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            //Notifies that the user is not authorized
            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;

            }
            return Convert.FromBase64String(base64);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var Payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(Payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
            return claims;
        }
    }
}
