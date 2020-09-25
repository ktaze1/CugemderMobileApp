using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.DataAccessClasses
{
    public class MeetingsDAL
    {
        public HttpClient _http = AppState._http;

        public async Task<List<Meetings>> GetMeetings()
        {
            return await _http.GetFromJsonAsync<List<Meetings>>("api/Meetings");
        }

        public async Task<Meetings> GetMeeting(int id)
        {
            return await _http.GetFromJsonAsync<Meetings>($"api/Meetings/{id}");
        }

        public async Task<List<Meetings>> GetMeetingNeedPoint(string id)
        {
            return await _http.GetFromJsonAsync<List<Meetings>>($"api/Meetings/user/points/{id}");
        }

        public async Task<List<Meetings>> GetMeetingOfUser(string id)
        {
            return await _http.GetFromJsonAsync<List<Meetings>>($"api/Meetings/user/{id}");
        }

        public async void PutMeetings(int id, Meetings meeting)
        {
            await _http.PutAsJsonAsync($"api/Meetings/{id}", meeting);
        }

        public async void PostMeetings(Meetings meeting)
        {
            await _http.PostAsJsonAsync("api/Meetings", meeting);
        }

        public async void DeleteMeetings(int id)
        {
            await _http.DeleteAsync($"api/Meetings/{id}");
        }
    }
}
