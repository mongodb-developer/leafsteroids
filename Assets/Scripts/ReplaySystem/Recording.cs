using System;
using System.Collections.Generic;
using System.Globalization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReplaySystem
{
    public class Recording
    {
        [BsonId] public ObjectId Id { get; set; } = new();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public List<Snapshot> Snapshots { get; set; } = new();

        public override string ToString()
        {
            var print = DateTime.ToString(CultureInfo.InvariantCulture);
            foreach (var snapshot in Snapshots!) print += $"\n{snapshot}";
            return print;
        }
    }
}