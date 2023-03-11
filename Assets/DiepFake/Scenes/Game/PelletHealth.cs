using UnityEngine;

namespace DiepFake.Scenes.Game
{
    public class PelletHealth : MonoBehaviour
    {
        // Variables to control item health and fading
        public int maxHealth = 100;
        public float currentHealth;
        public Renderer itemRendererInner;
        public Renderer itemRendererOuter;

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
            currentHealth -= bullet.damage;

            // Destroy the bullet
            Destroy(bullet.gameObject);

            // Destroy the item if its health is depleted
            if (currentHealth <= 0) Destroy(gameObject);
        }
    }
}