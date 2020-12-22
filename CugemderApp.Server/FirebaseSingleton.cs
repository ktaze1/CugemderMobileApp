using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CugemderApp.Server
{
    public class FirebaseSingleton
    {
        public static readonly FirebaseApp _firebaseApp = FirebaseApp.DefaultInstance == null ? FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile("./StaticFiles/Contents/beeportandroid-firebase-adminsdk-6or1y-cad1225bc9.json"),
        }) : FirebaseApp.DefaultInstance;
    }
}
