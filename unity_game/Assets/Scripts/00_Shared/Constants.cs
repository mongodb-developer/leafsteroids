namespace _00_Shared
{
    public static class Constants
    {
        public const string Version = "v1.0-RC2";

        public const string DataApiUrlInsertOne = "https://<YOUR_ATLAS_APP_SERVICES_ENDPOINT_URL_HERE>/snapshot";
        public const string DataApiUrlGetMany = "https://<YOUR_ATLAS_APP_SERVICES_ENDPOINT_URL_HERE>/snapshots";
        public const string DataApiUrlGetOne = "https://<YOUR_ATLAS_APP_SERVICES_ENDPOINT_URL_HERE>/snapshot?id=";
        public const string GetPlayersEndpoint = "https://<YOUR_ATLAS_APP_SERVICES_ENDPOINT_URL_HERE>/players";
        public const string GetConfigEndpoint = "https://<YOUR_ATLAS_APP_SERVICES_ENDPOINT_URL_HERE>/config";

        public const string DataApiKey = "<YOUR_APP_SERVICES_API_KEY>";

        public const float RecordingSpeed = 0.1f;
    }
}