using CugemderApp.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;


namespace CugemderApp.DataAccessClasses
{
    public class JobReferencesDAL
    {
        public HttpClient _http = AppState._http;
        public async Task<List<JobReferences>> GetJobReferences()
        {
            return await _http.GetFromJsonAsync<List<JobReferences>>("api/JobReferences");
        }

        public async Task<JobReferences> GetJobReference(int id)
        {
            return await _http.GetFromJsonAsync<JobReferences>($"api/JobReferences/{id}");
        }

        public async Task<List<JobReferences>> GetJobReferencesReferencer(string id)
        {
            return await _http.GetFromJsonAsync<List<JobReferences>>($"api/JobReferences/referencer/{id}");
        }

        public async Task<List<JobReferences>> GetJobReferencesReferenced(string id)
        {
            return await _http.GetFromJsonAsync<List<JobReferences>>($"api/JobReferences/referenced/{id}");
        }

        public async Task<List<JobReferences>> GetJobReferencesExpert(string id)
        {
            return await _http.GetFromJsonAsync<List<JobReferences>>($"api/JobReferences/expert/{id}");
        }

        public async void PutJobReferences(int id, JobReferences jobReference)
        {
            await _http.PutAsJsonAsync<JobReferences>($"api/JobReferences/{id}", jobReference);
        }

        public async void PostJobReferences(JobReferences jobReference)
        {
            await _http.PostAsJsonAsync("api/JobReferences", jobReference);
        }

        public async void DeleteUser(int id)
        {
            await _http.DeleteAsync($"api/JobReferences/{id}");
        }
    }
}
