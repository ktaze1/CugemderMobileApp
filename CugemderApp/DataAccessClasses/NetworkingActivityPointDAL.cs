using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.DataAccessClasses
{
    class NetworkingActivityPointDAL
    {
        public HttpClient _http = AppState._http;


        public async Task<List<NetworkingActivityPoint>> GetNetworkingActivityPoint()
        {
            return await _http.GetFromJsonAsync<List<NetworkingActivityPoint>>("api/NetworkingActivityPoints");
        }

        public async Task<NetworkingActivityPoint> GetNetworkingActivityPoint(int id)
        {
            return await _http.GetFromJsonAsync<NetworkingActivityPoint>($"api/NetworkingActivityPoints/{id}");
        }

        public async void PutNetworkingActivityPoint(int id, NetworkingActivityPoint points)
        {
            await _http.PutAsJsonAsync($"api/NetworkingActivityPoints/{id}", points);
        }

        public async void PostNetworkingActivityPoint(NetworkingActivityPoint points)
        {
            await _http.PostAsJsonAsync("api/NetworkingActivityPoints", points);
        }

        public async void DeleteNetworkingActivityPoint(int id)
        {
            await _http.DeleteAsync($"api/NetworkingActivityPoints/{id}");
        }

    }
}
