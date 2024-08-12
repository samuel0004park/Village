using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("References")]
    private OOCManager theOOC;

    [Tooltip("Slots Parent")]
    public Transform tf; //parent of slots

    [Tooltip("Inventory UI GameObject")]
    public GameObject go; //gameobject of inventory
    public GameObject go_OOC_UP;
    public GameObject go_OOC_DOWN;
    
    [Space]
    [Header("Prefab")]
    public GameObject prefab_FloatingText;

    [Space]
    [Header("Audio Name")]
    private string enter_sound = "enter_sound";
    private string open_sound = "open_sound";
    private string beep_sound = "beep";
    private string switch_sound = "switch_sound";

    [Space]
    [Header("Inventory Slots")]
    private InventorySlot[] slots; //each slots in grid
    private List<Item> inventoryItemList; //total item player currently has
    private List<Item> inventoryTabList; //item in tab, just for show (not actual item)

    [Space]
    [Header("Text and description")]
    public Text Description_Text; //descripton on selected item
    private int selectedItem;

    [Space]
    [Header("Variables")]
    [SerializeField] private bool activated; //var to keep track if inventory is active
    public bool stopKeyInput; //to to stop uneeded key inputs
    private bool preventExec; //prevent overlap
    public int page;
    private int slotCount; //number of slots active in inventory tab, from 0~9 
    private const int MAX_SLOTS_COUNT = 10;
    public int holdingItemID=0;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    #region Singleton
    static public Inventory instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion 

    void Start()
    {
        theOOC = FindObjectOfType<OOCManager>();
        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf.GetComponentsInChildren<InventorySlot>();
    }
    void Update()
    {
        //if can do key input
        if (!stopKeyInput)
        {
            if (activated &&  Input.GetKeyDown(KeyCode.Escape))
            {
                Hide();
            }
            //when press I(inventory)
            if (Input.GetKeyDown(KeyCode.I)) {

                activated = !activated;
                //if current state is activated state, show inventory
                if (activated)
                {
                    UIController.instance.isLive = false;
                    UIController.instance.pannelUP = true;
                    AudioManager.instance.Play(open_sound);
                    OrderManager.instance.ForceStop(2);
                    go.SetActive(true);
         
                    ShowItem();
  
                }
                //else, close inventory
                else if (!activated)
                {
                    Hide();
                }
            }
            //if is activated
            if (activated)
            {
               
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    //if up arrow should move to next page
                    if (selectedItem - 2 < 0)
                    {
                        //and if this is not the first, go to next page
                        if (page !=0)
                            page--;
                        //else, reset to last page
                        else
                            page = (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT;

                        RemoveSlots();
                        ShowPage();
                    }

                    if (selectedItem > 1)
                        selectedItem -= 2;
                    //select is on even and there are even slots
                    else if(CheckEven(slotCount-1))
                    {
                        if(selectedItem==1)
                            selectedItem = slotCount;
                        else
                            selectedItem = slotCount - 1;
                    }
                    else if(!CheckEven(slotCount-1)){
                        if (selectedItem == 1)
                        {
                            selectedItem = slotCount - 1;
                        }
                        else
                            selectedItem = slotCount;
                    }
                    AudioManager.instance.Play(switch_sound);
                    SelectedItem();
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (selectedItem + 2 > slotCount)
                    {
                        //and if this is not the last page, go to next page
                        if (page < (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT)
                            page++;
                        //else, reset to first page
                        else
                            page = 0;

                        RemoveSlots();
                        ShowPage();

                        //if currently in even slots, -1 to end up in +1
                        if (CheckEven(selectedItem - 1))
                            selectedItem = -1;
                        else
                            selectedItem = -2;
                    }

                    if (selectedItem < slotCount - 1)
                            selectedItem += 2;
                    else 
                        selectedItem %= 2;
                    AudioManager.instance.Play(switch_sound);
                    SelectedItem();
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    //if up arrow should move to next page
                    if (selectedItem - 1 < 0)
                    {
                        //and if this is not the first, go to next page
                        if (page != 0)
                            page--;
                        //else, reset to last page
                        else
                            page = (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT;

                        RemoveSlots();
                        ShowPage();
                    }
                    selectedItem--;
                    if (selectedItem < 0)
                        selectedItem = slotCount;
                    AudioManager.instance.Play(switch_sound);
                    SelectedItem();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (selectedItem + 1 > slotCount)
                    {
                        //and if this is not the last page, go to next page
                        if (page < (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT)
                            page++;
                        //else, reset to first page
                        else
                            page = 0;

                        RemoveSlots();
                        ShowPage();
                        selectedItem = -1;
                    }

                    selectedItem++;
                    if (selectedItem > slotCount)
                        selectedItem = 0;
                    AudioManager.instance.Play(switch_sound);
                    SelectedItem();
                }
                else if (Input.GetKeyDown(KeyCode.Z) && !preventExec)
                {
                    //usable item
                    switch (inventoryTabList[selectedItem].itemType)
                    {
                        case Item.ItemType.Use:
                            StartCoroutine(OOC_Coroutine(inventoryTabList[selectedItem].useDescriptionTop, inventoryTabList[selectedItem].useDescriptionBottom));
                            break;
                        case Item.ItemType.Quest:
                            StartCoroutine(QuesItemUseCoroutine());
                            break;
                        default:
                            AudioManager.instance.Play(beep_sound);
                            break;
                    };
                }
                //prevent z from selecting item right after selecting tab
                if (Input.GetKeyUp(KeyCode.Z))
                {
                    holdingItemID = 0;
                    preventExec = false;
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    Hide();
                }
            }
        }
    }

    private void Hide()
    {
        StopAllCoroutines();
        AudioManager.instance.Play(open_sound);
        OrderManager.instance.ContinueMove();
        go.SetActive(false);
        UIController.instance.isLive = true;
        UIController.instance.pannelUP = false;
        activated = false;
    }

    private void FloatText(int i)
    {
        var clone = Instantiate(prefab_FloatingText, PlayerManager.instance.transform.position, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingText>().text.text = "+ " + GameManager.instance.theData.itemList[i].itemName;
        clone.transform.SetParent(this.transform);
    }

    public int LookFor(int _itemID)
    {
        for (int i = 0; i < GameManager.instance.theData.itemList.Count; i++)
        {
            //find item with same id 
            if (GameManager.instance.theData.itemList[i].itemID == _itemID)
            {
                //if found, check if there is an item with same id already in inventory
                for (int j = 0; j < inventoryItemList.Count; j++)
                {
                    //... if yes 
                    if (inventoryItemList[j].itemID == _itemID)
                    {
                        return inventoryItemList[j].itemCount;
                    }
                }
            }
        }
        return 0;
    }
    public void GetItem(int _itemID, int _count=1)
    {
        //for all items in database
        for (int i = 0; i < GameManager.instance.theData.itemList.Count; i++)
        {
            //find item with same id 
            if(GameManager.instance.theData.itemList[i].itemID == _itemID)
            {
                //create floating text with item name
                FloatText(i);
                //if found, check if there is an item with same id already in inventory
                for (int j = 0; j < inventoryItemList.Count; j++)
                {
                    //... if yes 
                    if (inventoryItemList[j].itemID == _itemID)
                    {
                        //... and if it is a 소모품 type or ETC, only increase count
                        if (inventoryItemList[j].itemType == Item.ItemType.Use|| inventoryItemList[j].itemType == Item.ItemType.ETC|| inventoryItemList[j].itemType == Item.ItemType.Quest)
                        {
                            inventoryItemList[j].itemCount += _count;
                            return;
                        }
                        //... if its else, add another item of same name
                        else
                        {
                            inventoryItemList.Add(GameManager.instance.theData.itemList[i]);
                            return;
                        }
                    }
                }
                //...if no, add new item to list
                inventoryItemList.Add(GameManager.instance.theData.itemList[i]);
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
                return;
            }
        }
        Debug.Log("오류: 해당 아이템이 데이터베이스에서 찾을 수 없었습니다... 아이디를 확인하거나 아이템을 추가해 주세요.");
    } //Get item from game and add to inventory
    private void RemoveSlots()
    {
        //remove information on slots and disactivate their gameobject
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    } //Clear slots so no overlap happens

    private void ShowPage()
    {
        slotCount = -1;
        //use page to find a range of items in one page
        // - - - 0 - - -
        for (int i = page*MAX_SLOTS_COUNT; i < inventoryTabList.Count; i++)
        {
            //deduct unnecessary fat to only show to actualy number of items
            slotCount = i - (page * MAX_SLOTS_COUNT);
            slots[slotCount].gameObject.SetActive(true);
            slots[slotCount].Additem(inventoryTabList[i]);

            //range might not end evenly, so break when slotCount hits max page 
            if (slotCount == MAX_SLOTS_COUNT - 1)
                break;
        }
    }

    private void ShowItem()
    {
        //first clear item list and slot so it does not show item from previous tab
        inventoryTabList.Clear();
        RemoveSlots();
        selectedItem = 0;
        page = 0;

        //add items to list
        for (int i = 0; i < inventoryItemList.Count; i++)
            inventoryTabList.Add(inventoryItemList[i]);
               
        //enable slot and add item information
        ShowPage();
        SelectedItem();
    } //Show item contents depending on current tab, like refresh

    private void SelectedItem()
    {
        //stop previous coroutine
        StopAllCoroutines();
        //if there are items in current inventory page
        if (slotCount > -1)
        {
            Color temp = slots[0].selected_Item.GetComponent<Image>().color;
            temp.a = 0f;
            //Remove selected item flash for all slots
            for (int i = 0; i <= slotCount; i++)
            {
                slots[i].selected_Item.GetComponent<Image>().color = temp;
            }
            Description_Text.text = inventoryTabList[selectedItem].itemDescription;
            //enable flash for selected tab
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
        {
            Description_Text.text = "아이템이 없습니다.";
        }

    }//Change item description and start flash coroutine

    IEnumerator SelectedItemEffectCoroutine()
    {
        while (true)
        {
            Color color = slots[0].selected_Item.GetComponent<Image>().color;
            //increase alpha value
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            //decrease alpha value
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            //wait a little before flash again
            yield return new WaitForSeconds(0.3f);
        }
    } //Flash Effect for item slots

    IEnumerator QuesItemUseCoroutine()
    {
        //use holding quest item to a quest object if requirement is true
        
        ItemRequestObject itemRequestObject = PlayerManager.instance.scanObject.GetComponent<ItemRequestObject>();
        //if facing itemrequestObject and does have quest item on hand
        if (itemRequestObject != null)
        {
            if(itemRequestObject.requestItemID == inventoryTabList[selectedItem].itemID && itemRequestObject.requestItemCount <= inventoryTabList[selectedItem].itemCount)
            {
                //play sound and stop keyboard to choose option
                AudioManager.instance.Play(enter_sound);
                stopKeyInput = true;
                //show ooc gameobject and wait for result
                go_OOC_UP.gameObject.SetActive(true);
                go_OOC_DOWN.gameObject.SetActive(true);
                theOOC.ShowChoice(inventoryTabList[selectedItem].useDescriptionTop, inventoryTabList[selectedItem].useDescriptionBottom);
                yield return new WaitUntil(() => !theOOC.activated);

                //if result is top choice
                if (theOOC.GetResult())
                {
                    itemRequestObject.Result();
                    //remove item
                    ThrowItem(inventoryTabList[selectedItem].itemID, itemRequestObject.requestItemCount);
                    Hide();
                }
                //enable key input for inventory 
                stopKeyInput = false;
                go_OOC_UP.gameObject.SetActive(false);
                go_OOC_DOWN.gameObject.SetActive(false);
            }
            else
                AudioManager.instance.Play(beep_sound);
        }
  
    }
    IEnumerator OOC_Coroutine(string _up, string _down)
    {
        //Call in yes / no choice to use item or not

        //play sound and stop keyboard to choose option
        AudioManager.instance.Play(enter_sound);
        stopKeyInput = true;
        //show ooc gameobject and wait for result
        go_OOC_UP.gameObject.SetActive(true);
        go_OOC_DOWN.gameObject.SetActive(true);
        theOOC.ShowChoice(_up,_down);
        yield return new WaitUntil(() => !theOOC.activated);

        //if result is true, use item and remove if 1, decrease if > 1
        if (theOOC.GetResult())
        {
            //use and remove item
            UseItem(inventoryTabList[selectedItem].itemID);
            ShowItem();
        }
        //enable key input for inventory 
        stopKeyInput = false;
        go_OOC_UP.gameObject.SetActive(false);
        go_OOC_DOWN.gameObject.SetActive(false);
    } 
    private bool CheckEven(int _slotCount)
    {
        return (_slotCount % 2 == 0);
    }
    public List<Item> SaveItem()
    {
        return inventoryItemList;
    }

    public void LoadItem(List<Item> _iteList)
    {
        inventoryItemList = _iteList;
    }

    public void UseItem(int _itemId)
    {
        //use item and throw it away
        DatabaseManager.instance.UseItem(_itemId);
        ThrowItem(_itemId, 1);    
    }
    public void ThrowItem(int _itemId,int _count)
    {
        //throw item with given itemid
        int temp = GameManager.instance.theData.LookFor(_itemId);
        //look for actual item in inventory
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            if (inventoryItemList[i].itemID == _itemId)
            {
                if (inventoryItemList[i].itemCount > _count)
                    inventoryItemList[i].itemCount -= _count;
                //else, remove from inventory
                else
                    inventoryItemList.RemoveAt(i);
                break;
            }
        }
    }

    public void EmptyInventory()
    {
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
             inventoryItemList.RemoveAt(0);
        }
    }
}
