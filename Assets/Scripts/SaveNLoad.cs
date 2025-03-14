using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveNLoad : MonoBehaviour
{

    public static SaveNLoad Instance;

    private bool isLoading;
    private Vector3Int vector;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SaveSystem.Init();
    }

    public void Save()
    {
        if (isLoading)
            return;

        Data data = new Data();

        PlayerManager playerManager = PlayerManager.Instance;
        PlayerStat playerStat = playerManager.playerStat;

        Vector3Int playerPosition = new Vector3Int(
            (int)playerManager.PlayerMovement.gameObject.transform.position.x, (int)playerManager.PlayerMovement.gameObject.transform.position.y, 0); ;

        data.playerX = playerPosition.x;
        data.playerY = playerPosition.y;
        data.playerZ = playerPosition.z;
        
        data.playerCurrentHP = playerStat.currentHp;
        data.playerCurrentMP = playerStat.currentStamina;

        data.questID = QuestManager.Instance.questId;
        data.questIndex = QuestManager.Instance.questActionIndex;

        data.mapName = playerManager.CurrentMapName;
        data.sceneName = playerManager.currentSceneName;


        data.varNameList = DatabaseManager.Instance.GetVarNames();
        data.varNumList = DatabaseManager.Instance.GetVars();
        data.switchNameList = DatabaseManager.Instance.GetSwitchNames();
        data.switchList = DatabaseManager.Instance.GetSwitches();

        List<Item> temp = Inventory.Instance.GetItemList();
        data.playerItemInventory = temp.Select(item => item.ItemSO.itemID).ToList();
        data.playerItemInventoryCount = temp.Select(item => item.itemCount).ToList();

        //convert data to json file
        string json = JsonUtility.ToJson(data);
        //save json in save folder
        SaveSystem.Save(json);
    }

    public void Load() //load all previously saved info to current
    {
        PlayerManager playerManager = PlayerManager.Instance;

        //get saved string from json 
        string saveString = SaveSystem.Load();

        //convert back to custom class
        if (saveString != null)
        {
            Data data = JsonUtility.FromJson<Data>(saveString);

            playerManager.SetLocationInfo(data.sceneName,data.mapName);
            vector.Set(data.playerX, data.playerY, data.playerZ);
            GameManager.Instance.SetSpawnPoint(vector);
            playerManager.playerStat.LoadStat(data.playerCurrentHP, data.playerCurrentMP);
            QuestManager.Instance.LoadQuestProgress(data.questID, data.questIndex);
            DatabaseManager.Instance.InitiateDatabase(data.varNameList, data.varNumList, data.switchNameList, data.switchList);

            List<Item> itemList = new List<Item>();

            //for all items in inventory,
            for(int i=0;i< data.playerItemInventory.Count; i++) {
                var matchingItem = DatabaseManager.Instance.TryGetItem(data.playerItemInventory[i]);
                if (matchingItem != null) {
                    itemList.Add(new Item(matchingItem, data.playerItemInventoryCount[i]));
                }
            }

            //load completed item list to inventory and update equip
            Inventory.Instance.LoadItem(itemList);
        }
        else
            Debug.Log("No saved file");
    }

   
}

