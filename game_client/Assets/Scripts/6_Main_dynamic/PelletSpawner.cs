using System.Collections.Generic;
using _00_Shared;
using _00_Shared.Map;
using UnityEngine;

namespace _6_Main_dynamic
{
    public class PelletSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject palletParent;
        [SerializeField] private GameObject palletLargeContainerPrefab;

        private readonly Map _map = new()
        {
            Pallets = new List<PalletDto>
            {
                new()
                {
                    Position = new ObjectPosition(new Vector3(3f, 0.05f, 3f)),
                    PalletType = PalletType.Large
                }
            },
            PowerUps = new List<PowerUpDto>(),
            Enemies = new List<EnemyDto>()
        };

        private void Start()
        {
            CreatePallets();
        }

        private void CreatePallets()
        {
            foreach (var pallet in _map.Pallets)
            {
                if (pallet.PalletType == PalletType.Large)
                {
                    Instantiate(palletLargeContainerPrefab, pallet.Position.ToVector3(), Quaternion.identity,
                        palletParent.gameObject.transform);
                }
            }
        }
    }
}