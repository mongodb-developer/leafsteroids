using UnityEngine;

namespace _6_Main
{
    public class EnemyColor : MonoBehaviour
    {
        public float colorChangeInterval = 0.1f;
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
            var red = Random.value;
            if (red < 0.5f) red = 0.5f;
            _renderer!.material!.color = new Color(red, 0f, 0f);
        }
    }
}