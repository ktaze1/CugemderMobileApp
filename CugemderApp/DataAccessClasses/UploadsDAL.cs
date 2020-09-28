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
    public class UploadsDAL
    {
        public HttpClient _http = AppState._http;
        public async Task<List<Uploads>> GetUploads()
        {
            return await _http.GetFromJsonAsync<List<Uploads>>("api/Uploads");
        }

        public async Task<List<Uploads>> GetFileNames(string mail)
        {
            return await _http.GetFromJsonAsync<List<Uploads>>($"api/Uploads/user/{mail}");
        }

        public async Task<Uploads> GetUpload(int id)
        {
            return await _http.GetFromJsonAsync<Uploads>($"api/Uploads/{id}");
        }

        public async void PutUploads(int id, Uploads uploads)
        {
            await _http.PutAsJsonAsync($"api/Uploads/{id}", uploads);
        }

        public async void PostUploads(Uploads uploads)
        {
            await _http.PostAsJsonAsync("api/Uploads", uploads);
        }

        public async void DeleteUploads(string mail)
        {
            await _http.DeleteAsync($"api/Uploads/{mail}");
        }
    }
}
