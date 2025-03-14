using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceAndResult : MonoBehaviour
{
    [SerializeField]
    public Choice choice;

    [Header("Refences")]
    private ChoiceManager theChoice;
    private ObjectDialogue theDialogue;

    [Header("Values")]
    [SerializeField]private bool instantDeath;
    public bool choosing { get; private set; }
    public bool redo { get; private set; }
    private int result;
    public string type;
    public int[] itemID;
    public int[] requestNum;
    public int positiveChoice;
    public int revealItemID;

    public static event EventHandler OnChoiceEvent;

    void Start()
    {
        redo = true;
        theDialogue = GetComponent<ObjectDialogue>();
        theChoice = FindObjectOfType<ChoiceManager>();
    }

    public void StartChoice()
    {
        choosing = true;
        StartCoroutine(ChoiceCoroutine());
    }
  
    IEnumerator ChoiceCoroutine()
    {
        //show choice
        theChoice.ShowChoice(choice);

        //wait until finish choosing
        yield return new WaitUntil(() => !theChoice.choiceIng);
        result = theChoice.GetResult();

        CheckChoice();
        
        choosing = false;
        OnChoiceEvent?.Invoke(this, EventArgs.Empty);

        //if choice was negative, disable flag so can choose again 
        if (redo)
            yield return new WaitForSeconds(1f);
    }

    private void CheckChoice()
    {
        //if correct choice
        if (result != positiveChoice)
        {
            NegativeResult();
            return;
        }

        if (result == positiveChoice)
        {
            //check if there is requesting item
            if (Request()) // if enough
            {
                Inventory theInven = FindObjectOfType<Inventory>();
                for (int i = 0; i < itemID.Length; i++)
                    theInven.ThrowItem(itemID[i], requestNum[i]);
                Result();
            }
            else // if not enough
            {
                NegativeResult();
            }
        }
    }

    private void NegativeResult()
    {
        // if instant death, kill player, if not, enable redo
        if (instantDeath)
            PlayerManager.Instance.playerStat.Die();
        else
            redo = true;
    }

    private bool Request()
    {
        //if there is no request item, return true
        Inventory theInven = FindObjectOfType<Inventory>();
        if (itemID.Length == 0)
            return true;

        //check for requested item/items
        for (int i = 0; i < itemID.Length; i++)
        {
            var item = theInven.LookForItem(itemID[i]);
            int itemCount = item !=null? item.itemCount:0;

            if (itemCount < requestNum[i]) // if there are not enough of item, return false
                return false;    
        }
        //return true if enough items
        return true;
    }
    private void Result()
    {
        // disable redo 
        redo = false;

        switch (type) // give out result based on id
        {
            case "Fence": //open fence if have enough required item
                FenceChoice();
                break;
            case "Gift": //give item if have enough of required item
                GiftChoice();
                break;
            case "Reveal":
                RevealChoice();
                break;
            case "Remove":
                RemoveChoice();
                break;
            default:
                break;
        }
        CheckShrine();
    }

    private void RevealChoice()
    {
        if (revealItemID != -1)
            FindObjectOfType<QuestObjectManager>().ShowQuestObject(revealItemID);
    }

    private void FenceChoice()
    {
        //continue only if fence exists
        OpenFence theFence = GetComponent<OpenFence>();
        if (theFence == null)
            return;
        theFence.Open();
    }


    private void RemoveChoice() {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D.enabled = false;
        spriteRenderer.enabled = false;
    }

    private void GiftChoice()
    {
        theDialogue.GiveItem();   
    }
    private void CheckShrine()
    {
        if(theDialogue.GetID() < 100)
        {
            SaveNLoad.Instance.Save();
        }
    }
   
}
