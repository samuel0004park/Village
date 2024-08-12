using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int itemID;
    public int count;
    public string pick_sound;

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PlayerManager.instance.interact)
        {
            AudioManager.instance.Play(pick_sound);
            Inventory.instance.GetItem(itemID, count);

            Destroy(this.gameObject);
        }
    }

}
