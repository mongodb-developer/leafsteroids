using _3_Main;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private void Update()
    {
        GetComponent<TMP_Text>()!.text = $"Score: {SessionStatistics.Instance!.Score}";
    }
}