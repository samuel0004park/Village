using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRequestObject : MonoBehaviour
{
    [Header("Values")]
    public int requestItemID = 0;
    public int requestItemCount = 0;
    public string type;
    private bool flag=false;
    public int revealItemID = -1;

    public static event EventHandler ItemRequestPositiveEvent;
    public void Result()
    {
        if (flag)
            return;

        ObjectDialogue objectDialogue = GetComponent<ObjectDialogue>();
        flag = true;
        objectDialogue.flag = true;
        
        CheckShrine();

        switch (type) // give out result based on id
        {
            case "Fence":
                FenceChoice();
                break;
            case "Gift":
                GiftChoice();
                break;
            case "Reveal":
                RevealChoice();
                break;
            default:
                break;
        }
        
    }
    private void RevealChoice()
    {
        if(revealItemID!=-1)
            FindObjectOfType<QuestObjectManager>().ShowQuestObject(revealItemID);
    }
    private void FenceChoice()
    {
        //continue only if component exists
        OpenFence theFence = GetComponent<OpenFence>();
        if (theFence == null)
            return;

       //open fence
        theFence.Open();
        
    }

    private void GiftChoice()
    {
        //continue only if component exists
        ObjectDialogue objectDialogue = GetComponent<ObjectDialogue>();
        if (objectDialogue == null)
            return;

        objectDialogue.GiveItem();
    }

    private void CheckShrine()
    {
        ObjectDialogue temp = gameObject.GetComponent<ObjectDialogue>();
        if (temp.GetID() < 100)
        {
            ItemRequestPositiveEvent?.Invoke(this, EventArgs.Empty);
            SaveNLoad.Instance.Save();
        }
    }
}
