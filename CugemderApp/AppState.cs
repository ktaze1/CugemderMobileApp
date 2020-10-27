using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp
{
    public class AppState
    {
        public static HttpClient _http = new HttpClient() { BaseAddress = new Uri("http://192.168.1.55:3000/") };

        public event Func<Task> OnChange;
        private async Task NotifyStateChanged() => await OnChange?.Invoke();

    }
}
