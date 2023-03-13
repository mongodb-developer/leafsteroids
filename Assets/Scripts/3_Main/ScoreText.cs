using TMPro;
using UnityEngine;

namespace _3_Main
{
    public class ScoreText : MonoBehaviour
    {
        private void Update()
        {
            GetComponent<TMP_Text>()!.text = $"Score: {SessionStatistics.Instance!.Score}";
        }
    }
}