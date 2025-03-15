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
            SaveNLoad.Instance.Save();
        }
    }
  
}
