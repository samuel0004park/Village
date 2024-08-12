using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCredit : MonoBehaviour
{
    public GameObject go;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PlayerManager.instance.interact)
        {
            Debug.Log("HIT");
            go.SetActive(true);
            
        }
    }
}
