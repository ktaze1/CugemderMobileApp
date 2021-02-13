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
    public class PointsDAL
    {

        public HttpClient _http = AppState._http;

        public async Task<List<Points>> GetPoints()
        {
            return await _http.GetFromJsonAsync<List<Points>>("api/Points");
        }

        public async Task<Points> GetPoint(int id)
        {
            return await _http.GetFromJsonAsync<Points>($"api/Points/{id}");
        }

        public async Task PutPoints(int id, Points points)
        {
            await _http.PutAsJsonAsync($"api/Points/{id}", points);
        }

        public async Task PostPoints(Points points)
        {
            await _http.PostAsJsonAsync("api/Points", points);
        }

        public async Task PostPoints(List<Points> points)
        {
            await _http.PostAsJsonAsync("api/Points/list", points);
        }




        public async Task DeletePoints(string id)
        {
            await _http.DeleteAsync($"api/Points/{id}");
        }
    }
}
