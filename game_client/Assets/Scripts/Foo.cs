using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using _00_Shared;
using _6_Main;
using _6_Main._ReplaySystem;
using UnityEngine;

public class Foo : MonoBehaviour
{
    [SuppressMessage("ReSharper", "Unity.IncorrectMethodSignature")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private async Task Start()
    {
        var maps = await RestClient.GetMaps();
        Debug.Log(maps![0].Pallets[0].PalletType);

        SessionStatistics.Instance.BulletsFired = 0;
        SessionStatistics.Instance.DamageDone = 0;
        SessionStatistics.Instance.PelletsDestroyedSmall = 0;
        SessionStatistics.Instance.PelletsDestroyedMedium = 0;
        SessionStatistics.Instance.PelletsDestroyedLarge = 0;
        SessionStatistics.Instance.Score = 0;
        SessionStatistics.Instance.PowerUpBulletDamageCollected = 0;
        SessionStatistics.Instance.PowerUpBulletSpeedCollected = 0;
        SessionStatistics.Instance.PowerUpPlayerSpeedCollected = 0;
        var recording = new Recording
        {
            PlayerName = "Dominic",
            ConferenceId = "mdb-internal"
        };
        var postResult = await RestClient.PostRecording(recording);
        Debug.Log(postResult);
    }
}