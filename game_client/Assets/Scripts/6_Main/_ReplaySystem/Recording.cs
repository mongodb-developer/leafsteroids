using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace _6_Main._ReplaySystem
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Recording
    {
        public SessionStatisticsPlain SessionStatisticsPlain = SessionStatistics.Instance!.GetPlainCopy();
        public string PlayerName { get; set; }
        public List<Snapshot> Snapshots { get; set; } = new();
        public string EventId { get; set; }
    }
}

// {
//     "sessionStatisticsPlain": {
//         "bulletsFired": 0,
//         "damageDone": 0,
//         "pelletsDestroyedSmall": 0,
//         "pelletsDestroyedMedium": 0,
//         "pelletsDestroyedLarge": 0,
//         "score": 0,
//         "powerUpBulletDamageCollected": 0,
//         "powerUpBulletSpeedCollected": 0,
//         "powerUpPlayerSpeedCollected": 0
//     },
//     "playerName": "string",
//     "snapshots": [
//     {
//         "position": {
//             "x": 0,
//             "y": 0,
//             "z": 0
//         }
//     }
//     ],
//     "eventId": "string"
// }

// {
//   "SessionStatisticsPlain": {
//     "BulletsFired": 8,
//     "DamageDone": 90,
//     "PelletsDestroyedSmall": 0,
//     "PelletsDestroyedMedium": 1,
//     "PelletsDestroyedLarge": 0,
//     "Score": 75,
//     "PowerUpBulletDamageCollected": 0,
//     "PowerUpBulletSpeedCollected": 0,
//     "PowerUpPlayerSpeedCollected": 0
//   },
//   "PlayerName": "Dominic",
//   "Snapshots": [
//     {
//       "PlayerPosition": {
//         "X": 0.0,
//         "Y": -0.1,
//         "Z": 0.0
//       }
//     },
//   ]
// }