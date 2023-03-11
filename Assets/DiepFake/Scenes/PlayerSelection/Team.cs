using MongoDB.Bson;

namespace DiepFake.Scenes.PlayerSelection
{
    public class Team
    {
        public ObjectId Id { get; set; }
        public string Nickname { get; set; }
        public string TeamName { get; set; }
        public string Slogan { get; set; }
    }
}