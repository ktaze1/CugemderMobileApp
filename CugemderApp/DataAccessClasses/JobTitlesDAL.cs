using CugemderApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CugemderApp.DataAccessClasses
{
    public class JobTitlesDAL
    {
        public HttpClient _http = AppState._http;

        public async Task<List<JobTitles>> GetJobTitles()
        {
            return await _http.GetFromJsonAsync<List<JobTitles>>("api/JobTitles");
        }

        public async Task<JobTitles> GetJobTitles(int id)
        {
            return await _http.GetFromJsonAsync<JobTitles>($"api/JobTitles/{id}");
        }

        public async void PutJobTitles(int id, JobTitles jobTitles)
        {
            await _http.PutAsJsonAsync($"api/JobTitles/{id}", jobTitles);
        }

        public async void PostJobTitles(JobTitles jobTitles)
        {
            await _http.PostAsJsonAsync("api/JobTitles", jobTitles);
        }

        public async void DeleteJobTitles(int id)
        {
            await _http.DeleteAsync($"api/JobTitles/{id}");
        }
    }
}
