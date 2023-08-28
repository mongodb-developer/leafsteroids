using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace _6_Main._ReplaySystem
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Recording
    {
        public SessionStatisticsPlain SessionStatisticsPlain = SessionStatistics.Instance!.GetPlainCopy();
        public string PlayerName { get; set; }
        public List<Snapshot> Snapshots { get; set; } = new();
        [JsonProperty("eventId")] public string ConferenceId { get; set; }
    }
}