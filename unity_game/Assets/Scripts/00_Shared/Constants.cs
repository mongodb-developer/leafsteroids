namespace _00_Shared
{
    public static class Constants
    {
        public const string Version = "v1.0-RC2";

        public const string DataApiUrlInsertOne =
            "https://data.mongodb-api.com/app/leafsteroids-ljgok/endpoint/snapshot";

        public const string DataApiUrlGetMany =
            "https://data.mongodb-api.com/app/leafsteroids-ljgok/endpoint/snapshots";

        public const string DataApiUrlGetOne =
            "https://data.mongodb-api.com/app/leafsteroids-ljgok/endpoint/snapshot?id=";

        public const string GetPlayersEndpoint = "https://data.mongodb-api.com/app/leafsteroids-ljgok/endpoint/players";
        public const string GetEventsEndpoint = "https://data.mongodb-api.com/app/leafsteroids-ljgok/endpoint/events";
        public const string GetConfigEndpoint = "https://data.mongodb-api.com/app/leafsteroids-ljgok/endpoint/config";

        public const string DataApiKey = "QwUdPAFQHXnUtgrpNe4Ymq1uuWF93tiV3GBn3LujQxDRi9kGii0ZL8DS7Syf6duU";

        public const float RecordingSpeed = 0.1f;

        public static class SceneNames
        {
            public const string Loading = "1_Loading";
            public const string EventSelection = "2_EventSelection";
            public const string Welcome = "3_Welcome";
            public const string PlayerSelection = "4_PlayerSelection";
            public const string Instructions = "5_Instructions";
            public const string Main = "6_Main";
            public const string Playground = "xx_Playground";
        }

        public static class DotEnvFileKeys
        {
            public const string EventId = "EVENT_ID";
            public const string EventName = "EVENT_NAME";
            public const string EventLocation = "EVENT_LOCATION";
        }
    }
}