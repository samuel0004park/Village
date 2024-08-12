using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDialogue : MonoBehaviour
{
    //[SerializeField]
    //public Dialogue dialogue;

    [Header("References")]
    private DialogueManager theDialogue;
    private ChoiceAndResult theCaR;
    private QuestionAndResult theQaR;

    //player staminabar
    private Animator staminaBar;

    [Header("Values")]

    public int id;
    [SerializeField] private int item_id = 0;
    [SerializeField] private string type; 

    private string pick_sound= "pick_sound";
    public bool flag;
    private bool disposable;

    void Start()
    {
        flag = false;
        disposable = false;
        staminaBar = GameManager.instance.mpBar.GetComponent<Animator>();
    }
    public void Talk()
    {
        //accessible from outide
        //object starts its defined type of dialogue 
        if(!flag && PlayerManager.instance.interact)
        {
            flag = true;
            staminaBar.SetBool("Appear", false);
            //stop player 
            OrderManager.instance.ForceStop(2);

            //start coroutine of npc talk
            switch (type)
            {
                case "Dialogue":
                    StartCoroutine(DialogueCoroutine());
                    break;
                case "CaR":
                    StartCoroutine(ChoiceAndResultCoroutine());
                    break;
                case "Question":
                    StartCoroutine(QuestionAndResultCoroutine());
                    break;
                default:
                    break;
            }
        }
    }


    IEnumerator QuestionAndResultCoroutine()
    {
        theQaR = GetComponent<QuestionAndResult>();

        //check if talked npc was proper quest order. if so, increase quest_talk_index
        int temp_id = CalculateCorrectNPCTalk();

        theQaR.ShowQuestion();
        
        //wait unltil choice ends
        yield return new WaitUntil(() => !theQaR.choosing);

        //enable movement and show staminabar
        staminaBar.SetBool("Appear", true);
        OrderManager.instance.ContinueMove();

        //set flag to false a little later to prevent overlap 
        yield return new WaitForSeconds(1f);
        CheckRedo();
    }
   
    
    IEnumerator ChoiceAndResultCoroutine()
    {
        theCaR = GetComponent<ChoiceAndResult>();
        //if choice is redoable, continue
        if (theCaR.redo)
        {
            //check if talked npc was proper quest order. if so, increase quest_talk_index
            int temp_id = CalculateCorrectNPCTalk();

            theCaR.StartChoice();

            //wait unltil choice ends
            yield return new WaitUntil(() => !theCaR.choosing);
        }
        //enable movement and show staminabar
        staminaBar.SetBool("Appear", true);
        OrderManager.instance.ContinueMove();

        //set flag to false a little later to prevent overlap 
        yield return new WaitForSeconds(1f);
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
    
        //wait unltil dialogue ends
        yield return new WaitUntil(() => !theDialogue.talking);

        GiveItem();

        //enable movement and show staminabar
        staminaBar.SetBool("Appear", true);
        OrderManager.instance.ContinueMove();

        //set flag to false a little later to prevent overlap 
        yield return new WaitForSeconds(1f);
        CheckRedo();
    }

    public int CalculateCorrectNPCTalk()
    {
        //check if talked npc was proper quest order. if so, increase quest_talk_index
        bool isProper = QuestManager.instance.CheckQuest(id);
        int temp_index = id;
        //send id with incremented quest index only if talking with correct npc
        if (isProper)
        {
            int questTalkIndex = QuestManager.instance.GetQuestTalkIndex();
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
            AudioManager.instance.Play(pick_sound);
            Inventory.instance.GetItem(item_id, 1);
            Destroy(this.gameObject);
        }
    }
}
