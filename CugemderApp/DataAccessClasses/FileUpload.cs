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

        public async void PostPhoto(MultipartFormDataContent content)
        {
            await _http.PostAsync("api/FileUpload",content);
        }

    }
}
