using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerGroup : MonoBehaviour
{
    
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        UIManager.Instance.OnReturnToTitleEvent += Instance_OnReturnToTitleEvent;
    }

    private void OnDestroy() {
        UIManager.Instance.OnReturnToTitleEvent -= Instance_OnReturnToTitleEvent;
    }

    private void Instance_OnReturnToTitleEvent(object sender, System.EventArgs e) {
        Destroy(gameObject);
    }
}
