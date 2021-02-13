using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.DataAccessClasses
{
    public class FileUpload
    {
        public HttpClient _http = AppState._http;

        public async Task UploadDocument(MultipartFormDataContent content)
        {
            await _http.PostAsync("api/FileUpload",content);
        }

        public async Task UploadPhoto(MultipartFormDataContent content)
        {
            await _http.PostAsync("api/FileUpload/Photo", content);
        }

        public async Task DeletePhoto(string photoName)
        {
            await _http.DeleteAsync($"api/FileUpload/Photo/{photoName}");
        }

        public async Task DeleteDocument(string documentName)
        {
            await _http.DeleteAsync($"api/FileUpload/Document/{documentName}");
        }

        public async Task SendExcel()
        {
            await _http.GetAsync($"api/FileUpload/Document/Excel");
        }

    }
}
