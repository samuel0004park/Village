using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data {
    public int playerX;
    public int playerY;
    public int playerZ;

    public int questID;
    public int questIndex;

    public List<int> playerItemInventory;
    public List<int> playerItemInventoryCount;

    public Location.MapNames mapName;
    public Location.SceneNames sceneName;

    public List<bool> switchList;
    public List<string> switchNameList;
    public List<string> varNameList;
    public List<float> varNumList;

    public Data() {
        playerItemInventory = new List<int>();
        playerItemInventoryCount = new List<int>();
        switchList = new List<bool>();
        switchNameList = new List<string>();
        varNameList = new List<string>();
        varNumList = new List<float>();
    }
} 
