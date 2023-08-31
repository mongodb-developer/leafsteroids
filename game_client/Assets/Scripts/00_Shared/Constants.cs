namespace _00_Shared
{
    public static class Constants
    {
        public static class GameServerEndpoints
        {
            public const string GetConferences = "http://{0}:{1}/events";
            public const string GetConfig = "http://{0}:{1}/configs";
            public const string GetPlayers = "http://{0}:{1}/players";
            public const string GetMaps = "http://{0}:{1}/maps";
            public const string PostRecording = "http://{0}:{1}/recordings";
            public const string GetPlayerSearch = "http://{0}:{1}/players/search";
        }

        public const float RecordingSpeed = 0.1f;

        public static class SceneNames
        {
            public const string Loading = "1_Loading";
            public const string ConferenceSelection = "2_EventSelection";
            public const string Welcome = "3_Welcome";
            public const string PlayerSelection = "4_PlayerSelection";
            public const string Instructions = "5_Instructions";
            public const string Main = "6_Main";
            public const string MainDynamic = "6_Main_dynamic";
        }
    }
}