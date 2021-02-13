using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.DataAccessClasses
{
    public class EventsDAL
    {
        public HttpClient _http = AppState._http;

        public async Task<List<Events>> GetEvents()
        {
            return await _http.GetFromJsonAsync<List<Events>>("api/Events");
        }

        public async Task<Events> GetEvents(int id)
        {
            return await _http.GetFromJsonAsync<Events>($"api/Events/{id}");
        }

        public async Task PutEvents(int id, Events events)
        {
            await _http.PutAsJsonAsync($"api/Events/{id}", events);
        }

        public async Task PostEvents(Events events)
        {
            await _http.PostAsJsonAsync("api/Events", events);
        }

        public async Task DeleteEvents(int id)
        {
            await _http.DeleteAsync($"api/Events/{id}");
        }
    }
}
