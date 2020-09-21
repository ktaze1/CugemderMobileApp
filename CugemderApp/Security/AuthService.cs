using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.Security
{
    public class AuthService : IAuthService
    {
        HttpClient _httpClient = AppState._http;

        public async Task<CurrentUser> CurrentUserInfo()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<CurrentUser>("api/auth/currentuserinfo");
                Debug.WriteLine(result);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR ON API {ex.Message}");
            }
            return new CurrentUser() { IsAuthenticated = false };
        }

        public async Task Login(LoginRequest loginRequest)
        {
            try
            {
                Debug.WriteLine(loginRequest.UserName, loginRequest.Password);
                var result = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);
                Debug.WriteLine(result.Content, result.StatusCode.ToString());

                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(" DEBUG MSG : " + ex.Message);
            }
        }

        public async Task Logout()
        {
            var result = await _httpClient.PostAsync("api/auth/logout", null);
            result.EnsureSuccessStatusCode();
        }

        public async Task Register(RegisterRequest registerRequest)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/register", registerRequest);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
        }

    }
}
