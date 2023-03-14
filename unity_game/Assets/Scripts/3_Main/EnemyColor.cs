using UnityEngine;

namespace _3_Main
{
    public class EnemyColor : MonoBehaviour
    {
        public float colorChangeInterval = 0.08f;
        private Renderer _renderer;
        private float _timer;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer < colorChangeInterval) return;
            _timer = 0f;
            _renderer!.material!.color = new Color(255f, Random.value, Random.value);
        }
    }
}