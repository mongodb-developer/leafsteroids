using UnityEngine;

namespace DiepFake.Scenes.Game.ReplaySystem
{
    public class QuitButton : MonoBehaviour
    {
        public void QuitButtonClicked()
        {
            Application.Quit();
        }
    }
}