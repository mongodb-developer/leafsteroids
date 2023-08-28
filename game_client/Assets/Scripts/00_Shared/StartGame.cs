using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace _00_Shared
{
    public class StartGame : MonoBehaviour
    {
        [SerializeField] private PlayableDirector playableDirector;

        private void Start()
        {
            ResetPlayableDirector();
            StartCoroutine(WaitForTimeline());
        }

        private void ResetPlayableDirector()
        {
            Time.timeScale = 1f;
            playableDirector!.Stop();
            playableDirector!.time = 0;
            playableDirector.Evaluate();
            playableDirector!.Play();
        }

        private IEnumerator WaitForTimeline()
        {
            while (playableDirector!.state == PlayState.Playing) yield return null;
            SceneNavigation.SwitchToMainDynamic();
        }
    }
}