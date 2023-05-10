using UnityEngine;
using UnityEngine.UI;

namespace _3_Main
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;
        public Gradient gradient;
        public Image fill;

        public void SetMaxHealth(float health)
        {
            slider!.maxValue = health;
            fill!.color = gradient!.Evaluate(1f);
            SetCurrentHealth(health);
        }

        public void SetCurrentHealth(float health)
        {
            slider!.value = health;
            fill!.color = gradient!.Evaluate(slider.normalizedValue);
        }

        private void Start()
        {
            SetMaxHealth(100);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetCurrentHealth((int)(slider!.value - 10f));
            }
        }
    }
}