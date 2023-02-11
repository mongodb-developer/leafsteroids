using UnityEngine;

namespace ReplaySystem
{
    public class QuitButton : MonoBehaviour
    {
        public void QuitButtonClicked()
        {
            Application.Quit();
        }
    }
}