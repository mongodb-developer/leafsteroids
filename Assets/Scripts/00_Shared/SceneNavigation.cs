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
                    if (Input.GetKeyDown(KeyCode.Joystick1Button1))
                        SceneManager.LoadScene("1_Loading");
                    break;
                case "1_Loading":
                    break;
                case "2_PlayerSelection":
                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button3))
                        Application.Quit();
                    break;
                case "3_Main":
                    break;
            }
        }
    }
}