using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDialogue : MonoBehaviour, IInteract
{
    //[SerializeField]
    //public Dialogue dialogue;

    private DialogueManager theDialogue;
    private ChoiceAndResult theCaR;
    private QuestionAndResult theQaR;

    [SerializeField] private int id;
    [SerializeField] private int item_id = 0;
    [SerializeField] private string type;

    private WaitForSeconds DialogueEndDelay = new WaitForSeconds(0.5f);

    private string pick_sound= "pick_sound";
    public bool flag;
    private bool disposable;

    public static event EventHandler<OnObjectDialogueStateChangedEventArgs> OnObjectDialogueStateChangedEvent;
    public class OnObjectDialogueStateChangedEventArgs : EventArgs {
        public bool isTalking;
    }

    void Start()
    {
        flag = false;
        disposable = false;
    }
    
    public void Interact() {
        if (!flag ) {
            flag = true;
            //stop player 
            OnObjectDialogueStateChangedEvent?.Invoke(this, new OnObjectDialogueStateChangedEventArgs { isTalking = true });

            //start coroutine of npc talk
            switch (type) {
                case "Dialogue":
                    StartCoroutine(DialogueCoroutine());
                    break;
                case "CaR":
                    StartCoroutine(ChoiceAndResultCoroutine());
                    break;
                case "Question":
                    StartCoroutine(QuestionAndResultCoroutine());
                    break;
                case "Gift":
                    StartCoroutine(GiftCoroutine());
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator GiftCoroutine() {
        Inventory.Instance.GetItem(item_id,1);

        yield return DialogueEndDelay;

        OnObjectDialogueStateChangedEvent?.Invoke(this, new OnObjectDialogueStateChangedEventArgs { isTalking = false });
    }


    IEnumerator QuestionAndResultCoroutine()
    {
        theQaR = GetComponent<QuestionAndResult>();

        //check if talked npc was proper quest order. if so, increase quest_talk_index
        int temp_id = CalculateCorrectNPCTalk();

        theQaR.ShowQuestion();

        yield return new WaitUntil(() => !theQaR.choosing);

        yield return DialogueEndDelay;


        OnObjectDialogueStateChangedEvent?.Invoke(this, new OnObjectDialogueStateChangedEventArgs { isTalking = false });
        CheckRedo();
    }


    IEnumerator ChoiceAndResultCoroutine() {
        theCaR = GetComponent<ChoiceAndResult>();

        if (theCaR == null) yield break; // Ensure theCaR exists

        if (theCaR.redo) {
            int temp_id = CalculateCorrectNPCTalk();
            theCaR.StartChoice();

            yield return new WaitUntil(() => theCaR == null || !theCaR.choosing);
            if (theCaR == null) yield break; // Exit if object was destroyed
        }

        yield return DialogueEndDelay;
        OnObjectDialogueStateChangedEvent?.Invoke(this, new OnObjectDialogueStateChangedEventArgs { isTalking = false });

        CheckRedo();
    }

    IEnumerator DialogueCoroutine()
    {
        //stop player and show dialogue until end of dialogue. 
        theDialogue = FindObjectOfType<DialogueManager>();

        //check if talked npc was proper quest order. if so, increase quest_talk_index
        int temp_id=CalculateCorrectNPCTalk();

        //pass id of object and start diague 
        theDialogue.ShowDialogue(temp_id);
    
        yield return new WaitUntil(() => !theDialogue.talking);

        GiveItem();

        yield return DialogueEndDelay;
        OnObjectDialogueStateChangedEvent?.Invoke(this, new OnObjectDialogueStateChangedEventArgs { isTalking = false });

        CheckRedo();
    }

    public int CalculateCorrectNPCTalk()
    {
        //check if talked npc was proper quest order. if so, increase quest_talk_index
        bool isProper = QuestManager.Instance.CheckQuest(id);
        int temp_index = id;
        //send id with incremented quest index only if talking with correct npc
        if (isProper)
        {
            int questTalkIndex = QuestManager.Instance.GetQuestTalkIndex();
            temp_index += questTalkIndex;
        }
        return temp_index;
    }

    private void CheckRedo()
    {
        // if disposable, set flag to true so interaction is no longer possible after this
        flag = disposable;
    }

    public void SetDisposable()
    {
        disposable = true;
    }

    public void GiveItem()
    {
        if (item_id != 0) //give item dialogue gives item
        {
            SFXManager.Instance.Play(pick_sound);
            Inventory.Instance.GetItem(item_id, 1);
            RemoveItem();
        }
    }

    private void RemoveItem() {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D.enabled = false;
        spriteRenderer.enabled = false;
    }


    public int GetID() {
        return id;
    }
}
