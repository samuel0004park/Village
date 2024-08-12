using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public bool isAnim;
    public string typeSound;
    public GameObject EndCursor;
    public int cps; //char per seconds

    private AudioManager theAudio;
    string targetMsg;
    Text msgTxt;
    int index;
    float interval;

    private void Awake()
    {
        theAudio = FindObjectOfType<AudioManager>();
        msgTxt = GetComponent<Text>();
    }

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

        interval = 1.0f / cps;
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
        
        if (targetMsg[index] != ' ' || targetMsg[index]!='.')
            theAudio.Play(typeSound);

        index++;
        Invoke("Effecting", interval);
    }

    private void EffectEnd()
    {
        isAnim = false;
        EndCursor.SetActive(true);
    }
}
