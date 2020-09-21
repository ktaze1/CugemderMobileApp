﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp
{
    class AppState
    {
        public static HttpClient _http = new HttpClient() { BaseAddress = new Uri("http://192.168.1.22:3000/") };


        public event Func<Task> OnChange;
        private async Task NotifyStateChanged() => await OnChange?.Invoke();

    }
}
