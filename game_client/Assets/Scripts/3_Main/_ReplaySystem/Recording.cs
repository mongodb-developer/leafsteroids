using System.Collections.Generic;

namespace _3_Main._ReplaySystem
{
    public class Recording
    {
        public SessionStatisticsPlain SessionStatisticsPlain = SessionStatistics.Instance!.GetPlainCopy();
        public string PlayerName { get; set; }
        public List<Snapshot> Snapshots { get; set; } = new();
        public string EventId { get; set; }
    }
}