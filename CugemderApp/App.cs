using System;
using CugemderApp.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.MobileBlazorBindings;
using Microsoft.MobileBlazorBindings.WebView;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CugemderApp
{
    public class App : Application
    {
        public App(IFileProvider fileProvider = null)
        {

            var hostBuilder = MobileBlazorBindingsHost.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // Adds web-specific services such as NavigationManager
                    services.AddBlazorHybrid();
                    // Register app-specific services
                    services.AddSingleton<CounterState>();
                    services.AddSingleton<AppState>();
                    services.AddOptions();
                    services.AddAuthorizationCore();
                    services.AddScoped<CustomStateProvider>();
                    services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>());
                    services.AddScoped<IAuthService, AuthService>();


                    services.AddSingleton<DataAccessClasses.AspNetUsersDAL>();
                    services.AddSingleton<DataAccessClasses.CitiesDAL>();
                    services.AddSingleton<DataAccessClasses.DocumentsDAL>();
                    services.AddSingleton<DataAccessClasses.EventsDAL>();
                    services.AddSingleton<DataAccessClasses.FileUpload>();
                    services.AddSingleton<DataAccessClasses.GendersDAL>();
                    services.AddSingleton<DataAccessClasses.GroupsDAL>();
                    services.AddSingleton<DataAccessClasses.JobTitlesDAL>();
                    services.AddSingleton<DataAccessClasses.MeetingsDAL>();
                    services.AddSingleton<DataAccessClasses.MeetingPointsDAL>();
                    services.AddSingleton<DataAccessClasses.NetworkingMeetingPointDAL>();
                    services.AddSingleton<DataAccessClasses.NetworkingActivityPointDAL>();
                    services.AddSingleton<DataAccessClasses.NotificationsDAL>();
                    services.AddSingleton<DataAccessClasses.PointsDAL>();
                    services.AddSingleton<DataAccessClasses.PositionsDAL>();
                    services.AddSingleton<DataAccessClasses.RelationshipsDAL>();
                })
                 .UseWebRoot("wwwroot");

            if (fileProvider != null)
            {
                hostBuilder.UseStaticFiles(fileProvider);
            }
            else
            {
                hostBuilder.UseStaticFiles();
            }
            var host = hostBuilder.Build();

            MainPage = new ContentPage { Title = "My Application" };
            host.AddComponent<Main>(parent: MainPage);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
