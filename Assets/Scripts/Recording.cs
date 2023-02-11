using System;
using System.Collections.Generic;

public class Recording
{
    public DateTime Date { get; } = DateTime.Now;
    public List<Snapshot> Snapshots { get; } = new();
}