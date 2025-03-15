using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Text itemCount_Text;
    public Text itemName_Text;
    public GameObject selected_Item;

    public void Additem(Item _item)
    {
        itemName_Text.text = _item.ItemSO.itemName;
        icon.sprite = _item.ItemSO.itemIcon;

        if (_item.itemCount > 0)
            itemCount_Text.text = "x " + _item.itemCount.ToString();
        else
            itemCount_Text.text = "";
    }

    public void RemoveItem()
    {
        icon.sprite= null;
        itemCount_Text.text =  "";
        itemName_Text.text= "";
}
}
