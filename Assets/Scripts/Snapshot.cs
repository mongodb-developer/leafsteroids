namespace Recording
{
    public struct Snapshot
    {
        public Snapshot(Position pacManPosition, Position chuckPosition, Position nicPosition, Position hubertPosition,
            Position dominicPosition)
        {
            PacManPosition = pacManPosition;
            ChuckPosition = chuckPosition;
            NicPosition = nicPosition;
            HubertPosition = hubertPosition;
            DominicPosition = dominicPosition;
        }

        public Position PacManPosition { get; }
        public Position ChuckPosition { get; }
        public Position NicPosition { get; }
        public Position HubertPosition { get; }
        public Position DominicPosition { get; }
    }
}