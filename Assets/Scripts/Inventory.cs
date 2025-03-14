using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Tooltip("Inventory UI GameObject")]
    [SerializeField] private GameObject go_OOC_UP;
    [SerializeField] private GameObject go_OOC_DOWN;
    [SerializeField] private List<InventorySlot> slots; //each slots in grid
    [SerializeField] private Text Description_Text; //descripton on selected item
    [SerializeField] private CanvasGroup CanvasGroup;

    private List<Item> inventoryItemList = new List<Item>();
    
    public bool stopKeyInput { get; private set; }

    private bool activated, preventExec; 
    private const int MAX_SLOTS_COUNT = 10;

    private int rows, cols;
    private int[,] inventoryGrid;

    private int selectedRow, selectedColumn;
    private int selectedItem;
    private int page, totalPages;


    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);


    
    public event EventHandler<OnInventoryStateChangedEventArgs> OnInventoryStateChangedEvent;
    public event EventHandler OnItemPickUpEvent;
    public event EventHandler OnInventoryNavigateEvent;
    public event EventHandler OnInventoryNegativeResultEvent;
    public class OnInventoryStateChangedEventArgs : EventArgs {
        public bool isOpen;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializeInventoryGrid();
        HideVisuals();
    }




    private void InitializeInventoryGrid() {
        rows = MAX_SLOTS_COUNT / 2;
        cols = 2;
        inventoryGrid = new int[rows, cols];

        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                inventoryGrid[r, c] = r * cols + c;
    }



    #region Inventory Navigation
    void Update() {
        if (stopKeyInput) return;

        if (Input.GetKeyDown(KeyCode.I)) ToggleInventory();
        if (!activated) return;

        if (Input.GetKeyDown(KeyCode.UpArrow)) NavigateVertical(-1);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) NavigateVertical(1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) NavigateHorizontal(-1);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) NavigateHorizontal(1);
        else if (Input.GetKeyDown(KeyCode.Z) && !preventExec) UseItem();
        else if (Input.GetKeyUp(KeyCode.Z)) preventExec = false;
        else if (Input.GetKeyDown(KeyCode.X)) CloseInventory();
    }


    private void NavigateVertical(int step) {
        int nextRow = selectedRow + step;
        int nextItem;

        if (nextRow < 0) {
            // Move to the previous page if at the top row
            if (page > 0) {
                selectedRow = rows - 1; // Move to the last row of the new page
                ChangePage(-1);
            }
            return;
        }
        else if (nextRow >= rows) {
            // Attempt to move to the next page
            int nextPageStartIndex = (page + 1) * MAX_SLOTS_COUNT;

            if (nextPageStartIndex < inventoryItemList.Count) {
                selectedRow = 0; // Move to the first row of the new page
                ChangePage(1);
            }
            return;
        }

        nextItem = inventoryGrid[nextRow, selectedColumn] + (page * MAX_SLOTS_COUNT);

        // Ensure nextItem does not go out of bounds
        if (nextItem >= inventoryItemList.Count) return;

        selectedRow = nextRow;
        selectedItem = nextItem;

        OnInventoryNavigateEvent?.Invoke(this, EventArgs.Empty);
        SelectItem();
    }

    private void NavigateHorizontal(int step) {
        int nextColumn = (selectedColumn + step + cols) % cols;
        int nextItem = inventoryGrid[selectedRow, nextColumn] + (page * MAX_SLOTS_COUNT);

        // Ensure nextItem is within the current page's valid item range
        int startIndex = page * MAX_SLOTS_COUNT;
        int endIndex = Math.Min(startIndex + MAX_SLOTS_COUNT, inventoryItemList.Count);

        if (nextItem >= endIndex) return; // Prevent moving to an invalid item

        selectedColumn = nextColumn;
        selectedItem = nextItem;

        OnInventoryNavigateEvent?.Invoke(this, EventArgs.Empty);
        SelectItem();
    }

    private void ChangePage(int direction) {
        CleanVisuals();

        page = (page + direction + totalPages) % totalPages; // Cycle pages
        LoadPage();

        // Ensure selectedRow stays within the new page's bounds
        selectedRow = Math.Min(selectedRow, rows - 1);
        selectedItem = inventoryGrid[selectedRow, selectedColumn];

        OnInventoryNavigateEvent?.Invoke(this, EventArgs.Empty);
        SelectItem();
    }


    #endregion

    private void ToggleInventory() {
        if (!activated) OpenInventory();
        else CloseInventory();
    }


    private void UseItem() {
        if (inventoryItemList.Count == 0)
            return;

        int index = inventoryGrid[selectedRow, selectedColumn];

        Item selectedItem = inventoryItemList[index]; // Get item from list

        switch (selectedItem.ItemSO.itemType) {
            case ItemType.Use:
                StartCoroutine(OOC_Coroutine(selectedItem.ItemSO.useDescriptionTop, selectedItem.ItemSO.useDescriptionBottom));
                break;
            case ItemType.Quest:
                StartCoroutine(QuesItemUseCoroutine());
                break;
            default:
                OnInventoryNegativeResultEvent?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    private void ResetInventoryIndex() {
        selectedItem = 0;
        selectedRow = 0;
        selectedColumn = 0;
        page = 0;
        totalPages = Mathf.CeilToInt((float)inventoryItemList.Count / MAX_SLOTS_COUNT);
    }

    private void OpenInventory() {
        ResetInventoryIndex();
        ShowVisuals();
        ShowInventory();

        activated = true;

        OnInventoryStateChangedEvent?.Invoke(this,new OnInventoryStateChangedEventArgs { isOpen = true});
    }

    private void CloseInventory()
    {
        StopAllCoroutines();
        HideVisuals();

        activated = false;

        OnInventoryStateChangedEvent?.Invoke(this, new OnInventoryStateChangedEventArgs { isOpen = false });
    }

    private void RemoveSlots() {
        foreach (var slot in slots) {
            slot.RemoveItem();
            slot.gameObject.SetActive(false);
        }
    }

    private void LoadPage() {
        int slotIndex = 0;
        int startIndex = page * MAX_SLOTS_COUNT;

        for (int i = startIndex; i < startIndex + MAX_SLOTS_COUNT && i < inventoryItemList.Count; i++) {
            slots[slotIndex].gameObject.SetActive(true);
            slots[slotIndex].Additem(inventoryItemList[i]);
            slotIndex++;
        }

        // Hide any remaining empty slots if the page is not full
        for (; slotIndex < MAX_SLOTS_COUNT; slotIndex++) {
            slots[slotIndex].gameObject.SetActive(false);
        }
    }

    #region Visual Methods

    private void HideOOC_UI() {
        stopKeyInput = false;
        go_OOC_UP.gameObject.SetActive(false);
        go_OOC_DOWN.gameObject.SetActive(false);
    }

    private void ShowOOC_UI() {
        stopKeyInput = true;
        go_OOC_UP.gameObject.SetActive(true);
        go_OOC_DOWN.gameObject.SetActive(true);
    }


    public void ShowVisuals() {
        CanvasGroup.alpha = 1f;
    }

    public void HideVisuals() {
        CanvasGroup.alpha = 0f;
    }


    private void SelectItem()
    {
        StopAllCoroutines();
        RemoveSelectedItemFlash();

        if (inventoryItemList.Count == 0) {
            Description_Text.text = "아이템이 없습니다";
            return;
        }

        Description_Text.text = inventoryItemList[selectedItem].ItemSO.itemDescription;
        StartCoroutine(SelectedItemEffectCoroutine());
    }

    private void RemoveSelectedItemFlash() {
        Color temp = slots[0].selected_Item.GetComponent<Image>().color;
        temp.a = 0f;

        // Remove selected item flash for all slots
        for (int i = 0; i < slots.Count; i++) {  
            slots[i].selected_Item.GetComponent<Image>().color = temp;
        }
    }

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
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator QuesItemUseCoroutine() {
        // Check if facing a valid ItemRequestObject
        ItemRequestObject itemRequestObject = PlayerManager.Instance.PlayerInteraction.scanObject?.GetComponent<ItemRequestObject>();
        if (itemRequestObject == null) {
            OnInventoryNegativeResultEvent?.Invoke(this, EventArgs.Empty);
            yield break;
        }

        // Check if the player has the required quest item
        if (!IsQuestItemValid(itemRequestObject)) {
            OnInventoryNegativeResultEvent?.Invoke(this, EventArgs.Empty);
            yield break;
        }

        // Show OOC choice UI and wait for user input
        yield return ShowOOCChoice();

        // If player confirms, complete the quest item use
        if (OOCManager.Instance.result) {
            CompleteQuestItemUse(itemRequestObject);
        }

        // Reset input controls and UI
        HideOOC_UI();
    }

 
    private bool IsQuestItemValid(ItemRequestObject itemRequestObject) {
        Item item = inventoryItemList[selectedItem];
        return item.ItemSO.itemID == itemRequestObject.requestItemID &&
               item.itemCount >= itemRequestObject.requestItemCount;
    }

   
    private IEnumerator ShowOOCChoice() {
        // Play sound and disable key input
        ShowOOC_UI();

        OOCManager.Instance.ShowChoice(inventoryItemList[selectedItem].ItemSO.useDescriptionTop,
                                       inventoryItemList[selectedItem].ItemSO.useDescriptionBottom);

        // Wait for player decision
        yield return new WaitUntil(() => !OOCManager.Instance.activated);
    }

 
    private void CompleteQuestItemUse(ItemRequestObject itemRequestObject) {
        itemRequestObject.Result();  // Complete the quest objective
        ThrowItem(inventoryItemList[selectedItem].ItemSO.itemID, itemRequestObject.requestItemCount);  // Remove item
        CloseInventory();  // Close inventory UI
    }

    IEnumerator OOC_Coroutine(string _up, string _down) {
        // Play selection sound & disable input
        ShowOOC_UI();
        OOCManager.Instance.ShowChoice(_up, _down);

        // Wait until player confirms or cancels
        yield return new WaitUntil(() => !OOCManager.Instance.activated);

        if (OOCManager.Instance.result)  // If player confirms item use
        {
            int itemIndex = inventoryGrid[selectedRow, selectedColumn];  
            if (itemIndex < inventoryItemList.Count)  // Ensure valid index
            {
                Item selectedItem = inventoryItemList[itemIndex];

                UseItem(selectedItem.ItemSO.itemID); 
                ShowInventory(); 
            }
        }

        HideOOC_UI();
    }

    #endregion



    #region Item Methods

    public List<Item> GetItemList() {
        return inventoryItemList.ToList();
    }



    public Item LookForItem(int _itemID) {
        // Use LINQ to find the item in inventory directly, avoiding unnecessary loops
        return inventoryItemList.FirstOrDefault(i => i.ItemSO.itemID == _itemID);
    }


    public void GetItem(int _itemID, int _count = 1) {
        // Find the item in the database
        ItemSO itemSO = GameManager.Instance.DataManager.TryGetItem(_itemID);
        if (itemSO == null) {
            Debug.Log("오류: 해당 아이템이 데이터베이스에서 찾을 수 없었습니다... 아이디를 확인하거나 아이템을 추가해 주세요.");
            return;
        }

        // Check if the item already exists in inventory
        var inventoryItem = inventoryItemList.FirstOrDefault(i => i.ItemSO.itemID == _itemID);
        if (inventoryItem != null)
            inventoryItem.itemCount += _count;
        else
            inventoryItemList.Add(new Item(itemSO, _count));

        OnItemPickUpEvent?.Invoke(this, EventArgs.Empty);
    }

    private void ShowInventory() {
        CleanVisuals();
        LoadPage();
        SelectItem();
    }

    private void CleanVisuals() {
        RemoveSlots();
    }

    public void LoadItem(List<Item> _iteList)
    {
        inventoryItemList = new List<Item>(_iteList);
    }

    public void UseItem(int _itemId)
    {
        //use item and throw it away
        PlayerManager.Instance.UseItem(_itemId);
        ThrowItem(_itemId, 1);    
    }

    public void ThrowItem(int _itemId,int _count)
    {
        Item item = LookForItem(_itemId);
        if (item == null)
            return;

        if(item.itemCount>_count)
            item.itemCount -= _count;
        else
            inventoryItemList.Remove(item);
    }

    public void EmptyInventory() {
        inventoryItemList.Clear();
    }
    #endregion
}
