using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveNLoad : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public float playerX;
        public float playerY;
        public float playerZ;

        public int playerHP;
        public int playerMP;

        public int playerCurrentHP;
        public int playerCurrentMP;

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
    } //custom class with all save info

    [Header("References")]
    private PlayerManager thePlayer;
    private PlayerStat theStat;
    private Inventory theInven;
    private DatabaseManager theDatabase;
    private QuestManager theQuest;

    public Data data;

    public bool isLoading;
    public Vector3 vector;

    private void Awake()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theStat = FindObjectOfType<PlayerStat>();
        theInven = FindObjectOfType<Inventory>();
        theDatabase = FindObjectOfType<DatabaseManager>();
        theQuest = FindObjectOfType<QuestManager>();
        SaveSystem.Init();
    }
    public void Save() //save neccessary info into custom class and export as json file
    {
        //if loading prevent save so it doesnt save each time jelly is added
        if (isLoading)
            return;

        data.playerX = thePlayer.transform.position.x;
        data.playerY = thePlayer.transform.position.y;
        data.playerZ = thePlayer.transform.position.z;
        
        data.playerHP = theStat.hp;
        data.playerMP = theStat.stamina;
        data.playerCurrentHP = theStat.currentHp;
        data.playerCurrentMP = theStat.currentStamina;

        data.questID = theQuest.questId;
        data.questIndex = theQuest.questActionIndex;

        data.mapName = thePlayer.currentMapName;
        data.sceneName = thePlayer.currentSceneName;


        data.playerItemInventory.Clear();
        data.playerItemInventoryCount.Clear();

        for (int i = 0; i < theDatabase.var_name.Length; i++)
        {
            data.varNameList.Add(theDatabase.var_name[i]);
            data.varNumList.Add(theDatabase.var[i]);
        }
        for (int i = 0; i < theDatabase.switch_name.Length; i++)
        {
            data.switchNameList.Add(theDatabase.switch_name[i]);
            data.switchList.Add(theDatabase.switches[i]);
        }

        List<Item> temp = theInven.SaveItem();

        for (int i = 0; i < temp.Count; i++)
        {
            data.playerItemInventory.Add(temp[i].itemID);
            data.playerItemInventoryCount.Add(temp[i].itemCount);
        }
        //convert data to json file
        string json = JsonUtility.ToJson(data);
        //save json in save folder
        SaveSystem.Save(json);
    }

    public void Load() //load all previously saved info to current
    {
        //get saved string from json 
        string saveString = SaveSystem.Load();

        //convert back to custom class
        if (saveString != null)
        {
            Data data = JsonUtility.FromJson<Data>(saveString);

            thePlayer.currentMapName = data.mapName;
            thePlayer.currentSceneName = data.sceneName;
            vector.Set(data.playerX, data.playerY, data.playerZ);
            thePlayer.transform.position = vector;
            theStat.hp = data.playerHP;
            theStat.stamina = data.playerMP;
            theStat.currentHp = data.playerCurrentHP;
            theStat.currentStamina = data.playerCurrentMP;

            theQuest.questId = data.questID;
            theQuest.questActionIndex = data.questIndex;

            theDatabase.var = data.varNumList.ToArray();
            theDatabase.var_name = data.varNameList.ToArray();
            theDatabase.switch_name = data.switchNameList.ToArray();
            theDatabase.switches = data.switchList.ToArray();

            List<Item> itemList = new List<Item>();

            //for all items in inventory,
            for (int i = 0; i < data.playerItemInventory.Count; i++)
                //for all items in database
                for (int j = 0; j < theDatabase.itemList.Count; j++)
                    //if stored equip item matches, add to item list
                    if (data.playerItemInventory[i] == theDatabase.itemList[j].itemID)
                    {
                        itemList.Add(theDatabase.itemList[j]);
                        break;
                    }

            //match count for items in inventory
            for (int i = 0; i < data.playerItemInventoryCount.Count; i++)
                itemList[i].itemCount = data.playerItemInventoryCount[i];

            //load completed item list to inventory and update equip
            theInven.LoadItem(itemList);
        }
        else
            Debug.Log("No saved file");
    }

   
}

