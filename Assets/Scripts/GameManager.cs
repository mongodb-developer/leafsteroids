using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private GameObject validPlayingField;

    [SerializeField] private int playersReachedGoalCount;
    [SerializeField] private int playersLeftMapCount;

    [SerializeField] private float spawnRateS = 0.3f;

    [SerializeField] private int[] playerOverlapCount = { 0, 0, 0 };

    public void playerHitDeadzone(int overlappingColliderCount)
    {
        playerOverlapCount[overlappingColliderCount] += 1;
    }

    public void playerReachedGoal(Player player)
    {
        playersReachedGoalCount++;
        Destroy(player.gameObject);
    }

    public void playerLeftMap(Player player)
    {
        playersLeftMapCount++;
        Destroy(player.gameObject);
    }


    private void Start()
    {
        InvokeRepeating(nameof(SpawnPlayer), 0f, spawnRateS);
    }

    private void SpawnPlayer()
    {
        var player = Instantiate(playerPrefab, gameObject.transform, true);
        player.validPlayingField = validPlayingField;
        player.gameManager = this;
    }
}