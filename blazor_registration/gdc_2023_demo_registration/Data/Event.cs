using MongoDB.Bson;

namespace gdc_2023_demo_registration.Data;

public class Event
{
    public string Id { get; set; }
    public string? name { get; set; }
    public string? location { get; set; }
}