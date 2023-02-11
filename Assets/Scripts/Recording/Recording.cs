using System;
using System.Collections.Generic;

namespace Recording
{
    public class Recording
    {
        public DateTime Date { get; } = DateTime.Now;
        public List<Snapshot> Snapshots { get; } = new();
    }
}