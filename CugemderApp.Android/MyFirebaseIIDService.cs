﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Iid;
using Firebase.Messaging;
using System;
using System.Linq;
using System.Net.Http;
using CugemderApp.Shared.Models;

namespace CugemderApp.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseIIDService";
        HttpClient http = AppState._http;
        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);
            SendRegistrationToServer(p0);
        }
        //var refreshedToken = FirebaseInstanceId.Instance.Token;
        //SendRegistrationToServer(refreshedToken);
        void SendRegistrationToServer(string token)
        {
        }
    }
}