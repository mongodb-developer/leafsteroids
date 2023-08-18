using UnityEngine;

namespace _6_Main_dynamic
{
    public class PelletSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject PalletLargeContainerPrefab;


        private void Start()
        {
            Instantiate(PalletLargeContainerPrefab);
        }
    }
}