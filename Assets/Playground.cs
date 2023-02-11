using UnityEngine;

public class Playground : MonoBehaviour
{
    private void Start()
    {
        // Debug.Log("Start");
        //
        // var date = DateTime.Now;
        // var json1 = JsonUtility.ToJson(date);
        //
        // var number = 1f;
        // var json2 = JsonUtility.ToJson(number);
        //
        // var stringg = "foo";
        // var json3 = JsonUtility.ToJson(stringg);
        //
        // Debug.Log(json1);
        // Debug.Log(json2);
        // Debug.Log(json3);

        string playerName = "Dodo";
        int lives = 3;
        float health = 100f;

        Debug.Log(playerName);
        playerName = "bar";
        Debug.Log(playerName);

        Debug.Log(JsonUtility.ToJson(this));
    }
}