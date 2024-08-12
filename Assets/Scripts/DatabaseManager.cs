using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
            Destroy(this.gameObject);
        theStat = FindObjectOfType<PlayerStat>();
    }
    #endregion Singleton
    static public DatabaseManager instance;

    //
    public string[] var_name;
    public float[] var;

    //to track which events occured in-game
    public string[] switch_name;
    public bool[] switches; 
    
    [Header("Data Structures")]
    public List<Item> itemList = new List<Item>(); //list that contains all items in game

    [Header("References")]
    public GameObject parent;
    private PlayerStat theStat;

    void Start()
    {
        //USE
        itemList.Add(new Item(10001, "우유", "싱싱한 우유다. 마시면 체력을 회복할 수 있다", Item.ItemType.Use));
        itemList.Add(new Item(10002, "영약", "피처럼 진하게 빨간 약이다. 마시면 힘이 쌔진다", Item.ItemType.Use));
        itemList.Add(new Item(10003, "딸기", "잘 익은 딸기다.", Item.ItemType.Use));
        itemList.Add(new Item(10004, "빵", "겉은 바삭하고 속은 촉촉한 빵이다", Item.ItemType.Use));
        //ETC
        itemList.Add(new Item(20001, "사탕무","사탕무 씨앗이다.",Item.ItemType.ETC));
        itemList.Add(new Item(20002, "슬라임 액", "슬라임의 성분에서 90%를 차지하는 걸쭉한 액체다", Item.ItemType.ETC));
        itemList.Add(new Item(20003, "밀", "농부들의 땀과 자연의 도움으로 만들어진 밀이다", Item.ItemType.ETC));
        itemList.Add(new Item(20004, "빨간 열매", "불타는 듯 이글거리는 열매다", Item.ItemType.ETC));
        itemList.Add(new Item(20005, "주황 열매", "아름다운 노을빛으로 감싸진 열매다", Item.ItemType.ETC));
        //QUEST
        itemList.Add(new Item(30003, "금동전", "제법 무개가 나가는 금동전이다", Item.ItemType.Quest, "동전을 신사에 바친다", "바치지 않는다"));
        itemList.Add(new Item(30004, "장작", "불을 짚이기 적당한 장작이다", Item.ItemType.Quest, "장작을 화로안에 넣는다", "넣지 않는다"));
        itemList.Add(new Item(30005, "물고기", "싱싱한 물고기다", Item.ItemType.Quest, "물고기를 접시 위에 올려둔다","아무 것도 하지 않는다"));
        itemList.Add(new Item(30006, "철 파편", "화덕에서 떨어져 나온 파편이다", Item.ItemType.Quest, "철을 산사에 바친다", "바치지 않는다"));
        itemList.Add(new Item(30007, "해바라기", "태양같이 빛을 뿜어내는 꽃이다", Item.ItemType.Quest, "해바라기를 들어올린다", "아무 것도 하지 않는다"));
        itemList.Add(new Item(30008, "형의 기억", "자신을 위해서 희생하는 동생을 바라보는 슬픔이 담긴 기억이다", Item.ItemType.Quest, "", ""));
        itemList.Add(new Item(30009, "동생의 기억", "자신을 위해서 희생하는 형을 바라보는 슬픔이 담긴 기억이다", Item.ItemType.Quest, "", ""));


    }
    public int LookFor(int itemId)
    {
        ///recursively look for item with given itemID
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemId == itemList[i].itemID)
                return i;
        }
        return 0;
    }

    public void UseItem(int _itemID)
    {
        switch (_itemID)
        {
            case 10001: //milk
                theStat.Heal_HP(1);
                break;
            case 10002: //red_potion
                theStat.StaminaBuff(10);
                break;
            case 10003://strawberry
                theStat.Heal_HP(1);
                theStat.Heal_Stamina(200);
                break;
            case 10005://bread
                theStat.Heal_HP(3);
                theStat.Heal_Stamina(300);
                break;
            default:
                break;
        }
    }

}
