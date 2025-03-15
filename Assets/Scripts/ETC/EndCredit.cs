using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndCredit : MonoBehaviour, IInteract{
    public GameObject go;

    public void Interact() {
        Destroy(go);
    }
}
