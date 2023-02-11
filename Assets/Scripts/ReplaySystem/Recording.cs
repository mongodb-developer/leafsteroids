using System;

namespace ReplaySystem
{
    [Serializable]
    public struct Recording
    {
        public DateTime Date;
        public Snapshot[] snapshots;

        public Recording(Snapshot[] snapshots)
        {
            Date = DateTime.Now;
            this.snapshots = snapshots;
        }

        public override string ToString()
        {
            var stringValue = $"\n{Date}";
            foreach (var snapshot in snapshots!)
            {
                stringValue += $"\n{snapshot}";
            }
            return stringValue;
        }
    }
}