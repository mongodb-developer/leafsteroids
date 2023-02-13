using System.Threading.Tasks;

namespace Game
{
    public class PowerPellet : Pellet
    {
        public float duration = 8f;

        protected override async Task Eat()
        {
            await FindObjectOfType<GameManager>()!.PowerPelletEaten(this);
        }
    }
}