namespace _3_Main._ReplaySystem
{
    public class Snapshot
    {
        public Position PlayerPosition { get; set; }

        public override string ToString()
        {
            return $"\n{PlayerPosition}";
        }
    }
}