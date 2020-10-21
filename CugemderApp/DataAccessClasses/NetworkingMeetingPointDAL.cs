using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;


namespace CugemderApp.DataAccessClasses
{
    class NetworkingMeetingPointDAL
    {
        public HttpClient _http = AppState._http;

        public async Task<List<NetworkingMeetingPoints>> GetNetworkingMeetingPoints()
        {
            return await _http.GetFromJsonAsync<List<NetworkingMeetingPoints>>("api/NetworkingMeetingPoints");
        }

        public async Task<NetworkingMeetingPoints> GetNetworkingMeetingPoints(int id)
        {
            return await _http.GetFromJsonAsync<NetworkingMeetingPoints>($"api/NetworkingMeetingPoints/{id}");
        }

        public async void PutNetworkingMeetingPoints(int id, NetworkingMeetingPoints points)
        {
            await _http.PutAsJsonAsync($"api/NetworkingMeetingPoints/{id}", points);
        }

        public async void PostNetworkingMeetingPoints(NetworkingMeetingPoints points)
        {
            await _http.PostAsJsonAsync("api/NetworkingMeetingPoints", points);
        }

        public async void DeleteNetworkingMeetingPoints(int id)
        {
            await _http.DeleteAsync($"api/NetworkingMeetingPoints/{id}");
        }

    }
}
