namespace ReplaySystem
{
    public class Snapshot
    {
        public Position ChuckPosition { get; set; }
        public Position DominicPosition { get; set; }
        public Position HubertPosition { get; set; }
        public Position NicPosition { get; set; }
        public Position PacManPosition { get; set; }

        public override string ToString()
        {
            return $"\n{PacManPosition}\n{ChuckPosition}\n{NicPosition}\n{HubertPosition}\n{DominicPosition}";
        }
    }
}