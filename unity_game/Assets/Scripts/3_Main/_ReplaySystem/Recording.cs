using System;
using System.Collections.Generic;
using _00_Shared;
using Newtonsoft.Json;

namespace _3_Main._ReplaySystem
{
    public class Recording
    {
        public SessionStatisticsPlain SessionStatisticsPlain = SessionStatistics.Instance!.GetPlainCopy();
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
#nullable enable
        public string? Id { get; set; }
#nullable disable

        public DateTime DateTime { get; } = DateTime.UtcNow;
        public RegisteredPlayer Player { get; set; }
        public List<Snapshot> Snapshots { get; set; } = new();
    }
}