using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteract 
{
    public int itemID;
    public int count;


    public void Interact() {
        Inventory.Instance.GetItem(itemID, count);
        Destroy(gameObject);
    }

}
