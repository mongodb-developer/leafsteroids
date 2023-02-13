using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class Pellet : MonoBehaviour
    {
        public int points = 10;

        protected virtual async Task Eat()
        {
            await FindObjectOfType<GameManager>()!.PelletEaten(this);
        }

        private async Task OnTriggerEnter2D(Collider2D other)
        {
            if (other!.gameObject.layer == LayerMask.NameToLayer("Pacman"))
            {
                await Eat()!;
            }
        }
    }
}