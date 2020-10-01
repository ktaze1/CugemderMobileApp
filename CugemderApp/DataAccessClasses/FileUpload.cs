using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace CugemderApp.DataAccessClasses
{
    public class FileUpload
    {
        public HttpClient _http = AppState._http;

        public async void UploadDocument(MultipartFormDataContent content)
        {
            await _http.PostAsync("api/FileUpload",content);
        }

        public async void UploadPhoto(MultipartFormDataContent content)
        {
            await _http.PostAsync("api/FileUpload/Photo", content);
        }

        public async void DeletePhoto(string photoName)
        {
            await _http.DeleteAsync($"api/FileUpload/Photo/{photoName}");
        }

        public async void DeleteDocument(string documentName)
        {
            await _http.DeleteAsync($"api/FileUpload/Document/{documentName}");
        }



    }
}
