using System;
using System.Collections.Generic;
using _00_Shared;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace _3_Main._ReplaySystem
{
    public class Recording
    {
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        [CanBeNull] public string Id { get; set; }
        public SessionStatisticsPlain SessionStatisticsPlain = SessionStatistics.Instance!.GetPlainCopy();
        public DateTime DateTime { get; } = DateTime.UtcNow;
        public RegisteredPlayer Player { get; set; }
        public List<Snapshot> Snapshots { get; set; } = new();
        [CanBeNull] public string location { get; set; }
    }
}