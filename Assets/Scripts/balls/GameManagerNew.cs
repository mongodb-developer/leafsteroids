using UnityEngine;

public class GameManagerNew : MonoBehaviour
{
    [SerializeField] private SphereMovement playerPrefab;
    [SerializeField] private float spawnRateS = 0.3f;
    [SerializeField] private GameObject spawnLocation;

    [SerializeField] private int playersSpawnedCount = 0;
    [SerializeField] private int playersReachedGoalCountNormal = 0;
    [SerializeField] private int playersReachedGoalCountShielded = 0;
    [SerializeField] private int playersReachedGoalCountArmed = 0;
    [SerializeField] private int playersDiedCount = 0;

    [SerializeField] private Material playerPoweredUpMaterial;
    [SerializeField] private Material playerWithWeaponMaterial;

    public void PlayerReachedGoal(SphereMovement player)
    {
        if (player.hasWeapon)
        {
            playersReachedGoalCountArmed++;
        }
        else if (player.isPoweredUp)
        {
            playersReachedGoalCountShielded++;
        }
        else
        {
            playersReachedGoalCountNormal++;
        }

        Destroy(player.gameObject);
    }

    public void PlayerHitDeadZone(SphereMovement player)
    {
        if (player.isPoweredUp) return;

        playersDiedCount++;
        Destroy(player.gameObject);
    }

    public void PlayerHitPowerUpZone(SphereMovement player)
    {
        player.isPoweredUp = true;
        player.hasWeapon = false;
        player.gameObject.GetComponent<MeshRenderer>().material = playerPoweredUpMaterial;
    }

    public void PlayerHitWeaponZone(SphereMovement player)
    {
        player.hasWeapon = true;
        player.isPoweredUp = false;
        player.gameObject.GetComponent<MeshRenderer>().material = playerWithWeaponMaterial;
    }

    public void PlayerHitOtherPlayer(SphereMovement player, SphereMovement otherPlayer)
    {
        if (player.isPoweredUp) return;
        if (otherPlayer.hasWeapon)
        {
            playersDiedCount++;
            Destroy(player.gameObject);
        }
    }

    private void OnEnable()
    {
        spawnLocation.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnPlayer), 0f, spawnRateS);
    }

    private void SpawnPlayer()
    {
        var player = Instantiate(playerPrefab, spawnLocation.gameObject.transform);
        playersSpawnedCount++;
        player.gameObject.transform.parent = gameObject.transform;
        player.GameManagerNew = this;
    }
}