using _1_Loading;
using TMPro;
using UnityEngine;

namespace _3_Welcome
{
    public class Welcome : MonoBehaviour
    {
        [SerializeField] private TMP_Text welcomeTextField;

        private void Start()
        {
            welcomeTextField!.text = $"Welcome to\n{GameConfigLoader.Instance!.GameConfig!.Event!.Name}";
        }
    }
}