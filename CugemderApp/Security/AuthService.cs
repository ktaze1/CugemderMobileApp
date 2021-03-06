﻿using CugemderApp.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Net.Http.Json;
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
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR ON API {ex.Message}");
            }
            return new CurrentUser() { IsAuthenticated = false };
        }

        public async Task<HttpResponseMessage> Login(LoginRequest loginRequest)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/Login", loginRequest);
            if (result.IsSuccessStatusCode)
            {
                return result;
            }
            return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest };
        }

        public async Task Logout()
        {
            var result = await _httpClient.PostAsync("api/auth/logout", null);
            result.EnsureSuccessStatusCode();
        }

        public async Task ResetPassword(PasswordChangeModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/Changepassword", model);
            result.EnsureSuccessStatusCode();
        }

        public async Task Register(RegisterRequest registerRequest)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/register", registerRequest);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
        }

        public async Task SendConfirmationEmail(string id)
        {
            var result = await _httpClient.GetAsync($"api/auth/SendConfirmationEmail?id={id}");
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
        }

        public async Task SendNewUserEmail(string user)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/auth/SendNewUserEmail",user);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
        }

        public async Task ForgotPassword(ForgotPasswordModel model)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/SendEmailForConfirmation", model);

            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
        }
    }
}
