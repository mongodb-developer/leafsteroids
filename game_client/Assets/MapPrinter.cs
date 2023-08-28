using UnityEngine;

public class MapPrinter : MonoBehaviour
{
    [SerializeField] private GameObject Pallets;
    [SerializeField] private GameObject PowerUps;
    [SerializeField] private GameObject Enemies;

    void Start()
    {
        foreach (Transform palletCompound in Pallets.transform)
        {
            foreach (Transform pallet in palletCompound)
            {
                Debug.Log($"Pallet: ({pallet.position.x}, {pallet.position.y}, {pallet.position.z})");
            }
        }

        foreach (Transform powerup in PowerUps.transform)
        {
            Debug.Log($"Powerup: ({powerup.position.x}, {powerup.position.y}, {powerup.position.z})");
        }

        foreach (Transform enemy in Enemies.transform)
        {
            Debug.Log($"Enemy: ({enemy.position.x}, {enemy.position.y}, {enemy.position.z})");
        }
        
        // var json = """
        //              [
        //                {
        //                  "pallets": [
        //                    {
        //                      "position": {
        //                        "x": 3,
        //                        "y": 0.05,
        //                        "z": 3
        //                      },
        //                      "pallet_type": "Large"
        //                    },
        //                    {
        //                      "position": {
        //                        "x": 4,
        //                        "y": 0.05,
        //                        "z": 4
        //                      },
        //                      "pallet_type": "Medium"
        //                    }
        //                  ],
        //                  "power_ups": [
        //                    {
        //                      "position": {
        //                        "x": 50,
        //                        "y": 60,
        //                        "z": 60
        //                      }
        //                    }
        //                  ],
        //                  "enemies": [
        //                    {
        //                      "position": {
        //                        "x": 70,
        //                        "y": 80,
        //                        "z": 60
        //                      }
        //                    },
        //                    {
        //                      "position": {
        //                        "x": 90,
        //                        "y": 100,
        //                        "z": 60
        //                      }
        //                    }
        //                  ]
        //                }
        //              ]
        //            """;
    }
}


