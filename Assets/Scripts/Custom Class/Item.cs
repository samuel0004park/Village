using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item 
{
    public int itemID; //custom item id, cannot be overlapped
    public string itemName; //item name, can be oberlapped 
    public string itemDescription; //description of what item will be used for 
    public int itemCount;
    public Sprite itemIcon; //the sprite (use itemID to find)
    public ItemType itemType;
    public string useDescriptionTop;
    public string useDescriptionBottom;

    public enum ItemType
    {
        Use,
        Quest,
        ETC
    }


    public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType, string _useDescriptionTop="사용", string _useDescriptionBottom="취소",int _itemCount=1)
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDes;
        itemCount = _itemCount;
        itemType = _itemType;
        useDescriptionTop = _useDescriptionTop;
        useDescriptionBottom = _useDescriptionBottom;
        itemIcon = Resources.Load("ItemIcon/" + itemID.ToString(), typeof(Sprite)) as Sprite;
    }
}
