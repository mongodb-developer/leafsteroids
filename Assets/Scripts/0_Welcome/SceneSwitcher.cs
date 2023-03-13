using UnityEngine;
using UnityEngine.SceneManagement;

namespace _0_Welcome
{
    public class SceneSwitcher : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button1))
                SceneManager.LoadScene("1_Loading");
        }
    }
}