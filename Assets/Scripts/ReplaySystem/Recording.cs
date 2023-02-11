using System;
using System.Collections.Generic;

namespace ReplaySystem
{
    public struct Recording
    {
        public DateTime DateTime;
        public List<Snapshot> Snapshots;
    }
}