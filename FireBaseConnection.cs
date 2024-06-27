using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;

namespace Proyecto2Api
{
    public class FireBaseConnection
    {
        private readonly string _projectId;
        private readonly FirestoreDb _firestoreDb;

        public FireBaseConnection()
        {
            _projectId = "proyect2database";
            _firestoreDb = InitializeFirestoreDb();
        }

        private FirestoreDb InitializeFirestoreDb()
        {
            string base64Credentials = Environment.GetEnvironmentVariable("credentials");

            byte[] credentialBytes = Convert.FromBase64String(base64Credentials);
            string jsonCredentials = System.Text.Encoding.UTF8.GetString(credentialBytes);

            GoogleCredential googleCredential = GoogleCredential.FromJson(jsonCredentials);

            var firestoreClientBuilder = new FirestoreClientBuilder
            {
                ChannelCredentials = googleCredential.ToChannelCredentials()
            };

            return FirestoreDb.Create(_projectId, firestoreClientBuilder.Build());
        }

        public FirestoreDb GetFirestoreDb()
        {
            return _firestoreDb;
        }
    }
}
