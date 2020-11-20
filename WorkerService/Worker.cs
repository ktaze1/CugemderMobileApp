using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using CugemderApp.Shared.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        DateTime someDate = DateTime.Now.AddMinutes(1);
        HttpClient http = new HttpClient() { BaseAddress = new Uri( "http://api.cugemder.com/") };

        NotificaitonDAL DAL = new NotificaitonDAL();

        List<Notifications> notificationList;
        

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                if (DateTime.Now.Hour == 9)
                {
                    notificationList = await DAL.GetNotifications();
                    foreach (var notificaiton in notificationList)
                    {
                        _logger.LogInformation("Kaan taze -- time is now");
                        DAL.SendNotification(notificaiton.Body, notificaiton.Title, notificaiton.Receiver);
                        DAL.DeleteNotification(notificaiton.Id);
                    }
                }
                await Task.Delay(1000 * 60 * 60 * 24, stoppingToken); // wait for 24 hours, then re-run
            }
        }
    }

    public class NotificaitonDAL
    {

        HttpClient http = new HttpClient() { BaseAddress = new Uri("http://api.cugemder.com/") };


        public async Task<List<Notifications>> GetNotifications()
        {
            try
            {
                return await http.GetFromJsonAsync<List<Notifications>>("api/Notifications");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return new List<Notifications>();
        }

        public async void SendNotification(string _body, string _title, string _topic)
        {
            NotificationObject notif = new NotificationObject { body = _body, title = _title, topic = _topic };
            await http.PostAsJsonAsync("api/Notifications/sendNotification", notif);
        }

        public async void DeleteNotification(int id)
        {
            await http.DeleteAsync($"api/Notifications/{id}");
        }
    }

}
