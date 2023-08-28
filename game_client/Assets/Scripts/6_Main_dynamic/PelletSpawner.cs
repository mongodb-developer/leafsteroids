using _00_Shared.Map;
using _1_Loading;
using UnityEngine;

namespace _6_Main_dynamic
{
    public class PelletSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject palletParent;
        [SerializeField] private GameObject palletLargeContainerPrefab;
        [SerializeField] private GameObject palletMediumContainerPrefab;
        [SerializeField] private GameObject palletSmallContainerPrefab;

        private void Start()
        {
            CreatePallets();
        }

        private void CreatePallets()
        {
            var map = GameConfigLoader.Instance.GameConfig!.Maps![0];
            foreach (var pallet in map.Pallets)
            {
                if (pallet.PalletType == PalletType.Large)
                {
                    Instantiate(palletLargeContainerPrefab, pallet.Position.ToVector3(), Quaternion.identity,
                        palletParent.gameObject.transform);
                }

                if (pallet.PalletType == PalletType.Medium)
                {
                    Instantiate(palletMediumContainerPrefab, pallet.Position.ToVector3(), Quaternion.identity,
                        palletParent.gameObject.transform);
                }

                if (pallet.PalletType == PalletType.Small)
                {
                    Instantiate(palletSmallContainerPrefab, pallet.Position.ToVector3(), Quaternion.identity,
                        palletParent.gameObject.transform);
                }
            }
        }
    }
}