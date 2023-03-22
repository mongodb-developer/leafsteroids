using MongoDB.Bson;

namespace gdc_2023_demo_registration.Data;

public class Team
{
    public ObjectId Id { get; set; }
    public string? Nickname { get; set; }
    public string? TeamName { get; set; }
    public string? Email { get; set; }
}