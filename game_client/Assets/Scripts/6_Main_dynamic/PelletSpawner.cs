using UnityEngine;

namespace _6_Main_dynamic
{
    public class PelletSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject palletParent;
        [SerializeField] private GameObject palletLargeContainerPrefab;


        private void Start()
        {
            Instantiate(palletLargeContainerPrefab, new Vector3(3f, 0.05f, 3f), Quaternion.identity,
                palletParent.gameObject.transform);
        }
    }
}