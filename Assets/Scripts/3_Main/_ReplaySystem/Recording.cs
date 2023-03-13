using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace _3_Main._ReplaySystem
{
    public class Recording
    {
        public SessionStatisticsPlain SessionStatisticsPlain = SessionStatistics.Instance!.GetPlainCopy();
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
#nullable enable
        public string? Id { get; set; }
#nullable disable

        public DateTime DateTime { get; } = DateTime.UtcNow;
        public List<Snapshot> Snapshots { get; set; } = new();
    }
}