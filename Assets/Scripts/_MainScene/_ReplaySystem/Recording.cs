using System;
using System.Collections.Generic;
using System.Globalization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace _MainScene._ReplaySystem
{
    public class Recording
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
#nullable enable
        public string? Id { get; set; }
#nullable disable

        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public List<Snapshot> Snapshots { get; set; } = new();

        public override string ToString()
        {
            var print = DateTime.ToString(CultureInfo.InvariantCulture);
            foreach (var snapshot in Snapshots!) print += $"\n{snapshot}";
            return print;
        }
    }
}