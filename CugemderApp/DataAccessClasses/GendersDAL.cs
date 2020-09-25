using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.DataAccessClasses
{
    public class GendersDAL
    {
        public HttpClient _http = AppState._http;

        public async Task<List<Genders>> GetGenders()
        {
            return await _http.GetFromJsonAsync<List<Genders>>("api/Genders");
        }

        public async Task<Genders> GetGender(int id)
        {
            return await _http.GetFromJsonAsync<Genders>($"api/Genders/{id}");
        }

        public async void PutGenders(int id, Genders genders)
        {
            await _http.PutAsJsonAsync($"api/Genders/{id}", genders);
        }

        public async void PostGenders(Genders genders)
        {
            await _http.PostAsJsonAsync("api/Genders", genders);
        }

        public async void DeleteGenders(int id)
        {
            await _http.DeleteAsync($"api/Genders/{id}");
        }
    }
}
