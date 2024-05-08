using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public string name;
    public int id;
}

public class TempScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerInfo p = new();
        PlayerPrefs.SetString("player", JsonUtility.ToJson(p));
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInfo p = (PlayerInfo)JsonUtility.FromJson(PlayerPrefs.GetString("player"), typeof(PlayerInfo));
    }
}
