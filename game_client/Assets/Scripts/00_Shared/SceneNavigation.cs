using _1_Loading;
using _3_Main;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

namespace _00_Shared
{
    public class SceneNavigation : MonoBehaviour
    {

        [DllImport("__Internal")]
        private static extern string QuitGame();
        private void Update()
        {
            switch (SceneManager.GetActiveScene().name!)
            {
                case Constants.SceneNames.Loading:
                    if (ButtonMappings.CheckEscapeKey()) SwitchToWelcome();
                    break;
                case Constants.SceneNames.Welcome:
                    if (ButtonMappings.CheckEscapeKey())
                    {
                        Quit();   
                    }
                    if (ButtonMappings.CheckConfirmKey()) SwitchToMain();
                    break;
                case Constants.SceneNames.PlayerSelection:
                    if (ButtonMappings.CheckEscapeKey()) SwitchToWelcome();
                    break;
                case Constants.SceneNames.Instructions:
                    if (ButtonMappings.CheckEscapeKey()) SwitchToPlayerSelection();
                    if (ButtonMappings.CheckConfirmKey()) SwitchToMain();
                    break;
                case Constants.SceneNames.Main:
                    if (ButtonMappings.CheckEscapeKey()) SwitchToWelcome();
                    if (ButtonMappings.CheckReloadKey()) SwitchToMain();
                    break;
                case Constants.SceneNames.Playground:
                    if (ButtonMappings.CheckEscapeKey()) SwitchToPlayerSelection();
                    if (ButtonMappings.CheckReloadKey()) SwitchToMain();
                    break;
            }
        }


	public static void Quit()
	{
		QuitGame();
                Application.Quit();
	}

        public static void SwitchToEventSelection()
        {
            SceneManager.LoadScene(Constants.SceneNames.EventSelection);
        }

        public static void SwitchToWelcome()
        {
            SceneManager.LoadScene(Constants.SceneNames.Welcome);
        }

        private static void SwitchToLoading()
        {
            // if (GameConfigLoader.Instance != null)
            //     GameConfigLoader.Instance.GameConfig = null;
            SceneManager.LoadScene(Constants.SceneNames.Loading);
        }

        public static void SwitchToPlayerSelection()
        {
            SceneManager.LoadScene(Constants.SceneNames.PlayerSelection);
        }

        public static void SwitchToInstructions()
        {
            SceneManager.LoadScene(Constants.SceneNames.Instructions);
        }

        public static void SwitchToMain()
        {
            SessionStatistics.Instance!.Reset();
            SceneManager.LoadScene(Constants.SceneNames.Main);
        }

        public static void SwitchToPlayground()
        {
            SessionStatistics.Instance!.Reset();
            SceneManager.LoadScene(Constants.SceneNames.Playground);
        }
    }
}