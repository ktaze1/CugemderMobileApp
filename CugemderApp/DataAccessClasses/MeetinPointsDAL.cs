using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.DataAccessClasses
{
    class MeetingPointsDAL
    {
        public HttpClient _http = AppState._http;

        public async Task<List<MeetingPoints>> GetMeetingPoints()
        {
            return await _http.GetFromJsonAsync<List<MeetingPoints>>("api/MeetingPoints");
        }

        public async Task<Groups> GetMeetingPoint(int id)
        {
            return await _http.GetFromJsonAsync<Groups>($"api/MeetingPoints/{id}");
        }

        public async void PutMeetingPoints(int id, Groups groups)
        {
            await _http.PutAsJsonAsync($"api/MeetingPoints/{id}", groups);
        }

        public async void PostMeetinPoints(MeetingPoints meetingPoints)
        {
            await _http.PostAsJsonAsync("api/MeetingPoints", meetingPoints);
        }

        public async void DeleteMeetinPoints(int id)
        {
            await _http.DeleteAsync($"api/MeetingPoints/{id}");
        }
    }
}
