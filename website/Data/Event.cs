using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization.Attributes;

#pragma warning disable CS8618

namespace website.Data;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class Event
{
    [BsonId] public string Id { get; set; }
    public string? name { get; set; }
    public string? location { get; set; }
}