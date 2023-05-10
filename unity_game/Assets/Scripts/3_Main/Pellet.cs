using System;
using _00_Shared;
using _1_Loading;
using UnityEngine;

namespace _3_Main
{
    public class Pellet : MonoBehaviour
    {
        public Renderer itemRendererInner;
        public Renderer itemRendererOuter;
        public HealthBar healthBar;

        [SerializeField] private PelletSize pelletSize;
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;

        private GameConfig _gameConfig;

        private void Start()
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
            healthBar!.SetMaxHealth(maxHealth);
        }

        private void Update()
        {
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
            var bullet = collision!.gameObject!.GetComponent<Bullet>();
            if (bullet == null) return;

            currentHealth -= bullet.damage;
            healthBar!.SetCurrentHealth(currentHealth);
            SessionStatistics.Instance!.DamageDone += _gameConfig!.BulletDamage;

            Destroy(bullet.gameObject);

            if (currentHealth > 0) return;

            Destroy(transform.parent!.gameObject);
            SessionStatistics.Instance.Score += maxHealth;
            switch (pelletSize)
            {
                case PelletSize.Small:
                    SessionStatistics.Instance!.PelletsDestroyedSmall++;
                    break;
                case PelletSize.Medium:
                    SessionStatistics.Instance!.PelletsDestroyedMedium++;
                    break;
                case PelletSize.Large:
                    SessionStatistics.Instance!.PelletsDestroyedLarge++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum PelletSize
        {
            Small = 0,
            Medium = 1,
            Large = 2
        }
    }
}