using __Shared;
using _LoadingScene;
using UnityEngine;

namespace _MainScene
{
    public class PelletHealth : MonoBehaviour
    {
        // Variables to control item health and fading
        public int maxHealth = 100;
        public float currentHealth;
        public Renderer itemRendererInner;
        public Renderer itemRendererOuter;

        private GameConfig _gameConfig;

        private void Awake()
        {
            _gameConfig = GameConfigLoader.Instance!.GameConfig;
            Debug.Log(_gameConfig!.RoundDuration);
        }


        private void Start()
        {
            // Set the item's current health to the max health
            currentHealth = maxHealth;
        }

        // Update is called once per frame
        private void Update()
        {
            // Fade the item as its health decreases
            var alpha = Mathf.Lerp(1f, 0.25f, 1f - currentHealth / maxHealth);
            var colorInner = itemRendererInner!.material!.color;
            colorInner.a = alpha;
            itemRendererInner.material.color = colorInner;

            var colorOuter = itemRendererOuter!.material!.color;
            colorOuter.a = alpha;
            itemRendererOuter!.material!.color = colorOuter;
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Pellet only care about bullets.
            var bullet = collision!.gameObject!.GetComponent<Bullet>();
            if (bullet == null) return;

            // Reduce the item's health by the bullet's damage
            currentHealth -= _gameConfig!.BulletDamage;

            // Destroy the bullet
            Destroy(bullet.gameObject);

            // Destroy the item if its health is depleted
            if (currentHealth <= 0) Destroy(gameObject);
        }
    }
}