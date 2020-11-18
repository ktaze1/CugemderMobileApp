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
            Credential = GoogleCredential.FromFile("./StaticFiles/Contents/beeport-mobileandroid-firebase-adminsdk-g46jt-269c6b9f1d.json"),
        }) : FirebaseApp.DefaultInstance;
    }
}
