using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    static public DatabaseManager Instance;
    
    [SerializeField]private List<ItemSO> itemList = new List<ItemSO>(); 
    [SerializeField]private Dictionary<int, ItemSO> itemDictionary = new Dictionary<int, ItemSO>(); 

    private List<string> var_name = new List<string>();
    private List<float> var = new List<float>();
    private List<string> switch_name = new List<string>();
    private List<bool> switches = new List<bool>();

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SetUp();
    }

    public void InitiateDatabase(List<string> arr1, List<float> arr2, List<string> arr3, List<bool> arr4) {
        var_name = arr1;
        var = arr2;
        switch_name = arr3;
        switches = arr4;
    }

    public void SetUp() {
        foreach(var itemSO in itemList) {
            itemDictionary.Add(itemSO.itemID, itemSO);
        }
    }

    #region Getter

    public ItemSO TryGetItem(int itemID) {
        if(itemDictionary.ContainsKey(itemID))
            return itemDictionary[itemID];
        else
            return null;
    }

    public List<string> GetVarNames() {
        return new List<string>(var_name);
    }
    public List<float> GetVars() {
        return new List<float>(var);
    }
    public List<string> GetSwitchNames() {
        return new List<string>(switch_name);
    }
    public List<bool> GetSwitches() {
        return new List<bool>(switches);
    }
    #endregion

}
