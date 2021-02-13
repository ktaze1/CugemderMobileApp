using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.DataAccessClasses
{
    public class NotificationsDAL
    {
        public HttpClient _http = AppState._http;

        public async void SendNotification(string _body, string _title, string _topic)
        {
            NotificationObject notif = new NotificationObject { body = _body, title = _title, topic = _topic };
            await _http.PostAsJsonAsync("api/Notifications/sendNotification", notif);
        }

        public async Task AddNotificaiton(Notifications notification)
        {
            await _http.PostAsJsonAsync("api/Notifications", notification);
        }
        public async Task DeleteNotification(int id)
        {
            await _http.DeleteAsync($"api/Notifications/{id}");
        }

    }
}
