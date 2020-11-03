
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Util;
using System;
using Microsoft.MobileBlazorBindings.WebView.Android;
using Android.Runtime;
using CugemderApp.WebUI.Pages;
//using Plugin.PushNotification;
using System.Net.Http;
using System.Text;
using System.Globalization;
using System.Linq;

namespace CugemderApp.Droid
{
    [Activity(Label = "CugemderApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        HttpClient http = AppState._http;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            BlazorHybridAndroid.Init();

            var fileProvider = new AssetFileProvider(Assets, "wwwroot");

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App(fileProvider));


            WebUI.Pages.Index.topicSubscribed += OnSubscribeTopic;
            WebUI.Pages.Index.topicUnsubscribed += OnUnsubscribeTopic;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //groupname 
        public void OnSubscribeTopic(string groupname, string username)
        {
            var normalizedGroupName = String.Join("", groupname.Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)).Replace("ı", "i");
            Firebase.Messaging.FirebaseMessaging.Instance.SubscribeToTopic(normalizedGroupName);
            if(username != "")
                Firebase.Messaging.FirebaseMessaging.Instance.SubscribeToTopic(username);

        }

        public void OnUnsubscribeTopic(string groupname)
        {

            var normalizedGroupName = String.Join("", groupname.Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)).Replace("ı", "i");
            Firebase.Messaging.FirebaseMessaging.Instance.SubscribeToTopic(normalizedGroupName);
        }

        public static string RemoveDiacritics(string text)
        {
            Encoding srcEncoding = Encoding.UTF8;
            Encoding destEncoding = Encoding.GetEncoding(1252); // Latin alphabet

            text = destEncoding.GetString(Encoding.Convert(srcEncoding, destEncoding, srcEncoding.GetBytes(text)));

            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                if (!CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]).Equals(UnicodeCategory.NonSpacingMark))
                {
                    result.Append(normalizedString[i]);
                }
            }

            return result.ToString();
        }

        //protected override void OnNewIntent(Intent intent)
        //{
        //    if (intent.Extras != null)
        //    {
        //        var message = intent.GetStringExtra("message");
        //        (App.Current.MainPage as CugemderApp.Main)?.AddMessage(message);
        //    }

        //    base.OnNewIntent(intent);
        //}

        //bool IsPlayServiceAvailable()
        //{
        //    int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
        //    if (resultCode != ConnectionResult.Success)
        //    {
        //        if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
        //            Log.Debug(AppConstants.DebugTag, GoogleApiAvailability.Instance.GetErrorString(resultCode));
        //        else
        //        {
        //            Log.Debug(AppConstants.DebugTag, "This device is not supported");
        //        }
        //        return false;
        //    }
        //    return true;
        //}

        //void CreateNotificationChannel()
        //{
        //    // Notification channels are new as of "Oreo".
        //    // There is no need to create a notification channel on older versions of Android.
        //    if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        //    {
        //        var channelName = AppConstants.NotificationChannelName;
        //        var channelDescription = String.Empty;
        //        var channel = new NotificationChannel(channelName, channelName, NotificationImportance.Default)
        //        {
        //            Description = channelDescription
        //        };

        //        var notificationManager = (NotificationManager)GetSystemService(NotificationService);
        //        notificationManager.CreateNotificationChannel(channel);
        //    }
        //}
    }
}
