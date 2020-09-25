using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.DataAccessClasses
{
    public class GroupsDAL
    {
        public HttpClient _http = AppState._http;

        public async Task<List<Groups>> GetGroupsAsync()
        {
            return await _http.GetFromJsonAsync<List<Groups>>("api/Groups");
        }

        public async Task<Groups> GetGroup(int id)
        {
            return await _http.GetFromJsonAsync<Groups>($"api/Groups/{id}");
        }

        public async void PutGroups(int id, Groups groups)
        {
            await _http.PutAsJsonAsync($"api/Groups/{id}", groups);
        }

        public async void PostGroups(Groups groups)
        {
            await _http.PostAsJsonAsync("api/Groups", groups);
        }

        public async void DeleteGroups(int id)
        {
            await _http.DeleteAsync($"api/Groups/{id}");
        }
    }
}
