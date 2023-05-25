namespace _00_Shared
{
    public static class Constants
    {
        public const string Version = "v1.0-RC2";

        public const string DataApiUrlInsertOne = "http://leafsteroids.us-east-1.elasticbeanstalk.com/recording";

        // public const string DataApiUrlGetMany = "http://leafsteroids.us-east-1.elasticbeanstalk.com/snapshots";
        // public const string DataApiUrlGetOne = "http://leafsteroids.us-east-1.elasticbeanstalk.com/snapshot?id=";
        public const string GetPlayersEndpoint = "http://leafsteroids.us-east-1.elasticbeanstalk.com/players";
        public const string GetEventsEndpoint = "http://leafsteroids.us-east-1.elasticbeanstalk.com/events";
        public const string GetConfigEndpoint = "http://leafsteroids.us-east-1.elasticbeanstalk.com/config";

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