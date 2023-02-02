using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private GameObject validPlayingField;

    [SerializeField] private int playersReachedGoalCount;
    [SerializeField] private int playersLeftMapCount;

    [SerializeField] private float spawnRateS = 0.03f;

    [SerializeField] private int[] playerOverlapCount = { 0, 0, 0 };

    [SerializeField] private GameObject spawnLocation;

    private void OnEnable()
    {
        spawnLocation.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

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
        var player = Instantiate(playerPrefab, spawnLocation.gameObject.transform);
        player.gameObject.transform.parent = gameObject.transform;
        player.validPlayingField = validPlayingField;
        player.gameManager2 = this;
    }
}