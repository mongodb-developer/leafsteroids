using UnityEngine;

[RequireComponent(typeof(Ghost))]
public abstract class GhostBehavior : MonoBehaviour
{
    public float duration;
    protected Ghost Ghost { get; private set; }

    private void Awake()
    {
        Ghost = GetComponent<Ghost>();
    }

    public void Enable()
    {
        Enable(duration);
    }

    public virtual void Enable(float enabledDuration)
    {
        enabled = true;

        CancelInvoke();
        Invoke(nameof(Disable), enabledDuration);
    }

    public virtual void Disable()
    {
        enabled = false;

        CancelInvoke();
    }
}