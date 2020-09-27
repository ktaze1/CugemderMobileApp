using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.DataAccessClasses
{
    public class RelationshipsDAL
    {
        public HttpClient _http = AppState._http;

        public async Task<List<Relationship>> GetRelationships()
        {
            return await _http.GetFromJsonAsync<List<Relationship>>("api/Relationships");
        }

        public async Task<Relationship> GetRelationship(int id)
        {
            return await _http.GetFromJsonAsync<Relationship>($"api/Relationships/{id}");
        }

        public async void PutRelationship(int id, Relationship relationship)
        {
            await _http.PutAsJsonAsync($"api/Relationships/{id}", relationship);
        }

        public async void PostRelationship(Relationship relationship)
        {
            await _http.PostAsJsonAsync("api/Relationships", relationship);
        }

        public async void DeleteRelationship(int id)
        {
            await _http.DeleteAsync($"api/Relationships/{id}");
        }
    }
}
