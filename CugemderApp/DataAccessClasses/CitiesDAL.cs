using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.DataAccessClasses
{
    public class CitiesDAL
    {
        public HttpClient _http = AppState._http;

        public async Task<List<Cities>> GetCities()
        {
            return await _http.GetFromJsonAsync<List<Cities>>("api/Cities");
        }

        public async Task<Cities> GetCity(int id)
        {
            return await _http.GetFromJsonAsync<Cities>($"api/Cities/{id}");
        }

        public async void PutCities(int id, Cities cities)
        {
            await _http.PutAsJsonAsync($"api/Cities/{id}", cities);
        }

        public async void PostCities(Cities cities)
        {
            await _http.PostAsJsonAsync("api/Cities", cities);
        }

        public async void DeleteCities(int id)
        {
            await _http.DeleteAsync($"api/Cities/{id}");
        }
    }
}
