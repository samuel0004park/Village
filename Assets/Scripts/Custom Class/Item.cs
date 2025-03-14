using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item 
{
    public ItemSO ItemSO;
    public int itemCount;
    
    public Item(ItemSO itemSO, int _itemCount=1)
    {
        ItemSO = itemSO;
        itemCount = _itemCount;
    }
}
