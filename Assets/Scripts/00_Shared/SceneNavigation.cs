using _3_Main;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _00_Shared
{
    public class SceneNavigation : MonoBehaviour
    {
        private void Update()
        {
            switch (SceneManager.GetActiveScene().name!)
            {
                case "0_Welcome":
                    if (ButtonMappings.CheckEscapeKey()) Application.Quit();
                    if (ButtonMappings.CheckConfirmKey()) SwitchToLoading();
                    break;
                case "1_Loading":
                    break;
                case "2_PlayerSelection":
                    if (ButtonMappings.CheckEscapeKey()) Application.Quit();
                    break;
                case "3_Main":
                    if (ButtonMappings.CheckReloadKey()) SwitchToMainScene();
                    if (ButtonMappings.CheckEscapeKey()) SwitchToPlayerSelection();
                    break;
            }
        }

        private static void SwitchToLoading()
        {
            SceneManager.LoadScene("1_Loading");
        }

        public static void SwitchToPlayerSelection()
        {
            SceneManager.LoadScene("2_PlayerSelection");
        }

        public static void SwitchToMainScene()
        {
            SessionStatistics.Instance!.Reset();
            SceneManager.LoadScene("3_Main");
        }
    }
}