using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public bool flag;
    private LightController lightController;
    private void Start()
    {
        lightController = FindObjectOfType<LightController>();
        flag = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player" && flag)
        {
            flag = false;
            lightController.TurnOffFlash();
        }
    }
}
