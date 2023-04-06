namespace _00_Shared
{
    public static class Constants
    {
        public const string Version = "v1.0-RC2";

        public const string DataApiUrlInsertOne = "https://eu-west-1.aws.data.mongodb-api.com/app/<YOUR_APP_ID>/endpoint/snapshot";
        public const string DataApiUrlGetMany = "https://eu-west-1.aws.data.mongodb-api.com/app/<YOUR_APP_ID>/endpoint/snapshots";
        public const string DataApiUrlGetOne = "https://eu-west-1.aws.data.mongodb-api.com/app/<YOUR_APP_ID>/endpoint/snapshot?id=";
        public const string GetPlayersEndpoint = "https://eu-west-1.aws.data.mongodb-api.com/app/<YOUR_APP_ID>/endpoint/players";
        public const string GetConfigEndpoint = "https://eu-west-1.aws.data.mongodb-api.com/app/<YOUR_APP_ID>/endpoint/config";

        public const string DataApiKey = "<YOUR_API_KEY>";

        public const float RecordingSpeed = 0.1f;
    }
}