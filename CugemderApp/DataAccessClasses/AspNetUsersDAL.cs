using CugemderApp.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.DataAccessClasses
{
    public class AspNetUsersDAL
    {
        public HttpClient _http = AppState._http;
        public async Task<List<AspNetUsers>> GetUsers()
        {
            return await _http.GetFromJsonAsync<List<AspNetUsers>>("api/AspNetUsers");
        }

        public async Task<string> GetUserFullname(string id)
        {
            return await _http.GetFromJsonAsync<string>($"api/AspNetUsers/userFullname/{id}");
        }

        public async Task<List<AspNetUsers>> GetUsersNoNull()
        {
            return await _http.GetFromJsonAsync<List<AspNetUsers>>("api/AspNetUsers/NoNull");
        }

        public async Task<List<AspNetUsers>> GetUsersNoNullUserList()
        {
            return await _http.GetFromJsonAsync<List<AspNetUsers>>("api/AspNetUsers/NoNull/UserList");
        }

        public async Task<List<AspNetUsers>> GetUsersNoGroup()
        {
            return await _http.GetFromJsonAsync<List<AspNetUsers>>("api/AspNetUsers/noGroup");
        }

        public async Task<List<AspNetUsers>> GetUsersNoRole()
        {
            return await _http.GetFromJsonAsync<List<AspNetUsers>>("api/AspNetUsers/NoRole");
        }

        public async Task<AspNetUsers> GetUsers(string id)
        {
            return await _http.GetFromJsonAsync<AspNetUsers>($"api/AspNetUsers/{id}");
        }

        public async Task<AspNetUsers> GetUserID(string username)
        {
            return await _http.GetFromJsonAsync<AspNetUsers>($"api/AspNetUsers/getId/{username}");
        }

        public async Task<AspNetUsers> GetUsername(string email)
        {
            return await _http.GetFromJsonAsync<AspNetUsers>($"api/AspNetUsers/username/{email}");
        }

        public async Task PutUser(string id, AspNetUsers user)
        {
           await _http.PutAsJsonAsync<AspNetUsers>($"api/AspNetUsers/{id}", user);
        }

        public async void PutUserWithRole(string id, AspNetUsers user)
        {
           await _http.PutAsJsonAsync<AspNetUsers>($"api/AspNetUsers/newGroup/{id}", user);
        }


        public async void PutPhotoUrl(string id, string url)
        {
            await _http.PutAsJsonAsync($"api/AspNetUsers/photoUrl/{id}", url);
        }

        public async void PostUser(AspNetUsers user)
        {
            await _http.PostAsJsonAsync("api/AspNetUsers", user);
        }

        public async Task DeleteUser(string id)
        {
            await _http.DeleteAsync($"api/AspNetUsers/{id}");
        }

    }
}
