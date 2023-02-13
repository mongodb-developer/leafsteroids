using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Ghost))]
    public abstract class GhostBehavior : MonoBehaviour
    {
        protected Ghost Ghost { get; private set; }
        public float duration;

        private void Awake()
        {
            Ghost = GetComponent<Ghost>();
        }

        public void Enable()
        {
            Enable(duration);
        }

        public virtual void Enable(float d)
        {
            enabled = true;

            CancelInvoke();
            Invoke(nameof(Disable), d);
        }

        public virtual void Disable()
        {
            enabled = false;

            CancelInvoke();
        }
    }
}