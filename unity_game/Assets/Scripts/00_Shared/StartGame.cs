using System.Collections;
using _00_Shared;
using UnityEngine;
using UnityEngine.Playables;

public class StartGame : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;

    private void Start()
    {
        playableDirector!.Play();
        StartCoroutine(WaitForTimeline());
    }

    private IEnumerator WaitForTimeline()
    {
        while (playableDirector!.state == PlayState.Playing) yield return null;
        SceneNavigation.SwitchToMain();
    }
}