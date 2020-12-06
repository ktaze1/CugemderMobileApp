using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.DataAccessClasses
{
    public class UserNotifListDAL
    {
        public HttpClient _http = AppState._http;

        public async Task<List<UserNotificationList>> GetUserNotifications()
        {
            return await _http.GetFromJsonAsync<List<UserNotificationList>>("api/UserNotificationLists");
        }

        public async Task<List<UserNotificationList>> GetNotifListOfUser(string groupOrId)
        {
            return await _http.GetFromJsonAsync<List<UserNotificationList>>($"api/UserNotificationLists/user/{groupOrId}");
        }

        public async Task<UserNotificationList> GetUserNotifications(int id)
        {
            return await _http.GetFromJsonAsync<UserNotificationList>($"api/UserNotificationLists/{id}");
        }

        public async void PutUserNotifications(int id, UserNotificationList UserNotificationList)
        {
            await _http.PutAsJsonAsync($"api/UserNotificationLists/{id}", UserNotificationList);
        }

        public async void PostUserNotifications(UserNotificationList UserNotificationList)
        {
            await _http.PostAsJsonAsync("api/UserNotificationLists", UserNotificationList);
        }

        public async void DeleteUserNotifications(int id)
        {
            await _http.DeleteAsync($"api/UserNotificationLists/{id}");
        }
    }
}
