namespace _00_Shared
{
    public static class Constants
    {
        public const string Version = "v1.0-RC2";

        private const string BaseUrl = "http://54.88.138.219:8000";

        public static readonly string DataApiUrlInsertOne = $"{BaseUrl}/recording";

        // public const string DataApiUrlGetMany = $"{BaseUrl}/snapshots";
        // public const string DataApiUrlGetOne = $"{BaseUrl}/snapshot?id=";
        public static readonly string GetPlayersEndpoint = $"{BaseUrl}/players";
        public static readonly string GetEventsEndpoint = $"{BaseUrl}/events";
        public static readonly string GetConfigEndpoint = $"{BaseUrl}/config";

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