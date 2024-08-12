using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public bool flag=false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!flag && collision.CompareTag("Player"))
        {
            flag = true;
            GameObject player = GameObject.Find("Player");
            SaveNLoad save = player.GetComponent<SaveNLoad>();
            save.Save();
        }
    }
  
}
