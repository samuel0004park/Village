using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO :ScriptableObject
{
    [SerializeField] public int itemID;
    [SerializeField] public string itemName; 
    [SerializeField] public string itemDescription; 
    [SerializeField] public Sprite itemIcon;
    [SerializeField] public ItemType itemType;
    [SerializeField] public string useDescriptionTop;
    [SerializeField] public string useDescriptionBottom;
}

public enum ItemType {
    Use,
    Quest,
    ETC
}