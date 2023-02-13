using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class Pellet : MonoBehaviour
    {
        public int points = 10;

        private async void OnTriggerEnter2D(Collider2D other)
        {
            if (other!.gameObject.layer == LayerMask.NameToLayer("Pacman")) await Eat()!;
        }

        protected virtual async Task Eat()
        {
            await FindObjectOfType<GameManager>()!.PelletEaten(this);
        }
    }
}