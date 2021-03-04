using CugemderApp.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CugemderApp.Security
{
    public class CustomStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthService api;
        private CurrentUser _currentUser;
        HttpClient http = AppState._http;
        public CustomStateProvider(IAuthService api)
        {
            this.api = api;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetCurrentUser();
                if (userInfo.IsAuthenticated)
                {
                    var claims = new[] {
                        new Claim(ClaimTypes.Name, _currentUser.UserName)
                    }.Concat(_currentUser.Claims.Select(c => new Claim(c.Key, c.Value)));

                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private async Task<CurrentUser> GetCurrentUser()
        {
            if (_currentUser != null && _currentUser.IsAuthenticated) return _currentUser;
            _currentUser = await api.CurrentUserInfo();
            return _currentUser;
        }
        public async Task Logout()
        {
            await api.Logout();
            _currentUser = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public async Task<int> Login(LoginRequest loginParameters)
        {
            var result = await api.Login(loginParameters);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                return 0;
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return 1;
        }

        public async Task ResetPassword(PasswordChangeModel model)
        {
            await api.ResetPassword(model);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task SendConfirmationEmail(string id)
        {
            await api.SendConfirmationEmail(id);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task ForgotPassword(ForgotPasswordModel model)
        {
            await api.ForgotPassword(model);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task SendNewUserEmail(string user)
        {
            await api.SendNewUserEmail(user);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Register(RegisterRequest registerParameters)
        {
            await api.Register(registerParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
