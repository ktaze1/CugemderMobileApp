﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp
{
    public class AppState
    {
        public static HttpClient _http = new HttpClient() { BaseAddress = new Uri("http://192.168.1.20:3000") };

        public static CultureInfo trTR = new CultureInfo("tr-TR");

    }
}
