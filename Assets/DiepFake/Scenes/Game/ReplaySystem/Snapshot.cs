namespace DiepFake.Scenes.Game.ReplaySystem
{
    public class Snapshot
    {
        public Position PacManPosition { get; set; }

        public override string ToString()
        {
            return $"\n{PacManPosition}";
        }
    }
}