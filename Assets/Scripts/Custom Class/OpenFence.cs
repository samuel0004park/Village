using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFence : MonoBehaviour
{
    public GameObject[] fence;
    public float closeTimer;

    public static event EventHandler OnOpenFenceEvent;

    public void Open()
    {
        StartCoroutine(FenceRoutine());
    }
    IEnumerator FenceRoutine()
    {
        // open fence and close only if there is timer
        OpenOrClose(false);

        if(closeTimer != 0)
        {
            yield return new WaitForSeconds(closeTimer);
            OpenOrClose(true);
        }

    }
    private void OpenOrClose(bool tf)
    {
        //true for open false for close

        for (int i = 0; i < fence.Length; i++)
        {
            fence[i].gameObject.SetActive(tf);
        }
        OnOpenFenceEvent?.Invoke(this, EventArgs.Empty);
    }
}
