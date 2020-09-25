using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.DataAccessClasses
{
    public class PositionsDAL
    {

        public HttpClient _http = AppState._http;

        public async Task<List<Positions>> GetPositions()
        {
            return await _http.GetFromJsonAsync<List<Positions>>("api/Positions");
        }

        public async Task<Positions> GetPosition(int id)
        {
            return await _http.GetFromJsonAsync<Positions>($"api/Positions/{id}");
        }

        public async void PutPositions(int id, Positions positions)
        {
            await _http.PutAsJsonAsync($"api/Positions/{id}", positions);
        }

        public async void PostPositions(Positions positions)
        {
            await _http.PostAsJsonAsync("api/Positions", positions);
        }

        public async void DeletePositions(int id)
        {
            await _http.DeleteAsync($"api/Positions/{id}");
        }
    }
}
