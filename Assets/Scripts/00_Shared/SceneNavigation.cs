using UnityEngine;
using UnityEngine.SceneManagement;

namespace _00_Shared
{
    public class SceneNavigation : MonoBehaviour
    {
        [SerializeField] private ButtonMappings buttonMappings;

        private void Update()
        {
            switch (SceneManager.GetActiveScene().name!)
            {
                case "0_Welcome":
                    if (buttonMappings!.CheckConfirmKey())
                        SceneManager.LoadScene("1_Loading");
                    break;
                case "1_Loading":
                    break;
                case "2_PlayerSelection":
                    if (buttonMappings!.CheckEscapeKeu())
                        Application.Quit();
                    break;
                case "3_Main":
                    break;
            }
        }

        public void SwitchToPlayerSelection()
        {
            SceneManager.LoadScene("2_PlayerSelection");
        }

        public void SwitchToMainScene()
        {
            SceneManager.LoadScene("3_Main");
        }
    }
}