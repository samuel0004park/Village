using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    private static int CPS = 12; //char per seconds

    [SerializeField] private GameObject EndCursor;
    [SerializeField] private Text msgTxt;
    
    public bool isAnim { get; private set; }
    
    private string targetMsg;
    private int index;
    private float interval;

    public static event EventHandler OnCharacterTypeEvent;


    public void SetMsg(string str)
    {
        if (isAnim)
        {
            msgTxt.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        }
        else
        {
            targetMsg = str;
            EffectStart();
        }
    }

    private void EffectStart()
    {
        msgTxt.text = "";
        index = 0;
        EndCursor.SetActive(false);

        interval = 1.0f / CPS;
        isAnim = true;
        Invoke("Effecting", interval);
    }

    private void Effecting()
    {
        if (msgTxt.text == targetMsg)
        {
            EffectEnd();
            return;
        }
        msgTxt.text += targetMsg[index];

        if (targetMsg[index] != ' ' || targetMsg[index] != '.')
            OnCharacterTypeEvent?.Invoke(this, EventArgs.Empty);

        index++;
        Invoke("Effecting", interval);
    }

    private void EffectEnd()
    {
        isAnim = false;
        EndCursor.SetActive(true);
    }
}
