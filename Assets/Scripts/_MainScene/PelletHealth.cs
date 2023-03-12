using System;
using __Shared;
using _LoadingScene;
using UnityEngine;
using UnityEngine.Serialization;

namespace _MainScene
{
    public class PelletHealth : MonoBehaviour
    {
        [FormerlySerializedAs("size")] [SerializeField]
        private PelletSize pelletSize;

        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;
        public Renderer itemRendererInner;
        public Renderer itemRendererOuter;

        private GameConfig _gameConfig;

        private void Awake()
        {
            _gameConfig = GameConfigLoader.Instance!.GameConfig;
            maxHealth = pelletSize switch
            {
                PelletSize.Small => _gameConfig!.PelletHeatlhSmall,
                PelletSize.Medium => _gameConfig!.PelletHeatlhMedium,
                PelletSize.Large => _gameConfig!.PelletHeatlhLarge,
                _ => throw new ArgumentOutOfRangeException()
            };
            currentHealth = maxHealth;
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

        private enum PelletSize
        {
            Small = 0,
            Medium = 1,
            Large = 2
        }
    }
}