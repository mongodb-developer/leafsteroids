using JetBrains.Annotations;

namespace _00_Shared
{
    public class RegisteredPlayer
    {
        [CanBeNull] public string Email;
        public string Id;
        [CanBeNull] public string location;
        public string Nickname;
        [CanBeNull] public string TeamName;
    }
}