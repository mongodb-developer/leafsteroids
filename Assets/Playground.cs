using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Playground : MonoBehaviour
{
    private void Start()
    {
        var data = new Data
        {
            PlayerName = "Dodo",
            Lives = 3,
            Health = 100f,
            DateTimee = DateTime.Now,
            Position = new Vector3(1, 1, 1),
            Bullshit = new[] { new Vector3(2, 2, 2) },
            BullshitAgain = new List<Vector3>() { new Vector3(3, 3, 3) }
        };
        Debug.Log(data);
        Debug.Log(JsonUtility.ToJson(data));
        Debug.Log(JsonConvert.SerializeObject(data));
    }
}

public struct Data
{
    public string PlayerName;
    public int Lives;
    public float Health;
    public DateTime DateTimee;
    public Vector3 Position;
    public Vector3[] Bullshit;
    public List<Vector3> BullshitAgain;

    public override string ToString()
    {
        return $"\n{PlayerName}\n{Lives}\n{Health}\n{DateTimee}\n{Position}\n{Bullshit}\n{BullshitAgain}";
    }
}