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
                    if (ButtonMappings.CheckEscapeKey()) Application.Quit();
                    break;
                case "2_PlayerSelection":
                    if (ButtonMappings.CheckEscapeKey()) Application.Quit();
                    break;
                case "3_Instructions":
                    if (ButtonMappings.CheckEscapeKey()) SwitchToPlayerSelection();
                    if (ButtonMappings.CheckConfirmKey()) SwitchToMain();
                    break;
                case "4_Main":
                    if (ButtonMappings.CheckEscapeKey()) SwitchToPlayerSelection();
                    if (ButtonMappings.CheckReloadKey()) SwitchToMain();
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

        public static void SwitchToInstructions()
        {
            SceneManager.LoadScene("3_Instructions");
        }

        public static void SwitchToMain()
        {
            SessionStatistics.Instance!.Reset();
            SceneManager.LoadScene("4_Main");
        }
    }
}