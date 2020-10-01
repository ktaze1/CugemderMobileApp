using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;


namespace CugemderApp.DataAccessClasses
{
    public class DocumentsDAL
    {
        public HttpClient _http = AppState._http;

        public async Task<List<Documents>> GetDocuments()
        {
            return await _http.GetFromJsonAsync<List<Documents>>("api/Documents");
        }

        public async Task<Documents> GetDocument(int id)
        {
            return await _http.GetFromJsonAsync<Documents>($"api/Documents/{id}");
        }

        public async void PostDocuments(Documents documents)
        {
            await _http.PostAsJsonAsync("api/Documents", documents);
        }

        public async void DeleteDocuments(int id)
        {
            await _http.DeleteAsync($"api/Documents/{id}");
        }

    }
}
