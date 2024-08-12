using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{

    public Dialogue dialogue_1;
    public Dialogue dialogue_2;
    public Dialogue dialogue_3;

    private DialogueManager theDialogue;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private FadeManager theFade;


    private bool flag; //var so event only activates once

    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theDialogue = FindObjectOfType<DialogueManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if pressed z and player is in interactable state
        if(!flag && Input.GetKey(KeyCode.Z) && thePlayer.interact)
        {
            //set flag to true so event only occurs once
            flag = true;
            StartCoroutine(EventCoroutine());
        }
    }
    IEnumerator EventCoroutine()
    {
        //stop player movement
        theOrder.PreLoadCharacter();
        theOrder.ForceStop(2);

        //show dialogue and wait until finish
        //theDialogue.ShowDialogue(dialogue_1);
        yield return new WaitUntil(() => !theDialogue.talking);

        //move if there need movement and wait until finish
        theOrder.Move("player", "RIGHT");
        yield return new WaitForSeconds(0.5f);
        theOrder.Move("player", "LEFT");
        yield return new WaitUntil(() => thePlayer.queue.Count == 0);

        //show dialogue
        //theDialogue.ShowDialogue(dialogue_2);
        yield return new WaitUntil(() => !theDialogue.talking);

        theOrder.Move("player", "DOWN");
        theOrder.Turn("player", "UP");

        theFade.Flash();
        yield return new WaitUntil(() => thePlayer.queue.Count == 0);

        //theDialogue.ShowDialogue(dialogue_3);
        yield return new WaitUntil(() => thePlayer.queue.Count == 0);

        //continue player movement
        theOrder.ContinueMove();
        thePlayer.interact = false; //set player's state to normal 
    }
}
