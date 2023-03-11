using _MainScene._ReplaySystem;
using UnityEngine;

namespace _MainScene
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Recorder recorder;


        private void Start()
        {
            Invoke(nameof(PersistRecording), 30f);
        }

        private void PersistRecording()
        {
            recorder!.PersistRecording();
        }
    }
}